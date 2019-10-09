using System;

namespace MockLibrary
{
    /// <summary>
    /// InterfaceC
    /// </summary>
    public interface InterfaceC : InterfaceA, InterfaceB
    {
        /// <summary>
        /// Say Hello
        /// </summary>
        void SayHelloToTheWorld();
    }
}
