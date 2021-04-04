using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.ViewModel.Rooms
{
    public class RoomsAllViewModel
    {
        public string Search { get; set; }
        public IEnumerable<RoomsInfoViewModel> Rooms { get; set; }
    }
}
