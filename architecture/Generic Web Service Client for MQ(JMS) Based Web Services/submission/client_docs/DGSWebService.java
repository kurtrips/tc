/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * DGSWebService.java
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
package financing.tools.docgen.service;

import java.util.Properties;

import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.transaction.HeuristicMixedException;
import javax.transaction.HeuristicRollbackException;
import javax.transaction.NotSupportedException;
import javax.transaction.RollbackException;
import javax.transaction.SystemException;
import javax.transaction.UserTransaction;

import org.apache.log4j.Logger;
import org.springframework.orm.hibernate3.support.HibernateDaoSupport;

import financing.tools.docgen.service.templatemgt.process.CFETransactionProcess;
import financing.tools.docgen.util.DocgenLogger;



/**
 * This class corresponds to DGSBase Service class where all other service classes extends this class
 * @author Rajesh K Behera
 *
 */
public abstract class DGSWebService  extends HibernateDaoSupport{
	
	/** The logger. */
	private static final org.apache.log4j.Logger logger = Logger.getLogger(DGSWebService.class);
	
	private UserTransaction ut =null;
	private Context myCntxt=null;
	/** Log class */
	private static final String CLASS_NAME = DGSWebService.class.getName();
    /** Log class - app log */
	private static final String APP_LOG = new StringBuffer("appLog.").append(CLASS_NAME).toString();
	/** Log class - entry log */
	private static final String ENTRY_LOG = new StringBuffer("entryLog.").append(CLASS_NAME).toString();
	/** Log class - exit log */
	private static final String EXIT_LOG = 	new StringBuffer("exitLog.").append(CLASS_NAME).toString();

	/**
	 *DGSWebService default constructor
	 */
	public DGSWebService(){
		super();
	}
	
	/**
	 * Variable corresponds to a properties file
	 */
	private Properties properties;

	 /**
     * Getter for properties
     * @return java.util.Properties
     */
	public Properties getProperties() {
		return properties;
	}

	/**
	 * Setter for properties
	 * @param properties
	 */
	public void setProperties(Properties properties) {
		this.properties = properties;
	}
	
	/**
	 * Handle fatal error.
	 * 
	 * @param eMailText the e mail text
	 */
	protected void handleFatalError(String eMailText){
		logger.fatal(eMailText);	
	}	
	
	
	/**
	 * Gets the property.
	 * 
	 * Returns the property from the property file with a given property key.
	 * 
	 * @param propertyKey the property key
	 * 
	 * @return the property
	 */
	protected String getProperty(String propertyKey){
		
		logger.debug("--> String getProperty(String propertyKey)");
		
		String propertyValue = null;
		
		propertyValue = getProperties().getProperty(propertyKey);
		
		logger.debug("<-- String getProperty(String propertyKey)");
		
		return propertyValue;	
	}
	
	/**
	 * start transaction
	 */
	public void beginTransaction(){
		DocgenLogger.debug(ENTRY_LOG, "--> beginTransaction() ");
		try {
			myCntxt = new InitialContext();
			ut=(UserTransaction) myCntxt.lookup("java:comp/UserTransaction");
		} catch (NamingException e1) {
			DocgenLogger.error(APP_LOG,  "Naming Exception for InitialContext/UserTransaction ");
			DocgenLogger.fatal(APP_LOG, e1);
			DocgenLogger.debug(EXIT_LOG, "<-- beginTransaction() ");
		}
		try {
			ut.begin();
		} catch (NotSupportedException e) {
			DocgenLogger.error(APP_LOG,  "NotSupported Exception for UserTransaction beign method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- beginTransaction() ");
		} catch (SystemException e) {
			DocgenLogger.error(APP_LOG,  "System Exception for UserTransaction beign method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- beginTransaction() ");
		}
		DocgenLogger.debug(EXIT_LOG, "<-- beginTransaction() ");
	}
	
	/**
	 * end transaction
	 */
	public void commitTransaction(){
		DocgenLogger.debug(ENTRY_LOG, "--> commitTransaction() ");
		try {
			ut.commit();
		} catch (SecurityException e) {
			DocgenLogger.error(APP_LOG,  "Security Exception for UserTransaction commit method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- commitTransaction() ");
		} catch (IllegalStateException e) {
			DocgenLogger.error(APP_LOG,  "Illegal state Exception for UserTransaction commit method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- commitTransaction() ");
		} catch (RollbackException e) {
			DocgenLogger.error(APP_LOG,  "Rollback Exception for UserTransaction commit method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- commitTransaction() ");
		} catch (HeuristicMixedException e) {
			DocgenLogger.error(APP_LOG,  "Heuristic mixed Exception for UserTransaction commit method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- commitTransaction() ");
		} catch (HeuristicRollbackException e) {
			DocgenLogger.error(APP_LOG,  "Heuristic rollback Exception for UserTransaction commit method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- commitTransaction() ");
		} catch (SystemException e) {
			DocgenLogger.error(APP_LOG,  "System Exception for UserTransaction commit method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- commitTransaction() ");
		}
		DocgenLogger.debug(EXIT_LOG, "<-- commitTransaction() ");
	}
	
	/**
	 * cancel transaction
	 */
	public void rollbackTransaction(){
		DocgenLogger.debug(ENTRY_LOG, "--> rollbackTransaction() ");
		try {
			ut.rollback();
		} catch (IllegalStateException e) {
			DocgenLogger.error(APP_LOG,  "Illegal state Exception for UserTransaction commit method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- rollbackTransaction() ");
		} catch (SecurityException e) {
			DocgenLogger.error(APP_LOG,  "Security Exception for UserTransaction rollback method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- rollbackTransaction() ");
		} catch (SystemException e) {
			DocgenLogger.error(APP_LOG,  "System Exception for UserTransaction rollback method ");
			DocgenLogger.fatal(APP_LOG, e);
			DocgenLogger.debug(EXIT_LOG, "<-- rollbackTransaction() ");
		}
		DocgenLogger.debug(EXIT_LOG, "<-- rollbackTransaction() ");
	}
}
