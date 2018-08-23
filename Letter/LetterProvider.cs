using Astro.Library.Entities;
using Astrodon.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Astrodon.Letter
{
    public class LetterProvider
    {
        public static byte[] CreateIntransferLetter(Customer customer, Building building, User pm)
        {
            byte[] result = null;

            string html = ResourceManager.ReadFilePath("Astrodon.Letter.Templates.IntransferLetter.html");

            var pdf = new PDF();

            pdf.CreatePALetter(customer, building, pm, "Unit in Transfer " + customer.accNumber, DateTime.Today, html, null, false,true, out result);

            return result;
        }

        public static void Test()
        {
            var building = new Buildings(false).buildings.Where(a => a.ID == 212).First();
            var pm = new Users().GetUser(20);

            using (var context = SqlDataHandler.GetDataContext())
            {
                var customer = context.CustomerSet.Where(a => a.BuildingId == 212).First();
                var cust = new Customer()
                {
                    accNumber = customer.AccountNumber,
                };
                var data = CreateIntransferLetter(cust, building, pm);
                File.WriteAllBytes(@"C:\Temp\Test.pdf", data);
            }
        }
    }
}
