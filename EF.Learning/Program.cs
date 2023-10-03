using EF.Learning.Services;
using EF.Learning.Storage;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.Internal;

var cnnstr = "Host=postgres;Port=5432;Database=test;Username=postgres;Password=example";

try
{
    using var conn = new NpgsqlConnection("Host=postgres;Port=5432;Database=postgres;Username=postgres;Password=example");
    conn.Open();
    using var command1 = conn.CreateCommand();
    command1.CommandText = "CREATE DATABASE test";
    command1.CommandType = System.Data.CommandType.Text;
    command1.ExecuteNonQuery();
}
catch(Exception ex)
{

}
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<DbCreator>();
builder.Services.AddDbContextFactory<TestDbContext>(options => options.UseNpgsql(cnnstr));


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
