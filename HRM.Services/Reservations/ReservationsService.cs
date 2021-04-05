using HRM.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Reservations
{
    public class ReservationsService : IReservationsService
    {
        private readonly Context context;
        public ReservationsService(Context ctx)
        {
            this.context = ctx;
        }
        public async Task Reserve(DateTime checkIn, DateTime checkOut, string roomId, string customer, bool includeBreakfast, bool includeAllInclusive, decimal amount)
        {

        }
    }
}
