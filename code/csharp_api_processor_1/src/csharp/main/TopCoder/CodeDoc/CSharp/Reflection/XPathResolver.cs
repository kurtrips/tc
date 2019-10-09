// XPathResovler.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using TopCoder.LoggingWrapper;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// This internal class represents an XPath resolver which is used to replace the references in the given apiSpec
    /// XmlDocument with the XPath representation. Please refer to the apiSpec.xsd on the details of the format of the
    /// XPath. This class is used by the ReflectionEngine by the end of the API documentation generation.
    /// </summary>
    /// <threadsafety>
    /// <para>Thread Safety: This class is mutable and not thread safety.</para>
    /// </threadsafety>
    /// <author>urtks</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class XPathResolver
    {
        /// <summary>
        /// A mapping of name to Type of a class/interface/struct etc.
        /// </summary>
        private static IDictionary<string, IList<Type>> nameToType = new Dictionary<string, IList<Type>>();

        /// <summary>
        /// An array containing prefixes contained in compiler output documentation for various programming constructs.
        /// </summary>
        private static readonly string[] IDENTIFIER_PREFIXES =
            new string[] { "N:", "T:", "F:", "P:", "M:", "E:", "!:" };

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'class' and a given 'name' attribute
        /// </summary>
        private const string CLASS_XPATH_STUB = "/class[@name='{0}']";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'interface' and a given 'name' attribute
        /// </summary>
        private const string INTERFACE_XPATH_STUB = "/interface[@name='{0}']";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'enum' and a given 'type' attribute
        /// </summary>
        private const string ENUM_XPATH_STUB = "/enum[type='{0}']";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'struct' and a given 'name' attribute
        /// </summary>
        private const string STRUCT_XPATH_STUB = "/struct[@name='{0}']";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'package' and a given 'name' attribute
        /// </summary>
        private const string PACKAGE_XPATH_STUB = "/package[@name='{0}']";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'package' and a given 'name' attribute
        /// </summary>
        private const string CTOR_XPATH_STUB = "/constructor[@name='{0}' and ";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'method' and a given 'name' attribute.
        /// Also contains an and clause
        /// </summary>
        private const string METHOD_XPATH_STUB = "/method[@name='{0}' and ";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'indexer' and a given 'name' attribute.
        /// Also contains an and clause
        /// </summary>
        private const string INDEXER_XPATH_STUB = "/indexer[@name='{0}' and ";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'param' at given index and given value.
        /// </summary>
        private const string PARAM_XPATH_STUB = "param[{0}]='{1}'";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'field' and a given 'name' attribute
        /// </summary>
        private const string FIELD_XPATH_STUB = "/field[@name='{0}']";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'property' and a given 'name' attribute
        /// </summary>
        private const string PROPERTY_XPATH_STUB = "/property[@name='{0}']";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'event' and a given 'name' attribute
        /// </summary>
        private const string EVENT_XPATH_STUB = "/event[@name='{0}']";

        /// <summary>
        /// A general xpath stub selecting a child node with node name 'delegate' and a given 'name' attribute
        /// </summary>
        private const string DELEGATE_XPATH_STUB = "/delegate[@name='{0}']";

        /// <summary>
        /// A general stub for creating a 'typeref' node. See apiSpec.xsd for more information about typeref.
        /// </summary>
        private const string TYPEREF_PREFIX_STUB = "<typeref xpath=\"{0}\">{1}</typeref>";

        /// <summary>
        /// Maintains a copy of the XmlDocument instance passed to the AddXPathReferences for use by the
        /// GetXXXPath fucntions. It is initally null and set to null after each time the execution
        /// exits AddXPathReferences.
        /// </summary>
        private XmlDocument inputDoc = null;

        /// <summary>
        /// <para>Represents the MBRLogger instance used to log infos, warnings or errors.
        /// Initialized in the constructor. Reference not changed afterwards.
        /// Can be null indicating no logger is used.</para>
        /// </summary>
        private readonly MBRLogger logger;

        /// <summary>
        /// <para>Creates a new instance of XPathResolver with no logger.</para>
        /// </summary>
        public XPathResolver()
        {
        }

        /// <summary>
        /// <para>Creates a new instance of XPathResolver with the given MBRLogger instance.</para>
        /// </summary>
        /// <param name="logger">the MBRLogger instance used to log infos, warnings or errors.</param>
        /// <exception cref="ArgumentNullException">if logger is null.</exception>
        public XPathResolver(MBRLogger logger)
        {
            Helper.ValidateNotNull(logger, "logger");
            this.logger = logger;
        }

        /// <summary>
        /// <para>This method replaces the references in the apiSpec XmlDocument with the XPath representation. Please
        /// refer to the apiSpec.xsd on the details of the format of the XPath. This methods iterates over the elements
        /// whose type or base type is TypeValueDesc as well as the 'see' elements with the cref attribute, and
        /// resolve their XPath representation by calling ResolveXPath method and replace them with the XPath
        /// representation.</para>
        /// </summary>
        /// <param name="apiSpec">the XmlDocument contains the API documentation</param>
        /// <exception cref="ArgumentNullException">if apiSpec is null</exception>
        /// <exception cref="ArgumentException">if the root element of apiSpec is not 'apispec'</exception>
        public void AddXPathReferences(XmlDocument apiSpec)
        {
            //Validate
            Helper.ValidateNotNull(apiSpec, "apiSpec");
            if (apiSpec.DocumentElement == null || apiSpec.DocumentElement.Name != "apispec")
            {
                throw new ArgumentException("Root element of apiSpec must be 'apispec'");
            }

            try
            {
                // Create a schema validating XmlReader.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(Helper.GetApispecSchema());
                settings.ValidationType = ValidationType.Schema;

                //Load the existing xml to the validating XmlReader.
                MemoryStream ms = new MemoryStream();
                apiSpec.Save(ms);
                ms.Position = 0;
                XmlReader reader = XmlReader.Create(ms, settings);

                //Find and process nodes with type/base type as TypeValueDesc or 'cref' nodes
                apiSpec.Load(reader);
                inputDoc = apiSpec;
                DepthFirstSearch(apiSpec.DocumentElement as XmlNode);
            }
            finally
            {
                inputDoc = null;
            }
        }

        /// <summary>
        /// <para>Gets the XPath represetation of the given namespace, type or member specified by identifier argument.
        /// Sometimes it's impossible to uniquely identify the XPath representation of the given idenifier. In this
        /// case, the idenifier is simply returned, and the warnings are logged using the referened MBRLogger instance
        /// (if not null). The identifier may be an unique id from the /doc XML file generated by compiler or simply the
        /// full name of the type or member, or even some other strings that partially describe the member (e.g. the
        /// method's full name without listing its parameters). This method is heuristic and can be improved by deriving
        /// from this class and providing an extra implementation.
        /// </para>
        /// </summary>
        /// <param name="identifier">The identifier of the given namespace, type or member.</param>
        /// <exception cref="ArgumentNullException"> if identifier is null.</exception>
        /// <exception cref="ArgumentException"> if identifier is empty.</exception>
        protected virtual string ResolveXPath(string identifier)
        {
            Helper.ValidateNotNullNotEmpty(identifier, "identifier");

            //For CREF nodes
            for (int idx = 0; idx < IDENTIFIER_PREFIXES.Length; idx++)
            {
                if (identifier.StartsWith(IDENTIFIER_PREFIXES[idx]))
                {
                    switch (idx)
                    {
                        //Namespace
                        case 0:
                            {
                                return GetNamespaceXpath(identifier);
                            }
                        //Class, Delegates, interfaces etc.
                        case 1:
                            {
                                return GetClassXpath(identifier).XPath;
                            }
                        //Fields
                        case 2:
                            {
                                return GetEventFieldPropertyXPath(identifier, FIELD_XPATH_STUB).XPath;
                            }
                        //Property, indexers etc.
                        case 3:
                            {
                                return GetEventFieldPropertyXPath(identifier, PROPERTY_XPATH_STUB).XPath;
                            }
                        //Methods, Constructors etc.
                        case 4:
                            {
                                return GetMethodXPath(identifier).XPath;
                            }
                        //Events
                        case 5:
                            {
                                return GetEventFieldPropertyXPath(identifier, EVENT_XPATH_STUB).XPath;
                            }
                        //Error
                        case 6:
                            {
                                if (logger != null)
                                {
                                    logger.Log(Level.WARN, "Unknown identifier {0}.", identifier);
                                }
                                return identifier;
                            }
                    }
                }
            }

            //For TYPEVALUEDESC nodes
            identifier = identifier.Replace('+', '.');
            string ret = GetClassXpath("T:" + identifier);
            if (ret != "T:" + identifier)
            {
                return ret;
            }

            return identifier;
        }

        /// <summary>
        /// Gets the xpath for an identifier which could be for a event, property or a field.
        /// </summary>
        /// <param name="identifier">The identifier to use to form the xpath</param>
        /// <param name="xPathStub">A stub describing part of the xpath being formed.</param>
        /// <returns>The xpath formed. This points to a valid node in the xml document.</returns>
        private TypeRefDescriptor GetEventFieldPropertyXPath(string identifier, string xPathStub)
        {
            try
            {
                string prefixLessIdentifier = identifier.Remove(0, 2);
                string classPart = prefixLessIdentifier.Substring(0, prefixLessIdentifier.LastIndexOf('.'));
                string evntFldPropPart = prefixLessIdentifier.Substring(prefixLessIdentifier.LastIndexOf('.') + 1);

                //Get the type part
                string retXpath = GetClassXpath("T:" + classPart).XPath;

                //Get the field part
                retXpath += string.Format(xPathStub, evntFldPropPart);

                return new TypeRefDescriptor(retXpath);
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    logger.Log(Level.ERROR, "Could not get xpath for identifier {0}. Exception: {1}.",
                        new object[] { identifier, e.ToString() });
                }
                return new TypeRefDescriptor(identifier); ;
            }
        }

        /// <summary>
        /// Gets the xpath for an identifier which could be for a method.
        /// </summary>
        /// <param name="identifier">The identifier to use to form the xpath</param>
        /// <returns>The xpath formed. This points to a valid node in the xml document.</returns>
        private TypeRefDescriptor GetMethodXPath(string identifier)
        {
            try
            {
                string prefixLessIdentifier = identifier.Remove(0, 2);
                if (!prefixLessIdentifier.EndsWith(")"))
                {
                    prefixLessIdentifier += "()";
                }

                string[] split = prefixLessIdentifier.Split('(');
                string methodName = split[0].Substring(split[0].LastIndexOf('.') + 1);
                string className = split[0].Substring(0, split[0].LastIndexOf('.'));
                string paramPart = split[1].Remove(split[1].Length - 1);

                //Method could be of class/interface/struct
                string retXpath = GetClassXpath("T:" + className).XPath;

                //Get the parameters of the fuicntion
                string[] parameters = paramPart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Type[] paramTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramTypes[i] = FindType(parameters[i]);
                }

                //Get the method of the type
                Type foundType = FindType(className);
                MethodBase methodInfo;
                //It might be constructor also
                if (methodName == "#ctor")
                {
                    methodInfo = foundType.GetConstructor(Helper.GetBindingFlags(true), null, paramTypes, null);
                }
                else
                {
                    methodInfo = foundType.GetMethod(methodName,
                        Helper.GetBindingFlags(true), null, CallingConventions.Any, paramTypes, null);
                }

                if (Helper.IsIndexer(methodInfo))
                {
                    //Get the indexer part
                    retXpath += string.Format(INDEXER_XPATH_STUB, methodName);
                }
                else if (methodInfo.IsConstructor)
                {
                    //Get the constructor part
                    retXpath += string.Format(CTOR_XPATH_STUB, methodName.Replace('#', '.'));
                }
                else
                {
                    //Get the method part
                    retXpath += string.Format(METHOD_XPATH_STUB, methodName);
                }

                //Get the paramters part
                string paramXpath = string.Empty;
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramXpath += string.Format(PARAM_XPATH_STUB, i + 1, parameters[i]);
                    paramXpath += " and ";
                }

                paramXpath += "not(param[" + (parameters.Length + 1) + "])";
                retXpath += paramXpath + "]";

                //return the total xpath formed
                return new TypeRefDescriptor(retXpath);
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    logger.Log(Level.ERROR, "Could not get xpath for identifier {0}. Exception: {1}.",
                        new object[] { identifier, e.ToString() });
                }
                return new TypeRefDescriptor(identifier);
            }
        }

        /// <summary>
        /// Gets the xpath for an identifier which could be for a class
        /// and which contain nested classes, interface or structs.
        /// </summary>
        /// <param name="identifier">The identifier to use to form the xpath</param>
        /// <returns>The xpath formed. This points to a valid node in the xml document.</returns>
        private TypeRefDescriptor GetClassXpath(string identifier)
        {
            try
            {
                string prefixLessIdentifier = identifier.Remove(0, 2);

                //Get the index of the array opening. 
                //And store the array string part
                string arrayPart = string.Empty;
                if (prefixLessIdentifier.Contains("["))
                {
                    int startArray = prefixLessIdentifier.IndexOf('[');
                    arrayPart = prefixLessIdentifier.Substring(startArray);
                    prefixLessIdentifier = prefixLessIdentifier.Remove(startArray);
                }

                //Get type from the loaded assemblies
                IList<Type> foundTypes = FindAllTypes(prefixLessIdentifier);

                if (foundTypes == null)
                {
                    return new TypeRefDescriptor(identifier);
                }

                //Get the namespace part
                string nameSpace = foundTypes[0].Namespace;
                string retXpath = string.Format(PACKAGE_XPATH_STUB, nameSpace);

                //Get the classes part
                string classNames = string.Empty;
                foreach (Type foundType in foundTypes)
                {
                    //Can be a struct
                    if (foundType.IsValueType && !foundType.IsEnum)
                    {
                        retXpath += string.Format(STRUCT_XPATH_STUB, foundType.Name);
                    }
                    //Can be an interface
                    else if (foundType.IsInterface)
                    {
                        retXpath += string.Format(INTERFACE_XPATH_STUB, foundType.Name);
                    }
                    //Can be a delegate
                    else if (Helper.IsDelegate(foundType))
                    {
                        retXpath += string.Format(DELEGATE_XPATH_STUB, foundType.Name);
                    }
                    //Can be enum
                    else if (foundType.IsEnum)
                    {
                        retXpath += string.Format(ENUM_XPATH_STUB, foundType.Name);
                    }
                    //Can be a class
                    else if (foundType.IsClass)
                    {
                        //Get the classes part in the middle section
                        retXpath += string.Format(CLASS_XPATH_STUB, foundType.Name);
                    }
                    //Unknown type
                    else 
                    {
                        if (logger != null)
                        {
                            logger.Log(Level.ERROR, "Could not resolve type for identifier {0}.",
                                new object[] { identifier });
                        }
                        return new TypeRefDescriptor(identifier);
                    }

                    classNames += foundType.Name;
                }
                classNames.Remove(classNames.Length - 1);

                //Create TypeRefDescriptor and return
                TypeRefDescriptor ret = new TypeRefDescriptor(nameSpace + ".", retXpath, classNames, arrayPart);
                return ret;
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    logger.Log(Level.ERROR, "Could not get xpath for identifier {0}. Exception: {1}.",
                        new object[] { identifier, e.ToString() });
                }
                return new TypeRefDescriptor(identifier);
            }
        }

        /// <summary>
        /// Gets the xpath for an identifier which could be for a namespace.
        /// </summary>
        /// <param name="identifier">The identifier to use to form the xpath</param>
        /// <returns>The xpath formed. This points to a valid node in the xml document.</returns>
        private string GetNamespaceXpath(string identifier)
        {
            try
            {
                string prefixLessIdentifier = identifier.Remove(0, 2);

                //See if the identifier is an actual namespace in the xml.
                if (inputDoc.SelectSingleNode(
                    "apispec/package" + Helper.GetXpathAttributePredicate("name", prefixLessIdentifier)) != null)
                {
                    return string.Format(PACKAGE_XPATH_STUB, prefixLessIdentifier);
                }
                return identifier;
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    logger.Log(Level.ERROR, "Could not get xpath for identifier {0}. Exception: {1}.",
                        new object[] { identifier, e.ToString() });
                }
                return identifier;
            }
        }

        /// <summary>
        /// Does a depth first search of the entire xml, finding nodes which are of type/base type 'typeValueDesc'
        /// or are 'see cref' nodes. The xpaths for these node are then got using ResolveXPath and the the value of
        /// these nodes are then replaced with the xpath retreived.
        /// </summary>
        /// <param name="node">The current node being processed.</param>
        private void DepthFirstSearch(XmlNode node)
        {
            //Look for elements derived from TypeValueDesc and process them
            if (IsDerivedFromTypeValueDesc(node))
            {
                string nodeValue = node.InnerText;
                if (!nodeValue.Trim().Equals(string.Empty))
                {
                    string resolvedXpath = ResolveXPath(nodeValue);
                    node.InnerXml = resolvedXpath;
                }
            }

            //Look for 'cref' nodes and process them.
            if (IsCref(node))
            {
                //Get cref value
                string crefValue = node.Attributes.GetNamedItem("cref").InnerXml;

                //Get xpath value and set it
                string resolvedXpath = ResolveXPath(crefValue);
                XmlAttribute xpathAttr = node.OwnerDocument.CreateAttribute("xpath");
                xpathAttr.Value = resolvedXpath;
                node.Attributes.Append(xpathAttr);

                //Remove prefix from cref if present
                foreach (string prefix in IDENTIFIER_PREFIXES)
                {
                    if (crefValue.StartsWith(prefix))
                    {
                        crefValue = crefValue.Remove(0, 2);
                    }
                }
                node.Attributes["cref"].InnerXml = crefValue;
            }

            //Search deeper
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    DepthFirstSearch(child);
                }
            }
        }

        /// <summary>
        /// Checks whether a node is of TypeValueDesc schema type or is derived from it.
        /// </summary>
        /// <param name="node">The node to check</param>
        /// <returns>True if node is of TypeValueDesc type; false otherwise</returns>
        private static bool IsDerivedFromTypeValueDesc(XmlNode node)
        {
            //Check hierarchy if node is derived from TypeValueDesc
            XmlSchemaType schemaType = node.SchemaInfo.SchemaType;

            while (schemaType != null)
            {
                if (schemaType.Name == "TypeValueDesc")
                {
                    return true;
                }
                //Check base type
                schemaType = schemaType.BaseXmlSchemaType;
            }

            return false;
        }

        /// <summary>
        /// Checks whether a node is a node with name 'see' and with attribute 'cref'.
        /// </summary>
        /// <param name="node">The node to check</param>
        /// <returns>True if node is a 'see cref' node; false otherwise</returns>
        private static bool IsCref(XmlNode node)
        {
            return node.Attributes.GetNamedItem("cref") != null;
        }

        /// <summary>
        /// This returns a list of the types(in order) making up an id.
        /// For ex, if there is an id MyNS.ClassA.StructB.InterfaceC, then this method returns a list containing
        /// typeof(ClassA), typeof(StructB) and typeof(InterfaceC) in order.
        /// This method guarantees to return a non-empty list. It may return null however.
        /// </summary>
        /// <param name="id">The type id for which to get the composing types.</param>
        /// <returns>A list containing the composing types of a given string id.</returns>
        private static IList<Type> FindAllTypes(string id)
        {
            //if it has already been checked, return
            if (nameToType.ContainsKey(id))
            {
                return nameToType[id];
            }

            //Check in all loaded assemblies
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string[] ids = id.Split('.');

                for (int i = 0; i < ids.Length; i++)
                {
                    //Form a possible type name
                    string possibleTypeName = string.Empty;
                    for (int j = 0; j <= i; j++)
                    {
                        possibleTypeName += ids[j] + ".";
                    }
                    possibleTypeName = possibleTypeName.Remove(possibleTypeName.Length - 1);

                    //Try to load it
                    Type foundType = assembly.GetType(possibleTypeName, false);
                    if (foundType == null)
                    {
                        continue;
                    }

                    List<Type> types = new List<Type>();
                    types.Add(foundType);

                    //This is a nested type
                    if (i != ids.Length - 1)
                    {
                        for (int j = i + 1; j < ids.Length; j++)
                        {
                            foundType = foundType.GetNestedType(ids[j], Helper.GetBindingFlags(true));
                            types.Add(foundType);
                        }
                    }

                    //Store for possible later use.
                    nameToType[id] = types;
                    return types;
                }
            }

            nameToType[id] = null;
            return null;
        }

        /// <summary>
        /// Gets the innermost type making up an id.
        /// For ex, if there is an id MyNS.ClassA.StructB.InterfaceC, then this method returns typeof(InterfaceC).
        /// </summary>
        /// <param name="id">The string id for which to find the innermost type.</param>
        /// <returns>The innermost type composing the id.</returns>
        private static Type FindType(string id)
        {
            IList<Type> allTypes = FindAllTypes(id);
            return allTypes == null ? null : allTypes[allTypes.Count - 1];
        }

        /// <summary>
        /// This class is used by the ResolveXPath function.
        /// An xpath formed for a class can be of the form:
        /// [PreText.]&lt;typeref xpath="TheXPath"&gt;[TypeRefText]&lt;/typeref&gt;[PostText]
        /// For an example see the return node of TestMethod1 method in ClassB in sampleOutput.xml.
        /// This class encapsulates the PreText, TheXPath, TypeRefText and PostText values described above.
        /// </summary>
        private class TypeRefDescriptor
        {
            /// <summary>
            /// The text before the typeref node
            /// </summary>
            private string preText;

            /// <summary>
            /// The text after the typeref node
            /// </summary>
            private string postText;

            /// <summary>
            /// The value of xpath attribute of typeref node
            /// </summary>
            private string xpath;

            /// <summary>
            /// The node value of the typeref node
            /// </summary>
            private string typeRefText;

            /// <summary>
            /// The plain text to be placed in the calling node.
            /// </summary>
            private string plainText;

            /// <summary>
            /// Whether the calling node will have plain text or a typeref node
            /// </summary>
            private bool isPlainText;

            /// <summary>
            /// Constructor to be used if the calling node needs to have plain text placed within it.
            /// </summary>
            /// <param name="plainText">The plain text to place in the calling node</param>
            public TypeRefDescriptor(string plainText)
            {
                isPlainText = true;
                this.plainText = plainText;
            }

            /// <summary>
            /// Constructor to be used when the calling node needs to have the typeref node placed within it.
            /// </summary>
            /// <param name="preText">The text before the typeref ndoe</param>
            /// <param name="xpath">The text after the typeref node</param>
            /// <param name="typeRefText">The value of xpath attribute of typeref node</param>
            /// <param name="postText">The node value of the typeref node</param>
            public TypeRefDescriptor(string preText, string xpath, string typeRefText, string postText)
            {
                isPlainText = false;
                this.preText = preText == null ? string.Empty : preText;
                this.xpath = xpath == null ? string.Empty : xpath;
                this.typeRefText = typeRefText == null ? string.Empty : typeRefText;
                this.postText = postText == null ? string.Empty : postText;
            }

            /// <summary>
            /// The Xpath of the typeref node or the plain text of the node.
            /// </summary>
            /// <value>The Xpath of the typeref node or the plain text of the node.</value>
            public string XPath
            {
                get
                {
                    if (isPlainText)
                    {
                        return plainText;
                    }
                    else
                    {
                        return xpath;
                    }
                }
            }

            /// <summary>
            /// The complete text including the pre-text, the typeref node and the post-text.
            /// </summary>
            /// <returns>The complete text including the pre-text, the typeref node and the post-text.</returns>
            public override string ToString()
            {
                string ret = string.Empty;

                //Add the text before the typeref node
                ret += preText;

                //Add the typeref node
                ret += string.Format(TYPEREF_PREFIX_STUB, xpath, typeRefText);

                //Add the text after the typeref node
                ret += postText;

                return ret;
            }

            /// <summary>
            /// The string representation of the TypeRefDescriptor node.
            /// </summary>
            /// <param name="trd">The TypeRefDescriptor instance to convert to string.</param>
            /// <returns>The complete text including the pre-text, the typeref node and the post-text.</returns>
            public static implicit operator string(TypeRefDescriptor trd)
            {
                return trd.ToString();
            }
        }
    }
}
