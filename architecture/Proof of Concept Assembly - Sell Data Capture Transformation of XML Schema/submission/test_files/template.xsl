<?xml version="1.0" encoding="ISO-8859-1"?>
<!--
Copyright (C) 2011 TopCoder Inc., All Rights Reserved.
-->
<!--
This stylesheet is used for converting an xsd file into a human readable html page.
It supports the following xsd features:
     * xs:element
     * xs:sequence
     * xs:all
     * xs:attribute
     * xs:complexType
     * xs:simpleType
     * xs:restriction with xs:enumeration child
     * xs:choice
     * xs:union
Both nested and flat xsd structures are supported.
@author TCSASSEMBLER
@version 1.0
-->

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    exclude-result-prefixes="xs">
  <xsl:output	method="html"	omit-xml-declaration="yes" doctype-public="-//W3C//DTD HTML 4.01//EN" 
              doctype-system="http://www.w3.org/TR/html4/strict.dtd" />

  <!--
    This template matches all the elements in the schema
    It can be thought of as the entry point into the transform.
  -->
  <xsl:template match="/">
    
    <html>
      <head>
        <!--
          CSS elements to make the html look presentable and understandable.
          Can be removed safely without affecting functionality.
        -->
        <style type="text/css">
          .tbl {
          border-left:1px solid black;
          width:90%;
          }
          .sequence {
          background-color:#6CA5C1;
          }
          .all {
          background-color:#CC5252;
          }
          .choice {
          background-color:#FFFF66;
          }
          .attribute {
          background-color:#33CC66;
          }
          .attributecontent {
          background-color:#33CC66;
          display: inline-block;
          text-align:center;
          width:45%;
          border-width: 1px;
          border-style: solid;
          }
          .marginleft {
          margin-left:25px;
          }
          .margintoptop {
          margin-top:50px;
          }
          .restriction {
          background-color:#BEBEB8;
          }
          .tblcontent {
          display: inline-block;
          text-align:center;
          width:24%;
          border-width: 1px;
          border-style: solid;
          }
          .enumeration {
          text-align:center;
          border-width: 1px;
          border-style: solid;
          }
        </style>
        <title>Output HTML</title>
      </head>
      <body>
        <!--Select all element, complexType, simpleType nodes and process them-->
        <xsl:apply-templates select="xs:schema/xs:element" />
        <xsl:apply-templates select="xs:schema/xs:complexType" />
        <xsl:apply-templates select="xs:schema/xs:simpleType" />
      </body>
    </html>
  </xsl:template>

  <!--
    Matches all xs:element nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:element">
    <!--A variable pointing to the value of the type attribute of the element-->
    <xsl:variable name="typeVar" select="@type" />
    <xsl:variable name="nameVar" select="@name" />
    
    <xsl:choose>
      <!--Top level element types which only refer to another type. So ignore these.-->
      <xsl:when test="not(node()) and name(..) = 'xs:schema'" />
      <!--Top level element type which has children-->
      <xsl:when test="name(..) = 'xs:schema' or name(..) = 'xsd:schema'">
        <div class="margintoptop">
          <a name="{$nameVar}" href="#">
              <xsl:value-of select="@name"/>
          </a>
        </div>
        <xsl:apply-templates select="xs:complexType" />
      </xsl:when>
      <!--Mid level element - so its name, type etc information is shown-->
      <xsl:otherwise>
        <div>
          <span class="tblcontent">
            <xsl:value-of select="@name"/>
          </span>
          <span class="tblcontent">
            <xsl:choose>
              <xsl:when test="not(@type)">
                see inline type below
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <!--An xsd type, so no link needs to be created-->
                  <xsl:when test="starts-with(@type, 'xs:') or starts-with(@type, 'xsd:')">
                    <xsl:value-of select="@type"/>
                  </xsl:when>
                  <!--An non-xsd type, so link needs to be created-->
                  <xsl:otherwise>
                    <a href="#{$typeVar}">
                      <xsl:value-of select="@type"/>
                    </a>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </span>
          <span class="tblcontent">
            <xsl:choose>
              <xsl:when test="not(@minOccurs)">
                1
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="@minOccurs"/>
              </xsl:otherwise>
            </xsl:choose>
          </span>
          <span class="tblcontent">
            <xsl:choose>
              <xsl:when test="not(@maxOccurs)">
                1
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="@maxOccurs"/>
              </xsl:otherwise>
            </xsl:choose>
          </span>
        </div>
        <!--Non-Empty xs:element so we need to recurse further down-->
        <xsl:if test="node()">
          <xsl:apply-templates select="xs:complexType" />
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--
    Matches all xs:complexType nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:complexType">
    <xsl:variable name="nameVar" select="@name" />
    
    <!--This is a top level complex type-->
    <xsl:if test="name(..) = 'xs:schema' or name(..) = 'xsd:schema'">
      <div class="margintoptop">
        <a name="{$nameVar}" href="#">
          <xsl:value-of select="@name"/>
        </a>
      </div>
    </xsl:if>
    <!--Select all sequence, all, choice, attribute nodes and process them-->
    <xsl:apply-templates select="xs:sequence" />
    <xsl:apply-templates select="xs:all" />
    <xsl:apply-templates select="xs:choice" />
    <xsl:apply-templates select="xs:attribute" />
  </xsl:template>

  <!--
    Matches all xs:sequence nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:sequence">
    <div class="sequence">sequence</div>
    <div class="sequence">
      <div class="tbl sequence marginleft">
        <div>
          <div>
            <span class="tblcontent">
              <b>Name</b>
            </span>
            <span class="tblcontent">
              <b>Type</b>
            </span>
            <span class="tblcontent">
              <b>Min Occurs</b>
            </span>
            <span class="tblcontent">
              <b>Max Occurs</b>
            </span>
          </div>
        </div>
        <!--Select element nodes and process them-->
        <xsl:apply-templates select="xs:element" />
      </div>
    </div>
  </xsl:template>

  <!--
    Matches all xs:all nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:all">
    <div class="all">all</div>
    <div class="all">
      <div class="tbl all marginleft">
        <div>
          <span class="tblcontent">
            <b>Name</b>
          </span>
          <span class="tblcontent">
            <b>Type</b>
          </span>
          <span class="tblcontent">
            <b>Min Occurs</b>
          </span>
          <span class="tblcontent">
            <b>Max Occurs</b>
          </span>
        </div>
        <!--Select element nodes and process them-->
        <xsl:apply-templates select="xs:element" />
      </div>
    </div>
  </xsl:template>

  <!--
    Matches all xs:choice nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:choice">
    <div class="choice">choice</div>
    <div class="choice">
      <div class="tbl choice marginleft">
        <div>
          <span class="tblcontent">
            <b>Name</b>
          </span>
          <span class="tblcontent">
            <b>Type</b>
          </span>
          <span class="tblcontent">
            <b>Min Occurs</b>
          </span>
          <span class="tblcontent">
            <b>Max Occurs</b>
          </span>
        </div>
        <!--Select element nodes and process them-->
        <xsl:apply-templates select="xs:element" />
      </div>
    </div>
  </xsl:template>

  <!--
    Matches all xs:attribute nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:attribute">
    <xsl:variable name="typeVar" select="@type" />
    <!--If this is the first attribute, then print 'attributes' before the table-->
    <xsl:if test="not(preceding-sibling::xs:attribute)">
      <div class="attribute">attributes</div>
      <div class="attribute">
        <span class="attributecontent marginleft">
          <b>Name</b>
        </span>
        <span class="attributecontent">
          <b>Type</b>
        </span>
      </div>
    </xsl:if>
    <div class="attribute">
      <span class="attributecontent marginleft">
        <xsl:value-of select="@name"/>
      </span>
      <span class="attributecontent">
        <xsl:choose>
          <!--An xsd type, so no link needs to be created-->
          <xsl:when test="starts-with(@type, 'xs:') or starts-with(@type, 'xsd:')">
            <xsl:value-of select="@type"/>
          </xsl:when>
          <!--An non-xsd type, so link needs to be created-->
          <xsl:otherwise>
            <a href="#{$typeVar}">
              <xsl:value-of select="@type"/>
            </a>
          </xsl:otherwise>
        </xsl:choose>
      </span>
    </div>
  </xsl:template>

  <!--
    Matches all xs:simpleType nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:simpleType">
    <xsl:variable name="nameVar" select="@name" />
    <!--This is a top level simple type-->
    <xsl:if test="name(..) = 'xs:schema' or name(..) = 'xsd:schema'">
      <div class="margintoptop">
        <a name="{$nameVar}" href="#">
          <xsl:value-of select="@name"/>
        </a>
      </div>
    </xsl:if>
    <!--Select element and union nodes and process them-->
    <xsl:apply-templates select="xs:restriction" />
    <xsl:apply-templates select="xs:union" />
  </xsl:template>

  <!--
    Matches all xs:restriction nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:restriction">
    <div class="restriction">
      restriction of type <b>
        <xsl:value-of select="@base"/>
      </b>
    </div>
    <!--Select enumeration nodes and process them-->
    <xsl:apply-templates select="xs:enumeration" />
  </xsl:template>

  <!--
    Matches all xs:union nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:union">
    <div class="restriction">
      union of types <b>
        <xsl:value-of select="@memberTypes"/>
      </b>
    </div>
  </xsl:template>

  <!--
    Matches all xs:union nodes amongst the ones that are sent into it.
  -->
  <xsl:template match="xs:enumeration">
    <!--If this is the first enumeration node, then print header first-->
    <xsl:if test="not(preceding-sibling::xs:enumeration)">
      <div class="enumeration marginleft restriction">Enumeration Name</div>
    </xsl:if>
    <div class="enumeration marginleft restriction">
      <xsl:value-of select="@value"/>
    </div>
  </xsl:template>
</xsl:stylesheet>