// DummyKeyInfoProvider.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.

using System;
using System.Text;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is a dummy implementation of the ITransformer interface</p>
    /// </summary>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class DummyTransformer : ITransformer
    {
        /// <summary><p>Transforms the input data in some manner into the output data.
        /// This implementation simply replaces all space characters to ? characters.
        /// </p></summary>
        /// <param name="data">byte data to be transformed</param>
        /// <returns>the transformed data</returns>
        /// <exception cref="TransformerException">If there were any issues during execution of the Transform method.
        /// </exception>
        /// <exception cref="ArgumentNullException">If any input is null</exception>
        public byte[] Transform(byte[] data)
        {
            ExceptionHelper.ValidateNotNull(data, "data");

            try
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if ((byte)(data.GetValue(i)) == (byte)(' '))
                    {
                        data.SetValue((byte)('?'), i);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new TransformerException("Could not transform", ex);
            }
        }
    }
}
