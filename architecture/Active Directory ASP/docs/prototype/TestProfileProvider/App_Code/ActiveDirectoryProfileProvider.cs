using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Profile;
using System.DirectoryServices;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TopCoder.Web.Profile.Providers.ActiveDirectory.Configuration;
using System.Runtime.Serialization.Formatters.Binary;

namespace TopCoder.Web.Profile.Providers.ActiveDirectory
{
    /// <summary>
    /// Summary description for ActiveDirectoryProfileProvider
    /// </summary>
    ///  
    public class ActiveDirectoryProfileProvider : ProfileProvider
    {
        public ActiveDirectoryProfileProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private AttributeMappingList attributeList = new AttributeMappingList();
        protected AttributeMappingList AttributeList
        {
            get { return attributeList; }
        }

        private bool singleLevelSearch;
        protected bool SingleLevelSearch
        {
            get { return singleLevelSearch; }
        }

        private string userNameAttribute;
        protected string UserNameAttribute
        {
            get { return userNameAttribute; }
        }

        private string lastActivityDateAttribute;
        protected string LastActivityDateAttribute
        {
            get { return lastUpdateDateAttribute; }
            set { lastUpdateDateAttribute = value; }
        }

        private string lastUpdateDateAttribute;
        protected string LastUpdateDateAttribute
        {
            get { return lastUpdateDateAttribute; }
            set { lastUpdateDateAttribute = value; }
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string ApplicationName
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection ret = new SettingsPropertyValueCollection();

            //Get the user name
            string userName = context["UserName"] as string;

            //Get the DirectoryEntry for the user name
            using (DirectoryEntry userEntry = GetUserEntryByUserName(userName))
            {

                //Get each property
                IEnumerator en = collection.GetEnumerator();
                while (en.MoveNext())
                {
                    //Property to get
                    string propertyToGet = (en.Current as SettingsProperty).Name;

                    //Get the .NET type for the property, change attrValue to the correct type
                    Type detinationType = (en.Current as SettingsProperty).PropertyType;

                    //Get the SerializeAs of the property
                    SettingsSerializeAs serializeAs = (en.Current as SettingsProperty).SerializeAs;

                    //Create a SettingsPropertyValue and for setting its value
                    SettingsPropertyValue spv = new SettingsPropertyValue((en.Current as SettingsProperty));

                    //Build the current property recursively because it might be a complex object.
                    if (serializeAs == SettingsSerializeAs.ProviderSpecific)
                    { spv.PropertyValue = GetPropertyRecursively(userEntry, propertyToGet, detinationType, serializeAs, AttributeList); spv.Deserialized = true; }
                    else
                    { spv.SerializedValue = GetPropertyRecursively(userEntry, propertyToGet, detinationType, serializeAs, AttributeList); spv.Deserialized = false; }

                    //Add the settingsPropertyValue to return collection
                    ret.Add(spv);
                }

                //Set last actvity date
                if (LastActivityDateAttribute != null)
                    userEntry.InvokeSet(LastActivityDateAttribute, DateTime.Now);
            }

            return ret;

        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            //Get the user name
            string userName = context["UserName"] as string;

            //Get the DirectoryEntry for the user name
            using (DirectoryEntry userEntry = GetUserEntryByUserName(userName))
            {

                //Get each property
                IEnumerator en = collection.GetEnumerator();
                while (en.MoveNext())
                {
                    SettingsPropertyValue spv = (en.Current as SettingsPropertyValue);

                    //The name of the property
                    string propertyToSet = spv.Name;

                    //The AttributeMapping associated with the property
                    AttributeMapping am = AttributeList[propertyToSet];
                    //if no such key exists, simply continue;

                    //Set properties recursievly because it might be a complex object
                    SetPropertyRecursively(userEntry, spv.Deserialized ? spv.PropertyValue : spv.SerializedValue, !spv.Deserialized, am);

                }

                //Set last actvity date
                if (LastActivityDateAttribute != null)
                {
                    userEntry.InvokeSet(LastActivityDateAttribute, DateTime.Now);
                }
                //Set last update date
                if (LastUpdateDateAttribute != null)
                {
                    userEntry.InvokeSet(LastUpdateDateAttribute, DateTime.Now);
                }

                //Commit changes
                userEntry.CommitChanges();
            }            
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            ActiveDirectoryProfileProviderSection sec = ConfigurationManager.GetSection("activeDirectoryProvider") as ActiveDirectoryProfileProviderSection;


            //string connName = sec.ConnectionName;
            //connectionString  = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
            //loginName = sec.LoginName;
            //loginPassword = sec.LoginPassword;
            //authType = sec.AuthenticationType;
            //...........

            //Process Attribute Mappings List
            IEnumerator en = sec.AttributeMappings.GetEnumerator();
            IDictionary<string, string> attrMappingsDict = new Dictionary<string, string>();
            IList<string> groupedAttributes = new List<string>();
            while (en.MoveNext())
            {
                AttributeMappingElement elem = (AttributeMappingElement)en.Current;
                attrMappingsDict[elem.ProfileAttribute] = elem.ActiveDirectoryUserAttribute;

                //A grouped property. So no recursion would be required
                if (elem.IsGroupedProperty)
                    groupedAttributes.Add(elem.ProfileAttribute);
            }

            //Recursively process the AttributeMappingList
            this.attributeList = CreateAttributeMappingList(attrMappingsDict, groupedAttributes);

            base.Initialize(name, config);
        }

