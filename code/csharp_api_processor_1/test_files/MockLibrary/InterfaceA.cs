using System;
using System.Collections.Generic;
using System.Text;

namespace MockLibrary
{
    /// <summary>
    /// InterfaceA
    /// </summary>
    
    public interface InterfaceA
    {
        /// <summary>
        /// GetAInstance method
        /// </summary>
        /// <param name="b">ClassB instance</param>
        /// <returns>ClassA instance</returns>
        EnumA GetAInstance(ClassB b);
    }
}
