using System;
using System.Collections;
using System.Text;

namespace MockLibrary
{
    /// <summary>
    /// InterfaceB
    /// </summary>
    public interface InterfaceB
    {
        /// <summary>
        /// JustDoIt
        /// </summary>
        /// <param name="listParam">listParam</param>
        /// <param name="boolParam">boolParam</param>
        /// <param name="arrayParam">arrayParam</param>
        /// <returns></returns>
        string JustDoIt(IList listParam, bool boolParam, string[] arrayParam);
    }
}
