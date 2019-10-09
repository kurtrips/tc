// XmlMessageTypeDetector .cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using TopCoder.Util.DataValidator;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Util.ObjectFactory;
using TopCoder.MSMQ.MessageProcessingWorkflow;

namespace TopCoder.MSMQ.MessageProcessingWorkflow.Detectors
{
    /// <summary>
    /// <p>This is a specific detector for Xml format based messages. It can be initialized
    /// through configuration or directly by passing the validator, type detection map, and type recipe map into the
    /// constructor.</p>
    /// <p><strong>Thread-Safety:</strong></p><p>This is thread safe as it is immutable.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class XmlMessageTypeDetector : IMessageTypeDetector
    {
        /// <summary>
        /// The pluggable format of the values to be stored in typeDetectionDataMap.
        /// </summary>
        private const string XPATH_VALUES_FORMAT = "XPath={0}::XPathValue={1}";

        /// <summary>
        /// The values to extract from the detector configuration.
        /// </summary>
        private static readonly string[] VALUES_TO_EXTRACT
            = { "xpath", "xpath_value", "type_recipe_file", "validator" };

        /// <summary>
        /// The in-order bool values indicating whether the array contents of VALUES_TO_EXTRACT are required or not.
        /// </summary>
        private static readonly bool[] REQUIRED_VALUES = { true, true, true, false };

        /// <summary>
        /// <p>This is a mapping of a configured message type to the actual parsing
        /// recipe. The recipe are a simple XmlDocument and are directly loaded from a file as configured in
        /// configuration manager or passed in as a specific attribute.
        /// Cannot hold null values.</p>
        /// </summary>
        private readonly IDictionary<string, XmlDocument> typeToRecipeMap;

        /// <summary>
        /// <p>This is a mapping of message types to the actual parsing data used to
        /// map the type dynamically from an input string. Each &lt;value&gt; is a type name and each &lt;key&gt; is
        /// stored as XPath::XPathValue</p>
        /// <p>These are then used against the input xml to try to match
        /// the input string in the GetMessageType method.</p>
        /// </summary>
        private readonly IDictionary<string, string> typeDetectionDataMap;

        /// <summary>
        /// <p>This is a simple mapping of a message type name to an IValidator
        /// instance. Value cannot be null, also key cannot be an empty string.
        /// Since validators are optional this map can be empty but it cannot be null.</p>
        /// </summary>
        private readonly IDictionary<string, IValidator> validatorMap;

        /// <summary>
        /// <p>Create an instance of this class based on the input configuration namespace.</p>
        /// </summary>
        /// <exception cref="ArgumentNullException">If nameSpace is null</exception>
        /// <exception cref="ArgumentException">If nameSpace is empty</exception>
        /// <exception cref="ConfigurationException">If any issue is met when reading configuration for creating
        /// the instance</exception>
        /// <param name="nameSpace">configuration namespace for configuration manager.</param>
        public XmlMessageTypeDetector(string nameSpace)
        {
            Helper.ValidateNotNullNotEmpty(nameSpace, "nameSpace");

            //Initialize dictionaries
            typeToRecipeMap = new Dictionary<string, XmlDocument>();
            typeDetectionDataMap = new Dictionary<string, string>();
            validatorMap = new Dictionary<string, IValidator>();

            //Read config
            try
            {
                //Get the namespace describing the detector
                ConfigManager configMgr = ConfigManager.GetInstance();

                Namespace ns = configMgr.GetNamespace(nameSpace);
                if (ns == null)
                {
                    throw new ConfigurationException("Namespace: " + nameSpace + " is not found");
                }
                Property[] typesInfo = ns.Properties;

                foreach (Property typeInfo in typesInfo)
                {
                    //Read values and add to the correct dictionaries.
                    string[] parsedValues = ParseFromValuesArray(typeInfo.Values);

                    //Check for duplicate type names
                    if (typeDetectionDataMap.ContainsKey(parsedValues[1]))
                    {
                        //Two types of the same name must not be present
                        throw new ConfigurationException("Configuration for detectors contains repeated type: "
                            + parsedValues[1]);
                    }
                    else
                    {
                        //Add to typeDetectionDataMap
                        typeDetectionDataMap.Add(parsedValues[1],
                            String.Format(XPATH_VALUES_FORMAT, parsedValues[0], parsedValues[1]));
                    }

                    //Add to typeToRecipeMap
                    try
                    {
                        XmlDocument recipeFile = new XmlDocument();
                        recipeFile.Load(parsedValues[2]);
                        typeToRecipeMap.Add(parsedValues[1], recipeFile);
                    }
                    catch (Exception e)
                    {
                        throw new ConfigurationException("Could not load recipe file: " + parsedValues[2], e);
                    }

                    //Add to validatorMap if validator was found
                    if (parsedValues[3] != null)
                    {
                        //Validator must be defined in config with namespace [nameSpace].[ValidatorName]
                        //For ex. TopCoder.MSMQ.MessageProcessingWorkflow.MockValidator
                        IValidator validator = ObjectFactory.GetDefaultObjectFactory().CreateDefinedObject(
                            parsedValues[3]) as IValidator;
                        if (validator == null)
                        {
                            throw new ConfigurationException("Could not load validator: " + parsedValues[3]);
                        }
                        validatorMap.Add(parsedValues[1], validator);
                    }
                }
            }
            catch (ConfigurationException ce)
            {
                throw ce;
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Unable to construct XmlMessageTypeDetector from configuration.", e);
            }
        }

        /// <summary>
        /// This is a direct initialization of variable data.
        /// The dictionary must have the following 3 entries:
        /// <para>
        /// Value: Generic Dictionary with key as string and value as XmlDocument.
        /// Key: "recipe-map"
        /// Constraints: It must not be null, must not be empty, must not contain null values or empty/null keys.
        /// The value of inner dictionary is the recipe file and key must be the message type name
        /// </para>
        /// <para>
        /// Value: Generic Dictionary with key as string and value as string.
        /// Key: "detection-data-map"
        /// Constraints: It must not be null, must not be empty, must not contain null/empty values or empty/null keys.
        /// The value of inner dictionary must be a string in format [Xpath]::[XPath_Value] and the key
        /// must be the message type name.
        /// </para>
        /// <para>
        /// Value: Generic Dictionary with key as string and value as IValidator.
        /// Key: "validator-map"
        /// Constraints: It can be null, but if not null the must not be empty, must not contain null elements or
        /// empty/null keys.
        /// The value of inner dictionary must be an IValidator instance and the key must be the message type name.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">If configParameters is null</exception>
        /// <exception cref="ArgumentException">If any of the above mentioned validations fail.</exception>
        /// <param name="configParameters">a collection or configuration properties.</param>
        public XmlMessageTypeDetector(IDictionary<string, IDictionary> configParameters)
        {
            Helper.ValidateNotNull(configParameters, "configParameters");

            //recipeMap
            IDictionary<string, XmlDocument> recipeMap;
            try
            {
                recipeMap = (IDictionary<string, XmlDocument>) configParameters["recipe-map"];
            }
            catch (Exception ex)
            {
                throw new ArgumentException("recipe-map dictionary is either null or has incorrect type.", ex);
            }
            //Validate the collection
            Helper.ValidateCollection<XmlDocument>(recipeMap, "recipe-map");
            //Create shallow copy
            this.typeToRecipeMap = new Dictionary<string, XmlDocument>(recipeMap);

            //detectionDataMap
            IDictionary<string, string> detectionDataMap;
            try
            {
                detectionDataMap = (IDictionary<string, string>) configParameters["detection-data-map"];
            }
            catch (Exception ex)
            {
                throw new ArgumentException("detection-data-map dictionary is either null or has incorrect type.", ex);
            }
            //Validate the collection
            Helper.ValidateCollection<string>(detectionDataMap, "detection-data-map");
            //Create shallow copy
            this.typeDetectionDataMap = new Dictionary<string, string>(detectionDataMap);

            //validatorMap
            IDictionary<string, IValidator> validatorMap;
            try
            {
                validatorMap = (IDictionary<string, IValidator>) configParameters["validator-map"];
            }
            catch (Exception ex)
            {
                throw new ArgumentException("validator-map dictionary has incorrect type.", ex);
            }
            if (validatorMap == null)
            {
                this.validatorMap = null;
            }
            else
            {
                //Validate
                Helper.ValidateCollection<IValidator>(validatorMap, "validator-map");
                //Create shallow copy
                this.validatorMap = new Dictionary<string, IValidator>(validatorMap);
            }
        }

        /// <summary>
        /// <p>This is a direct initialization of variable data.</p>
        /// <para>
        /// typeToRecipeMap: Generic Dictionary with key as string and value as XmlDocument.
        /// Constraints: It must not be null, must not be empty, must not contain null values or empty/null keys.
        /// The value is the recipe file and key must be the message type name.
        /// </para>
        /// <para>
        /// typeDetectionDataMap: Generic Dictionary with key as string and value as string.
        /// Constraints: It must not be null, must not be empty, must not contain null/empty values or empty/null keys.
        /// The value must be a string in format [Xpath]::[XPath_Value] and the key
        /// must be the message type name.
        /// </para>
        /// <para>
        /// validatorMap: Generic Dictionary with key as string and value as IValidator.
        /// Constraints: It can be null, but if not null the must not be empty, must not contain null elements or
        /// empty/null keys.
        /// The value must be an IValidator instance and the key must be the message type name.
        /// </para>
        /// </summary>
        /// <param name="typeToRecipeMap">
        /// This is a mapping of a configured message type to the actual parsing recipie.
        /// </param>
        /// <param name="typeDetectionDataMap">
        /// This is a mapping of message types to the actual parsing data used to map the type dynamically from an input
        /// string.
        /// </param>
        /// <param name="validatorMap">
        /// This is a simple mapping of a message type name to an IValidator instance.
        /// </param>
        /// <exception cref="ArgumentNullException">If typeToRecipeMap or typeDetectionDataMap is null.</exception>
        /// <exception cref="ArgumentException">If any of the other constraints mentioned above fail.</exception>
        public XmlMessageTypeDetector(IDictionary<string, XmlDocument> typeToRecipeMap,
                                      IDictionary<string, string> typeDetectionDataMap,
                                      IDictionary<string, IValidator> validatorMap)
        {
            Helper.ValidateNotNull(typeToRecipeMap, "typeToRecipeMap");
            Helper.ValidateNotNull(typeDetectionDataMap, "typeDetectionDataMap");

            //Validate
            Helper.ValidateCollection<XmlDocument>(typeToRecipeMap, "typeToRecipeMap");
            Helper.ValidateCollection<string>(typeDetectionDataMap, "typeDetectionDataMap");

            //Shallow Copies
            this.typeToRecipeMap = new Dictionary<string, XmlDocument>(typeToRecipeMap);
            this.typeDetectionDataMap = new Dictionary<string, string>(typeDetectionDataMap);

            if (validatorMap == null)
            {
                this.validatorMap = null;
            }
            else
            {
                //Validate
                Helper.ValidateCollection<IValidator>(validatorMap, "validatorMap");
                //shallow copy
                this.validatorMap = new Dictionary<string, IValidator>(validatorMap);
            }
        }

        /// <summary>
        /// <p>This method specifies a contract for matching a message type (string)
        /// to a specific input message (string)</p>
        /// <para>
        /// The message type is found by loading the messageText into an Xml,
        /// walking through the typeDetectionDataMap and running each "Xpath" query on the xml
        /// and checking if the return value is equal to the "XPathValue".
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">If the input is null.</exception>
        /// <exception cref="ArgumentException">If the input is empty string.</exception>
        /// <exception cref="UnknownMessageTypeException">If the message type cannot be found.</exception>
        /// <exception cref="UnknownMessageFormatException">If the input is not valid xml.</exception>
        /// <param name="messageText">The actual message for which we want to get the type.</param>
        /// <returns>The actual type of the message.</returns>
        public string GetMessageType(string messageText)
        {
            Helper.ValidateNotNullNotEmpty(messageText, "messageText");
            //Load into XmlDocument
            XmlDocument doc = Helper.LoadXmlDocument(messageText, "messageText");

            //Match xpath results with xpath value.
            string foundType = null;
            foreach (string key in typeDetectionDataMap.Keys)
            {
                //Get the xpath and xpath_value pair
                string pair = typeDetectionDataMap[key];

                //Parse string selimited by ::
                int indexOfTwoColons = pair.IndexOf("::");
                string[] extracted = new string[2];
                extracted[0] = pair.Substring(0, indexOfTwoColons);
                extracted[1] = pair.Substring(indexOfTwoColons + 2);

                XmlNode foundNode = doc.SelectSingleNode(extracted[0].Remove(0, VALUES_TO_EXTRACT[0].Length + 1));
                if (foundNode != null &&
                    foundNode.Value.Trim().Equals(extracted[1].Remove(0, VALUES_TO_EXTRACT[1].Length)))
                {
                    //If xpath_value and type parsed from message are equal, then the message type is found.
                    foundType = foundNode.Value.Trim();
                    break;
                }
            }

            if (foundType == null)
            {
                throw new UnknownMessageTypeException("Could not find Message type from xml string.");
            }
            else
            {
                return foundType;
            }
        }

        /// <summary>
        /// <p>This method returns an XmlDocument representing the specific instance of parsing
        /// recipe for the specific message type. The recipe file for the message type is returned.</p>
        /// </summary>
        /// <param name="messageType">message type</param>
        /// <returns>
        /// XmlDocument representing the parsing information needed by the parser to parse messages of this type.
        /// </returns>
        /// <exception cref="ArgumentNullException">If messageType is null</exception>
        /// <exception cref="ArgumentException">If messageType is empty</exception>
        /// <exception cref="UnknownMessageTypeException">If no message type with given name is found.</exception>
        public object GetMessageTypeParseRecipe(string messageType)
        {
            Helper.ValidateNotNullNotEmpty(messageType, "messageType");

            if (typeToRecipeMap.ContainsKey(messageType))
            {
                return typeToRecipeMap[messageType];
            }
            throw new UnknownMessageTypeException("Could not find message type: " + messageType);
        }

        /// <summary>
        /// <p>This method returns a validator available for the specific message type.
        /// Callers would then use such a validator to validate the specific message.</p>
        /// </summary>
        /// <param name="messageType">the actual type of the message.</param>
        /// <returns>a validator for this message type if one is set; null otherwise</returns>
        /// <exception cref="ArgumentNullException">If messageType is null</exception>
        /// <exception cref="ArgumentException">If messageType is empty</exception>
        public IValidator GetValidator(string messageType)
        {
            //Validate
            Helper.ValidateNotNullNotEmpty(messageType, "messageType");

            //Get validator
            IValidator validator = null;

            //ValidatorMap may itself be null.
            if (validatorMap != null)
            {
                validatorMap.TryGetValue(messageType, out validator);
            }
            return validator;
        }

        /// <summary>
        /// <p>This is a convenient method which allows users to query this
        /// detector to see if it can handle the message (i.e. if it can handle its type)</p>
        /// </summary>
        /// <exception cref="ArgumentNullException">If the input is null.</exception>
        /// <exception cref="ArgumentException">If the input is empty string.</exception>
        /// <exception cref="UnknownMessageFormatException">If the input is not valid xml.</exception>
        /// <param name="message">The actual message for which we want to get the type.</param>
        /// <returns>true if message can be handled; false otherwise</returns>
        public bool CanHandleMessage(string message)
        {
            Helper.ValidateNotNullNotEmpty(message, "message");

            try
            {
                GetMessageType(message);
                return true;
            }
            catch (UnknownMessageFormatException umfe)
            {
                throw umfe;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// <para>Parses the "xpath", "xpath_value", "type_recipe_file", "validator" keys for the specified message type
        /// from a ConfigManager Value array specified in the configuration file under the
        /// <b>
        /// &lt;namespace name="type.detector.[Type Config Name]"&gt; / &lt;property name="message_type_[Type Name]"&gt;
        /// </b>
        /// hierarchy.</para>
        /// </summary>
        /// <param name="array">An array of ConfigManager Values containing the keys mentioned above.</param>
        /// <returns>The parsed values for the keys mentioned above.</returns>
        /// <exception cref="ConfigurationException">
        /// If key/value is in incorrect format or one of the required keys was not found.
        /// </exception>
        private static string[] ParseFromValuesArray(string[] array)
        {
            bool[] found = new bool[VALUES_TO_EXTRACT.Length];
            string[] parsedValues = new string[VALUES_TO_EXTRACT.Length];

            //Extract "xpath", "xpath_value", "type_recipe_file", "validator" for the specified message type.
            for (int i = 0; i < array.Length; i++)
            {
                int indexOfEqual = array[i].IndexOf("=");

                //Check format (Must have an equal sign and the equal sign must not be at the left edge of the string)
                if (indexOfEqual <= 0 || indexOfEqual == array[i].Length - 1)
                {
                    throw new ConfigurationException(array[i] + " value is in incorrect format.");
                }

                //Parse value
                string[] pair = new string[2];
                pair[0] = array[i].Substring(0, indexOfEqual);
                pair[1] = array[i].Substring(indexOfEqual + 1);
                for (int j = 0; j < VALUES_TO_EXTRACT.Length; j++)
                {
                    if (pair[0].Trim().Equals(VALUES_TO_EXTRACT[j]))
                    {
                        parsedValues[j] = pair[1];
                        found[j] = true;
                    }
                }
            }

            //Throw exception if one of the required values is not found.
            for (int i = 0; i < VALUES_TO_EXTRACT.Length; i++)
            {
                if (REQUIRED_VALUES[i] && !found[i])
                {
                    throw new ConfigurationException(VALUES_TO_EXTRACT[i] + " value is not found.");
                }
            }

            return parsedValues;
        }

    }
}
