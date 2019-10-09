// ReflectionEngine.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;
using TopCoder.XML.CmdLineProcessor;
using TopCoder.LoggingWrapper;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// This class extends MarshalByRefObject and represents an API reflection engine that is used to generate the API
    /// metadata of the given assemblies, modules, namespaces or types using reflection and from a partially filled API
    /// spec string. This class is created in a separated app domain other than the main app domain. Because the
    /// assemblies to analyze will be loaded to the current app domain, we create the ReflectionEngine in the separated
    /// domain which can be unloaded after the generation so that the main app domain keeps clean. Because its base
    /// class MarshalByRefObject, an instance of this class is transferred by reference between two different domains.
    /// <para>An instance of this class is supposed to be created in the method ProcessDocument of class
    /// CSharpAPIProcessor using AppDomain.CreateInstanceAndUnwrap method.</para>
    /// </summary>
    ///
    /// <threadsafety>
    /// <para>Thread Safety: This class is mutable and not thread safe.</para>
    /// </threadsafety>
    ///
    /// <author>urtks</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class ReflectionEngine : MarshalByRefObject
    {
        /// <summary>
        /// <para>Represents the MBRLogger instance used to log infos, warnings or errors.
        /// Initialized in the constructor. Reference not changed afterwards. Can be null indicating no logger
        /// is used.</para>
        /// </summary>
        private readonly MBRLogger logger;

        /// <summary>
        /// <para>Represents the ReflectionEngineParameters instance currently passed to the method WriteAPISpec. This
        /// field is assigned to an instance at the begining of the execution of the method WriteAPISpec and reset to
        /// null when the method WriteAPISpec exits. This field is used by the various private WriteXXX methods as a
        /// shared object. The content of the ReflectionEngineParameters instance is never modified.</para>
        /// </summary>
        private ReflectionEngineParameters rep;

        /// <summary>
        /// <para>Represents the XmlDocument instance to write the API documentation to. This field is assigned to an
        /// XmlDocument instance which is loaded from the given apiSpec argument at the begining of the execution of the
        /// method WriteAPISpec and reset to null when the method WriteAPISpec exits.The content of the
        /// XmlDocument instance is modified during the execution of WriteAPISpec. The innerXml text of it is used as
        /// the return value of WriteAPISpec. This field is used by the various private WriteXXX methods as a shared
        /// object.</para>
        /// </summary>
        private XmlDocument spec;

        /// <summary>
        /// <para>Represents the SlashDocCache instance used to get the inline documentation for a given member
        /// (namespace, type or member).
        /// This field is assigned to a new instance of SlashDocCache and loaded with the /doc XML
        /// files sepcified by rep.SlashDocFileNames at the begining of the execution of the method WriteAPISpec and
        /// reset to null when the method WriteAPISpec exits.The content of the SlashDocCache instance is modified
        /// during the execution of WriteAPISpec(except the initlization).This field is used by the various private
        /// WriteXXX methods as a shared object to get the inline documentation of the required members.</para>
        /// </summary>
        private SlashDocCache docCache;

        /// <summary>
        /// Represents the name of a package node
        /// </summary>
        private const string PACKAGE = "package";

        /// <summary>
        /// Represents the name of a 'name' attribute
        /// </summary>
        private const string NAME = "name";

        /// <summary>
        /// Represents the name of a 'class' node
        /// </summary>
        private const string CLASS = "class";

        /// <summary>
        /// Represents the name of a 'constructor' node
        /// </summary>
        private const string CONSTRUCTOR = "constructor";

        /// <summary>
        /// Represents the name of a 'method' node
        /// </summary>
        private const string METHOD = "method";

        /// <summary>
        /// Represents the name of a 'indexer' node
        /// </summary>
        private const string INDEXER = "indexer";

        /// <summary>
        /// Represents the name of a 'property' node
        /// </summary>
        private const string PROPERTY = "property";

        /// <summary>
        /// Represents the name of a 'field' node
        /// </summary>
        private const string FIELD = "field";

        /// <summary>
        /// Represents the name of an 'event' node
        /// </summary>
        private const string EVENT = "event";

        /// <summary>
        /// Represents the name of a 'interface' node
        /// </summary>
        private const string INTERFACE = "interface";

        /// <summary>
        /// Represents the name of a 'enum' node
        /// </summary>
        private const string ENUM = "enum";

        /// <summary>
        /// Represents the name of a 'struct' node
        /// </summary>
        private const string STRUCT = "struct";

        /// <summary>
        /// Represents the name of a 'parent' node
        /// </summary>
        private const string PARENT = "parent";

        /// <summary>
        /// Represents the name of a 'annotation' node
        /// </summary>
        private const string ANNOTATION = "annotation";

        /// <summary>
        /// Represents the name of a 'type' node
        /// </summary>
        private const string TYPE = "type";

        /// <summary>
        /// Represents the name of a 'value' attribute
        /// </summary>
        private const string VALUE = "value";

        /// <summary>
        /// Represents the name of a 'delegate' node
        /// </summary>
        private const string DELEGATE = "delegate";

        /// <summary>
        /// Represents the name of a 'return' node
        /// </summary>
        private const string RETURN = "return";

        /// <summary>
        /// Represents the name of a 'doc' node
        /// </summary>
        private const string DOC = "doc";

        /// <summary>
        /// Represents the name of a 'param' node
        /// </summary>
        private const string PARAM = "param";

        /// <summary>
        /// Represents the name of a 'typevaluespec' attribute
        /// </summary>
        private const string TYPEVALUESPEC = "typevaluespec";

        /// <summary>
        /// <para>Creates a new instance of ReflectionEngine with no logger.</para>
        /// </summary>
        public ReflectionEngine()
        {
            PrepareXsdElementsMap();
        }

        /// <summary>
        /// <para>Creates a new instance of ReflectionEngine with the given MBRLogger instance.</para>
        /// </summary>
        /// <param name="logger">MBRLogger instance used to log infos, warnings or errors.</param>
        /// <exception cref="ArgumentNullException">if logger is null.</exception>
        public ReflectionEngine(MBRLogger logger) : this()
        {
            Helper.ValidateNotNull(logger, "logger");
            this.logger = logger;
        }

        /// <summary>
        /// <para>This method encapsulates the functionality of generating the API specification.
        /// This method writes the API documentation from the partially filled API spec string and returns the XML
        /// string of the new API spec. The reflection and documentation process is tailored according to the passed
        /// ReflectionEngineParamters instance.</para>
        /// </summary>
        ///
        /// <param name="apiSpec">the string containing the XML of the partially filled API spec.</param>
        /// <param name="parameters">the ReflectionEngineParamters instance that specifies the parameters
        /// of the generation process.</param>
        ///
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        /// <exception cref="ArgumentException">if parameters.AssemblyFileNames is null or empty,
        /// or apiSpec is not a valid Xml document or the root of apiSpec is not 'apispec'.</exception>
        /// <exception cref="XmlProcessorException">If anything goes wrong with the API writing
        /// and it is not logged.</exception>
        public string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        {
            //Validate
            Helper.ValidateNotNull(parameters, "parameters");
            Helper.ValidateNotNullNotEmpty(apiSpec, "apiSpec");
            if (parameters.AssemblyFileNames == null)
            {
                throw new ArgumentException("AssemblyFileNames array of parameters must be non null.", "parameters");
            }
            Helper.ValidateArray(parameters.AssemblyFileNames, "AssemblyFileNames of ReflectionEngineParameters",
                false, true, false, false);

            //Load the apiSpec into an xml document.
            spec = new XmlDocument();
            try
            {
                spec.LoadXml(apiSpec);
            }
            catch (XmlException xe)
            {
                throw new ArgumentException("apiSpec is not a valid xml string.", xe);
            }

            //throw exception if root node is not apiSpec
            if (spec.DocumentElement.Name != "apispec")
            {
                throw new ArgumentException("apiSpec must denote an empty node with name 'apispec'", "apiSpec");
            }

            AssemblyLoader assemblyLoader = null;
            try
            {
                rep = parameters;

                //Initiate AssemblyLoader instance
                assemblyLoader = (logger == null) ? new AssemblyLoader(rep.ReferencePaths)
                    : new AssemblyLoader(rep.ReferencePaths, logger);
                assemblyLoader.InstallAssemblyResolver();

                //Load the compiler output doc files (if present)
                docCache = new SlashDocCache();
                if (rep.SlashDocFileNames != null)
                {
                    docCache.AddSlashDocFiles(rep.SlashDocFileNames);
                }

                //Load each assembly and read all the types present in it
                List<Type> types = new List<Type>();
                foreach (string assemblyName in rep.AssemblyFileNames)
                {
                    //1)Load the assembly from the current assembly name.
                    Assembly assembly = assemblyLoader.LoadAssembly(assemblyName);

                    //2)Enumerate the modules in the current assembly.
                    foreach (Module module in assembly.GetModules())
                    {
                        //2.1) If rep.ModuleNames is not null or empty,
                        //check if the module.ScopeName is in the rep.ModuleNames array.
                        //Only consider the modules that meet this condition.
                        if (rep.ModuleNames != null && rep.ModuleNames.Length != 0)
                        {
                            //Filter modules
                            if (Array.IndexOf<string>(rep.ModuleNames, module.ScopeName) < 0)
                            {
                                continue;
                            }
                        }

                        //2.2) Get the types in the current module.
                        Type[] moduleTypes = module.GetTypes();

                        //2.3) Enumerate the types in the current module.
                        foreach (Type type in moduleTypes)
                        {
                            //Only consider the types that are not nested type and
                            //not private is rep.DocumentPrivates is false.
                            if (!rep.DocumentPrivates)
                            {
                                if (type.IsNested || type.IsNotPublic)
                                {
                                    continue;
                                }
                            }

                            //2.3.1) if rep.TypePrefixes is not null or empty,
                            if (rep.TypePrefixes != null && rep.TypePrefixes.Length != 0)
                            {
                                //check if type.FullName starts with one of the prefixes in the rep.TypePrefixes array.
                                bool found = false;
                                foreach (string typePrefix in rep.TypePrefixes)
                                {
                                    if (type.FullName.StartsWith(typePrefix))
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    continue;
                                }
                            }

                            //Add the type to types IList.
                            types.Add(type);
                        }
                    }
                }

                //7. Create a List of strings to keep the namespaces of the types in types IList:
                List<string> namespaces = new List<string>();

                //Add to namespaces
                for (int i = 0; i < types.Count; i++)
                {
                    //get the namespace of the type:
                    string nameSpace = types[i].Namespace;
                    if (nameSpace == null)
                    {
                        nameSpace = Helper.GLOBALNS;
                    }
                    if (!namespaces.Contains(nameSpace))
                    {
                        namespaces.Add(nameSpace);
                    }
                }


                //Write the namespaces. This generates the output xml, less the resolved xpaths.
                WriteNamespaces(namespaces.ToArray(), types.ToArray(), spec.DocumentElement);

                //System.IO.File.WriteAllText("../../test_files/intermediate.xml", spec.OuterXml);

                //Resolve the xpaths
                XPathResolver xpathResolver;
                if (logger != null)
                {
                    xpathResolver = new XPathResolver(logger);
                }
                else
                {
                    xpathResolver = new XPathResolver();
                }
                xpathResolver.AddXPathReferences(spec);

                //System.IO.File.WriteAllText("../../test_files/op.xml", spec.OuterXml);
                //System.IO.File.WriteAllText("../../test_files/sampleOutput.xml", spec.OuterXml);

                return spec.InnerXml;
            }
            catch (Exception e)
            {
                throw new XmlProcessorException("Unable to write API specification.", e);
            }
            finally
            {
                assemblyLoader.UninstallAssemblyResolver();
                rep = null;
                spec = null;
                docCache = null;
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the given set of namespaces under the root element.
        /// types array specifies the overall types to document.</para>
        /// <para>Any exceptions are logged using logger if not null and swallowed for each namespace so that we can
        /// continue with the remaining namespaces.</para>
        /// </summary>
        /// <param name="nameSpaces">the set of namespaces to write API documentation of</param>
        /// <param name="root">the root XmlElement under which the API documentation is written.</param>
        /// <param name="types">the overall types to document.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        /// <exception cref="ArgumentException">if nameSpaces contains null or empty elements,
        /// or types contains null elements.</exception>
        private void WriteNamespaces(string[] nameSpaces, Type[] types, XmlElement root)
        {
            Helper.ValidateNotNull(root, "root");
            Helper.ValidateArray(nameSpaces, "nameSpaces", true, false, true, true);
            Helper.ValidateArray(types, "types", true, false, true, false);

            foreach (string nameSpace in nameSpaces)
            {
                try
                {
                    XmlElement packageNode = FindOrCreateChildNodeWithAttrValue(root, PACKAGE, NAME, nameSpace);

                    //Iterate over the types array, and retrives the types under the current namespace
                    List<Type> typesCurNs = new List<Type>();
                    foreach (Type type in types)
                    {
                        if (type.Namespace == nameSpace || (nameSpace == Helper.GLOBALNS && type.Namespace == null))
                        {
                            typesCurNs.Add(type);
                        }
                    }

                    //Write the various types of programming constructs inside the namespace.
                    WriteClasses(typesCurNs.ToArray(), packageNode);
                    WriteInterfaces(typesCurNs.ToArray(), packageNode);
                    WriteStructs(typesCurNs.ToArray(), packageNode);
                    WriteEnums(typesCurNs.ToArray(), packageNode);
                    WriteDelegates(typesCurNs.ToArray(), packageNode);
                }
                //Simply log so that other namespaces can be processed
                catch (Exception e)
                {
                    if (logger != null)
                    {
                        logger.Log(Level.ERROR, "Namespace {0} could not be documented. Reason: {1}.",
                            new object[] { nameSpace, e.ToString() });
                    }
                }
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the given set of classes under the subRoot element.
        /// types array specifies the overall types to document. It may contain types that are not classes.</para>
        /// </summary>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <param name="types">the overall types to document.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        /// <exception cref="ArgumentException">if types contains null elements.</exception>
        private void WriteClasses(Type[] types, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateArray(types, "types", true, false, true, false);

            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsValueType &&
                    !(type.IsSubclassOf(typeof(Delegate)) || type.IsSubclassOf(typeof(MulticastDelegate))))
                {
                    XmlElement classNode = FindOrCreateChildNodeWithAttrValue(subRoot, CLASS, NAME, type.Name);

                    //get the visibility of this type and set it to the attribute 'visibility'.
                    classNode.SetAttribute("visibility", ReflectionEngineUtility.GetTypeVisibility(type));

                    string modifiers = ReflectionEngineUtility.GetTypeModifiers(type);
                    if (modifiers != null)
                    {
                        classNode.SetAttribute("modifiers", modifiers);
                    }

                    //Use docCache to get the inline documentation:
                    string documentation = docCache[ReflectionEngineUtility.GetUniqueID(type)];
                    AddDocNode(classNode, documentation);

                    //Get the custom attributes of this type and add annotation nodes
                    AddAnnotations(type.GetCustomAttributes(false), classNode);

                    //Add parent node if needed.
                    WriteBaseType(type, classNode);

                    //Find base interface types and add 'parent' child node
                    WriteBaseInterfaces(type, classNode);

                    //Write the various types of programming constructs inside the class.
                    WriteConstructors(type, classNode);
                    WriteMethods(type, classNode);
                    WriteProperties(type, classNode);
                    WriteFields(type, classNode);
                    WriteEvents(type, classNode);

                    //Get the nested types in the class
                    Type[] nestedTypes = type.GetNestedTypes(Helper.GetBindingFlags(rep.DocumentPrivates));

                    //Write the various types of programming constructs inside the nested types.
                    WriteClasses(nestedTypes, classNode);
                    WriteInterfaces(nestedTypes, classNode);
                    WriteStructs(nestedTypes, classNode);
                    WriteEnums(nestedTypes, classNode);
                    WriteDelegates(nestedTypes, classNode);
                }
            }
        }

        /// <summary>
        /// Writes the base type of the given type to the node representing the given type.
        /// </summary>
        /// <param name="type">The type for which to write the base type</param>
        /// <param name="typeNode">The node representing the given type</param>
        private void WriteBaseType(Type type, XmlElement typeNode)
        {
            Type baseType = type.BaseType;
            if (baseType != null && baseType != typeof(object))
            {
                AddChildXmlElement(typeNode, PARENT, null, baseType.FullName);
            }
        }

        /// <summary>
        /// Writes the base interfaces of the given type to the node representing the given type.
        /// </summary>
        /// <param name="type">The type for which to write the base interfaces</param>
        /// <param name="typeNode">The node representing the given type</param>
        private void WriteBaseInterfaces(Type type, XmlElement typeNode)
        {
            TypeFilter tf = new TypeFilter(InterfaceBaseTypesFilter);
            Type[] baseInterfaces = type.FindInterfaces(tf, type);
            foreach (Type baseInterface in baseInterfaces)
            {
                AddChildXmlElement(typeNode, PARENT, null, baseInterface.FullName);
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the given set of interfaces under the subRoot element. types
        /// array specifies the overall types to document. It may contain types that are not intefaces.</para>
        /// </summary>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <param name="types"> the overall types to document.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        /// <exception cref="ArgumentException">if types contains null elements.</exception>
        private void WriteInterfaces(Type[] types, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateArray(types, "types", true, false, true, false);

            foreach (Type type in types)
            {
                if (!type.IsInterface)
                {
                    continue;
                }

                XmlElement interfaceNode = FindOrCreateChildNodeWithAttrValue(subRoot, INTERFACE, NAME, type.Name);

                //get the visibility of this type and set it to the attribute 'visibility'.
                interfaceNode.SetAttribute("visibility", ReflectionEngineUtility.GetTypeVisibility(type));

                //Use docCache to get the inline documentation:
                string documentation = docCache[ReflectionEngineUtility.GetUniqueID(type)];
                AddDocNode(interfaceNode, documentation);

                //Get the custom attributes of this type and add annotation nodes
                AddAnnotations(type.GetCustomAttributes(false), interfaceNode);

                //Find base interface types and add 'parent' child node
                WriteBaseInterfaces(type, interfaceNode);

                WriteMethods(type, interfaceNode);
                WriteProperties(type, interfaceNode);
                WriteEvents(type, interfaceNode);
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the given set of structs under the subRoot element.
        /// types array specifies the overall types to document. It may contain types that are not structs.</para>
        /// </summary>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <param name="types">the overall types to document.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        /// <exception cref="ArgumentException">if types contains null elements.</exception>
        private void WriteStructs(Type[] types, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateArray(types, "types", true, false, true, false);

            foreach (Type type in types)
            {
                if (!type.IsValueType || type.IsEnum)
                {
                    continue;
                }

                XmlElement structNode = FindOrCreateChildNodeWithAttrValue(subRoot, STRUCT, NAME, type.Name);

                //get the visibility of this type and set it to the attribute 'visibility'.
                structNode.SetAttribute("visibility", ReflectionEngineUtility.GetTypeVisibility(type));

                //Use docCache to get the inline documentation:
                string documentation = docCache[ReflectionEngineUtility.GetUniqueID(type)];
                AddDocNode(structNode, documentation);

                //Get the custom attributes of this type and add annotation nodes
                AddAnnotations(type.GetCustomAttributes(false), structNode);

                //Add parent node if needed.
                WriteBaseType(type, structNode);

                //Find base interface types and add 'parent' child node
                WriteBaseInterfaces(type, structNode);

                //Write the various types of programming constructs inside the structure.
                WriteConstructors(type, structNode);
                WriteMethods(type, structNode);
                WriteProperties(type, structNode);
                WriteFields(type, structNode);
                WriteEvents(type, structNode);

                //Get the nested types in the structure
                Type[] nestedTypes = type.GetNestedTypes(Helper.GetBindingFlags(rep.DocumentPrivates));

                //Write the various types of programming constructs inside the nested types.
                WriteClasses(nestedTypes, structNode);
                WriteInterfaces(nestedTypes, structNode);
                WriteStructs(nestedTypes, structNode);
                WriteEnums(nestedTypes, structNode);
                WriteDelegates(nestedTypes, structNode);
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the given set of enums under the subRoot element.
        /// types array specifies the overall types to document. It may contain types that are not enums.</para>
        /// </summary>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <param name="types">the overall types to document.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        /// <exception cref="ArgumentException">if types contains null elements.</exception>
        private void WriteEnums(Type[] types, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateArray(types, "types", true, false, true, false);

            foreach (Type type in types)
            {
                if (!type.IsEnum)
                {
                    continue;
                }

                //Create node with given name
                XmlElement enumNode = FindOrCreateChildNodeWithAttrValue(subRoot, ENUM, NAME, type.Name);

                //get the visibility of this type and set it to the attribute 'visibility'.
                enumNode.SetAttribute("visibility", ReflectionEngineUtility.GetTypeVisibility(type));

                //Add documentation
                AddDocNode(enumNode, docCache[ReflectionEngineUtility.GetUniqueID(type)]);

                //For each attribute, add a <annotation> sub element with value attr.TypeId.ToString() to elem.
                AddAnnotations(type.GetCustomAttributes(false), enumNode);

                //Add Type
                AddChildXmlElement(enumNode, TYPE, null, Enum.GetUnderlyingType(type).FullName);

                //Get the fields in the enum
                FieldInfo[] fields = type.GetFields(Helper.GetBindingFlags(rep.DocumentPrivates));
                foreach (FieldInfo fieldInfo in fields)
                {
                    if (fieldInfo.Name != "value__")
                    {
                        AddChildXmlElement(enumNode, VALUE, NAME, fieldInfo.Name);
                    }
                }
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the given set of delegates under the subRoot element.
        /// types array specifies the overall types to document. It may contain types that are not delegates.</para>
        /// </summary>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <param name="types">the overall types to document.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        /// <exception cref="ArgumentException">if types contains null elements.</exception>
        private void WriteDelegates(Type[] types, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateArray(types, "types", true, false, true, false);

            foreach (Type type in types)
            {
                if (!Helper.IsDelegate(type))
                {
                    continue;
                }

                //Find child element with node name 'delegate' and add if not found.
                XmlElement delegateNode = FindOrCreateChildNodeWithAttrValue(subRoot, DELEGATE, NAME, type.Name);

                //get the visibility of this type and set it to the attribute 'visibility'.
                delegateNode.SetAttribute("visibility", ReflectionEngineUtility.GetTypeVisibility(type));

                //Use docCache to get the inline documentation:
                string documentation = docCache[ReflectionEngineUtility.GetUniqueID(type)];
                AddDocNode(delegateNode, documentation);

                AddAnnotations(type.GetCustomAttributes(false), delegateNode);

                //Get the return type method and add it.
                MethodInfo method = type.GetMethod("Invoke", Helper.GetBindingFlags(rep.DocumentPrivates));
                Type retType = method.ReturnType;
                AddChildXmlElement(delegateNode, RETURN, null, retType.FullName);

                //Get the method parameters and add param nodes
                AddParameterNodes(delegateNode, method);
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the set of constructors of the given type under the subRoot
        /// element.</para>
        /// </summary>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <param name="type">The type to get constructors from.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        private void WriteConstructors(Type type, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateNotNull(type, "type");

            ConstructorInfo[] ctorInfos = type.GetConstructors(Helper.GetBindingFlags(rep.DocumentPrivates));

            foreach (ConstructorInfo ctorInfo in ctorInfos)
            {
                //Find child element with node name '<constructor>' and add if not found.
                XmlElement ctorNode = FindOrCreateMethodNode(subRoot, ctorInfo, false);

                //get the visibility of this type and set it to the attribute 'visibility'.
                ctorNode.SetAttribute("visibility", ReflectionEngineUtility.GetMethodVisibility(ctorInfo));

                string modifiers = ReflectionEngineUtility.GetMethodModifiers(ctorInfo);
                if (modifiers != null)
                {
                    ctorNode.SetAttribute("modifiers", modifiers);
                }

                //Use docCache to get the inline documentation:
                string documentation = docCache[ReflectionEngineUtility.GetUniqueID(ctorInfo)];
                AddDocNode(ctorNode, documentation);

                AddAnnotations(ctorInfo.GetCustomAttributes(false), ctorNode);

                //Get the method parameters and add param nodes
                AddParameterNodes(ctorNode, ctorInfo);
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the set of methods of the given type under the subRoot
        /// element.</para>
        /// </summary>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <param name="type">The type to get methods from.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        private void WriteMethods(Type type, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateNotNull(type, "type");

            MethodInfo[] methodInfos = type.GetMethods(Helper.GetBindingFlags(rep.DocumentPrivates));

            foreach (MethodInfo methodInfo in methodInfos)
            {
                bool isIndexer = Helper.IsIndexer(methodInfo);
                if (methodInfo.IsSpecialName && !isIndexer)
                {
                    continue;
                }

                //Find child element with node name '<method>' or '<indexer>' and add if not found.
                XmlElement methodNode = FindOrCreateMethodNode(subRoot, methodInfo, isIndexer);
                if (isIndexer)
                {
                    //Get the PropertyInfo instance corresponding to the given indexer.
                    PropertyInfo indexerInfo = GetCorrespondingIndexer(methodInfo, type);

                    //get the visibility of this type and set it to the attribute 'visibility'.
                    methodNode.SetAttribute("visibility", ReflectionEngineUtility.GetPropertyVisibility(indexerInfo));

                    string modifiers = ReflectionEngineUtility.GetPropertyModifiers(indexerInfo);
                    if (modifiers != null)
                    {
                        methodNode.SetAttribute("modifiers", modifiers);
                    }

                    //Use docCache to get the inline documentation:
                    string documentation = docCache[ReflectionEngineUtility.GetUniqueID(indexerInfo)];
                    AddDocNode(methodNode, documentation);
                }
                else
                {
                    //get the visibility of this type and set it to the attribute 'visibility'.
                    methodNode.SetAttribute("visibility", ReflectionEngineUtility.GetMethodVisibility(methodInfo));

                    string modifiers = ReflectionEngineUtility.GetMethodModifiers(methodInfo);
                    if (modifiers != null)
                    {
                        methodNode.SetAttribute("modifiers", modifiers);
                    }

                    //Use docCache to get the inline documentation:
                    string documentation = docCache[ReflectionEngineUtility.GetUniqueID(methodInfo)];
                    AddDocNode(methodNode, documentation);
                }

                AddAnnotations(methodInfo.GetCustomAttributes(false), methodNode);

                //Get the return type of the method
                Type retType = methodInfo.ReturnType;
                if (methodNode[RETURN] == null)
                {
                    AddChildXmlElement(methodNode, RETURN, null, retType.FullName);
                }
                else
                {
                    methodNode[RETURN].InnerXml = retType.FullName;
                }

                //Get the method parameters and add param nodes
                AddParameterNodes(methodNode, methodInfo);
            }
        }

        /// <summary>
        /// Gets the corresponding PropertyInfo for a MethodInfo object defining an indexer
        /// </summary>
        /// <param name="methodInfo">The MethodInfo object defining an indexer</param>
        /// <param name="type">The type to which the indexer belongs</param>
        /// <returns>The corresponding PropertyInfo for a MethodInfo object defining an indexer</returns>
        private PropertyInfo GetCorrespondingIndexer(MethodInfo methodInfo, Type type)
        {
            //Get the types of the parameters of the indexer.
            ParameterInfo[] paramsInfo = methodInfo.GetParameters();
            int arrLen = methodInfo.Name.StartsWith("set_")
                ? paramsInfo.Length - 1 : paramsInfo.Length;

            //Get the return type of the property
            Type retType = methodInfo.Name.StartsWith("set_")
                ? paramsInfo[paramsInfo.Length - 1].ParameterType : methodInfo.ReturnType;

            Type[] paramTypes = new Type[arrLen];
            for (int i = 0; i < arrLen; i++)
            {
                paramTypes[i] = paramsInfo[i].ParameterType;
            }

            //Look for property with name "Item" and the given parameter types.
            return type.GetProperty(
                "Item", Helper.GetBindingFlags(rep.DocumentPrivates), null, retType, paramTypes, null);
        }

        /// <summary>
        /// <para>This method writes API documentation of the set of properties of the given type under the subRoot
        /// element.</para>
        /// </summary>
        /// <param name="type">The type to get properties from.</param>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        private void WriteProperties(Type type, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateNotNull(type, "type");

            PropertyInfo[] propInfos = type.GetProperties(Helper.GetBindingFlags(rep.DocumentPrivates));

            foreach (PropertyInfo propInfo in propInfos)
            {
                //Ignore indexers as they are already handled by the WriteMethods method.
                if (propInfo.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                //Find child element with node name '<method>' and add if not found.
                XmlElement propertyNode = FindOrCreateChildNodeWithAttrValue(subRoot, PROPERTY, NAME, propInfo.Name);

                //get the visibility of this type and set it to the attribute 'visibility'.
                propertyNode.SetAttribute("visibility", ReflectionEngineUtility.GetPropertyVisibility(propInfo));

                string modifiers = ReflectionEngineUtility.GetPropertyModifiers(propInfo);
                if (modifiers != null)
                {
                    propertyNode.SetAttribute("modifiers", modifiers);
                }

                //Use docCache to get the inline documentation:
                string documentation = docCache[ReflectionEngineUtility.GetUniqueID(propInfo)];
                AddDocNode(propertyNode, documentation);

                AddAnnotations(propInfo.GetCustomAttributes(false), propertyNode);

                //Add the type of the property
                AddChildXmlElement(propertyNode, "type", null, propInfo.PropertyType.FullName);
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the set of fields of the given type under the subRoot
        /// element.</para>
        /// </summary>
        /// <param name="type">The type to get fields from.</param>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        private void WriteFields(Type type, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateNotNull(type, "type");

            FieldInfo[] fieldInfos = type.GetFields(Helper.GetBindingFlags(rep.DocumentPrivates));

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                //Find child element with node name '<method>' and add if not found.
                XmlElement fieldNode = FindOrCreateChildNodeWithAttrValue(subRoot, FIELD, NAME, fieldInfo.Name);

                //get the visibility of this type and set it to the attribute 'visibility'.
                fieldNode.SetAttribute("visibility", ReflectionEngineUtility.GetFieldVisibility(fieldInfo));

                string modifiers = ReflectionEngineUtility.GetFieldModifiers(fieldInfo);
                if (modifiers != null)
                {
                    fieldNode.SetAttribute("modifiers", modifiers);
                }

                //Use docCache to get the inline documentation:
                string documentation = docCache[ReflectionEngineUtility.GetUniqueID(fieldInfo)];
                AddDocNode(fieldNode, documentation);

                AddAnnotations(fieldInfo.GetCustomAttributes(false), fieldNode);

                //Add the type of the property
                AddChildXmlElement(fieldNode, "type", null, fieldInfo.FieldType.FullName);
            }
        }

        /// <summary>
        /// <para>This method writes API documentation of the set of events of the given type under the subRoot
        /// element.</para>
        /// </summary>
        /// <param name="type">The type to get events from.</param>
        /// <param name="subRoot">the sub root XmlElement under which the API documentation is written.</param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        private void WriteEvents(Type type, XmlElement subRoot)
        {
            Helper.ValidateNotNull(subRoot, "subRoot");
            Helper.ValidateNotNull(type, "type");

            EventInfo[] eventInfos = type.GetEvents(Helper.GetBindingFlags(rep.DocumentPrivates));

            foreach (EventInfo eventInfo in eventInfos)
            {
                //Find child element with node name '<event>' and add if not found.
                XmlElement eventNode = FindOrCreateChildNodeWithAttrValue(subRoot, EVENT, NAME, eventInfo.Name);

                //get the visibility of this type and set it to the attribute 'visibility'.
                eventNode.SetAttribute("visibility",
                    ReflectionEngineUtility.GetMethodVisibility(eventInfo.GetAddMethod(true)));

                string modifiers = ReflectionEngineUtility.GetMethodModifiers(eventInfo.GetAddMethod(true));
                if (modifiers != null)
                {
                    eventNode.SetAttribute("modifiers", modifiers);
                }

                //Use docCache to get the inline documentation:
                string documentation = docCache[ReflectionEngineUtility.GetUniqueID(eventInfo)];
                AddDocNode(eventNode, documentation);

                AddAnnotations(eventInfo.GetCustomAttributes(false), eventNode);

                //Add the type of the property
                AddChildXmlElement(eventNode, TYPE, null, eventInfo.EventHandlerType.FullName);
            }
        }

        /// <summary>
        /// Delegate function for Type.GetInterfaces method.
        /// This method does nothing.
        /// </summary>
        /// <param name="type">The Type which should be the base type.</param>
        /// <param name="obj">The Type instance to check.</param>
        /// <returns>Returns true always</returns>
        private static bool InterfaceBaseTypesFilter(Type type, object obj)
        {
            return true;
        }

        /// <summary>
        /// Adds an 'annotation' child to 'node' for each custom attribute.
        /// </summary>
        /// <param name="customAttributes">Array containing attributes</param>
        /// <param name="node">The node to which to add the child annotation node.</param>
        private static void AddAnnotations(object[] customAttributes, XmlElement node)
        {
            //For each attribute, add a <annotation> sub element with value attr.TypeId.ToString() to elem.
            foreach (object customAttribute in customAttributes)
            {
                Attribute attr = (Attribute)customAttribute;
                AddChildXmlElement(node, ANNOTATION, null, attr.TypeId.ToString());
            }
        }

        /// <summary>
        /// Finds or Creates(if not found), a child node of 'parent' with node name as 'childName'
        /// and with an attribute 'attrName' with value 'attrValue'
        /// </summary>
        /// <param name="parent">The parent node</param>
        /// <param name="childName">The name to be given to the child node</param>
        /// <param name="attrName">The name to be given to the attribute of the child node.</param>
        /// <param name="attrValue">The value to be given to the attribute of the child node.</param>
        /// <returns>The child element formed.</returns>
        private static XmlElement FindOrCreateChildNodeWithAttrValue(
            XmlElement parent, string childName, string attrName, string attrValue)
        {
            XmlElement childNode = (XmlElement)parent.SelectSingleNode(childName +
                Helper.GetXpathAttributePredicate(attrName, attrValue));

            if (childNode == null)
            {
                childNode = AddChildXmlElement(parent, childName, attrName, attrValue);
            }

            return childNode;
        }

        /// <summary>
        /// This function can acheive 2 things:
        /// <para>
        /// Add a child node to the parent with node name as 'childName' and an attribute
        /// with name 'attrName' and attribute value 'value'
        /// </para>
        /// <para>
        /// Add a child node to the parent with node name as 'childName' and value 'value'
        /// </para>
        /// <para>Th decision is taken based on whether attrName is null or not.</para>
        /// </summary>
        /// <param name="parent">The parent to which to add the child node</param>
        /// <param name="childName">The node name of the child to be formed.</param>
        /// <param name="attrName">The name of the attribute to be formed.</param>
        /// <param name="value">The value of the attribute to be formed or the value to be given to child node</param>
        /// <returns>The child node so formed.</returns>
        private static XmlElement AddChildXmlElement(XmlElement parent, string childName, string attrName, string value)
        {
            //Create child to insert
            XmlElement childToInsert = parent.OwnerDocument.CreateElement(childName);
            //Must set an attribute of the child node formed with name 'attrName' and value 'value'
            if (attrName != null)
            {
                childToInsert.SetAttribute(attrName, value);
            }
            //Must set the value of the child node formed with value 'value'
            else
            {
                childToInsert.InnerXml = value;
            }

            //Get the child sequence of the parent type as per the xsd
            IList<string> childSequence = xsdElementSequenceMap[parent.Name];
            int childPos = childSequence.IndexOf(childName);

            //Move back from the index found and insert after the appropriate node
            int curIndex;
            for (curIndex = childPos - 1; curIndex >= 0; curIndex--)
            {
                string curNodeName = childSequence[curIndex];
                XmlNodeList childNodes = parent.SelectNodes(curNodeName);
                if (childNodes.Count > 0)
                {
                    //Insert after the previous sibling found
                    parent.InsertAfter(childToInsert, childNodes[childNodes.Count - 1]);
                    break;
                }
            }

            //No previous sibling found. Simply append child to parent
            if (curIndex < 0)
            {
                parent.AppendChild(childToInsert);
            }

            return childToInsert;
        }

        /// <summary>
        /// Adds 'documentation' to the 'doc' child of parent. If doc child is not present, then it is created.
        /// </summary>
        /// <param name="parent">The parent node.</param>
        /// <param name="documentation">the documentation string to add.</param>
        private static void AddDocNode(XmlNode parent, string documentation)
        {
            //No action if documentation is null
            if (documentation == null)
            {
                return;
            }

            XmlNode docNode = parent.SelectSingleNode(DOC);

            //Add doc if it does not exist
            if (docNode == null)
            {
                AddChildXmlElement((XmlElement)parent, DOC, null, documentation);
            }
            else
            {
                //Set the documentation to the docNode
                docNode.InnerXml = documentation;
            }
        }

        /// <summary>
        /// Adds parameter nodes (as per ParamDecl in xsd) to the parent node for all
        /// parameters found in the method node.
        /// </summary>
        /// <param name="parent">The node to which to add the param nodes</param>
        /// <param name="method">The method information.</param>
        private static void AddParameterNodes(XmlNode parent, MethodBase method)
        {
            //Get the method parameters and add param nodes
            ParameterInfo[] paramInfos = method.GetParameters();
            foreach (ParameterInfo paramInfo in paramInfos)
            {
                XmlElement paramNode = parent.OwnerDocument.CreateElement(PARAM);
                paramNode.SetAttribute(NAME, paramInfo.Name);
                paramNode.SetAttribute(TYPEVALUESPEC, paramInfo.ParameterType.FullName);
                paramNode.InnerXml = paramInfo.ParameterType.FullName;
                parent.AppendChild(paramNode);
            }
        }

        /// <summary>
        /// This method finds a child 'method' node which matches the method definition i.e both the name and the
        /// number/type of parameters as per the method information in the methodInfo instance.
        /// If a mtaching method node is not found, then it is created as per the method
        /// information in the methodInfo instance.
        /// </summary>
        /// <param name="parent">The parent in which to find the child method node</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="isIndexer">Whether the method is an indexer.</param>
        /// <returns>The found or the created 'method' node</returns>
        private static XmlElement FindOrCreateMethodNode(XmlElement parent, MethodBase methodInfo, bool isIndexer)
        {
            //The name of the node to be formed
            string childName = isIndexer ? INDEXER : METHOD;
            if (methodInfo is ConstructorInfo)
            {
                childName = CONSTRUCTOR;
            }

            //The value of its name attribute
            string methodName = isIndexer ? methodInfo.Name.Remove(0, 4) : methodInfo.Name;

            XmlElement childNode = (XmlElement)parent.SelectSingleNode(childName +
                Helper.GetXpathAttributePredicate(NAME, methodName));

            //No method with name as methodInfo.Name exists
            if (childNode == null)
            {
                childNode = AddChildXmlElement(parent, childName, NAME, methodName);
            }
            //Method with name as methodInfo.Name exists but it may be an overload
            else
            {
                XmlNodeList paramNodes = childNode.SelectNodes("param");
                ParameterInfo[] methodParams = methodInfo.GetParameters();

                //Diferent number of parameters so it is an overload.
                if (paramNodes.Count != methodInfo.GetParameters().Length)
                {
                    childNode = AddChildXmlElement(parent, childName, NAME, methodName);
                }
                //Same number of parameters but it may still be an overload.
                else
                {
                    bool isSameOverload = true;
                    for (int i = 0; i < paramNodes.Count; i++)
                    {
                        //Each paramter type must be the same.
                        if (paramNodes[i].InnerXml != methodParams[i].ParameterType.Name)
                        {
                            isSameOverload = false;
                            break;
                        }
                    }

                    //Atleast one of the parameters did not match
                    if (!isSameOverload)
                    {
                        childNode = AddChildXmlElement(parent, childName, NAME, methodName);
                    }
                }
            }

            return childNode;
        }

        /// <summary>
        /// This dictionary holds the ordered list of child nodes of elements in xsd.
        /// It is used by the AddChildXmlElement function.
        /// This is helpful when adding child nodes to a node, so that the child node is added in the correct position.
        /// 
        /// For ex. take class which is of ConstructedType. Please refer to the apispec.xsd file.
        /// The list for class will hold 'doc', 'annotation', 'genericParam' and 'parent' in order.
        /// Note that 'constructor', 'destructor' etc are ignored because they should always
        /// be appended to the class node and they can appear in any order
        /// (because they are in xsd:sequence , xsd:choice combination)
        /// </summary>
        private static IDictionary<string, IList<string>> xsdElementSequenceMap =
            new Dictionary<string, IList<string>>();

        /// <summary>
        /// Prepares the xsdElementSequenceMap map.
        /// This function basically iterates through all xsd:element types in the xsd
        /// and stores their child nodes as defined by xsd:sequence.
        /// This is acheived using the StoreSequenceForElement method which is recursive in nature.
        /// </summary>
        /// <remarks>
        /// This method is not totally generic i.e. it would not apply all xsd types as it is tailor made
        /// for some of the rules observed in apispec.xsd
        /// </remarks>
        private static void PrepareXsdElementsMap()
        {
            XmlSchema apiSpecXsd = Helper.GetApispecSchema();

            //Get the xsd:element nodes in the xsd
            XmlSchemaObjectTable xsot = apiSpecXsd.Elements;

            //For each such node recursively create and store their child sequences.
            foreach (XmlQualifiedName elementName in xsot.Names)
            {
                XmlSchemaElement schemaElem = (XmlSchemaElement)xsot[elementName];
                StoreSequenceForElement(schemaElem);
            }
        }

        /// <summary>
        /// This method preapres the list of child nodes of a complex xsd element with sequences.
        /// The sequences are processed by the ProcessSequence method.
        /// Since the sequence can contain more elements of complex type which need to be stored,
        /// the ProcessSequence fucntion calls this method for processing these complex elements.
        /// Thus these 2 methods are dependently recursive
        /// </summary>
        /// <param name="schemaElem">The xsd element for which to prepare the list.</param>
        private static void StoreSequenceForElement(XmlSchemaElement schemaElem)
        {
            //If already read return
            if (xsdElementSequenceMap.ContainsKey(schemaElem.QualifiedName.Name))
            {
                return;
            }

            //We are interested only in elements having complex content and a sequence
            if (!(schemaElem.ElementSchemaType is XmlSchemaComplexType))
            {
                return;
            }
            XmlSchemaComplexType elemSchemaType = (XmlSchemaComplexType)schemaElem.ElementSchemaType;
            //Get the sequence if present in the complex type
            if (elemSchemaType.Particle == null || !(elemSchemaType.Particle is XmlSchemaSequence))
            {
                return;
            }
            XmlSchemaSequence elemSchemaSeq = (XmlSchemaSequence)elemSchemaType.Particle;
            
            //Prepare the list of child nodes of the sequence and store
            IList<string> sequenceData = ProcessSequence(elemSchemaSeq);
            xsdElementSequenceMap[schemaElem.QualifiedName.Name] = sequenceData;
        }

        /// <summary>
        /// Process an xsd:sequence and returns a list of the child node names of the sequence.
        /// It also process nested sequences which may be present in it using the same method.
        /// More so there may be more elements of complex types in this sequence so they are
        /// processed by the StoreSequenceForElement method.
        /// </summary>
        /// 
        /// <param name="elemSchemaSeq">The xsd:sequence to process.</param>
        /// <returns>A list of child node names for the sequence</returns>
        /// <remarks>
        /// This method is not totally generic i.e. it would not apply all xsd types as it is tailor made
        /// for some of the rules observed in apispec.xsd.
        /// </remarks>
        private static IList<string> ProcessSequence(XmlSchemaSequence elemSchemaSeq)
        {
            IList<string> sequenceList = new List<string>();
            foreach (XmlSchemaObject xso in elemSchemaSeq.Items)
            {
                //Child is and xsd:element. Add it to list and it may need further processing
                if (xso is XmlSchemaElement)
                {
                    XmlSchemaElement childSchemaElem = (XmlSchemaElement)xso;
                    sequenceList.Add(childSchemaElem.QualifiedName.Name);
                    StoreSequenceForElement(childSchemaElem);
                }
                //Child is a nested sequence. Process it recursively and append to the current node names.
                else if (xso is XmlSchemaSequence)
                {
                    XmlSchemaSequence childSchemaSeq = (XmlSchemaSequence)xso;
                    IList<string> childSequenceList = ProcessSequence(childSchemaSeq);

                    //Add to sequenceList
                    foreach (string elementQualName in childSequenceList)
                    {
                        sequenceList.Add(elementQualName);
                    }
                }
                //Child is a choice. Though these node names don't need to be added to the list,
                //but the contained xsd:element and xsd:sequence might need processing.
                else if (xso is XmlSchemaChoice)
                {
                    XmlSchemaChoice choice = (XmlSchemaChoice)xso;
                    foreach (XmlSchemaObject choiceChild in choice.Items)
                    {
                        if (choiceChild is XmlSchemaElement)
                        {
                            StoreSequenceForElement((XmlSchemaElement)choiceChild);
                        }
                        else if (choiceChild is XmlSchemaSequence)
                        {
                            ProcessSequence((XmlSchemaSequence)choiceChild);
                        }
                    }
                }
            }

            return sequenceList;
        }

    }
}
