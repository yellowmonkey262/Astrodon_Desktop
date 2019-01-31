using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Astrodon.CustomerMaintenance
{
    [DataContract]
    public class CustomerCategory
    {
        public CustomerCategory()
        {

        }
        public CustomerCategory(DataRow row)
        {
            CategoryId = (short)row["CCCode"];
            CategoryName = (string)row["CCDesc"];
            CategoryName = CategoryName.Trim();
        }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string CategoryName { get; set; }


        public static List<CustomerCategory> CategoryList
        {
            get
            {
                var result = new List<CustomerCategory>()
                {
                    new CustomerCategory(){ CategoryId  = 0, CategoryName   = "None".ToUpper() },
                    new CustomerCategory(){ CategoryId  = 1, CategoryName   = "Debit Orders".ToUpper() },
                    new CustomerCategory() { CategoryId = 2, CategoryName = "Units in Transfer".ToUpper() },
                    new CustomerCategory() { CategoryId = 3, CategoryName = "Arrangements".ToUpper() },
                    new CustomerCategory() { CategoryId = 4, CategoryName = "Unit Disconnected".ToUpper() },
                    new CustomerCategory() { CategoryId = 5, CategoryName = "Legal Hand Over".ToUpper() },
                    new CustomerCategory() { CategoryId = 6, CategoryName = "Astrodon Rentals".ToUpper() },
                    new CustomerCategory() { CategoryId = 7, CategoryName = "Trustees".ToUpper() },
                    new CustomerCategory() { CategoryId = 8, CategoryName = "Registrations".ToUpper() },
              //      new CustomerCategory() { CategoryId = 9, CategoryName = "Creditors" },
             //       new CustomerCategory() { CategoryId = 10, CategoryName = "Unallocated deposits" },
                    new CustomerCategory() { CategoryId = 11, CategoryName = "Transferred Units/PMS".ToUpper() }

            };
                return result;
            }
        }

    }
}
