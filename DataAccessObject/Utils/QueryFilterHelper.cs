using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models.UtilsModels;

namespace DataAccessObject.Utils
{
    public class QueryFilterHelper
    {

        public static string WhereString(bool equals
       , List<string> columns
       , string parameterName
       , object value
       , ref List<SqlParameter> parms
       , bool firstQueryElement = false
       )
        {
            string ret = String.Empty;

            if (columns.Count > 0 && !String.IsNullOrWhiteSpace(parameterName) && value != null && parms != null)
            {
                ret = " " + (firstQueryElement ? "WHERE" : "AND") + "\n\t(";

                for (int i = 0, size = columns.Count; i < size; i++)
                {
                    if (i != (size - 1))
                    {
                        if (equals)
                            ret += columns[i] + " = @" + parameterName + " OR ";
                        else
                            ret += columns[i] + " LIKE '%' + @" + parameterName + " + '%' OR ";
                    }
                    else
                    {
                        if (equals)
                            ret += columns[i] + " = @" + parameterName;
                        else
                            ret += columns[i] + " LIKE '%' + @" + parameterName + " + '%'";
                    }
                }

                parms.Add(new SqlParameter("@" + parameterName, value));

                ret += ")\n";
            }

            return ret;
        }
    
    }
}
