using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.LocalDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Models.Enum.UserRoleEnum;

namespace BusinessLogicLayerUnitTests.BusinessLogicUnitTests
{
    [TestClass]
    public class UserLogicUnitTests
    {
        private UserLogic _classTest;
        private LocalUserDataAccess luDA;
        private LocalLogDataAccess llDA;

        #region Init Data
        protected User user_a = new User() { ID = 1, Username = "Admin", Password = "Admin", Role = eUserRole.Admin };
        protected User user_u = new User() { ID = 2, Username = "User", Password = "User", Role = eUserRole.User };
        #endregion

        [TestInitialize]
        public void OnTestInit()
        {
            luDA = new LocalUserDataAccess();
            llDA = new LocalLogDataAccess();
            _classTest = new UserLogic(luDA, llDA);
            LocalUserData.Reboot();
            LocalUserData.GetData().Add(user_a);
            LocalUserData.GetData().Add(user_u);
        }

        [TestMethod]
        public void GetAll()
        {

            Assert.IsTrue(_classTest.GetAll().Count == 2);
        }

        [TestMethod]
        public void Get()
        {
            Assert.IsTrue(_classTest.Get(99).ID == 0);
            Assert.IsTrue(_classTest.Get(1).Equals(user_a));
            Assert.IsTrue(_classTest.Get(2).Equals(user_u));
        }

        [TestMethod]
        public void Create()
        {
            User fail = new User();

            User success = new User()
            {
                ID = 99,
                Username = "success",
                Password = "success",
                Role = eUserRole.User
            };

            User resultfail = _classTest.Create(fail);

            Assert.IsTrue(resultfail.ID == 0);
            Assert.IsTrue(LocalUserData.GetData().Count == 2);

            User resultSuccess = _classTest.Create(success);

            Assert.IsTrue(resultSuccess.ID != 0);
            Assert.IsTrue(LocalUserData.GetData().Count != 2);
        }


        [TestMethod]
        public void Update()
        {
            User fail_1 = new User();

            User fail_2 = new User()
            {
                ID = 99,
            };

            User same = new User() {
                ID = 1,
                Username = "Admin",
                Password = "Admin",
                Role = eUserRole.Admin
            };


            User success = new User()
            {
                ID = 1,
                Username = "Admin",
                Password = "Farfetch'd",
                Role = eUserRole.User
            };

            User resultfail_1 = _classTest.Update(1, fail_1);
            Assert.IsTrue(resultfail_1.ID == 0);
            User resultfail_2 = _classTest.Update(1, fail_2);
            Assert.IsTrue(resultfail_2.ID == 0);

            User resultSame = _classTest.Update(1,same);
            Assert.IsTrue(resultSame.ID == same.ID);
            Assert.IsTrue(resultSame.Password == same.Password);

            User resultSuccess = _classTest.Update(1, success);
            Assert.IsTrue(resultSuccess.ID == resultSuccess.ID);
            Assert.IsTrue(resultSuccess.Password == success.Password);
        }


        [TestMethod]
        public void Delete()
        {
            int previous = LocalUserData.GetData().Count;
            _classTest.Delete(1);

            Assert.IsTrue(previous > LocalUserData.GetData().Count);
            Assert.IsTrue(LocalUserData.GetData().Where(n => n.ID == 1).Count() == 0);
        }

        [TestMethod]
        public void Exists()
        {
            Assert.IsFalse(_classTest.Exists(99));
            Assert.IsTrue(_classTest.Exists(1));
        }

        [TestMethod]
        public void AttemptLogin()
        {
            User fail_1 = new User() { ID = 99 };
            User fail_2 = new User()
            {
                ID = 1,
                Username = "Admin",
                Password = "dif-Admin",
            };
            User success = new User()
            {
                ID = 1,
                Username = "Admin",
                Password = "Admin",
                Role = eUserRole.Admin
            };

            Assert.AreNotEqual(_classTest.AttemptLogin(fail_1), fail_1);
            Assert.AreNotEqual(_classTest.AttemptLogin(fail_2), fail_2);
            Assert.IsTrue(_classTest.AttemptLogin(success).Equals(success));
        }
    }
}
