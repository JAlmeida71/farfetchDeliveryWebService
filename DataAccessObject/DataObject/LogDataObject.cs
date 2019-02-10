using DataAccessObject.Utils;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessObject.DataObject
{
    public class LogDataObject : DataObjectHelper
    {
        public List<SqlParameter> SetSqlParamenters(Log model)
        {
            List<SqlParameter> SqlParamenters = new List<SqlParameter>();
            SqlParamenters.Add(new SqlParameter("@Date", model.Date));
            SqlParamenters.Add(new SqlParameter("@User_ID", model.UserID));
            SqlParamenters.Add(new SqlParameter("@Action", model.Action.ToString()));
            SqlParamenters.Add(new SqlParameter("@ModelName", model.ModelName));
            SqlParamenters.Add(new SqlParameter("@Model_ID", model.ModelID));
            SqlParamenters.Add(new SqlParameter("@Result", model.Result.ToString()));
            SqlParamenters.Add(new SqlParameter("@ExceptionName", model.ExceptionName));
            SqlParamenters.Add(new SqlParameter("@Message", model.Message));
            SqlParamenters.Add(new SqlParameter("@StackTrace", model.StackTrace));
            SqlParamenters.Add(new SqlParameter("@AddedMessage", model.AddedMessage));
            return SqlParamenters;
        }
    }
}
