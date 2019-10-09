/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * CreateDocumentWebService.java
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
///**
// * CreateDocumentWebServiceSOAPImpl.java
// *
// * This file was auto-generated from WSDL
// * by the IBM Web services WSDL2Java emitter.
// * cf90721.10 v53107135043
// */
package financing.tools.docgen.service.create;

import java.io.IOException;
import java.rmi.RemoteException;
import java.util.Properties;
import javax.activation.DataHandler;
import javax.activation.DataSource;
import org.apache.soap.util.mime.ByteArrayDataSource;
import org.hibernate.Hibernate;
import org.hibernate.HibernateException;
import org.hibernate.ObjectNotFoundException;
import org.hibernate.Session;
import financing.tools.docgen.beans.AsfRequest;
import financing.tools.docgen.beans.hibernate.AsfHostNickname;
import financing.tools.docgen.beans.hibernate.DgsFormattingParameters;
import financing.tools.docgen.client.asf.AsfClient;
import financing.tools.docgen.client.asf.request.DocCommandServiceImplementationTransactionalDataValue;
import financing.tools.docgen.client.asf.response.AuthenticationException;
import financing.tools.docgen.client.asf.response.BinaryInstanceFormUnavailableException;
import financing.tools.docgen.client.asf.response.DocCommandServiceImplementationReturnValue;
import financing.tools.docgen.client.asf.response.InstanceCreationException;
import financing.tools.docgen.client.asf.response.InstanceTransformationException;
import financing.tools.docgen.service.DGSWebService;
import financing.tools.docgen.util.DGSMessageProperties;
import financing.tools.docgen.util.DocgenCommonUtil;
import financing.tools.docgen.util.DocgenLogger;
import financing.tools.docgen.util.keys.DocgenConstants;
import financing.tools.docgen.xml.databind.create.request.CreateDocumentWebServiceRequest;
import financing.tools.docgen.xml.databind.create.response.CreateDocumentWebServiceResponse;
import financing.tools.docgen.xml.databind.faults.ibm.MovedPermanently;
import financing.tools.docgen.xml.databind.faults.ibm.ServiceUnavailable;
import financing.tools.docgen.xml.databind.types.common.DocgenHeaders;
import financing.tools.docgen.xml.databind.types.error.BusinessErrorType;
import financing.tools.docgen.xml.databind.types.error.SystemErrorType;

/**
 * The Class CreateDocumentWebServiceSOAPImpl.
 * 
 * This class implements Document Generation Web Service.
 */
