using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessObject.Utils
{
    public static class SqlConnectionHelper
    {
        private static string DataSource = "routeserviceff.database.windows.net";
        private static string UserID = "adminff";
        private static string Password = "admin_ff_xpto123";
        private static string InitialCatalog = "RoutesServiceDatabase";

        private static string ConnectionString = String.Empty;

        public static string getConnectionString()
        {
            if (String.IsNullOrEmpty(ConnectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = DataSource;
                builder.UserID = UserID;
                builder.Password = Password;
                builder.InitialCatalog = InitialCatalog;
                ConnectionString = builder.ConnectionString;
            }
            return ConnectionString;
        }

        public static SqlCommand CreateCommand(string query,
            SqlConnection connection,
            List<SqlParameter> parms )
        {
            SqlCommand command = new SqlCommand(query, connection);

            for (int i = 0; i < parms.Count; i++)
            {
                command.Parameters.Add(parms[i]);
            }

            return command;
        }

        public static SqlCommand CreateCommand(string query,SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(query, connection);

            return command;
        }

    }
}
