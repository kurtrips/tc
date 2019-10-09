// ISigner.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Xml;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is a contract for a signer, which will take input data of bytes and will sign this data (usually a digest
    /// followed by signing followed by base64 encoding to fit it into an xml string) Usually a signer needs a
    /// private key to be able to sign the content of the data, but this contract doesn’t limit the application  of
    /// generating a signature to key based only.</p>
    /// <p><b>Thread Safety: </b>All future signer implementations must be thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ISigner
    {

        /// <summary>
        /// <p>This is a contracts that specifies how an array of bytes is signed into a resulting string. On many
        /// occasions the string must be Base64Encoded. </p>
        /// </summary>
        /// <param name="data">data to be signed</param>
        /// <returns>the actual digital signature</returns>
        string Sign(byte[] data);

        /// <summary>
        /// <para>Used to verify the signature produced from the algorithm used by this signer</para>
        /// </summary>
        /// <param name="canonicalized">The canonicalized string from which to produce the digest from</param>
        /// <param name="keyInfoInst">Holds the public key for verification of signature</param>
        /// <param name="node">The Signature node from which to extract the Signature value</param>
        void Verify(string canonicalized, IKeyInfoProvider keyInfoInst, XmlNode node);
    }
}
