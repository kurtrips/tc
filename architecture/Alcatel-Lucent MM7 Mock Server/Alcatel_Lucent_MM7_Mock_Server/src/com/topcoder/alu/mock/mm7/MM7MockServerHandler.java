/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import java.beans.PersistenceDelegate;
import java.beans.XMLEncoder;
import java.io.BufferedOutputStream;
import java.io.FileOutputStream;
import java.math.BigInteger;
import java.util.ArrayList;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.HashMap;
import java.util.List;
import java.util.UUID;

import javax.xml.bind.JAXBElement;
import javax.xml.datatype.DatatypeConfigurationException;
import javax.xml.datatype.DatatypeFactory;
import javax.xml.datatype.XMLGregorianCalendar;
import javax.xml.namespace.QName;

import org.apache.mina.core.service.IoHandlerAdapter;
import org.apache.mina.core.session.IoSession;

import com.topcoder.alu.mock.mm7.entities.AddressType;
import com.topcoder.alu.mock.mm7.entities.DeliverReqType;
import com.topcoder.alu.mock.mm7.entities.DeliverRspType;
import com.topcoder.alu.mock.mm7.entities.DeliveryReportReqType;
import com.topcoder.alu.mock.mm7.entities.Detail;
import com.topcoder.alu.mock.mm7.entities.Fault;
import com.topcoder.alu.mock.mm7.entities.GenericResponseType;
import com.topcoder.alu.mock.mm7.entities.MmDeliveryStatusType;
import com.topcoder.alu.mock.mm7.entities.MultiAddressType;
import com.topcoder.alu.mock.mm7.entities.RecipientsType;
import com.topcoder.alu.mock.mm7.entities.ResponseStatusType;
import com.topcoder.alu.mock.mm7.entities.SubmitReqType;
import com.topcoder.alu.mock.mm7.entities.SubmitRspType;
import com.topcoder.alu.mock.mm7.entities.AddressType.RFC2822Address;

/**
 * This is the IOHander implementation of the server.
 * It receives the decoded entities, applies business logic to them and then,
 * writes other entities which are then encoded by the encoders and then written to the sockets.
 * See the messageReceived method documentation for more detail. 
 * @author TCSDEVELOPER
 * @version 1.0
 * 
 */
public class MM7MockServerHandler extends IoHandlerAdapter {
	
	/**
	 * Default constructor. Does nothing
	 */
	public MM7MockServerHandler() {
		
	}
	
	/**
	 * This method will be called when any unknown exception is caught.
	 * So a SOAP fault with server error (with code 3000) will be thrown
	 * @param session Session to write to
	 * @param cause The exception caught 
	 */
	public void exceptionCaught(IoSession session, Throwable cause) {
		System.out.println("UNKNOWN ERROR: " + cause.getLocalizedMessage());
		session.write(createFault(cause.getMessage()));
	}
	
