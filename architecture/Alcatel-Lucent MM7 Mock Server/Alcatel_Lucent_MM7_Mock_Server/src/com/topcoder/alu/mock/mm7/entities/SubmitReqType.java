//
// This file was generated by the JavaTM Architecture for XML Binding(JAXB) Reference Implementation, vJAXB 2.1.10 in JDK 6 
// See <a href="http://java.sun.com/xml/jaxb">http://java.sun.com/xml/jaxb</a> 
// Any modifications to this file will be lost upon recompilation of the source schema. 
// Generated on: 2010.10.07 at 05:25:00 AM IST 
//


package com.topcoder.alu.mock.mm7.entities;

import java.math.BigInteger;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlAttribute;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlSchemaType;
import javax.xml.bind.annotation.XmlType;
import javax.xml.datatype.XMLGregorianCalendar;


/**
 * <p>Java class for submitReqType complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="submitReqType">
 *   &lt;complexContent>
 *     &lt;extension base="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}genericVASPRequestType">
 *       &lt;sequence>
 *         &lt;element name="Recipients" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}recipientsType"/>
 *         &lt;element name="ServiceCode" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}serviceCodeType" minOccurs="0"/>
 *         &lt;element name="LinkedID" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}messageIDType" minOccurs="0"/>
 *         &lt;element name="MessageClass" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}messageClassType" minOccurs="0"/>
 *         &lt;element name="TimeStamp" type="{http://www.w3.org/2001/XMLSchema}dateTime" minOccurs="0"/>
 *         &lt;element name="ReplyCharging" minOccurs="0">
 *           &lt;complexType>
 *             &lt;complexContent>
 *               &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *                 &lt;attribute name="replyChargingSize" type="{http://www.w3.org/2001/XMLSchema}positiveInteger" />
 *                 &lt;attribute name="replyDeadline" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}relativeOrAbsoluteDateType" />
 *               &lt;/restriction>
 *             &lt;/complexContent>
 *           &lt;/complexType>
 *         &lt;/element>
 *         &lt;element name="EarliestDeliveryTime" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}relativeOrAbsoluteDateType" minOccurs="0"/>
 *         &lt;element name="ExpiryDate" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}relativeOrAbsoluteDateType" minOccurs="0"/>
 *         &lt;element name="DeliveryReport" type="{http://www.w3.org/2001/XMLSchema}boolean" minOccurs="0"/>
 *         &lt;element name="ReadReply" type="{http://www.w3.org/2001/XMLSchema}boolean" minOccurs="0"/>
 *         &lt;element name="Priority" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}priorityType" minOccurs="0"/>
 *         &lt;element name="Subject" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         &lt;element name="ChargedParty" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}chargedPartyType" minOccurs="0"/>
 *         &lt;element name="ChargedPartyID" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}chargedPartyIDType" minOccurs="0"/>
 *         &lt;element name="DistributionIndicator" type="{http://www.w3.org/2001/XMLSchema}boolean" minOccurs="0"/>
 *         &lt;element name="DeliveryCondition" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}deliveryConditionType" minOccurs="0"/>
 *         &lt;element name="ApplicID" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         &lt;element name="ReplyApplicID" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         &lt;element name="AuxApplicInfo" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         &lt;element name="ContentClass" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}contentClassType" minOccurs="0"/>
 *         &lt;element name="DRMContent" type="{http://www.w3.org/2001/XMLSchema}boolean" minOccurs="0"/>
 *         &lt;element name="Content" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}contentReferenceType" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/extension>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "submitReqType", propOrder = {
    "recipients",
    "serviceCode",
    "linkedID",
    "messageClass",
    "timeStamp",
    "replyCharging",
    "earliestDeliveryTime",
    "expiryDate",
    "deliveryReport",
    "readReply",
    "priority",
    "subject",
    "chargedParty",
    "chargedPartyID",
    "distributionIndicator",
    "deliveryCondition",
    "applicID",
    "replyApplicID",
    "auxApplicInfo",
    "contentClass",
    "drmContent",
    "content"
})
public class SubmitReqType
    extends GenericVASPRequestType
{

    @XmlElement(name = "Recipients", required = true)
    protected RecipientsType recipients;
    @XmlElement(name = "ServiceCode")
    protected ServiceCodeType serviceCode;
    @XmlElement(name = "LinkedID")
    protected String linkedID;
    @XmlElement(name = "MessageClass", defaultValue = "Informational")
    protected MessageClassType messageClass;
    @XmlElement(name = "TimeStamp")
    @XmlSchemaType(name = "dateTime")
    protected XMLGregorianCalendar timeStamp;
    @XmlElement(name = "ReplyCharging")
    protected SubmitReqType.ReplyCharging replyCharging;
    @XmlElement(name = "EarliestDeliveryTime")
    protected String earliestDeliveryTime;
    @XmlElement(name = "ExpiryDate")
    protected String expiryDate;
    @XmlElement(name = "DeliveryReport")
    protected Boolean deliveryReport;
    @XmlElement(name = "ReadReply")
    protected Boolean readReply;
    @XmlElement(name = "Priority")
    protected PriorityType priority;
    @XmlElement(name = "Subject")
    protected String subject;
    @XmlElement(name = "ChargedParty")
    protected ChargedPartyType chargedParty;
    @XmlElement(name = "ChargedPartyID")
    protected String chargedPartyID;
    @XmlElement(name = "DistributionIndicator")
    protected Boolean distributionIndicator;
    @XmlElement(name = "DeliveryCondition")
    protected DeliveryConditionType deliveryCondition;
    @XmlElement(name = "ApplicID")
    protected String applicID;
    @XmlElement(name = "ReplyApplicID")
    protected String replyApplicID;
    @XmlElement(name = "AuxApplicInfo")
    protected String auxApplicInfo;
    @XmlElement(name = "ContentClass")
    protected ContentClassType contentClass;
    @XmlElement(name = "DRMContent")
    protected Boolean drmContent;
    @XmlElement(name = "Content")
    protected ContentReferenceType content;

    /**
     * Gets the value of the recipients property.
     * 
     * @return
     *     possible object is
     *     {@link RecipientsType }
     *     
     */
    public RecipientsType getRecipients() {
        return recipients;
    }

    /**
     * Sets the value of the recipients property.
     * 
     * @param value
     *     allowed object is
     *     {@link RecipientsType }
     *     
     */
    public void setRecipients(RecipientsType value) {
        this.recipients = value;
    }

    /**
     * Gets the value of the serviceCode property.
     * 
     * @return
     *     possible object is
     *     {@link ServiceCodeType }
     *     
     */
    public ServiceCodeType getServiceCode() {
        return serviceCode;
    }

    /**
     * Sets the value of the serviceCode property.
     * 
     * @param value
     *     allowed object is
     *     {@link ServiceCodeType }
     *     
     */
    public void setServiceCode(ServiceCodeType value) {
        this.serviceCode = value;
    }

    /**
     * Gets the value of the linkedID property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getLinkedID() {
        return linkedID;
    }

    /**
     * Sets the value of the linkedID property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setLinkedID(String value) {
        this.linkedID = value;
    }

    /**
     * Gets the value of the messageClass property.
     * 
     * @return
     *     possible object is
     *     {@link MessageClassType }
     *     
     */
    public MessageClassType getMessageClass() {
        return messageClass;
    }

    /**
     * Sets the value of the messageClass property.
     * 
     * @param value
     *     allowed object is
     *     {@link MessageClassType }
     *     
     */
    public void setMessageClass(MessageClassType value) {
        this.messageClass = value;
    }

    /**
     * Gets the value of the timeStamp property.
     * 
     * @return
     *     possible object is
     *     {@link XMLGregorianCalendar }
     *     
     */
    public XMLGregorianCalendar getTimeStamp() {
        return timeStamp;
    }

    /**
     * Sets the value of the timeStamp property.
     * 
     * @param value
     *     allowed object is
     *     {@link XMLGregorianCalendar }
     *     
     */
    public void setTimeStamp(XMLGregorianCalendar value) {
        this.timeStamp = value;
    }

    /**
     * Gets the value of the replyCharging property.
     * 
     * @return
     *     possible object is
     *     {@link SubmitReqType.ReplyCharging }
     *     
     */
    public SubmitReqType.ReplyCharging getReplyCharging() {
        return replyCharging;
    }

    /**
     * Sets the value of the replyCharging property.
     * 
     * @param value
     *     allowed object is
     *     {@link SubmitReqType.ReplyCharging }
     *     
     */
    public void setReplyCharging(SubmitReqType.ReplyCharging value) {
        this.replyCharging = value;
    }

    /**
     * Gets the value of the earliestDeliveryTime property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getEarliestDeliveryTime() {
        return earliestDeliveryTime;
    }

    /**
     * Sets the value of the earliestDeliveryTime property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setEarliestDeliveryTime(String value) {
        this.earliestDeliveryTime = value;
    }

    /**
     * Gets the value of the expiryDate property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getExpiryDate() {
        return expiryDate;
    }

    /**
     * Sets the value of the expiryDate property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setExpiryDate(String value) {
        this.expiryDate = value;
    }

    /**
     * Gets the value of the deliveryReport property.
     * 
     * @return
     *     possible object is
     *     {@link Boolean }
     *     
     */
    public Boolean isDeliveryReport() {
        return deliveryReport;
    }

    /**
     * Sets the value of the deliveryReport property.
     * 
     * @param value
     *     allowed object is
     *     {@link Boolean }
     *     
     */
    public void setDeliveryReport(Boolean value) {
        this.deliveryReport = value;
    }

    /**
     * Gets the value of the readReply property.
     * 
     * @return
     *     possible object is
     *     {@link Boolean }
     *     
     */
    public Boolean isReadReply() {
        return readReply;
    }

    /**
     * Sets the value of the readReply property.
     * 
     * @param value
     *     allowed object is
     *     {@link Boolean }
     *     
     */
    public void setReadReply(Boolean value) {
        this.readReply = value;
    }

    /**
     * Gets the value of the priority property.
     * 
     * @return
     *     possible object is
     *     {@link PriorityType }
     *     
     */
    public PriorityType getPriority() {
        return priority;
    }

    /**
     * Sets the value of the priority property.
     * 
     * @param value
     *     allowed object is
     *     {@link PriorityType }
     *     
     */
    public void setPriority(PriorityType value) {
        this.priority = value;
    }

    /**
     * Gets the value of the subject property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getSubject() {
        return subject;
    }

    /**
     * Sets the value of the subject property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setSubject(String value) {
        this.subject = value;
    }

    /**
     * Gets the value of the chargedParty property.
     * 
     * @return
     *     possible object is
     *     {@link ChargedPartyType }
     *     
     */
    public ChargedPartyType getChargedParty() {
        return chargedParty;
    }

    /**
     * Sets the value of the chargedParty property.
     * 
     * @param value
     *     allowed object is
     *     {@link ChargedPartyType }
     *     
     */
    public void setChargedParty(ChargedPartyType value) {
        this.chargedParty = value;
    }

    /**
     * Gets the value of the chargedPartyID property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getChargedPartyID() {
        return chargedPartyID;
    }

    /**
     * Sets the value of the chargedPartyID property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setChargedPartyID(String value) {
        this.chargedPartyID = value;
    }

    /**
     * Gets the value of the distributionIndicator property.
     * 
     * @return
     *     possible object is
     *     {@link Boolean }
     *     
     */
    public Boolean isDistributionIndicator() {
        return distributionIndicator;
    }

    /**
     * Sets the value of the distributionIndicator property.
     * 
     * @param value
     *     allowed object is
     *     {@link Boolean }
     *     
     */
    public void setDistributionIndicator(Boolean value) {
        this.distributionIndicator = value;
    }

    /**
     * Gets the value of the deliveryCondition property.
     * 
     * @return
     *     possible object is
     *     {@link DeliveryConditionType }
     *     
     */
    public DeliveryConditionType getDeliveryCondition() {
        return deliveryCondition;
    }

    /**
     * Sets the value of the deliveryCondition property.
     * 
     * @param value
     *     allowed object is
     *     {@link DeliveryConditionType }
     *     
     */
    public void setDeliveryCondition(DeliveryConditionType value) {
        this.deliveryCondition = value;
    }

    /**
     * Gets the value of the applicID property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getApplicID() {
        return applicID;
    }

    /**
     * Sets the value of the applicID property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setApplicID(String value) {
        this.applicID = value;
    }

    /**
     * Gets the value of the replyApplicID property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getReplyApplicID() {
        return replyApplicID;
    }

    /**
     * Sets the value of the replyApplicID property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setReplyApplicID(String value) {
        this.replyApplicID = value;
    }

    /**
     * Gets the value of the auxApplicInfo property.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getAuxApplicInfo() {
        return auxApplicInfo;
    }

    /**
     * Sets the value of the auxApplicInfo property.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setAuxApplicInfo(String value) {
        this.auxApplicInfo = value;
    }

    /**
     * Gets the value of the contentClass property.
     * 
     * @return
     *     possible object is
     *     {@link ContentClassType }
     *     
     */
    public ContentClassType getContentClass() {
        return contentClass;
    }

    /**
     * Sets the value of the contentClass property.
     * 
     * @param value
     *     allowed object is
     *     {@link ContentClassType }
     *     
     */
    public void setContentClass(ContentClassType value) {
        this.contentClass = value;
    }

    /**
     * Gets the value of the drmContent property.
     * 
     * @return
     *     possible object is
     *     {@link Boolean }
     *     
     */
    public Boolean isDRMContent() {
        return drmContent;
    }

    /**
     * Sets the value of the drmContent property.
     * 
     * @param value
     *     allowed object is
     *     {@link Boolean }
     *     
     */
    public void setDRMContent(Boolean value) {
        this.drmContent = value;
    }

    /**
     * Gets the value of the content property.
     * 
     * @return
     *     possible object is
     *     {@link ContentReferenceType }
     *     
     */
    public ContentReferenceType getContent() {
        return content;
    }

    /**
     * Sets the value of the content property.
     * 
     * @param value
     *     allowed object is
     *     {@link ContentReferenceType }
     *     
     */
    public void setContent(ContentReferenceType value) {
        this.content = value;
    }


    /**
     * <p>Java class for anonymous complex type.
     * 
     * <p>The following schema fragment specifies the expected content contained within this class.
     * 
     * <pre>
     * &lt;complexType>
     *   &lt;complexContent>
     *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
     *       &lt;attribute name="replyChargingSize" type="{http://www.w3.org/2001/XMLSchema}positiveInteger" />
     *       &lt;attribute name="replyDeadline" type="{http://www.3gpp.org/ftp/Specs/archive/23_series/23.140/schema/REL-6-MM7-1-4}relativeOrAbsoluteDateType" />
     *     &lt;/restriction>
     *   &lt;/complexContent>
     * &lt;/complexType>
     * </pre>
     * 
     * 
     */
    @XmlAccessorType(XmlAccessType.FIELD)
    @XmlType(name = "")
    public static class ReplyCharging {

        @XmlAttribute
        @XmlSchemaType(name = "positiveInteger")
        protected BigInteger replyChargingSize;
        @XmlAttribute
        protected String replyDeadline;

        /**
         * Gets the value of the replyChargingSize property.
         * 
         * @return
         *     possible object is
         *     {@link BigInteger }
         *     
         */
        public BigInteger getReplyChargingSize() {
            return replyChargingSize;
        }

        /**
         * Sets the value of the replyChargingSize property.
         * 
         * @param value
         *     allowed object is
         *     {@link BigInteger }
         *     
         */
        public void setReplyChargingSize(BigInteger value) {
            this.replyChargingSize = value;
        }

        /**
         * Gets the value of the replyDeadline property.
         * 
         * @return
         *     possible object is
         *     {@link String }
         *     
         */
        public String getReplyDeadline() {
            return replyDeadline;
        }

        /**
         * Sets the value of the replyDeadline property.
         * 
         * @param value
         *     allowed object is
         *     {@link String }
         *     
         */
        public void setReplyDeadline(String value) {
            this.replyDeadline = value;
        }

    }

}