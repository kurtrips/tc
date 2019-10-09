// KeyInfoProviderType.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is an enumeration used to specify available key info provider types that are recognized by this
    /// component.  These are: KeyName, KeyValue, RetrievalMethod, X509Data, PGPData, SKIPData, and MgmtData. This will
    /// used by the  SignatureManager (actually by ISigner implementations) to identify the correct type of provider for
    /// the specific needs at  hand.</p>  
    /// <p>KeyInfo is an optional element that enables the recipient(s) to obtain the key needed to validate 
    /// the signature. KeyInfo may contain keys, names, certificates and other public key management information, 
    /// such as in-band key  distribution or key agreement data. This specification defines a few simple types but 
    /// applications may extend those types  or all together replace them with their own key identification and exchange 
    /// semantics using the XML namespace facility. </p>
    /// <p><b>Thread Safety: </b>As an enum it is thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public enum KeyInfoProviderType
    {
        /// <summary>
        /// <p>The KeyName element contains a string value (in which white space is significant) which may be used by
        /// the signer to  communicate a key identifier to the recipient. Typically, KeyName contains an identifier
        /// related to the key pair used to  sign the message, but it may contain other protocol-related information
        /// that indirectly identifies a key pair.  (Common uses of KeyName include simple string names for keys, a key
        /// index, a distinguished name (DN), an email address,  etc.)  This type denotes exactly that. </p>
        /// </summary>
        KeyName,

        /// <summary>
        /// <p>The KeyValue element contains a single public key that may be useful in validating the signature The
        /// KeyValue element may include externally defined public keys values represented as PCDATA or element types
        /// from an external namespace.  Structured formats for defining DSA (REQUIRED) and RSA (RECOMMENDED) public
        /// keys are defined (p[lease consult the  KeyValueType enum) </p>
        /// </summary>
        KeyValue,

        /// <summary>
        /// <p>A RetrievalMethod element within KeyInfo is used to convey a reference to KeyInfo information that is
        /// stored at another  location. For example, several signatures in a document might use a key verified by an
        /// X.509v3 certificate chain  appearing once in the document or remotely outside the document; each signature's
        /// KeyInfo can reference this chain  using a single RetrievalMethod element instead of including the entire
        /// chain with a sequence of X509Certificate  elements. </p>
        /// </summary>
        RetrievalMethod,

        /// <summary>
        /// <p>An X509Data element within KeyInfo contains one or more identifiers of keys or X509 certificates (or
        /// certificates'  identifiers or a revocation list). The content of X509Data is:  At least one element, from
        /// the following set of element types; any of these may appear together or more than once  iff (if and only if)
        /// each instance describes or is related to the same certificate:    - The X509IssuerSerial element, which
        /// contains an X.509 issuer distinguished name/serial number pair that      SHOULD be compliant with RFC2253
        /// [LDAP-DN],   - The X509SubjectName element, which contains an X.509 subject distinguished name that SHOULD
        /// be compliant      with RFC2253 [LDAP-DN],   - The X509SKI element, which contains the base64 encoded plain
        /// (i.e. non-DER-encoded) value of a X509     V.3 SubjectKeyIdentifier extension.   - The X509Certificate
        /// element, which contains a base64-encoded [X509v3] certificate, and   - Elements from an external namespace
        /// which accompanies/complements any of the elements above.   - The X509CRL element, which contains a
        /// base64-encoded certificate revocation list (CRL) [X509v3] </p>
        /// </summary>
        X509Data,

        /// <summary>
        /// <p>The PGPData element within KeyInfo is used to convey information related to PGP public key pairs and
        /// signatures  on such keys. The PGPKeyID's value is a base64Binary sequence containing a standard PGP public
        /// key identifier as  defined in [PGP, section 11.2]. The PGPKeyPacket contains a base64-encoded Key Material
        /// Packet as defined in  [PGP, section 5.5]. These children element types can be complemented/extended by
        /// siblings from an external  namespace within PGPData, or PGPData can be replaced all together with an
        /// alternative PGP XML structure as a  child of KeyInfo. PGPData must contain one PGPKeyID and/or one
        /// PGPKeyPacket and 0 or more elements from an  external namespace. </p>
        /// </summary>
        PGPData,

        /// <summary>
        /// <p>The SPKIData element within KeyInfo is used to convey information related to SPKI public key pairs,
        /// certificates and  other SPKI data. SPKISexp is the base64 encoding of a SPKI canonical S-expression.
        /// SPKIData must have at least one  SPKISexp; SPKISexp can be complemented/extended by siblings from an
        /// external namespace within SPKIData, or  SPKIData can be entirely replaced with an alternative SPKI XML
        /// structure as a child of KeyInfo. </p>
        /// </summary>
        SKIPData,

        /// <summary>
        /// <p>The MgmtData element within KeyInfo is a string value used to convey in-band key distribution or
        /// agreement data.  For example, DH key exchange, RSA key encryption, etc. Use of this element is NOT
        /// RECOMMENDED. It provides a  syntactic hook where in-band key distribution or agreement data can be placed.
        /// However, superior interoperable child  elements of KeyInfo for the transmission of encrypted keys and for
        /// key agreement are being specified by the W3C XML  Encryption Working Group and they should be used instead
        /// of MgmtData. .</p>
        /// </summary>
        MgmtData
    }
}
