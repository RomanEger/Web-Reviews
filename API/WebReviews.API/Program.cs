using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Service.Helpers;
using WebReviews.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCors();
//������������ �����
builder.Services.ConfigureDataBase(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();

//������������ �������������� �������
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<EntityChecker>();
builder.Services.AddAutoMapper(typeof(Program));

//������������ ��� JWT
builder.Services.ConfigureJwtSetting(builder.Configuration);
builder.Services.ConfigureJwt(builder.Configuration);

//������������ ��� ����
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

//������������ ��� ����������� ������
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

//������������ ����
app.UseResponseCaching();

//����������� � ������������
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
