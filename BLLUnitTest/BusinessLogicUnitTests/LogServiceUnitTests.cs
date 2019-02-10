using BusinessLogicLayer.Service;
using DataAccessLayer.LocalDataAccess;
using DataAccessLayer.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Models.UtilsModels.LogActionEnum;

namespace BusinessLogicLayerUnitTests.BusinessLogicUnitTests
{
    [TestClass]
    public class LogServiceUnitTests
    {
        private LogService _classTest;
        private LocalLogDataAccess llogDA;
        [TestInitialize]
        public void OnTestInit()
        {
            llogDA = new LocalLogDataAccess();
            LocalLogData.Reboot();
            _classTest = new LogService(llogDA);
        }


        [TestMethod]
        public void CreateLog()
        {
            Log expected = new Log()
            {
                Date = new DateTime(),
                Action = eLogAction.Update,
                Result = eLogResult.Error,
                ModelName = "CLASS",
                ModelID = 1,
                UserID = 2,
                ExceptionName = "randomEx",
                Message = "ANDIWILLALWAYSLOVEYOUUUU",
                StackTrace = "IFYOULIKEDITTHANYOUSHOULDHAVEPUTARINGONIT",
                AddedMessage = "ENESSALADEIAOMENINOERAEU"
            };
            _classTest.CreateLog(expected);
            Assert.AreEqual(expected, LocalLogData.GetData().First());
        }
    }
}
