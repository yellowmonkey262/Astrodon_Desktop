using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class SMSMessage
    {
        public int id { get; set; }

        public String building { get; set; }

        public String customer { get; set; }

        public String number { get; set; }

        public String reference { get; set; }

        public String message { get; set; }

        public bool direction { get; set; }

        public DateTime sent { get; set; }

        public String sender { get; set; }

        public bool billable { get; set; }

        public bool bulkbillable { get; set; }

        public String astStatus { get; set; }

        public String batchID { get; set; }

        public String status { get; set; }

        public DateTime nextPolled { get; set; }

        public int pollCount { get; set; }

        public double cbal { get; set; }

        public String smsType { get; set; }
    }

    public class SMSCustomers
    {
        public SMSCustomers()
        {
            customers = new List<SMSCustomer>();
        }

        public List<SMSCustomer> customers { get; set; }
    }

    public class SMSCustomer
    {
        public bool include { get; set; }

        public String customerName { get; set; }

        public String customerAccount { get; set; }

        public String customerNumber { get; set; }
    }
}