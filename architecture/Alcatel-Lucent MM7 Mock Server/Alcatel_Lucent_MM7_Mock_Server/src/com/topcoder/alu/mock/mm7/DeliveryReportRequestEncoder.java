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
import com.topcoder.alu.mock.mm7.entities.DeliveryReportReqType;

/**
 *  
 * This class is the encoder for the MM7_delivery_report.REQ.
 * It converts the DeliveryReportRequest instance into the soap format which can then be sent to the VASP server.
 * @author TCSDEVELOPER
 * @version 1.0
 * 
 */
public class DeliveryReportRequestEncoder implements MessageEncoder<DeliveryReportRequest> {

	/**
	 * Converts a DeliveryReportRequest instance into the proper soap format,
	 * so that it can be written over the socket as the body of an http response
	 * @param session The current session
	 * @param deliveryReportRequest The DeliveryReportRequest to be converted to soap
	 * @param op The ProtocolEncoderOutput instance into which to write
	 * @throws Exception If there is any issue in the conversion
	 */
	public void encode(
			IoSession session, DeliveryReportRequest deliveryReportRequest, ProtocolEncoderOutput op) throws Exception {
		
		//First write the http headers to the output stream
		op.write(IoBuffer.wrap(Constants.HTTP_RESP_HEADER.getBytes()));
		
		Document deliveryReportRequestXml = buildDeliveryReportRequestXml(deliveryReportRequest);
		
		//Write the created delivery report request document to the output stream
		op.write(IoBuffer.wrap(CommonUtil.docToBytes(deliveryReportRequestXml)));
		op.flush();
	}
	
	/**
	 * Builds the soap document from the DeliveryReportRequest.
	 * @param deliveryReportRequest The deliveryReportRequest from which to build
	 * @return The soap document
	 * @throws Exception If there is any issue in the conversion
	 */
    @SuppressWarnings("unchecked")
	private static Document buildDeliveryReportRequestXml(
			DeliveryReportRequest deliveryReportRequest) throws Exception {
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
		marshaller.marshal(deliveryReportRequest.getHeader(), headerNode);

		//Marshall the DeliveryReportReq node
		marshaller.marshal(
				new JAXBElement<DeliveryReportReqType>(
						new QName(Constants.MM7_NS, "DeliveryReportReq"),
						(Class<DeliveryReportReqType>) deliveryReportRequest.getRequest().getClass(),
						deliveryReportRequest.getRequest()),
						bodyNode);
    	
		return responseXml;
    }
}
