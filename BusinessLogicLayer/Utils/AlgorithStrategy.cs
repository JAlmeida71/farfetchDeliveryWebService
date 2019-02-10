using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.DataAccess;
using DataAccessLayer.Utils;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Models.UtilsModels.PathsEnum;

namespace BusinessLogicLayer.Utils
{
    public interface IAlgorithmtrategy
    {
        Path CalculateBestPath(Path path);
        Path SetBestImmediatePath(Path path, List<Connection> outGoingConnections);
    }

    public class LessTimeAlgorithm : IAlgorithmtrategy
    {
        private IConnectionDataAccess connectionDA;
        private INodeDataAccess nodeDA;
        private ILogDataAccess logDA;
        private List<KeyValuePair<Node, decimal>> distanceToNode;
        private List<KeyValuePair<Node, Connection>> connectionTaken;
        private List<Node> visited;

        public LessTimeAlgorithm(IConnectionDataAccess iConnectionDA, INodeDataAccess iNodeDA, ILogDataAccess iLogDA)
        {
            connectionDA = iConnectionDA;
            nodeDA = iNodeDA;
            logDA = iLogDA;
        }

        //Returns OutGoing connections to nodes not yet visited
        internal List<Connection> GetOutgoing(Node nodeOrig, Path path)
        {
            List<Connection> list, toReturn = new List<Connection>();
            if (nodeOrig.ID == path.StartNode.ID)
                list = new ConnectionLogic(connectionDA, nodeDA, logDA)
                    .GetOutgoing(nodeOrig)
                    .Where(c => c.EndNode.ID != path.EndNode.ID)
                    .ToList();
            else
                list = new ConnectionLogic(connectionDA, nodeDA, logDA)
                    .GetOutgoing(nodeOrig)
                    .ToList();

            foreach (Connection c in list)
                if (!visited.Contains(c.EndNode))
                    toReturn.Add(c);

            return toReturn;
        }

        internal bool IsVisited(Node node)
        {
            return visited.Where(dn => dn.ID == node.ID).Count() > 0;
        }

        internal bool LessDistance(Connection connection)
        {
            if (distanceToNode.Where(dn => dn.Key.ID == connection.EndNode.ID).Count() != 0)
            {
                if (distanceToNode.Find(dn => dn.Key.ID == connection.StartNode.ID).Value == decimal.MaxValue) return true;
                return
                    distanceToNode.Find(dn => dn.Key.ID == connection.StartNode.ID).Value + connection.Time
                    <
                    distanceToNode.Find(dn => dn.Key.ID == connection.EndNode.ID).Value;
            }
            return true;
        }

        //Set Value of Distance and Updates if it already exists
        internal void UpdateDistance(Connection connection)
        {
            if(connectionTaken.Where(c => c.Key.ID == connection.EndNode.ID).Count() > 0)
                connectionTaken.Remove(connectionTaken.Find(dn => dn.Key.ID == connection.EndNode.ID));
            if (distanceToNode.Where(c => c.Key.ID == connection.EndNode.ID).Count() > 0)
                distanceToNode.Remove(distanceToNode.Find(dn => dn.Key.ID == connection.EndNode.ID));

            connectionTaken.Add(new KeyValuePair<Node, Connection>(connection.EndNode, connection));
            distanceToNode.Add(new KeyValuePair<Node, decimal>(connection.EndNode, 
                (distanceToNode.Find(dn => dn.Key.ID == connection.StartNode.ID).Value == decimal.MaxValue ? 
                connection.Time : 
                distanceToNode.Find(dn => dn.Key.ID == connection.StartNode.ID).Value + connection.Time))
                );
        }

        internal Node PickLessDistante()
        {
            Node pickedNode = distanceToNode.Where(dn => !IsVisited(dn.Key)).OrderBy(dn => dn.Value).First().Key;
            visited.Add(pickedNode);
            return pickedNode;
        }

        //Retraces the steps to fill in Path data
        internal Path BuildPath(Path path)
        {
            Node retracingNode = path.EndNode;
            Connection retracingConnection;

            path.NodesTaken.Add(retracingNode);
            while (true)
            {
                retracingConnection = connectionTaken.Find(c => c.Key.ID == retracingNode.ID).Value;
                path.ConnectionsTaken.Add(retracingConnection);
                path.TotalCost += retracingConnection.Cost;
                path.TotalTime += retracingConnection.Time;
                retracingNode = retracingConnection.StartNode;
                path.NodesTaken.Add(retracingNode);
                if (retracingNode.ID == path.StartNode.ID) break;
            }
            path.ConnectionsTaken.Reverse();
            path.NodesTaken.Reverse();

            return path;
        }

