using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Astrodon.Email
{
    public class EmailProvider
    {
        //uRequisitionBatch line 906
        public static void RequisitionBatchSendPaymentNotifications(string fromEmail, string notifyEmailAddress, string contactPerson, decimal amount, string payreference)
        {
            string status;

            string emailBody = ResourceManager.ReadRequisitionPaymentTemplate();

            emailBody = emailBody.Replace("{{CONTACT_PERSON}}", contactPerson);
            emailBody = emailBody.Replace("{{AMOUNT}}", "R" + amount.ToString("###,##0.00", CultureInfo.InvariantCulture));
            emailBody = emailBody.Replace("{{REFERENCE}}", payreference);

            try
            {
                Mailer.SendMail(fromEmail, new string[] { notifyEmailAddress }, "Payment Scheduled", emailBody, false, false, out status, new string[] { });
            }
            catch (Exception e)
            {
                Controller.HandleError(e);
            }
        }

        public static bool SendRequisitionNotification(string toEmail, Dictionary<string, byte[]> attachments)
        {
            string status;
            return Mailer.SendMailWithAttachments(Controller.user.email, new string[] { "payments@astrodon.co.za", toEmail },
              "Payment Requisitions",
              "Please find attached requisitions", false, false, out status, attachments);
        }

        public static bool SendBulkMail(string fromAddress, string[] emailAddys, string subject, string message, Dictionary<string, string> attachments)
        {
            string status;
            //attachments --all files must be uploaded to an account and these are the links to the files
            message = GenerateAndAppendAttachentLinks(message, attachments);
            return Mailer.SendMail(fromAddress, emailAddys, subject, message, false, false, out status, new string[] { });
        }

        public static bool SendCustomerFile(string debtorEmail, string emailAddresses, bool addcc, string subject, 
            string accountNumber,bool isRental, string url, out string status)
        {
            string emailBody = ResourceManager.ReadCustomerLetterTemplate();

            emailBody = emailBody.Replace("{{URL}}", url);
            emailBody = emailBody.Replace("{{Account_Number}}", accountNumber);
            emailBody = emailBody.Replace("{{OWNERTYPE}}", isRental ? " Tenant":"Owner");

            string[] emailTo = emailAddresses.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            return Mailer.SendMail(debtorEmail, emailTo, subject, emailBody, addcc, false, out status, new string[] { });
        }

        public static bool SendDirectMail(int buildingId, string accountNumber, string[] emails, string cc, string bcc, string subject, string message, Dictionary<string, string> attachments)
        {
            string status;

            message = GenerateAndAppendAttachentLinks(message, attachments);

            return Mailer.SendDirectMail(Controller.user.email, emails, cc, bcc, subject, message, true, out status, new string[] { });
        }

        public static bool SendStatement(string debtorEmail, string[] emailTo, string customerAccountNumber,
            string fileName, DateTime statementDate, string statmentUrl, bool rental)
        {
            string status;
            string subject = Path.GetFileNameWithoutExtension(fileName) + " " + DateTime.Now.ToString();

            string emailBody = ResourceManager.ReadStatementTemplate();

            string ownerType = (rental ? "tenant" : "owner");
            emailBody = emailBody.Replace("{{OWNER_TYPE}}", ownerType);
            emailBody = emailBody.Replace("{{URL}}", statmentUrl);
            emailBody = emailBody.Replace("{{Account_Number}}", customerAccountNumber);

            return Mailer.SendMail(debtorEmail, emailTo, subject, emailBody, false, false, out status, new string[] { });
        }

        public static bool SendUserJobEmail(string fromEmail, string[] toEmail, string cc, string bcc, string subject, string message, Dictionary<string, string> attachments)
        {
            message = GenerateAndAppendAttachentLinks(message, attachments);
            string status;
            return Mailer.SendDirectMail(fromEmail, toEmail, cc, bcc, subject, message, false, out status);
        }

        public static bool SendPaymentRequisitionMessage(string email, string message, string[] attachments)
        {
            string status;
            return Mailer.SendMail(Controller.user.email, new string[] { email }, "Payment Requisitions", message, false, false, out status, attachments);
        }

        public static bool SendClearanceCertificate(string fromAddress, string toAddress, string[] attachments)
        {
            string status;
            return Mailer.SendMail(fromAddress, new String[] { toAddress }, "Clearance Certificate", "Please find attached clearance certificate", false, false, out status, attachments);
        }

        public static bool SendCalendarInvite(string fromEmail, string[] toEmail, string subject, string bodyContent, Dictionary<string, byte[]> attachments, string bccEmail)
        {
            string status;
            return Mailer.SendMail(fromEmail, toEmail, subject, bodyContent, false, false, out status, attachments);
        }

        public static bool SendTrusteeCalendarInvite(string fromEmail, string[] toEmail, string subject, string bodyContent, Dictionary<string, byte[]> ical,
            string bccEmail, Dictionary<string, string> attachments)
        {
            bodyContent = GenerateAndAppendAttachentLinks(bodyContent, attachments);

            string status;
            return Mailer.SendMail(fromEmail, toEmail, subject, bodyContent, false, false, out status, ical);
        }

        private static string GenerateAndAppendAttachentLinks(string bodyContent, Dictionary<string, string> attachments)
        {
            if (attachments != null)
            {
                string attachmentPart = "<br /><br />";

                string attachmentTemplate = ResourceManager.ReadAttachmentTemplate();


                foreach (var fileName in attachments.Keys.ToList())
                {
                    string html = attachmentTemplate;
                    html = html.Replace("{{FILENAME}}", Path.GetFileNameWithoutExtension(fileName));
                    html = html.Replace("{{URL}}", attachments[fileName]);

                    attachmentPart = attachmentPart + "<br />" + html;
                }

           

                bodyContent = bodyContent + attachmentPart;
            }

            return bodyContent;
        }
    }
}