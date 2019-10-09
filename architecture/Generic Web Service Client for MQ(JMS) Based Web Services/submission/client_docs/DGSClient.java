/**
 * IBM Confidential
 *
 * ${financing.tools.docgen.services.DGSClient}
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 */
package financing.tools.docgen.client;

import java.util.Properties;

import org.springframework.beans.factory.ListableBeanFactory;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import financing.tools.docgen.util.DGSProperties;

/**
 * This class corresponds to DGSBase Client class where all other WebService Client classes extends this class
 * @author Rajesh K Behera
 *
 */
public abstract class DGSClient {
	
	/**
	 * This variable stores the path of spring clientcontext.xml file
	 */
	private static String clientContextPath;
	
	static{
		DGSProperties dgsProperties=DGSProperties.getInstance();
		clientContextPath=dgsProperties.getValueFromProperties("properties.spring.clientcontextpath");
	
	}
	
	/**
	 * This variable is initiallized and all beans are loaded into memory
	 */
	protected static final ListableBeanFactory beanFactory =  new ClassPathXmlApplicationContext(clientContextPath);
	
	/**
	 *DGSClient default constructor
	 */
	public DGSClient(){
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
	
	
}
