using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.Utils;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Models.UtilsModels.PathsEnum;

namespace BusinessLogicLayer.Utils
{
    public class BestPathAlgorithm
    {
        public IAlgorithmtrategy _strategy;
        private IConnectionDataAccess connectionDA { get; set; }
        private INodeDataAccess nodeDA { get; set; }
        private ILogDataAccess logDA { get; set; }

        public void SetStrategy(IAlgorithmtrategy strategy, IConnectionDataAccess iConnectionDA, INodeDataAccess iNodeDA, ILogDataAccess iLogDA)
        {
            this._strategy = strategy;
            connectionDA = iConnectionDA;
            nodeDA = iNodeDA;
            logDA = iLogDA;
        }

        internal Path GetBestPath(Path path)
        {
            path = _strategy.CalculateBestPath(path);

            List<Connection> outGoingConnections = new ConnectionLogic(connectionDA, nodeDA, logDA).GetOutgoing(path.StartNode);
            if (OnlyConnectionIsDirect(path, outGoingConnections))
                path = _strategy.SetBestImmediatePath(path,outGoingConnections);

            return path;
        }

        public bool OnlyConnectionIsDirect (Path path, List<Connection> outGoingConnections)
        {
            if (path.Status == ePathStatus.notConnectedNodesGiven)
                if (outGoingConnections.Where(c => c.EndNode.ID == path.EndNode.ID).Count() > 0)
                    return true;
            return false;
        }

    
    }
}
