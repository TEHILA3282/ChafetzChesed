using Microsoft.EntityFrameworkCore;
using ChafetzChesed.BLL.Interfaces;
using ChafetzChesed.BLL.Services;
using ChafetzChesed.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IDepositTypeService, DepositTypeService>();
builder.Services.AddScoped<ILoanTypeService, LoanTypeService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IDepositService, DepositService>();

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
