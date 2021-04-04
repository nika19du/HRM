using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.Data.Models
{
    public class Customer : User
    {
        //public string Id { get; set; }
        //[Required]
        //public string FirstName { get; set; }
        //[Required]
        //public string Surname { get; set; }
        //[Required]
        //public string PhoneNumber { get; set; }
        //[Required]
        //[EmailAddress]
        //[RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not valid")]
        //public string Email { get; set; }
        public bool IsItOld { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
