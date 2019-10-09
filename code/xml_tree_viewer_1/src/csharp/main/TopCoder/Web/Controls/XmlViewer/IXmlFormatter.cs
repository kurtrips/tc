// IXmlFormatter.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// <para>This interface defines a format operation that is used by the XmlTreeViewControl to format XML in a
    /// specific way for the user when displaying the raw XML.
    /// This interface contains only one method which takes an XML string and returns a formatted string,
    /// not necessarily in XML format anymore, although the input is expected to be XML.</para>
    /// <para>Implementations of this interface
    /// do not need to be thread safe, due to how they will be called in the ASP.NET environment, but implementations
    /// will be placed into the ViewState of XmlTreeViewControls, so they need to be serializable.</para>
    /// </summary>
    /// <author>Ghostar</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IXmlFormatter
    {
        /// <summary>
        /// This method formats the given XML string for display in the XmlTreeViewControl's raw XML view. This
        /// operation expects well-formed XML in the input, but the output does not necessarily have to be in XML
        /// format.
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <exception cref="InvalidXmlException">
        /// InvalidXmlException if the given parameter is not well-formed XML
        /// </exception>
        /// <param name="xml">The XML to format</param>
        /// <returns>The formatted string</returns>
        string Format(string xml);
    }
}
