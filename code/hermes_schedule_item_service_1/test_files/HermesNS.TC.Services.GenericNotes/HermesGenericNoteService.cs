using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using HermesNS.TC.Services.GenericNotes.Entities;

namespace HermesNS.TC.Services.GenericNotes
{
    /// <summary>
    /// A mock implementation of the <see cref="HermesGenericNoteService"/> class.
    /// This implementation uses dictionaries to mock the behavior of a database.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes")]
    public class HermesGenericNoteService
    {
        /// <summary>
        /// The dictionary for mocking a database table.
        /// </summary>
        private static readonly IDictionary<string, HermesGenericNote> notesDb =
            new Dictionary<string, HermesGenericNote>();

        /// <summary>
        /// Mock implementation of the GetGenericNote method
        /// </summary>
        /// <param name="noteId">The note id of the note</param>
        /// <param name="timeZone">time zone</param>
        /// <param name="userName">The user name</param>
        /// <param name="userId">The user id</param>
        /// <returns>The HermesGenericNote for the given paramaters</returns>
        [OperationContract]
        public HermesGenericNote GetGenericNote(string noteId, TimeZone timeZone, string userName, string userId)
        {
            if (notesDb.ContainsKey(noteId))
            {
                return notesDb[noteId];
            }
            return null;
        }

        /// <summary>
        /// Mock implementation for adding a new note.
        /// </summary>
        /// <param name="note">The note to be added.</param>
        /// <param name="userName">The user name</param>
        /// <param name="userId">The user id</param>
        /// <returns>The added note</returns>
        [OperationContract]
        public HermesGenericNote AddGenericNote(HermesGenericNote note, string userName, string userId)
        {
            note.Id = Guid.NewGuid().ToString();
            notesDb[note.Id] = note;
            return note;
        }

        /// <summary>
        /// Mock implementation for updating an existing note.
        /// </summary>
        /// <param name="note">The note to be added.</param>
        /// <param name="userName">The user name</param>
        /// <param name="userId">The user id</param>
        /// <returns>The updated note</returns>
        [OperationContract]
        public HermesGenericNote UpdateGenericNote(HermesGenericNote note, string userName, string userId)
        {
            notesDb[note.Id] = note;
            return note;
        }

        /// <summary>
        /// Mock implementation for deleting a note.
        /// </summary>
        /// <param name="note">The note to be deleted.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="userId">The user id.</param>
        [OperationContract]
        public void DeleteGenericNote(HermesGenericNote note, string userName, string userId)
        {
            notesDb.Remove(note.Id);
        }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            List<Type> knownTypes = new List<Type>();

            // Add any types to include here.
            knownTypes.Add(TimeZone.CurrentTimeZone.GetType());
            knownTypes.Add(typeof(System.Globalization.DaylightTime));
            return knownTypes;
        }

    }
}
