using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.InputModel.Reservations
{
    public class ReservationInputModel
    {
        public string Id { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime AccommodationDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        public bool Breakfast { get; set; }

        public bool AllInclusive { get; set; }

        [BindNever]
        public double Price { get; set; }
        [BindNever]
        public int RoomCapacity { get; set; } 

        [BindNever]
        public double RoomAdultPrice { get; set; }

        [BindNever]
        public double RoomChildrenPrice { get; set; }
        [BindNever]
        public string RoomId { get; set; }
        public string UserId { get; set; }
    }
}
