using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Enum
{
    public class UserRoleEnum
    {
        public enum eUserRole
        {
            [Description( "Admin")]
            Admin = 0,
            [Description( "User")]
            User = 1
        }
    }
}
