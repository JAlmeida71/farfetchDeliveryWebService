using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DataAccessObject.Utils;
using Models.BussinessModels;

namespace DataAccessObject.DataObject
{
    public class NodeDataObject : DataObjectHelper 
    {
        public List<SqlParameter> SetSqlParamenters(Node model)
        {
            List<SqlParameter> SqlParamenters = new List<SqlParameter>();
            SqlParamenters.Add(new SqlParameter("@Name", model.Name));
            return SqlParamenters;
        }

        public List<Node> buildModelListFromReader(SqlDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();
            List<Node> model = new List<Node>();

            while (reader.Read())
            {
                model.Add(readerToModel(reader, schema));
            }
            reader.Close();

            return model;
        }

        public Node buildModelFromReader(SqlDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();
            Node model = new Node();

            if (reader.Read())
            {
                model = readerToModel(reader, schema);
            }
            reader.Close();

            return model;
        }

        internal Node readerToModel(SqlDataReader reader, DataTable schema)
        {
            Node model = new Node();

            if (DoesColumnExist(schema, "ID")) model.ID = GetInt(reader["ID"]);
            if (DoesColumnExist(schema, "Name")) model.Name = GetString(reader["Name"]);

            return model;
        }
    }
}
