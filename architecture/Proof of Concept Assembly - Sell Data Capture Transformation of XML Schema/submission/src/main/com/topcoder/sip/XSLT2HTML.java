/*
 * Copyright (C) 2011 TopCoder Inc., All Rights Reserved.
 */
package com.topcoder.sip;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;

/**
 * This class is used to generate an HTML file which displays the contents of an XSD file based on an XSL template.
 * Sample Usage:
 * <code>XSLT2HTML.generateHtml("some_xsd.xsd", "template.xsl", "some_output.html");</code>
 *
 * <p>Thread safe: This class is thread safe because it has no state</p>
 *
 * @author TCSASSEMBLER
 * @version 1.0
*/
public class XSLT2HTML
{
    /**
     * Generates an html file describing an xsd file using an xsl file as template.
     *
     * @param xsdFileName The path (inclusive of file name) of the xsd file to be described
     * @param templateFileName The path (inclusive of file name) of the xsl template used to generate the output html
     * @param outputFileName The path (inclusive of file name) where the html file is to be generated
     *
     * @throws IllegalArgumentException if any parameter is null or empty
     * @throws FileNotFoundException If either the xsd file or the xsl template file is not found
     * @throws TransformerException If an unrecoverable error occurs during the course of the transformation.
     * @throws TransformerConfigurationException May throw this during the parse when it is
     * @throws IOException Thrown if the underlying stream to output file fails to close
     * constructing the Templates object and fails.
     */
    public static void generateHtml(String xsdFileName, String templateFileName, String outputFileName)
    throws TransformerException, TransformerConfigurationException, FileNotFoundException, IOException {
        if (xsdFileName == null || xsdFileName.trim().isEmpty()) {
            throw new IllegalArgumentException("xsdFileName should not be null or empty");
        }
        if (templateFileName == null || templateFileName.trim().isEmpty()) {
            throw new IllegalArgumentException("templateFileName should not be null or empty");
        }
        if (outputFileName == null || outputFileName.trim().isEmpty()) {
            throw new IllegalArgumentException("outputFileName should not be null or empty");
        }

        FileOutputStream fos = null;
        try {
            // Use the static TransformerFactory.newInstance() method to instantiate
            // a TransformerFactory. The javax.xml.transform.TransformerFactory
            // system property setting determines the actual class to instantiate --
            // org.apache.xalan.transformer.TransformerImpl.
            TransformerFactory tFactory = TransformerFactory.newInstance();

            // Use the TransformerFactory to instantiate a Transformer that will work with
            // the stylesheet you specify. This method call also processes the stylesheet
            // into a compiled Templates object.
            Transformer transformer = tFactory.newTransformer(new StreamSource(templateFileName));

            // Use the Transformer to apply the associated Templates object to an XSD document
            // and write the output to an HTML file
            fos = new FileOutputStream(outputFileName);
            transformer.transform(new StreamSource(xsdFileName),
                new StreamResult(fos));
        } finally {
            //Clean up
            if (fos != null) {
                fos.close();
            }
        }
    }
}
