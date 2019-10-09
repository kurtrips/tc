using System;
using System.Collections;

namespace MockLibrary
{
    /// <summary>
    /// Class B doc.
    /// Inherits from ClassA.
    /// Demonstrates public field, method, property.
    /// Demonstrates a method using instance of <see cref="ClassA"/>
    /// and returning a <see cref="T:MockLibrary.ClassA"/> array. Demonstrates the xpath formed for a class.
    /// Also demonstrates the xpath formed for a field and a method and property.
    /// </summary>
    public class ClassB : ClassA
    {
        /// <summary>
        /// Contains id to name mappings.
        /// </summary>
        public IDictionary namesIds = new Hashtable();

        /// <summary>
        /// TestMethod1 doc.
        /// </summary>
        /// <param name="objA">ClassA instance</param>
        /// <returns>a string</returns>
        public ClassA[] TestMethod1(ClassA objA)
        {
            return new ClassA[12];
        }

        /// <summary>
        /// A test property with type <see cref="MockLibrary.ClassA"/>
        /// </summary>
        virtual public ClassA TestProperty1
        {
            get
            {
                return new ClassA();
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets the name for a given id.
        /// 1st overload for this method.
        /// A sample xpath should be formed <see cref="MockLibrary.ClassB.GetName(System.Int32)"/>
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>The name</returns>
        public static string GetName(int id)
        {
            return "My name is Lucifer." + id;
        }

        /// <summary>
        /// Gets the name for a given string id.
        /// 2nd overload. Same number of parameters but different type of parameter.
        /// A sample xpath should be formed <see cref="MockLibrary.ClassB.GetName(System.String)"/>
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>The name</returns>
        public static string GetName(string id)
        {
            return "My name is Lucifer." + id;
        }

        /// <summary>
        /// Gets the name for a given string id.
        /// 3rd overload. Different number of parameters.
        /// A sample xpath should be formed <see cref="MockLibrary.ClassB.GetName(System.String, System.Int32)"/>
        /// </summary>
        /// <param name="id">string id</param>
        /// <param name="iid">Int id</param>
        /// <returns>The name</returns>
        public static string GetName(string id, int iid)
        {
            return "My name is Lucifer." + id + iid;
        }

        /// <summary>
        /// Gets the name for a given id.
        /// Simply delegates to <see cref="MockLibrary.ClassB.GetName(System.Int32)"/>
        /// </summary>
        /// <param name="ids">The id</param>
        /// <returns>The names</returns>
        public static IList GetNames(int[] ids)
        {
            IList ret = new ArrayList();
            foreach (int id in ids)
            {
                ret.Add(GetName(id));
            }
            return ret;
        }

        /// <summary>
        /// Adds the id and name to the <see cref="MockLibrary.ClassB.namesIds"/> variable.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        public void SetName(int id, string name)
        {
            namesIds.Add(id, name);
        }

        /// <summary>
        /// This method demonstartes an xpath to a property.
        /// It uses the <see cref="MockLibrary.ClassB.TestProperty1"/> property.
        /// </summary>
        /// <returns></returns>
        public bool IsPropNull()
        {
            return TestProperty1 == null ? true : false;
        }

        /// <summary>
        /// An example indexer.
        /// A sample xpath should be formed <see cref="MockLibrary.ClassB.get_Item(System.Int32)"/>
        /// </summary>
        /// <param name="id">An id</param>
        /// <returns>A string</returns>
        public string this[int id]
        {
            get
            {
                return id.ToString();
            }
        }

        /// <summary>
        /// An example indexer. 2nd overload with same number of params but different types.
        /// A sample xpath should be formed <see cref="MockLibrary.ClassB.get_Item(System.String)"/>
        /// </summary>
        /// <param name="id">An id</param>
        /// <returns>A string</returns>
        public string this[string id]
        {
            get
            {
                return id.ToString();
            }
        }

        /// <summary>
        /// An example indexer. 3nd overload with different number of params.
        /// A sample xpath should be formed <see cref="MockLibrary.ClassB.get_Item(System.String, System.Int32)"/>
        /// </summary>
        /// <param name="id">An id</param>
        /// <param name="iid">int id</param>
        /// <returns>A string</returns>
        public string this[string id, int iid]
        {
            get
            {
                return id.ToString() + iid.ToString();
            }
        }

        /// <summary>
        /// Some folks dont really like generics. They prefer really big and nested arrays.
        /// </summary>
        /// <returns>Phew</returns>
        public ClassA[][, ,][] BigAndNestedArrayReturningFunction()
        {
            return null;
        }
    }
}
