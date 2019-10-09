/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import java.util.UUID;

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
import com.topcoder.alu.mock.mm7.entities.Fault;
import com.topcoder.alu.mock.mm7.entities.TransactionID;

/**
 * This class is the encoder for the MM7's Faults
 * It converts the Fault instance into the soap format which can then be written as http over tcp
 * @author TCSDEVELOPER
 * @version 1.0
 */
public class FaultEncoder implements MessageEncoder<Fault> {

	/**
	 * Converts a Fault instance into the proper soap format.
	 * It then writes the soap to the VASP server's socket connection
	 * @throws Exception If anything goes wrong with the encode
	 * @param session The current session
	 * @param fault The Fault which needs to be serialized to soap
	 * @param op The ProtocolEncoderOutput into which to write 
	 */
	public void encode(IoSession session, Fault fault, ProtocolEncoderOutput op) throws Exception {
		//First write the http headers to the output stream
        op.write(IoBuffer.wrap(Constants.HTTP_RESP_ERROR_HEADER.getBytes()));
		
		Document faultXml = buildFaultXml(fault);
		
		//Write the created faultXml to the output stream
		op.write(IoBuffer.wrap(CommonUtil.docToBytes(faultXml)));
		op.flush();
	}
	
	/**
	 * Builds a soap document from the Fault instance
	 * @param fault The input Fault instance
	 * @return The soap document
	 * @throws Exception If any error encountered during the conversion
	 */
    @SuppressWarnings("unchecked")
	private static Document buildFaultXml(Fault fault) throws Exception {
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

		//Marshall the Fault node
		marshaller.marshal(
				new JAXBElement<Fault>(
						new QName(Constants.MM7_NS, "RSErrorRsp"),
						(Class<Fault>) fault.getClass(),
						fault),
						bodyNode);
    	
		return xml;
    }
}