	/**
	 * This method is called after the server receives a message and the message decoder has
	 * decoded the low-level (HTTP over TCP) message data into a high-level object. 
	 * This method is responsible for business logic and does not concern itself with the TCP encoding-decoding details.
	 * It receives 4 kinds of messages defined in the spec: 
	 * 1) MM7_submit.REQ - The server responds with one MM7_delivery_report.REQ message for each recipient 
	 * 2) MM7_delivery_report.RES - The server simply logs it to a file
	 * 3) MM7_deliver.RES - The server simply logs it to a file
	 * 4) MM1_submit.REQ - The server responds to the VASP with a MM7_deliver.REQ
	 * 
	 * NOTE: It can also be called in the case of a Fault. In that case the Fault itself is written to the session
	 * @param session Session to write to
	 * @param message The message received from the decoder
	 * @throws Exception Bubbles through any exceptions 
	 */
	public void messageReceived(IoSession session, Object message) throws Exception {
		
		if (message.getClass().equals(SubmitRequest.class)) {
			//This is a submit request. Server generates multiple delivery_report requests after this.
			handleSubmitRequest(session, message);
		}
		else if (message.getClass().equals(GenericMM7Response.class)) {
			System.out.println("Inside delivery response.");
			
			//This is either a delivery_request response or a delivery response
			//Either way we simply log it to some file on the server (because server takes no action after this)
			GenericMM7Response response = (GenericMM7Response) message;
			if (response.getResponse().getClass().equals(GenericResponseType.class)) {
				writeObjectToFile(response, "latest_delivery_request_response.log");
			}
			else if (response.getResponse().getClass().equals(DeliverRspType.class)) {
				writeObjectToFile(response, "latest_deliver_response.log");
			}
		}
		else if (message.getClass().equals(String.class) && ((String)message).equals(Constants.MM1_SUBMIT_REQ)) {
			//This is an incoming MM1_submit.REQ
			//Create a MM7_deliver.REQ and write to session. The encoder will then send it to VASP
			DeliverReqType deliverReqType = new DeliverReqType();
			deliverReqType.setMM7Version(Constants.MMS_VERSION);
			AddressType sender = new AddressType();
			RFC2822Address rfc2822Address = new RFC2822Address(); 
			rfc2822Address.setValue(Constants.MMS_SERVER_ADDRESS);
			sender.setRFC2822Address(rfc2822Address);
			deliverReqType.setSender(sender);
			
			session.write(deliverReqType);
		}
		else if (message.getClass().equals(Fault.class)) {
			//It is a fault. Write it as is.
			session.write(message);
		}
	}
	

	/**
	 * This method takes in a SubmitRequest and generates a bunch of MM7_delivery_report.REQ (one for each recipient)
	 * See the Figure 8. Sample data flow of MM7 message distribution of the spec for more details
	 * @param session The session onto which to write the delivery report requests
	 * @param message The SubmitRequest to which to respond
	 * @throws DatatypeConfigurationException If there is any error in data conversion
	 */
	private static void handleSubmitRequest(IoSession session, Object message) throws DatatypeConfigurationException {
		SubmitRequest submitRequest = (SubmitRequest) message;
		SubmitReqType submitReqType = (SubmitReqType) submitRequest.getRequest();
		
		//Generate a message id for this submit request
		String messageId = UUID.randomUUID().toString();
		
		//Check the recipients
		HashMap<AddressType, Boolean> recipientAddresses = getAddressesFromRecipients(submitReqType.getRecipients());
		int numValidRecipients = 0;
		for (AddressType address : recipientAddresses.keySet()) {
			if (recipientAddresses.get(address).equals(Boolean.TRUE)) {
				numValidRecipients++;
			}
		}

		//Generate the submit response
		GenericMM7Response submitReqResponse = populateSubmitResponse(
				numValidRecipients, recipientAddresses, messageId, submitRequest);
		
		//Write to session
		session.write(submitReqResponse);
		
		//Send the MM7 delivery report requests for each recipient
		List<DeliveryReportRequest> deliveryReports = 
			populateDeliveryReportRequests(recipientAddresses, submitRequest, messageId);
		
		//Write each MM7 delivery report request to session
		for (DeliveryReportRequest deliveryReport : deliveryReports) {
			session.write(deliveryReport);
		}
	}

