using Astrodon.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Threading;
using System.Linq;

namespace Astrodon
{
    public class Mailer
    {
        static string smtpHost = "10.0.1.1";
        static Data.tblUser _LastUserSent = null;
        static string bccAlwaysTo = "noreply@astrodon.co.za";

        public Mailer()
        {
            // TODO: Add constructor logic here
        }

        private static String generatHTMLEmail(String requestString, String emailString, string fromEmail)
        {
            if (!emailString.ToLower().Contains("<br />"))
                emailString = emailString.Replace(Environment.NewLine, "<br />");

            string html = ResourceManager.EmailLayout(requestString);
            html = html.Replace("{{SenderEmail}}", fromEmail);
            html = html.Replace("{{CONTENT-GOES-HERE}}", emailString);

            if (_LastUserSent == null || _LastUserSent.email != fromEmail)
            {

                using (var context = SqlDataHandler.GetDataContext())
                {
                    var sender = context.tblUsers.Where(a => a.email == fromEmail).FirstOrDefault();
                    if(sender == null)
                    {
                        _LastUserSent = new Data.tblUser()
                        {
                            email = fromEmail,
                            name = "Astrodon",
                            phone = "011 867 3183",
                            fax = "011 867 3163"
                        };
                    }
                    else
                    {
                        _LastUserSent = sender;

                        if (string.IsNullOrWhiteSpace(_LastUserSent.phone))
                            _LastUserSent.phone = "011 867 3183";
                        if (string.IsNullOrWhiteSpace(_LastUserSent.fax))
                            _LastUserSent.fax = "011 867 3163";
                    }

                }
            }

            html = html.Replace("{{SENDER_TEL_NUMBER}}", _LastUserSent.phone);
            html = html.Replace("{{SENDER_FAX_NUMBER}}", _LastUserSent.fax);
            html = html.Replace("{{SENDER_NAME}}", _LastUserSent.name);


            /*

            String html = "";
            html += "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>";
            html += "<html xmlns='http://www.w3.org/1999/xhtml'>";
            html += "<head>";
            html += "<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />";
            html += "<title>" + requestString + "</title>";
            html += "</head>";
            html += "<body>";

            html += emailString;

            html += "</body>";
            html += "</html>";*/

            return html;
        }

        public static bool SendMailWithAttachments(String fromEmail, String[] toMail,
            String subject, String message, bool addcc, bool readreceipt, out String status,
            Dictionary<string, byte[]> attachments, string bccEmail = null)
        {
            Dictionary<string, MemoryStream> attachmentStreams = new Dictionary<string, MemoryStream>();
            foreach (string key in attachments.Keys)
            {
                attachmentStreams.Add(key, new MemoryStream(attachments[key]));
            }
            String mailBody = "";
            status = String.Empty;
            mailBody = generatHTMLEmail(subject, message,fromEmail);

            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage objMail = new MailMessage();
                MailAddress objMail_fromaddress = new MailAddress(fromEmail);
                try
                {
                    foreach (String emailAddress in toMail)
                    {
                        MailAddress objMail_toaddress = new MailAddress(emailAddress);
                        objMail.To.Add(objMail_toaddress);
                    }
                }
                catch (Exception ex)
                {
                    status = "Invalid email address";
                    foreach (var stream in attachmentStreams.Values)
                    {
                        stream.Close();
                        stream.Dispose();
                    }

                    return false;
                }
                if (!string.IsNullOrWhiteSpace(bccEmail))
                {
                    objMail.Bcc.Add(bccEmail);
                }
                objMail.From = objMail_fromaddress;
                objMail.IsBodyHtml = true;
                objMail.Body = mailBody;
                objMail.Priority = MailPriority.High;
                if (addcc)
                {
                    MailAddress cc = new MailAddress(fromEmail);
                    objMail.CC.Add(cc);
                }
                try
                {
                    foreach (string key in attachmentStreams.Keys)
                    {
                        objMail.Attachments.Add(new Attachment(attachmentStreams[key], key));
                    }
                }
                catch
                {
                    status = "Invalid attachment";
                    foreach (var stream in attachmentStreams.Values)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                    return false;
                }

                smtpClient.Host = smtpHost;

                try
                {
                    objMail.Subject = subject;
                    if (readreceipt)
                        objMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                    objMail.Bcc.Add(new MailAddress(bccAlwaysTo));
                    smtpClient.Send(objMail);
                }
                catch (Exception ex)
                {
                    status = ex.Message;
                    foreach (var stream in attachmentStreams.Values)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
                foreach (var stream in attachmentStreams.Values)
                {
                    stream.Close();
                    stream.Dispose();
                }
                return false;
            }
            foreach (var stream in attachmentStreams.Values)
            {
                stream.Close();
                stream.Dispose();
            }
            return true;
        }

