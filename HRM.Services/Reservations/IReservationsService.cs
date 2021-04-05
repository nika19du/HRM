using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Reservations
{
    public interface IReservationsService
    {
        Task Reserve(DateTime checkIn,DateTime checkOut,string roomId, string customer,bool includeBreakfast,bool includeAllInclusive,decimal amount);
    }
}
