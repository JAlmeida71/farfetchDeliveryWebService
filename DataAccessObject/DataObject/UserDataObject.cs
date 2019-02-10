using DataAccessObject.Utils;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using static Models.Enum.UserRoleEnum;

namespace DataAccessObject.DataObject
{
    public class UserDataObject : DataObjectHelper
    {
        public List<SqlParameter> SetSqlParamenters(User model)
        {
            List<SqlParameter> SqlParamenters = new List<SqlParameter>();
            SqlParamenters.Add(new SqlParameter("@Username", model.Username));
            SqlParamenters.Add(new SqlParameter("@Password", model.Password));
            SqlParamenters.Add(new SqlParameter("@Role", model.Role));
            return SqlParamenters;
        }

        public List<User> buildModelListFromReader(SqlDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();
            List<User> model = new List<User>();

            while (reader.Read())
            {
                model.Add(readerToModel(reader, schema));
            }
            reader.Close();

            return model;
        }

        public User buildModelFromReader(SqlDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();
            User model = new User();

            if (reader.Read())
            {
                model = readerToModel(reader, schema);
            }
            reader.Close();

            return model;
        }

        internal User readerToModel(SqlDataReader reader, DataTable schema)
        {
            User model = new User();

            if (DoesColumnExist(schema, "ID")) model.ID = GetInt(reader["ID"]);
            if (DoesColumnExist(schema, "Username")) model.Username = GetString(reader["Username"]);
            if (DoesColumnExist(schema, "Password")) model.Password = GetString(reader["Password"]);
            if (DoesColumnExist(schema, "Role")) model.Role = (eUserRole)GetInt(reader["Role"]);

            return model;
        }
    }
}
