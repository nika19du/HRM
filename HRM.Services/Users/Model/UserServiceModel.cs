using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Services.Users.Model
{
    public class UserServiceModel
    {
        public string Id { get; set; } 
        public string UserName { get; set; } 
        public string FirstName { get; set; } 
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string EGN { get; set; }
        public bool IsItActive { get; set; }
        public bool IsItOld { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
