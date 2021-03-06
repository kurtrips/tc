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
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for senderIDType complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="senderIDType">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="VASPID" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}entityIDType" minOccurs="0"/>
 *         &lt;element name="VASID" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}entityIDType" minOccurs="0"/>
 *         &lt;element name="SenderAddress" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}addressType" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "senderIDType", propOrder = {
    "vaspid",
    "vasid",
    "senderAddress"
})
public class SenderIDType {

    @XmlElement(name = "VASPID")
    protected String vaspid;
    @XmlElement(name = "VASID")
    protected String vasid;
    @XmlElement(name = "SenderAddress")
    protected AddressType senderAddress;

    /**
     * Gets the value of the vaspid property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getVASPID() {
        return vaspid;
    }

    /**
     * Sets the value of the vaspid property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setVASPID(String value) {
        this.vaspid = value;
    }

    /**
     * Gets the value of the vasid property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getVASID() {
        return vasid;
    }

    /**
     * Sets the value of the vasid property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setVASID(String value) {
        this.vasid = value;
    }

    /**
     * Gets the value of the senderAddress property.
     * 
     * @return
     *     possible object is
     *     {@link AddressType }
     *     
     */
    public AddressType getSenderAddress() {
        return senderAddress;
    }

    /**
     * Sets the value of the senderAddress property.
     * 
     * @param value
     *     allowed object is
     *     {@link AddressType }
     *     
     */
    public void setSenderAddress(AddressType value) {
        this.senderAddress = value;
    }

}
