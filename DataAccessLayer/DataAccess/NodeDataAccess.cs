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
    public class NodeDataAccess : INodeDataAccess
    {
        internal NodeDataObject nodeDO = new NodeDataObject();

        public List<Node> GetAll(Filter filter)
        {
            List<Node> listModel = new List<Node>();

            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                string query = NodeDataObjectQuery.Select;

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection))
                    listModel = nodeDO.buildModelListFromReader(command.ExecuteReader());
            }

            return listModel;
        }

        public Node Get(int id)
        {
            Node model = new Node();

            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                string query = NodeDataObjectQuery.Select
                + QueryFilterHelper.WhereString(true, new List<string> { "[Node].[ID]" }, "ID", id, ref parameters);

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                {
                    model = nodeDO.buildModelFromReader(command.ExecuteReader());
                }
            }

            return model;
        }

        public Node Create(Node model)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = nodeDO.SetSqlParamenters(model);
                string query = NodeDataObjectQuery.Insert;

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                    model.ID = (int)command.ExecuteScalar();
            }
            return model;
        }

        public void Update(Node model)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = nodeDO.SetSqlParamenters(model);
                string query = NodeDataObjectQuery.Update
                + QueryFilterHelper.WhereString(true, new List<string> { "[Node].[ID]" }, "ID", model.ID, ref parameters, true);

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
                string query = NodeDataObjectQuery.Delete
                + QueryFilterHelper.WhereString(true, new List<string> { "[Node].[ID]" }, "ID", id, ref parameters, true);

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                    command.ExecuteNonQuery();
            }
        }

    }
}
