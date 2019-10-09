// IReference.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is a contract for a value object, which contains all the necessary information about how a reference is
    /// structured (or to be structured) within a signature. The structure is quite simple: A reference has a URI
    /// (required) associated with it as well as a set of transformers (optional), which are ordered, to be used in
    /// transforming this reference. It also has an associated digester (required) and a protocol describing which 
    /// protocol to use for loading this reference. This will then be passed to the
    /// SignatureManager (as a list of references) Note that the reference is not required to actually hold the specific
    /// instances of the objects that are associated with it (such as Transformers or Digesters) but rather registry
    /// keys.</p>
    /// <p><b>Thread Safety: </b>Implementations must be thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IReference
    {
        /// <summary>
        /// <p>Represents the uri string for this reference. This must always be set and this method should
        /// never return a null or empty.</p>
        /// </summary>
        /// <value>the uri string for this reference</value>
        string ReferenceURI
        {
            get;
        }

        /// <summary>
        /// <p>Represents the transformers to be applied to this reference. Since transformers are optional this can be
        /// an empty (but not null) list. The return list when not empty will contain string where each string
        /// represents a key to be used in creating/fetching the transformer based on some registry.</p>
        /// </summary>
        /// <value>the transformers to be applied to this reference</value>
        IList<InstantiationVO> TransformerInstanceDefinitions
        {
            get;
        }

        /// <summary>
        /// Represents the key and parameter name-value pairs for the digester to use for this reference.
        /// </summary>
        /// <value>the key and parameter name-value pairs for the digester to use for this reference.</value>
        InstantiationVO DigesterInstanceDefinition
        {
            get;
        }

        /// <summary>
        /// The protocol to use for fetching the reference.
        /// </summary>
        /// <value>The protocol to use</value>
        string Protocol
        {
            get;
        }

        /// <summary>
        /// The SoapMessageReferenceLoader instance to use for loading the SoapMessage
        /// </summary>
        IReferenceLoader SoapRefLoader
        {
            get;
        }
    }
}
