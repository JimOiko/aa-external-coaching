using AppointmentManagementSystem.DbObjects;
using Appointments.API.Interfaces;
using Appointments.API.Services;
using Appointments.DAL;
using Appointments.DAL.Interfaces;
using Customers.DAL;
using Customers.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentsService, AppointmentsService>();
builder.Services.AddDbContext<AppointmentManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentManagementDatabase")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
