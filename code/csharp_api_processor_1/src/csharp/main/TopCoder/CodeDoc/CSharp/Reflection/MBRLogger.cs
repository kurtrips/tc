// MBRLogger.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using TopCoder.LoggingWrapper;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// This class extends MarshalByRefObject and represents a simple wrapper of the Logger instance that can be
    /// transferred between two different app domains by reference. The Logger instance doesn't extend MarshalByRefObject
    /// and can not be passed between two app domains. This class solves this problem.
    /// </summary>
    ///
    /// <threadsafety>
    /// <para>Thread Safety: This class is
    /// immutable and its referenced Logger instance is also immutable and therefore this class is thread safe.</para>
    /// </threadsafety>
    ///
    /// <author>urtks</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MBRLogger : MarshalByRefObject
    {
        /// <summary>
        /// <para>Represents the Logger instance to be wrapped. Initialized in the constructor. Reference not changed
        /// afterwards. Can not be null.</para>
        /// </summary>
        private readonly Logger logger;


        /// <summary>
        /// <para>Creates a new instance of MBRLogger instance with the given Logger instance.</para>
        /// </summary>
        /// <param name="logger">the Logger instance to be wrapped.</param>
        /// <exception cref="ArgumentNullException">if logger is null.</exception>
        public MBRLogger(Logger logger)
        {
            Helper.ValidateNotNull(logger, "logger");
            this.logger = logger;
        }

        /// <summary>
        /// <para>Log a message using the underlying Logger instance.</para>
        /// </summary>
        /// <param name="level">The logging level of the message being logged.</param>
        /// <param name="message">The parameters used to format the message (if needed).</param>
        /// <param name="param">The parameters used to format the message (if needed).</param>
        public void Log(Level level, string message, params object[] param)
        {
            logger.Log(level, message, param);
        }

        /// <summary>
        /// <para>Log a message using the underlying Logger instance.</para>
        /// </summary>
        /// <param name="message">The message to log, can contain {0}, {1}, ... for inserting parameters.</param>
        /// <param name="param">The parameters used to format the message (if needed).</param>
        public void Log(string message, params object[] param)
        {
            logger.Log(message, param);
        }
    }
}
