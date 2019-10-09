// ITransformer.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is a contract for creating a transformation, which will accept input data (either binary or text) and
    /// which will then transform this data in some predictable way.</p>
    /// <p><b>Thread Safety: </b>All future transformer implementations must be thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ITransformer
    {

        /// <summary><p>Transforms the input data in some meanner into the output data.</p></summary>
        /// <param name="data">byte data to be transformed</param>
        /// <returns>the transformed data</returns>
        /// <exception cref="TransformerException">If there were any issues during execution of the Transform method.
        /// </exception>
        /// <exception cref="ArgumentNullException">If any input is null</exception>
        byte[] Transform(byte[] data);

    }
}
