// IDigester.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p> This is a contract for creating a digest based on input byte data.</p>
    /// <p><b>Thread Safety: </b>All digester implementations must be thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IDigester
    {
        /// <summary>
        /// <p>Computes a hash code (digest) for the input stream.</p>
        /// </summary>
        /// <exception cref="DigesterException">If there is an issue with producing the digest.</exception>
        /// <exception cref="ArgumentNullException">If any passed parameter is null</exception>
        /// <param name="inputData">digest input byte array</param>
        /// <returns>digested hash as a byte array</returns>
        byte[] Digest(byte[] inputData);

    }
}
