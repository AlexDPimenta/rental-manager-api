using Carter;
using RentalManager.WebApi;
using RentalManager.WebApi.Common.Extension;
using RentalManager.WebApi.Settings;

var builder = WebApplication.CreateBuilder(args);


var configuration = AppSettings.Configuration();

builder.Services.AddWebApi(configuration);  


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.ApplyMigrations();
app.MapCarter();

app.Run();




public partial class Program;
