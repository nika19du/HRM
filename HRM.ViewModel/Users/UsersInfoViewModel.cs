using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.ViewModel.Users
{
    public class UsersInfoViewModel
    {
        public string Id { get; set; }
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        [Display(Name = "First Name")] 
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")] 
        public string MiddleName { get; set; } 
        public string Surname { get; set; }
        public string EGN { get; set; } 
        public bool IsItActive { get; set; }
        public bool IsItOld { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        public string TextEmail => this.Email.Substring(0, Math.Min(4, Email.Length))+ "...";  
    }
}
