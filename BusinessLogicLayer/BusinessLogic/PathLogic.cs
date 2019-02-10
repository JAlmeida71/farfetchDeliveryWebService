using BusinessLogicLayer.Service;
using BusinessLogicLayer.Utils;
using DataAccessLayer.Utils;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Text;
using static Models.UtilsModels.LogActionEnum;
using static Models.UtilsModels.PathsEnum;

namespace BusinessLogicLayer.BusinessLogic
{
    public class PathLogic
    {
        private IConnectionDataAccess connectionDA { get; set; }
        private INodeDataAccess nodeDA { get; set; }
        private ILogDataAccess logDA { get; set; }
        public PathLogic(IConnectionDataAccess iConnectionDA, INodeDataAccess iNodeDA, ILogDataAccess iLogDA)
        {
            connectionDA = iConnectionDA;
            nodeDA = iNodeDA;
            logDA = iLogDA;
        }

        public Path GetBestPath(ePathType type, int startNode_ID, int endNode_ID, User user = null)
        {
            if (startNode_ID == endNode_ID) return new Path() { Status = ePathStatus.invalidNodesGiven };
            Path bestPath = new Path() {Type = type, StartNode = new Node() { ID = startNode_ID }, EndNode = new Node() { ID = endNode_ID } };
            try
            {
                if (!new NodeLogic(connectionDA,nodeDA, logDA).NodeExists(bestPath.StartNode.ID) 
                    || !new NodeLogic(connectionDA,nodeDA, logDA).NodeExists(bestPath.EndNode.ID))
                {
                    Logger.Register(logDA ,eLogAction.Specific, eLogResult.Error, new Path(), user, 0, "invalidNodesGiven");
                    bestPath.Status = ePathStatus.invalidNodesGiven;
                    return bestPath;
                }

                BestPathAlgorithm algorithm = SetAlgorithm(type);

                bestPath.StartNode = new NodeLogic(connectionDA,nodeDA, logDA).Get(bestPath.StartNode.ID,user);
                bestPath.EndNode = new NodeLogic(connectionDA,nodeDA, logDA).Get(bestPath.EndNode.ID, user);
                bestPath = algorithm.GetBestPath(bestPath);

                Logger.Register(logDA ,eLogAction.Specific, eLogResult.Sucess, new Path(), user, 0, bestPath.Status.ToString());
                return bestPath;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Specific, eLogResult.Error, new Path(), user, 0, "Exception", ex);
                return new Path() { Status = ePathStatus.WebServiceError };
            }
        }

        public BestPathAlgorithm SetAlgorithm(ePathType type)
        {
            BestPathAlgorithm algorithm = new BestPathAlgorithm();
            switch (type)
            {
                case ePathType.byCost:
                    algorithm.SetStrategy(new LessCostAlgorithm(connectionDA, nodeDA, logDA), connectionDA, nodeDA, logDA);
                    break;
                case ePathType.byTime:
                    algorithm.SetStrategy(new LessTimeAlgorithm(connectionDA, nodeDA, logDA), connectionDA, nodeDA, logDA);
                    break;
            }
            return algorithm;
        }
    }
}
