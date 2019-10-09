using System;

namespace MockLibrary.Nested
{
    /// <summary>
    /// Class C doc. Contains many different visibilities and modifiers. Also contains nested classes.
    /// Also this class is in the <see cref="MockLibrary.Nested"/> namespace.
    /// So demonstrates the xpath for namespaces also.
    /// </summary>
    public class ClassC
    {
        /// <summary>
        /// Constructor
        /// Sample xpath is <see cref="ClassC()"/>
        /// </summary>
        public ClassC()
        {
            privateInt = 1;
        }

        /// <summary>
        /// A private integer.
        /// Sample xpath is <see cref="privateInt"/>
        /// </summary>
        private int privateInt = 0;

        /// <summary>
        /// A nested interface.
        /// Sample xpath is <see cref="MockLibrary.Nested.ClassC.InterfaceD"/>
        /// </summary>
        public interface InterfaceD
        {
            /// <summary>
            /// Sample xpath is <see cref="MakeMyDay()"/>
            /// </summary>
            void MakeMyDay();
        }

        /// <summary>
        /// A nested class of ClassC.
        /// A sample xpath is <see cref="ClassD"/>
        /// </summary>
        public class ClassD
        {
            /// <summary>
            /// An int variable with static and readonly.
            /// A sample xpath is <see cref="ClassD.count"/>
            /// </summary>
            public static readonly int count = 0;

            /// <summary>
            /// A method
            /// </summary>
            /// <param name="x">ClassA param</param>
            /// <param name="y">ClassE param</param>
            /// <returns>a string array</returns>
            protected virtual string[] MethodC(ClassB x, ClassE y)
            {
                return null;
            }

            /// <summary>
            /// A constructor
            /// </summary>
            /// <param name="x">int param</param>
            public ClassD(int x)
            {
                anEvent += new AlarmEventHandler(ClassEvent);
            }

            /// <summary>
            /// event handler for anEvent
            /// </summary>
            /// <param name="sender">sender</param>
            /// <param name="e">e</param>
            /// <returns>empty string</returns>
            public string ClassEvent(object sender, EventArgs e)
            {
                return "";
            }

            /// <summary>
            /// An event.
            /// </summary>
            public event AlarmEventHandler anEvent;

            /// <summary>
            /// A protected property of type ClassE.
            /// </summary>
            protected ClassE Prop
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
            /// A nested class of <see cref="MockLibrary.Nested.ClassC.ClassD"/> 
            /// </summary>
            public class ClassE
            {
            }
        }
    }
}
