using Astrodon.Data;
using Desktop.Lib.Pervasive;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.DataProcessor
{
    public class ODBCConnectionTest
    {
        private DataContext _Context;

        public ODBCConnectionTest(DataContext dc)
        {
            _Context = dc;
        }

        public void Process()
        {
            DateTime checkDate = DateTime.Today.AddDays(-2);

            var q = from b in _Context.tblBuildings
                    where b.BuildingDisabled == false
                    && (b.ODBCConnectionOK == false || b.LastODBConnectionTest == null || b.LastODBConnectionTest <= checkDate)
                    select b;

            foreach (var building in q.ToList())
            {
                building.ODBCConnectionOK = TestBuilding(building.DataPath);
                building.LastODBConnectionTest = DateTime.Today;
                _Context.SaveChanges();
            }
        }

        private bool TestBuilding(string dataPath)
        {
            try
            {
                string odbcQuery = "select ID from [DataSet].LedgerParameters";

                odbcQuery = PervasiveSqlUtilities.SetDataSource(odbcQuery, dataPath);

                foreach (DataRow row in PervasiveSqlUtilities.FetchPervasiveData(odbcQuery).Rows)
                {
                    return true; //data found
                }

                throw new Exception("LedgerParameters returned zero rows");

            }
            catch (Exception e)
            {
                _Context.SystemLogSet.Add(new Data.Log.SystemLog()
                {
                    EventTime = DateTime.Now,
                    Message = "ODBC Connection Test " + e.Message,
                    StackTrace = e.StackTrace
                });
            }
            return false;
        }
    }
}
