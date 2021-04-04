using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRM.InputModel.RoomTypes
{
   public class RoomTypesCreateInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}
