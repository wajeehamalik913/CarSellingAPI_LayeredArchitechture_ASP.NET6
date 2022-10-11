#nullable disable
using CarSellingAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.Data.Models
{
    public class Cars
    {
        [Key]
        public int CarId { get; set; }
        public string CarName { get; set; }
        public string BrandName { get; set; }
        public int Price { get; set; }
        public int MinDeposit { get; set; }
        public string Status { get; set; }
        public string BuyerId { get; set; }
        [ForeignKey(nameof(BuyerId))]
        public User User { get; set; }
    }
}
