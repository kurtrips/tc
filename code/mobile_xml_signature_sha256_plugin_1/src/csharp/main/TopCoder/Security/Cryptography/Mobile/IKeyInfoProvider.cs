// IKeyInfoProvider.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Security.Cryptography;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This processor provides key info data for verification of signatures. Thus we could implement for
    /// example an X509CerificateKeyInfoProvider, which would be able to provide certificate (public key) information.
    /// Please note that unlike other processors such as Digesters or Transformers a KeyInfoProvider cannot be an
    /// arbitrary type of processor. It must be one of the enums in KeyInfoProviderType.</p>
    /// <p><b>Thread Safety: </b>Implementations must be thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IKeyInfoProvider
    {
        /// <summary>
        /// <p>Represents the type that this provider represnts. For example we could have a provider of X509Data type
        /// which would mean that we are dealing with a certificate based key for signature verification. </p>
        /// </summary>
        /// <value>the type that this provider represnts.</value>
        KeyInfoProviderType Type
        {
            get;
        }

        /// <summary>
        /// <p>Represents an optional private key used by many signature algorithms (such as DSA for example) Can be
        /// null. Can be any type. </p>
        /// </summary>
        /// <value>optional private key used by many signature algorithms</value>
        object PrivateKeyInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the RSA or DSA public key used for verification
        /// </summary>
        /// <value>the RSA or DSA public key used for verification</value>
        object PublicKey
        {
            get;
            set;
        }

    }
}
