/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * AsfClient.java
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 * Module Information:
 *
 * DESCRIPTION:  Document Generation System application.
 * ===========================================================================
 */
/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * AsfClient.java
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 * Module Information:
 *
 * DESCRIPTION:  Document Generation System application.
 * ===========================================================================
 */
/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * AsfClient.java
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 * Module Information:
 *
 * DESCRIPTION:  Document Generation System application.
 * ===========================================================================
 */
/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * AsfClient.java
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 * Module Information:
 *
 * DESCRIPTION:  Document Generation System application.
 * ===========================================================================
 */
/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * AsfClient.java
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 * Module Information:
 *
 * DESCRIPTION:  Document Generation System application.
 * ===========================================================================
 */
/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * AsfClient.java
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 * Module Information:
 *
 * DESCRIPTION:  Document Generation System application.
 * ===========================================================================
 */
/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * AsfClient.java
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 * Module Information:
 *
 * DESCRIPTION:  Document Generation System application.
 * ===========================================================================
 */
package financing.tools.docgen.client.asf;

import java.rmi.RemoteException;
import java.util.HashMap;

import financing.tools.docgen.beans.AsfRequest;
import financing.tools.docgen.client.DGSClient;
import financing.tools.docgen.client.asf.response.AuthenticationException;
import financing.tools.docgen.client.asf.response.BinaryInstanceFormUnavailableException;
import financing.tools.docgen.client.asf.response.DocCommandServiceImplementationReturnValue;
import financing.tools.docgen.client.asf.response.InstanceCreationException;
import financing.tools.docgen.client.asf.response.InstanceTransformationException;
import financing.tools.docgen.util.DocgenLogger;
import financing.tools.docgen.util.keys.DocgenConstants;
import financing.tools.docgen.xml.databind.types.common.DocgenHeaders;

/**
 * The Class AsfClient.
 * 
 * This class has implementation to call ASF Web service through generated DocCommandServiceImplementationProxy. 
 * class.
 * 
 * @author Administrator
 */
public class AsfClient extends DGSClient {
	
	/** Log class */
	private static final String CLASS_NAME = 
		AsfClient.class.getName();
    /** Log class - app log */
	private static final String APP_LOG = 
		new StringBuffer("appLog.").append(CLASS_NAME).toString();
	/** Log class - entry log */
	private static final String ENTRY_LOG = 
		new StringBuffer("entryLog.").append(CLASS_NAME).toString();
	/** Log class - exit log */
	private static final String EXIT_LOG = 
		new StringBuffer("exitLog.").append(CLASS_NAME).toString();
	
	
	
	/**
	 * Instantiates a new asf client.
	 */
	public AsfClient() {
		super();		
	}


	/**
	 * Generate document.
	 * 
	 * This method calls ASF Web service through generated DocCommandServiceImplementationProxy
	 * 
	 * @param asfRequest ASF Request
	 * 
	 * @return the doc command service implementation return value
	 * @throws InstanceTransformationException 
	 * @throws AuthenticationException 
	 * @throws BinaryInstanceFormUnavailableException 
	 * @throws InstanceCreationException 
	 * @throws RemoteException 
	 */
	public DocCommandServiceImplementationReturnValue generateDocument(AsfRequest asfRequest) throws RemoteException, InstanceCreationException, BinaryInstanceFormUnavailableException, AuthenticationException, InstanceTransformationException{
		
		
		DocgenLogger.debug(ENTRY_LOG, "--> DocCommandServiceImplementationReturnValue generateDocument(AsfRequest asfRequest)");
		
		DocCommandServiceImplementationProxy asfServiceProxy = null;
		DocCommandServiceImplementationReturnValue returnValue = null;
		
		asfServiceProxy = (DocCommandServiceImplementationProxy) beanFactory.getBean(DocgenConstants.ASF_PROXY);
		
		DocgenLogger.debug(APP_LOG, "Calling ASF service...");
		java.util.HashMap headerMap = new HashMap();
		javax.xml.namespace.QName qn = new javax.xml.namespace.QName("Qname");
		  //	getting  other DgsHeaders variables to persist in DB
		String applicationName = DocgenConstants.BLANK;;
		String documentNumber = DocgenConstants.BLANK;;
		String clientLoggingId = DocgenConstants.BLANK;
		String dgsRequestId = DocgenConstants.BLANK;
		
		DocgenHeaders header = asfRequest.getDocgenHeaders();		
		applicationName = header.getApplicationName();		
		documentNumber = header.getDocumentNumber();
		clientLoggingId = header.getClientLoggingId();
		dgsRequestId = header.getDgsRequestID();		
		DocgenLogger.debug(APP_LOG,new StringBuffer("dgsrequestid in asfclient:").append(dgsRequestId).toString());
		
		StringBuffer docgenHeaderSB=new StringBuffer();
		docgenHeaderSB.append("<p58:DocgenHeaders xmlns:p58=\"http://w3.ibm.com/xmlns/ibmww/igf/docgen/CommonTypes\">");
		docgenHeaderSB.append("<ApplicationName>");
		docgenHeaderSB.append(applicationName );
		docgenHeaderSB.append( "</ApplicationName>");
		docgenHeaderSB.append("<DgsRequestID>");
		docgenHeaderSB.append(dgsRequestId);
		docgenHeaderSB.append("</DgsRequestID>");
		docgenHeaderSB.append("<DocumentNumber>");
		docgenHeaderSB.append(documentNumber);
		docgenHeaderSB.append("</DocumentNumber>");
		docgenHeaderSB.append("<ClientLoggingId>");
		docgenHeaderSB.append(clientLoggingId);
		docgenHeaderSB.append("</ClientLoggingId>");
		docgenHeaderSB.append("</p58:DocgenHeaders>");
		
		
		headerMap.put(qn,docgenHeaderSB.toString());			
		((javax.xml.rpc.Stub)asfServiceProxy.getDocCommandServiceImplementation())._setProperty(com.ibm.websphere.webservices.Constants.REQUEST_SOAP_HEADERS,headerMap);
		returnValue = asfServiceProxy.generateDocument(asfRequest.getTemplateName(), 
					asfRequest.getApplicationName(), 
					asfRequest.getDocumentLanguage(), 
					asfRequest.getParams(), 
					asfRequest.getTransactionalData(), 
					asfRequest.getPreviewFormatter(), 
					asfRequest.getDocumentRequest(), 
					asfRequest.getSessionLanguage(), 
					asfRequest.getPageLayoutName(), 
					asfRequest.getDcfOptions(), 
					asfRequest.getOutputType(), 
					asfRequest.getHostNickName());
	
      
		DocgenLogger.debug(APP_LOG, "Got response from ASF service...");
			
		DocgenLogger.debug(EXIT_LOG, "<-- DocCommandServiceImplementationReturnValue generateDocument(AsfRequest asfRequest)");
	
		
		return returnValue;		
	}	
}
