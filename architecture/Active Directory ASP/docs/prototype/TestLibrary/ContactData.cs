using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace TestLibrary
{
public class UserContactInfo
{
    private string emailId;

    public string EmailId
    {
        get { return emailId; }
        set { emailId = value; }
    }

    private int phoneNumber;

    public int PhoneNumber
    {
        get { return phoneNumber; }
        set { phoneNumber = value; }
    }

    private AddressInfo addressInfo;

    public AddressInfo AddressInfo
    {
        get { return addressInfo; }
        set { addressInfo = value; }
    }

}

public class AddressInfo
{
    private string[] addresses;

    public string[] Addresses
    {
        get { return addresses; }
        set { addresses = value; }
    }

    private string officeAddress;

    public string OfficeAddress
    {
        get { return officeAddress; }
        set { officeAddress = value; }
    }
}

    [Serializable]
    public class ProgrammingLanguages
    {
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string SerializeObjectToString()
        {
            // uses UTF8 encoding, in a more complicated situation this would be configurable

            System.Text.Encoding ENCODING = System.Text.Encoding.UTF8;

            XmlSerializer ser = new XmlSerializer(this.GetType());
            MemoryStream mem = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mem, ENCODING);

            ser.Serialize(writer, this);

            return ENCODING.GetString(mem.GetBuffer());

        }
    }
}
