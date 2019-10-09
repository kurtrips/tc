/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import java.math.BigInteger;

import javax.xml.namespace.QName;

import org.apache.james.mime4j.parser.MimeStreamParser;
import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolDecoderOutput;
import org.apache.mina.filter.codec.demux.MessageDecoderAdapter;
import org.apache.mina.filter.codec.demux.MessageDecoderResult;

import com.topcoder.alu.mock.mm7.entities.Detail;
import com.topcoder.alu.mock.mm7.entities.Fault;
import com.topcoder.alu.mock.mm7.entities.GenericResponseType;
import com.topcoder.alu.mock.mm7.entities.ResponseStatusType;

/**
 * This class is the entry point into MINA for the incmong messages.
 * It first checks whether a message is decodable.
 * It can return OK if it can be decoded.
 * It can return OK and return a fault, if message is wrong (for ex. not a POST)
 * It can return NEED_DATA if it needs more data. (It might take more than one buffer because of IP fragmentation)
 * Once the message is declared decodable, MINA calls the decode method.
 * In the decode method it:
 * a) parses the http content using Mime4J
 * b) converts the underlying soap data into entities
 * c) Writes the entities to session so that they then travel to the IOHandler 
 *  
 * @author TCSDEVELOPER
 * @version 1.0
 * 
 */
public class SoapRequestDecoder extends MessageDecoderAdapter {

	/**
	 * The bytes for ConntentType header
	 */
	private static final byte[] CONTENT_TYPE = new String("Content-Type: ").getBytes();
	
	/**
	 * The bytes for multipart/related header
	 */
	private static final byte[] MULTIPART_RELATED = new String("multipart/related").getBytes();
	
	/**
	 * The bytes for boundary attribute
	 */
	private static final byte[] BOUNDARY_ATTRIBUTE = new String("boundary=\"").getBytes();

	/**
	 * The end of Header index
	 */
	private int endOfHeader = 1;
	
	/**
	 * The boundary value parsed from http header
	 */
	private String boundary = null;
	
	/**
	 * Whether this is an MM1 request.
	 */
	private boolean isMM1Req = false;
	
	/**
	 * The Fault to write to response
	 */
	private Fault fault = null;
	
	/**
	 * Checks whether the message is decodable.
	 * @param session The current session
	 * @param in The input buffer
	 */
	public MessageDecoderResult decodable(IoSession session, IoBuffer in) {
		MessageDecoderResult res;
		try {
		    res = messageComplete(in);
		} catch (Exception ex) {
			res = MessageDecoderResult.NOT_OK;
		}
		//System.out.println("Inside decodable with result: " + res.toString());
		return res;
	}

	/**
	 * This method decodes the message, parse out entities and forwards them for more processing
	 * @param session The current session
	 * @param in The input buffer
	 * @param op The ProtocolDecoderOutput to write the entities to.
	 * @throws Exception In case of any problem wit the decoding process 
	 */
	public MessageDecoderResult decode(IoSession session, IoBuffer in,
			ProtocolDecoderOutput op) throws Exception {
		//System.out.println("Inside decode");

		//Use the Apache Mime4j library to parse out the soap content from the multipart http request
		MimeStreamParser parser = new MimeStreamParser();
		MM7IncomingMessageContentHandler handler = new MM7IncomingMessageContentHandler();
		parser.setContentHandler(handler);
		parser.parse(in.asInputStream());
		
		//Check for http basic authentication should pass
		if (!handler.isAuthPassed()) {
			populateFault(2001, "The request was refused due to lack of permission to execute the command.");
		}
		
		if (isMM1Req) {
			//We will not use anything from the MM1 request (because this is a mock) 
			//We will simply pass a string "MM1_submit.REQ" to the server's handler which will take further action
			op.write(Constants.MM1_SUBMIT_REQ);
		}
		else if (fault == null) {
			
			//Write the high-level decoded message to the ProtocolDecoderOutput for further business processing
			op.write(handler.getContentHandlerOutput());
		}
		else if (fault != null) {
			//Write the Fault to the ProtocolDecoderOutput for further business processing
			op.write(fault);
		}

		return MessageDecoderResult.OK;
	}

