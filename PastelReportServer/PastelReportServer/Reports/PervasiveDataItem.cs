using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Astrodon.Reports
{
    public class PervasiveDataItem: ReportDataBase
    {
        protected decimal ReadDecimal(DataRow row, string column)
        {
            if (row[column] == null || row[column] is DBNull)
                return 0;
            else
            {
                if (row[column].GetType() == typeof(System.Single))
                    return Convert.ToDecimal((Single)row[column]);
            }
            return Convert.ToDecimal(Math.Round((double)row[column], 2)); //possible data issue here
        }

    }
}
