using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.LocalDataAccess;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Models.Enum.UserRoleEnum;

namespace IdentityServerProvider
{
    public static class Config
    {
        public static List<User> GetUsers()
        {
            return new UserLogic(new LocalUserDataAccess()).GetAll();
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
    }
}
