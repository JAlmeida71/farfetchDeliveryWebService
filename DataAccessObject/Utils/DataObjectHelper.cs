using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessObject.Utils
{
    public  class DataObjectHelper
    {
        public static void AddParameter(List<SqlParameter> parms, string colunm_name, object obj, bool isId = false)
        {
            if (!colunm_name.StartsWith("@"))
                colunm_name = "@" + colunm_name;

            if (obj != null && (isId && (int)obj > 0))
                parms.Add(new SqlParameter(colunm_name, obj));
            else
                parms.Add(new SqlParameter(colunm_name, DBNull.Value));

        }

        public static bool DoesColumnExist(DataTable schema, string columnName)
        {
            schema.DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            return (schema.DefaultView.Count > 0);
        }
       
        #region Data Gets

        public static bool GetBool(object obj)
        {
            return (obj == DBNull.Value ? new bool() : (bool)obj);
        }

        public static string GetString(object obj)
        {
            return (obj == DBNull.Value ? String.Empty : (string)obj);
        }

        public static int GetInt(object obj)
        {
            return (obj == DBNull.Value ? 0 : (int)obj);
        }

        public static decimal GetDecimal(object obj)
        {
            return (obj == DBNull.Value ? 0 : (decimal)obj);
        }

        #endregion
    }
}
