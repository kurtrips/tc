/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import java.io.OutputStream;
import java.net.InetAddress;
import java.net.Socket;
import java.util.UUID;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.Marshaller;
import javax.xml.namespace.QName;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.filter.codec.ProtocolEncoderOutput;
import org.apache.mina.filter.codec.demux.MessageEncoder;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import com.topcoder.alu.mock.mm7.entities.DeliverReqType;
import com.topcoder.alu.mock.mm7.entities.TransactionID;

/**
 * This class is the encoder for the MM7_deliver.REQ.
 * It converts the DeliverReqType instance into the soap format which can then be sent to the VASP server
 * @author TCSDEVELOPER
 * @version 1.0
 */
public class DeliverRequestEncoder implements MessageEncoder<DeliverReqType> {

	/**
	 * Converts a DeliverReqType instance into the proper soap format.
	 * It then writes the soap to the VASP server's socket connection.
	 * NOTE that the VASP server's socket is not the same as the client for the current session (which is the MMUA)
	 * @throws Exception If anything goes wrong with the encode
	 * @param session The current session
	 * @param deliverReqType The DeliverReqType which needs to be serialized to soap
	 * @param op The ProtocolEncoderOutput into which to write
	 */
	public void encode(IoSession session, DeliverReqType deliverReqType, ProtocolEncoderOutput op) throws Exception {
        Socket vaspClientSock = new Socket(InetAddress.getLocalHost(), Constants.VASP_PORT);
        OutputStream os = vaspClientSock.getOutputStream();
        
		//First write the http headers to the output stream of a separate socket i.e. VASP socket
        os.write(Constants.HTTP_RESP_HEADER.getBytes());
		
		Document deliverRequestXml = buildDeliverRequestXml(deliverReqType);
		
		//Write the created deliverRequestXml to the output stream of a separate socket i.e. VASP socket
		os.write(CommonUtil.docToBytes(deliverRequestXml));
		os.flush();
		os.close();
	}
	

	/**
	 * Builds a soap document from the DeliverReqType instance
	 * @param deliverReqType The input DeliverReqType instance
	 * @return The sopa document
	 * @throws Exception If any error encountered during the conversion
	 */
    @SuppressWarnings("unchecked")
	private static Document buildDeliverRequestXml(DeliverReqType deliverReqType) throws Exception {
    	Marshaller marshaller = CommonUtil.getMarshaller();
    	
    	//Create the soap response
		DocumentBuilderFactory documentBuilderFactory = DocumentBuilderFactory.newInstance();
		DocumentBuilder documentBuilder = documentBuilderFactory.newDocumentBuilder();
		Document xml = documentBuilder.newDocument();
		
		//Create the soap envelope
		Element envelopeNode = xml.createElementNS(
				Constants.SOAP_ENV_NS, "env:Envelope");
		xml.appendChild(envelopeNode);
		
		//Create the soap header
		Element headerNode = xml.createElementNS(Constants.SOAP_ENV_NS, "env:Header");
		envelopeNode.appendChild(headerNode);
		
		//Create the soap body
		Element bodyNode = xml.createElementNS(Constants.SOAP_ENV_NS, "env:Body");
		envelopeNode.appendChild(bodyNode);
		
		//Marshall the TransactionId node. Note that here it is server generated.
		TransactionID transactionId = new TransactionID();
		transactionId.setValue(UUID.randomUUID().toString());
		marshaller.marshal(transactionId, headerNode);

		//Marshall the DeliverReq node
		marshaller.marshal(
				new JAXBElement<DeliverReqType>(
						new QName(Constants.MM7_NS, "DeliverReq"),
						(Class<DeliverReqType>) deliverReqType.getClass(),
						deliverReqType),
						bodyNode);
    	
		return xml;
    }
}
