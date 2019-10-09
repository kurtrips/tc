//
// This file was generated by the JavaTM Architecture for XML Binding(JAXB) Reference Implementation, vJAXB 2.1.10 in JDK 6 
// See <a href="http://java.sun.com/xml/jaxb">http://java.sun.com/xml/jaxb</a> 
// Any modifications to this file will be lost upon recompilation of the source schema. 
// Generated on: 2010.10.07 at 05:25:00 AM IST 
//


package com.topcoder.alu.mock.mm7.entities;

import java.util.ArrayList;
import java.util.List;
import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlElementRefs;
import javax.xml.bind.annotation.XmlType;


/**
 * At least one of To,CC,Bcc
 * 
 * <p>Java class for recipientsType complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="recipientsType">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence maxOccurs="unbounded">
 *         &lt;choice>
 *           &lt;element name="To" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}multiAddressType"/>
 *           &lt;element name="Cc" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}multiAddressType"/>
 *           &lt;element name="Bcc" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}multiAddressType"/>
 *         &lt;/choice>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "recipientsType", propOrder = {
    "toOrCcOrBcc"
})
public class RecipientsType {

    @XmlElementRefs({
        @XmlElementRef(name = "Bcc", namespace = "http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4", type = JAXBElement.class),
        @XmlElementRef(name = "To", namespace = "http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4", type = JAXBElement.class),
        @XmlElementRef(name = "Cc", namespace = "http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4", type = JAXBElement.class)
    })
    protected List<JAXBElement<MultiAddressType>> toOrCcOrBcc;

    /**
     * Gets the value of the toOrCcOrBcc property.
     * 
     * <p>
     * This accessor method returns a reference to the live list,
     * not a snapshot. Therefore any modification you make to the
     * returned list will be present inside the JAXB object.
     * This is why there is not a <CODE>set</CODE> method for the toOrCcOrBcc property.
     * 
     * <p>
     * For example, to add a new item, do as follows:
     * <pre>
     *    getToOrCcOrBcc().add(newItem);
     * </pre>
     * 
     * 
     * <p>
     * Objects of the following type(s) are allowed in the list
     * {@link JAXBElement }{@code <}{@link MultiAddressType }{@code >}
     * {@link JAXBElement }{@code <}{@link MultiAddressType }{@code >}
     * {@link JAXBElement }{@code <}{@link MultiAddressType }{@code >}
     * 
     * 
     */
    public List<JAXBElement<MultiAddressType>> getToOrCcOrBcc() {
        if (toOrCcOrBcc == null) {
            toOrCcOrBcc = new ArrayList<JAXBElement<MultiAddressType>>();
        }
        return this.toOrCcOrBcc;
    }

}