public class CreateDocumentWebService extends DGSWebService
		implements
		CreateDocumentWebService_PortType {
	


	/** Log class */
	private static final String CLASS_NAME = 
		CreateDocumentWebService.class.getName();
    /** Log class - app log */
	private static final String APP_LOG = 
		new StringBuffer("appLog.").append(CLASS_NAME).toString();
	/** Log class - entry log */
	private static final String ENTRY_LOG = 
		new StringBuffer("entryLog.").append(CLASS_NAME).toString();
	/** Log class - exit log */
	private static final String EXIT_LOG = 
		new StringBuffer("exitLog.").append(CLASS_NAME).toString();
	
	
	/** The asf service client. */
	private AsfClient asfServiceClient = null;
	
	/**To read from properties file*/
	private Properties msgProperties=null; 
	/**
	 * Instantiates a new creates the document web service soap impl.
	 */
	public CreateDocumentWebService(){
		super();
	}

	/**
	 * Creates the document.
	 * 
	 * This method will be invoked by corresponding spring proxy class 
	 * when calls document creation web service.
	 * 
	 * @param request the request
	 * @param headers the headers
	 * 
	 * @return the creates the document web service response
	 * 
	 * @throws RemoteException the remote exception
	 * @throws SystemErrorType the system error type fault
	 * @throws MovedPermanently the moved permanently fault
	 * @throws BusinessErrorType the business error type fault
	 * @throws ServiceUnavailable the service unavailable fault
	 */
	public CreateDocumentWebServiceResponse createDocument(
			CreateDocumentWebServiceRequest request,
			DocgenHeaders headers)
			throws RemoteException,
			SystemErrorType,
			MovedPermanently,
			BusinessErrorType,
			ServiceUnavailable {		

		CreateDocumentWebServiceResponse response = null;
		AsfRequest asfRequest = null;
		String faultName = null;
		String faultMessage = null;
		String eMailText = null;
		String applicationName = null;
		String documentNumber = null;
		
		DocCommandServiceImplementationReturnValue asfresponse = null;
		
		DocgenLogger.debug(ENTRY_LOG, "--> CreateDocumentWebServiceResponse createDocument(CreateDocumentWebServiceRequest request,financing.tools.docgen.service.common.types.DocgenHeaders headers)");
		
		if(headers != null){
			applicationName = headers.getApplicationName();
			documentNumber = headers.getDocumentNumber();
		}
			
		logHeaders(headers);	
		if(msgProperties==null){
			msgProperties=DGSMessageProperties.getInstance().getProperties();
		}
		try { 
			DocgenLogger.debug(APP_LOG, "Control is just before createAsfRequest method..");
			/*
			 * Populate ASF web service request parameters in AsfRequest. If PredefinedAsfDocTypes is 
			 * equals to client then populate all values from request else populate host nickname from
			 * ASF_HOST_NICKNAME table and formatting parameters from DGS_FORMATTING_PARAMETERS table.
			 */
			asfRequest = createAsfRequest(request , headers);
			
			DocgenLogger.debug(APP_LOG, "Control is just after createAsfRequest method..");
			
			DocgenLogger.debug(APP_LOG, "Control is just before getAsfServiceClient().generateDocument(asfRequest)");
			/*
			 * Call ASF web service. 
			 */
			asfresponse = getAsfServiceClient().generateDocument(asfRequest);
			
			DataSource ds1 = null;//DataSource ds2 = null;
	    	DataSource ds = null;	
	    	try{
		    	ds=asfresponse.getDocumentInstance().getDataSource();		    	
		    	ds1 = new ByteArrayDataSource(ds.getInputStream(),ds.getContentType()) ;		        
		        asfresponse.setDocumentInstance(new DataHandler(ds1)); 
		           	       
	    	}catch(IOException e){
	    		DocgenLogger.error(APP_LOG,new StringBuffer("Exception in fix for concurrentModificationCode").append(e).toString());    	
	    	}
			DocgenLogger.debug(APP_LOG, "Control is just after getAsfServiceClient().generateDocument(asfRequest)");

			DocgenLogger.debug(APP_LOG, " Control is just before createResponse(asfresponse)");
			/*
			 * Create response.
			 */
			response = createResponse(asfresponse);
			DocgenLogger.debug(APP_LOG, " Control is just after createResponse(asfresponse)");
		} catch (InstanceCreationException e) {
			
			DocgenLogger.error(APP_LOG,"DC received InstanceCreationException from ASF");
			
			throw new BusinessErrorType(getProperty(DocgenConstants.DC_RECEIVED_INSTANCE_CREATION_EXCEPTION), e.getMessage(),msgProperties.getProperty(DocgenConstants.DC_XXXX));
			
		} catch (BinaryInstanceFormUnavailableException e) {
			
			DocgenLogger.error(APP_LOG,"DC received BinaryInstanceFormUnavailableException from ASF");
			
			throw new BusinessErrorType(getProperty(DocgenConstants.DC_RECEIVED_BINARY_INSTANCE_FORM_UN_AVAILABLE_EXCEPTION), e.getMessage(),msgProperties.getProperty(DocgenConstants.DC_XXXX));
			
		} catch (AuthenticationException e) {
			
			DocgenLogger.error(APP_LOG,"DC received AuthenticationException from ASF");
			
			throw new BusinessErrorType(getProperty(DocgenConstants.DC_RECEIVED_AUTHENTICATION_EXCEPTION), e.getMessage(),msgProperties.getProperty(DocgenConstants.DC_XXXX));
			
		} catch (InstanceTransformationException e) {
			
			DocgenLogger.error(APP_LOG,"DC received InstanceTransformationException from ASF");
			
			throw new BusinessErrorType(getProperty(DocgenConstants.DC_RECEIVED_INSTANCE_TRANSFORMATION__EXCEPTION), e.getMessage(),msgProperties.getProperty(DocgenConstants.DC_XXXX));
			
		} catch (RemoteException e) {
			
			String[] params = new String[3];
			params[0] = e.toString();
			params[1] = applicationName;
			params[2] = documentNumber;
			
			faultMessage = DocgenCommonUtil.getCompletedMessage(DocgenConstants.DC_RECEIVED_REMOTE_EXCEPTION, params);
			eMailText = DocgenCommonUtil.getCompletedMessage(DocgenConstants.DC_RECEIVED_REMOTE_EXCEPTION, params); 
			faultName = getProperty(DocgenConstants.DC_RECEIVED_REMOTE_EXCEPTION_FAULT_NAME);
				
			handleFatalError(eMailText);				
					
			DocgenLogger.error(APP_LOG,"DC received RemoteException while calling ASF Web Service");
			DocgenLogger.error(APP_LOG,faultMessage);	
			
			throw new SystemErrorType(faultName , faultMessage, msgProperties.getProperty(DocgenConstants.DC_RECEIVED_REMOTE_ERROR_CODE), DocgenConstants.DC_IMPLEMENTATION, null);
			
		}finally{
			
			DocgenLogger.debug(EXIT_LOG,"<-- CreateDocumentWebServiceResponse createDocument(CreateDocumentWebServiceRequest request,financing.tools.docgen.service.common.types.DocgenHeaders headers)");
		}	
		
		return response;
	}

	
	
	/**
	 * Log headers.
	 * 
	 * @param headers the headers
	 */
	private void logHeaders(DocgenHeaders headers) {
		
		DocgenLogger.debug(ENTRY_LOG,"--> void logHeaders(DocgenHeaders headers)");
		
		if(headers != null){
			DocgenLogger.debug(APP_LOG,"Document Web Service implementation received the following headers. ");
			DocgenLogger.debug(APP_LOG,new StringBuffer("ApplicationName = ").append(headers.getApplicationName()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("ClientLoggingId = ").append(headers.getClientLoggingId()).toString());	
			DocgenLogger.debug(APP_LOG,new StringBuffer("DgsRequestID = ").append(headers.getDgsRequestID()).toString());	
			DocgenLogger.debug(APP_LOG,new StringBuffer("DocumentNumber = ").append(headers.getDocumentNumber()).toString());	
			DocgenLogger.debug(APP_LOG,new StringBuffer("TypeOfDocument = ").append(headers.getTypeOfDocument()).toString());	
			DocgenLogger.debug(APP_LOG,"End of headers logging..");
		}		
		DocgenLogger.debug(EXIT_LOG,"<-- void logHeaders(DocgenHeaders headers)");
	}

	

	/**
	 * Creates the response.
	 * 
	 * @param asfResponse the asf response
	 * 
	 * @return the document web service response
	 */
	private CreateDocumentWebServiceResponse createResponse(
			DocCommandServiceImplementationReturnValue asfResponse) {
		
		DocgenLogger.debug(ENTRY_LOG,"--> CreateDocumentWebServiceResponse createresponse(DocCommandServiceImplementationReturnValue asfresponse)");

		final CreateDocumentWebServiceResponse response = new CreateDocumentWebServiceResponse();
		
		DocgenLogger.debug(APP_LOG,"Control is in create method...");
		response.setDocument(asfResponse.getDocumentInstance());
		DocgenLogger.debug(APP_LOG,new StringBuffer("DocumentType returned from ASF is ").append(asfResponse.getDocumentType()).toString());
		
		DocgenLogger.debug(EXIT_LOG,"<-- CreateDocumentWebServiceResponse createresponse(DocCommandServiceImplementationReturnValue asfresponse)");
		return response;
	}

	/**
	 * Gets the asf service client.
	 * 
	 * @return the asfServiceClient
	 */
	public AsfClient getAsfServiceClient() {
		DocgenLogger.debug(ENTRY_LOG,"--> AsfClient getAsfServiceClient()");
		DocgenLogger.debug(EXIT_LOG,"<-- AsfClient getAsfServiceClient()");
		return asfServiceClient;
	}

	/**
	 * Sets the asf service client.
	 * 
	 * @param asfServiceClient the asfServiceClient to set
	 */
	public void setAsfServiceClient(AsfClient asfServiceClient) {
		DocgenLogger.debug(ENTRY_LOG,"--> void setAsfServiceClient(AsfClient asfServiceClient)");
		
		this.asfServiceClient = asfServiceClient;
		
		DocgenLogger.debug(EXIT_LOG,"<-- void setAsfServiceClient(AsfClient asfServiceClient)");
	}

	/**
	 * Gets the asf host nickname from ASF_HOST_NICKNAME table by passing application name.
	 * 
	 * @param applicationName the application name
	 * 
	 * @return the asf host nickname
	 * 
	 * @throws SystemErrorType the system error type fault
	 * @throws BusinessErrorType the business error type fault
	 */
	private String getAsfHostNickname(String applicationName) throws SystemErrorType, BusinessErrorType {
		
		DocgenLogger.debug(ENTRY_LOG,"--> String getAsfHostNickname(String applicationName)");

		final Session session = getSession();
		AsfHostNickname asfHostNickname = null;
		String nickName = null;
		String faultMessage = null;
		String faultName = null;
		String eMailText = null;
		if(msgProperties==null){
			msgProperties=DGSMessageProperties.getInstance().getProperties();
		}
		try{
			asfHostNickname = (AsfHostNickname) session.load(AsfHostNickname.class, applicationName);
			Hibernate.initialize(asfHostNickname);
			nickName = asfHostNickname.getHostNickname();
			
			if(DocgenCommonUtil.isNullOrEmptyString(nickName)){
				
				faultMessage =DocgenCommonUtil.getCompletedMessage(DocgenConstants.BLANK_IN_HOST_NICK_NAME_TABLE, null);
				faultName = getProperty(DocgenConstants.BLANK_IN_HOST_NICK_NAME_TABLE_FAULT_NAME);
				
				DocgenLogger.error(APP_LOG,faultMessage);
				DocgenLogger.debug(EXIT_LOG,"<-- String getAsfHostNickname(String applicationName)");
				throw new BusinessErrorType(faultName , faultMessage ,msgProperties.getProperty(DocgenConstants.BLANK_IN_HOST_NICK_NAME_ERROR_CODE));			
			} 
			
		}catch (ObjectNotFoundException e) {
			
			String[] params = new String[1];
			params[0] = applicationName;
			
			faultMessage = DocgenCommonUtil.getCompletedMessage(DocgenConstants.NO_RECORD_IN_HOST_NICK_NAME_TABLE, params);
			faultName = getProperty(DocgenConstants.NO_RECORD_IN_HOST_NICK_NAME_TABLE_FAULT_NAME);	
			
			DocgenLogger.error(APP_LOG,faultMessage);			
			DocgenLogger.debug(EXIT_LOG,"<-- String getAsfHostNickname(String applicationName)");
			
			throw new BusinessErrorType(faultName , faultMessage ,msgProperties.getProperty(DocgenConstants.NO_RECORD_IN_HOST_NICK_NAME_ERROR_CODE));
			
		}catch (HibernateException e) {
			
			String[] params = new String[2];
			params[0] = e.toString();//e.getCause().getMessage();
			params[1] = applicationName;
			
			faultMessage = DocgenCommonUtil.getCompletedMessage(DocgenConstants.SQL_EXCEPTION_WHILE_QUERYING_HOST_NICK_NAME_TABLE, params);
			eMailText = DocgenCommonUtil.getCompletedMessage(DocgenConstants.SQL_EXCEPTION_WHILE_QUERING_HOST_NICK_NAME_TABLE_EMAIL, params); 
			faultName = getProperty(DocgenConstants.SQL_EXCEPTION_WHILE_QUERYING_HOST_NICK_NAME_TABLE_FAULT_NAME);
				
			handleFatalError(eMailText);	
			
			DocgenLogger.error(APP_LOG,faultMessage);			
			DocgenLogger.debug(EXIT_LOG,"<-- String getAsfHostNickname(String applicationName)");
			
			throw new SystemErrorType(faultName , faultMessage, msgProperties.getProperty(DocgenConstants.SQL_EXCEPTION_WHILE_QUERYING_HOST_NICK_NAME_ERROR_CODE), DocgenConstants.DC_IMPLEMENTATION, e.getMessages());			
		}finally {		
			if (session != null){
			    session.close();
		    }
			DocgenLogger.debug(EXIT_LOG,"<-- String getAsfHostNickname(String applicationName)");
		}			

		return nickName;

	}

	/**
	 * Gets the formatting parameters from DGS_FORMATTING_PARAMETERS table by passinmg predefined asf doc type.
	 * 
	 * @param predefinedASFDoctypes the predefined asf doc type
	 * 
	 * @return the formatting parameters
	 * 
	 * @throws SystemErrorType the system error type fault
	 * @throws BusinessErrorType the business error type fault
	 */
	private DgsFormattingParameters getFormattingParameters(String predefinedASFDoctypes) throws SystemErrorType, BusinessErrorType {
		
		DocgenLogger.debug(ENTRY_LOG,"--> DgsFormattingParameters getFormattingParameters(String predefinedASFDoctypes)");

		final Session session = getSession();
		DgsFormattingParameters formattingParameters = null;
		String faultMessage = null;
		String eMailText = null;
		String faultName = null;
		if(msgProperties==null){
			msgProperties=DGSMessageProperties.getInstance().getProperties();
		}
		try{ 
			formattingParameters = (DgsFormattingParameters) session.load(
					DgsFormattingParameters.class, predefinedASFDoctypes);
			Hibernate.initialize(formattingParameters);
			
			if(DocgenCommonUtil.isNullOrEmptyString(formattingParameters.getDocReq())){
				
				faultMessage = DocgenCommonUtil.getCompletedMessage(DocgenConstants.DOC_REQ_BLANK_IN_FORMATTING_PARAMETERS_TABLE, null);
				faultName = getProperty(DocgenConstants.DOC_REQ_BLANK_IN_FORMATTING_PARAMETERS_TABLE_FAULT_NAME);
				
				DocgenLogger.error(APP_LOG,faultMessage);				
				DocgenLogger.debug(EXIT_LOG,"<-- DgsFormattingParameters getFormattingParameters(String predefinedASFDoctypes)");
				throw new BusinessErrorType(faultName, faultMessage , msgProperties.getProperty(DocgenConstants.DOC_REQ_BLANK_IN_FORMATTING_PARAMETERS_ERROR_CODE));			
			}
			
			if(DocgenCommonUtil.isNullOrEmptyString(formattingParameters.getPreviewFormatter())){
				
				faultMessage = DocgenCommonUtil.getCompletedMessage(DocgenConstants.PREVIEW_FORMATTER_BLANK_IN_FORMATTING_PARAMETERS_TABLE, null);
				faultName = getProperty(DocgenConstants.PREVIEW_FORMATTER_BLANK_IN_FORMATTING_PARAMETERS_TABLE_FAULT_NAME);
				
				DocgenLogger.error(APP_LOG,faultMessage);				
				DocgenLogger.debug(EXIT_LOG,"<-- DgsFormattingParameters getFormattingParameters(String predefinedASFDoctypes)");
				throw new BusinessErrorType(faultName , faultMessage, msgProperties.getProperty(DocgenConstants.PREVIEW_FORMATTER_BLANK_IN_FORMATTING_PARAMETERS_ERROR_CODE));			
			}
			
			if(DocgenCommonUtil.isNullOrEmptyString(formattingParameters.getSessionLanguageCode())){
				
				faultMessage = DocgenCommonUtil.getCompletedMessage(DocgenConstants.SESSION_LANGUAGE_CODE_BLANK_IN_FORMATTING_PARAMETERS_TABLE, null);
				faultName = getProperty(DocgenConstants.SESSION_LANGUAGE_CODE_BLANK_IN_FORMATTING_PARAMETERS_TABLE_FAULT_NAME);
					
				DocgenLogger.error(APP_LOG,faultMessage);				
				DocgenLogger.debug(EXIT_LOG,"<-- DgsFormattingParameters getFormattingParameters(String predefinedASFDoctypes)");
				throw new BusinessErrorType(faultName , faultMessage, msgProperties.getProperty(DocgenConstants.SESSION_LANGUAGE_CODE_BLANK_IN_FORMATTING_PARAMETERS_ERROR_CODE));			
			}
			
		}catch (ObjectNotFoundException e) {
			
			String[] params = new String[1];
			params[0] = predefinedASFDoctypes;
			
			faultMessage = DocgenCommonUtil.getCompletedMessage(DocgenConstants.NO_RECORD_IN_FORMATTING_PARAMETERS_TABLE, params);
			faultName =  getProperty(DocgenConstants.NO_RECORD_IN_FORMATTING_PARAMETERS_TABLE_FAULT_NAME);
			
			DocgenLogger.error(APP_LOG,faultMessage);			
			DocgenLogger.debug(EXIT_LOG,"<-- DgsFormattingParameters getFormattingParameters(String predefinedASFDoctypes)");
			throw new BusinessErrorType(faultName, faultMessage, msgProperties.getProperty(DocgenConstants.NO_RECORD_IN_FORMATTING_PARAMETERS_ERROR_CODE));
			
		}catch (HibernateException e) {
			
			String[] params = new String[2];
			params[0] = e.toString();//e.getCause().getMessage();
			params[1] = predefinedASFDoctypes;			
			
			faultMessage = DocgenCommonUtil.getCompletedMessage(DocgenConstants.SQL_EXCEPTION_WHILE_QUERYING_FORMATTING_PARAMETERS_TABLE, params);
			faultName = getProperty(DocgenConstants.SQL_EXCEPTION_WHILE_QUERYING_FORMATTING_PARAMETERS_TABLE_FAULT_NAME);
			eMailText = DocgenCommonUtil.getCompletedMessage(DocgenConstants.SQL_EXCEPTION_WHILE_QUERYING_FORMATTING_PARAMETERS_TABLE_EMAIL, params);
							
			handleFatalError(eMailText);	
			
			DocgenLogger.error(APP_LOG,faultMessage);			
			DocgenLogger.debug(EXIT_LOG,"<-- DgsFormattingParameters getFormattingParameters(String predefinedASFDoctypes)");
			throw new SystemErrorType(faultName, faultMessage, msgProperties.getProperty(DocgenConstants.SQL_EXCEPTION_WHILE_QUERYING_FORMATTING_PARAMETERS_ERROR_CODE), DocgenConstants.DC_IMPLEMENTATION, null);
		}finally {
			if (session != null){
			    session.close();
		    }
			DocgenLogger.debug(EXIT_LOG,"<-- DgsFormattingParameters getFormattingParameters(String predefinedASFDoctypes)");			
		} 		
		return formattingParameters;

	}

	
	/**
	 * Creates the asf request.
	 * 
	 * This method accepts CreateDocumentWebServiceRequest and creates the AsfRequest. AsfRequest is holder
	 * for asf request webservice parameters. If predefinedAsfDocTypes is client then it uses all parameters
	 * passed by client else it fills host nickname from ASF_HOST_NICKNAME table and formatting parameters 
	 * from DGS_FORMATTING_PARAMETERS table.
	 * 
	 * @param request the request
	 * @param headers 
	 * 
	 * @return the asf request
	 * 
	 * @throws SystemErrorType the system error type fault
	 * @throws BusinessErrorType the business error type fault
	 */
	private AsfRequest createAsfRequest(CreateDocumentWebServiceRequest request, DocgenHeaders headers) throws SystemErrorType, BusinessErrorType {
		
		DocgenLogger.debug(ENTRY_LOG,"--> AsfRequest createAsfrequest(CreateDocumentWebServiceRequest request, DocgenHeaders headers)");

		final AsfRequest asfRequest = new AsfRequest();
		DgsFormattingParameters formattingParameters = null;
		String asfHostNickname = null;	
		
		asfRequest.setApplicationName(request.getApplicationName());
		asfRequest.setDocumentLanguage(request.getDocumentLanguageCode());
		asfRequest.setTemplateName(request.getTemplateName());
		asfRequest.setParams(request.getParams());
		asfRequest.setTransactionalData(createAsfTransactionalData(request.getTransactionalData()));
		asfRequest.setDocgenHeaders(headers);

		if (DocgenConstants.PREDEFINED_ASF_DOCTYPE_CLIENT.equalsIgnoreCase(request.getPredefinedAsfDocTypes())) {
			
			DocgenLogger.debug(APP_LOG,"Document Creation Web Service received client as PredefinedAsfDocTypes");			
			DocgenLogger.debug(APP_LOG,"Document Creation Web Service received following values from client...");
			DocgenLogger.debug(APP_LOG,new StringBuffer("HostNickname => ").append(request.getHostNickname()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("DcfOptions => ").append(request.getDcfOptions()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("DocReq => ").append( request.getDocReq()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("OutputType => " ).append( request.getOutputType()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("PageLayoutName => " ).append( request.getPageLayoutName()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("PreviewFormatter => " ).append( request.getPreviewFormatter()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("SessionLanguageCode => " ).append( request.getSessionLanguageCode()).toString());
						
			asfRequest.setHostNickName(request.getHostNickname());
			asfRequest.setDcfOptions(request.getDcfOptions());
			asfRequest.setDocumentRequest(request.getDocReq());
			asfRequest.setOutputType(request.getOutputType());
			asfRequest.setPageLayoutName(request.getPageLayoutName());
			asfRequest.setPreviewFormatter(request.getPreviewFormatter());
			asfRequest.setSessionLanguage(request.getSessionLanguageCode());
		} else {
			DocgenLogger.debug(APP_LOG,new StringBuffer("Document Creation Web Service received ").append( request.getPredefinedAsfDocTypes() ).append("as PredefinedAsfDocTypes").toString());
			formattingParameters = getFormattingParameters(request.getPredefinedAsfDocTypes());
			asfHostNickname = getAsfHostNickname(request.getApplicationName());		
						
			asfRequest.setHostNickName(DocgenCommonUtil.trimIfNotNull(asfHostNickname));
			asfRequest.setDcfOptions(DocgenCommonUtil.trimIfNotNull(formattingParameters.getDcfOptions()) );
			asfRequest.setDocumentRequest(DocgenCommonUtil.trimIfNotNull(formattingParameters.getDocReq()) );
			asfRequest.setOutputType(formattingParameters.getOutputType());
			asfRequest.setPageLayoutName( DocgenCommonUtil.trimIfNotNull(formattingParameters.getPageLayout()) );
			asfRequest.setPreviewFormatter( DocgenCommonUtil.trimIfNotNull(formattingParameters.getPreviewFormatter()));
			asfRequest.setSessionLanguage(DocgenCommonUtil.trimIfNotNull(formattingParameters.getSessionLanguageCode()));
			
			DocgenLogger.debug(APP_LOG,"Document Creation Web Service populated following values in ASF request... ");
			DocgenLogger.debug(APP_LOG,new StringBuffer("HostNickName => ").append( asfRequest.getHostNickName()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("DcfOptions => ").append( asfRequest.getDcfOptions()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("DocumentRequest => ").append( asfRequest.getDocumentRequest()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("OutputType => ").append( asfRequest.getOutputType()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("PageLayoutName => ").append( asfRequest.getPageLayoutName()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("PreviewFormatter => ").append( asfRequest.getPreviewFormatter()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("SessionLanguage => ").append( asfRequest.getSessionLanguage()).toString());						
		}
		
		DocgenLogger.debug(EXIT_LOG,"<-- AsfRequest createAsfrequest(CreateDocumentWebServiceRequest request, DocgenHeaders headers)");

		return asfRequest;
	}

	/**
	 * Craete asf transactional data.
	 * 
	 * This method accepts transactional data object from DC Web Service request and creates 
	 * transactiona data object for Asf web service.
	 * 
	 * @param docCommandServiceImplementationTransactionalDataValues the transactional data
	 * 
	 * @return the DocCommandServiceImplementationTransactionalDataValue[]
	 */
	private DocCommandServiceImplementationTransactionalDataValue[] createAsfTransactionalData(
			financing.tools.docgen.xml.databind.create.request.DocCommandServiceImplementationTransactionalDataValue[] docCommandServiceImplementationTransactionalDataValues) {
		
		DocgenLogger.debug(ENTRY_LOG,"--> DocCommandServiceImplementationTransactionalDataValue[] craeteAsfTransactionalData(financing.tools.docgen.service.create.service.request.DocCommandServiceImplementationTransactionalDataValue[] transactionalData)");

		if (docCommandServiceImplementationTransactionalDataValues == null){
			
			DocgenLogger.debug(APP_LOG,"Document Creation Web Service null transactiona data");
			
			
			DocgenLogger.debug(EXIT_LOG,"<-- DocCommandServiceImplementationTransactionalDataValue[] craeteAsfTransactionalData(financing.tools.docgen.service.create.service.request.DocCommandServiceImplementationTransactionalDataValue[] transactionalData)");
			return new DocCommandServiceImplementationTransactionalDataValue[0]; // We should return empty array instead of null as per RSA code review comment.
		}	
		
		DocCommandServiceImplementationTransactionalDataValue[] transData = new DocCommandServiceImplementationTransactionalDataValue[docCommandServiceImplementationTransactionalDataValues.length];
		
		DocgenLogger.debug(APP_LOG,new StringBuffer("Document Creation Web Service received ").append( docCommandServiceImplementationTransactionalDataValues.length ).append( " attachments..").toString());

		for (int i = 0; i < docCommandServiceImplementationTransactionalDataValues.length; i++) {
			DocCommandServiceImplementationTransactionalDataValue tValue = new DocCommandServiceImplementationTransactionalDataValue();
			tValue.setDataType(docCommandServiceImplementationTransactionalDataValues[i].getDataType());
			tValue.setMimeType(docCommandServiceImplementationTransactionalDataValues[i].getMimeType());
			tValue.setName(docCommandServiceImplementationTransactionalDataValues[i].getName());
			tValue.setTransactionalData(docCommandServiceImplementationTransactionalDataValues[i]
					.getTransactionalData());
			transData[i] = tValue;
			DocgenLogger.debug(APP_LOG,"Transactional data details......");
			DocgenLogger.debug(APP_LOG,new StringBuffer("DataType = " ).append( tValue.getDataType()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("MimeType = " ).append( tValue.getMimeType()).toString());
			DocgenLogger.debug(APP_LOG,new StringBuffer("Name = ").append( tValue.getName()).toString());
			if(tValue.getTransactionalData() != null){
				DocgenLogger.debug(APP_LOG,"ContentType of TransactionalData" + tValue.getTransactionalData().getContentType());
			}			
			DocgenLogger.debug(APP_LOG,"Transactional data details end here.");			
		}
		
		DocgenLogger.debug(EXIT_LOG,"<-- DocCommandServiceImplementationTransactionalDataValue[] craeteAsfTransactionalData(financing.tools.docgen.service.create.service.request.DocCommandServiceImplementationTransactionalDataValue[] transactionalData)");
		return transData;
	}

}
