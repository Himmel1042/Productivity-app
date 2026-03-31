using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using ProductivityApi.Data;
using ProductivityApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<TaskService>();

// Controllers
builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Swagger Development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Productivity API (Dev)",
            Version = "v1"
        });
    });
}

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

// Swagger UI Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Productivity API (Dev) v1");
    });
}

Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

app.Run();
