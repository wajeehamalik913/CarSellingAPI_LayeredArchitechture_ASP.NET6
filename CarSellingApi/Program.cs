
using Microsoft.EntityFrameworkCore;
using CarSellingAPI.DAL;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CarSellingAPI.Business.Interfaces;
using CarSellingAPI.Business.Services;
using CarSellingAPI.DAL.Models;
using CarSellingAPI.DAL.QueryInterface;
using CarSellingAPI.DAL.QueryProviders;
using CarSellingAPI.DAL.Data.Models.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using CarSellingAPI.Common.Middlewares;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


//Add database context to app.
builder.Services.AddDbContext<CardbContext>(
options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("cardb"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.25-mysql"));
});

//Adding services to app.
builder.Services.AddScoped<IAuth, AuthServices>();
builder.Services.AddScoped<BuyingDirectCarVM>();
builder.Services.AddScoped<BuyingBookedCarVM>();
builder.Services.AddScoped<ICar, CarServices>();
builder.Services.AddScoped<ICarQuery, CarQueryProvider>();
builder.Services.AddScoped<IBookingQuery, BookingQueryProvider>();
builder.Services.AddScoped<ITransactionQuery, TransactionQueryProvider>();



//Add token validation services.
var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"])),

    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],

    ValidateAudience = false,
    ValidAudience = builder.Configuration["JWT:Audience"],

    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
};


//Add single instance of the service to be used througout.
builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddMemoryCache();

//Add identity services
builder.Services.AddIdentityCore<User>(
    options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    })
    .AddEntityFrameworkStores<CardbContext>()
    .AddDefaultTokenProviders();

//Add JWT Authentcation service 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
//Add JWT Bearer
.AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = tokenValidationParameters;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Adding Middlewares to app.
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
