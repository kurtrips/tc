/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import com.topcoder.alu.mock.mm7.entities.SubmitReqType;
import com.topcoder.alu.mock.mm7.entities.TransactionID;

/**
 * Wrapper around the TransactionID and SubmitReqType types
 * @author TCSDEVELOPER
 * @version 1.0
 * 
 */
public class SubmitRequest {
	/**
	 * The TransactionID
	 */
	private TransactionID header;
	/**
	 * The SubmitReqType
	 */
	private SubmitReqType request;
	/**
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
	 * @param request the request to set
	 */
	public void setRequest(SubmitReqType request) {
		this.request = request;
	}
	/**
	 * @return the request
	 */
	public SubmitReqType getRequest() {
		return request;
	}
}
