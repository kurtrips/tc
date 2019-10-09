/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import com.topcoder.alu.mock.mm7.entities.DeliveryReportReqType;
import com.topcoder.alu.mock.mm7.entities.TransactionID;

/**
 * Simple wrapper class over a TransactionID and DeliveryReportReqType.
 * @author TCSDEVELOPER
 * @version 1.0
 * @threadsafety This class is not thread safe.
 */
public class DeliveryReportRequest {
	
	/**
	 * The TransactionID instance
	 */
	private TransactionID header;
	
	/**
	 * The DeliveryReportReqType instance
	 */
	private DeliveryReportReqType request;
	
	/**
	 * Sets the header using the TransactionID instance  
	 * @param header the header to set
	 */
	public void setHeader(TransactionID header) {
		this.header = header;
	}
	
	/**
	 * Gets the header
	 * @return the header
	 */
	public TransactionID getHeader() {
		return header;
	}
	
	/**
	 * Sets the header using the DeliveryReportReqType instance
	 * @param request the request to set
	 */
	public void setRequest(DeliveryReportReqType request) {
		this.request = request;
	}
	
	/**
	 * Gets the Request
	 * @return the request
	 */
	public DeliveryReportReqType getRequest() {
		return request;
	}

}