	/**
	 * Checks whether the message is complete.
	 * @param in The input message
	 * @return The MessageDecoderResult 
	 * @throws Exception If there's any problem with the checking process
	 */
	private MessageDecoderResult messageComplete(IoBuffer in) throws Exception {
		int last = in.remaining() - 1;
		if (in.remaining() < 4) {
			return MessageDecoderResult.NEED_DATA;
		}
		
		//Must start with POST
		if (!startsWithPost(in)) {
			populateFault(2000, "Incmoing message must start with POST");
			return MessageDecoderResult.OK;
		}
		
		//Get the index where the header ends and data starts
		endOfHeader = getEndOfHeaderIndex(in);
		if (endOfHeader == 1) {
			return MessageDecoderResult.NEED_DATA;
		}

		//Find the Content-Type: header. If it's missing then format of request is wrong.
		int indexOfContentType = getContentTypeIndex(in);
		if (indexOfContentType == -1) {
			populateFault(2000, "Incmoing message is missing Content-Type header");
			return MessageDecoderResult.OK;
		}

		//Check for MM1 request first
		if (isMM1SubmitReq(in, indexOfContentType)) {
			isMM1Req = true;
			return MessageDecoderResult.OK;
		}
		
		// Check if the value of Content-Type is "multipart/related" otherwise format of request is wrong
		if (!checkContentTypeValue(in, indexOfContentType)) {
			populateFault(2000, "Incmoing message has incorrect value of Content-Type header");
			return MessageDecoderResult.OK;
		}

		//Get the boundary attribute for the multipart message 
		boundary = getBoundaryValue(in, indexOfContentType);
		if (boundary == null) {
			populateFault(2000, "Incmoing message is missing boundary for multipart request");
			return MessageDecoderResult.OK;
		}

		//End of message in multipart message is denoted as --[boundary]--
		String endOfMessage = "--" + boundary + "--";
		if (in.remaining() < endOfMessage.length()) {
			return MessageDecoderResult.NEED_DATA;
		}
		//The ending of the message is not correct, so we need more data
		//TODO - not sure if we should return NEED_DATA or NOT_OK because we might end up returning NEED_DATA forever.
		for (int i = 0; i < endOfMessage.length(); i++) {
			if (in.get(last - i) != (byte) endOfMessage.charAt(endOfMessage.length() - 1 - i)) {
				return MessageDecoderResult.NEED_DATA;
			}
		}
		
		// All tests pass. We have the complete message in the buffer
		return MessageDecoderResult.OK;
	}
	
	/**
	 * This method checks if this is an MM1 request. 
	 * Only the following rules are checked (because this is just a mock server)
	 * http://www.nowsms.com/documentation/ProductDocumentation/mms_notifications_and_content/Submitting_MMS_Messages_MM1.htm
	 * Rule 1: The Post must be to the URI "/mm1"
	 * Rule 2: The Content-type must be "application/vnd.wap.mms-message"
	 * @param in The incoming IO buffer
	 * @param indexOfContentType The index where the ContentType header starts
	 * @return Whether this is an MM1 request
	 */
	private boolean isMM1SubmitReq(IoBuffer in, int indexOfContentType) {
		//The value of Content-Type header must be 'application/vnd.wap.mms-message'
		int init = indexOfContentType + CONTENT_TYPE.length;
		for (int i = init; i < init + Constants.MM1_CONTENT_TYPE.length(); i++) {
			if (in.get(i) != (byte) Constants.MM1_CONTENT_TYPE.charAt(i - init)) {
				return false;
			}
		}
		
		//Also uri must be '/mm1'
		return in.get(5) == (byte) '/' && in.get(6) == (byte) 'm' 
		&& in.get(7) == (byte) 'm' && in.get(8) == (byte) '1';
	}

	/**
	 * Checks if the buffer starts with POST
	 * @param in The input buffer
	 * @return Whether the buffer starts with POST
	 */
	private boolean startsWithPost(IoBuffer in) {
		return in.get(0) == (byte) 'P' && in.get(1) == (byte) 'O' 
		&& in.get(2) == (byte) 'S' && in.get(3) == (byte) 'T' ? true : false;
	}
	
