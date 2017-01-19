namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblDebtor
    {
        public int id { get; set; }

        public int buildingID { get; set; }

        public DateTime completeDate { get; set; }

        [StringLength(50)]
        public string category { get; set; }

        public bool lettersupdated { get; set; }

        public bool lettersageanalysis { get; set; }

        public bool lettersprintemail { get; set; }

        public bool lettersfiled { get; set; }

        public bool stmtupdated { get; set; }

        public bool stmtinterest { get; set; }

        public bool stmtprintemail { get; set; }

        public bool stmtfiled { get; set; }

        public bool meupdate { get; set; }

        public bool meinvest { get; set; }

        public bool me9990 { get; set; }

        public bool me4000 { get; set; }

        public bool mepettycash { get; set; }

        public bool dailytrust { get; set; }

        public bool dailyown { get; set; }

        public bool dailyfile { get; set; }
    }
}
