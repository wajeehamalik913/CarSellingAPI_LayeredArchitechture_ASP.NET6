using CarSellingAPI.DAL.Data.Helpers;
using CarSellingAPI.DAL.Data.Models;
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
    public interface ICar
    {
        public Task<ActionResult<IEnumerable<Cars>>> Getcars();
        public Task<ResponseResult> Book([FromBody] BookingVM bookingVM);
        public Task<ResponseResult> UpdateDeposit([FromBody] UpdateDepositVM updateDepositVM);
        public Task<ResponseResult> RemoveDeposit(int bookingId);
        public Task<ResponseResult> Buy([FromBody] BuyingCarVM buyingCarVM);
    }
}
