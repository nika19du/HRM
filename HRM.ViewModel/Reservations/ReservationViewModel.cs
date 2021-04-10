using HRM.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.ViewModel.Reservations
{
    public class ReservationViewModel
    {
        public string Id { get; set; }
        public int AddultCount { get; set; }
        public int ChildCount { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public bool IncludeBreakfast { get; set; }
        public bool IsItAllInclusive { get; set; }
        public decimal Amount { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public Room Room{ get; set; }
        public string RoomType { get; set; }
        public string RoomNumber { get; set; }
        public string UserFullName => $"{UserFirstName} {UserLastName}";
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public bool UserIsAdult { get; set; }
    }
}
