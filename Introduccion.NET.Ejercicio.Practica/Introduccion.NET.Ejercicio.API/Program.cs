using Introduccion.NET.Ejercicio.API.Extensions;


var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
IWebHostEnvironment env = builder.Environment;
var enviroment = env.EnvironmentName;

//Configuration
builder.Services.ConfigureCors();
builder.Services.ConfigureSqlContext(configuration);
builder.Services.AddTokenAuthentication(configuration);
builder.Services.ConfigureInterfaces();
builder.Services.AddSwagger(configuration, env);
builder.Services.AddServiceFactory(configuration, env);

//Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Builde APP
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
