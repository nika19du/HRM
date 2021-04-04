using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.ViewModel.Users
{
    public class UsersAllViewModel
    {
        public string Search { get; set; }
        public IEnumerable<UsersInfoViewModel> Users { get; set; }
    }
}
