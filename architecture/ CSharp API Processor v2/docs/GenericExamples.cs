using System;
using System.Collections.Generic;

namespace MockLibrary
{
    /// <summary>
    /// A normal non generic class.
    /// </summary>
    public class NormalClass
    {
    }

    /// <summary>
    /// A generic class
    /// </summary>
    /// <typeparam name="T">The type of elements in collection</typeparam>
    public class GenericCollection<T>
    {
    }

    /// <summary>
    /// This is a generic class called <see cref="MockLibrary.GenericList&lt;T, U&gt;"/>
    /// It derives from <see cref="MockLibrary.GenericCollection&lt;T&gt;"/>
    /// 1st overload for Pop is <see cref="MockLibrary.GenericList&lt;T, U&gt;.Pop&lt;X, Y, Z&gt;"/>
    /// 2nd overload for Pop is <see cref="MockLibrary.GenericList&lt;T, U&gt;.Pop&lt;X, Y&gt;"/>
    /// It conatins a property called <see cref="MockLibrary.GenericList&lt;T, U&gt;.GenericProperty1"/>
    /// </summary>
    /// <typeparam name="T">The generic type paramter</typeparam>
    /// <typeparam name="U">Another generic type paramter</typeparam>
    public class GenericList<T, U> : GenericCollection<int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericList()
        {
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        public GenericList(GenericList<T, U> gen)
        {
        }

        /// <summary>
        /// Constructor 3
        /// </summary>
        public GenericList(GenericList<string, U> gen)
        {
        }

        /// <summary>
        /// Constructor 4
        /// </summary>
        public GenericList(GenericList<string, int> gen)
        {
        }

        /// <summary>
        /// Constructor 5
        /// </summary>
        public GenericList(GenericList<NormalClass, NormalClass> gen)
        {
        }

        /// <summary>
        /// A method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string AMethod(IGenericBaseInterface<string, U> p)
        {
            return "";
        }

        /// <summary>
        /// A method
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string AMethod(IGenericBaseInterface<T, U> p)
        {
            return "";
        }

        /// <summary>
        /// A generic method.
        /// </summary>
        /// <returns>A generic value</returns>
        /// <typeparam name="X">A generic type param</typeparam>
        /// <typeparam name="Y">A generic type param</typeparam>
        /// <typeparam name="Z">A generic type param</typeparam>
        public string Pop<X, Y, Z>(T id1, U id2)
        {
            return "";
        }

        /// <summary>
        /// A generic method.
        /// </summary>
        /// <returns>A generic value</returns>
        /// <typeparam name="X">The generic type param</typeparam>
        /// <typeparam name="Y">The generic type param</typeparam>
        public NormalClass Pop<X, Y>(T id1, U id2)
        {
            return null;
        }

        /// <summary>
        /// A method with 2 generic parameters.
        /// </summary>
        /// <param name="input">A generic parameter</param>
        /// <param name="input2">Another generic parameter</param>
        public T Add(T input, IGenericBaseInterface<GenericCollection<NestedList<T>>, U> input2) 
        {
            return default(T);
        }

        /// <summary>
        /// A method with generic parameters.
        /// </summary>
        /// <param name="input">A generic parameter</param>
        public GenericCollection<T> Add(T input)
        {
            return null;
        }

        /// <summary>
        /// A generic property.
        /// </summary>
        public GenericStruct<T, U> GenericProperty1
        {
            get
            {
                return default(GenericStruct<T, U>);
            }
            set
            {
            }
        }

        /// <summary>
        /// Another generic property.
        /// </summary>
        public T GenericProperty2
        {
            get
            {
                return default(T);
            }
            set
            {
            }
        }

        /// <summary>
        /// Another generic property.
        /// </summary>
        public GenericStruct<T, string> GenericProperty3
        {
            get
            {
                return default(GenericStruct<T, string>);
            }
            set
            {
            }
        }

        /// <summary>
        /// Another generic property.
        /// </summary>
        public GenericStruct<int, string> GenericProperty4
        {
            get
            {
                return default(GenericStruct<int, string>); ;
            }
            set
            {
            }
        }

        /// <summary>
        /// Generic event.
        /// </summary>
        public event GenericEventHandler<T, EventArgs> genericEvent;

        /// <summary>
        /// A generic field
        /// </summary>
        public IGenericBaseInterface<T, U> genericField;

        /// <summary>
        /// A nice and big nested genereic field.
        /// </summary>
        public IGenericBaseInterface<GenericCollection<NestedList<T>>, U> bigGenericField;

        /// <summary>
        /// A field of type struct.
        /// </summary>
        public GenericStruct<T, U> structField;

        /// <summary>
        /// Another indexer 5
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public GenericCollection<int> this[IGenericBaseInterface<GenericCollection<NestedList<T>>, string> idx]
        {
            get
            {
                return default(GenericCollection<int>);
            }
        }

        /// <summary>
        /// A indexer 0
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public string this[int idx]
        {
            get { return ""; }
        }

        /// <summary>
        /// An indexer 1. 
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public T this[string idx]
        {
            get { return default(T); }
        }

        /// <summary>
        /// An indexer 2
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public T this[GenericCollection<T> idx]
        {
            get { return default(T); }
        }

        /// <summary>
        /// Another indexer 3
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public T this[GenericCollection<int> idx]
        {
            get
            {
                return default(T);
            }
        }

        /// <summary>
        /// Another indexer 4
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public GenericCollection<T> this[GenericCollection<string> idx]
        {
            get
            {
                return default(GenericCollection<T>);
            }
        }
    }

    /// <summary>
    /// Generic interface
    /// </summary>
    /// <typeparam name="T">a</typeparam>
    /// <typeparam name="U">s</typeparam>
    public interface IGenericBaseInterface<T, U>
    {
    }

    /// <summary>
    /// This is an interface called <see cref="IGenericInterface&lt;T&gt;"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericInterface<T> : IGenericBaseInterface<string, int>
    {
        /// <summary>
        /// A method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        T GetIt(T obj);
    }

    /// <summary>
    /// Generic event handler
    /// </summary>
    /// <typeparam name="T">Type of sender</typeparam>
    /// <typeparam name="U">Type of event args</typeparam>
    /// <param name="sender">sender</param>
    /// <param name="eventArgs">args</param>
    public delegate void GenericEventHandler<T, U>(T sender, U eventArgs);

    /// <summary>
    /// A Generic class. It contains a nested class <see cref="MockLibrary.NestedList&lt;T&gt;.NestedChildList&lt;U, V&gt;"/>
    /// </summary>
    /// <typeparam name="T">type</typeparam>
    public class NestedList<T>
    {
        /// <summary>
        /// A nested type.
        /// </summary>
        /// <typeparam name="U">type1</typeparam>
        /// <typeparam name="V">type2</typeparam>
        public class NestedChildList<U, V>
        {
        }
    }

    /// <summary>
    /// A partial class
    /// </summary>
    public partial class PartialClass
    {
    }

    /// <summary>
    /// Part 2 of the partial class
    /// </summary>
    public partial class PartialClass
    {
    }

    /// <summary>
    /// A generic structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public struct GenericStruct<T, U> : IGenericBaseInterface<T, U>
    {
    }
}
