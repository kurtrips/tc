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
 * Extended Cancel Response
 * 
 * <p>Java class for extendedcancelRspType complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="extendedcancelRspType">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="MM7Version" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}versionType"/>
 *         &lt;element name="Status" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}extendedcancelresponseStatusType"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "extendedcancelRspType", propOrder = {
    "mm7Version",
    "status"
})
public class ExtendedcancelRspType {

    @XmlElement(name = "MM7Version", required = true)
    protected String mm7Version;
    @XmlElement(name = "Status", required = true)
    protected ExtendedcancelresponseStatusType status;

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
     * Gets the value of the status property.
     * 
     * @return
     *     possible object is
     *     {@link ExtendedcancelresponseStatusType }
     *     
     */
    public ExtendedcancelresponseStatusType getStatus() {
        return status;
    }

    /**
     * Sets the value of the status property.
     * 
     * @param value
     *     allowed object is
     *     {@link ExtendedcancelresponseStatusType }
     *     
     */
    public void setStatus(ExtendedcancelresponseStatusType value) {
        this.status = value;
    }

}
