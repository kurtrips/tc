// XmlMessageParser.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Util.ObjectFactory;
using TopCoder.MSMQ.MessageProcessingWorkflow.Detectors;
using TopCoder.MSMQ.ConversationManager.Entities;
using TopCoder.MSMQ.ConversationManager.DataAccess;
using TopCoder.Util.DataValidator;

namespace TopCoder.MSMQ.MessageProcessingWorkflow.Parsers
{
    /// <summary>
    /// <para>This is a specific implementation of a parser that specializes with Xml message formats. Here the actual
    /// recipes will all be configured based on Xpath matching.</para>
    /// <p><strong>Thread-Safety:</strong></p>
    /// <p>This implementation is thread-safe as it is immutable.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class XmlMessageParser : IMessageParser
    {
        /// <summary>
        /// A list containing the detectors mapped to this instance of XmlMessageParser
        /// </summary>
        private readonly IList<IMessageTypeDetector> detectorsList;

        /// <summary>
        /// Property in config containing the detectors configured for this instance of XmlMessageParser
        /// </summary>
        private const string DETECTOR_PROPERTY_NAME = "MessageTypeDetectors";

        /// <summary>
        /// Constructor.
        /// Loads the detectors from the given namespace. Stores these detectors for further use.
        /// </summary>
        /// <param name="namespace">The namespace containing the configured detectors.</param>
        /// <exception cref="ConfigurationException">
        /// If detectors are not able to be loaded because of incorrect configuration file format.
        /// </exception>
        public XmlMessageParser(string @namespace)
        {
            Helper.ValidateNotNullNotEmpty(@namespace, "@namespace");
            detectorsList = new List<IMessageTypeDetector>();

            try
            {
                string[] detectors = ConfigManager.GetInstance().GetValues(@namespace, DETECTOR_PROPERTY_NAME);

                //Create detectors and add them to list
                foreach (string detector in detectors)
                {
                    IMessageTypeDetector detectorInst = ObjectFactory.GetDefaultObjectFactory().
                                                        CreateDefinedObject(detector.Trim()) as IMessageTypeDetector;

                    //Detector instance must not be null
                    if (detectorInst == null)
                    {
                        throw new ConfigurationException("Unable to load MessageTypeDetector instance for key: " +
                            detector.Trim());
                    }

                    detectorsList.Add(detectorInst);
                }
            }
            catch (ConfigurationException ce)
            {
                throw ce;
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Unable to load MessageTypeDetector instance.", e);
            }
        }

        /// <summary>
        /// Constructor.
        /// Loads the configParameters into its own copy of detector list.
        /// </summary>
        /// <param name="configParameters">
        /// A list containing the detectors mapped to this instance of XmlMessageParser
        /// </param>
        /// <exception cref="ArgumentException">If configParameters contains null entry.</exception>
        /// <exception cref="ArgumentNullException">If configParameters is itself null.</exception>
        public XmlMessageParser(IList<IMessageTypeDetector> configParameters)
        {
            //Validate
            Helper.ValidateNotNull(configParameters, "configParameters");
            for (int i = 0; i < configParameters.Count; i++)
            {
                if (configParameters[i] == null)
                {
                    throw new ArgumentException("List must not contain null.", "configParameters");
                }
            }

            this.detectorsList = new List<IMessageTypeDetector>(configParameters);
        }

        /// <summary>
        /// This function is actually responsible for parsing the message.
        /// Depending on the message text, it loads the appropriate recipe file and applies the
        /// xpath expressions and other rules on the message and thus creates the Message node.
        /// </summary>
        /// <param name="messageText">The message text. This must be a valid xml</param>
        /// <returns>The Message node created.</returns>
        /// <exception cref="UnknownMessageFormatException">Message is not valid xml.</exception>
        /// <exception cref="ArgumentNullException">If message is null</exception>
        /// <exception cref="ArgumentException">If message is empty</exception>
        /// <exception cref="MessageValidationException">
        /// If one of the classes of DataEntities throws exception at their constructors.
        /// If the type specified in Attribute node either does not exist or the value parsed cannot be converted.
        /// </exception>
        /// <exception cref="MessageParsingException">
        /// If Recipe file has wrong/missing values of certain nodes and attributes.
        /// If Message has wrong/missing values of certain nodes and attributes.
        /// </exception>
        public Message ParseMessage(string messageText)
        {
            Helper.ValidateNotNullNotEmpty(messageText, "messageText");
            XmlDocument doc = Helper.LoadXmlDocument(messageText, "messageText");

            foreach (IMessageTypeDetector detector in detectorsList)
            {
                if (detector.CanHandleMessage(messageText))
                {
                    //Get message type and recipe
                    string messageType = detector.GetMessageType(messageText);
                    XmlDocument recipe = detector.GetMessageTypeParseRecipe(messageType) as XmlDocument;

                    //Get MessageType instance
                    XmlNode messageTypeNode = SelectSingleNode(recipe, "MessageConfig/MessageType", true, "recipe");
                    MessageType objMessageType = CreateMessageType(messageTypeNode, doc);

                    //Get AttributeSet instances
                    XmlNodeList atrributeSetNodes = recipe.SelectNodes("MessageConfig/AttributeSet");
                    IList<AttributeSet> attributeSets = new List<AttributeSet>();
                    foreach (XmlNode attributeSetNode in atrributeSetNodes)
                    {
                        string xpathNodeVal = GetXmlAttributeValue(attributeSetNode, "XPath", true, false);
                        string innerCountVal = GetXmlAttributeValue(attributeSetNode, "InnerCount", true, true);

                        if (innerCountVal.Equals("1"))
                        {
                            attributeSets.Add(CreateAttributeSet(attributeSetNode, xpathNodeVal, doc));
                        }
                        else if (innerCountVal.Equals("*"))
                        {
                            IList<AttributeSet> innerSets = CreateAttributeSets(attributeSetNode, xpathNodeVal, doc);
                            foreach (AttributeSet innerSet in innerSets)
                            {
                                attributeSets.Add(innerSet);
                            }
                        }
                        else
                        {
                            throw new MessageParsingException("Invalid value of InnerCount attribute: " + 
                                innerCountVal);
                        }
                    }
                    // Convert IList<AttributeSet> to array
                    AttributeSet[] attributeSetsArr = new AttributeSet[attributeSets.Count];
                    attributeSets.CopyTo(attributeSetsArr, 0);

                    //Read sourceMessageQueue
                    XmlNode sourceQueueNode = SelectSingleNode(recipe, "MessageConfig/SourceMessageQueue",
                        true, "recipe");
                    MessageQueue sourceMessageQueue = ReadMessageQueue(sourceQueueNode, doc, true);

                    //Read destMessageQueue
                    XmlNode destQueueNode = SelectSingleNode(recipe, "MessageConfig/DestinationMessageQueue",
                        false, "recipe");
                    MessageQueue destMessageQueue = null;
                    if (destQueueNode != null)
                    {
                        destMessageQueue = ReadMessageQueue(destQueueNode, doc, false);
                    }

                    //Create node
                    Message message = new Message(objMessageType, sourceMessageQueue, destMessageQueue, messageText,
                                                  DateTime.Now, attributeSetsArr);

                    //Validate if required
                    IValidator validator = detector.GetValidator(messageType);
                    if (validator != null && !validator.IsValid(message))
                    {
                        throw new MessageValidationException("Output message failed validation");
                    }

                    return message;
                }
            }

            //No detector found capable of handling messageText
            throw new ConfigurationException("No detector found capable of handling message text.");
        }

        /// <summary>
        /// Creates a clone of the parser instance.
        /// </summary>
        /// <returns>A clone of the parser instance.</returns>
        public object Clone()
        {
            //New list for holding detectors
            IList<IMessageTypeDetector> cloneParams = new List<IMessageTypeDetector>(detectorsList);
            return new XmlMessageParser(cloneParams);
        }

        /// <summary>
        /// Reads the message type from message and creates a MessageType instance.
        /// </summary>
        /// <param name="messageTypeNode">The MessageType node in the recipe file.</param>
        /// <param name="message">The message from which to extract the Message type.</param>
        /// <returns>The MessageType instance created</returns>
        /// <exception cref="MessageParsingException">
        /// If xpath attribute in messageTypeNode is not found or is empty string.
        /// If no node is found in the message at the specified xpath in recipe.
        /// </exception>
        /// <exception cref="MessageValidationException">
        /// If MessageType constructor throws exception.
        /// </exception>
        private static MessageType CreateMessageType(XmlNode messageTypeNode, XmlDocument message)
        {
            //Get the xpath expression to run on the message for finding message type.
            string xpathNodeVal = GetXmlAttributeValue(messageTypeNode, "XPath", true, true);

            //Get the node containing the message type information
            XmlNode typeNode = SelectSingleNode(message, xpathNodeVal, true, "message");

            //Get the message type value
            string typeName = typeNode.InnerText.Trim();

            //Create MessageType
            try
            {
                //Create MessageType using persistence.
                ConversationManagerPersistence persistence = new ConversationManagerPersistence();
                return persistence.FindOrCreateMessageType(typeName);
            }
            catch (Exception e)
            {
                throw new MessageValidationException("Could not load MessageType from persistence.", e);
            }
        }

        /// <summary>
        /// Creates an AttributeSet instance for AttributeSet nodes specified in recipe with inner count as 1.
        /// </summary>
        /// <param name="attributeSetNode">The AttributeSet node specified in recipe with inner count as 1.</param>
        /// <param name="xpathNodeVal">The base xpath applied to all child attributes.</param>
        /// <param name="message">The message from which to extract information.</param>
        /// <returns>An AttributeSet instance for the AttributeSet node in recipe</returns>
        /// <exception cref="MessageParsingException">
        /// If XPath attribute of the Attribute node is not found.
        /// If Attribute node is mandatory and no corresponding node is found in message.
        /// If Type of Attribute node is not found or is empty.
        /// </exception>
        /// <exception cref="MessageValidationException">
        /// If MessageAttribute constructor throws exception.
        /// </exception>
        private static AttributeSet CreateAttributeSet(XmlNode attributeSetNode, string xpathNodeVal,
            XmlDocument message)
        {
            //Get all child attribute nodes
            XmlNodeList attributeNodes = attributeSetNode.ChildNodes;

            IList<MessageAttribute> attributes = new List<MessageAttribute>();
            //Create the attributes for the attributeSet
            for (int i = 0; i < attributeNodes.Count; i++)
            {
                //Create MessageAttribute instance
                MessageAttribute createdAttribute = GetMessageAttribute(attributeNodes[i], message, xpathNodeVal);

                //Ignore attributes that were not mandatory and were not found
                if (createdAttribute != null)
                {
                    attributes.Add(createdAttribute);
                }
            }

            //Convert attributes list to array
            MessageAttribute[] attributesArr = new MessageAttribute[attributes.Count];
            attributes.CopyTo(attributesArr, 0);

            return new AttributeSet(attributesArr);
        }

        /// <summary>
        /// Creates a variable number of AttributeSet instances for an AttributeSet node with inner count as *.
        /// </summary>
        /// <param name="attributeSetNode">The AttributeSet node specified in recipe with inner count as 1.</param>
        /// <param name="basePath">The base xpath applied to all child attributes.</param>
        /// <param name="message">The message from which to extract information.</param>
        /// <returns>A list of AttributeSet instances for the AttributeSet node in recipe</returns>
        /// <exception cref="MessageParsingException">
        /// If XPath attribute of Attribute node is missing.
        /// </exception>
        /// <exception cref="MessageValidationException">
        /// If MessageAttribute class throws exception at constructor.
        /// </exception>
        private static IList<AttributeSet> CreateAttributeSets(XmlNode attributeSetNode,
            string basePath, XmlDocument message)
        {
            //Select all child Attribute nodes
            XmlNodeList attributeNodes = attributeSetNode.ChildNodes;

            //Get all the queries to be run. Also get the names of the MessageAttributes to create
            IList<string> xPathQueries = new List<string>();
            IList<string> names = new List<string>();
            foreach (XmlNode attrNode in attributeNodes)
            {
                xPathQueries.Add(basePath + GetXmlAttributeValue(attrNode, "XPath", true, false));
                names.Add(GetXmlAttributeValue(attrNode, "Name", false, false));
            }

            //Get the query results
            IList<XmlNodeList> xPathResults = new List<XmlNodeList>();
            foreach (string query in xPathQueries)
            {
                xPathResults.Add(message.SelectNodes(query));
            }

            //Get least number of AttributeSets required
            int count = int.MaxValue;
            foreach (XmlNodeList nodeList in xPathResults)
            {
                count = Math.Min(nodeList.Count, count);
            }

            //Create the required number of AttributeSets
            IList<AttributeSet> ret = new List<AttributeSet>();
            for (int i = 0; i < count; i++)
            {
                MessageAttribute[] attributes = new MessageAttribute[attributeNodes.Count];
                for (int j = 0; j < attributeNodes.Count; j++)
                {
                    //Exceptions thrown by DataEntities must be caught and rethrown as MessageValidationException
                    try
                    {
                        attributes[j] = new MessageAttribute(names[j], xPathResults[i][j].InnerText, names[j]);
                    }
                    catch (Exception e)
                    {
                        throw new MessageValidationException("Invalid parameters for MessageAttribute constructor.", e);
                    }
                }

                ret.Add(new AttributeSet(attributes));
            }

            return ret;
        }

        /// <summary>
        /// Creates a MessageQueue instance based on a SourceMessageQueue or DestinationMessageQueue node
        /// specified in a recipe file.
        /// </summary>
        /// <param name="messageQueueNode">The node specified in the recipe</param>
        /// <param name="message">The message to read information from.</param>
        /// <param name="isSourceQueue">Whether this method has been called for creating the source queue
        /// or the destination queue.</param>
        /// <returns>A MessageQueue instance read from the message</returns>
        /// <exception cref="MessageParsingException">
        /// If XPath attribute of messageQueueNode is null or empty.
        /// </exception>
        /// <exception cref="MessageValidationException">
        /// If exception is thrown by MessageQueue constructor.
        /// </exception>
        private static MessageQueue ReadMessageQueue(XmlNode messageQueueNode, XmlDocument message, bool isSourceQueue)
        {
            //XPath must be present for SourceQueue as it is not optional
            string xPath = GetXmlAttributeValue(messageQueueNode, "XPath", isSourceQueue, isSourceQueue);

            //Node must be found for SourceQueue as it is not optional
            XmlNode queueNodeFound = SelectSingleNode(message, xPath, isSourceQueue, "message");

            string name = null;
            if (isSourceQueue || (!isSourceQueue && queueNodeFound != null))
            {
                name = queueNodeFound.InnerText.Trim();
            }

            //Throw exception if name is empty and it is a sourceQueue
            if (isSourceQueue && name.Equals(String.Empty))
            {
                throw new MessageParsingException("Name of SourceQueue cannot be empty string.");
            }
            //return null if this is destination queue.
            else if (!isSourceQueue && name.Equals(String.Empty))
            {
                return null;
            }

            //Exceptions thrown by FindMessageQueue must be caught and rethrown as ConfigurationException
            try
            {
                ConversationManagerPersistence persistence = new ConversationManagerPersistence();
                return persistence.FindMessageQueue(name);
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Could not load MessageQueue from persistence", e);
            }
        }

        /// <summary>
        /// Creates a MessageAttribute instance from an Attribute node specified in the recipe.
        /// </summary>
        /// <param name="attributeNode">The Attribute node specified in the recipe.</param>
        /// <param name="message">The message from which to extract information</param>
        /// <param name="basePath">The base xpath to prefix before finding in the message.</param>
        /// <returns>A MessageAttribute instance parsed from the message using
        /// the Attribute node specified in the recipe.</returns>
        /// <exception cref="MessageParsingException">
        /// If XPath attribute of the Attribute node is not found.
        /// If Attribute node is mandatory and no corresponding node is found in message.
        /// If Type of Attribute node is not found or is empty.
        /// </exception>
        /// <exception cref="MessageValidationException">
        /// If MessageAttribute constructor throws exception.
        /// </exception>
        private static MessageAttribute GetMessageAttribute(XmlNode attributeNode, XmlDocument message, string basePath)
        {
            string name = GetXmlAttributeValue(attributeNode, "Name", false, false);
            string xpathNodeVal = GetXmlAttributeValue(attributeNode, "XPath", true, false);

            //Get Mandatory attribute
            bool mandatory = false;
            string mandatoryText = GetXmlAttributeValue(attributeNode, "Mandatory", false, false);
            if (mandatoryText != null && mandatoryText.ToUpper().Trim().Equals("YES"))
            {
                mandatory = true;
            }

            XmlNode valueNode = message.SelectSingleNode(basePath + xpathNodeVal);

            if (valueNode == null)
            {
                //Throw exception if node with given xpath was not found and it was mandatory.
                if (mandatory)
                {
                    throw new MessageParsingException("Mandatory node is not found in message.");
                }
                //Return null if node with given xpath was not found and it was not mandatory.
                else
                {
                    return null;
                }
            }
            string value = valueNode.InnerText.Trim();

            //Get type and validate
            string typeName = GetXmlAttributeValue(attributeNode, "Type", true, true);
            ValidateType(typeName, value);

            //Exceptions thrown by DataEntities must be caught and rethrown as MessageValidationException
            try
            {
                return new MessageAttribute(name, value, name);
            }
            catch (Exception e)
            {
                throw new MessageValidationException("Invalid parameters for MessageAttribute constructor", e);
            }
        }

        /// <summary>
        /// Validates that the value for an Attribute node parsed from message is convertible to the type
        /// mentioned for the Attribute.
        /// </summary>
        /// <param name="typeName">The name of the type as specified in Attribute node.</param>
        /// <param name="value">The string value parsed from the message for the Attribute node.</param>
        /// <exception cref="MessageValidationException">
        /// If specified type does not exist or if the given value is not convertible to the given type.
        /// </exception>
        private static void ValidateType(string typeName, string value)
        {
            //Create type
            Type type;
            type = Type.GetType(typeName);

            //Throw exception if no such type exists
            if (type == null)
            {
                throw new MessageValidationException("No such type exists: " + typeName);
            }

            //Check the value only for ValueType types.
            if (type.IsValueType)
            {
                try
                {
                    Convert.ChangeType(value, type);
                }
                catch (Exception e)
                {
                    throw new MessageValidationException(value + " cannot be converted to type " + typeName, e);
                }
            }
        }

        /// <summary>
        /// Utility function for getting the value of an xml attribute of an XmlNode.
        /// </summary>
        /// <param name="node">The xml node</param>
        /// <param name="name">The xml attribute name</param>
        /// <param name="checkNull">Whether to throw exception if no attribute was found.</param>
        /// <param name="checkEmpty">Whether to throw exception if value of attribute found is empty string.</param>
        /// <returns>The value of the attribute node.</returns>
        /// <exception cref="MessageParsingException">
        /// If checkNull is true and the specified xml attribute is not found.
        /// If checkEmpty is true and the value of the xml attribute is empty string.
        /// </exception>
        private static string GetXmlAttributeValue(XmlNode node, string name, bool checkNull, bool checkEmpty)
        {
            //Get the attribute needed
            XmlNode attrNode = node.Attributes.GetNamedItem(name);
            if (attrNode == null)
            {
                if (checkNull)
                {
                    throw new MessageParsingException(name + "attribute of " + node.Name + " node must be present.");
                }
                return null;
            }

            //Get the value of the attribute.
            string value = attrNode.Value.Trim();
            if (value.Equals(String.Empty) && checkEmpty)
            {
                throw new MessageParsingException(name + "attribute of " + node.Name + " node must not be empty.");
            }
            return value;
        }

        /// <summary>
        /// Utility function for selecting an xml node from an xml document as per the xpath specified.
        /// </summary>
        /// <param name="doc">The xml document in which to search.</param>
        /// <param name="xpath">The xpath with which to search.</param>
        /// <param name="throwOnError">Whether to throw error if no node is found at the specified xpath.</param>
        /// <param name="docName">The name of the xml document in which to search.</param>
        /// <returns>The xml node found.</returns>
        /// <exception cref="MessageParsingException">
        /// If throwOnError is true and if no node is found at the specified xpath.
        /// </exception>
        private static XmlNode SelectSingleNode(XmlDocument doc, string xpath, bool throwOnError, string docName)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            if (node == null && throwOnError)
            {
                throw new MessageParsingException("Could not find any node at " + xpath + " in " + docName);
            }
            return node;
        }
    }
}
