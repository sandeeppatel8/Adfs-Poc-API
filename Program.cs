using Adfs_Poc_API;
using Adfs_Poc_API.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

//var bindSwaggerAdSettings = new SwaggerAzureAdSettings();
//builder.Configuration.Bind("SwaggerAzureAd", bindSwaggerAdSettings);
//SwaggerConfig.SwaggerConfigurations(builder.Services, builder.Configuration, bindSwaggerAdSettings);

var devCorsPolicy = "devCorsPolicy";
builder.Services.AddCors(options => { options.AddPolicy(devCorsPolicy, builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(devCorsPolicy);
app.MapControllers();

app.Run();
