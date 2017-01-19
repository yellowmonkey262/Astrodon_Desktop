namespace Astrodon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblUserBuilding
    {
        public int id { get; set; }

        public int userid { get; set; }

        public int buildingid { get; set; }
    }
}
