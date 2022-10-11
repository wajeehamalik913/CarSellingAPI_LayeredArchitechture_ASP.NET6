using CarSellingAPI.Business.Interfaces;
using CarSellingAPI.DAL;
using CarSellingAPI.DAL.Data.Helpers;
using CarSellingAPI.DAL.Data.Models;
using CarSellingAPI.DAL.Data.Models.ViewModels;
using CarSellingAPI.DAL.Models;
using CarSellingAPI.DAL.QueryInterface;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.Business.Services
{
    public class CarServices :ICar
    {
        //Used for querying car related database
        private readonly ICarQuery _car; 

        //Used for querying booking related database
        private readonly IBookingQuery _booking; 

        //Used for querying transaction related database
        private readonly ITransactionQuery _transaction; 

        public CarServices(ICarQuery car, IBookingQuery booking, ITransactionQuery transaction)
        {
            _car = car;
            _booking = booking;
            _transaction = transaction;
        }

        //booking the car to buy
        public async Task<ResponseResult> Book([FromBody] BookingVM bookingVM)
        {
            //getting car from DAL 
            var car = _car.GetCarbyId(bookingVM.CarId);

            //if the car is already booked or sold
            if(car.Result.Status=="Booked" || car.Result.Status == "Owned")
            {
                ResponseResult response = new ResponseResult()
                {
                    Succeeded = false,
                    Message = "Car not available",
                    StatusCode = 400,
                };
                return response;
            }

            //if the car does not exists
            else if(car == null)
            {
                ResponseResult response = new ResponseResult()
                {
                    Succeeded = false,
                    Message = "No such car exist",
                    StatusCode = 400,
                };
                return response;
            }
            //checking if the deposit amount is less than min allowed deposit
            else if (bookingVM.Deposit < car.Result.MinDeposit)
            {
                ResponseResult response = new ResponseResult()
                {
                    Succeeded = false,
                    Message = "Your deposit is less than min deposit required",
                    StatusCode = 400,
                };
                return response;
            }

            //updating the status of car to booked and buyerId
            car.Result.BuyerId = bookingVM.UserId;
            car.Result.Status = "Booked";
            //request to DAL to update the car status
            await _car.UpdateCarStatus(car.Result);

            //creating a new booking to be added to database
            Booking newBooking = new Booking()
            {
                CarId = bookingVM.CarId,
                UserId = bookingVM.UserId,
                Deposit = bookingVM.Deposit,
                AmountLeft = car.Result.Price - bookingVM.Deposit,
            };

            //creating a new transaction to be added to database
            Transaction newTransaction = new Transaction()
            {
                CarId = bookingVM.CarId,
                UserId = bookingVM.UserId,
                Payment = bookingVM.Deposit,
                PaymentType = "Deposit",
            };
            //requesting the data access layer to add booking and transaction
            await _booking.AddBooking(newBooking);
            await _transaction.AddTransaction(newTransaction);
            return new ResponseResult()
            {
                Succeeded = true,
                Message = "Your Car id Booked",
                StatusCode = 200,
            };
        }

        //getting all cars
        public async Task<ActionResult<IEnumerable<Cars>>> Getcars()
        {
            var val = await _car.GetAllcars();
            return val;
        }

        //updating the deposit
        public async Task<ResponseResult> UpdateDeposit([FromBody] UpdateDepositVM updateDepositVM)
        {
            //getting the booking and updating it
            var book = _booking.UpdateBooking(updateDepositVM);

            //if booking exist to be updated
            if (book == null){
                return new ResponseResult()
                {
                    Succeeded = false,
                    Message = "No such Booking exist",
                    StatusCode = 400,
                };
            }
            //adding a new transaction of deposit added
            Transaction newTransaction = new Transaction()
            {
                CarId = book.Result.CarId,
                UserId = book.Result.UserId,
                Payment = book.Result.Deposit,
                PaymentType = "Deposit-Update",
            };
            //request to add transaction in database
            await _transaction.AddTransaction(newTransaction);

            return new ResponseResult()
            {
                Succeeded = true,
                Message = "Deposit Updated",
                StatusCode = 200,
            };

        }

        //Removing deposit and deleting the bookings
        public async Task<ResponseResult> RemoveDeposit(int bookingId)
        {
            //deleting the bookings from database
            var book = await _booking.DeleteBooking(bookingId);
          
            //adding new transaction of giving deposit back
            Transaction newTransaction = new Transaction()
            {
                CarId = book.CarId,
                UserId = book.UserId,
                Payment = 0-book.Deposit,
                PaymentType = "Deposit-Returned",
            };
            //request to add transaction to database
            await _transaction.AddTransaction(newTransaction);

            //getting car whose booking is deleted 
            var car = _car.GetCarbyId(book.CarId);

            //updating the status of that car to available again and buyer id as null
            car.Result.BuyerId = null;
            car.Result.Status = "Available";

            //request to update car status in database
            await _car.UpdateCarStatus(car.Result);
            return new ResponseResult()
            {
                Succeeded = true,
                Message = "Deposit Updated",
                StatusCode = 200,
            };
        }

        //Buying a booked car or a direct car
        public async Task<ResponseResult> Buy([FromBody] BuyingCarVM buyingCarVM)
        {

            //getting car to buy from database
            var car = _car.GetCarbyId(buyingCarVM.CarId);

            

            //checking if car is already sold
            if (car.Result.Status == "Owned")
            {
                return new ResponseResult()
                {
                    Succeeded = false,
                    Message = "Car already Sold",
                    StatusCode = 400,
                };
            }
            //checking if car is booked
            else if (car.Result.Status == "Booked")
            {
                //getting the booking from database
                var booking = _booking.GetBookingbyId(buyingCarVM.BookingId);

                //checking if the user id is same to the user booked the  car
                if (car.Result.BuyerId != buyingCarVM.UserId)
                {
                    return new ResponseResult()
                    {
                        Succeeded = false,
                        Message = "car booked by another user",
                        StatusCode = 400,
                    };
                }
                //checking the amount payed by user is equal to amount left to be payed
                else if ( buyingCarVM.amount != booking.Result.AmountLeft)
                {
                    return new ResponseResult()
                    {
                        Succeeded = false,
                        Message = "Pay full amount to buy",
                        StatusCode = 400,
                    };

                }
                
                //once owned deleting the car from bookings table
                await _booking.DeleteBooking(buyingCarVM.BookingId);

                //adding a new transaction of car being sold
                Transaction newTransaction = new Transaction()
                {
                    CarId = buyingCarVM.CarId,
                    UserId = buyingCarVM.UserId,
                    Payment = 0 - buyingCarVM.amount,
                    PaymentType = "Remaining-Payment",
                };
                //requesting database to add transaction
                await _transaction.AddTransaction(newTransaction);

                //changing the status of car to owned 
                car.Result.BuyerId = buyingCarVM.UserId;
                car.Result.Status = "Owned";
                await _car.UpdateCarStatus(car.Result);
            }
            //if the car is bought directly

            //checking if the amount to be payed is equal to price of car
            if (buyingCarVM.amount != car.Result.Price)
            {
                return new ResponseResult()
                {
                    Succeeded = false,
                    Message = "Pay full amount to buy",
                    StatusCode = 400,
                };
            }
            //add a new transaction
            Transaction newTransaction2 = new Transaction()
            {
                CarId = buyingCarVM.CarId,
                UserId = buyingCarVM.UserId,
                Payment = 0 - buyingCarVM.amount,
                PaymentType = "Full-Payment",
            };
            await _transaction.AddTransaction(newTransaction2);
            car.Result.BuyerId = buyingCarVM.UserId;
            car.Result.Status = "Owned";
            await _car.UpdateCarStatus(car.Result);
            return new ResponseResult()
            {
                Succeeded = true,
                Message = "Hurray! You bought a Car",
                StatusCode = 200,
            };
        }
    }
}
