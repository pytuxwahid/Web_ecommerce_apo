using Microsoft.AspNetCore.Mvc;
using Ecommerce_web_api.DTOs;
using Ecommerce_web_api.Services;
using Ecommerce_web_api.Interfaces;
using Ecommerce_web_api.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
// Add services to the controller.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddDbContext<AppDbContext>
    (options=>options.UseNpgsql
    (builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options=>
{
    options.InvalidModelStateResponseFactory=context=>
    {
        var errors=context.ModelState.Where
        (e=>e.Value !=null && e.Value.Errors.Count>0)
        .SelectMany(e=>e.Value?.Errors!= null? e.Value.Errors
        .Select(er=>er.ErrorMessage):new List<string>()).ToList();
        return new BadRequestObjectResult
        (ApiResponse<object>.ErrorResponse(errors,400,"Validation Failed"));
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>"API is working fine");

app.MapControllers();
app.Run();

//ok when confused what i am doing here please check my hand notes