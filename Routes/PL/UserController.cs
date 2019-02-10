using API.Auth;
using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.DataAccess;
using DataAccessLayer.LocalDataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.PL
{
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        //[Authorize(Policy = "Admins")] Para facilitar o Login . Em Live teria de se colocar activa a policy
        public ActionResult<IEnumerable<Object>> Get()
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            return new UserLogic(new UserDataAccess(), new LogDataAccess()).GetAll(user);
        }

        [HttpPost]
        [Authorize(Policy = "Admins")]
        public void Post([FromBody] User model)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            new UserLogic(new UserDataAccess(), new LogDataAccess()).Create(model, user);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Admins")]
        public void Put(int id, [FromBody] User model)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            new UserLogic(new UserDataAccess(), new LogDataAccess()).Update(id, model, user);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admins")]
        public void Delete(int id)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            new UserLogic(new UserDataAccess(), new LogDataAccess()).Delete(id,user);
        }

    }
}
