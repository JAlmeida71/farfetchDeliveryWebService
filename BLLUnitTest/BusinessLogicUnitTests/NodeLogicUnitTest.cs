using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.LocalDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogicLayerUnitTests.BusinessLogicUnitTests
{

    [TestClass]
    public class NodeLogicUnitTest
    {

        private NodeLogic _classTest;
        private LocalConnectionDataAccess lcDA;
        private LocalNodeDataAccess lnDA;
        private LocalLogDataAccess llDA;
        [TestInitialize]
        public void OnTestInit()
        {
            lcDA = new LocalConnectionDataAccess();
            lnDA = new LocalNodeDataAccess();
            llDA = new LocalLogDataAccess();
            LocalConnectionData.Reboot();
            LocalNodeData.Reboot();
            _classTest = new NodeLogic(lcDA, lnDA, llDA);
            Node firstNode = new Node() { ID = 1,Name = "FirstNode" };
            LocalNodeData.GetData().Add(firstNode);
        }

        [TestMethod]
        public void GetAll()
        {
            Assert.IsTrue(_classTest.GetAll().Count == 1);
        }

        [TestMethod]
        public void Get()
        {
            Assert.IsTrue(_classTest.Get(2).ID == 0);
            Assert.IsTrue(_classTest.Get(1).ID == 1 && _classTest.Get(1).Name.Equals("FirstNode"));
        }

        [TestMethod]
        public void Create()
        {
            Node nodefail = new Node()
            {
                Name = String.Empty
            };

            Node nodeSuccess = new Node()
            {
                Name = "Farfetch'd"
            };

            Node resultfail = _classTest.Create(nodefail);

            Assert.IsTrue(resultfail.ID == 0);
            Assert.IsTrue(LocalNodeData.GetData().Count() == 1);

            Node resultSuccess = _classTest.Create(nodeSuccess);

            Assert.IsTrue(resultSuccess.ID != 0);
            Assert.IsTrue(LocalNodeData.GetData().Count() != 1);
        }


        [TestMethod]
        public void Update()
        {
            Node nodefail = new Node()
            {
                Name = String.Empty
            };

            Node nodeSame = new Node()
            {
                ID = 1,
                Name = "FirstNode"
            };

            Node nodeSuccess = new Node()
            {
                ID = 1,
                Name = "Farfetch'd"
            };

            Node resultfail = _classTest.Update(1, nodefail);
            Assert.IsTrue(resultfail.ID == 0);

            Node resultSame = _classTest.Update(1, nodeSame);
            Assert.IsTrue(resultSame.ID == nodeSame.ID);
            Assert.IsTrue(LocalNodeData.GetData().First().Name == nodeSame.Name);

            Node resultSuccess = _classTest.Update(1, nodeSuccess);
            Assert.IsTrue(resultSuccess.ID == resultSuccess.ID);
            Assert.IsTrue(LocalNodeData.GetData().First().Name != "FirstNode");
        }


        [TestMethod]
        public void Delete()
        {
            int previous = LocalNodeData.GetData().Count;
            _classTest.Delete(1);

            Assert.IsTrue(previous > LocalNodeData.GetData().Count);
            Assert.IsTrue(LocalNodeData.GetData().Where(n => n.ID == 1).Count() == 0);
        }


        [TestMethod]
        public void NodeExists()
        {
            Assert.IsFalse(_classTest.NodeExists(2));
            Assert.IsTrue(_classTest.NodeExists(1));
        }


    }
}
