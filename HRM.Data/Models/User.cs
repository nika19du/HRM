using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string EGN { get; set; }
        public DateTime NominationDate { get; 
set; }
        public bool IsItActive { get; set; }
        public DateTime ReleaseDate { get; set; }

        public bool IsItOld { get; set; }
        public virtual List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
