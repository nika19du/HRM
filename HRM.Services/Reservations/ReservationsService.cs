
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HRM.Data;
using HRM.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Reservations
{
    public class ReservationsService : IReservationsService
    {
        private readonly Context context;
        private readonly IMapper mapper;
        public ReservationsService(Context ctx,IMapper mapper)
        {
            this.context = ctx;
            this.mapper = mapper;
        }
        public async Task Reserve(DateTime checkIn, DateTime checkOut, string roomId, User user, bool includeBreakfast, bool includeAllInclusive, int addultCount,int childCount)
        {
            var room = this.context.Rooms.Where(x => x.Id == roomId).FirstOrDefault();
            if (!await AreDatesAcceptable(roomId, checkIn, checkOut))
            {
                return;
            }
            decimal price = addultCount * room.AdultPrice + childCount * room.ChildPrice;
            if (includeAllInclusive)
            {
                var allIncl = (price / 100) * 15;
                price += allIncl;
            }
            if (includeBreakfast)
            {
                var breakfast = (price / 100) * 5;
                price += breakfast;
            }
            var reservation = new Reservation()
            {
                Id=Guid.NewGuid().ToString(),
                CheckIn = checkIn,
                CheckOut = checkOut,
                IsItAllInclusive = includeAllInclusive,
                IncludeBreakfast = includeBreakfast,
                Amount=price,
                Customer=user,
                CustomerId=user.Id,
                Room=room,
                RoomId=room.Id,
                AddultCount=addultCount,
                ChildCount=childCount
            };
            user.Reservations.Add(reservation);  
            this.context.Reservations.Add(reservation);
            await this.context.SaveChangesAsync();  
        }
        public async Task<Reservation> GetReservation<TModel>(string id)
        {
            return await this.context.Reservations.Where(x => x.Id == id).FirstOrDefaultAsync();  
        }
        public async Task<bool> DeleteReservation(string id)
        {
            var reservation = await this.context.Reservations.FindAsync(id);

            if (reservation != null)
            {  
                this.context.Reservations.Remove(reservation);
                await this.context.SaveChangesAsync(); 
                return true;
            } 
            return false;
        }
        public async Task<IEnumerable<TModel>> GetReservationsForUser<TModel>(string userId)
        {
            var model = await this.context.Reservations.Where(x => x.CustomerId == userId).OrderByDescending(x => x.CheckIn).ProjectTo<TModel>(this.mapper.ConfigurationProvider).ToListAsync();

            return model; 
        }

        public async Task<bool> AreDatesAcceptable(string roomId,
                                                  DateTime checkIn,
                                                  DateTime checkOut,
                                                  string reservationId = null)
        {
            if (checkIn >= checkOut || checkIn < DateTime.Today)
            {
                return false;
            }

            var reservationPeriods = await context.
                                           Reservations.
                                           Where(x => x.Room.Id == roomId).
                                           Select(x => new Tuple<DateTime, DateTime>
                                                        (x.CheckIn, x.CheckOut).
                                                        ToValueTuple()).
                                          ToListAsync();

            if (!string.IsNullOrWhiteSpace(reservationId))
            {
                var reservation = await context.Reservations.FirstOrDefaultAsync(x => x.Id == reservationId);
                reservationPeriods = reservationPeriods.Where(x => x.Item1 != reservation.CheckIn &&
                                                              x.Item2 != reservation.CheckOut).ToList();
            }

            return !reservationPeriods.Any(x =>
                (x.Item1 > checkIn && x.Item1 < checkOut) ||
                (x.Item2 > checkIn && x.Item2 < checkOut) ||
                (x.Item1 > checkIn && x.Item2 < checkOut) ||
                (x.Item1 < checkIn && x.Item2 > checkOut));
        }

        public async Task<bool> UpdateReservation(string id,
                                           DateTime checkIn,
                                           DateTime checkOut,
                                           bool allInclusive,
                                           bool breakfast,
                                           int addultCount, int childCount,
                                           User user)
        {
            var reservation = await context.Reservations.FirstOrDefaultAsync(x=>x.Id==id);

            var room = await context.Rooms.FirstOrDefaultAsync(x =>x.Id==reservation.RoomId);
              
            decimal price = addultCount * room.AdultPrice + childCount * room.ChildPrice;
            if (allInclusive)
            {
                var allIncl = (price / 100) * 15;
                price += allIncl;
            }
            if (breakfast)
            {
                var breakfastPrice = (price / 100) * 5;
                price += breakfastPrice;
            }
            var newReservation = new Reservation()
            {
                Id = reservation.Id,
                CheckIn = checkIn,
                CheckOut = checkOut,
                IsItAllInclusive = allInclusive,
                IncludeBreakfast = breakfast,
                Amount = price,
                Customer = user,
                CustomerId = user.Id,
                Room = room,
                RoomId = room.Id,
                AddultCount = addultCount,
                ChildCount = childCount
            }; 
            context.Entry(reservation).CurrentValues.SetValues(newReservation);
            await this.context.SaveChangesAsync();

            return true;
        }

    }
}