        private AttributeMappingList CreateAttributeMappingList(IDictionary<string, string> attrMappingsDict, IList<string> groupedAttributes)
        {
            AttributeMappingList ret = new AttributeMappingList();

            //Key is prefix. Value is a genaral mapping dictionary similar to input of method
            Dictionary<string, IDictionary<string, string>> prefixedAttributes = new Dictionary<string, IDictionary<string, string>>();

            //Iterate dictionary
            IEnumerator<KeyValuePair<string, string>> en = attrMappingsDict.GetEnumerator();
            while (en.MoveNext())
            {
                //Complex object with nested properties using the '.' delimiter
                if (en.Current.Key.Contains(".") && !groupedAttributes.Contains(en.Current.Key))
                {
                    string prefix = en.Current.Key.Substring(0, en.Current.Key.IndexOf('.'));

                    //Is this first entry into prefix dictionary?
                    if (!prefixedAttributes.ContainsKey(prefix))
                        prefixedAttributes[prefix] = new Dictionary<string, string>();

                    prefixedAttributes[prefix].Add(en.Current.Key.Remove(0, prefix.Length + 1), en.Current.Value);
                }
                //Simple or grouped object
                else
                {
                    ret.Add(new AttributeMapping(en.Current.Key, en.Current.Value, null));
                }
            }

            //Now recursively process the prefixedAttributes dictionary.
            IEnumerator<KeyValuePair<string, IDictionary<string, string>>> enPrefix = prefixedAttributes.GetEnumerator();
            while (enPrefix.MoveNext())
            {
                //Recursively process the attribute mapping dictionary for current prefix
                AttributeMappingList listForPrefix = CreateAttributeMappingList(enPrefix.Current.Value, groupedAttributes);

                //Create an AttributeMapping with the innerMappingList set.
                ret.Add(new AttributeMapping(enPrefix.Current.Key, null, listForPrefix));
            }

            return ret;
        }

        private DirectoryEntry GetUserEntryByUserName(string userName)
        {
            return new DirectoryEntry();
        }

        private object ConvertObject(object attrValue, Type destinationType)
        {
            //Dummy implementation for designer's testing. Must implement this using impl details in CS.

            if (destinationType.Equals(typeof(string)))
            {
                return "kurtrips";
            }
            else if (destinationType.Equals(typeof(int)))
            {
                return 123;
            }
            else if (destinationType.IsArray)
            {
                return new string[] { "BLE", "Pops" };
            }
            else if (typeof(ICollection).IsAssignableFrom(destinationType))
            {
                System.Collections.Specialized.StringCollection ret = new System.Collections.Specialized.StringCollection();
                ret.Add("kyky");
                return ret;
            }
            else if (destinationType.Equals(typeof(DateTime)))
            {
                return DateTime.Today;
            }


            throw new Exception("Unknown type.");
        }

        private object GetConvertedAttributeValueForUser(DirectoryEntry userEntry, string attributeName, Type destinationType)
        {
            //This call is just for designer's testing. It needs to be removed.
            return ConvertObject(null, destinationType);

            //Get the value of the attribute
            object attrValue = userEntry.InvokeGet(attributeName);
            
            //Convert it into correct type
            object convertedObject = ConvertObject(attrValue, destinationType);
        }

