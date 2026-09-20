using FluentValidation;
using Infrastructure.Configuration.Filters;
using Infrastructure.Extensions;
using WebApi.Base.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<AppExceptionFilterAttribute>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddCustomCors();

builder.Services.AddApplication(configuration);
builder.Services.AddInfrastructure(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UsePathBase(configuration.GetValue("PathBase", string.Empty));
app.UseHeaders();
app.UseSwagger(configuration);
app.UseHttpsRedirection();
app.UseRouting();
app.UseCustomCors();

app.MapControllers();

app.Run();
