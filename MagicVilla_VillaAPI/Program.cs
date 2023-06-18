
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.Configure<VillaStoreDatabaseSettings>(
    builder.Configuration.GetSection(nameof(VillaStoreDatabaseSettings)));
builder.Services.AddSingleton<IVillaStoreDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<VillaStoreDatabaseSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(s=>
    new MongoClient(builder.Configuration.GetValue<string>("VillaStoreDatabaseSettings:ConnectionString")));
builder.Services.AddScoped<IVillaService, VillaService>();
builder.Services.AddControllers(option => { option.ReturnHttpNotAcceptable = false; }).AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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