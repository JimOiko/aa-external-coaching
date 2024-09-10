using AppointmentManagementSystem.DbObjects;
using Appointments.BLL;
using Appointments.DAL;
using AppointmentManagementSystem.Abstractions;
using Customers.DAL;
using Microsoft.EntityFrameworkCore;
using AppointmentManagementSystem.DomainObjects;

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
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<INameDayApiClient, NamedayApiClient>();
builder.Services.AddDbContext<AppointmentManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentManagementDatabase")));
builder.Services.AddHttpClient();
var configuration = new ConfigurationBuilder()
   .SetBasePath(Directory.GetCurrentDirectory())
   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
   .Build();
builder.Services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));

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
