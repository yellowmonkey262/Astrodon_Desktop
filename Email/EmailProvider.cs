﻿using System;
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


            if (attachments != null)
            {
                string attachmentPart = "<br /><br />";

                string attachmentTemplate = ResourceManager.ReadAttachmentTemplate();


                foreach (var fileName in attachments.Keys.ToList())
                {
                    string html = attachmentTemplate;
                    html = html.Replace("{{FILENAME}}", Path.GetFileNameWithoutExtension(fileName));
                    html = html.Replace("{{URL}}", attachments[fileName]);

                    attachmentPart = attachmentPart + Environment.NewLine + html;
                }

                if (!message.Contains("<br />"))
                    message = message.Replace(Environment.NewLine, "<br />");

                message = message + Environment.NewLine + attachmentPart;
            }

            return Mailer.SendMail(fromAddress, emailAddys, subject, message, false, false, out status, new string[] { });
        }

        public static bool SendCustomerFile(string debtorEmail, string emailAddresses,bool addcc, string subject, string accountNumber, string url)
        {
            string emailBody = ResourceManager.ReadCustomerLetterTemplate();

            emailBody = emailBody.Replace("{{URL}}", url);
            emailBody = emailBody.Replace("{{Account_Number}}", accountNumber);
           
            string[] emailTo = emailAddresses.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            string status;
            return Mailer.SendMail(debtorEmail, emailTo, subject, emailBody, addcc, false, out status, new string[] { });
        }


        public static bool SendDirectMail(string[] emails, string cc, string bcc,string subject,string message, string[] attachments)
        {
           string status;
           return Mailer.SendDirectMail(Controller.user.email, emails,cc, bcc, subject, message, true, out status, attachments);
        }

        public static bool SendStatement(string debtorEmail, string[] emailTo, string customerAccountNumber, string fileName, DateTime statementDate, string statmentUrl, bool rental)
        {
            string status;
            string subject = Path.GetFileNameWithoutExtension(fileName) + " " + DateTime.Now.ToString();

            string emailBody = ResourceManager.ReadStatementTemplate();

            string ownerType = (rental ? "tenant" : "owner");
            emailBody = emailBody.Replace("{{OWNER_TYPE}}", ownerType);
            emailBody = emailBody.Replace("{{URL}}", statmentUrl);

            return Mailer.SendMail(debtorEmail, emailTo, subject, emailBody,  false, false, out status, new string[] { });
        }


        public static bool SendUserJobEmail(string fromEmail, string[] toEmail, string cc, string bcc, string subject, string message)
        {
            string status;
            return Mailer.SendDirectMail(fromEmail, toEmail, cc, bcc, subject, message, false, out status);
        }

   
        public static bool SendPaymentRequisitionMessage(string email, string message, string[] attachments)
        {
            string status;
            return Mailer.SendMail(Controller.user.email, new string[] { email }, "Payment Requisitions", message, false, false, out status, attachments);
        }

        public static bool SendClearanceCertificate(string fromAddress, string toAddress,string[] attachments)
        {
            string status;
            return Mailer.SendMail(fromAddress, new String[] { toAddress }, "Clearance Certificate", "Please find attached clearance certificate", false, false, out status, attachments);
        }

        internal static bool SendCalendarInvite(string fromEmail, string[] toEmail, string subject, string bodyContent, Dictionary<string, byte[]> attachments, string bccEmail)
        {
            string status;
            return Mailer.SendMail(fromEmail, toEmail, subject, bodyContent, false, false, out status, attachments);
        }
    }
}
