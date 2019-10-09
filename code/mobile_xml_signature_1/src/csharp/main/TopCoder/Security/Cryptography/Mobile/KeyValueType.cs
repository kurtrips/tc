// KeyValueType.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is a sub-enumeration of Key Values (as defined in KeyInfoProviderType) and lists the two currently
    /// recognized key value types. RSA and DSA.</p>
    /// <p><b>Thread Safety: </b>As an enum it is thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public enum KeyValueType
    {
        /// <summary><p>
        /// Identifies the DSA keys and the DSA signature algorithm.
        /// </p></summary>
        DSAKeyValue,

        /// <summary>
        /// <p>Identifies the DSA keys and the DSA signature algorithm. RSA key values have two fields: Modulus and
        /// Exponent: for example:     <RSAKeyValue>
        /// <Modulus>xA7SEU+e0yQH5rm9kbCDN9o3aPIo7HbP7tX6WOocLZAtNfyxSZDU16ksL6W
        /// jubafOqNEpcwR3RdFsT7bCqnXPBe5ELh5u4VEy19MzxkXRgrMvavzyBpVRgBUwUlV
        /// 5foK5hhmbktQhyNdy/6LpQRhDUDsTvK+g9Ucj47es9AQJ3U=</Modulus><Exponent>AQAB</Exponent>
        /// </RSAKeyValue>  This means that when dealing with this type we expect to be dealing with the above style of
        /// data.   </p>
        /// </summary>
        RSAKeyValue
    }
}
