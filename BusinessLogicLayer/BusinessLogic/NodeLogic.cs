using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Models.UtilsModels;
using DataAccessLayer.DataAccess;
using DataAccessLayer.Utils;
using BusinessLogicLayer.Service;
using static Models.UtilsModels.LogActionEnum;

namespace BusinessLogicLayer.BusinessLogic
{
    public class NodeLogic
    {
        private IConnectionDataAccess connectionDA { get; set; }
        private INodeDataAccess nodeDA { get; set; }
        private ILogDataAccess logDA { get; set; }

        public NodeLogic(IConnectionDataAccess iConnectionDA, INodeDataAccess iNodeDA, ILogDataAccess iLogDA)
        {
            connectionDA = iConnectionDA;
            nodeDA = iNodeDA;
            logDA = iLogDA;
        }

        public List<Node> GetAll(User user = null)
        {
            List<Node> nodeList = new List<Node>();
            try
            {
                nodeList = nodeDA.GetAll(new Filter());
                Logger.Register(logDA ,eLogAction.GetAll, eLogResult.Sucess, new Node(), user, 0, "");
                return nodeList;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.GetAll, eLogResult.Error, new Node(), user, 0, "Exception", ex);
                return nodeList;
            }


        }

        public Node Get(int id, User user = null)
        {
            try
            {
                Node node = nodeDA.Get(id);
                if(node.ID == 0)
                {
                    Logger.Register(logDA, eLogAction.Get, eLogResult.Error, new Node(), user, id, "Node does not exist");
                    throw (new Exception("Node does not exist"));
                }
                Logger.Register(logDA ,eLogAction.Get, eLogResult.Sucess, new Node(), user, id, "");
                return node;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Get, eLogResult.Error, new Node(), user, id, "Exception", ex);
                return new Node(); 
            }
        }

        public Node Create(Node node , User user = null)
        {
            try
            {
                if(!node.Validate())
                {
                    Logger.Register(logDA ,eLogAction.Create, eLogResult.Error, new Node(), user, 0, "Invalid Model");
                    return new Node();
                }
                node = nodeDA.Create(node);
                Logger.Register(logDA ,eLogAction.Create, eLogResult.Sucess, new Node(), user, node.ID, "");
                return node;
            }
            catch(Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Create, eLogResult.Error, new Node(), user, 0, "Exception", ex);
                return new Node();
            }
        }

        public Node Update(int id,Node node, User user = null)
        {
            try
            {
                node.ID = id;
                if (!node.Validate(true))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Node(), user, id, "Invalid Model");
                    return new Node();
                }
                if (!NodeExists(id))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Node(), user, id, "Node does not exist");
                    return node;
                }
                if (node.Equals(nodeDA.Get(node.ID)))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Node(), user, id, "Same object, no changes made");
                    return node;
                }

                nodeDA.Update(node);
                Logger.Register(logDA ,eLogAction.Update, eLogResult.Sucess, new Node(), user, id, "");
                return node;
            }
            catch(Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Node(), user, id, "Exception", ex);
                return new Node();
            }
        }

        public void Delete (int id, User user = null)
        {
            try
            {
                if (!NodeExists(id))
                {
                    Logger.Register(logDA ,eLogAction.Delete, eLogResult.Error, new Node(), user, id, "Node does not exist");
                }
                else if(new ConnectionLogic(connectionDA, nodeDA, logDA).NodeHasActiveConnections(id))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Node(), user, id, "Node with Active Connection, not allowed to remove");
                }
                else
                {
                    nodeDA.Delete(id);
                    Logger.Register(logDA ,eLogAction.Delete, eLogResult.Sucess, new Node(), user, id, "");
                }
            }
            catch(Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Delete, eLogResult.Error, new Node(), user, id, "Exception", ex);
            }
        }

        public bool NodeExists(int id)
        {
            return (nodeDA.Get(id).ID > 0);
        }
    }
}
