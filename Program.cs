using Microsoft.EntityFrameworkCore;
using GestionViajes.API.Data;
using GestionViajes.API.Interfaces;
using GestionViajes.API.Repositories;
using GestionViajes.API.Services;
using GestionViajes.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IDestinoRepository, DestinoRepository>();
builder.Services.AddScoped<ITuristaRepository, TuristaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();

// Register services
builder.Services.AddScoped<IDestinoService, DestinoService>();
builder.Services.AddScoped<ITuristaService, TuristaService>();
builder.Services.AddScoped<IReservaService, ReservaService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Gestión de Viajes API", 
        Version = "v1",
        Description = "API para la gestión de destinos turísticos y turistas",
        Contact = new()
        {
            Name = "Equipo de Desarrollo",
            Email = "desarrollo@gestionviajes.com"
        }
    });
    
    // Include XML comments for better API documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestión de Viajes API v1");
        c.RoutePrefix = "swagger"; // Swagger UI at /swagger
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
