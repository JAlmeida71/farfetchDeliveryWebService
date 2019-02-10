using BusinessLogicLayer.BusinessLogic;
using BusinessLogicLayer.Utils;
using DataAccessLayer.LocalDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Text;
using static Models.UtilsModels.PathsEnum;

namespace BusinessLogicLayerUnitTests.BusinessLogicUnitTests
{
    [TestClass]
    public class PathLogicUnitTests
    {
        private PathLogic _classTest;
        protected Connection cAB, cAC, cAD, cCE, cCG, cDE, cDG, cGE;

        #region Init Data

        protected Node nA = new Node() { ID = 1, Name = "A" };
        protected Node nB = new Node() { ID = 2, Name = "B" };
        protected Node nC = new Node() { ID = 3, Name = "C" };
        protected Node nD = new Node() { ID = 4, Name = "D" };
        protected Node nE = new Node() { ID = 5, Name = "E" };
        protected Node nG = new Node() { ID = 6, Name = "G" };
        protected Node nZ = new Node() { ID = 99, Name = "Z" };

        #endregion

        [TestInitialize]
        public void OnTestInit()
        {
            LocalConnectionData.Reboot();
            LocalNodeData.Reboot();

            _classTest = new PathLogic(new LocalConnectionDataAccess(), new LocalNodeDataAccess(), new LocalLogDataAccess());


            LocalNodeData.GetData().Add(nA);
            LocalNodeData.GetData().Add(nB);
            LocalNodeData.GetData().Add(nC);
            LocalNodeData.GetData().Add(nD);
            LocalNodeData.GetData().Add(nE);
            LocalNodeData.GetData().Add(nG);
            LocalNodeData.GetData().Add(nZ);

            cAB = new Connection() { ID = 1, StartNode = nA, EndNode = nB, Time = 1, Cost = 1 };
            cAC = new Connection() { ID = 2, StartNode = nA, EndNode = nC, Time = 10, Cost = 5 };
            cAD = new Connection() { ID = 3, StartNode = nA, EndNode = nD, Time = 5, Cost = 10 };

            cCE = new Connection() { ID = 4, StartNode = nC, EndNode = nE, Time = 2, Cost = 20 };
            cCG = new Connection() { ID = 5, StartNode = nC, EndNode = nG, Time = 3, Cost = 10 };

            cDE = new Connection() { ID = 6, StartNode = nD, EndNode = nE, Time = 3, Cost = 5 };
            cDG = new Connection() { ID = 6, StartNode = nD, EndNode = nG, Time = 5, Cost = 20 };

            cGE = new Connection() { ID = 7, StartNode = nG, EndNode = nE, Time = 50, Cost = 50 };
            LocalConnectionData.GetData().Add(cAB);
            LocalConnectionData.GetData().Add(cAC);
            LocalConnectionData.GetData().Add(cAD);
            LocalConnectionData.GetData().Add(cCE);
            LocalConnectionData.GetData().Add(cCG);
            LocalConnectionData.GetData().Add(cDE);
            LocalConnectionData.GetData().Add(cDG);
            LocalConnectionData.GetData().Add(cGE);
        }

