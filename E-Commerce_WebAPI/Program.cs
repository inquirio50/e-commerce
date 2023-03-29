using Data;
using DataAccess.Repository;
using Domain.Contract;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<IBaseContract<Review>, ReviewRepository>();

builder.Services.AddScoped<IBaseContract<Product>, ProductRepository>();
builder.Services.AddScoped<IBaseContract2<Review>, ProductRepository>();

builder.Services.AddScoped<IBaseContract<Category>, CategoryRepository>();

builder.Services.AddScoped<IBaseContract<Order>, OrderRepository>();
builder.Services.AddScoped<IBaseContract2<OrderDetails>, OrderRepository>();

builder.Services.AddScoped<IBaseContract<Customer>, CustomerRepository>();
builder.Services.AddScoped<IBaseContract2<Order>, CustomerRepository>();
builder.Services.AddScoped<IBaseContractNR<Review>, CustomerRepository>();

//builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



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

//public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
//{
//    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        return DateOnly.FromDateTime(reader.GetDateTime());
//    }

//    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
//    {
//        var isoDate = value.ToString("O");
//        writer.WriteStringValue(isoDate);
//    }
//}