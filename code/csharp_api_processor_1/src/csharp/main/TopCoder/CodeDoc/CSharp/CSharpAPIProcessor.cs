// CSharpAPIProcessor.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Xml;
using TopCoder.XML.CmdLineProcessor;
using TopCoder.CodeDoc.CSharp.Reflection;
using TopCoder.LoggingWrapper;
using System.Reflection;
using System.Globalization;

namespace TopCoder.CodeDoc.CSharp
{
    /// <summary>
    /// This class implements IXmlProcessor and represents a CSharp API processor which analyzes a given set of
    /// assemblies, modules, namespaces or types and then writes the API metadata along with the inline documentation to
    /// the given XmlDocument instance.
    /// <para>The general processing sequence is as follows: The ProcessDocument method is
    /// called each time to explore the API metadata using reflection. In order to keep current AppDomain clean, a new
    /// AppDomain named 'ReflectionEngine' is created and finally unloaded by the time the ProcessDocument method exits.
    /// A ReflectionEngine instance is created with the referenced ReflectionEngineParameters instance in the new
    /// Domain. And all the assemblies to be analyzed are also loaded in the new AppDomain. A MarshalByRef logger is
    /// created when possible and passed to the new Domain so that the logging is acutally performed in the
    /// current domain. Finally when the process is finished, all the assemblies in the new domain are unloaded. And the
    /// current AppDomain keeps clean.</para>
    /// <para>Instances of this processor can be created by CSharpAPIProcessorFactory.</para>
    /// </summary>
    ///
    /// <threadsafety>
    /// <para>The class is immutable but not thread-safe, because the
    /// referenced ReflectionEngineParameters instance is mutable.</para>
    /// </threadsafety>
    ///
    /// <author>urtks</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class CSharpAPIProcessor : IXmlProcessor
    {
        /// <summary>
        /// <para>Represents the ReflectionEngineParameters instance that contains the parameters with which to create
        /// the new ReflectionEngine instance. Initialized in the constructor. Never changed afterwards. Can never be
        /// null.</para>
        /// </summary>
        private readonly ReflectionEngineParameters rep;

        /// <summary>
        /// <para>Creates a new instance of CSharpAPIProcessor with the given ReflectionEngineParameters instance used
        /// to create the ReflectionEngine.</para>
        /// </summary>
        /// <param name="rep">the ReflectionEngineParameters instance used to create the ReflectionEngine.</param>
        /// <exception cref="ArgumentNullException">if rep is null.</exception>
        public CSharpAPIProcessor(ReflectionEngineParameters rep)
        {
            Helper.ValidateNotNull(rep, "rep");
            this.rep = rep;
        }

        /// <summary>
        /// <para>This method generates the CSharp API documentation to the given XmlDocument instance. The API
        /// documentation is merged into the XmlDocument instance so that any additional information in the XmlDocument
        /// is kept unchanged and the overlapped information is updated.</para>
        /// <para>The general processing sequence is as follows: The ProcessDocument method is called each time
        /// to explore the API metadata using reflection. In order to keep current AppDomain clean, a new AppDomain
        /// named 'ReflectionEngine' is created and finally unloaded by the time the ProcessDocument method exits.
        /// A ReflectionEngine instance is created with the
        /// referenced ReflectionEngineParameters instance in the new Domain. And all the assemblies to be analyzed are
        /// also loaded in the new AppDomain. A MarshalByRef logger is created when possible and passed to the new
        /// Domain so that the logging is acutally performed in the current domain. Finally when the process is
        /// finished, all the assemblies in the new domain are unloaded. And the current AppDomain keeps clean.</para>
        /// </summary>
        /// <param name="apiSpec">the given XmlDocument instance to which the CSharp API documentation is generated.
        /// This XmlDocument should follow the apiSpec.xsd file given by this component.</param>
        /// <exception cref="ArgumentNullException">if apiSpec is null.</exception>
        /// <exception cref="ArgumentException">if the root element of apiSpec is not 'apispec'</exception>
        /// <exception cref="XmlProcessorException">if anything else goes wrong.</exception>
        public void ProcessDocument(XmlDocument apiSpec)
        {
            Helper.ValidateNotNull(apiSpec, "apiSpec");
            //throw exception if root node is not apiSpec
            if (apiSpec.DocumentElement == null || apiSpec.DocumentElement.Name != "apispec")
            {
                throw new ArgumentException("apiSpec must have root node with name 'apispec'", "apiSpec");
            }

            AppDomain appDomain = null;
            try
            {
                MBRLogger mbrLogger = null;
                if (rep.LoggerNamespace != null)
                {
                    Logger logger = LogManager.CreateLogger(rep.LoggerNamespace);
                    mbrLogger = new MBRLogger(logger);
                }

                //Create new app domain
                appDomain = AppDomain.CreateDomain("ReflectionEngine", AppDomain.CurrentDomain.Evidence,
                    AppDomain.CurrentDomain.SetupInformation);

                //Create the ReflectionEngine instance in the new AppDomain
                object[] createInstanceArgs = mbrLogger == null ? new object[0] : new object[] { mbrLogger };
                ReflectionEngine re = (ReflectionEngine)appDomain.CreateInstanceAndUnwrap(
                    typeof(ReflectionEngine).Assembly.FullName, typeof(ReflectionEngine).FullName, false,
                    BindingFlags.Public | BindingFlags.Instance, null, createInstanceArgs,
                    CultureInfo.InvariantCulture, new object[0], AppDomain.CurrentDomain.Evidence);

                //Process the APIs
                string newSpec = re.WriteAPISpec(rep, apiSpec.InnerXml);

                //Load the newly formed API specification in the xml document.
                apiSpec.LoadXml(newSpec);
            }
            catch (Exception e)
            {
                throw new XmlProcessorException("Unable to generate CSharp API documentation.", e);
            }
            finally
            {
                if (appDomain != null)
                {
                    AppDomain.Unload(appDomain);
                }
            }
        }

    }
}
