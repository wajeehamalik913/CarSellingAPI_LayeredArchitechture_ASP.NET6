using CarSellingAPI.DAL;
using CarSellingAPI.DAL.Data.Models.ViewModels;
using CarSellingAPI.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.Business.Interfaces
{
    public interface IAuth
    {
        public Task<User?> Register([FromBody] RegisterVM registerVM);
        public Task<AuthTokenVM> Login([FromBody] LoginVM loginVM);
        public Task<AuthTokenVM> RefreshToken([FromBody] RefreshTokenVM refreshTokenVM);
    }
}
