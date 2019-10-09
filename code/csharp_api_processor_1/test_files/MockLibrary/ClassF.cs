using System;
using BaseLibrary;

namespace MockLibrary
{
    /// <summary>
    /// A class demonstrating dependent library.
    /// </summary>
    public class ClassF : ClassBaseA
    {
        /// <summary>
        /// Function returning <see cref="BaseLibrary.ClassBaseA"/> instance. 
        /// </summary>
        /// <returns>ClassBaseA instance.</returns>
        public ClassBaseA ADependentFunction()
        {
            return null;
        }
    }
}
