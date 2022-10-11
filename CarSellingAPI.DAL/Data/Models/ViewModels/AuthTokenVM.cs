#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.Data.Models.ViewModels
{
    public class AuthTokenVM
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
