using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using todoapi.Controllers;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddTransient<IStartupFilter, RequestSetOptionsStartupFilter>();

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList")); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseExceptionHandler("/Error-dev");
    /*
    app.UseExceptionHandler(appError =>
        appError.Run( async context =>
        {
            context.Response.StatusCode = 555;
            context.Response.ContentType = "application/json";
            var contFeat = context.Features.Get<IExceptionHandler>();
            if (contFeat is not null)
            {
                await context.Response.WriteAsJsonAsync(new { StatusCode = 556, Message = "response vraceny z handleru"});
            }
        }
            
            )
    );*/
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