        internal bool AllConnectionTaken()
        {
            return distanceToNode.Where(dn => !visited.Contains(dn.Key)).Count() == 0;
        }

        internal Node HandleDeadEndNode(Node node)
        {
            visited.Add(node);
            distanceToNode.Remove(distanceToNode.Find(dn => dn.Key.ID == node.ID));
            distanceToNode.Add(new KeyValuePair<Node, decimal>(node, decimal.MaxValue));
            return connectionTaken.Find(c => c.Key.ID == node.ID).Value.StartNode;
        }

        public Path CalculateBestPath(Path path)
        {
            distanceToNode = new List<KeyValuePair<Node, decimal>>();
            connectionTaken = new List<KeyValuePair<Node, Connection>>();
            visited = new List<Node>();

            Node currentNode = path.StartNode;

            distanceToNode.Add(new KeyValuePair<Node, decimal>(currentNode, decimal.MaxValue));
            visited.Add(currentNode);

            while (true)
            {
                if (currentNode.ID == path.EndNode.ID) { break; }
                List<Connection> currentOutgoing = GetOutgoing(currentNode, path);
                if (currentOutgoing.Count == 0)
                {
                    if (AllConnectionTaken())
                        return new Path() { StartNode = path.StartNode, EndNode = path.EndNode, Status = ePathStatus.notConnectedNodesGiven };
                    else
                    {
                        currentNode = HandleDeadEndNode(currentNode);
                        continue;
                    }
                }

                foreach (Connection outGoingConnection in currentOutgoing)
                    if (!IsVisited(outGoingConnection.EndNode) && LessDistance(outGoingConnection))
                        UpdateDistance(outGoingConnection);
                currentNode = PickLessDistante();
            }

            return BuildPath(path);
        }

        public Path SetBestImmediatePath(Path path, List<Connection> outGoingConnections)
        {
            path.Status = ePathStatus.foundOnlyImmediatePath;
            path.NodesTaken.Add(path.EndNode);
            Connection connection = outGoingConnections.Where(c => c.EndNode.Equals(path.EndNode)).OrderBy(c => c.Time).First();
            path.TotalCost = connection.Cost;
            path.TotalTime = connection.Time;
            return path;
        }
    }

    public class LessCostAlgorithm : IAlgorithmtrategy
    {
        private IConnectionDataAccess connectionDA;
        private INodeDataAccess nodeDA;
        private ILogDataAccess logDA;
        private List<KeyValuePair<Node, decimal>> distanceToNode;
        private List<KeyValuePair<Node, Connection>> connectionTaken;
        private List<Node> visited;

        public LessCostAlgorithm(IConnectionDataAccess iConnectionDA, INodeDataAccess iNodeDA, ILogDataAccess iLogDA)
        {
            connectionDA = iConnectionDA;
            nodeDA = iNodeDA;
            logDA = iLogDA;
        }

        //Returns OutGoing connections to nodes not yet visited
        internal List<Connection> GetOutgoing(Node nodeOrig, Path path)
        {
            List<Connection> list, toReturn = new List<Connection>();
            if (nodeOrig.ID == path.StartNode.ID)
                list = new ConnectionLogic(connectionDA, nodeDA, logDA)
                    .GetOutgoing(nodeOrig)
                    .Where(c => c.EndNode.ID != path.EndNode.ID)
                    .ToList();
            else
                list = new ConnectionLogic(connectionDA, nodeDA, logDA)
                    .GetOutgoing(nodeOrig)
                    .ToList();

            foreach (Connection c in list)
                if (!visited.Contains(c.EndNode))
                    toReturn.Add(c);

            return toReturn;
        }

        internal bool IsVisited(Node node)
        {
            return visited.Where(dn => dn.ID == node.ID).Count() > 0;
        }

        internal bool LessDistance(Connection connection)
        {
            if (distanceToNode.Where(dn => dn.Key.ID == connection.EndNode.ID).Count() != 0)
            {
                if (distanceToNode.Find(dn => dn.Key.ID == connection.StartNode.ID).Value == decimal.MaxValue) return true;
                return
                    distanceToNode.Find(dn => dn.Key.ID == connection.StartNode.ID).Value + connection.Cost
                    <
                    distanceToNode.Find(dn => dn.Key.ID == connection.EndNode.ID).Value;
            }
            return true;
        }

