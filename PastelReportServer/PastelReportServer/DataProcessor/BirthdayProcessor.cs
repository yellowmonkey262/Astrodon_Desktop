using Astrodon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Astrodon.Data.CustomerData;

namespace Astrodon.DataProcessor
{
    public class BirthdayProcessor
    {
        private DataContext _Context;
        public BirthdayProcessor(DataContext dc)
        {
            _Context = dc;
        }

        public void Process()
        {
            var todayMonth = DateTime.Today.Month;
            var todayDay = DateTime.Today.Day;
            var checkDate = DateTime.Today.AddDays(-7);


            var customers = from c in _Context.CustomerSet
                            where c.DateOfBirth != null
                            && c.SendBirthdayNotification == true
                            && (c.LastBirthdayNotificationSent == null || c.LastBirthdayNotificationSent < checkDate)
                            && c.DateOfBirth.Value.Month == todayMonth
                            && c.DateOfBirth.Value.Day == todayDay
                            select c;

            foreach(var bdayCustomer in customers.ToList())
            {
                SendNotificationSMS(bdayCustomer);
            }
        }

        private void SendNotificationSMS(Customer bdayCustomer)
        {
            if (!String.IsNullOrWhiteSpace(bdayCustomer.CellNumber))
            {
                string smsText = "Happy birthday " + bdayCustomer.CustomerFullName + ". We hope you have a wonderful day. Regards Astrodon";

                string number = bdayCustomer.CellNumber;
                var sms = new SMS();
                string status;
                string batchId;

                sms.SendSMS(number,smsText, out status, out batchId);

                bdayCustomer.LastBirthdayNotificationSent = DateTime.Now;
                bdayCustomer.BirthdaySMSText = smsText;
                bdayCustomer.BirthDaySMSStatus = status;
                bdayCustomer.BirthDaySMSBatch = batchId;

                _Context.SaveChanges();


            }

        }
    }
}
