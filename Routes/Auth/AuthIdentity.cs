using Microsoft.AspNetCore.Http;
using Models.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Auth
{
    public static class AuthIdentity
    {
        public static User GetUserIdentity(HttpContext context)
        {
            User user = new User();
            ClaimsIdentity identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                if(identity.Claims.Count() > 0)
                {
                    var userid = identity.Claims.First(c => c.Type == ClaimTypes.PrimarySid).Value;
                    try
                    {
                        user.ID = Convert.ToInt32(userid);
                    }
                    catch { }
                }
            }
            return user;
        }

    }
}
