using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Astrodon.DataContracts
{
    [DataContract]
    public class PervasiveItem
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


        protected int ReadInt(DataRow row, string column)
        {
            if (row[column] == null || row[column] is DBNull)
                return 0;
            else
                return (int)row[column];
        }

        protected string ReadString(DataRow row, string column)
        {
            if (row[column] == null || row[column] is DBNull)
                return null;
            else
                return (row[column] as string).Trim();
        }

        protected DateTime ReadDate(DataRow row, string column)
        {
            if (row[column] == null || row[column] is DBNull)
                return new DateTime(1900,1,1);
            else
                return (DateTime)row[column];
        }
    }
}
