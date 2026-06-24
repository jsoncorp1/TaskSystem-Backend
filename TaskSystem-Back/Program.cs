using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TaskSystem_Back.Data;
using TaskSystem_Back.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Swagger------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT"
    });
});

var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();



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
builder.Services.AddScoped<TaskItemService>();
builder.Services.AddScoped<SubTaskService>();
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
