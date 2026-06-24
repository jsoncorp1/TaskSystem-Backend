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
    options.UseNpgsql(builder.Configuration.GetConnectionString("CoreConnection"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
//------------------------------------------------------
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<ProjectTypeService>();
builder.Services.AddScoped<ClientCompanyService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DepartmentUserService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<SubProjectService>();
//------------------------------------------------------
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
