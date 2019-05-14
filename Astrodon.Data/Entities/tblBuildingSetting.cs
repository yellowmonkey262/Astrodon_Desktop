namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBuildingSetting
    {
        public int id { get; set; }

        public int buildingID { get; set; }

        public decimal reminderFee { get; set; }

        public decimal reminderSplit { get; set; }

        public decimal finalFee { get; set; }

        public decimal finalSplit { get; set; }

        public decimal disconnectionNoticefee { get; set; }

        public decimal disconnectionNoticeSplit { get; set; }

        public decimal summonsFee { get; set; }

        public decimal summonsSplit { get; set; }

        public decimal disconnectionFee { get; set; }

        public decimal disconnectionSplit { get; set; }

        public decimal handoverFee { get; set; }

        public decimal handoverSplit { get; set; }

        public string reminderTemplate { get; set; }

        public string finalTemplate { get; set; }

        public string diconnectionNoticeTemplate { get; set; }

        public string summonsTemplate { get; set; }

        public string reminderSMS { get; set; }

        public string finalSMS { get; set; }

        public string disconnectionNoticeSMS { get; set; }

        public string summonsSMS { get; set; }

        public string disconnectionSMS { get; set; }

        public string handoverSMS { get; set; }

        public decimal DebitOrderFee { get; set; }

        public decimal SMSFee { get; set; }
    }
}
