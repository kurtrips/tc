/*
 * Copyright (C) 2011 TopCoder Inc., All Rights Reserved.
 */
package com.topcoder.sip.test;

import java.io.File;
import junit.framework.TestCase;
import static org.junit.Assert.*;
import com.topcoder.sip.XSLT2HTML;

/**
 * The demo of <code>XSLT2HTML</code> class
 *
 * @author TCSASSEMBLER
 * @version 1.0
 */
public class XSLT2HTMLTest extends TestCase {

    /**
     * Demo showing the generation of the HTML file corresposning to the Tactical.xsd
     * The output html is created in the bin folder and can be viewed in any browser
     *
     * @throws Exception to JUnit
     */
    public void test_demo_tactical() throws Exception {
        XSLT2HTML.generateHtml(
            "test_files/Tactical.xsd", "test_files/template.xsl", "bin/demo_tactical.html");

        assertTrue("Html file must be generated in bin folder",
            (new File("bin/demo_tactical.html")).exists()
        );
    }

    /**
     * Demo showing that the XSLT2HTML can work on any other xsd with the same xsd features used in Tactical.xsd
     * The schema being used is the schema for the deployment descriptor (web.xml) of a Servlet 2.5 application
     * The output html is created in the bin folder and can be viewed in any browser
     *
     * The following xsd features are used:
     * xs:element
     * xs:sequence
     * xs:all
     * xs:attribute
     * xs:complexType
     * xs:simpleType
     * xs:restriction with xs:enumeration
     * xs:union
     *
     * Importantly it also demonstrates, that the XSL silently ignores xsd features it does not recognize
     * (instead of throwing error)
     * @throws Exception to JUnit
     */
    public void test_demo_servlet_web_app() throws Exception {
        XSLT2HTML.generateHtml(
            "test_files/web-app_2_5.xsd", "test_files/template.xsl", "bin/demo_web_app_2_5.html");

        assertTrue("Html file must be generated in bin folder",
            (new File("bin/demo_web_app_2_5.html")).exists()
        );
    }
}
