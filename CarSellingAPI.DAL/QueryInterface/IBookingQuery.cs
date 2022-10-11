using CarSellingAPI.DAL.Data.Models;
using CarSellingAPI.DAL.Data.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.QueryInterface
{
    public interface IBookingQuery
    {
        public Task AddBooking(Booking booking);
        public Task<Booking> GetBookingbyId(int booking_id);
        public Task<Booking> UpdateBooking([FromBody] UpdateDepositVM updateDepositVM);
        public Task<Booking> DeleteBooking(int bookingId);
    }
}
