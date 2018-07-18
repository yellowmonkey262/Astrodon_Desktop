using Astrodon.Classes;
using Astrodon.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Astrodon.DataProcessor
{
    public class CustomerDocumentReminders
    {
        private DataContext _Context;

        public CustomerDocumentReminders(DataContext dc)
        {
            _Context = dc;
        }

        public void Process()
        {
            var expireCheck = DateTime.Today.AddDays(14);

            var qry = from b in _Context.tblBuildings
                            from c in b.CustomerList
                            from d in c.Documents
                            where d.DocumentExpires <= expireCheck
                            && d.ExpireNotificationDisabled == false
                            group d by new { Building = b } into grouped
                            select new
                            {
                                Building = grouped.Key.Building,
                                Items = grouped.Select(a => new ExpiryItem
                                {
                                    Id = a.id,
                                    DocumentType = a.CustomerDocumentType.Name,
                                    Expires = a.DocumentExpires,
                                    AccountNumber = a.Customer.AccountNumber
                                })
                            };



            foreach (var expiredAccount in qry.ToList())
            {
                SendNotification(expiredAccount.Building,expiredAccount.Items);
                foreach (var itm in expiredAccount.Items)
                {
                    _Context.CustomerDocumentNotificationSent(itm.Id);
                }
            }
        }

        private void SendNotification(tblBuilding building, IEnumerable<ExpiryItem> items)
        {
            string status;
            var pm = building.pm;

            if (String.IsNullOrWhiteSpace(pm))
                pm = "tertia@astrodon.co.za";

            string bodyContent = "Good day" + Environment.NewLine +
                "Please note that building " + building.Building + " has the following document(s) set to expire:" + Environment.NewLine;

            foreach(var d in items.OrderBy(a => a.AccountNumber).ThenBy(a => a.Expires))
            {
                bodyContent = bodyContent + "Account: " + d.AccountNumber + " " + d.DocumentType + " expires on " + d.Expires.Value.ToString("yyyy/MM/dd", CultureInfo.InstalledUICulture) + Environment.NewLine;
            }

            if (String.IsNullOrWhiteSpace(bodyContent))
                bodyContent = "";

            bodyContent = bodyContent + Environment.NewLine + Environment.NewLine;

            bodyContent += "Kind Regards" + Environment.NewLine;
            bodyContent += "Tel: 011 867 3183" + Environment.NewLine;
            bodyContent += "Fax: 011 867 3163" + Environment.NewLine;
            bodyContent += "Direct Fax: 086 657 6199" + Environment.NewLine;
            bodyContent += "BEE Level 4 Contributor" + Environment.NewLine;

            bodyContent += "FOR AND ON BEHALF OF ASTRODON(PTY) LTD" + Environment.NewLine;
            bodyContent += "The information contained in this communication is confidential and may be legally privileged.It is intended solely for the use of the individual or entity to whom it is addressed and others authorized to receive it.If you are not the intended recipient you are hereby notified that any disclosure, copying, distribution or taking action in reliance of the contents of this information is strictly prohibited and may be unlawful.The company is neither liable for proper, complete transmission of the information contained in this communication nor any delay in its receipt." + Environment.NewLine;


            List<string> toAddress = new List<string>();
            toAddress.Add(pm);


            if (!Mailer.SendMailWithAttachments("noreply@astrodon.co.za", toAddress.Distinct().ToArray(),
                "Customer document expiry notification " + building.Building, bodyContent,
                false, false, false, out status, new Dictionary<string, byte[]>(), "tertia@astrodon.co.za"))
            {
                Console.WriteLine("Error seding email " + status, "Email error");
            }
        }

        class ExpiryItem
        {
            public string AccountNumber { get; internal set; }
            public string DocumentType { get; set; }
            public DateTime? Expires { get; set; }
            public int Id { get; internal set; }
        }

      

    }
}
