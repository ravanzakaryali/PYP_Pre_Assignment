using Business.Common;
using Business.Consumer;
using Business.Services.Abstracts;
using Business.Services.Implementations;
using Business.Validations;
using Core.Abstracts.UnitOfWork;
using Data.DataAccess;
using Data.Implementations.UnitOfWork;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<ReportConsumer>();
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["RabbitMq:URI"]), h =>
        {
            h.Username(builder.Configuration["RabbiqMq:Username"]);
            h.Password(builder.Configuration["RabbitMq:Password"]);
        });
        cfg.ReceiveEndpoint(RabbitMqConstants.SendReportQueue, ep =>
        {
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.Consumer<ReportConsumer>(context);
        });
    });
});

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SQlServer")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();