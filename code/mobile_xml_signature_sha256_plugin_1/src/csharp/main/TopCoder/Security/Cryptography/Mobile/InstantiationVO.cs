// InstantiationVO.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System.Collections.Generic;
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <strong>Purpose:</strong> <p>This is a simple Value Object class that is used to store the instantiation key for
    /// a class (used in the ProcessRegistry) and the corresponding parameters map used to initialize the resulting
    /// instance object.</p> <p>The basic use of this class will be store infromation about instantiation for some class
    /// instance. Thus the key variable will be used as an identifier of which class we want to get an
    /// instance of, and params will hold the parameters (or properties) that should be initialized with the
    /// provided values.</p> 
    /// <p><strong>Thread-Safety</strong></p> <p>This class is immutable and therefore thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class InstantiationVO
    {
        /// <summary>
        /// <p><strong>Represents:</strong></p> <p>This represents a key of a registered process in the ProcessRegistry.
        /// This is initialized in the constructor and will not change after that.</p> <p>The key will be used as an
        /// identifier into such methods as GetTransformer or GetSigner where the key which identify which process (i.e.
        /// amongst the available transformers or signers for example) should be instantiated.</p>
        /// </summary>
        private string key;

        /// <summary>
        /// <p><strong>Represents:</strong></p> <p>This represents the parameters that will be used to initialize the
        /// state of a process (like a signer or transformer). Each parameter is basically a key,value pair with the key
        /// identifying the Property to use and the value being what should be set in that property. For example if
        /// we have a RSADSASigner which has a DSAKeyInfo property then we would use the DSAKeyInfo as our key and
        /// the actual instance as our value.</p> <p>This is initialized in the constructor and then remains
        /// unchanged.</p>
        /// </summary>
        private IDictionary<string, object> parameters;

        /// <summary>
        /// <p>This is the property getter for the key variable. </p>
        /// </summary>
        /// <value>The key with which to instantiate a class</value>
        public string Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// <p>This is the property getter for the params variable. </p> <p></p>
        /// </summary>
        /// <value>Map containg the name-value pairs with to set the properties of a class</value>
        public IDictionary<string, object> Params
        {
            get
            {
                return parameters;
            }
        }

        /// <summary>
        /// <p><strong>Purpose:</strong></p>  <p>This is the only constructor of this Value Object which initializes the
        /// corresponding parameters to the variables in the class.</p>
        /// </summary>
        /// <param name="key">key for this helper</param>
        /// <param name="parameters">parameters to be used with the key (use to set the object)</param>
        /// <exception cref="ArgumentNullException">If any parameter is null</exception>
        /// <exception cref="ArgumentException">If key is empty. If parameters contains a null reference
        /// or an empty string</exception>
        public InstantiationVO(string key, IDictionary<string, object> parameters)
        {
            //Throw exception if needed
            ExceptionHelper.ValidateStringNotNullNotEmpty(key, "key");
            ExceptionHelper.ValidateInstantiationVOParams(parameters);

            this.key = key;
            this.parameters = parameters;
        }
    }
}
