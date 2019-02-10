using DataAccessLayer.DataAccess;
using DataAccessLayer.Utils;
using Models.BussinessModels;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Text;
using static Models.UtilsModels.LogActionEnum;

namespace BusinessLogicLayer.Service
{
    public static class Logger
    {
        public static void Register
            (
                ILogDataAccess llogDA,
                eLogAction action,
                eLogResult result,
                object model,
                User user,
                int model_id,
                string message,
                Exception ex
            )
        {
            int user_id = (user == null ? 0 : user.ID);
            new LogService(llogDA)
            .CreateLog(new Log()
            {
                Date = DateTime.UtcNow,
                Action = action,
                Result = result,
                ModelName = model.GetType().Name,
                ModelID = model_id,
                UserID = user_id,
                ExceptionName = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                AddedMessage = message
            });
        }

        public static void Register(
                ILogDataAccess llogDA,
                eLogAction action,
                eLogResult result,
                object model,
                User user,
                int model_id,
                string message

            )
        {
            int user_id = (user == null ? 0 : user.ID);
            new LogService(llogDA)
            .CreateLog(new Log()
            {
                Date = DateTime.UtcNow,
                Action = action,
                Result = result,
                ModelName = model.GetType().Name,
                ModelID = model_id,
                UserID = user_id,
                ExceptionName = String.Empty,
                Message = String.Empty,
                StackTrace = String.Empty,
                AddedMessage = message
            });
        }

    }

    public class LogService
    {
        private ILogDataAccess logDA;

        public LogService(ILogDataAccess iLogDA) => logDA = iLogDA;

        public void CreateLog(Log model) => logDA.Create(model);
      
    }
}
