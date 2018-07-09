using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Astrodon.DataContracts
{
    [DataContract]
    public class BuildingClosingBalance
    {
        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public decimal OpeningBalance { get; set; }

        [DataMember]
        public decimal ClosingBalance { get; set; }

        [DataMember]
        public int Period { get; set; }

        [DataMember]
        public DateTime PeriodDate { get; set; }

        [DataMember]
        public decimal Due { get;  set; }
    }
}
