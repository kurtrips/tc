/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import java.io.IOException;
import java.io.InputStream;
import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBElement;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.xpath.XPath;
import javax.xml.xpath.XPathConstants;
import javax.xml.xpath.XPathExpressionException;
import javax.xml.xpath.XPathFactory;

import org.apache.james.mime4j.MimeException;
import org.apache.james.mime4j.descriptor.BodyDescriptor;
import org.apache.james.mime4j.parser.AbstractContentHandler;
import org.apache.james.mime4j.parser.Field;
import org.w3c.dom.Document;
import org.w3c.dom.Node;
import org.xml.sax.SAXException;

import com.topcoder.alu.mock.mm7.entities.DeliverRspType;
import com.topcoder.alu.mock.mm7.entities.GenericResponseType;
import com.topcoder.alu.mock.mm7.entities.SubmitReqType;
import com.topcoder.alu.mock.mm7.entities.TransactionID;

/**
 * This class is an implemenetation of Mime4j's ContentHandler which parses out the
 * various headers and different content sections of a multipart http message.
 * It is a SAX like parser which calls various methods in response to start and end of sections, body, fields etc.
 * It is mainly used to parse out the http and convert it into entities which can then be passed on to the IoHandler.
 * It is also used to check the authorization header. 
 * @author TCSDEVELOPER 
 * @version 1.0
 */
public class MM7IncomingMessageContentHandler extends AbstractContentHandler {

	/**
	 * The entity which has been parsed out from the http message
	 */
	private Object contentHandlerOutput;
	
	/**
	 * The message type of http message
	 */
	private IncomingMessageType incomingMessageType;
	
	/**
	 * Whether the authorization had the correct credentials
	 */
	private boolean authPassed = false;
	
	/**
	 * Gets whether the authorization had the correct credentials
	 * @return the authPassed
	 */
	public boolean isAuthPassed() {
		return authPassed;
	}

	/**
	 * Gets the entity which has been parsed out from the http message
	 * @return the contentHandlerOutput
	 */
	public Object getContentHandlerOutput() {
		return contentHandlerOutput;
	}

	/**
	 * Gets the message type of http message
	 * @return the incomingMessageType
	 */
	public IncomingMessageType getIncomingMessageType() {
		return incomingMessageType;
	}
	
	/**
	 * This method is called for each header field.
	 * We will be using this for checking for proper authentication and authorization
	 * Only username="Aladdin" and password="open sesame" is allowed access to this server
	 * Please see http://en.wikipedia.org/wiki/Basic_access_authentication for more details
	 */
	@Override
	public void field(Field field) {
		if (field.getName().equals("Authorization") && 
				field.getBody().equals(" Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==")) {
			authPassed = true;
		}
	}

	/**
	 * This method is called when the body of a part of multipart message is reached.
	 * In this case, we will read the SOAP request from the body and store it.
	 * All further body(s) are part of the mms content which we do not need here. 
	 */
	@Override
	public void body(BodyDescriptor bodyDescriptor, InputStream inputStream)
			throws MimeException, IOException {
		if (contentHandlerOutput != null) {
			return;
		}
		
		DocumentBuilderFactory documentBuilderFactory = DocumentBuilderFactory.newInstance();
		documentBuilderFactory.setNamespaceAware(true);
		DocumentBuilder documentBuilder = null;
		Document soapContent;
		try {
			//Get the soap request
			documentBuilder = documentBuilderFactory.newDocumentBuilder();
			soapContent = documentBuilder.parse(inputStream);

			JAXBContext jaxbContext = JAXBContext.newInstance("com.topcoder.alu.mock.mm7.entities");
			Unmarshaller um = jaxbContext.createUnmarshaller();
			
			XPathFactory factory = XPathFactory.newInstance();
			XPath xpath = factory.newXPath();
			MM7NamespaceContext nsContext = new MM7NamespaceContext(); 
			xpath.setNamespaceContext(nsContext);
			
			//Get the TransactionID node
			Node transactionIdNode = (Node) 
				xpath.evaluate("./env:Header/mm7:TransactionID", soapContent.getDocumentElement(), XPathConstants.NODE);
			TransactionID transactionID = (TransactionID) um.unmarshal(transactionIdNode);
			
			//Get the body of the soap and use it to get the actual incoming message type
			Node node = interpretIncomingMessageType(soapContent, xpath);
			
			//Unmarshal according to message type
			unmarshalIncomingMessageType(node, um, transactionID);
		} catch (JAXBException e) {
			throw new IOException(e);
		} catch (XPathExpressionException e) {
			throw new IOException(e);
		} catch (SAXException e) {
			throw new IOException(e);
		} catch (ParserConfigurationException e) {
			throw new IOException(e);
		}
	}
	
