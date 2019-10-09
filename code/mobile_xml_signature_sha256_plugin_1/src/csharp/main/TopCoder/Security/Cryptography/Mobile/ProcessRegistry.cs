// ProcessRegistry.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using TopCoder.Util.CompactConfigurationManager;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p><strong>Purpose:</strong></p>
    /// <p>This is a registry used to create different objects needed for processing of
    /// signature requests (i.e. either signing or verifying)</p>
    /// <p>This is a registry of processors that can process different aspects of the signing or
    /// verifying main process. This is really nothing more than a configurable dictionary of keys, which acts as a
    /// factory in creating/fetching the required elements. For example when the SignatureManager needs to create a
    /// digest it will ask the registry to fetch it a specific (key based) Digester from the registry.</p> <p>The
    /// registry is initialized through a configuration file and loads up class names, which are, used to (through
    /// reflection) instantiate the specific instances.</p>
    /// <p>Since each class will be instantiated through a default constructor, it will also be the responsibility of
    /// the registry to initialize the instantiated object at the time of caller request for the instance, with property
    /// values passed in via the IDictionary params parameter which we simply use through reflection by using each key in
    /// the dictionary as the name for the setter that we are looking for.</p>
    /// <p><strong>Thread Safety:</strong></p>
    /// <p>Implementation of this class is thread-safe since each object is created on demand and thus this is a
    /// stack-based creation. In addition to that the actual map using in the registry is immutable after being created
    /// so it is a read only based state.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class ProcessRegistry
    {
        /// <summary>
        /// Represents the path of the configuration file to load
        /// </summary>
        private const string CONFIG_FILE_PATH = "../../conf/preload.xml";

        /// <summary>
        /// <p><strong>Represents:</strong></p> <p>This is the default configuration namespace to be used when
        /// instantiating and initializing an instance of registry.</p>
        /// </summary>
        private static string DefaultNamespace = "TopCoder.Security.Cryptography.Mobile";

        /// <summary>
        /// Error message if any instance was not able to be created.
        /// </summary>
        private const string UNABLE_CRT_INST_MSG =
            " instance could not be created. Please see inner exception for more details";

        /// <summary>
        /// Represents the Configuration Manager instance used to load key-class name pairs.
        /// Initialized in constructor and cannot be null.
        /// </summary>
        private ConfigMgr configMgr;

        /// <summary>
        /// <p>Represents a map of keys to class names. What will happen is that on demand when a caller requests an
        /// instance the following will happen:</p>
        /// <p>A new istance of the class is create through reflection. This instance is then populated through
        /// available setters through provided information in the GetInstance which takes in a map of parameters
        /// with their respective names.</p>
        /// <p>This is initialized in the constuctor or through a dedicated setter property.
        /// It can not be null but can be an empty map (default). After constructor, this instance
        /// should not be empty.</p>
        /// </summary>
        private IDictionary<string, string> registeredDefinitionsMap;

        /// <summary>
        /// <p>Represents a map of key info providers types to Object Factory keys Initialized in one of the
        /// constructors and immutable after that. Contents are not readable from outside.
        /// Used to create instances of IKeyInfoProvider, ICanonicalizer, ISigner, ITransformer, IDigester and
        /// IReferenceLoaders</p>
        /// <p>Can not be empty, and can never be null.</p>
        /// </summary>
        /// <value>The map holding the key class name pairs</value>
        /// <exception cref="ArgumentNullException">If value passes in for setting is null</exception>
        public IDictionary<string, string> RegisteredDefinitionsMap
        {
            get
            {
                return registeredDefinitionsMap;
            }
            set
            {
                ExceptionHelper.ValidateNotNull(value, "value");
                registeredDefinitionsMap = value;
            }
        }

        /// <summary>
        /// <p><strong>Purpose:</strong></p> <p>Creates a new instance of the registry with default namespace</p>
        /// </summary>
        /// <exception cref="ConfigurationException">If any required property is missed or property is in invalid
        /// format.</exception>
        public ProcessRegistry() : this(DefaultNamespace)
        {
        }

        /// <summary>
        /// <p><strong>Purpose:</strong></p> <p>Creates an instance of the ProcessRegistry initialized with values from
        /// configuration.</p><p>It loads up the transformer, digester, signer, canonicalizer, keyInfoProvider and
        /// reference_loader data into a dictionary. This dictionary can subsequently be accesses via the
        /// RegisteredDefinitionsMap property.</p>
        /// </summary>
        /// <param name="aNamespace">configuration namespace</param>
        /// <exception cref="ConfigurationException">If any required property is missed or property is in invalid
        /// format.</exception>
        /// <exception cref="ArgumentNullException">If @namespace is null</exception>
        /// <exception cref="ArgumentException">If @namespace is empty</exception>
        public ProcessRegistry(string aNamespace)
        {
            //Validate string
            ExceptionHelper.ValidateStringNotNullNotEmpty(aNamespace, "aNamespace");

            try
            {
                //Initiate Key-ClassName map
                RegisteredDefinitionsMap = new Dictionary<string, string>();

                // In Compact Framework, all relative paths are relative to the root, not the current directory
                // and CodeBase returns the full path, not the URL
                string assemblyFilePath = Assembly.GetExecutingAssembly().GetName().CodeBase;
                int idx = assemblyFilePath.IndexOf("file:///");
                // remove "file:///" in order to make tests run
                if (idx == 0)
                {
                    assemblyFilePath = assemblyFilePath.Substring(8);
                }

                // remove the assembly file name, Path.GetDirectoryName doesn't support URI
                int idx1 = assemblyFilePath.LastIndexOf(Path.DirectorySeparatorChar);
                int idx2 = assemblyFilePath.LastIndexOf(Path.AltDirectorySeparatorChar);
                idx = idx1 < idx2 ? idx2 : idx1;
                assemblyFilePath = assemblyFilePath.Substring(0, idx + 1);

                string configFilePath = assemblyFilePath + CONFIG_FILE_PATH;

                //Create new instance of ConfigManager
                configMgr = new ConfigMgr(configFilePath);

                //Set Digesters
                SetPropertyFromConfig("digesters", aNamespace);

                //Set signers
                SetPropertyFromConfig("signers", aNamespace);

                //Set transformers
                SetPropertyFromConfig("transformers", aNamespace);

                //Set canonicalizers
                SetPropertyFromConfig("canonicalizers", aNamespace);

                //Set reference_loaders
                SetPropertyFromConfig("reference_loaders", aNamespace);

                //Set key_info_providers
                SetPropertyFromConfig("key_info_providers", aNamespace);

                //Set Verifiers
                SetPropertyFromConfig("verifiers", aNamespace);
            }
            catch (Exception ex)
            {
                throw new ConfigurationException("Unable to load properties from Configuration Manager", ex);
            }
        }

        /// <summary>
        /// <p>Creates and returns a new ITransformer instance based on the input key and the parameters map</p>
        /// </summary>
        /// <exception cref="ProcessRegistryException">If the requested instance cannot be cast to ITransformer
        /// or if any reflection related exceptions are raised</exception>
        /// <exception cref="ArgumentNullException">If key or parameters is null</exception>
        /// <exception cref="ArgumentException">If key is empty string</exception>
        /// <param name="key">transformer key</param>
        /// <param name="parameters">parameters</param>
        /// <returns>instance of ITransformer as identified by the key or null if key does not exist in map</returns>
        public ITransformer GetTransformerInstance(string key, IDictionary<string, object> parameters)
        {
            //Validate key and parameters
            ExceptionHelper.ValidateStringNotNullNotEmpty(key, "key");
            ExceptionHelper.ValidateNotNull(parameters, "parameters");

            //If key does not exist, return null
            if (!registeredDefinitionsMap.ContainsKey(key))
            {
                return null;
            }

            try
            {
                //Get type of class to load, from RegisteredDefinitionsMap
                Type typeOfClass = GetTypeOfClass(key);

                //Create instance of class by reflection
                ITransformer ret = (ITransformer)Activator.CreateInstance(typeOfClass);

                //Set properties of the instance created
                SetPropertiesOfInstance(ret, parameters, typeOfClass);

                return ret;
            }
            catch (Exception ex)
            {
                throw new ProcessRegistryException("Transformer" + UNABLE_CRT_INST_MSG, ex);
            }
        }

        /// <summary>
        /// <p>Creates and returns a new IDigester instance based on the input key</p>
        /// </summary>
        /// <remarks>new value</remarks>
        /// <exception cref="ProcessRegistryException">if the requested instance cannot be cast to IDigester
        /// or if any reflection related exceptions are raised</exception>
        /// <exception cref="ArgumentNullException">If key or parameters is null</exception>
        /// <exception cref="ArgumentException">If key is empty string</exception>
        /// <param name="key">digester key</param>
        /// <param name="parameters">parameters for setting properties</param>
        /// <returns>instance of IDigester as identified by the key or null if key does not exist in map</returns>
        public IDigester GetDigesterInstance(string key, IDictionary<string, object> parameters)
        {
            //Validate key and parameters
            ExceptionHelper.ValidateStringNotNullNotEmpty(key, "key");
            ExceptionHelper.ValidateNotNull(parameters, "parameters");

            //If key does not exist, return null
            if (!registeredDefinitionsMap.ContainsKey(key))
            {
                return null;
            }

            try
            {
                //Get type of class to load, from RegisteredDefinitionsMap
                Type typeOfClass = GetTypeOfClass(key);

                //Create instance of class by reflection
                IDigester ret = (IDigester)Activator.CreateInstance(typeOfClass);

                //Set properties of the instance created
                SetPropertiesOfInstance(ret, parameters, typeOfClass);

                return ret;
            }
            catch (Exception ex)
            {
                throw new ProcessRegistryException("Digester" + UNABLE_CRT_INST_MSG, ex);
            }
        }

        /// <summary>
        /// <p>Creates and returns a new ISigner instance based on the input key</p>
        /// </summary>
        /// <exception cref="ProcessRegistryException">if the requested instance cannot be cast to ISigner
        /// or if any reflection related exceptions are raised</exception>
        /// <exception cref="ArgumentNullException">If key or parameters is null</exception>
        /// <exception cref="ArgumentException">If key is empty string</exception>
        /// <param name="key">signer key</param>
        /// <param name="parameters">parameters</param>
        /// <returns>instance of ISigner as identified by the key or null if key does not exist in map</returns>
        public ISigner GetSignerInstance(string key, IDictionary<string, object> parameters)
        {
            //Validate key and parameters
            ExceptionHelper.ValidateStringNotNullNotEmpty(key, "key");
            ExceptionHelper.ValidateNotNull(parameters, "parameters");

            //If key does not exist, return null
            if (!registeredDefinitionsMap.ContainsKey(key))
            {
                return null;
            }

            try
            {
                //Get type of class to load, from RegisteredDefinitionsMap
                Type typeOfClass = GetTypeOfClass(key);

                //Create instance of class by reflection
                ISigner ret = (ISigner)Activator.CreateInstance(typeOfClass);

                //Set properties of the instance created
                SetPropertiesOfInstance(ret, parameters, typeOfClass);

                return ret;
            }
            catch (Exception ex)
            {
                throw new ProcessRegistryException("Signer" + UNABLE_CRT_INST_MSG, ex);
            }
        }

        /// <summary>
        /// <p>Creates and returns a new ICanonicalizer instance based on the input key.</p>
        /// </summary>
        /// <exception cref="ProcessRegistryException">if the requested instance cannot be cast to ICanonicalizer
        /// or if any reflection related exceptions are raised</exception>
        /// <exception cref="ArgumentNullException">If key or parameters is null</exception>
        /// <exception cref="ArgumentException">If key is empty string</exception>
        /// <param name="key">canonicalizer key</param>
        /// <param name="parameters">parameters</param>
        /// <returns>instance of ICanonicalizer as identified by the key or null if key does not exist in map</returns>
        public ICanonicalizer GetCanonicalizerInstance(string key, IDictionary<string, object> parameters)
        {
            //Validate key and parameters
            ExceptionHelper.ValidateStringNotNullNotEmpty(key, "key");
            ExceptionHelper.ValidateNotNull(parameters, "parameters");

            //If key does not exist, return null
            if (!registeredDefinitionsMap.ContainsKey(key))
            {
                return null;
            }

            try
            {
                //Get type of class to load, from RegisteredDefinitionsMap
                Type typeOfClass = GetTypeOfClass(key);

                //Create instance of class by reflection
                ICanonicalizer ret = (ICanonicalizer)Activator.CreateInstance(typeOfClass);

                //Set properties of the instance created
                SetPropertiesOfInstance(ret, parameters, typeOfClass);

                return ret;
            }
            catch (Exception ex)
            {
                throw new ProcessRegistryException("Canonicalizer" + UNABLE_CRT_INST_MSG, ex);
            }
        }

        /// <summary>
        /// <p>Creates and returns a new IReferenceLoader instance based on the input key. The input key is treated
        /// almso like a type which is usually gotten from the protocol that is used to load up the reference.</p>
        /// </summary>
        /// <exception cref="ProcessRegistryException">if the requested instance cannot be cast to IReferenceLoader
        /// or the parameters cannot be set due to reflection issues
        /// </exception>
        /// <exception cref="ArgumentNullException">If key or parameters is null</exception>
        /// <exception cref="ArgumentException">If key is empty string</exception>
        /// <param name="key">reference loader key (protocol type)</param>
        /// <param name="parameters">parameters</param>
        /// <returns>instance of IreferenceLoader as identified by the key or null if key does not exist in map</returns>
        public IReferenceLoader GetReferenceLoaderInstance(string key, IDictionary<string, object> parameters)
        {
            //Validate key and parameters
            ExceptionHelper.ValidateStringNotNullNotEmpty(key, "key");
            ExceptionHelper.ValidateNotNull(parameters, "parameters");

            //If key does not exist, return null
            if (!registeredDefinitionsMap.ContainsKey(key))
            {
                return null;
            }

            try
            {
                //Get type of class to load, from RegisteredDefinitionsMap
                Type typeOfClass = GetTypeOfClass(key);

                //Create instance of class by reflection
                IReferenceLoader ret = (IReferenceLoader)Activator.CreateInstance(typeOfClass);

                //Set properties of the instance created
                SetPropertiesOfInstance(ret, parameters, typeOfClass);

                return ret;
            }
            catch (Exception ex)
            {
                throw new ProcessRegistryException("Reference Loader" + UNABLE_CRT_INST_MSG, ex);
            }
        }

        /// <summary>
        /// <p>Creates and returns a new IKeyInfoProvider instance based on the input key.</p>
        /// </summary>
        /// <exception cref="ProcessRegistryException">if the requested instance cannot be cast to IKeyInfoProvider
        /// or if any reflection related exceptions are raised</exception>
        /// <exception cref="ArgumentNullException">If key or parameters is null</exception>
        /// <exception cref="ArgumentException">If key is empty string</exception>
        /// <param name="key">key info provider key (type)</param>
        /// <param name="parameters">parameters</param>
        /// <returns>instance of IKeyInfoProvider as identified by the key or null if key does not exist in map</returns>
        public IKeyInfoProvider GetKeyInfoProviderInstance(string key, IDictionary<string, object> parameters)
        {
            //Validate key and parameters
            ExceptionHelper.ValidateStringNotNullNotEmpty(key, "key");
            ExceptionHelper.ValidateNotNull(parameters, "parameters");

            //If key does not exist, return null
            if (!registeredDefinitionsMap.ContainsKey(key))
            {
                return null;
            }

            try
            {
                //Get type of class to load, from RegisteredDefinitionsMap
                Type typeOfClass = GetTypeOfClass(key);

                //Create instance of class by reflection
                IKeyInfoProvider ret = (IKeyInfoProvider)Activator.CreateInstance(typeOfClass);

                //Set properties of the instance created
                SetPropertiesOfInstance(ret, parameters, typeOfClass);

                return ret;
            }
            catch (Exception ex)
            {
                throw new ProcessRegistryException("KeyInfoProvider" + UNABLE_CRT_INST_MSG, ex);
            }
        }

        #region "Private Helper functions"

        /// <summary>
        /// Returns the Type of class name as identified by the key in the map.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>Type of class as identified by the key in the map.</returns>
        /// <exception cref="ArgumentNullException">Raised from GetType function</exception>
        /// <exception cref="ArgumentException">Raised from GetType function</exception>
        /// <exception cref="TypeLoadException">Raised from GetType function</exception>
        /// <exception cref="TargetInvocationException">Raised from GetType function</exception>
        /// <remarks>The above exceptions are caught in the calling function and are rethrown as
        /// ArgumentException</remarks>
        private Type GetTypeOfClass(string key)
        {
            string className = registeredDefinitionsMap[key];
            try
            {
                Type typeofClass = Type.GetType(className, true);
                return typeofClass;
            }
            catch (TypeLoadException)
            {
                Assembly assembly = Assembly.Load(className.Substring(0, className.LastIndexOf('.')));
                Type typeofClass = assembly.GetType(className, true);
                return typeofClass;
            }
        }

        /// <summary>
        /// Sets the public properties of class instance using key of dictionary as property name
        /// and value of dictionary as property value
        /// </summary>
        /// <param name="instance">The instance of the class for which to set the properties</param>
        /// <param name="parameters">The dictionary from which to set the properties</param>
        /// <param name="typeOfClass">The type of the class</param>
        /// <exception cref="MethodAccessException">Raised from SetValue function of PropertyInfo</exception>
        /// <exception cref="ArgumentException">Raised from SetValue function of PropertyInfo</exception>
        /// <exception cref="TargetException">Raised from SetValue function of PropertyInfo</exception>
        /// <exception cref="TargetParameterCountException">Raised from SetValue function of PropertyInfo</exception>
        /// <exception cref="ArgumentNullException">If property with given name does not exist in the class</exception>
        /// <remarks>The above exceptions are caught in the calling function and are rethrown as
        /// ProcessRegistry Exception</remarks>
        private void SetPropertiesOfInstance(object instance, IDictionary<string, object> parameters,
            Type typeOfClass)
        {
            foreach (string paramKey in parameters.Keys)
            {
                //Get propertyInfo for property with name as paramKey
                PropertyInfo propInfo = typeOfClass.GetProperty(paramKey);

                //Validate propInfo is not null before proceeding
                ExceptionHelper.ValidateNotNull(propInfo, "propInfo");

                //Set the value of the property
                propInfo.SetValue(instance, parameters[paramKey], null);
            }
        }

        /// <summary>
        /// Initializes registeredDefinitionsMap using property and namespace name in config file
        /// </summary>
        /// <param name="name">the name of property in the config file</param>
        /// <param name="namespace">the namespace under which the property appears in the config file</param>
        /// <exception cref="ConfigurationException">If name is required and not present in config file
        /// or if format of property value in config file is not correct.</exception>
        private void SetPropertyFromConfig(string name, string @namespace)
        {
            try
            {
                string[] propertyVals = configMgr.GetValues(@namespace, name);

                //Check if property is present if it is required
                if (IsRequiredPropertyAndNonNull(propertyVals, name))
                {
                    foreach (string propertyVal in propertyVals)
                    {
                        string[] keyClassPair = propertyVal.Split(new char[] { ',' });

                        //Check if each value is the valid Key,Classname format
                        if (keyClassPair.Length != 2)
                        {
                            throw new ConfigurationException("Each value in property " + name +
                                " must be in the format [Key,ClassName].");
                        }

                        //Add key class name pair to map only if both values are non empty and non null
                        ExceptionHelper.ValidateStringNotNullNotEmptyArg(keyClassPair[0] , "Key Name");
                        ExceptionHelper.ValidateStringNotNullNotEmptyArg(keyClassPair[1] , "Key Value");
                        registeredDefinitionsMap.Add(keyClassPair[0], keyClassPair[1]);
                    }
                }
            }
            catch (ConfigurationException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw new ConfigurationException("Invalid Configuration File parameters.", ex);
            }
        }

        /// <summary>
        /// Checks if required properties are present in the config file and are non null
        /// </summary>
        /// <param name="properties">An array of properties corresponding to a particular property name</param>
        /// <param name="name">The name of the property</param>
        /// <returns>true if property is non null</returns>
        /// <exception cref="ConfigurationException">If the property is required but is null</exception>
        private bool IsRequiredPropertyAndNonNull(string[] properties, string name)
        {
            //Check if the required properties are present in config file
            if (name == "digesters" || name == "signers" || name == "canonicalizers"
                || name == "reference_loaders" || name == "key_info_providers")
            {
                if (properties == null)
                {
                    throw new ConfigurationException("There must be at least one property with name "
                        + name + " in config file.");
                }
                return true;
            }
            //Checks for optional properties
            else
            {
                if (properties == null)
                {
                    return false;
                }
                return true;
            }
        }

        #endregion
    }
}
