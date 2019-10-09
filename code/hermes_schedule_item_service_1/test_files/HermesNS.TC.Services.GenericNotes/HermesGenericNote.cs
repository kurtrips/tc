// HermesGenericNote.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
using TopCoder.Services.WCF.GenericNotes.Entities;

namespace HermesNS.TC.Services.GenericNotes.Entities
{
    /// <summary>A mock implementation of the HermesGenericNoteItem interface</summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public class HermesGenericNote :
        GenericNoteBase<HermesGenericNote, HermesGenericNoteItem, HermesGenericNoteItemHistory, string>
    {
        /// <summary>
        /// Creates a new HermesGenericNote instance.
        /// </summary>
        public HermesGenericNote()
        {
        }
    }

    /// <summary>A mock implementation of the HermesGenericNoteItem interface</summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public class HermesGenericNoteItem :
        GenericNoteItemBase<HermesGenericNoteItem, HermesGenericNoteItemHistory, string>
    {
    }

    /// <summary>A mock implementation of the HermesGenericNoteItemHistory interface</summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public class HermesGenericNoteItemHistory :
        GenericNoteItemHistoryBase<HermesGenericNoteItemHistory>
    {
    }
}
