using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class MyReminders
    {
        public int RemID { get; set; }

        public String Building { get; set; }

        public String Customer { get; set; }

        public DateTime ReminderDate { get; set; }

        public String Reminder { get; set; }

        public String Contacts { get; set; }

        public String Phone { get; set; }

        public String Fax { get; set; }

        public String Email { get; set; }

        public bool Action { get; set; }
    }
}