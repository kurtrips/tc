// WebBasedReferenceLoader.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Net;
using System.IO;
using System.Text;
using TopCoder.Security.Cryptography.Mobile;

namespace TopCoder.Security.Cryptography.Mobile.ReferenceLoaders
{
    /// <summary>
    /// <p>This is a simple implementation of the IReferenceLoader interface, which simply loads the whole resource,
    /// specified in the provided URI using the http protocol. Note that currently no
    /// credentials are being set for access to references.</p>
    /// <p><b>Thread Safety: </b>This implementation is not thread-safe. Calling function must lock
    /// the LoadReferenceData method to ensure thread safety.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class WebBasedReferenceLoader : IReferenceLoader
    {
        /// <summary>
        /// Reference Loading exception message
        /// </summary>
        private string REF_LOAD_EX_MSG =
            "Specified reference could not be loaded. Please see inner exception for details";

        /// <summary>
        /// <p>a default no-op constructor</p>
        /// </summary>
        public WebBasedReferenceLoader()
        {
        }

        /// <summary>
        /// <p>Loads the data specified by the input URI using the http protocol.</p>
        /// </summary>
        /// <exception cref="ReferenceLoadingException">
        /// If there are issues encountered during the loading. This could be dues to IO for example.
        /// </exception>
        /// <exception cref="ArgumentNullException">If uri is null</exception>
        /// <param name="uriString">the URI string from which to load the resource</param>
        /// <returns>the resource as an array of bytes</returns>
        public byte[] LoadReferenceData(string uriString)
        {
            ExceptionHelper.ValidateNotNull(uriString, "uriString");

            try
            {
                //Create URI
                Uri uri = new Uri(uriString);
                // create a request
                WebRequest request = WebRequest.Create(uri);

                string responseFromServer;
                // Get the response.
                using (WebResponse response = request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            // Read the content.
                            responseFromServer = reader.ReadToEnd();
                        }
                    }
                }

                // return the results as an array of bytes
                return new UnicodeEncoding().GetBytes(responseFromServer);
            }
            catch (Exception ex)
            {
                throw new ReferenceLoadingException(REF_LOAD_EX_MSG, ex);
            }
        }
    }
}
