using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.Data.Models.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
        
    }
}
