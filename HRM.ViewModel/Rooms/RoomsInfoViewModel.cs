using HRM.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.ViewModel.Rooms
{
    public class RoomsInfoViewModel
    {
        public string Id { get; set; }
        public RoomType RoomType { get; set; } 
        public int Capacity { get; set; } 
        public bool IsItAvailable { get; set; } 
        public string Image { get; set; } 
        public decimal AdultPrice { get; set; } 
        public decimal ChildPrice { get; set; }
        public int Number { get; set; }
    }
}
