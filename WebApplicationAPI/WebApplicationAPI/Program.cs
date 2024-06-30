using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplicationAPI.DBContext;
using WebApplicationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
//              .ReferenceHandler = ReferenceHandler.Preserve); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CItyInfoDBContext>(options => {
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:CityConnection"]);
});
builder.Services.AddScoped<ICityInfoRepository,CityIfoRepository>();
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