	/**
	 * This method actually performs the unmarshalling of a Document's Node to an Entity,
	 * based upon the message type
	 * @param node The Node which has to be unmarshaleld
	 * @param um The unmarshaller
	 * @throws JAXBException If there's any exception while unmarshalling 
	 */
	@SuppressWarnings("unchecked")
	private void unmarshalIncomingMessageType(
		Node node, Unmarshaller um, TransactionID transactionID) throws JAXBException {
		if (node == null) {
			return;
		}
		switch (incomingMessageType) {
			case SUBMIT_REQ: {
				SubmitReqType submitReqType = ((JAXBElement<SubmitReqType>) um.unmarshal(node)).getValue();

				SubmitRequest submitRequest = new SubmitRequest();
				submitRequest.setHeader(transactionID);
				submitRequest.setRequest(submitReqType);
				contentHandlerOutput = submitRequest;

				break;
			}
			case DELIVERY_REPORT_RES: {
				GenericResponseType deliverReportRspType = ((JAXBElement<GenericResponseType>) um.unmarshal(node)).getValue();

				GenericMM7Response genericMM7Response = new GenericMM7Response();
				genericMM7Response.setHeader(transactionID);
				genericMM7Response.setResponse(deliverReportRspType);
				contentHandlerOutput = genericMM7Response;

				break;
			}
			case DELIVERY_RES: {
				DeliverRspType deliverRspType = ((JAXBElement<DeliverRspType>) um.unmarshal(node)).getValue();

				GenericMM7Response genericMM7Response = new GenericMM7Response();
				genericMM7Response.setHeader(transactionID);
				genericMM7Response.setResponse(deliverRspType);
				contentHandlerOutput = genericMM7Response;

				break;
			}
		}
	}

	/**
	 * This method checks the soap document and finds out the message type based on the contained nodes.
	 * If a known message type is found, then that node is returned
	 * If no known message type is found, it returns null
	 * @param soapContent The SOAP document which will be inspected
	 * @param xpath The XPath instance which will be used to inspect nodes
	 * @return The Document Node which is the found message type
	 * @throws XPathExpressionException If there's any exception while inspecting nodes using xpath
	 */
	private Node interpretIncomingMessageType(Document soapContent, XPath xpath) 
		throws XPathExpressionException {
		Node node = null;
		
		node = (Node) xpath.evaluate(
				"./env:Body/mm7:SubmitReq", soapContent.getDocumentElement(), XPathConstants.NODE);
		
		if (node != null) {
			incomingMessageType =  IncomingMessageType.SUBMIT_REQ;
			return node;
		}
		
		node = (Node) xpath.evaluate(
				"./env:Body/mm7:DeliverRsp", soapContent.getDocumentElement(), XPathConstants.NODE);
		
		if (node != null) {
			incomingMessageType =  IncomingMessageType.DELIVERY_RES;
			return node;
		}
		
		node = (Node) xpath.evaluate(
				"./env:Body/mm7:DeliveryReportRsp", soapContent.getDocumentElement(), XPathConstants.NODE);
		
		if (node != null) {
			incomingMessageType =  IncomingMessageType.DELIVERY_REPORT_RES;
			return node;
		}
		
		incomingMessageType =  IncomingMessageType.NONE;
		return null;
	}
	
	/**
	 * A simple enum which contains all known message types which the server can receive.
	 * @author TCSDEVELOPER 
	 * @version 1.0
	 *
	 */
	enum IncomingMessageType {
		SUBMIT_REQ,
		DELIVERY_REPORT_RES,
		DELIVERY_RES,
		NONE
	}
}
