using System.Text;
using BookstoreAPI.Settings;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddTransient<IMail_Service, BookstoreAPI.Settings.MailService>();


builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString ?? throw new Exception("Token key doesn't found"))),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


var app = builder.Build();


app.UseCors("DevCors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
