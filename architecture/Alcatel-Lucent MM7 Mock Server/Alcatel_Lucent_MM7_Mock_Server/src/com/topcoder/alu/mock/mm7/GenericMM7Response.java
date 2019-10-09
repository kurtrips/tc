/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import com.topcoder.alu.mock.mm7.entities.GenericResponseType;
import com.topcoder.alu.mock.mm7.entities.TransactionID;

/**
 * Simple wrapper class over a TransactionID and GenericResponseType.
 * @author TCSDEVELOPER
 * @version 1.0
 * @threadsafety This class is not thread safe. 
 * 
 */
public class GenericMM7Response {
	private TransactionID header;
	private GenericResponseType response;
	
	/**
	 * Sets the transactionId header 
	 * @param header the header to set
	 */
	public void setHeader(TransactionID header) {
		this.header = header;
	}
	
	/**
	 * @return the header
	 */
	public TransactionID getHeader() {
		return header;
	}
	
	/**
	 * Sets the GenericResponseType response
	 * @param response the response to set
	 */
	public void setResponse(GenericResponseType response) {
		this.response = response;
	}
	/**
	 * @return the response
	 */
	public GenericResponseType getResponse() {
		return response;
	}
}
