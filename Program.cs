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

var devCorsPolicy = "devCorsPolicy";
builder.Services.AddCors(options => { options.AddPolicy(devCorsPolicy, builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "AllowOrigin",
//        builder =>
//        {
//            builder.WithOrigins("https://localhost:7299")
//                                .AllowAnyHeader()
//                                .AllowAnyMethod();
//        });
//});
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
