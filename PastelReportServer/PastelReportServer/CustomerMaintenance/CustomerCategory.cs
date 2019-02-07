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

                    //NORMAL CATEGORIES below 100
                    new CustomerCategory(){ CategoryId  = 1, CategoryName   = "Debit Orders".ToUpper() },
                    new CustomerCategory() { CategoryId = 2, CategoryName = "Units in Transfer".ToUpper() },
                    new CustomerCategory() { CategoryId = 3, CategoryName = "Arrangements".ToUpper() },
                    new CustomerCategory() { CategoryId = 4, CategoryName = "Unit Disconnected".ToUpper() },
                    new CustomerCategory() { CategoryId = 5, CategoryName = "Legal Hand Over".ToUpper() },
                    new CustomerCategory() { CategoryId = 6, CategoryName = "Astrodon Rentals".ToUpper() },
                    new CustomerCategory() { CategoryId = 7, CategoryName = "Trustees".ToUpper() },
                    new CustomerCategory() { CategoryId = 8, CategoryName = "Registrations".ToUpper() },
                    new CustomerCategory() { CategoryId = 10, CategoryName = "Unallocated Deposits".ToUpper() },
                    new CustomerCategory() { CategoryId = 11, CategoryName = "Transferred Units/PMS".ToUpper() },

                    //RENTAL CATEGORIES above 100

                     new CustomerCategory() { CategoryId = 101, CategoryName = "January".ToUpper() },
                     new CustomerCategory() { CategoryId = 102, CategoryName = "February".ToUpper() },
                     new CustomerCategory() { CategoryId = 103, CategoryName = "March".ToUpper() },
                     new CustomerCategory() { CategoryId = 104, CategoryName = "April".ToUpper() },
                     new CustomerCategory() { CategoryId = 105, CategoryName = "May".ToUpper() },
                     new CustomerCategory() { CategoryId = 106, CategoryName = "June".ToUpper() },
                     new CustomerCategory() { CategoryId = 107, CategoryName = "July".ToUpper() },
                     new CustomerCategory() { CategoryId = 108, CategoryName = "August".ToUpper() },
                     new CustomerCategory() { CategoryId = 109, CategoryName = "September".ToUpper() },
                     new CustomerCategory() { CategoryId = 110, CategoryName = "October".ToUpper() },
                     new CustomerCategory() { CategoryId = 111, CategoryName = "November".ToUpper() },
                     new CustomerCategory() { CategoryId = 112, CategoryName = "December".ToUpper() },

                     new CustomerCategory() { CategoryId = 113, CategoryName = "2011".ToUpper() },
                     new CustomerCategory() { CategoryId = 114, CategoryName = "MTM".ToUpper() },
                     new CustomerCategory() { CategoryId = 115, CategoryName = "Arrangement".ToUpper() },
                     new CustomerCategory() { CategoryId = 116, CategoryName = "Vacated".ToUpper() },
                     new CustomerCategory() { CategoryId = 117, CategoryName = "Sinai Marketing - Off Admin".ToUpper() },
                     new CustomerCategory() { CategoryId = 118, CategoryName = "March 6 months".ToUpper() },


            };
                return result;
            }
        }

    }
}
