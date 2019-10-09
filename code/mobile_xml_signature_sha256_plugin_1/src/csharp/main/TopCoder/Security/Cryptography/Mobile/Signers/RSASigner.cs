// RSASigner.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Text;
using System.Xml;
using System.Security.Cryptography;

namespace TopCoder.Security.Cryptography.Mobile.Signers
{
    /// <summary>
    /// <p>This is a specific ISigner implementation, which uses RSA asymmetric encryption algorithms for
    /// signing</p>
    /// <p><b>Thread Safety: </b>This implementation is thread-safe since it has no mutable state.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class RSASigner : ISigner
    {
        /// <summary>
        /// The signing exception message.
        /// </summary>
        private const string SIGN_EX_MSG = "Data could not be signed. Please see inner exception for more details";

        /// <summary>
        /// Error message if verification of signature failed.
        /// </summary>
        private const string SIGN_VERIF_FAILED =
            "Verification failed. Either the document itself or the signature value is tampered";

        /// <summary>
        /// Represents the default XML digital signature namespace
        /// </summary>
        private const string DEF_XMLDSIG_NS = "http://www.w3.org/2000/09/xmldsig#";

        /// <summary>
        /// <p>Represents a private key for the RSA part of this signer.This is manipulated through a dedicated
        /// property. Used in calculation of the RSA signature.</p>
        /// </summary>
        private RSAParameters rsaKeyInfo;

        /// <summary>
        /// <p>Represents the property of the currently use RSAKeyInfo variable.</p>
        /// </summary>
        /// <value>The RSA key info to use for signing</value>
        public RSAParameters RSAKeyInfo
        {
            get
            {
                return rsaKeyInfo;
            }
            set
            {
                rsaKeyInfo = value;
            }
        }

        /// <summary>
        /// <p>Creates a new instance of this class.</p>
        /// </summary>
        public RSASigner()
        {
        }

        /// <summary>
        /// <p>Computes the signature of the data provided.</p>
        /// </summary>
        /// <exception cref="ArgumentNullException">if the input is null</exception>
        /// <exception cref="SigningException">If signing fails.</exception>
        /// <param name="data">The text of the SignedInfo to be digested and signed</param>
        /// <returns>digital signature</returns>
        public string Sign(byte[] data)
        {
            ExceptionHelper.ValidateNotNull(data , "data");

            try
            {
                //This signer uses a SHA1 digest algorithm
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();

                //Digest
                byte[] digested = sha.ComputeHash(data);

                //Create a new instance of RSACryptoServiceProvider.
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                //Import the key information.
                rsa.ImportParameters(RSAKeyInfo);

                //Sign the hash
                byte[] signedHash = rsa.SignHash(digested, CryptoConfig.MapNameToOID("SHA1"));

                return Convert.ToBase64String(signedHash);
            }
            catch (Exception ex)
            {
                throw new SigningException(SIGN_EX_MSG, ex);
            }
        }

        /// <summary>
        /// <para>Used to verify the signature produced from the algorithm used by this signer</para>
        /// </summary>
        /// <param name="canonicalized">The canonicalized string from which to produce the digest from</param>
        /// <param name="keyInfoInst">Holds the public key for verification of signature</param>
        /// <param name="node">The Signature node from which to extract the Signature value</param>
        /// <exception cref="VerificationFailedException">If the signature verification failed
        /// i.e. the signatures did not match</exception>
        public void Verify(string canonicalized, IKeyInfoProvider keyInfoInst, XmlNode node)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            //Calculate digest of SignedInfoNode.
            byte[] digested = sha1.ComputeHash(Encoding.UTF8.GetBytes(canonicalized));

            // we use namespace manager since <Signature> has default namespace
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(node.OwnerDocument.NameTable);
            nsMgr.AddNamespace("ds", DEF_XMLDSIG_NS);

            //Verify Sign
            XmlNode signedValNode = node.SelectSingleNode("ds:SignatureValue", nsMgr);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            //Import public key parameter
            rsa.ImportParameters((RSAParameters)keyInfoInst.PublicKey);

            //Verify Sign
            if (rsa.VerifyHash(digested, CryptoConfig.MapNameToOID("SHA1"), 
                Convert.FromBase64String(signedValNode.InnerXml)) == false)
            {
                throw new VerificationFailedException(SIGN_VERIF_FAILED);
            }
        }
    }
}
