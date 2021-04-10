using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;

namespace HRM.InputModel.Reservations
{
    public class BookCreateInputViewModel
    {
        public string Id { get; set; }
        public string RoomTypeName{ get; set; }
        [Display(Name = "Adult price")]
        public decimal AdultPrice { get; set; }
        [Display(Name = "Child price")]
        public decimal ChildPrice { get; set; }  
        public int AddultCount { get; set; }
        public int Capacity { get; set; }
        public int ChildCount { get; set; }
        [Required]
        public DateTime CheckIn
        {
            get;
            set;
        }
        [Required]
        public DateTime CheckOut { get; set; }
        public bool IncludeBreakfast { get; set; }
        public bool IsItAllInclusive { get; set; } 
    }
}
