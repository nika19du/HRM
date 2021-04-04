using AutoMapper;
using HRM.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Services.Rooms.Model
{
    public class RoomServiceModel
    {
        public string Id { get; set; }
        public int Capacity { get; set; }
        public bool IsItAvailable { get; set; }
        public string Type { get; set; }
        public RoomType RoomType { get; set; }
       [IgnoreMap]
        public string Image { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public int Number { get; set; }
    }
}
