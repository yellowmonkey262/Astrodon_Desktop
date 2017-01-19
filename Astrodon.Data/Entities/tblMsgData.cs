namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMsgData")]
    public partial class tblMsgData
    {
        public int id { get; set; }

        public int msgID { get; set; }

        public string Name { get; set; }

        [StringLength(50)]
        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }
}
