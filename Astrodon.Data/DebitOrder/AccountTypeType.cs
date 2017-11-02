using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Astrodon.Data.DebitOrder
{
    [DataContract]
    public enum AccountTypeType
    {
        [EnumMember]
        Cheque = 1
    }
}
