using System;
using System.Collections.Generic;
using System.Text;
using static Models.Enum.UserRoleEnum;

namespace Models.BussinessModels
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty; 
        public eUserRole Role { get; set; }

        public bool Validate(bool checkID = false)
        {

            if (checkID && ID <= 0) return false;
            if (String.IsNullOrEmpty(Username)) return false;
            if (String.IsNullOrEmpty(Password)) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is User)) return false;

            User model = obj as User;

            if (this.ID != model.ID) return false;
            if (this.Username != model.Username) return false;
            if (this.Password != model.Password) return false;
            if (this.Role != model.Role) return false;

            return true;
        }
    }

}
