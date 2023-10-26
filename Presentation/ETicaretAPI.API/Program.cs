using ETicaretAPI.API.Configurations;
using ETicaretAPI.API.Extensions;
using ETicaretAPI.Application;
using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Persistence;
using ETicaretAPISignalR;
using ETicaretAPISignalR.Hubs;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
//IoC servisleri
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
builder.Services.AddSignalServices();

builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options=>options.AddDefaultPolicy(policy=>
    policy.WithOrigins("http://localhost:4200", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
SqlColumn sqlColumn = new SqlColumn();
sqlColumn.ColumnName = "UserName";
sqlColumn.DataType = System.Data.SqlDbType.NVarChar;
sqlColumn.PropertyName = "UserName";
sqlColumn.DataLength = 50;
sqlColumn.AllowNull = true;
ColumnOptions columnOpt = new ColumnOptions();
columnOpt.Store.Remove(StandardColumn.Properties);
columnOpt.Store.Add(StandardColumn.LogEvent);
columnOpt.AdditionalColumns = new Collection<SqlColumn> { sqlColumn };
Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("SQLServer"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "logs",
            AutoCreateSqlTable = true
        },
        columnOptions: columnOpt)
    .WriteTo.Seq(builder.Configuration["Seq:ServerUrl"])
    .Enrich.FromLogContext()
    .Enrich.With<UsernameColumnWriter>()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields=HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
builder.Services.AddControllers(options => options.Filters.Add<ValitadionFilter>())
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);



/*
    

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
*/

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            //ClockSkew = TimeSpan.Zero,
            LifetimeValidator = ( notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow.AddHours(3) : false,
            NameClaimType=ClaimTypes.Name 
        };
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();

app.UseSerilogRequestLogging(); // bu log fonksiyonu çaðýrýldýðý yerden sonraki yerler loglanýr.
app.UseHttpLogging();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (context, next) => {
    var userName = context.User?.Identity?.IsAuthenticated!=null || true ? context.User.Identity.Name:null;
    LogContext.PushProperty("UserName",userName);
    await next();
});

app.MapControllers();
app.MapHubs();
app.Run();
