using CarSellingAPI.DAL;
using CarSellingAPI.DAL.Data.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrypt;
using System.Security.Claims;
using CarSellingAPI.DAL.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using CarSellingAPI.Business.Interfaces;
using CarSellingAPI.DAL.Models;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace CarSellingAPI.Business.Services
{
    public class AuthServices : IAuth
    {
        private readonly UserManager<User> _userManager; //api for managing users
        private readonly IConfiguration _configuration; //provides configuration
        private readonly TokenValidationParameters _tokenValidationParameters; //Tokens validation
        private readonly IMemoryCache _cache; //local In-memory cache

        public AuthServices(UserManager<User> userManager,
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
            _cache = cache;
        }
        //Login Functionality Implmented
        public async Task<AuthTokenVM> Login([FromBody] LoginVM loginVM)
        {
            //encoder for hashing password
            ScryptEncoder encoder = new ScryptEncoder();
            var userExists = await _userManager.FindByEmailAsync(loginVM.Email);
            
            //checking if user exist
            if (userExists != null && await _userManager.CheckPasswordAsync(userExists, loginVM.Password))
            {
                //generate jwt token
                var tokenValue = await GenerateJWTTokenAsync(userExists, null);
                return tokenValue;
            }
            return null;
        }

        //Generating JWT token implemented
        private async Task<AuthTokenVM> GenerateJWTTokenAsync(User user, string rToken)
        {

            //adding claims for authentication
            var authClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.Name, user.UserName),
                new Claim (ClaimTypes.NameIdentifier, user.Id),
                new Claim (JwtRegisteredClaimNames.Email,user.Email),
                new Claim (JwtRegisteredClaimNames.Sub,user.Email),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };


            //using authsigning key for signing
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            //creating a token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT.Audience"],
                expires: DateTime.UtcNow.AddMinutes(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            //creating a jwt token
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            //checking if the refresh token exists
            if (rToken != null)
            {
                var rTokenResponse = new AuthTokenVM()
                {
                    Token = jwtToken,
                    RefreshToken = rToken,
                    UserId = user.Id,
                    ExpiresAt = token.ValidTo,
                };
                //storing the tken in cache
                _cache.Set(key: "token", rTokenResponse, TimeSpan.FromMinutes(3));
                return rTokenResponse;
            }
            //creating refresh token
            var refreshToken = new RefreshToken()
            {
                JWTId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = (DateTime.UtcNow.AddSeconds(6)).ToOADate(),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
            };

            // await _context.RefreshTokens.AddAsync(refreshToken);
            //await _context.SaveChangesAsync();
            var response = new AuthTokenVM()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                UserId = refreshToken.UserId,
                ExpiresAt = token.ValidTo,
            };

            //caching the token
            _cache.Set(key: "token", response, TimeSpan.FromMinutes(3));
            return response;
        }
        public async Task<User?> Register([FromBody] RegisterVM registerVM)
        {
            //encoder for hashing password
            ScryptEncoder encoder = new ScryptEncoder();
            var pass = encoder.Encode(registerVM.Password);

            //checking if user exists
            var userExists = await _userManager.FindByEmailAsync(registerVM.Email);
            if (userExists != null)
            {
                return null;
            }
            
            User newUser = new User()
            {
                UserName = registerVM.UserName,
                
                Email = registerVM.Email,
               
                PasswordHash = pass,
              
            };
            //var res = await _context.Users.AddAsync(newUser);
            //_context.SaveChanges();

            //registring the user
            var result = await _userManager.CreateAsync(newUser, registerVM.Password);
            //_context.SaveChanges();
           
            return newUser;

        }

        //creating refresh token
        public async Task<AuthTokenVM> RefreshToken([FromBody] RefreshTokenVM refreshTokenVM)
        {
            var result = await VerifyAndGenerateTokenAsync(refreshTokenVM);
            return result;
        }

        //verifying the token and refresh token and generating a new token
        private async Task<AuthTokenVM> VerifyAndGenerateTokenAsync(RefreshTokenVM refreshTokenVM)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            AuthTokenVM authToken = new AuthTokenVM();
            User user = await _userManager.FindByIdAsync(refreshTokenVM.user_Id);
            //getting the cache value
            authToken = _cache.Get<AuthTokenVM>(key: "token");

            //checking if token exist in cache if not generate a new token
            if (authToken == null)
            {
                return await GenerateJWTTokenAsync(user, refreshTokenVM.RefreshToken);
            }

            //getting the token from cache
            var storedToken = authToken.RefreshToken;

            //getting user of the token
            var dbUser = await _userManager.FindByIdAsync(authToken.UserId);
            try
            {
                //checking validity of token
                var tokenCheckResult = jwtTokenHandler.ValidateToken(authToken.Token, _tokenValidationParameters,
                    out var validatedToken);
                return await GenerateJWTTokenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException)
            {

                return await GenerateJWTTokenAsync(dbUser, null);


            }
        }

    }
}
