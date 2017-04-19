using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class SummRep
    {
        private String _trustCode, _buildingName, _bank, _code;
        private Double _buildBalance, _centrecBalance, _difference;

        public String Bank { get { return _bank; } set { _bank = value; } }

        public String Code { get { return _code; } set { _code = value; } }

        public String TrustCode { get { return _trustCode; } set { _trustCode = value; } }

        public String BuildingName { get { return _buildingName; } set { _buildingName = value; } }

        public Double BuildBal { get { return _buildBalance; } set { _buildBalance = value; } }

        public Double CentrecBal { get { return _centrecBalance; } set { _centrecBalance = value; } }

        public Double Difference { get { return _difference; } set { _difference = value; } }
    }
}