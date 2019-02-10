using Models.BussinessModels;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Utils
{
    public interface IConnectionDataAccess
    {
         List<Connection> GetAll(Filter filter);
         Connection Get(int id);
         Connection Create(Connection model);
         void Update(Connection model);
         void Delete(int id);
         int CountNodeConnection(int id);
    }
}
