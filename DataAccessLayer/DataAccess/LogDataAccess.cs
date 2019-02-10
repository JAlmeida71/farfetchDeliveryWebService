using DataAccessLayer.Utils;
using DataAccessObject.DataObject;
using DataAccessObject.Querys;
using DataAccessObject.Utils;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessLayer.DataAccess
{
    public class LogDataAccess : ILogDataAccess
    {
        internal LogDataObject logDO = new LogDataObject();

        public Log Create(Log model)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = logDO.SetSqlParamenters(model);
                string query = LogDataObjectQuery.Insert;

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                    model.ID = (int)command.ExecuteScalar();
            }
            return model;
        }
    }
}
