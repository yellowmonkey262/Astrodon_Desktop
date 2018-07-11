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

        public bool Process
        {
            get { return process; }
            set { process = value; }
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

        private int _buildingId = 0;

        public int GetBuildingId()
        {
            return _buildingId;
        }

        public StatementBuilding(int buildingId, String build, String dp, int p, DateTime lastProcessed)
        {
            _buildingId = buildingId;
            Process = false;
            Building = build;
            DataPath = dp;
            Period = p;
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