using CarSellingAPI.DAL.Data.Models;
using CarSellingAPI.DAL.Data.Models.ViewModels;
using CarSellingAPI.DAL.QueryInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.QueryProviders
{
    public class BookingQueryProvider : IBookingQuery
    {
        private readonly CardbContext _context;
        public BookingQueryProvider(CardbContext context)
        {
            _context = context;
        }

        public async Task AddBooking(Booking booking)
        {
            await _context.Booking.AddAsync(booking);
            _context.SaveChanges();

        }
        public async Task<Booking> UpdateBooking([FromBody] UpdateDepositVM updateDepositVM)
        {
            var val = await _context.Booking.SingleOrDefaultAsync(x => x.BookingId == updateDepositVM.BookingId);
            if (val == null) { return null; }
            val.Deposit = val.Deposit+updateDepositVM.amount;
            val.AmountLeft = val.AmountLeft - updateDepositVM.amount;
            _context.SaveChanges();
            return val;

        }

        public async Task<Booking> DeleteBooking(int bookingId)
        {
            var val = await _context.Booking.SingleOrDefaultAsync(x => x.BookingId == bookingId);
            _context.Booking.Remove(val);
            _context.SaveChanges();
            return val;
        }

        public async Task<Booking> GetBookingbyId(int booking_id)
        {
            var val = await _context.Booking.SingleOrDefaultAsync(x => x.BookingId == booking_id);
            if (val == null)
            {
                return null;
            }
            
            return val;
        }

    }
}
