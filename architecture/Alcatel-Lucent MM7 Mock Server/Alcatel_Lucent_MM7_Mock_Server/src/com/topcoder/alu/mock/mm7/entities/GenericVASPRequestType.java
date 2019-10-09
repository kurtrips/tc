//
// This file was generated by the JavaTM Architecture for XML Binding(JAXB) Reference Implementation, vJAXB 2.1.10 in JDK 6 
// See <a href="http://java.sun.com/xml/jaxb">http://java.sun.com/xml/jaxb</a> 
// Any modifications to this file will be lost upon recompilation of the source schema. 
// Generated on: 2010.10.07 at 05:25:00 AM IST 
//


package com.topcoder.alu.mock.mm7.entities;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlSeeAlso;
import javax.xml.bind.annotation.XmlType;


/**
 * Base type for all requests from VASP to R/S
 * 
 * <p>Java class for genericVASPRequestType complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="genericVASPRequestType">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="MM7Version" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}versionType"/>
 *         &lt;element name="SenderIdentification" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}senderIDType"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "genericVASPRequestType", propOrder = {
    "mm7Version",
    "senderIdentification"
})
@XmlSeeAlso({
    SubmitReqType.class,
    ReplaceReqType.class,
    ExtendedcancelReqType.class,
    CancelReqType.class
})
public class GenericVASPRequestType {

    @XmlElement(name = "MM7Version", required = true)
    protected String mm7Version;
    @XmlElement(name = "SenderIdentification", required = true)
    protected SenderIDType senderIdentification;

    /**
     * Gets the value of the mm7Version property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getMM7Version() {
        return mm7Version;
    }

    /**
     * Sets the value of the mm7Version property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setMM7Version(String value) {
        this.mm7Version = value;
    }

    /**
     * Gets the value of the senderIdentification property.
     * 
     * @return
     *     possible object is
     *     {@link SenderIDType }
     *     
     */
    public SenderIDType getSenderIdentification() {
        return senderIdentification;
    }

    /**
     * Sets the value of the senderIdentification property.
     * 
     * @param value
     *     allowed object is
     *     {@link SenderIDType }
     *     
     */
    public void setSenderIdentification(SenderIDType value) {
        this.senderIdentification = value;
    }

}
