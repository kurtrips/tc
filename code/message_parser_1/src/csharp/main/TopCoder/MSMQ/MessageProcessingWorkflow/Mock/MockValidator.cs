// MessageParserManager .cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using TopCoder.Util.DataValidator;
using System.Runtime.Serialization;
using TopCoder.MSMQ.ConversationManager.Entities;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <para>This is a mock implementation of the IValidator interface.</para>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MockValidator : IValidator
    {
        /// <summary>
        /// Validates an object. This is a mock implementation.
        /// </summary>
        /// <param name="obj">The object to validate</param>
        /// <returns>True if obj is not null, is of type Message and has SourceQueue with name "Source Queue"</returns>
        public bool IsValid(object obj)
        {
            if (obj == null || !(obj is Message))
            {
                return false;
            }

            Message message = (Message)obj;
            if (message.SourceQueue.Name == "Source Queue")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Mock implementation of GetObjectData. Does nothing.
        /// </summary>
        /// <param name="info">info</param>
        /// <param name="context">context</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
