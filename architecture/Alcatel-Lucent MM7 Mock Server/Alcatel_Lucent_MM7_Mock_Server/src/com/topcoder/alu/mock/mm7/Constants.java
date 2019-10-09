/**
 * 
 */
package com.topcoder.alu.mock.mm7;

/**
 * @author TCSDEVELOPER
 * This class stores the various constants used by the project.
 * 
 */
class Constants {
	/**
	 * The MM7 namespace
	 */
	public static final String MM7_NS = 
		"http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4";
	
	/**
	 * The Soap Env namespace
	 */
	public static final String SOAP_ENV_NS = 
		"http://schemas.xmlsoap.org/soap/envelope/";
	
	/**
	 * An HTTP success response's header
	 */
	public static final String HTTP_RESP_HEADER = 
		"HTTP/1.1  200 OK\r\nContent-Type: text/xml; charset=\"utf-8\"\r\nContent-Length: nnnn\r\n\r\n";
	
	/**
	 * The content-type of an MM1 submit request
	 */
	public static final String MM1_CONTENT_TYPE = 
		"application/vnd.wap.mms-message";

	/**
	 * The version of the MMS being used in this server.
	 */
	public static final String MMS_VERSION = "5.6.0";

	/**
	 * A mock server address for this mock server
	 */
	public static final String MMS_SERVER_ADDRESS = "mms.omms.com";

	/**
	 * A convenience name for the MM1 Submit request 
	 */
	public static final Object MM1_SUBMIT_REQ = "MM1_submit.REQ";

	/**
	 * The port for the VASP server in the scenario where the client is MMUA and server is the mock server
	 * In this scenario, the VASP is the third server and hence the need for the port. 
	 */
	public static final int VASP_PORT = 6669;

	/**
	 * An HTTP server error response's header
	 */
	public static final String HTTP_RESP_ERROR_HEADER = 
		"HTTP/1.1 500 Internal Server Error\r\nContent-Type: text/xml; charset=\"utf-8\"\r\nContent-Length: nnnn\r\n\r\n";

	/**
	 * The XSI schema namespace
	 */
	public static final String XSI = "http://www.w3.org/2001/XMLSchema-instance";
}
