using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class User
    {
        public int id { get; set; }

        public String username { get; set; }

        public String password { get; set; }

        public bool admin { get; set; }

        public String email { get; set; }

        public String name { get; set; }

        public String phone { get; set; }

        public String fax { get; set; }

        public int usertype { get; set; }

        public List<int> buildings { get; set; }

        public Image signature { get; set; }
    }

    public class UserTypes
    {
        public List<UserType> types;

        public UserTypes()
        {
            types = new List<UserType>();
            types.Add(new UserType(1, "Admin"));
            types.Add(new UserType(2, "PM"));
            types.Add(new UserType(3, "Debtor"));
            types.Add(new UserType(4, "PA"));
        }
    }

    public class UserType
    {
        public int typeID { get; set; }

        public String type { get; set; }

        public UserType(int uTypeID, String uType)
        {
            typeID = uTypeID;
            type = uType;
        }
    }
}