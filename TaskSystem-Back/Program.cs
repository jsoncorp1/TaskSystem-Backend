using Microsoft.EntityFrameworkCore;
using TaskSystem_Back.Data;
using TaskSystem_Back.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Swagger------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//PostgreSQL / Neon-------------------------------------- // <-- nuevo
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CoreConnection")));
//------------------------------------------------------
builder.Services.AddScoped<RoleService>();
var app = builder.Build();
//------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//-----------------------------------------------------
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
