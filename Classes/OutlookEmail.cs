using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Astrodon.Classes
{
    public class OutlookEmail
    {
        public static void SendEmail(string address, string cc, string bcc, string subject, string message, List<Tuple<string,byte[]>> attachments)
        {
            string fileFolder = Path.GetTempPath();
            if (!fileFolder.EndsWith("\\"))
                fileFolder = fileFolder + "\\";


            Outlook.Application outlookApp = new Outlook.Application();

            Outlook.MailItem mailItem = outlookApp.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
            mailItem.Subject = subject;
            mailItem.To = address;
            if(!string.IsNullOrWhiteSpace(cc))
              mailItem.CC = cc;
            mailItem.Body = message;

            if(!string.IsNullOrWhiteSpace(bcc))
              mailItem.BCC = bcc;

            foreach(var file in attachments)
            {
                string filePath = fileFolder + file.Item1;
                if (File.Exists(filePath))
                    File.Delete(filePath);
                File.WriteAllBytes(filePath, file.Item2);

                mailItem.Attachments.Add(filePath, Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
            }

            mailItem.Display(true);


            foreach (var file in attachments)
            {
             
                string filePath = fileFolder + file.Item1;
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
}
