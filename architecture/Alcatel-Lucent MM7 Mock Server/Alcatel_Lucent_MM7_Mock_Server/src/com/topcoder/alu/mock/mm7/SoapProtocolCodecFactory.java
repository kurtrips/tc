/**
 * 
 */
package com.topcoder.alu.mock.mm7;

import org.apache.mina.filter.codec.demux.DemuxingProtocolCodecFactory;

import com.topcoder.alu.mock.mm7.entities.DeliverReqType;
import com.topcoder.alu.mock.mm7.entities.Fault;

/**
 * This class is the Codec Factory for the server. 
 * It keeps a bunch of decoders and encoders for handling the incoming and outgoing messages (respectively)
 * @author TCSDEVELOPER
 * @version 1.0
 * 
 */
public class SoapProtocolCodecFactory extends DemuxingProtocolCodecFactory {
	/**
	 * Constructor which initailizes the decoders and encoders used by the codec factory
	 */
	public SoapProtocolCodecFactory() {
		super.addMessageDecoder(SoapRequestDecoder.class);
		super.addMessageEncoder(GenericMM7Response.class, GenericMM7ResponseEncoder.class);
		super.addMessageEncoder(DeliveryReportRequest.class, DeliveryReportRequestEncoder.class);
		super.addMessageEncoder(DeliverReqType.class, DeliverRequestEncoder.class);
		super.addMessageEncoder(Fault.class, FaultEncoder.class);
	}
}
