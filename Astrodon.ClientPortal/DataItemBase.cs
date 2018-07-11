using System;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;

namespace Astrodon.ClientPortal
{
   
    public class DataItemBase
    {
        #region Ctor

        public DataItemBase()
        {

        }

        public DataItemBase(DataRow data)
        {
            if (data != null)
                Load(data);
        }

        #endregion

        protected DataRow SourceRow { get; private set; }

        protected virtual void Load(DataRow dataRow)
        {
            SourceRow = dataRow;
            string fieldName = string.Empty;

            try
            {
                foreach (PropertyInfo pi in GetType().GetProperties())
                {
                    if (System.Attribute.GetCustomAttribute(pi, typeof(FromDBAttribute)) != null)
                    {
                        fieldName = GetFieldName(pi.Name);
                        object dataValue = null;
                        dataValue = dataRow[fieldName];

                        if (dataValue == null || dataValue is DBNull)
                            pi.SetValue(this, null, null);
                        else
                        {
                            object val = GetSetValue(dataValue, pi, TrimStringValue(pi.Name));
                            pi.SetValue(this, val, null);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected object GetSetValue(object value, PropertyInfo pi, bool trimString)
        {
            if (value == null || value is DBNull)
                return null;

            Type type = pi.PropertyType;

            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                type = Nullable.GetUnderlyingType(type);

            if (type.IsEnum)
                return Enum.ToObject(type, value);

            if (Type.GetTypeCode(type) == TypeCode.Char)
            {
                string s = value.ToString();
                if (s.Length > 0)
                    return s[0];
            }

            if (Type.GetTypeCode(type) == TypeCode.String)
            {
                string s = value as string;

                if (s != null)
                {
                    if (trimString)
                        return s.Trim();
                    else
                        return s;
                }
            }

            if (Type.GetTypeCode(type) == TypeCode.Boolean)
            {
                if (value is Int32)
                    return (int)value > 0;
            }

            return value;
        }

        protected virtual string GetFieldName(string propertyName)
        {
            PropertyInfo pi = GetType().GetProperty(propertyName);
            var attr = System.Attribute.GetCustomAttribute(pi, typeof(FromDBAttribute)) as FromDBAttribute;

            if (attr != null && !String.IsNullOrWhiteSpace(attr.FieldName))
                return attr.FieldName;
            else
                return pi.Name;
        }

        protected virtual bool TrimStringValue(string propertyName)
        {
            PropertyInfo pi = GetType().GetProperty(propertyName);
            var attr = System.Attribute.GetCustomAttribute(pi, typeof(FromDBAttribute)) as FromDBAttribute;
            return attr.TrimStringValue;
        }

    }

    public class FromDBAttribute : System.Attribute
    {
        public FromDBAttribute(string fieldName = "", bool trimStringValue = true)
        {
            FieldName = fieldName;
            TrimStringValue = trimStringValue;
        }

        public string FieldName { get; set; }

        public bool TrimStringValue { get; set; }
    }

}
