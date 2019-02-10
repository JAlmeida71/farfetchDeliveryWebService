using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Auth;
using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.BussinessModels;

namespace API
{
    [Route("api/Node")]
    [ApiController]
    public class NodeController : ControllerBase
    {
        #region Get
        [HttpGet]
        public ActionResult<IEnumerable<Object>> Get()
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            return new NodeLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).GetAll(user);
        }

        [HttpGet("{id}")]
        public ActionResult<Object> Get(int id)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            return new NodeLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).Get(id, user);
        }

        #endregion
         
        #region Create

        [HttpPost]
        [Authorize(Policy = "Admins")]
        public void Post([FromBody] Node node)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            new NodeLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).Create(node, user);
        }

        #endregion

        #region Edit

        [HttpPut("{id}")]
        [Authorize(Policy = "Admins")]
        public void Put(int id, [FromBody] Node node)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            new NodeLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).Update(id,node, user);
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admins")]
        public void Delete(int id)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            new NodeLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).Delete(id, user);
        }

        #endregion
    }
}
