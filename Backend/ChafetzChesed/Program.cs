using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using ChafetzChesed;
using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.BLL.Services;
using ChafetzChesed.DAL.Data;
using ChafetzChesed.Common;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// טעינת הגדרות JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

// הגדרת אימות JWT עם שם משתמש מתוך claim בשם "sub"
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

        // 💥 שורה קריטית: ת"ז תישלף מתוך claim בשם "sub"
        NameClaimType = JwtRegisteredClaimNames.Sub
    };
});

// CORS לאנגולאר
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// שירותים חיצוניים
builder.Services.AddHttpClient();

// רישום שירותים
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IDepositTypeService, DepositTypeService>();
builder.Services.AddScoped<ILoanTypeService, LoanTypeService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddScoped<IExternalFormService, ExternalFormService>();
builder.Services.AddScoped<IExternalUserSyncService, ExternalUserSyncService>();
builder.Services.AddScoped<JwtService>();

// חיבור למסד הנתונים
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost4200");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
