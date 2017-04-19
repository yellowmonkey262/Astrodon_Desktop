using Astro.Library.Entities;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace Astrodon
{
    public class SMS
    {
        private const String username = "astrodon_sms";
        private const String password = "[sms@66r94e!@#]";
        private SqlDataHandler dh;

        public SMS()
        {
            dh = new SqlDataHandler();
        }

        private double GetCredits(out String status)
        {
            string url = "http://bulksms.2way.co.za:5567/eapi/user/get_credits/1/1.1";
            string data = "";
            data += "username=" + HttpUtility.UrlEncode(username, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            data += "&password=" + HttpUtility.UrlEncode(password, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            String result = Post(url, data);
            string[] parts = result.Split('|');
            if (parts.Length > 1)
            {
                string statusCode = parts[0];
                string statusString = parts[1];
                if (statusCode == "0")
                {
                    status = "";
                    return double.Parse(statusString);
                }
                else
                {
                    status = statusString;
                    return -1;
                }
            }
            else
            {
                status = parts[0];
                return -1;
            }
        }

        public string Post(string url, string data)
        {
            string result = null;
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(data);

                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url);
                WebReq.Method = "POST";
                WebReq.ContentType = "application/x-www-form-urlencoded";
                WebReq.ContentLength = buffer.Length;
                Stream PostData = WebReq.GetRequestStream();

                PostData.Write(buffer, 0, buffer.Length);
                PostData.Close();
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                Console.WriteLine(WebResp.StatusCode);

                Stream Response = WebResp.GetResponseStream();
                StreamReader _Response = new StreamReader(Response);
                result = _Response.ReadToEnd();
            }
            catch (Exception ex)
            {
                result += "\n" + ex.Message;
            }
            return result.Trim() + "\n";
        }

        public string createMessage(string msisdn, string message)
        {
            string data = "";
            data += "username=" + HttpUtility.UrlEncode(username, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            data += "&password=" + HttpUtility.UrlEncode(password, System.Text.Encoding.GetEncoding("ISO-8859-1"));
            data += "&message=" + HttpUtility.UrlEncode(character_resolve(message), System.Text.Encoding.GetEncoding("ISO-8859-1"));
            data += "&msisdn=" + msisdn;
            data += "&want_report=1";
            return data;
        }

        public string character_resolve(string body)
        {
            Hashtable chrs = new Hashtable();
            chrs.Add('Ω', "Û");
            chrs.Add('Θ', "Ô");
            chrs.Add('Δ', "Ð");
            chrs.Add('Φ', "Þ");
            chrs.Add('Γ', "¬");
            chrs.Add('Λ', "Â");
            chrs.Add('Π', "º");
            chrs.Add('Ψ', "Ý");
            chrs.Add('Σ', "Ê");
            chrs.Add('Ξ', "±");

            string ret_str = "";
            foreach (char c in body)
            {
                if (chrs.ContainsKey(c))
                {
                    ret_str += chrs[c];
                }
                else
                {
                    ret_str += c;
                }
            }
            return ret_str;
        }

        public Hashtable send_sms(string data, string url)
        {
            string sms_result = Post(url, data);
            Hashtable result_hash = new Hashtable();
            string tmp = "";
            tmp += "Response from server: " + sms_result + "\n";
            string[] parts = sms_result.Split('|');
            string statusCode = parts[0];
            string statusString = parts[1];
            result_hash.Add("api_status_code", statusCode);
            result_hash.Add("api_message", statusString);
            if (parts.Length != 3)
            {
                tmp += "Error: could not parse valid return data from server.\n";
            }
            else
            {
                if (statusCode.Equals("0"))
                {
                    result_hash.Add("success", 1);
                    result_hash.Add("api_batch_id", parts[2]);
                    tmp += "Message sent - batch ID " + parts[2] + "\n";
                }
                else if (statusCode.Equals("1"))
                {
                    // Success: scheduled for later sending.
                    result_hash.Add("success", 1);
                    result_hash.Add("api_batch_id", parts[2]);
                }
                else
                {
                    result_hash.Add("success", 0);
                    tmp += "Error sending: status code " + parts[0] + " description: " + parts[1] + "\n";
                }
            }
            result_hash.Add("details", tmp);
            return result_hash;
        }

        public string formatted_server_response(Hashtable result)
        {
            string ret_string = "";
            if ((int)result["success"] == 1)
            {
                ret_string += "Success: batch ID " + (string)result["api_batch_id"] + "API message: " + (string)result["api_message"] + "\nFull details " + (string)result["details"];
            }
            else
            {
                ret_string += "Fatal error: HTTP status " + (string)result["http_status_code"] + " API status " + (string)result["api_status_code"] + " API message " + (string)result["api_message"] + "\nFull details " + (string)result["details"];
            }
            return ret_string;
        }

        public bool SendSMS(String phoneNumber, String message, out String status, out String batchID)
        {
            double credits = GetCredits(out status);
            batchID = "";
            if (credits > 0)
            {
                if (!phoneNumber.StartsWith("27"))
                {
                    if (phoneNumber.StartsWith("0")) { phoneNumber = phoneNumber.Substring(1); }
                    phoneNumber = "27" + phoneNumber;
                }

                string url = "http://bulksms.2way.co.za:5567/eapi/submission/send_sms/2/2.0";
                Hashtable result;

                string data = createMessage(phoneNumber, message);
                result = send_sms(data, url);
                if ((int)result["success"] == 1)
                {
                    batchID = result["api_batch_id"].ToString().Replace("\n", "");
                    status = formatted_server_response(result);
                    return true;
                }
                else
                {
                    status = formatted_server_response(result);
                    return false;
                }
            }
            else
            {
                if (credits == 0) { status = "Insufficient credits"; }
                return false;
            }
        }

        public bool SendBulkMessage(SMSMessage msg, bool immediate, out String status)
        {
            if (msg.id == 0)
            {
                //billable, bulkbillable, astStatus, batchID, status, nextPolled, pollCount
                msg.id = dh.SaveOutboundMessage(msg.id, msg.building, msg.customer, msg.number, msg.reference, msg.message, msg.billable, msg.bulkbillable, msg.sent, msg.sender, msg.astStatus,
                msg.batchID, msg.status, msg.nextPolled, msg.pollCount, msg.cbal, msg.smsType, out status);
            }
            else
            {
                dh.SaveOutboundMessage(msg.id, msg.building, msg.customer, msg.number, msg.reference, msg.message, msg.billable, msg.bulkbillable, msg.sent, msg.sender, msg.astStatus,
                msg.batchID, msg.status, msg.nextPolled, msg.pollCount, msg.cbal, msg.smsType, out status);
            }
            String bid = "";
            String[] numbers = msg.number.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (immediate)
            {
                foreach (String number in numbers)
                {
                    bool success = SendSMS(number, String.Format("{0}", msg.message), out status, out bid);
                    if (success)
                    {
                        msg.batchID += bid + ";";
                    }
                }
                dh.SaveOutboundMessage(msg.id, msg.building, msg.customer, msg.number, msg.reference, msg.message, msg.billable, msg.bulkbillable, msg.sent, msg.sender, msg.astStatus,
                msg.batchID, msg.status, msg.nextPolled, msg.pollCount, msg.cbal, msg.smsType, out status);
            }
            return true;
        }

        public bool SendMessage(SMSMessage msg, bool immediate, out String status)
        {
            int rs = 0;
            status = String.Empty;
            if (msg.id == 0)
            {
                //billable, bulkbillable, astStatus, batchID, status, nextPolled, pollCount
                msg.id = dh.SaveQueuedMessage(msg.building, msg.customer, msg.cbal, msg.smsType, msg.number, msg.reference, msg.message, msg.billable, msg.bulkbillable, msg.sent, msg.sender, msg.astStatus,
                msg.batchID, msg.status);
                msg.reference = String.Format("{0}/{1}", msg.customer, msg.id);
                rs = dh.SaveOutboundMessage(msg.id, msg.building, msg.customer, msg.number, msg.reference, msg.message, msg.billable, msg.bulkbillable, msg.sent, msg.sender, msg.astStatus,
                msg.batchID, msg.status, msg.nextPolled, msg.pollCount, msg.cbal, msg.smsType, out status);
                if (msg.smsType == "Disconnection SMS" || immediate)
                {
                    String bid;
                    bool success = SendSMS(msg.number, String.Format("{0}", msg.message), out status, out bid);
                    if (success)
                    {
                        msg.batchID = bid;
                        msg.status = "SENT";
                        dh.SaveOutboundMessage(msg.id, msg.building, msg.customer, msg.number, msg.reference, msg.message, msg.billable, msg.bulkbillable, msg.sent, msg.sender, msg.astStatus,
                        msg.batchID, msg.status, msg.nextPolled, msg.pollCount, msg.cbal, msg.smsType, out status);
                    }
                    else
                    {
                        MessageBox.Show("Cannot send sms to " + msg.customer);
                    }
                }
            }
            return (rs != -1 ? true : false);
        }
    }
}