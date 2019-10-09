// DummyKeyInfoProvider.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Dummy implementation of IKeyInfoProvider.
    /// This class is for testing purpose only.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class DummyKeyInfoProvider : IKeyInfoProvider
    {
        /// <summary>
        /// The KeyInfoProvider type to use
        /// </summary>
        /// <value>The KeyInfoProvider type to use</value>
        public KeyInfoProviderType Type
        {
            get
            {
                return KeyInfoProviderType.KeyValue;
            }
        }

        /// <summary>
        /// <p>Represents an optional private key used by many signature algorithms (such as DSA for example) Can be
        /// null. Can be any type. </p>
        /// </summary>
        /// <value>an optional private key</value>
        public object PrivateKeyInfo
        {
            get
            {
                return null;
            }
            set
            {
                value = null;
            }
        }

        /// <summary>
        /// Represents public key used for verification
        /// </summary>
        private object publicKey;

        /// <summary>
        /// Represents the RSA public key used for verification
        /// </summary>
        /// <value>the RSA or DSA public key used for verification</value>
        public object PublicKey
        {
            get
            {
                return publicKey;
            }
            set
            {
                publicKey = value;
            }
        }
    }
}
