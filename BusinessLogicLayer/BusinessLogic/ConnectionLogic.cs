using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogicLayer.Service;
using DataAccessLayer.DataAccess;
using DataAccessLayer.Utils;
using Models.BussinessModels;
using Models.UtilsModels;
using static Models.UtilsModels.LogActionEnum;

namespace BusinessLogicLayer.BusinessLogic
{
    public class ConnectionLogic
    {
        private IConnectionDataAccess connectionDA { get; set; }
        private INodeDataAccess nodeDA { get; set; }
        private ILogDataAccess logDA { get; set; }

        public ConnectionLogic(IConnectionDataAccess iConnectionDA, INodeDataAccess iNodeDA, ILogDataAccess iLogDA)
        {
            connectionDA = iConnectionDA;
            nodeDA = iNodeDA;
            logDA = iLogDA;
        }

        public List<Connection> GetAll(User user = null)
        {
            List<Connection> connectionList = new List<Connection>();
            try
            {
                connectionList = connectionDA.GetAll(new Filter());
                Logger.Register(logDA,eLogAction.GetAll, eLogResult.Sucess, new Connection(), user, 0, "");
                return connectionList;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA,eLogAction.GetAll, eLogResult.Error, new Connection(), user, 0, "Exception", ex);
                throw (new Exception("Ocorreu um problema"));
            }
        }

        public Connection Get(int id, User user = null)
        {
            try
            {
                Connection connection = connectionDA.Get(id);
                if(connection.ID == 0)
                {
                    Logger.Register(logDA, eLogAction.Get, eLogResult.Error, new Connection(), user,id , "");
                return connection;
                }
                Logger.Register(logDA ,eLogAction.Get, eLogResult.Sucess, new Connection(), user, id, "");
                return connection;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Get, eLogResult.Error, new Connection(), user, 0, "Exception", ex);
                throw (new Exception("Ocorreu um problema"));
            }
        }

        public Connection Create(Connection connection, User user = null)
        {
            try
            {
                if (!connection.Validate())
                {
                    Logger.Register(logDA ,eLogAction.Create, eLogResult.Error, new Connection(), user, 0, "Invalid Model");
                    return connection;
                }
                if (!new NodeLogic(connectionDA, nodeDA, logDA).NodeExists(connection.StartNode.ID)
                    || !new NodeLogic(connectionDA, nodeDA, logDA).NodeExists(connection.EndNode.ID))
                {
                    Logger.Register(logDA ,eLogAction.Create, eLogResult.Error, new Connection(), user, 0, "Invalid Nodes Given");
                    return connection;
                }
                connection = connectionDA.Create(connection);
                Logger.Register(logDA ,eLogAction.Create, eLogResult.Sucess, new Connection(), user, connection.ID, "");
                return connection;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Create, eLogResult.Error, new Connection(), user, 0, "Exception", ex);
                throw (new Exception("Ocorreu um problema"));
            }
        }

        public Connection Update(int id, Connection connection, User user = null)
        {
            try
            {
                connection.ID = id;
                if (!connection.Validate(true))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Connection(), user, id, "Invalid Model");
                    return new Connection();
                }
                if (connectionDA.Get(connection.ID).ID == 0)
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Connection(), user, id, "Conection does not exist");
                    return new Connection();
                }
                if (!new NodeLogic(connectionDA, nodeDA, logDA).NodeExists(connection.StartNode.ID)
                    || !new NodeLogic(connectionDA, nodeDA, logDA).NodeExists(connection.EndNode.ID))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Connection(), user, id, "Invalid Nodes Given");
                    return new Connection();
                }
                if (connection.Equals(connectionDA.Get(connection.ID)))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Connection(), user, id, "Same object, no changes made");
                    return connection;
                }
                connectionDA.Update(connection);
                Logger.Register(logDA ,eLogAction.Update, eLogResult.Sucess, new Connection(), user, id, "");
                return connection;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new Connection(), user, id, "Exception", ex);
                return new Connection();
            }

        }

        public void Delete(int id, User user = null)
        {
            try
            {
                if (connectionDA.Get(id).ID == 0)
                {
                    Logger.Register(logDA ,eLogAction.Delete, eLogResult.Error, new Connection(), user, id, "Conection does not exist");
                }
                else
                {
                    connectionDA.Delete(id);
                    Logger.Register(logDA ,eLogAction.Delete, eLogResult.Sucess, new Connection(), user, id, "");
                }
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Delete, eLogResult.Error, new Connection(), user, id, "Exception", ex);
            }
        }

        public bool NodeHasActiveConnections(int id)
        {
            if (connectionDA.CountNodeConnection(id) > 0) return true;
            return false;
        }

        public List<Connection> GetOutgoing(Node currentNode)
        {
            List<Connection> connectionList = new List<Connection>();
            try
            {

                Filter filter = new Filter() { StartNode = currentNode };
                if (!filter.Validate())
                {
                    return connectionList;
                }
                connectionList = connectionDA.GetAll(filter);
                return connectionList;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Specific, eLogResult.Error, new Connection(), new User(), 0, "GetOutgoing-Exception", ex);
                return connectionList;
            }
        }
    }
}
