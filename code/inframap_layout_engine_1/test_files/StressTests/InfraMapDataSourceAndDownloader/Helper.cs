/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using TopCoder.Util.ExceptionManager.SDE;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Astraea.Inframap
{
    /// <summary>
    /// <para>
    /// Helper class for the component.
    /// It provides useful common methods for all the classes in this component.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is thread safe by introducing no state information.
    /// </threadsafety>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class Helper
    {
        /// <summary>
        /// <para>
        /// Validates the value of a variable.
        /// The value cannot be <c>null</c>.
        /// </para>
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        /// if the value of the variable is <c>null</c>.
        /// </exception>
        ///
        /// <param name="value">
        /// the value of the variable to be validated.
        /// </param>
        /// <param name="name">
        /// the name of the variable to be validated.
        /// </param>
        internal static void CheckNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name, name + " cannot be null.");
            }
        }
        /// <summary>
        /// <para>
        /// Validates the value of a variable.
        /// The value cannot be <c>null</c> or an empty string.
        /// </para>
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        /// if the value of the variable is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// if the value of the variable is an empty string.
        /// </exception>
        ///
        /// <param name="value">
        /// the value of the variable to be validated.
        /// </param>
        /// <param name="name">
        /// the name of the variable to be validated.
        /// </param>
        internal static void CheckNullOrEmpty(string value, string name)
        {
            // check null first
            CheckNull(value, name);

            if (value.Trim().Length == 0)
            {
                throw new ArgumentException(name + " cannot be an empty string.", name);
            }
        }

        /// <summary>
        /// <para>
        /// Constructs a <see cref="SelfDocumentingException"/> instance with all related data.
        /// </para>
        /// </summary>
        ///
        /// <param name="sde">
        /// The <see cref="SelfDocumentingException"/> instance.
        /// </param>
        /// <param name="instanceNames">
        /// the instance variable names.
        /// </param>
        /// <param name="instanceValues">
        /// the instance variable values.
        /// </param>
        /// <param name="paramNames">
        /// The parameter variable names.
        /// </param>
        /// <param name="paramValues">
        /// The parameter variable values.
        /// </param>
        /// <param name="localNames">
        /// The local variable names.
        /// </param>
        /// <param name="localValues">
        /// The local variable values.
        /// </param>
        /// <param name="stackFrameIndex">
        /// The index to the stack frame where the method base will be
        /// obtained from the stack trace.
        /// </param>
        ///
        /// <returns>
        /// The <see cref="SelfDocumentingException"/> instance.
        /// </returns>
        internal static SelfDocumentingException ConstructSDE(
            SelfDocumentingException sde,
            string[] instanceNames, object[] instanceValues,
            string[] paramNames, object[] paramValues,
            string[] localNames, object[] localValues,
            int stackFrameIndex)
        {
            // Pin the method. An index is used to determine which particular stack frame to get the method from.
            MethodBase methodBase = new StackTrace().GetFrame(stackFrameIndex).GetMethod();

            MethodState ms = sde.PinMethod(methodBase.DeclaringType.FullName + "." + methodBase.Name,
                ((sde.InnerException == null) ? sde : sde.InnerException).StackTrace);

            // Add instance variables
            if (instanceNames != null)
            {
                for (int i = 0; ((i < instanceNames.Length) && (i < instanceValues.Length)); i++)
                {
                    ms.AddInstanceVariable(instanceNames[i], instanceValues[i]);
                }
            }

            // Add parameter variables
            if (paramNames != null)
            {
                for (int i = 0; ((i < paramNames.Length) && (i < paramValues.Length)); i++)
                {
                    ms.AddMethodParameter(paramNames[i], paramValues[i]);
                }
            }

            // Add local variables
            if (localNames != null)
            {
                for (int i = 0; ((i < localNames.Length) && (i < localValues.Length)); i++)
                {
                    ms.AddLocalVariable(localNames[i], localValues[i]);
                }
            }

            ms.Lock();

            return sde;
        }

        /// <summary>
        /// <para>
        /// Executes the query and builds an <see cref="IDataReader"/>.
        /// </para>
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/>.
        /// </param>
        /// <param name="queryText">
        /// command query text.
        /// </param>
        /// <param name="queryParam">
        /// command query params.
        /// </param>
        /// <returns>
        /// The query result.
        /// </returns>
        internal static IDataReader ExecuteReader(IDbConnection connection, string queryText)
        {
            using (IDbCommand command = PrepareCommand(connection, queryText))
            {
                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// <para>
        /// Prepare the command using the text and params.
        /// </para>
        /// </summary>
        /// <param name="connection">
        /// The <see cref="IDbConnection"/>.
        /// </param>
        /// <param name="queryText">
        /// command query text.
        /// </param>
        /// <param name="queryParam">
        /// command query params.
        /// </param>
        /// <returns>
        /// The <see cref="IDbCommand"/> instance.
        /// </returns>
        private static IDbCommand PrepareCommand(IDbConnection connection, string queryText)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandText = queryText;

            return command;
        }
    }
}
