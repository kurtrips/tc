using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Profile;
using System.DirectoryServices;

namespace TopCoder.Web.Profile.Providers.ActiveDirectory.Configuration
{
    public class ActiveDirectoryProfileProviderSection : ConfigurationSection
    {
        //private static ConfigurationPropertyCollection properties;

        //private static ConfigurationProperty useSingleLevelSearch = new ConfigurationProperty("useSingleLevelSearch", typeof(bool), true);

        //private static ConfigurationProperty rootDirectory = new ConfigurationProperty("rootDirectory", typeof(string), "", ConfigurationPropertyOptions.IsRequired);

        //private static ConfigurationProperty connectionName = new ConfigurationProperty("connectionName", typeof(string), "", ConfigurationPropertyOptions.IsRequired);

        //private static ConfigurationProperty loginName = new ConfigurationProperty("loginName", typeof(string), null);

        //private static ConfigurationProperty loginPassword = new ConfigurationProperty("loginPassword", typeof(string), null);

        //private static ConfigurationProperty authenticationType = new ConfigurationProperty("authenticationType", typeof(string), "None");

        //private static ConfigurationProperty attributeMappings = new ConfigurationProperty("attributeMappings", typeof(AttributeMappingsCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

        public ActiveDirectoryProfileProviderSection()
            : base()
        {
            //attributeMappings.IsDefaultCollection = false;

            //properties = new ConfigurationPropertyCollection();
            //properties.Add(useSingleLevelSearch);
            //properties.Add(rootDirectory);
            //properties.Add(connectionName);
            //properties.Add(loginName);
            //properties.Add(loginPassword);
            //properties.Add(attributeMappings);
        }

        //protected override ConfigurationPropertyCollection Properties
        //{
        //    get
        //    {
        //        return properties;
        //    }
        //}

        [ConfigurationProperty("useSingleLevelSearch", DefaultValue = false, IsRequired = false)]
        public bool UseSingleLevelSearch
        {
            get { return (bool)base["useSingleLevelSearch"]; }
            set { base["useSingleLevelSearch"] = value; }
        }

        [ConfigurationProperty("connectionName", IsRequired = true)]
        //[StringValidator(MinLength = 1)]
        public string ConnectionName
        {
            get { return base["connectionName"] as string; }
        }

        [ConfigurationProperty("loginName", IsRequired = false)]
        public string LoginName
        {
            get { return base["loginName"] as string; }
        }

        [ConfigurationProperty("loginPassword", IsRequired = false)]
        public string LoginPassword
        {
            get { return base["loginPassword"] as string; }
        }

        [ConfigurationProperty("authenticationType", IsRequired = false, DefaultValue = AuthenticationTypes.None)]
        public AuthenticationTypes AuthenticationType
        {
            get { return (AuthenticationTypes)base["authenticationType"]; }
        }

        [ConfigurationProperty("attributeMappings", IsDefaultCollection = false)]
        public AttributeMappingsCollection AttributeMappings
        {
            get
            {
                return base["attributeMappings"] as AttributeMappingsCollection;
            }
        }

        //[ConfigurationProperty("applicationName", IsRequired = true)]
        //public string ApplicationName
        //{
        //    get { return base["applicationName"] as string; }
        //}

        [ConfigurationProperty("userNameAttribute", IsRequired = true)]
        public string UserNameAttribute
        {
            get { return base["userNameAttribute"] as string; }
        }

        [ConfigurationProperty("applicationNameAttribute", IsRequired = true)]
        public string ApplicationNameAttribute
        {
            get { return base["applicationNameAttribute"] as string; }
        }

        
    }

    public class AttributeMappingElement : ConfigurationElement
    {
        public AttributeMappingElement() { }

        [ConfigurationProperty("profileAttribute", IsRequired = true, IsKey = true)]
        //[StringValidator(MinLength=1)]
        public string ProfileAttribute
        {
            get { return this["profileAttribute"] as string; }
            set { this["profileAttribute"] = value; }
        }

        [ConfigurationProperty("activeDirectoryUserAttribute", IsRequired = true)]
        //[StringValidator(MinLength = 1)]
        public string ActiveDirectoryUserAttribute
        {
            get { return this["activeDirectoryUserAttribute"] as string; }
            set { this["activeDirectoryUserAttribute"] = value; }
        }

        [ConfigurationProperty("isGroupedProperty", IsRequired = false, DefaultValue = false)]
        //[StringValidator(MinLength = 1)]
        public bool IsGroupedProperty
        {
            get { return (bool)this["isGroupedProperty"]; }
            set { this["isGroupedProperty"] = value; }
        }
    }

    public class AttributeMappingsCollection : ConfigurationElementCollection
    {
        public AttributeMappingsCollection()
        {
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AttributeMappingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AttributeMappingElement)element).ProfileAttribute;
        }

        //public AttributeMappingElement this[int index] {
        //    get {
        //        return BaseGet(index) as AttributeMappingElement;
        //    }
        //}
    }

}
