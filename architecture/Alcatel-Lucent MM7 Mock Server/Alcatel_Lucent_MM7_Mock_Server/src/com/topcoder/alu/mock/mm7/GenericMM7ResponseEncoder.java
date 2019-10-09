/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.Marshaller;
import javax.xml.namespace.QName;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolEncoderOutput;
import org.apache.mina.filter.codec.demux.MessageEncoder;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import com.topcoder.alu.mock.mm7.entities.GenericResponseType;

/**
 * This class is the encoder for the MM7_submit.RES.
 * It converts the GenericMM7Response instance into the soap format which can then be sent to the VASP server.
 * @author TCSDEVELOPER
 * @version 1.0 
 */
public class GenericMM7ResponseEncoder implements MessageEncoder<GenericMM7Response> {

	/**
	 * Converts a GenericMM7Response instance into the proper soap format,
	 * so that it can be written over the socket as the body of an http response
	 * @param session The current session
	 * @param submitReqResponse The Submit Request Response to be converted
	 * @param op The ProtocolEncoderOutput to write to
	 * @throws Exception In case anything goes wrong with the conversion
	 */
	public void encode(IoSession session, GenericMM7Response submitReqResponse,
			ProtocolEncoderOutput op) throws Exception {
		
		//First write the http headers to the output stream
		op.write(IoBuffer.wrap(Constants.HTTP_RESP_HEADER.getBytes()));
		
		Document responseXml = buildSubmitResponseXml(submitReqResponse);
		//Write the created submit response document to the output stream
		op.write(IoBuffer.wrap(CommonUtil.docToBytes(responseXml)));
		op.flush();
	}
	
	/**
	 * Builds the soap document from the GenericMM7Response.
	 * @param response The GenericMM7Response from which to build the soap
	 * @return The soap document
	 * @throws Exception If there is any issue in the conversion
	 */
    @SuppressWarnings("unchecked")
	private static Document buildSubmitResponseXml(GenericMM7Response response) throws Exception {
		Marshaller marshaller = CommonUtil.getMarshaller();
    	
    	//Create the soap response
		DocumentBuilderFactory documentBuilderFactory = DocumentBuilderFactory.newInstance();
		DocumentBuilder documentBuilder = documentBuilderFactory.newDocumentBuilder();
		Document responseXml = documentBuilder.newDocument();
		
		//Create the soap envelope
		Element envelopeNode = responseXml.createElementNS(
				Constants.SOAP_ENV_NS, "env:Envelope");
		responseXml.appendChild(envelopeNode);
		
		//Create the soap header
		Element headerNode = responseXml.createElementNS(Constants.SOAP_ENV_NS, "env:Header");
		envelopeNode.appendChild(headerNode);
		
		//Create the soap body
		Element bodyNode = responseXml.createElementNS(Constants.SOAP_ENV_NS, "env:Body");
		envelopeNode.appendChild(bodyNode);
		
		//Marshall the TransactionId node
		marshaller.marshal(response.getHeader(), headerNode);

		//Marshall the SubmitRsp node
		marshaller.marshal(
				new JAXBElement<GenericResponseType>(
						new QName(Constants.MM7_NS, "SubmitRsp"),
						(Class<GenericResponseType>) response.getResponse().getClass(),
						response.getResponse()),
						bodyNode);
    	
		return responseXml;
    }
}