        public static bool SendMail(String fromEmail, String[] toMail,
            String subject, String message, bool addcc, bool readreceipt,
            out String status, String[] attachments = null)
        {

            if (attachments != null && attachments.Length == 0)
                attachments = null;

            String mailBody = "";
            status = String.Empty;
            mailBody = generatHTMLEmail(subject, message, fromEmail);
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage objMail = new MailMessage();
                MailAddress objMail_fromaddress = new MailAddress(fromEmail);
                bool toAdded = false;
                try
                {
                    foreach (String emailAddress in toMail)
                    {
                        if (!String.IsNullOrWhiteSpace(emailAddress) && emailAddress.Contains("@"))
                        {
                            MailAddress objMail_toaddress = new MailAddress(emailAddress);
                            objMail.To.Add(objMail_toaddress);
                            toAdded = true;
                        }
                    }
                }
                catch
                {
                    status = "Invalid email address";
                    return false;
                }
                if (!toAdded)
                {
                    status = "Invalid email address";
                    return false;
                }
                objMail.From = objMail_fromaddress;
                objMail.IsBodyHtml = true;
                objMail.Body = mailBody;
                objMail.Priority = MailPriority.High;
                if (addcc)
                {
                    MailAddress cc = new MailAddress(fromEmail);
                    objMail.CC.Add(cc);
                }
                try
                {
                    if (attachments != null && attachments.Length > 0)
                    {
                        foreach (String attachment in attachments)
                        {
                            objMail.Attachments.Add(new Attachment(attachment));
                        }
                    }
                }
                catch
                {
                    status = "Invalid attachment";
                    return false;
                }

                smtpClient.Host = smtpHost;

                try
                {
                    objMail.Subject = subject;
                    if (readreceipt)
                    {
                        objMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                    }
                    objMail.Bcc.Add(new MailAddress(bccAlwaysTo));
                    smtpClient.Send(objMail);
                }
                catch (Exception ex)
                {
                    status = ex.Message;
                    return false;
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
            return true;
        }

        public static bool SendDirectMail(String fromEmail, String[] toMail, String cc, String bcc, String subject, String message,
            bool readreceipt, out String status, String[] attachments = null)
        {

            String mailBody = "";
            status = String.Empty;
            mailBody = generatHTMLEmail(subject, message, fromEmail);
            try
            {
                String errorTrapper = "";
                SmtpClient smtpClient = new SmtpClient();
                MailMessage objMail = new MailMessage();
                errorTrapper = "From address = " + fromEmail;
                MailAddress objMail_fromaddress = new MailAddress(fromEmail);


                bool toAdded = false;
                try
                {
                    foreach (String emailAddress in toMail)
                    {
                        if (!String.IsNullOrWhiteSpace(emailAddress) && emailAddress.Contains("@"))
                        {
                            MailAddress objMail_toaddress = new MailAddress(emailAddress);
                            objMail.To.Add(objMail_toaddress);
                            toAdded = true;
                        }
                    }
                }
                catch
                {
                    status = "Invalid email address";
                    return false;
                }
                if (!toAdded)
                {
                    status = "Invalid email address";
                    return false;
                }


                objMail.From = objMail_fromaddress;
                objMail.IsBodyHtml = true;
                errorTrapper = "Mail body";
                objMail.Body = mailBody;
                objMail.Priority = MailPriority.High;
                if (!String.IsNullOrEmpty(cc))
                {
                    String[] ccAddys = cc.Split(new String[] { ";", "," }, StringSplitOptions.None);
                    foreach (String ccAddy in ccAddys)
                    {
                        try
                        {
                            MailAddress mcc = new MailAddress(ccAddy.Trim());
                            objMail.CC.Add(mcc);
                        }
                        catch { }
                    }
                }
                if (!String.IsNullOrEmpty(bcc))
                {
                    String[] bccs = bcc.Split(new String[] { ";", "," }, StringSplitOptions.None);
                    foreach (String bcca in bccs)
                    {
                        try
                        {
                            if (bcca.Trim().ToLower() != bccAlwaysTo)
                            {
                                MailAddress mcc = new MailAddress(bcca.Trim());
                                objMail.Bcc.Add(mcc);
                            }
                        }
                        catch { }
                    }
                }
                try
                {
                    if (attachments != null && attachments.Length > 0)
                    {
                        foreach (String attachment in attachments)
                        {
                            objMail.Attachments.Add(new Attachment(attachment));
                        }
                    }
                }
                catch
                {
                    status = "Invalid attachment";
                    return false;
                }

                smtpClient.Host = smtpHost;

                try
                {
                    objMail.Subject = subject;
                    if (readreceipt)
                    {
                        objMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                    }
                    objMail.Bcc.Add(new MailAddress(bccAlwaysTo));
                    smtpClient.Send(objMail);
                }
                catch (Exception ex)
                {
                    status = ex.Message;
                    return false;
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
            return true;
        }

        public static bool SendMail(String fromEmail, String[] toMail, String subject, String message,
            bool addcc, bool readreceipt, out String status, Dictionary<String, byte[]> attachments = null)
        {

            String mailBody = "";
            status = String.Empty;
            mailBody = generatHTMLEmail(subject, message, fromEmail);
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage objMail = new MailMessage();
                MailAddress objMail_fromaddress = new MailAddress(fromEmail);
                try
                {
                    foreach (String emailAddress in toMail)
                    {
                        MailAddress objMail_toaddress = new MailAddress(emailAddress.Trim());
                        objMail.To.Add(objMail_toaddress);
                    }
                }
                catch (Exception ex)
                {
                    status = "Invalid email address";
                    return false;
                }
                objMail.From = objMail_fromaddress;
                objMail.IsBodyHtml = true;
                objMail.Body = mailBody;
                objMail.Priority = MailPriority.High;
                if (addcc)
                {
                    MailAddress cc = new MailAddress(fromEmail);
                    objMail.CC.Add(cc);
                }
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (KeyValuePair<String, byte[]> attachment in attachments)
                    {
                        try
                        {
                            MemoryStream ms = new MemoryStream(attachment.Value);
                            objMail.Attachments.Add(new Attachment(ms, attachment.Key));
                        }
                        catch (Exception ex)
                        {
                            status = "Invalid attachment";
                            continue;
                        }
                    }
                }
                smtpClient.Host = smtpHost;

                try
                {
                    objMail.Subject = subject;
                    if (readreceipt)
                    {
                        objMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                    }
                    objMail.Bcc.Add(new MailAddress(bccAlwaysTo));
                    smtpClient.Send(objMail);
                }
                catch (Exception ex)
                {
                    status = ex.Message;
                    return false;
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
            return true;
        }

        private static bool CheckPort()
        {
            int port = 25; //<--- This is your value
            bool isAvailable = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }
            return isAvailable;
        }

        public static bool SendMail(String fromEmail, String[] toMail, String cc, String bcc, String subject, String message, out String status, Dictionary<String, byte[]> attachments = null)
        {

            int count = 0;
            while (count <= 3 && !CheckPort())
            {
                Thread.Sleep(2000);
                count += 1;
            }
            if (!CheckPort())
            {
                status = "Port 25 in use or unavailable. Please try again later. If the problem persists please request IT to open this port on your firewall.";
                return false;
            }
            String mailBody = "";
            status = String.Empty;
            mailBody = generatHTMLEmail(subject, message, fromEmail);
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                MailMessage objMail = new MailMessage();
                MailAddress objMail_fromaddress = new MailAddress(fromEmail);
                try
                {
                    foreach (String emailAddress in toMail)
                    {
                        try
                        {
                            MailAddress objMail_toaddress = new MailAddress(emailAddress);
                            objMail.To.Add(objMail_toaddress);
                        }
                        catch { }
                    }
                }
                catch (Exception ex)
                {
                    status = "Invalid email address";
                    return false;
                }
                if (objMail.To.Count == 0)
                {
                    status = "Invalid email address";
                    return false;
                }
                objMail.From = objMail_fromaddress;
                objMail.IsBodyHtml = true;
                objMail.Body = mailBody;
                objMail.Priority = MailPriority.High;
                if (cc.Trim() != "")
                {
                    String[] ccMail = cc.Split(new String[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (String ccAddy in ccMail)
                    {
                        MailAddress ccAddress = new MailAddress(ccAddy.Trim());
                        objMail.CC.Add(ccAddress);
                    }
                }
                if (bcc.Trim() != "")
                {
                    String[] bccMail = bcc.Split(new String[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (String bccAddy in bccMail)
                    {
                        if (bccAddy.ToLower().Trim() != bccAlwaysTo)
                        {
                            MailAddress bccAddress = new MailAddress(bccAddy.Trim());
                            objMail.Bcc.Add(bccAddress);
                        }
                    }
                }
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (KeyValuePair<String, byte[]> attachment in attachments)
                    {
                        try
                        {
                            MemoryStream ms = new MemoryStream(attachment.Value);
                            objMail.Attachments.Add(new Attachment(ms, attachment.Key));
                        }
                        catch (Exception ex)
                        {
                            status = "Invalid attachment";
                            continue;
                        }
                    }
                }

                smtpClient.Host = smtpHost;

                try
                {
                    objMail.Subject = subject;
                    objMail.Bcc.Add(new MailAddress(bccAlwaysTo));
                    smtpClient.Send(objMail);
                }
                catch (Exception ex)
                {
                    status = ex.Message + "-" + ex.StackTrace;
                    return false;
                }
            }
            catch (Exception ex)
            {
                status = ex.Message + "-" + ex.StackTrace;
                return false;
            }
            return true;
        }
    }
}
