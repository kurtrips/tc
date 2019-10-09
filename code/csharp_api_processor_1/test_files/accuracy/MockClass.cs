/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using System;
using System.IO;
using System.Collections;

namespace TopCoder.CodeDoc.CSharp.AccuracyTests
{
    /// <summary>
    /// A sample class, <see cref="System.Object"/>.
    /// </summary>
    /// <seealso cref="System.Exception"/>
    /// <author>
    /// cnettel
    /// </author>
    /// <version>
    /// 1.0
    /// </version>
    /// <copyright>
    /// Copyright (C) 2007 TopCoder Inc., All rights reserved.
    /// </copyright>
    [Obsolete("Testing attributes.")]
    public class MockClass : Exception
    {
        /// <summary>
        /// This is a private field.
        /// </summary>
        /// <seealso cref="privateStaticField"/>
        private readonly int privateField;

        /// <summary>
        /// This is a public field.
        /// </summary>
        public MockClass engine;

        /// <summary>
        /// This is a private static field.
        /// </summary>
        private int privateStaticField;

        /// <summary>
        /// This is an internal static field.
        /// </summary>
        internal static int[,] internalStaticField;

        /// <summary>
        /// A delegate that can be used.
        /// </summary>
        /// <param name="arrayParam">An array parameter.</param>
        /// <returns>The result.</returns>
        internal delegate int DelegateHere(int[] arrayParam);

        /// <summary>
        /// A delegate that can be used, just like <see cref="DelegateHere"/>.
        /// </summary>
        /// <param name="arrayParam">A nested array parameter.</param>
        /// <returns>The result.</returns>
        internal delegate MockClass DelegateThere(int[][][][] arrayParam);

        /// <summary>
        /// A static event.
        /// </summary>
        public static event ADelegate staticEvent;

        /// <summary>
        /// A protected event.
        /// </summary>
        protected event ADelegate protectedEvent;

        /// <summary>
        /// A private event, almost like <see cref="staticEvent"/>.
        /// </summary>
        private event DelegateHere privateEvent;

        /// <summary>
        /// A virtual method.
        /// </summary>
        public virtual void VirtMethod()
        {
        }

        /// <summary>
        /// A property.
        /// </summary>
        /// <value>
        /// No actual value.
        /// </value>
        public virtual MockClass Property
        {
            private get
            {
                return null;
            }
            set
            {
            }
        }

        /// <summary>
        /// A property.
        /// </summary>
        /// <value>
        /// No actual value.
        /// </value>
        public static string StaticProperty
        {
            private get
            {
                return null;
            }
            set
            {
            }
        }

        /// <summary>
        /// A property.
        /// </summary>
        /// <value>
        /// No actual value.
        /// </value>
        private MockClass PrivateProperty
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        /// <summary>
        /// A property, but this is private, not like <see cref="PrivateProperty"/>.
        /// </summary>
        /// <value>
        /// No actual value.
        /// </value>
        public static string PrivateStaticProperty
        {
            private get
            {
                return null;
            }
            set
            {
            }
        }

        /// <summary>
        /// A public indexer.
        /// </summary>
        /// <param name="x">Some index.</param>
        /// <value>The value.</value>
        public int this[int x]
        {
            set
            {
            }
        }

        /// <summary>
        /// A private indexer.
        /// </summary>
        /// <param name="x">Some index.</param>
        /// <value>The value.</value>
        private int this[string x]
        {
            set
            {
            }
        }

        /// <summary>
        /// A private indexer.
        /// </summary>
        /// <param name="x">Some index.</param>
        /// <value>The value.</value>
        private int this[double x]
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        /// <summary>
        /// A nested (inner) class.
        /// </summary>
        private abstract class PrivateInner
        {
            /// <summary>
            /// A public constructor, but almost like <see cref="PrivateInner()"/>.
            /// </summary>
            /// <param name="mock">An argument.</param>
            public PrivateInner(MockClass mock)
            {
            }

            /// <summary>
            /// A private constructor, somewhat like <see cref="PrivateInner(MockClass)"/>.
            /// </summary>
            private PrivateInner()
            {
            }

            /// <summary>
            /// This is an abstract method.
            /// </summary>
            /// <param name="paramList">An array parameter.</param>
            /// <exception cref="FileNotFoundException">A cref to an exception.</exception>
            public abstract void Method(params MockClass[] paramList);

            /// <summary>
            /// Another juicy interface.
            /// </summary>
            private interface SomeCollection
            {
            }

            /// <summary>
            /// An inner form of the outer abstract class.
            /// </summary>
            internal abstract protected class AnotherInner : PrivateInner, SomeCollection
            {
                /// <summary>
                /// The abstract method incarnated.
                /// </summary>
                /// <param name="paramList">Parameter list.</param>
                public override void Method(params MockClass[] paramList)
                {
                }
            }

            /// <summary>
            /// Explicit conversion.
            /// </summary>
            /// <param name="x">Param.</param>
            /// <returns>41.0</returns>
            public static explicit operator double(PrivateInner x)
            {
                return 41.0;
            }

            /// <summary>
            /// Implicit conversion.
            /// </summary>
            /// <param name="x">Param.</param>
            /// <returns>41.0</returns>
            public static implicit operator string(PrivateInner x)
            {
                return "41";
            }

            /// <summary>
            /// "Real" operatover overloading.
            /// </summary>
            /// <param name="x">The argument.</param>
            /// <returns>Itself.</returns>
            public static PrivateInner operator ++(PrivateInner x)
            {
                return x;
            }
        }
    }

    /// <summary>
    /// A struct.
    /// </summary>
    public struct Struct
    {
        /// <summary>
        /// A field.
        /// </summary>
        int x;

        /// <summary>
        /// Inner enums can exist in anything.
        /// </summary>
        public enum Hej
        {
        }

        /// <summary>
        /// Inner classes can exist in anything.
        /// </summary>
        public class Hoj
        {
        }
    }

    /// <summary>
    /// An enum.
    /// </summary>
    public enum Enum
    {
        /// <summary>
        /// x is 42.
        /// </summary>
        x = 42,
        /// <summary>
        /// y is 43
        /// </summary>
        y = 43
    }

    /// <summary>
    /// An interface. A very empty one, but here is some inheritance.
    /// </summary>
    public interface AnInterface : ICollection
    {
    }

    /// <summary>
    /// A delegate that can be used.
    /// </summary>
    /// <param name="param">A parameter.</param>
    /// <returns>The result.</returns>
    public delegate void ADelegate(double param);
}