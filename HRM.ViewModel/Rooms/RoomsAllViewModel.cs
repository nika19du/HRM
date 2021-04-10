using System;
using System.Collections.Generic;
using System.Linq; 

namespace HRM.ViewModel.Rooms
{
    public class RoomsAllViewModel
    {
        public string Search { get; set; }
        public IEnumerable<RoomsInfoViewModel> Rooms { get; set; } 
    }
}
