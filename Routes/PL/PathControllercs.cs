using API.Auth;
using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Models.UtilsModels.PathsEnum;

namespace API.PL
{
    [Route("api/BestPath")]
    [ApiController]
    [Authorize]
    public class PathControllercs : ControllerBase
    {
        [HttpGet("byTime")]
        public ActionResult<Object> GetBestPathByTime(int startNode_ID, int endNode_ID)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            Path path = new PathLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).GetBestPath(ePathType.byTime ,startNode_ID, endNode_ID,user );
            if(path.Status != ePathStatus.foundBestPath)
                return BadRequest(path.Status.ToString());
            return path;
        }

        [HttpGet("byCost")]
        public ActionResult<Object> GetBestPathByCost(int startNode_ID, int endNode_ID)
        {
            var user = AuthIdentity.GetUserIdentity(this.HttpContext);
            Path path = new PathLogic(new ConnectionDataAccess(), new NodeDataAccess(), new LogDataAccess()).GetBestPath(ePathType.byCost, startNode_ID, endNode_ID,user);
            if(path.Status != ePathStatus.foundBestPath)
                return BadRequest(path.Status.ToString());
            return path;
        }
    }
}
