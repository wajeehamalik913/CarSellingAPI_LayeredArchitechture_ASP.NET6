using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace CarSellingAPI.DAL.Data.Models
{
    public class Transaction
    {
        [Key]
        public int TranId { get; set; }
        public int CarId { get; set; }
        public string UserId { get; set; }
        public int Payment { get; set; }
        public string PaymentType { get; set; }
    }
}
