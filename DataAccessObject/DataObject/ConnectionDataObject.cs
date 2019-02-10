using DataAccessObject.Utils;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessObject.DataObject
{
    public class ConnectionDataObject : DataObjectHelper
    {
        public List<SqlParameter> SetSqlParamenters(Connection model)
        {
            List<SqlParameter> SqlParamenters = new List<SqlParameter>();
            SqlParamenters.Add(new SqlParameter("@StartNode_ID", model.StartNode.ID));
            SqlParamenters.Add(new SqlParameter("@EndNode_ID", model.EndNode.ID));
            SqlParamenters.Add(new SqlParameter("@Cost", model.Cost));
            SqlParamenters.Add(new SqlParameter("@Time", model.Time));
            return SqlParamenters;
        }

        public List<Connection> buildModelListFromReader(SqlDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();
            List<Connection> model = new List<Connection>();

            while (reader.Read())
            {
                model.Add(readerToModel(reader, schema));
            }
            reader.Close();

            return model;
        }

        public Connection buildModelFromReader(SqlDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();
            Connection model = new Connection();

            if (reader.Read())
            {
                model = readerToModel(reader, schema);
            }
            reader.Close();

            return model;
        }

        internal Connection readerToModel(SqlDataReader reader, DataTable schema)
        {
            Connection model = new Connection();

            if (DoesColumnExist(schema, "ID")) model.ID = GetInt(reader["ID"]);
            if (DoesColumnExist(schema, "StartNode_ID")) model.StartNode.ID = GetInt(reader["StartNode_ID"]);
            if (DoesColumnExist(schema, "EndNode_ID")) model.EndNode.ID = GetInt(reader["EndNode_ID"]);
            if (DoesColumnExist(schema, "Cost")) model.Cost = GetDecimal(reader["Cost"]);
            if (DoesColumnExist(schema, "Time")) model.Time = GetDecimal(reader["Time"]);

            return model;
        }

    }
}
