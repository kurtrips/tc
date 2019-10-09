using System;

/// <summary>
/// An empty global class.
/// Sample xpath is <see cref="GlobalClass"/>
/// </summary>
public class GlobalClass
{
}

namespace MockLibrary
{
    /// <summary>
    /// Empty class.
    /// Use by other classes.
    /// Sample xpath is <see cref="ClassA"/>
    /// 
    /// This is a sample reference to a non-existing class <see cref="T:NoSuchClass"/>
    /// This is a sample reference to a non-existing namespace <see cref="N:NoSuchNamespace"/>
    /// This is a sample reference to a non-existing method <see cref="M:MockLibrary.ClassA.NoSuchMethod"/>
    /// This is a sample reference to a non-existing field <see cref="F:MockLibrary.ClassA.NoSuchField"/>
    /// </summary>
    public class ClassA : InterfaceA
    {
        /// <summary>
        /// GetAInstance method
        /// Sample xpath is <see cref="GetAInstance(ClassB)"/>
        /// </summary>
        /// <param name="b">ClassB instance</param>
        /// <returns>ClassA instance</returns>
        public EnumA GetAInstance(ClassB b)
        {
            return EnumA.Value1;
        }
    }
}
