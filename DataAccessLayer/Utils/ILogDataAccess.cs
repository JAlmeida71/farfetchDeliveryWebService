using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Utils
{
    public interface ILogDataAccess
    {
        Log Create(Log model);
    }
}
