using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<AppDbContext>(o => {
    o.UseSqlite("Data Source=FreshDb.db");
});

builder.Services.AddScoped<CategoryService>();

var key = Encoding.ASCII.GetBytes("MY_SUPER_SECRET_KEY_1234567890123456");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

builder.Services.AddCors(p => {
    p.AddPolicy("Any", cfg =>
    {
        cfg.AllowAnyOrigin();
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseRouting();
app.UseCors("Any");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
