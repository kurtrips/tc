// SHA1Digester.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Security.Cryptography;

namespace TopCoder.Security.Cryptography.Mobile.Digesters
{
    /// <summary>
    /// <p>This is an implementation of IDigester interface and provides a SHA-1 digester which provides a 160 bit
    /// digest from the input data.</p>
    /// <p><b>Thread Safety: </b>This is a thread-safe implementation, as it has no mutable state.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class SHA1Digester : IDigester
    {
        /// <summary>
        /// Digest exception message
        /// </summary>
        private const string DIGEST_EX_MSG = "Digest could not be calculated. Please see inner exception for details";

        /// <summary>
        /// <p>a default no-op constructor</p>
        /// </summary>
        public  SHA1Digester()
        {
        }

        /// <summary>
        /// <p>Computes a SHA-1 digest based on the data in the input stream. </p>
        /// </summary>
        /// <param name="inputData">digest input byte array</param>
        /// <returns>digested hash as a byte array</returns>
        /// <exception cref="DigesterException">
        /// If there is an issue with producing the digest such as due to IO
        /// </exception>
        /// <exception cref="ArgumentNullException">If any parameter is null</exception>
        public byte[] Digest(byte[] inputData)
        {
            ExceptionHelper.ValidateNotNull(inputData,"inputData");

            try
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                return sha.ComputeHash(inputData);
            }
            catch (Exception ex)
            {
                throw new DigesterException(DIGEST_EX_MSG, ex);
            }
        }

    }
}
