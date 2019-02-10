using BusinessLogicLayer.BusinessLogic;
using DataAccessLayer.DataAccess;
using DataAccessLayer.LocalDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    [Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration config;

        public LoginController(IConfiguration iConfig)
        {
            config = iConfig;
        }

        [HttpPost("auth/login")]
        public IActionResult Login([FromBody] User model)
        {
            try
            {
                User loggedUser = new UserLogic(new UserDataAccess(), new LogDataAccess()).AttemptLogin(model);

                if (loggedUser.ID != 0)
                {
                    AuthProvider auth = new AuthProvider(config);
                    JwtSecurityToken token = auth.CreateToken(loggedUser);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expires = token.ValidTo
                    });

                }
                return BadRequest("Login Failed");
            }
            catch
            {
                return BadRequest("Login Failed");
            }
        }
    }
}