	/**
	 * Extracts the Address from the Recipients.
	 * If there are many addresses for a recipient, only the last one will be returned.
	 * A mock rule is used that an address should start with '7' otherwise it is invalid. 
	 * @param recipients The recipients extracted from the MM7 Submit Request
	 * @return A hash map with a single address extracted for each recipient (key) and whether the 
	 * address is valid or not (value)
	 */
	private static HashMap<AddressType, Boolean> getAddressesFromRecipients(RecipientsType recipients) {
		HashMap<AddressType, Boolean> ret = new HashMap<AddressType, Boolean>();
		for (JAXBElement<MultiAddressType> recipient : recipients.getToOrCcOrBcc()) {
			List<Object> addresses = recipient.getValue().getRFC2822AddressOrNumberOrShortCode();
			boolean invalid = false;
			AddressType addressType = null;
			
			for (Object singleAddress : addresses) {
				Class<?> clazz = singleAddress.getClass();
				addressType = new AddressType();
				
				//Get the address based on the actual address type
				if (AddressType.Number.class.equals(clazz)) {
					addressType.setNumber((AddressType.Number)singleAddress);
					if (!addressType.getNumber().getValue().startsWith("7")) {
						invalid = true;
					}
				}
				else if (AddressType.RFC2822Address.class.equals(clazz)) {
					addressType.setRFC2822Address((AddressType.RFC2822Address)singleAddress);
					if (!addressType.getRFC2822Address().getValue().startsWith("7")) {
						invalid = true;
					}
				}
				else if (AddressType.ShortCode.class.equals(clazz)) {
					addressType.setShortCode((AddressType.ShortCode)singleAddress);
					if (!addressType.getShortCode().getValue().startsWith("7")) {
						invalid = true;
					}					
				}
			}
			
			if (invalid) {
				ret.put(addressType, Boolean.FALSE);
			}
			else {
				ret.put(addressType, Boolean.TRUE);
			}
		}
		return ret;
	}
	
	/**
	 * Simple method to return the current timestamp in XMLGregorianCalendar format  
	 * @return the current timestamp in XMLGregorianCalendar format
	 * @throws DatatypeConfigurationException If there is any error in data conversion
	 */
	private static XMLGregorianCalendar currentXMLDate() throws DatatypeConfigurationException {
		GregorianCalendar c = new GregorianCalendar();
		c.setTime(new Date());
		DatatypeFactory df = DatatypeFactory.newInstance();
		XMLGregorianCalendar x = df.newXMLGregorianCalendar(c);
		return x;
	}

	/**
	 * This method populates the MM7_Submit.RES for the MM7_Submit.REQ 
	 * @param numValidRecipients The number of valid recipients found
	 * @param recipientAddresses The parsed out addresses for each recipient
	 * @param messageId The message id for the request
	 * @param submitRequest The Submit Request itself
	 * @return A GenericMM7Response containing the TransactionId and SubmitRspType
	 */
	private static GenericMM7Response populateSubmitResponse(int numValidRecipients,
			HashMap<AddressType, Boolean> recipientAddresses, String messageId, SubmitRequest submitRequest) {
		
		ResponseStatusType responseStatus = new ResponseStatusType();
		SubmitRspType submitRspType = new SubmitRspType();
		SubmitReqType submitReqType = submitRequest.getRequest();
		
		if (numValidRecipients == 0) {
			//No addresses were right so error
			responseStatus.setStatusCode(BigInteger.valueOf(2002));
			responseStatus.setStatusText("Address Error");
		} 
		else if (numValidRecipients < recipientAddresses.size()) {
			//Some addresses were right so partial success
			responseStatus.setStatusCode(BigInteger.valueOf(1100));
			responseStatus.setStatusText("Partial success");
			submitRspType.setMessageID(messageId);
		} 
		else {
			responseStatus.setStatusCode(BigInteger.valueOf(1000));
			responseStatus.setStatusText("Success");
			submitRspType.setMessageID(messageId);
		}
		
		//Populate the submit response
		submitRspType.setMM7Version(submitReqType.getMM7Version());
		submitRspType.setStatus(responseStatus);
		
		//Create the GenericMM7Response wrapper
		GenericMM7Response submitReqResponse = new GenericMM7Response();
		submitReqResponse.setHeader(submitRequest.getHeader());
		submitReqResponse.setResponse(submitRspType);
		
		return submitReqResponse;
	}
	
