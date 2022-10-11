using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace CarSellingAPI.DAL.Data.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public int CarId { get; set; }
        public string UserId { get; set; }

        public int Deposit { get; set; }
        public int AmountLeft { get; set; }

    }
}
