using DataAccessLayer.Utils;
using Models.BussinessModels;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.LocalDataAccess
{

    public static class LocalNodeData
    {

        private static List<Node> data;

        public static List<Node> GetData()
        {
            if (data == null)
            {
                data = new List<Node>();
            }
            return data;
        }
        public static void Reboot()
        {
            data = new List<Node>();
        }

    }

    public class LocalNodeDataAccess : INodeDataAccess
    {
        public List<Node> GetAll(Filter filter)
        {
            return LocalNodeData.GetData();
        }

        public Node Get(int id)
        {
            if (LocalNodeData.GetData().Where(n => n.ID == id).Count() == 1)
                return LocalNodeData.GetData().Where(n => n.ID == id).First();
            else
                return new Node();
        }

        public Node Create(Node model)
        {
            model.ID = LocalNodeData.GetData().Count + 1;
            LocalNodeData.GetData().Add(model);
            return model;
        }

        public void Update(Node model)
        {
            Get(model.ID).Name = model.Name;
        }

        public void Delete(int id)
        {
            LocalNodeData.GetData().Remove(Get(id));
        }
    }
}
