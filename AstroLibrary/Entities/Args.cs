using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class SqlArgs : EventArgs
    {
        public String msgArgs;

        public SqlArgs(String args)
        {
            msgArgs = args;
        }
    }

    public class MessageArgs : EventArgs
    {
        public String message { get; set; }

        public MessageArgs(String Message)
        {
            message = Message;
        }
    }

    public class CustomerArgs : EventArgs
    {
        public Customer customer { get; set; }

        public String building { get; set; }

        public CustomerArgs(Customer c, String b)
        {
            customer = c; building = b;
        }
    }
}