	/**
	 * Get the index where the header end.
	 * @param in The input buffer
	 * @return the index where the header ends
	 */
	private int getEndOfHeaderIndex(IoBuffer in) {
		//Check for the new lines that denote end of POST
		int eoh = 1;
		for (int i = in.remaining() - 1; i > 2; i--) {
			if (in.get(i) == (byte) 0x0A && in.get(i - 1) == (byte) 0x0D
					&& in.get(i - 2) == (byte) 0x0A
					&& in.get(i - 3) == (byte) 0x0D) {
				eoh = i + 1;
				break;
			}
		}
		return eoh;
	}
	
	/**
	 * Get the index of the Content-Type header
	 * @param in The input bufefr
	 * @return the index of the Content-Type header
	 */
	private int getContentTypeIndex(IoBuffer in) {
		//Find the Content-Type: header
		for (int i = 0; i < endOfHeader; i++) {
			boolean found = true;
			for (int j = 0; j < CONTENT_TYPE.length; j++) {
				if (in.get(i + j) != CONTENT_TYPE[j]) {
					found = false;
					break;
				}
			}
			if (found) {
				return i;
			}
		}
		return -1;
	}
	
	/**
	 * Checks if the Content-Type is multipart/related
	 * @param in inpout buffer
	 * @param contentTypeIndex the index where Content-Type starts 
	 * @return if the Content-Type is multipart/related
	 */
	private boolean checkContentTypeValue(IoBuffer in, int contentTypeIndex) {
		//The value of Content-Type header must be 'multipart/related'
		int init = contentTypeIndex + CONTENT_TYPE.length;
		for (int i = init; i < init + MULTIPART_RELATED.length; i++) {
			if (in.get(i) != MULTIPART_RELATED[i - init]) {
				return false;
			}
		}
		return true;
	}
	
	/**
	 * Gets the value of the boundary of the multipart http request 
	 * @param in The input buffer
	 * @param contentTypeIndex The index of start of content type header
	 * @return The value of the boundary
	 */
	private String getBoundaryValue(IoBuffer in, int contentTypeIndex) {
		int init = contentTypeIndex + CONTENT_TYPE.length;
		for (int i = init; i < endOfHeader; i++) {
			//Newline represents the end of the content_type header
			if (in.get(i) == 0x0D) {
				return null;
			}
			
			//Find the index where boundary attribute starts 
			boolean boundaryFound = true;
			for (int j = i; j < i + BOUNDARY_ATTRIBUTE.length; j++) {
				if (in.get(j) != BOUNDARY_ATTRIBUTE[j-i]) {
					boundaryFound = false;
					break;
				}
			}
			if (!boundaryFound) {
				continue;
			}
			
			//Get the value of the boundary attribute
			StringBuilder contentLengthStr = new StringBuilder();			
			for (int j = i + BOUNDARY_ATTRIBUTE.length; ; j++) {
				//End of quote of boundary attribute
				if (in.get(j) == 0x22) {
					break;
				}
				//End of line 
				if (in.get(j) == 0x0d) {
					return null;
				}				
				contentLengthStr.append(new String(new byte[] { in.get(j) }));
			}
			return contentLengthStr.toString();
		}
		return null;
	}

	/**
	 * Populates a client fault using the code an error message  
	 * @param code The error code
	 * @param errorMessage The error message
	 */
	private void populateFault(int code, String errorMessage) {
		fault = new Fault();
		fault.setFaultcode(new QName("env", "Client"));
		fault.setFaultstring("Client error");
		
		GenericResponseType rsErrorRsp = new GenericResponseType();
		rsErrorRsp.setMM7Version(Constants.MMS_VERSION);

		ResponseStatusType responseStatusType = new ResponseStatusType();
		responseStatusType.setStatusCode(BigInteger.valueOf(code));
		responseStatusType.setStatusText(errorMessage);
		
		Detail detail = new Detail();
		detail.getAny().add(responseStatusType);
		
		fault.setDetail(detail);
	}
}
