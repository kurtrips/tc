// SecurityIdDetails.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para> This class represents the security id details, it contains security id and security id type attributes.
    /// Its type can be any value defined in the SecurityIdType, but it's not limited to those as we can have more
    /// security id types in the future.</para>
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is immutable and thread-safe.
    /// </threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class SecurityIdDetails
    {
        /// <summary>
        /// <para>Represents the security id. It is initialized in the constructor, and never changed afterwards. It has
        /// property-getter to access it. It must be non-null, non-empty string.</para>
        /// </summary>
        private readonly string id;

        /// <summary>
        /// <para>Represents the security id type. It is initialized in the constructor, and never changed afterwards.
        /// It has property-getter to access it. It must be non-null, non-empty string.</para>
        /// </summary>
        private readonly string type;

        /// <summary>
        /// <para>Represents the property getter for the security id</para>
        /// </summary>
        /// <value>Represents the security id.</value>
        public string Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// <para>Represents the property getter for the security id type</para>
        /// </summary>
        /// <value>Represents the security id type.</value>
        public string Type
        {
            get
            {
                return type;
            }
        }


        /// <summary>
        /// <para>Constructor with the id and type.</para>
        /// </summary>
        /// <param name="id">the security id.</param>
        /// <param name="type">the security id type.</param>
        /// <exception cref="ArgumentNullException">if any given argument is null.</exception>
        /// <exception cref="ArgumentNullException">if any given argument is empty.</exception>
        public SecurityIdDetails(string id, string type)
        {
            //Validate
            Helper.ValidateNotNullNotEmpty(id, "id");
            Helper.ValidateNotNullNotEmpty(type, "type");

            //Assign
            this.id = id;
            this.type = type;
        }

    }
}
