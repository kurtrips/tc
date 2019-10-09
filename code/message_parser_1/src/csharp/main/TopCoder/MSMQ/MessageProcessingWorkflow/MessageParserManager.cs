// MessageParserManager .cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Util.ObjectFactory;
using TopCoder.MSMQ.MessageProcessingWorkflow.Parsers;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <p>This is basically a factory to be used when creating configured parsers. User may create a
    /// default parser (which currently is the xml based parser) or fetch a named parser.
    /// User can also ask the factory to refresh the configuration and fetch the latest version of a named parser.</p>
    /// <p>Parser entries are cached so that new requests for already seen parsers are very
    /// efficient.</p>
    /// <p><strong>Thread-Safety:</strong></p> <p>This class is thread-safe as it locks on
    /// all read/write operations involving the parsersMap variable (i.e. the cache)</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MessageParserManager
    {
        /// <summary>
        /// <p>This is the default parser name which is used when the default GetParser() method is called.</p>
        /// <p>This is currently the "XmlMessageParser"</p>
        /// </summary>
        public const string DefaultMessageParserName = "XmlMessageParser";

        /// <summary>
        /// The config namespace from which to read parser names.
        /// </summary>
        private const string DefaultNamespace = "TopCoder.MSMQ.MessageProcessingWorkflow.MessageParserManager";

        /// <summary>
        /// <p>This is a map of names to IMessageParser instances.
        /// Neither the names not values can be null. In addition names cannot be empty.
        /// This is initialized statically (and is modified as such) in the constructor and is them modified as we go
        /// along and we use the GetParser methods. Each time one of the GetParser methods is called it will update this
        /// map if an entry just created/arrived doesn't exist yet in the map. Once this is initialized user cannot
        /// directly access/read this from the outside.</p>
        /// </summary>
        private static readonly IDictionary<string, IMessageParser> parsersMap =
            new Dictionary<string, IMessageParser>();

        /// <summary>
        /// <p>This is a default constructor.</p>
        /// <p>This constructor is private so that no copies of this manager can be created.</p>
        /// </summary>
        private MessageParserManager()
        {
        }

        /// <summary>
        /// <p>This method will create a default parser based on the DefaultMessageParserName.</p>
        /// </summary>
        /// <exception cref="ConfigurationException">This exception wraps any configuration related exception
        /// encountered when creating the parser instance.</exception>
        /// <returns>default parser instance</returns>
        public static IMessageParser GetParser()
        {
            return GetParser(DefaultMessageParserName);
        }

        /// <summary>
        /// <p>This method fetches a named message parser (either from the cache or creates a new one otherwise)</p>
        /// </summary>
        /// <exception cref="ArgumentException">If name is empty.</exception>
        /// <exception cref="ArgumentNullException">If name is null.</exception>
        /// <exception cref="ConfigurationException">This exception wraps any configuration related exception
        /// encountered when creating the parser instance.</exception>
        /// <param name="name">configuration name of the parser to be loaded</param>
        /// <returns>An instance of the named parser</returns>
        public static IMessageParser GetParser(string name)
        {
            Helper.ValidateNotNullNotEmpty(name, "name");

            try
            {
                //Return if already present in cache
                lock (parsersMap)
                {
                    if (parsersMap.ContainsKey(name))
                    {
                        return parsersMap[name];
                    }
                }

                //Read parsers to be created
                ConfigManager configMgr = ConfigManager.GetInstance();
                string[] parsers = configMgr.GetValues(DefaultNamespace, "MessageParsers");

                //Find the correct parser
                bool found = false;
                for (int i = 0; i < parsers.Length; i++)
                {
                    if (parsers[i].Trim().Equals(name.Trim()))
                    {
                        found = true;
                        break;
                    }
                }

                //If not found, throw exception
                if (!found)
                {
                    throw new ConfigurationException("No parser definition found with name: " + name);
                }

                //Create IMessageParser instance using Object Factory
                IMessageParser parserToRet = ObjectFactory.GetDefaultObjectFactory().CreateDefinedObject(name)
                    as IMessageParser;

                //Throw exception if null
                if (parserToRet == null)
                {
                    throw new ConfigurationException("Unable to create parser instance for key: " + name);
                }

                //Update parsersMap
                lock (parsersMap)
                {
                    parsersMap[name] = parserToRet;
                }

                return parserToRet;
            }
            catch (ConfigurationException cfe)
            {
                throw cfe;
            }
            catch (Exception e)
            {
                throw new ConfigurationException("Unable to create parser instance.", e);
            }
        }

        /// <summary>
        /// <p>This is a simple refresh functionality which removes any previously created parser (if it exists)
        /// with the given name and replaces it with a new one. The cache is updated with the newly created parser.</p>
        /// </summary>
        /// <param name="name">
        /// configuration name of the parser for which the configuration data should be re-loaded/re-freshed.
        /// </param>
        /// <exception cref="ArgumentException">If name is empty.</exception>
        /// <exception cref="ArgumentNullException">If name is null.</exception>
        /// <exception cref="ConfigurationException">This exception wraps any configuration related exception
        /// encountered when creating the parser instance.</exception>
        public static void RefreshConfiguration(string name)
        {
            //Validate
            Helper.ValidateNotNullNotEmpty(name, "name");

            lock (parsersMap)
            {
                parsersMap.Remove(name);
            }

            //Get a fresh parser
            GetParser(name);
        }

    }
}
