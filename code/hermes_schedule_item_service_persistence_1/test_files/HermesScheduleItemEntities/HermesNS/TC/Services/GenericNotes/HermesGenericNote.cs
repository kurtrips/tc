using System;
using System.Collections.Generic;
using TopCoder.Services.WCF.GenericNotes.Entities;
using HermesNS.TC.Services.AuditTrail;

namespace HermesNS.TC.Services.GenericNotes
{
    public class HermesGenericNote :
        GenericNoteBase<HermesGenericNote, HermesGenericNoteItem, HermesGenericNoteItemHistory, string>,
        IAuditable<HermesGenericNote>
    {
        public IList<HermesAuditRecord> Audit(HermesGenericNote old)
        {
            throw new NotImplementedException();
        }
    }

    public class HermesGenericNoteItem :
        GenericNoteItemBase<HermesGenericNoteItem, HermesGenericNoteItemHistory, string>,
        IAuditable<HermesGenericNoteItem>
    {
        public IList<HermesAuditRecord> Audit(HermesGenericNoteItem old)
        {
            throw new NotImplementedException();
        }
    }

    public class HermesGenericNoteItemHistory :
        GenericNoteItemHistoryBase<HermesGenericNoteItemHistory>,
        IAuditable<HermesGenericNoteItemHistory>
    {
        public IList<HermesAuditRecord> Audit(HermesGenericNoteItemHistory old)
        {
            throw new NotImplementedException();
        }
    }
}
