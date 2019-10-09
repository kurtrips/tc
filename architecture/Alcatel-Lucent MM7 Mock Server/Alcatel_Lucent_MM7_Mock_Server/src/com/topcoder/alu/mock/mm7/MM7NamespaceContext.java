/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import java.util.Iterator;

import javax.xml.XMLConstants;
import javax.xml.namespace.NamespaceContext;

import com.sun.xml.bind.marshaller.NamespacePrefixMapper;

/**
 * This class serves 2 purposes:
 * 1. It implements NamespaceContext so that namespace aware xpath expressions can be evaluated
 * 2. It implements NamespacePrefixMapper so that JAXB does not write arbitrary namespaces when marshalling data.
 * @author TCSDEVELOPER
 * @version 1.0
 * 
 */
public class MM7NamespaceContext extends NamespacePrefixMapper implements NamespaceContext {

	/**
	 * It is used by the XPath class to get the retrieve the namespace for a given prefix
	 * @param prefix The prefix for which to return namespace
	 * @return The namespace corresponding to the prefix 
	 */
	public String getNamespaceURI(String prefix) {
		 if (prefix.equals("env"))
             return Constants.SOAP_ENV_NS;
         else if (prefix.equals("mm7"))
             return Constants.MM7_NS;
         else
             return XMLConstants.NULL_NS_URI;
	}

	/**
	 * Not used
	 */
	public String getPrefix(String namespace) {
		// Not needed to implement
		return null;
	}

	/**
	 * Not used
	 */
	@SuppressWarnings("rawtypes")
	public Iterator getPrefixes(String namespace) {
		// Not needed to implement
		return null;
	}

	/**
	 * It is used by the NamespacePrefixMapper of JAXB to find the correct prefix to use when marshalling data.
	 * @param namespaceUri The namespace for which to return prefix
	 * @param suggestion not used
	 * @param requirePrefix not used
	 */
	@Override
	public String getPreferredPrefix(String namespaceUri, String suggestion, boolean requirePrefix) {
		 if (namespaceUri.equals(Constants.SOAP_ENV_NS))
             return "env";
         else if (namespaceUri.equals(Constants.MM7_NS))
             return "mm7";
         else if (namespaceUri.equals(Constants.XSI))
        	 return "xsi";
         else
             return null;
	}

}
