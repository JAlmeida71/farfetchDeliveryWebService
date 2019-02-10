using DataAccessLayer.Utils;
using DataAccessObject.DataObject;
using DataAccessObject.Querys;
using DataAccessObject.Utils;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DataAccessLayer.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {

        internal UserDataObject userDO = new UserDataObject();

        public List<User> GetAll()
        {
            List<User> list = new List<User>();

            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                string query = UserDataObjectQuery.Select;

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection))
                    list = userDO.buildModelListFromReader(command.ExecuteReader());
            }

            return list;
        }

        public User Get(int id)
        {
            User model = new User();

            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                string query = UserDataObjectQuery.Select
                + QueryFilterHelper.WhereString(true, new List<string> { "[User].[ID]" }, "ID", id, ref parameters);

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                {
                    model = userDO.buildModelFromReader(command.ExecuteReader());
                }
            }

            return model;
        }

        public User Create(User model)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = userDO.SetSqlParamenters(model);
                string query = UserDataObjectQuery.Insert;

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                    model.ID = (int)command.ExecuteScalar();
            }
            return model;
        }

        public void Update(User model)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = userDO.SetSqlParamenters(model);
                string query = UserDataObjectQuery.Update
                + QueryFilterHelper.WhereString(true, new List<string> { "[User].[ID]" }, "ID", model.ID, ref parameters, true);

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
                string query = UserDataObjectQuery.Delete
                + QueryFilterHelper.WhereString(true, new List<string> { "[User].[ID]" }, "ID", id, ref parameters, true);

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                    command.ExecuteNonQuery();
            }
        }

        public User Exists(string username)
        {
            User model = new User();

            using (SqlConnection connection = new SqlConnection(SqlConnectionHelper.getConnectionString()))
            {
                connection.Open();

                List<SqlParameter> parameters = new List<SqlParameter>();
                string query = UserDataObjectQuery.Select
                + QueryFilterHelper.WhereString(true, new List<string> { "[User].[Username]" }, "Username", username, ref parameters);

                using (SqlCommand command = SqlConnectionHelper.CreateCommand(query, connection, parameters))
                {
                    model = userDO.buildModelFromReader(command.ExecuteReader());
                }
            }

            return model;
        }


    }

}