        [TestMethod]
        public void GetBestPath_InvalidNodes()
        {
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byCost, -1, 1).Status == ePathStatus.invalidNodesGiven);
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byTime, 1, -1).Status == ePathStatus.invalidNodesGiven);
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byCost, 1, 1).Status == ePathStatus.invalidNodesGiven);
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byTime, 999, 1).Status == ePathStatus.invalidNodesGiven);
        }

        [TestMethod]
        public void GetBestPath_NotConnected()
        {
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byTime, 99, 1).Status == ePathStatus.notConnectedNodesGiven);
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byTime, 2, 1).Status == ePathStatus.notConnectedNodesGiven);
        }

        [TestMethod]
        public void GetBestPath_ImediateBestButOtherFound()
        {
            Path result = _classTest.GetBestPath(ePathType.byTime, 4, 5);

            Path expected = new Path()
            {
                Type = ePathType.byTime,
                StartNode = nD,
                EndNode = nE,
                Status = ePathStatus.foundBestPath,
                NodesTaken = new List<Node>() { nD, nG, nE },
                ConnectionsTaken = new List<Connection>() { cDG, cGE },
                TotalCost = 70,
                TotalTime = 55
            };
            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        public void GetBestPath_SameCostTime()
        {

            Path expected = new Path()
            {
                Type= ePathType.byTime,
                StartNode = nA,
                EndNode = nE,
                Status = ePathStatus.foundBestPath,
                NodesTaken = new List<Node>() { nA, nD, nE },
                ConnectionsTaken = new List<Connection>() { cAD, cDE },
                TotalCost = 15,
                TotalTime = 8
            };
            Path result_byTime = _classTest.GetBestPath(ePathType.byTime, 1, 5);
            Assert.AreEqual(result_byTime, expected);

            expected.Type = ePathType.byCost;
            Path result_byCost = _classTest.GetBestPath(ePathType.byCost, 1, 5);
            Assert.AreEqual(result_byCost, expected);


        }

        [TestMethod]
        public void GetBestPath_DifCostTime()
        {
            Path expected_byTime = new Path()
            {
                Type = ePathType.byTime,
                StartNode = nA,
                EndNode = nG,
                Status = ePathStatus.foundBestPath,
                NodesTaken = new List<Node>() { nA, nD, nG },
                ConnectionsTaken = new List<Connection>() { cAD, cDG },
                TotalCost = 30,
                TotalTime = 10
            };
            Path result_byTime = _classTest.GetBestPath(ePathType.byTime, 1, 6);
            Assert.AreEqual(result_byTime, expected_byTime);

            Path expected_byCost= new Path()
            {
                Type = ePathType.byCost,
                StartNode = nA,
                EndNode = nG,
                Status = ePathStatus.foundBestPath,
                NodesTaken = new List<Node>() { nA, nC, nG },
                ConnectionsTaken = new List<Connection>() { cAC, cCG },
                TotalCost = 15,
                TotalTime = 13
            };
            Path result_byCost = _classTest.GetBestPath(ePathType.byCost, 1, 6);
            Assert.AreEqual(result_byCost, expected_byCost);
        }

        [TestMethod]
        public void GetBestPath_ImediateOnlyFound()
        {
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byTime, 1, 2).Status == ePathStatus.foundOnlyImmediatePath);
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byTime, 1, 3).Status == ePathStatus.foundOnlyImmediatePath);
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byTime, 1, 4).Status == ePathStatus.foundOnlyImmediatePath);
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byCost, 1, 2).Status == ePathStatus.foundOnlyImmediatePath);
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byCost, 1, 3).Status == ePathStatus.foundOnlyImmediatePath);
            Assert.IsTrue(_classTest.GetBestPath(ePathType.byCost, 1, 4).Status == ePathStatus.foundOnlyImmediatePath);
        }

        [TestMethod]
        public void SetAlgorithm()
        {
            //Setup
            ePathType typeCost = ePathType.byCost;
            BestPathAlgorithm expectedCost = new BestPathAlgorithm();
            expectedCost._strategy = new LessCostAlgorithm(new LocalConnectionDataAccess(), new LocalNodeDataAccess(), new LocalLogDataAccess());

            ePathType typeTime = ePathType.byTime;
            BestPathAlgorithm expectedTime = new BestPathAlgorithm();
            expectedTime._strategy = new LessTimeAlgorithm(new LocalConnectionDataAccess(), new LocalNodeDataAccess(), new LocalLogDataAccess());

            //Handle
            BestPathAlgorithm result = _classTest.SetAlgorithm(typeCost);
            //Assert
            Assert.ReferenceEquals(expectedCost._strategy, result._strategy);

            Assert.IsInstanceOfType(result._strategy, expectedCost._strategy.GetType());
            Assert.IsNotInstanceOfType(result._strategy, expectedTime._strategy.GetType());

            //Handle
            result = _classTest.SetAlgorithm(typeTime);
            //Assert
            Assert.IsInstanceOfType(result._strategy, expectedTime._strategy.GetType());
            Assert.IsNotInstanceOfType(result._strategy, expectedCost._strategy.GetType());

        }

    }
}
