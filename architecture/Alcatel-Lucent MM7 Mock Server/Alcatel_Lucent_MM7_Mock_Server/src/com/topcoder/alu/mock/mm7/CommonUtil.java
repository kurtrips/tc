/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import java.io.ByteArrayOutputStream;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Marshaller;
import javax.xml.transform.Result;
import javax.xml.transform.Source;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Node;

/**
 * @author TCSDEVELOPER
 * This class is used for refactoring common functionality into helper methods.
 * @version 1.0 
 * 
 */
class CommonUtil {
	
	/**
	 * An instance of a Marshaller which is used by many encoders.
	 * Once initialized, it is not changed afterwards
	 */
	private static Marshaller marshaller = null;
	
	/**
	 * Converts a Node into a byte array
	 * @param node The Node which has to be converted
	 * @return The byte array representation of node
	 * @throws TransformerException If there's an issue with the transform from Node to byte array
	 */
    public static byte[] docToBytes(Node node) throws TransformerException  {
		Source source = new DOMSource(node);
		ByteArrayOutputStream out = new ByteArrayOutputStream();
		Result result = new StreamResult(out);
		TransformerFactory factory = TransformerFactory.newInstance();
		Transformer transformer = factory.newTransformer();
		transformer.transform(source, result);
		return out.toByteArray();
    }
    
    /**
     * Gets a Marshaller instance which is used by the encoders.
     * It always returns the same instance
     * @return The Marshaller instance
     * @throws JAXBException If there's any issue with creation of JAXBContext or Marshaller
     */
    public static Marshaller getMarshaller() throws JAXBException {
    	if (marshaller == null) {
			JAXBContext jaxbContext = JAXBContext.newInstance("com.topcoder.alu.mock.mm7.entities");
			marshaller = jaxbContext.createMarshaller();
			marshaller.setProperty("com.sun.xml.bind.namespacePrefixMapper", new MM7NamespaceContext());
    	}
    	return marshaller;
    }
}
