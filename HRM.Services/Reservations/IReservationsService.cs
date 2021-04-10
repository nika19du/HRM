using HRM.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Reservations
{
    public interface IReservationsService
    {
        Task Reserve(DateTime checkIn, DateTime checkOut, string roomId, User user, bool includeBreakfast, bool includeAllInclusive, int addultCount, int childCount);
        Task<bool> AreDatesAcceptable(string roomId,
                                                  DateTime checkIn,
                                                  DateTime checkOut,
                                                  string reservationId = null);
        Task<IEnumerable<TModel>> GetReservationsForUser<TModel>(string userId);
        Task<bool> DeleteReservation(string id);
        Task<Reservation> GetReservation<TModdel>(string id);
        Task<bool> UpdateReservation(string id,
                                           DateTime checkIn,
                                           DateTime checkOut,
                                           bool allInclusive,
                                           bool breakfast,
                                           int addultCount, int childCount,
                                           User user);
    }
}
