using DataAccessLayer.Utils;
using Models.BussinessModels;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.LocalDataAccess
{

    public static class LocalConnectionData
    {

        private static List<Connection> data;

        public static List<Connection> GetData()
        {
            if (data == null)
            {
                data = new List<Connection>();
            }
            return data;
        }
        public static void Reboot()
        {
            data = new List<Connection>();
        }

    }

    public class LocalConnectionDataAccess : IConnectionDataAccess
    {
        public List<Connection> GetAll(Filter filter)
        {
            List<Connection> list = LocalConnectionData.GetData();
            if (filter.StartNode.ID > 0)
                list = list.Where(c => c.StartNode.ID == filter.StartNode.ID).ToList();
            if (filter.EndNode.ID > 0)
                list = list.Where(c => c.EndNode.ID == filter.EndNode.ID).ToList();

            return list;
        }

        public Connection Get(int id)
        {
            if (LocalConnectionData.GetData().Where(n => n.ID == id).Count() == 1)
                return LocalConnectionData.GetData().Where(n => n.ID == id).First();
            else
                return new Connection();
        }

        public Connection Create(Connection model)
        {
            model.ID = LocalConnectionData.GetData().Count + 1;
            LocalConnectionData.GetData().Add(model);
            return model;
        }

        public void Update(Connection model)
        {
            Get(model.ID).StartNode = model.StartNode;
            Get(model.ID).EndNode = model.EndNode;
            Get(model.ID).Cost = model.Cost;
            Get(model.ID).Time = model.Time;
        }

        public void Delete(int id)
        {
            LocalConnectionData.GetData().Remove(Get(id));
        }

        public int CountNodeConnection(int id)
        {
            return LocalConnectionData.GetData().Where(c => c.StartNode.ID == id).Count()
                + LocalConnectionData.GetData().Where(c => c.EndNode.ID == id).Count();
        }
    }
}
