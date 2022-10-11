using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace CarSellingAPI.Common.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        // private readonly ILogger _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next/*,ILogger logger*/)
        {
            this.Next = next;
            //_logger = logger;
        }
        public RequestDelegate Next { get; private set; }

        //Invoke the handler
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await Next(httpContext);
            }
            catch (Exception error)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case TimeoutException e:
                        //_logger.LogError("timed out");
                        response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
                        break;

                    case OutOfMemoryException e:
                        //_logger.LogError("Out of memory!!! You have insufficient storage");
                        response.StatusCode = (int)HttpStatusCode.InsufficientStorage;
                        break;

                    case KeyNotFoundException e:
                        //_logger.LogError("Not Found");
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case SecurityTokenExpiredException e:
                        //_logger.LogError("The token is expired");
                        httpContext.Response.Headers.Add("Token-Expired", "true");
                        break;

                    case UnauthorizedAccessException e:
                        //_logger.LogError("Unuthorized to access this resource");
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    default:

                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        ///_logger.LogError(response.ToString());
                        break;
                }

            }

            //await Next(httpContext);
        }
    
    }
}
