using System;
using System.Xml;

namespace MockLibrary
{
    /// <summary>
    /// Structure A
    /// </summary>
    [Serializable]
    public struct StructA : InterfaceA
    {
        /// <summary>
        /// Method1
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <returns>empty string</returns>
        public string Method1(XmlDocument doc)
        {
            return "";
        }

        /// <summary>
        /// A <see cref="MockLibrary.StructA"/> array.
        /// </summary>
        public StructA[] array;

        /// <summary>
        /// GetInstance method. Returns a <see cref="MockLibrary.EnumA"/> instance.
        /// </summary>
        /// <param name="b">ClassB instance</param>
        /// <returns>EnumA instance</returns>
        public EnumA GetAInstance(ClassB b)
        {
            return EnumA.Value1;
        }
    }
}
