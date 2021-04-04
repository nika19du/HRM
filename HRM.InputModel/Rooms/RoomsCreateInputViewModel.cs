using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.InputModel.Rooms
{
    public class RoomsCreateInputViewModel
    { 
        public string Id { get; set; }
        [Required]
        [Display(Name = "Room type")]
        public string Type { get; set; }
        [Required]
        public int Capacity { get; set; } 
        [Display(Name ="Is it available?")]
        public bool IsItAvailable { get; set; } 
        public IFormFile Image { get; set; }
        [Required]
        [Display(Name ="Adult price")]
        public decimal AdultPrice { get; set; }
        [Required]
        [Display(Name = "Child price")] 
        public decimal ChildPrice { get; set; }
        public int Number { get; set; }
    }
}
