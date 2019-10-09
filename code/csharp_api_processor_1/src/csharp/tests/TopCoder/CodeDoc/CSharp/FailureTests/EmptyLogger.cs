/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using TopCoder.LoggingWrapper;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// <para>The Logger subclass used for testing purpose.</para>
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [CoverageExclude]
    public class EmptyLogger : Logger
    {
        /// <summary>
        /// <para>Create an instance of EmptyLogger</para>
        /// </summary>
        /// <param name="name">the logger name</param>
        public EmptyLogger(string name) : base(name)
        {
        }

        /// <summary>
        /// <para>Do nothing.</para>
        /// </summary>
        /// <param name="level">The logging level.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="param">The parameters used to format the message.</param>
        public override void Log(Level level, string message, params object[] param)
        {
            string.Format(message, param);
        }

        /// <summary>
        /// <para>Always returns true.</para>
        /// </summary>
        /// <param name="level">The level to check.</param>
        /// <returns>true.</returns>
        public override bool IsLevelEnabled(Level level)
        {
            return true;
        }

        /// <summary>
        /// <para>Disposes this instance.</para>
        /// </summary>
        public override void Dispose()
        {
        }
    }
}
