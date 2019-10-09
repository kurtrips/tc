/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * CreateDocumentWebServiceRequest.java
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 * Module Information:
 *
 * DESCRIPTION:  Document Generation System application.
 * ===========================================================================
 */
/**
 * $Id$
 *===========================================================================
 *
 * IBM Confidential
 *
 * CreateDocumentWebServiceRequest.java
 *
 * (C) Copyright IBM Corp., 2009.
 *
 * The source code for this program is not published or otherwise divested of
 * its trade secrets, irrespective of what has been deposited with the U.S.
 * Copyright office.
 * ===========================================================================
 * Module Information:
 *
 * DESCRIPTION:  Document Generation System application.
 * ===========================================================================
 */
/**
 * CreateDocumentWebServiceRequest.java
 *
 * This file was auto-generated from WSDL
 * by the IBM Web services WSDL2Java emitter.
 * cf190834.07 v9308141138
 */

package financing.tools.docgen.xml.databind.create.request;

public class CreateDocumentWebServiceRequest  {
    private java.lang.String templateName;
    private java.lang.String applicationName;
    private java.lang.String documentLanguageCode;
    private java.lang.String[][] params;
    private financing.tools.docgen.xml.databind.create.request.DocCommandServiceImplementationTransactionalDataValue[] transactionalData;
    private java.lang.String predefinedAsfDocTypes;
    private boolean EMailIndicator;
    private financing.tools.docgen.xml.databind.types.common.EMail EMail;
    private java.lang.String hostNickname;
    private java.lang.String previewFormatter;
    private java.lang.String docReq;
    private java.lang.String sessionLanguageCode;
    private java.lang.String pageLayoutName;
    private java.lang.String dcfOptions;
    private java.lang.Integer outputType;

    public CreateDocumentWebServiceRequest() {
    }

    public java.lang.String getTemplateName() {
        return templateName;
    }

    public void setTemplateName(java.lang.String templateName) {
        this.templateName = templateName;
    }

    public java.lang.String getApplicationName() {
        return applicationName;
    }

    public void setApplicationName(java.lang.String applicationName) {
        this.applicationName = applicationName;
    }

    public java.lang.String getDocumentLanguageCode() {
        return documentLanguageCode;
    }

    public void setDocumentLanguageCode(java.lang.String documentLanguageCode) {
        this.documentLanguageCode = documentLanguageCode;
    }

    public java.lang.String[][] getParams() {
        return params;
    }

    public void setParams(java.lang.String[][] params) {
        this.params = params;
    }

    public financing.tools.docgen.xml.databind.create.request.DocCommandServiceImplementationTransactionalDataValue[] getTransactionalData() {
        return transactionalData;
    }

    public void setTransactionalData(financing.tools.docgen.xml.databind.create.request.DocCommandServiceImplementationTransactionalDataValue[] transactionalData) {
        this.transactionalData = transactionalData;
    }

    public java.lang.String getPredefinedAsfDocTypes() {
        return predefinedAsfDocTypes;
    }

    public void setPredefinedAsfDocTypes(java.lang.String predefinedAsfDocTypes) {
        this.predefinedAsfDocTypes = predefinedAsfDocTypes;
    }

    public boolean isEMailIndicator() {
        return EMailIndicator;
    }

    public void setEMailIndicator(boolean EMailIndicator) {
        this.EMailIndicator = EMailIndicator;
    }

    public financing.tools.docgen.xml.databind.types.common.EMail getEMail() {
        return EMail;
    }

    public void setEMail(financing.tools.docgen.xml.databind.types.common.EMail EMail) {
        this.EMail = EMail;
    }

    public java.lang.String getHostNickname() {
        return hostNickname;
    }

    public void setHostNickname(java.lang.String hostNickname) {
        this.hostNickname = hostNickname;
    }

    public java.lang.String getPreviewFormatter() {
        return previewFormatter;
    }

    public void setPreviewFormatter(java.lang.String previewFormatter) {
        this.previewFormatter = previewFormatter;
    }

    public java.lang.String getDocReq() {
        return docReq;
    }

    public void setDocReq(java.lang.String docReq) {
        this.docReq = docReq;
    }

    public java.lang.String getSessionLanguageCode() {
        return sessionLanguageCode;
    }

    public void setSessionLanguageCode(java.lang.String sessionLanguageCode) {
        this.sessionLanguageCode = sessionLanguageCode;
    }

    public java.lang.String getPageLayoutName() {
        return pageLayoutName;
    }

    public void setPageLayoutName(java.lang.String pageLayoutName) {
        this.pageLayoutName = pageLayoutName;
    }

    public java.lang.String getDcfOptions() {
        return dcfOptions;
    }

    public void setDcfOptions(java.lang.String dcfOptions) {
        this.dcfOptions = dcfOptions;
    }

    public java.lang.Integer getOutputType() {
        return outputType;
    }

    public void setOutputType(java.lang.Integer outputType) {
        this.outputType = outputType;
    }

}
