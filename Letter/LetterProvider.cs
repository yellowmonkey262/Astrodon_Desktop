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

     
    }
}
