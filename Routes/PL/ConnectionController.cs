using API.Auth;
using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.BussinessModels;
using Models.UtilsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    [Route("api/Connection")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        #region Get
        [HttpGet]
        public ActionResult<IEnumerable<Object>> Get()
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            return new ConnectionLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).GetAll(user);
        }

        [HttpGet("{id}")]
        public ActionResult<Object> Get(int id)
        {
            try{
                var user = AuthIdentity.GetUserIdentity(this.HttpContext);
                return new ConnectionLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).Get(id, user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Create

        [HttpPost]
        [Authorize(Policy = "Admins")]
        public void Post([FromBody] Connection connection)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            new ConnectionLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).Create(connection, user);
        }

        #endregion

        #region Edit

        [HttpPut("{id}")]
        [Authorize(Policy = "Admins")]
        public void Put(int id, [FromBody] Connection connection)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            new ConnectionLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).Update(id,connection,user);
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admins")]
        public void Delete(int id)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            new ConnectionLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).Delete(id,user);
        }

        #endregion


    }
}
