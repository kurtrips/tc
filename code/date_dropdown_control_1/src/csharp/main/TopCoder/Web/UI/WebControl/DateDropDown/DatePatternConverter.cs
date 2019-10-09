// DatePatternConverter.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.ComponentModel;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>The custom type converter for converting expandable DatePattern custom property to and from other
    /// representations. The main purpose of this class is to serialize the DatePattern object to a string object for
    /// further storing in the  ViewState. And the deserialization from string to the DatePattern is supported. </para>
    /// </summary>
    /// 
    /// <threadsafety>
    /// This class is not thread-safe. It inherits from not thread-safe parent class
    /// (ExpandableObjectConverter) and will be used by web-controls, which do not execute additional threads.
    /// </threadsafety>
    /// 
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class DatePatternConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// <para>The default constructor.</para>
        /// </summary>
        public DatePatternConverter() : base()
        {
        }

        /// <summary>
        /// <para>Overrides the method, which returns the whether this converter can convert an object of the given type to
        /// the type of this converter, using the specified context.</para>
        /// </summary>
        /// <param name="context">
        /// an instance of the ITypeDescriptorContext that provides a format context. Can not be null.
        /// </param>
        /// <param name="sourceType">
        /// an instance of the Type that represents the type you want to convert from. Can not be null.
        /// </param>
        /// <returns>
        /// the flag representing whether the converter can provide this conversion or not. true - if this converter can
        /// perform the conversion; otherwise, false.
        /// Always returns true for string type.
        /// </returns>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            Helper.ValidateNotNull(context, "context");
            Helper.ValidateNotNull(sourceType, "sourceType");

            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        /// <summary>
        /// <para>Overrides the method, which returns whether this converter can convert the object to the specified type,
        /// using the specified context.</para>
        /// </summary>
        /// <param name="context">
        /// an instance of the ITypeDescriptorContext that provides a format context. Can not be null.
        /// </param>
        /// <param name="destinationType">
        /// an instance of the Type that represents the type you want to convert to. Can not be null.
        /// </param>
        /// <returns>
        /// the flag representing whether the converter can provide this conversion or not. true - if this converter can
        /// perform the conversion; otherwise, false.
        /// Alwyas returns true for string destinationType.
        /// </returns>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            Helper.ValidateNotNull(context, "context");
            Helper.ValidateNotNull(destinationType, "destinationType");

            if (destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        /// <summary>
        /// <para>Overrides the method, which converts the given object to the type of this converter,
        /// using the specified context and culture information.</para>
        /// </summary>
        /// <param name="context">
        /// an instance of the ITypeDescriptorContext that provides a format context. Can be null.
        /// </param>
        /// <param name="culture">an instance of  CultureInfo to use as the current culture. Cannot be null.</param>
        /// <param name="value">an instance of the Object to convert. Can be null.</param>
        /// <returns>
        /// an Object instance representing the converted value. In will be DatePattern instance in most cases. It
        /// should not be null.
        /// </returns>
        /// <exception cref="ArgumentException">if the value contains
        /// not valid string representation of the DatePattern.</exception>
        /// <exception cref="ArgumentNullException">If culture is null.</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Helper.ValidateNotNull(culture, "culture");
            DatePattern ret = new DatePattern();

            if (value == null)
            {
                return ret;
            }

            if (value is string)
            {
                string stringRep = value as string;

                //Empty string representation means a new DatePattern
                if (stringRep.Trim().Equals(String.Empty))
                {
                    return ret;
                }

                try
                {
                    //Using XmlTextReader so that special characters in date format string like '<' can be handled!
                    using (XmlTextReader reader = new XmlTextReader(stringRep, XmlNodeType.Document, null))
                    {
                        Rule rule = null;
                        while (reader.Read())
                        {
                            //Only interesed in the Element nodes.
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    //Create the DatePattern instance with its properties
                                    case "DatePattern":
                                        {
                                            ret.InputDateFormat = GetDatePatternPropertyValue(
                                                reader.GetAttribute("InputDateFormat"), String.Empty);

                                            ret.DisplayDateFormat = GetDatePatternPropertyValue(
                                                reader.GetAttribute("DisplayDateFormat"), String.Empty);

                                            ret.StartDate = GetDatePatternPropertyValue(
                                                reader.GetAttribute("StartDate"), String.Empty);

                                            ret.StopDate = GetDatePatternPropertyValue(
                                                reader.GetAttribute("StopDate"), String.Empty);

                                            break;
                                        }
                                    //Create the Rule instance with its properties
                                    case "Rule":
                                        {
                                            if (rule != null)
                                            {
                                                ret.Rules.Add(rule);
                                            }
                                            rule = new Rule();

                                            rule.DateValue = GetDatePatternPropertyValue(
                                                reader.GetAttribute("DateValue"), String.Empty);

                                            rule.DateType = GetDatePatternPropertyValue(
                                                reader.GetAttribute("DateType"), String.Empty);

                                            rule.TimeValue = GetDatePatternPropertyValue(
                                                reader.GetAttribute("TimeValue"), String.Empty);

                                            rule.ReverseOrder = bool.Parse(GetDatePatternPropertyValue(
                                                reader.GetAttribute("ReverseOrder"), "False"));

                                            rule.YearDivisor = int.Parse(GetDatePatternPropertyValue(
                                                reader.GetAttribute("YearDivisor"), "1"));

                                            rule.Interleave = int.Parse(GetDatePatternPropertyValue(
                                                reader.GetAttribute("Interleave"), "0"));

                                            break;
                                        }
                                }
                            }
                        }

                        //The last rule still needs to be added
                        if (rule != null)
                        {
                            ret.Rules.Add(rule);
                        }

                        return ret;
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Could not perform deserialization of DatePattern.", e);
                }
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        /// <summary>
        /// <para>Overrides the method, which converts the given value to the specified type, using the specified context
        /// and culture information.</para>
        /// <para>Exceptions Handling:  throws ArgumentNullException if any argument (except
        /// cultureInfo and value) is null. throws ArgumentException if the value contains not valid DatePattern
        /// instance. </para>
        /// </summary>
        /// <param name="context">
        /// an instance of the ITypeDescriptorContext that provides a format context. Can be null.
        /// </param>
        /// <param name="culture">
        /// an instance of  CultureInfo to use as the current culture. Can be null - means that the current culture
        /// should be used.
        /// </param>
        /// <param name="value">
        /// and instance of the Object to convert. Can be null. If not null, then should contain a valid DatePattern
        /// instance.
        /// </param>
        /// <param name="destinationType">
        /// an instance of Type object to convert the "value" argument to. Can not be null.
        /// </param>
        /// <returns>
        /// an Object instance representing the converted value. In will be string instance in most cases. It should not
        /// be null.
        /// </returns>
        /// <exception cref="ArgumentNullException">If destinationType is null</exception>
        /// <exception cref="ArgumentException">if the value contains not DatePattern instance.</exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            //Must check this before checking for the type of 'value'
            if (value == null && destinationType == typeof(string))
            {
                return String.Empty;
            }

            //Validate
            if (!(value is DatePattern))
            {
                throw new ArgumentException("value must be of type DatePattern.", "value");
            }
            Helper.ValidateNotNull(destinationType, "destinationType");

            if (destinationType == typeof(string))
            {
                //Prepare
                StringBuilder builder = new StringBuilder();
                DatePattern objDatePattern = value as DatePattern;

                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlTextWriter writer = new XmlTextWriter(stringWriter))
                    {
                        //Serialize

                        //Serialize the DatePattern instance.
                        writer.WriteStartElement("DatePattern");
                        if (objDatePattern.InputDateFormat != null &&
                            !objDatePattern.InputDateFormat.Equals(String.Empty))
                        {
                            writer.WriteAttributeString("InputDateFormat", objDatePattern.InputDateFormat);
                        }
                        if (objDatePattern.DisplayDateFormat != null &&
                            !objDatePattern.DisplayDateFormat.Equals(String.Empty))
                        {
                            writer.WriteAttributeString("DisplayDateFormat", objDatePattern.DisplayDateFormat);
                        }
                        if (objDatePattern.StartDate != null && !objDatePattern.StartDate.Equals(String.Empty))
                        {
                            writer.WriteAttributeString("StartDate", objDatePattern.StartDate);
                        }
                        if (objDatePattern.StopDate != null && !objDatePattern.StopDate.Equals(String.Empty))
                        {
                            writer.WriteAttributeString("StopDate", objDatePattern.StopDate);
                        }

                        //Serialize each child rule of the DatePattern instance.
                        foreach (Rule rule in objDatePattern.Rules)
                        {
                            writer.WriteStartElement("Rule");
                            if (!(rule.DateType.Equals(String.Empty)))
                            {
                                writer.WriteAttributeString("DateType", rule.DateType);
                            }
                            if (!(rule.DateValue.Equals(String.Empty)))
                            {
                                writer.WriteAttributeString("DateValue", rule.DateValue);
                            }
                            if (!(rule.TimeValue.Equals(String.Empty)))
                            {
                                writer.WriteAttributeString("TimeValue", rule.TimeValue);
                            }
                            if (rule.YearDivisor != 1)
                            {
                                writer.WriteAttributeString("YearDivisor", rule.YearDivisor.ToString());
                            }
                            if (rule.ReverseOrder != false)
                            {
                                writer.WriteAttributeString("ReverseOrder", rule.ReverseOrder.ToString().ToLower());
                            }
                            if (rule.Interleave != 0)
                            {
                                writer.WriteAttributeString("Interleave", rule.Interleave.ToString());
                            }
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    return stringWriter.ToString();
                }
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        /// <summary>
        /// Checks a string for null. If it is null, returns the defaultValue otherwise returns the string itself.
        /// </summary>
        /// <param name="attrValue">The string to check</param>
        /// <param name="defaultValue">The default value to set to.</param>
        /// <returns>If string is null, returns the defaultValue otherwise returns the string itself.</returns>
        private static string GetDatePatternPropertyValue(string attrValue, string defaultValue)
        {
            return attrValue == null ? defaultValue : attrValue;
        }
    }
}
