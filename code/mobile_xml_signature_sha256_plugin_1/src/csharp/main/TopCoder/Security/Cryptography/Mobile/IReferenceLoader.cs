// IReferenceLoader.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>Since references might have to be loaded from a number for location and from different formats this interface
    /// represents a contract for such loading. Since all the references will be given in form of a URI it is possible
    /// that in the future  we might need loaders that are quite sophisticated. All loaders will return byte data, which
    /// will then be used to sign the  reference itself. </p>
    /// <p><b>Thread Safety: </b>Implementations are not required to be thread-safe, but it is
    /// an assumption that they will be used in a thread-safe manner (i.e. by a single thread)</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IReferenceLoader
    {
        /// <summary>
        /// <p>Loads the data from the specifed URI. In some cases this might have to be sophisticated as in only
        /// capturing a portion of the document or through authenticated channels only.</p>
        /// </summary>
        /// <param name="uriString">the URI string from which to load the resource</param>
        /// <returns>the resource as an array of bytes</returns>
        /// <exception cref="ReferenceLoadingException">
        /// if there are issues encountered during the loading. This could be due to IO for example.
        /// </exception>
        byte[] LoadReferenceData(string uriString);
    }
}
