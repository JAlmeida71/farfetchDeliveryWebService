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
    public class ConnectionLogicUnitTests
    {
        private ConnectionLogic _classTest;
        private LocalConnectionDataAccess lcDA;
        private LocalNodeDataAccess lnDA;
        private LocalLogDataAccess llDA;
        protected Node nA, nB, nC;
        protected Connection cAB, cAC;

        [TestInitialize]
        public void OnTestInit()
        {
            lcDA = new LocalConnectionDataAccess();
            lnDA = new LocalNodeDataAccess();
            llDA = new LocalLogDataAccess();

            _classTest = new ConnectionLogic(lcDA, lnDA, llDA);
            /*
                        [B]
                [A] -----|  
                        [C]
            A->B
            A->C
             */
            LocalConnectionData.Reboot();
            LocalNodeData.Reboot();
            nA = new Node() { ID = 1, Name = "A" };
            nB = new Node() { ID = 2, Name = "B" };
            nC = new Node() { ID = 3, Name = "C" };
            cAB = new Connection() { ID = 1, Cost = 1, Time = 1, StartNode = nA, EndNode = nB };
            cAC = new Connection() { ID = 2, Cost = 2, Time = 2, StartNode = nA, EndNode = nC };
            LocalConnectionData.GetData().Add(cAB);
            LocalConnectionData.GetData().Add(cAC);

        }

        [TestMethod]
        public void GetAll()
        {
            List<Connection> result = _classTest.GetAll();
            List<Connection> expected = LocalConnectionData.GetData();
            Assert.IsTrue(_classTest.GetAll().Count == LocalConnectionData.GetData().Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.IsTrue(expected[i].Equals(result[i]));
                Assert.IsTrue(expected[i].ID == result[i].ID);
            }

        }

        [TestMethod]
        public void Get()
        {
            Assert.IsTrue(_classTest.Get(1).ID == 1);
            Assert.IsFalse(_classTest.Get(2).ID == 1);
        }

        [TestMethod]
        public void Create()
        {

            Connection cFail_1 = new Connection() { Cost = -1, Time = -1, StartNode = new Node() { ID = 1 }, EndNode = nA };
            Connection cFail_2 = new Connection() { Cost = 1, Time = 1, StartNode = new Node() { ID = 0 }, EndNode = new Node() { ID = 3 } };
            Connection cFail_SNode = new Connection() { Cost = 1, Time = 1, StartNode = new Node() { ID = 99 }, EndNode = nB };
            Connection cFail_ENode = new Connection() { Cost = 1, Time = 1, StartNode = nA, EndNode = new Node() { ID = 99 } };
            Connection cSuccess = new Connection() { Cost = 1, Time = 1, StartNode = nC, EndNode = nB };

            LocalNodeData.GetData().Add(nA);
            LocalNodeData.GetData().Add(nB);
            LocalNodeData.GetData().Add(nC);

            int previous = LocalConnectionData.GetData().Count;
            Assert.IsTrue(_classTest.Create(cFail_1).ID == 0);
            Assert.IsTrue(_classTest.Create(cFail_2).ID == 0);
            Assert.IsTrue(_classTest.Create(cFail_SNode).ID == 0);
            Assert.IsTrue(_classTest.Create(cFail_ENode).ID == 0);
            Assert.IsTrue(_classTest.Create(cSuccess).ID != 0);
            Assert.IsTrue(previous < LocalConnectionData.GetData().Count);

        }

        [TestMethod]
        public void Update()
        {
            Connection cFail_1 = new Connection() { Cost = -1, Time = -1, StartNode = new Node() { ID = 1 }, EndNode = nA };
            Connection cFail_2 = new Connection() { Cost = 1, Time = 1, StartNode = new Node() { ID = 0 }, EndNode = new Node() { ID = 3 } };
            Connection cFail_SNode = new Connection() { Cost = 1, Time = 1, StartNode = new Node() { ID = 99 }, EndNode = nB };
            Connection cFail_ENode = new Connection() { Cost = 1, Time = 1, StartNode = nA, EndNode = new Node() { ID = 99 } };
            Connection cSame = new Connection() { Cost = cAC.Cost, Time = cAC.Time, StartNode = cAC.StartNode, EndNode = cAC.EndNode };
            Connection cSuccess = new Connection() { Cost = 99, Time = 99, StartNode = nC, EndNode = nA };

            LocalNodeData.GetData().Add(nA);
            LocalNodeData.GetData().Add(nB);
            LocalNodeData.GetData().Add(nC);

            int previous = LocalConnectionData.GetData().Count;

            Assert.IsTrue(_classTest.Update(0, cFail_1).ID == 0);
            Assert.IsTrue(_classTest.Update(1, cFail_2).ID == 0);
            Assert.IsTrue(_classTest.Update(1, cFail_SNode).ID == 0);
            Assert.IsTrue(_classTest.Update(1, cFail_ENode).ID == 0);

            Connection original = new Connection() { ID = cAC.ID, Cost = cAC.Cost, Time = cAC.Time, StartNode = cAC.StartNode, EndNode = cAC.EndNode };
            Connection result = _classTest.Update(cAC.ID, cSame);
            Assert.IsTrue(result.ID == cAC.ID && result.Equals(cAC));

            result = _classTest.Update(cAC.ID, cSuccess);
            Assert.IsTrue(result.ID == cAC.ID && !result.Equals(original));

            Assert.IsTrue(previous == LocalConnectionData.GetData().Count);
        }

        [TestMethod]
        public void Delete()
        {
            int previous = LocalConnectionData.GetData().Count;
            _classTest.Delete(1);

            Assert.IsTrue(previous > LocalNodeData.GetData().Count);
            Assert.IsTrue(LocalNodeData.GetData().Where(n => n.ID == 1).Count() == 0);
        }

        [TestMethod]
        public void NodeHasActiveConnections()
        {
            Node notConnectedNode = new Node() { ID = 99, Name = "Not Connected Node" };
            LocalNodeData.GetData().Add(notConnectedNode);

            Assert.IsFalse(_classTest.NodeHasActiveConnections(99));
            Assert.IsTrue(_classTest.NodeHasActiveConnections(1));
        }

        [TestMethod]
        public void GetOutgoing()
        {

            Node notConnectedNode = new Node() { ID = 99, Name = "Not Connected Node" };
            LocalNodeData.GetData().Add(notConnectedNode);

            Assert.IsTrue(_classTest.GetOutgoing(notConnectedNode).Count == 0);
            Assert.IsTrue(_classTest.GetOutgoing(nC).Count == 0);
            Assert.IsTrue(_classTest.GetOutgoing(nB).Count == 0);

            List<Connection> outGoingA = _classTest.GetOutgoing(nA);
            Assert.IsTrue(outGoingA.Count == 2);
            for (int i = 0; i < outGoingA.Count; i++)
            {
                Assert.IsTrue(outGoingA[i].ID == cAC.ID || outGoingA[i].ID == cAB.ID);
            }
        }
    }
}