	/**
	 * This method populates the MM7_Delivery_Report.REQ instances for the MM7_Submit.REQ
	 * One MM7_Delivery_Report.REQ instance for each recipient.
	 * @param recipientAddresses The parsed out addresses for each recipient
	 * @param messageId The message id for the request
	 * @param submitRequest The Submit Request itself
	 * @return A lsit of DeliveryReportRequest instances, one for each recipeint 
	 * @throws DatatypeConfigurationException If there is any issue with creating the XmlDate
	 */
	private static List<DeliveryReportRequest> populateDeliveryReportRequests(
			HashMap<AddressType, Boolean> recipientAddresses, SubmitRequest submitRequest, String messageId)
			throws DatatypeConfigurationException {
		
		SubmitReqType submitReqType = submitRequest.getRequest();
		List<DeliveryReportRequest> ret = new ArrayList<DeliveryReportRequest>();
		
		for (AddressType recipientAddress : recipientAddresses.keySet()) {
			DeliveryReportReqType deliverReportReq = new DeliveryReportReqType();
			deliverReportReq.setMM7Version(submitReqType.getMM7Version());
			deliverReportReq.setMMSRelayServerID("Mock Alcatel Lucent MMS Server");
			//Note that this is the same message id generated by server for the submit request
			deliverReportReq.setMessageID(messageId);
			//Set the recipient 
			deliverReportReq.setRecipient(recipientAddress);
			//Set sender - This might not be right because it might not even be there
			deliverReportReq.setSender(submitReqType.getSenderIdentification().getSenderAddress());
			//Set to current date
			deliverReportReq.setDate(currentXMLDate());
			//Set the mm status 
			//If not a valid address then status = Rejected otherwise status = Delivered
			//Note that RETRIEVED is equivalent to delivered!
			if (recipientAddresses.get(recipientAddress).equals(Boolean.TRUE)) {
				deliverReportReq.setMMStatus(MmDeliveryStatusType.RETRIEVED);
			}
			else {
				deliverReportReq.setMMStatus(MmDeliveryStatusType.REJECTED);
			}
			
			DeliveryReportRequest deliverReportReqWrap = new DeliveryReportRequest();
			deliverReportReqWrap.setHeader(submitRequest.getHeader());
			deliverReportReqWrap.setRequest(deliverReportReq);
			ret.add(deliverReportReqWrap);
		}
		
		return ret;
	}

	/**
	 * Simple method which takes an object and converts it into XML and then writes it to a file
	 * It is used for logging-like purpose 
	 * @param object The object to be written
	 * @param filename The name of the file to be written
	 * @throws Exception Any issue with the encoding of the xml
	 */
    private static void writeObjectToFile(Object object, String filename) throws Exception{
        XMLEncoder encoder =
           new XMLEncoder(
              new BufferedOutputStream(
                new FileOutputStream(filename)));
        
        PersistenceDelegate pd=encoder.getPersistenceDelegate(Integer.class);
        encoder.setPersistenceDelegate(BigInteger.class, pd);
        
        encoder.writeObject(object);
        encoder.close();
    }

    /**
     * Creates a server Fault instance with its status code set to 3000.
     * This is used to report unexpected errors i.e. when the server raised an exception when it shouldn't have done so
     * @param errorMessage The error message to populate
     * @return The created Fault instance
     */
	private Fault createFault(String errorMessage) {
		Fault fault = new Fault();
		fault.setFaultcode(new QName("env", "Server"));
		fault.setFaultstring("Server error");
		
		GenericResponseType rsErrorRsp = new GenericResponseType();
		rsErrorRsp.setMM7Version(Constants.MMS_VERSION);

		ResponseStatusType responseStatusType = new ResponseStatusType();
		responseStatusType.setStatusCode(BigInteger.valueOf(3000));
		responseStatusType.setStatusText(errorMessage);
		
		Detail detail = new Detail();
		detail.getAny().add(responseStatusType);
		
		fault.setDetail(detail);
		return fault;
	}
}
