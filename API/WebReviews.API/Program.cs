using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Helpers;
using WebReviews.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors();
//Конфигурация слоев
builder.Services.ConfigureDataBase(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();

//Конфигурация дополнительных классов
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<EntityChecker>();
builder.Services.AddAutoMapper(typeof(Program));

//Конфигурация для JWT
builder.Services.ConfigureJwtSetting(builder.Configuration);
builder.Services.ConfigureJwt(builder.Configuration);

//Конфигурация для кэша
builder.Services.ConfigureResponseCaching();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


builder.Services.AddControllers(config =>
{
    config.CacheProfiles.Add("10minutesDurationPublic", new CacheProfile 
    { 
        Duration = 600,
        Location = ResponseCacheLocation.Any 
    });

    config.CacheProfiles.Add("5minutesDurationPrivate", new CacheProfile
    {
        Duration = 300,
        Location = ResponseCacheLocation.Client
    });
}).AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Конфигурация для обработчика ошибок
app.ConfigureExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
}); ;
app.UseCors("CorsPolicy");

//Конфигурация кэша
app.UseResponseCaching();

//Авторизация и аунтификация
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
