using DataAccessLayer.Utils;
using DataAccessObject.DataObject;
using DataAccessObject.Querys;
using DataAccessObject.Utils;
using Models.BussinessModels;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessLayer.DataAccess
{
    public class ConnectionDataAccess : IConnectionDataAccess
    {
        internal ConnectionDataObject connectionDO = new ConnectionDataObject();

        public List<Connection> GetAll(Filter filter)
        {
            List<Connection> listModel = new List<Connection>();

            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();
                List<SqlParameter> parameters = new List<SqlParameter>();

                string query = ConnectionDataObjectQuery.Select
                    + (filter.StartNode.ID > 0 
                        ? QueryFilterHelper.WhereString(true, new List<string> { "[Connection].[StartNode_ID]" }, "StartNode_ID", filter.StartNode.ID, ref parameters): String.Empty)
                    + (filter.EndNode.ID > 0 
                        ? QueryFilterHelper.WhereString(true, new List<string> { "[Connection].[EndNode_ID]" }, "EndNode_ID", filter.EndNode.ID, ref parameters):String.Empty);


                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                    listModel = connectionDO.buildModelListFromReader(command.ExecuteReader());
            }

            return listModel;
        }

        public Connection Get(int id)
        {
            Connection model = new Connection();

            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                string query = ConnectionDataObjectQuery.Select
                + QueryFilterHelper.WhereString(true, new List<string> { "[Connection].[ID]" }, "ID", id, ref parameters);

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                {
                    model = connectionDO.buildModelFromReader(command.ExecuteReader());
                }
            }

            return model;
        }

        public Connection Create(Connection model)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = connectionDO.SetSqlParamenters(model);
                string query = ConnectionDataObjectQuery.Insert;

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                    model.ID = (int)command.ExecuteScalar();
            }
            return model;
        }

        public void Update(Connection model)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = connectionDO.SetSqlParamenters(model);
                string query = ConnectionDataObjectQuery.Update
                + QueryFilterHelper.WhereString(true, new List<string> { "[Connection].[ID]" }, "ID", model.ID, ref parameters, true);

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                    command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                string query = ConnectionDataObjectQuery.Delete
                + QueryFilterHelper.WhereString(true, new List<string> { "[Connection].[ID]" }, "ID", id, ref parameters, true);

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                    command.ExecuteNonQuery();
            }
        }

        public int CountNodeConnection(int id)
        {
            int value = 0;

            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                string query = ConnectionDataObjectQuery.Count
                + QueryFilterHelper.WhereString(true, 
                new List<string> { "[Connection].[StartNode_ID]", "[Connection].[EndNode_ID]" }, 
                "ID", id, ref parameters);

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read()) value = reader.GetInt32(0);
                }
            }

            return value;
        }
    }
}
