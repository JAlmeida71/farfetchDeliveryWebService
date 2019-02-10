using BusinessLogicLayer.Service;
using DataAccessLayer.Utils;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Text;
using static Models.UtilsModels.LogActionEnum;

namespace BusinessLogicLayer.BusinessLogic
{
    public class UserLogic
    {
        private IUserDataAccess userDA { get; set; }
        private ILogDataAccess logDA { get; set; }

        public UserLogic(IUserDataAccess iUserDA, ILogDataAccess iLogDA)
        {
            userDA = iUserDA;
            logDA = iLogDA;
        }

        public List<User> GetAll(User user = null)
        {
            List<User> list = new List<User>();
            try
            {
                list = userDA.GetAll();
                Logger.Register(logDA ,eLogAction.GetAll, eLogResult.Sucess, new User(), user, 0, "");
                return list;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.GetAll, eLogResult.Error, new User(), user, 0, "Exception", ex);
                return list;
            }


        }

        public User Get(int id, User user = null)
        {
            try
            {
                User model = userDA.Get(id);
                Logger.Register(logDA ,eLogAction.Get, eLogResult.Sucess, new User(), user, id, "");
                return model;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Get, eLogResult.Error, new User(), user, id, "Exception", ex);
                return new User();
            }
        }

        public User Create(User model, User user = null)
        {
            try
            {
                if (!model.Validate())
                {
                    Logger.Register(logDA ,eLogAction.Create, eLogResult.Error, new User(), user, 0, "Invalid Model");
                    return new User();
                }
                model = userDA.Create(model);
                Logger.Register(logDA ,eLogAction.Create, eLogResult.Sucess, new User(), user, model.ID, "");
                return model;
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Create, eLogResult.Error, new User(), user, 0, "Exception", ex);
                return new User();
            }
        }

        public User Update(int id, User model, User user = null)
        {
            try
            {
                model.ID = id;
                if (!model.Validate(true))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new User(), user, id, "Invalid Model");
                    return new User();
                }
                if (!Exists(id))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new User(), user, id, "User does not exist");
                    return model;
                }
                if (model.Equals(userDA.Get(model.ID)))
                {
                    Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new User(), user, id, "Same object, no changes made");
                    return model;
                }

                userDA.Update(model);
                Logger.Register(logDA ,eLogAction.Update, eLogResult.Sucess, new User(), user, id, "");
                return model;
            }
            catch(Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Update, eLogResult.Error, new User(), user, id, "Exception", ex);
                return new User();
            }
        }

        public void Delete(int id, User user = null)
        {
            try
            {
                if (userDA.Get(id).ID == 0)
                {
                    Logger.Register(logDA ,eLogAction.Delete, eLogResult.Error, new User(), user, id, "User does not exist");
                }
                else
                {
                    userDA.Delete(id);
                    Logger.Register(logDA ,eLogAction.Delete, eLogResult.Sucess, new User(), user, id, "");
                }
            }
            catch (Exception ex)
            {
                Logger.Register(logDA ,eLogAction.Delete, eLogResult.Error, new User(), user, id, "Exception", ex);
            }
        }

        public bool Exists(int id)
        {
            return (userDA.Get(id).ID > 0);
        }

        public User AttemptLogin(User model)
        {
            User user = userDA.Exists(model.Username);
            if (user.ID > 0)
                if (model.Password == user.Password)
                    return user;
            return new User();
        }
    }
}
