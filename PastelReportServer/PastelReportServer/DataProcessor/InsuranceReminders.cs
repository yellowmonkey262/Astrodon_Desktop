using Astrodon.Classes;
using Astrodon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrodon.DataProcessor
{
    public class InsuranceReminders
    {
        private DataContext _Context;

        public InsuranceReminders(DataContext dc)
        {
            _Context = dc;
        }

        public void Process()
        {
            var today = DateTime.Today;

            var endDate = today.AddDays(32);

            var buildings = from b in _Context.tblBuildings
                            where b.InsurancePolicyRenewalDate <= today
                            && b.InsurancePolicyExpiryDate >= today
                            && b.InsurancePolicyExpiryDate <= endDate
                            select b;

            foreach (var building in buildings.ToList())
            {
                SendNotification(building);
            }
        }

        private void SendNotification(tblBuilding building)
        {
            string status;
            var pm = building.pm;

            if (String.IsNullOrWhiteSpace(pm))
                pm = "tertia@astrodon.co.za";

            string bodyContent = "Good day" + Environment.NewLine +
                "Please note that the Insurance Policy for " + building.Building + " is set to expire on " + building.InsurancePolicyExpiryDate.Value.ToString("yyyy-MM-dd") + Environment.NewLine +
                "Please remember to update the insurance policy on the building." + Environment.NewLine;


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
            toAddress.Add(building.pm);


            if (!Mailer.SendMailWithAttachments("noreply@astrodon.co.za", toAddress.Distinct().ToArray(),
                "Insurnace Policy Expiry " + building.Building, bodyContent,
                false, false, false, out status, new Dictionary<string, byte[]>(), "tertia@astrodon.co.za"))
            {
                Console.WriteLine("Error seding email " + status, "Email error");
            }
        }

    }
}
