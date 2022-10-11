#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.Data.Models.ViewModels
{
    public class RefreshTokenVM
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string user_Id { get; set; }
    }
}
