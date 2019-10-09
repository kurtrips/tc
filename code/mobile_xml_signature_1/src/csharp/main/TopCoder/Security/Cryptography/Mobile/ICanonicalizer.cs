// ICanonicalizer.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Text;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is a contract for creating a canonical form from some input data. Different data streams with the same
    /// XML  information set may have different textual representations, e.g. differing as to white space. To help
    /// prevent inaccurate  verification results, XML information sets must first be canonized (standardized) before
    /// extracting their bit representation  for signature processing. Each canonicalizer will also provide its name (or
    /// ID) so that it can be looked up later during a process of for example  verification.</p>
    /// <p><b>Thread Safety: </b>All canonicalizer implementations must be thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ICanonicalizer
    {
        /// <summary>
        /// <p>Represents the encoding used for the canonicalized form. </p>
        /// </summary>
        /// <value>Represents the encoding to use for the canonicalization process</value>
        Encoding Encoding
        {
            get;
        }

        /// <summary>
        /// <p>This method will accept a text string and will modify (if necessary) the string data to bring this string
        /// into a canonical  form. A canonical form is one that has been standardized..</p>
        /// </summary>
        /// <param name="text">string to be canonicalized</param>
        /// <returns>canonicalized string</returns>
        /// <exception cref="CanonicalizationException">there are any issues with this process.</exception>
        /// <exception cref="ArgumentNullException">If any passed parameter is null</exception>
        string BringToCanonicalForm(string text);

    }
}
