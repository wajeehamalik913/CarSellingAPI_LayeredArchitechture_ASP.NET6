using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.Data.Models.ViewModels
{
    public class BuyingBookedCarVM : Resource
    {
        public int BookingId { get; set; }
        public int Amount { get; set; }
    }
}
