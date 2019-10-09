// Reference.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is a simple implementation of the IReference interface, which holds information about a reference.
    /// </p>
    /// <p><b>Thread Safety: </b>Implementation is thread-safe as it is immutable.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class Reference : IReference
    {
        /// <summary>
        /// <p>Represents a non-null and non-empty uri string from which to load this reference's contents. This is set
        /// in constructor and is immutable thereafter. </p>
        /// </summary>
        private readonly string referenceURI;

        /// <summary>
        /// <p><strong>Represents:</strong></p>  <p>This is a possibly empty, list ofTransformer InstantiationVO
        /// definitions. Transformers will be used to transform the contents in some way. Note that the order of
        /// transformers is important and transformers will be processed in order from beginning to end of list. This is
        /// set in constructor and is immutable thereafter.</p>  <p>If present this information will be used to create
        /// specific instances of the classes defined in the list.</p>
        /// </summary>
        private readonly IList<InstantiationVO> transformerInstanceDefinitions;

        /// <summary>
        /// <p>Represents, a non-null InstntationVO which contains a digester key (key used to instantiate a digester)
        /// with parameter information for any setters for this reference. Digester will be used to create a hash for
        /// the contents of this reference. This is set in constructor and is immutable thereafter.</p>
        /// </summary>
        private readonly InstantiationVO digesterInstanceDefinition;

        /// <summary>
        /// The protocol to use for fetching the reference
        /// </summary>
        private readonly string protocol;

        /// <summary>
        /// An instance of SoapMessageReferenceLoader used for loading the soap references
        /// </summary>
        private readonly IReferenceLoader soapRefLoader;

        /// <summary>
        /// The protocol to use for fetching the reference.
        /// This is set in constructor and is immutable thereafter.
        /// </summary>
        /// <value>The protocol to use for fetching the reference</value>
        public string Protocol
        {
            get
            {
                return protocol;
            }
        }

        /// <summary>
        /// <p>Represents a getter for the referenceURI member variable. </p>
        /// </summary>
        /// <value>The URI for this reference</value>
        public string ReferenceURI
        {
            get
            {
                return referenceURI;
            }
        }

        /// <summary>
        /// <p>Represents a getter for the transformerInstanceDefinitions member variable.</p>
        /// </summary>
        /// <value>The ordered list of key and parameters used to create transformers for this reference</value>
        public IList<InstantiationVO> TransformerInstanceDefinitions
        {
            get
            {
                return transformerInstanceDefinitions;
            }
        }

        /// <summary>
        /// <p>Represents a getter for the digesterInstanceDefinition member variable.</p>
        /// </summary>
        /// <value>The key and parameters used to create the digester for this reference</value>
        public InstantiationVO DigesterInstanceDefinition
        {
            get
            {
                return digesterInstanceDefinition;
            }
        }

        /// <summary>
        /// The SoapMessageReferenceLoader instance to use for loading the SoapMessage
        /// </summary>
        /// <value>SoapMessageReferenceLoader instance to use for loading the SoapMessage</value>
        public IReferenceLoader SoapRefLoader
        {
            get
            {
                return soapRefLoader;
            }
        }

        /// <summary>
        /// <p>Create a new Reference instance with the input parameters initilaized to member variables.</p>
        /// <para> This constructor should not be used when signing SOAP messages.</para>
        /// </summary>
        /// <exception cref="ArgumentNullException">if any argument is null</exception>
        /// <exception cref="ArgumentException">
        /// if the referenceURI or digesterKey are empty. Also transformerInstanceDefinitions list can
        /// be empty but when not empty it must contain valid non-null/non-empty strings
        /// </exception>
        /// <param name="referenceURI">uri for the reference</param>
        /// <param name="transformerInstanceDefinitions">
        /// a ordered list of keys that identify transformers to use
        /// </param>
        /// <param name="digesterInstanceDefinition">
        /// a single digester instance definition used to fetch/create a corresponding digester
        /// </param>
        /// <param name="protocol">The protocol to use for loading the reference</param>
        public Reference(string referenceURI, IList<InstantiationVO> transformerInstanceDefinitions,
            InstantiationVO digesterInstanceDefinition, string protocol)
        {
            //Check for Null and Check for empty strings
            ExceptionHelper.ValidateNotNull(transformerInstanceDefinitions, "transformerInstanceDefinitions");
            ExceptionHelper.ValidateNotNull(digesterInstanceDefinition, "digesterInstanceDefinition");
            ExceptionHelper.ValidateStringNotNullNotEmpty(protocol, "protocol");
            ExceptionHelper.ValidateStringNotNullNotEmpty(referenceURI, "referenceURI");

            this.referenceURI = referenceURI;
            this.transformerInstanceDefinitions = transformerInstanceDefinitions;
            this.digesterInstanceDefinition = digesterInstanceDefinition;
            this.protocol = protocol;
        }

        /// <summary>
        /// This constructor should be used when signing SOAP messages.
        /// Create a new Reference instance with the input parameters initilaized to member variables.
        /// </summary>
        /// <param name="referenceURI">uri for the reference</param>
        /// <param name="transformerInstanceDefinitions">
        /// a ordered list of keys that identify transformers to use</param>
        /// <param name="digesterInstanceDefinition">
        /// a single digester instance definition used to fetch/create a corresponding digester</param>
        /// <param name="soapRefLoader">The SOAPMessageReferenceLoader to use for loading the data of soapMessage
        /// </param>
        public Reference(string referenceURI, IList<InstantiationVO> transformerInstanceDefinitions,
            InstantiationVO digesterInstanceDefinition, IReferenceLoader soapRefLoader) : this(referenceURI,
            transformerInstanceDefinitions, digesterInstanceDefinition, "soap")
        {
            ExceptionHelper.ValidateNotNull(soapRefLoader, "soapRefLoader");
            this.soapRefLoader = soapRefLoader;
        }
    }
}