        //Set Value of Distance and Updates if it already exists
        internal void UpdateDistance(Connection connection)
        {
            if (connectionTaken.Where(c => c.Key.ID == connection.EndNode.ID).Count() > 0)
                connectionTaken.Remove(connectionTaken.Find(dn => dn.Key.ID == connection.EndNode.ID));
            if (distanceToNode.Where(c => c.Key.ID == connection.EndNode.ID).Count() > 0)
                distanceToNode.Remove(distanceToNode.Find(dn => dn.Key.ID == connection.EndNode.ID));

            connectionTaken.Add(new KeyValuePair<Node, Connection>(connection.EndNode, connection));
            distanceToNode.Add(new KeyValuePair<Node, decimal>(connection.EndNode,
                (distanceToNode.Find(dn => dn.Key.ID == connection.StartNode.ID).Value == decimal.MaxValue ?
                connection.Cost :
                distanceToNode.Find(dn => dn.Key.ID == connection.StartNode.ID).Value + connection.Cost))
                );
        }

        internal Node PickLessDistante()
        {
            Node pickedNode = distanceToNode.Where(dn => !IsVisited(dn.Key)).OrderBy(dn => dn.Value).First().Key;
            visited.Add(pickedNode);
            return pickedNode;
        }

        //Retraces the steps to fill in Path data
        internal Path BuildPath(Path path)
        {
            Node retracingNode = path.EndNode;
            Connection retracingConnection;

            path.NodesTaken.Add(retracingNode);
            while (true)
            {
                retracingConnection = connectionTaken.Find(c => c.Key.ID == retracingNode.ID).Value;
                path.ConnectionsTaken.Add(retracingConnection);
                path.TotalCost += retracingConnection.Cost;
                path.TotalTime += retracingConnection.Time;
                retracingNode = retracingConnection.StartNode;
                path.NodesTaken.Add(retracingNode);
                if (retracingNode.ID == path.StartNode.ID) break;
            }
            path.ConnectionsTaken.Reverse();
            path.NodesTaken.Reverse();

            return path;
        }

        internal bool AllConnectionTaken()
        {
            return distanceToNode.Where(dn => !visited.Contains(dn.Key)).Count() == 0;
        }

        internal Node HandleDeadEndNode(Node node)
        {
            visited.Add(node);
            distanceToNode.Remove(distanceToNode.Find(dn => dn.Key.ID == node.ID));
            distanceToNode.Add(new KeyValuePair<Node, decimal>(node, decimal.MaxValue));
            return connectionTaken.Find(c => c.Key.ID == node.ID).Value.StartNode;
        }

        public Path CalculateBestPath(Path path)
        {
            distanceToNode = new List<KeyValuePair<Node, decimal>>();
            connectionTaken = new List<KeyValuePair<Node, Connection>>();
            visited = new List<Node>();

            Node currentNode = path.StartNode;

            distanceToNode.Add(new KeyValuePair<Node, decimal>(currentNode, decimal.MaxValue));
            visited.Add(currentNode);

            while (true)
            {
                if (currentNode.ID == path.EndNode.ID) { break; }
                List<Connection> currentOutgoing = GetOutgoing(currentNode, path);
                if (currentOutgoing.Count == 0)
                {
                    if (AllConnectionTaken())
                        return new Path() { StartNode = path.StartNode, EndNode = path.EndNode, Status = ePathStatus.notConnectedNodesGiven };
                    else
                    {
                        currentNode = HandleDeadEndNode(currentNode);
                        continue;
                    }
                }

                foreach (Connection outGoingConnection in currentOutgoing)
                    if (!IsVisited(outGoingConnection.EndNode) && LessDistance(outGoingConnection))
                        UpdateDistance(outGoingConnection);
                currentNode = PickLessDistante();
            }

            return BuildPath(path);
        }

        public Path SetBestImmediatePath(Path path, List<Connection> outGoingConnections)
        {
            path.Status = ePathStatus.foundOnlyImmediatePath;
            path.NodesTaken.Add(path.EndNode);
            Connection connection = outGoingConnections.Where(c => c.EndNode.Equals(path.EndNode)).OrderBy(c => c.Cost).First();
            path.TotalCost = connection.Cost;
            path.TotalTime = connection.Time;
            return path;
        }
    }

}
