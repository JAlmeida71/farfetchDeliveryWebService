using DataAccessLayer.Utils;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.LocalDataAccess
{
    public static class LocalLogData
    {
        private static List<Log> data;
        public static List<Log> GetData()
        {
            if (data == null)
            {
                data = new List<Log>();
            }
            return data;
        }
        public static void Reboot()
        {
            data = new List<Log>();
        }

    }
    public class LocalLogDataAccess : ILogDataAccess
    {
        public Log Create(Log model)
        {
            model.ID = LocalLogData.GetData().Count + 1;
            LocalLogData.GetData().Add(model);
            return model;
        }
    }
}
