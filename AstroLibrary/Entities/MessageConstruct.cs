using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public struct MessageConstruct
    {
        public String building;
        public String customer;
        public String number;
        public String reference;
        public String text;
        public int id;
        public DateTime sent;
        public String sender;
        public bool billable;
        public bool bulkbillable;
        public String astStatus;
        public String batchID;
        public String status;
        public DateTime nextPolled;
        public int pollCount;
        public double os;
        public String msgType;
    }
}