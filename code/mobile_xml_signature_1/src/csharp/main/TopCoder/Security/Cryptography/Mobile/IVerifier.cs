// IVerifier.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p> This processor is the reverse of the ISigner one. Here we take a signature and verify that it is authentic.
    /// </p>
    /// <p><b>Thread Safety: </b>All future verifier implementations must be thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IVerifier
    {
        /// <summary>
        /// <p>Represents an optional key info provider. In some cases this must be present in some cases it doesn't
        /// have to be. This will be determined on case by case basis. </p>
        /// </summary>
        /// <value>an optional key info provider.</value>
        IKeyInfoProvider KeyInfoProvider
        {
            get;
            set;
        }

        /// <summary><p>This is a contracts that specifies how an array of bytes is verified </p></summary>
        /// <param name="data">data to be verified</param>
        /// <returns>true if signature is fine; false if not</returns>
        bool VerifySignature(byte[] data);

    }
}
