using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.ViewModel.RoomTypes
{
   public class RoomTypesAllViewModel
    {
        public string Search
        {
            get;
            set;
        }
        public IEnumerable<RoomTypesInfoViewModel> Types { get; set; }
    }
}
