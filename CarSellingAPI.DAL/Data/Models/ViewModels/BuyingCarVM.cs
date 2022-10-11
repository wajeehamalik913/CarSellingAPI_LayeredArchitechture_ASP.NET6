using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.Data.Models.ViewModels
{
    public class BuyingCarVM
    {
        public int BookingId { get; set; }
        public int CarId { get; set; }
        public string UserId { get; set; }
        public int amount { get; set; }
    }
}
