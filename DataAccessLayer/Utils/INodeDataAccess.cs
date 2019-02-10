using Models.BussinessModels;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Utils
{
    public interface INodeDataAccess
    {
        List<Node> GetAll(Filter filter);
        Node Get(int id);
        Node Create(Node model);
        void Update(Node model);
        void Delete(int id);
    }
}
