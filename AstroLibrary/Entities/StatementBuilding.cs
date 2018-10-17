using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astro.Library.Entities
{
    public class StatementBuilding
    {
        private bool process = false;
        private String building = String.Empty;
        private String dataPath = String.Empty;
        private int period = 0;
        public bool hoa = false;
        public bool bc = false;
        private String lastProcessed = String.Empty;

        private int _BuildingId { get; set; }
        private bool _ElevatedUser = false;

        public bool Process
        {
            get { return process; }
            set
            {
                if (_ElevatedUser)
                {
                    process = value;
                }
                else
                {
                    if (value && Allowed)
                        process = true;
                    else
                        process = false;
                }
            }
        }

        public String Building
        {
            get { return building; }
            set { building = value; }
        }

        public bool HOA
        {
            get { return hoa; }
        }

        public bool BC
        {
            get { return bc; }
        }

        public String LastProcessed
        {
            get { return lastProcessed; }
            set { lastProcessed = value; }
        }

        public String DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }

        public int Period
        {
            get { return period; }
            set { period = value; }
        }

        public bool Allowed
        {
            get;
            set;
        }

        public int GetBuildingId()
        {
            return _BuildingId;
        }

        public StatementBuilding(int buildingId, String build, String dp, int p, DateTime lastProcessed, bool elevatedUser)
        {
            Allowed = true;
            _ElevatedUser = elevatedUser;
            _BuildingId = buildingId;
            Process = false;
            Building = build;
            DataPath = dp;
            Period = p;
            if (lastProcessed > DateTime.Today.AddDays(-7))
                Allowed = false;
            LastProcessed = lastProcessed.ToString("yyyy/MM/dd");
            if (building.ToLower().Contains("hoa"))
            {
                hoa = true;
                bc = false;
            }
            else
            {
                hoa = false;
                bc = true;
            }
        }
    }
}