using CarSellingAPI.Business.Interfaces;
using CarSellingAPI.DAL.Data.Helpers;
using CarSellingAPI.DAL.Data.Models;
using CarSellingAPI.DAL.Data.Models.ViewModels;
using CarSellingAPI.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;
using static System.Net.WebRequestMethods;

namespace CarSellingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarController : ControllerBase
    {
        private readonly ICar _car;


        public CarController(ICar car, BuyingBookedCarVM buyingBookedCarVM, BuyingDirectCarVM buyingDirectCarVM)
        {
            _car = car;
            
        }
        // GET: api/Car
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cars>>> GetCars()
        {
            return await _car.Getcars();
        }

        // POST: api/Car/book
        [HttpPost("book")]
        public async Task<ActionResult<ResponseResult>> Book([FromBody] BookingVM bookingVM)
        {
            //booking the car using services
            var Booking = await _car.Book(bookingVM);
            return Booking;
        }

        // PUT: api/Car/UpdateDeposit
        [HttpPut("/UpdateDeposit")]
        public async Task<ResponseResult> UpdateDeposit([FromBody] UpdateDepositVM updateDepositVM)
        {
            var result = await _car.UpdateDeposit(updateDepositVM);

            return result;
        }

        // DELETE: api/Car/Removedeposit
        [HttpDelete("/RemoveDeposit")]
        public async Task<ResponseResult> RemoveDeposit(int bookingId)
        {
            var result = await _car.RemoveDeposit(bookingId);

            return result;
        }

        // POST: api/Car/buy/bookedcars
        [HttpPost("buy/bookedcars")]
        public async Task<ActionResult<BuyingBookedCarVM>> BuyBookedCars()
        {
            BuyingBookedCarVM _buyingBookedCarVM = new BuyingBookedCarVM
            {
                Href = Url.Link(nameof(BuyBookedCars), null)
            };
            return _buyingBookedCarVM;
        }

        // POST: api/Car/buy/directcars
        [HttpPost("buy/directcars")]
        public async Task<ActionResult<BuyingDirectCarVM>> BuyDirectCars()
        {
            BuyingDirectCarVM _buyingBookedCarVM = new BuyingDirectCarVM
            {
                Href = Url.Link(nameof(BuyingDirectCarVM), null)
            };
            return _buyingBookedCarVM;
        }

        // POST: api/Car/buy
        [HttpPost("buy")]
        public async Task<ActionResult<ResponseResult>> Buy(BuyingCarVM buyingCarVM)
        {
            //var response = new
            //{
            //    booked = new { href= Url.Link("http://localhost:7102/api/car/buy/bookedcars",null) },
            //    Direct = new { href = Url.Link(nameof(CarController.BuyBookedCars), null) }
            //};
            //return Ok(response);

            var result = await _car.Buy(buyingCarVM);

            return result;

        }


    }

}
