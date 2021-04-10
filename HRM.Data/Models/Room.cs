using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.Data.Models
{
    public class Room
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public bool IsItAvailable { get; set; }
        [Required]
        public string Type { get; set; }
        public RoomType RoomType { get; set; }
        public string Image { get; set; }
        [Required]
        public decimal AdultPrice { get; set; }
        [Required]
        public decimal ChildPrice { get; set; }
        public int Number { get; set; }
    }
}
