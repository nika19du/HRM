using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.Data.Models
{
    public class Reservation
    { 
        public string RoomId { get; set; }
        public Room Room { get; set; }
        public string CustomerId { get; set; } 
        public User Customer { get; set; }//customer request
        /*        public List<Customer> Customers { get; set; }//for wich type adult or child how much guest */

        public virtual List<User> TypeGuests { get; set; } = new List<User>();
        [Required]
        public DateTime CheckIn { get; 
set; }
        [Required]
        public DateTime CheckOut { get; set; }
        public bool IncludeBreakfast { get; set; }
        public bool IsItAllInclusive { get; set; }
        public decimal Amount { get; set; }
    }
}
