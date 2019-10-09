/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */
using System;
using System.Collections.Generic;
using System.Text;
using HermesNS.TC.Services.GenericNotes;

namespace HermesNS.TC.Services.ScheduleItem.Clients
{
    /// <summary>
    /// Mock.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    public class GenericNotesServiceClient
    {
        /// <summary>
        /// Mock.
        /// </summary>
        public void Close()
        { 
        }

        /// <summary>
        /// Mock.
        /// </summary>
        public void Open()
        {
        }

        /// <summary>
        /// Mock.
        /// </summary>
        public void Abort()
        {
        }

        /// <summary>
        /// Mock.
        /// </summary>
        /// <param name="note">Mock.</param>
        /// <param name="s">Mock.</param>
        /// <param name="name">Mock.</param>
        public void DeleteGenericNote(HermesGenericNote note, string s, string name)
        {
        }

        /// <summary>
        /// Mock.
        /// </summary>
        /// <param name="id">Mock.</param>
        /// <param name="zone">Mock.</param>
        /// <param name="userName">Mock.</param>
        /// <param name="userId">Mock.</param>
        /// <returns>Mock.</returns>
        public HermesGenericNote GetGenericNote(string id, TimeZone zone, string userName, string userId)
        {
            HermesGenericNote note = new HermesGenericNote();
            note.Id = id;
            return note;
        }

        /// <summary>
        /// Mock.
        /// </summary>
        /// <param name="note">Mock.</param>
        /// <param name="userName">Mock.</param>
        /// <param name="userId">Mock.</param>
        /// <returns>Mock.</returns>
        public HermesGenericNote UpdateGenericNote(HermesGenericNote note, string userName, string userId)
        {
            return note;
        }

        /// <summary>
        /// Mock.
        /// </summary>
        /// <param name="note">Mock.</param>
        /// <param name="userName">Mock.</param>
        /// <param name="userId">Mock.</param>
        /// <returns>Mock.</returns>
        public HermesGenericNote AddGenericNote(HermesGenericNote note, string userName, string userId)
        {
            return note;
        }
    }
}