        private object GetPropertyRecursively(DirectoryEntry userEntry, string propertyToGet, Type destinationType,
            SettingsSerializeAs serializeAs, AttributeMappingList attributeListForContext)
        {
            //Get the Active Directory User attribute mapping
            AttributeMapping attrMap = attributeListForContext[propertyToGet];
            //if no such key exists i.e. attrMap is null, simply return null;

            //Primitives, string, simple collections and serialized complex types
            if (destinationType.IsPrimitive || destinationType.Equals(typeof(string)) ||
                typeof(ICollection).IsAssignableFrom(destinationType) || destinationType.IsArray || destinationType.IsValueType)
            {
                //Get value from Active Directory and convert it into correct type
                object convertedObject = GetConvertedAttributeValueForUser(userEntry, attrMap.ActiveDirectoryUserAttribute, destinationType);

                return convertedObject;
            }
            //Complex types
            else
            {
                if (serializeAs != SettingsSerializeAs.ProviderSpecific)
                {
                    if (serializeAs == SettingsSerializeAs.String || serializeAs == SettingsSerializeAs.Xml)
                        return GetConvertedAttributeValueForUser(userEntry, attrMap.ActiveDirectoryUserAttribute, typeof(string));
                    else
                        return GetConvertedAttributeValueForUser(userEntry, attrMap.ActiveDirectoryUserAttribute, typeof(byte[]));
                }
                else
                {
                    //Get the mappings for the inner properties
                    AttributeMappingList innerMappings = attributeListForContext[propertyToGet].InnerMappingList;

                    //Create a new instance of the complex type using Activator.CreateInstance and default constructor.
                    object complexObj = Activator.CreateInstance(destinationType);

                    //Call AD to get value for each inner property
                    foreach (AttributeMapping innerMapping in innerMappings)
                    {
                        //Get the current property
                        PropertyInfo propInfo = destinationType.GetProperty(innerMapping.ProfileAttribute);

                        //Recursively call BuildComplexObject
                        object convertedObject = GetPropertyRecursively(userEntry, innerMapping.ProfileAttribute,
                            propInfo.PropertyType, SettingsSerializeAs.ProviderSpecific, innerMappings);

                        //Set the property of complex type
                        propInfo.SetValue(complexObj, convertedObject, null);
                    }

                    return complexObj;
                }
            }

        }

        private void SetPropertyRecursively(DirectoryEntry userEntry, object propertyValue, bool isSerialized, AttributeMapping am)
        {
            Type propertyType = propertyValue.GetType();

            //Simple property
            if (am.InnerMappingList == null || am.InnerMappingList.Count == 0)
            {
                //Primitives, strings and serialized objects
                if (propertyType.IsPrimitive || propertyType.Equals(typeof(string)) || propertyType.IsValueType || isSerialized)
                {
                    //Set the value of the attribute. Uncomment this
                    //userEntry.InvokeSet(am.ActiveDirectoryUserAttribute, propertyValue);

                    return;
                }
                //Collections and arrays
                else if (typeof(ICollection).IsAssignableFrom(propertyType))
                {
                    ICollection coll = (ICollection)propertyValue;
                    object[] arr = new object[coll.Count];

                    //Populate the object array that will be saved.
                    int i = 0;
                    IEnumerator en = coll.GetEnumerator();
                    while (en.MoveNext())
                    {
                        arr[i] = en.Current;
                        i++;
                    }

                    //Save. Uncomment this
                    //userEntry.InvokeSet(am.ActiveDirectoryUserAttribute, arr);

                    return;
                }

                throw new Exception("Unknown format");
            }
            //Complex property
            else
            {
                AttributeMappingList innerMappingList = am.InnerMappingList;
                foreach (AttributeMapping innerAm in innerMappingList)
                {
                    SetPropertyRecursively(userEntry, propertyType.GetProperty(innerAm.ProfileAttribute).GetValue(propertyValue, null), false, innerAm);
                }
            }
        }
    }

    public class AttributeMapping
    {
        string profileAttribute;

        public string ProfileAttribute
        {
            get { return profileAttribute; }
            set { profileAttribute = value; }
        }

        string activeDirectoryUserAttribute;

        public string ActiveDirectoryUserAttribute
        {
            get { return activeDirectoryUserAttribute; }
            set { activeDirectoryUserAttribute = value; }
        }

        public AttributeMapping(string profileAttribute, string activeDirectoryUserAttribute, AttributeMappingList innerMappingList)
        {
            this.activeDirectoryUserAttribute = activeDirectoryUserAttribute;
            this.profileAttribute = profileAttribute;
            this.innerMappingList = innerMappingList;
        }

        AttributeMappingList innerMappingList;

        public AttributeMappingList InnerMappingList
        {
            get { return innerMappingList; }
            set { innerMappingList = value; }
        }

    }

    public class AttributeMappingList : List<AttributeMapping>
    {
        public AttributeMapping this[string profileAttributeName]
        {
            get
            {
                foreach (AttributeMapping am in this)
                {
                    if (am.ProfileAttribute == profileAttributeName)
                        return am;
                }
                return null;


            }


        }
    }
}

