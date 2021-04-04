using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Data.Models
{
    public class Reservation
    { 
        public string RoomId { get; set; }
        public Room Room { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }//customer request
        public List<Customer> Customers { get; set; }//for wich type adult or child how much guest 
        public DateTime CheckIn { get; 
set; }
        public DateTime CheckOut { get; set; }
        public bool IncludeBreakfast { get; set; }
        public bool IsItAllInclusive { get; set; }
        public decimal Amount { get; set; }
    }
}
