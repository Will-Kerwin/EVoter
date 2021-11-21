using EVoterAPI.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EVoterContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EVoter")));

var app = builder.Build();
SeedData.EnsureSeeded(app.Services);

app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowedToAllowWildcardSubdomains());

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
