using DocumentFormat.OpenXml.Drawing;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using HostelHelpDesk.Application.Interfaces;
using HostelHelpDesk.Application.Services;
using HostelHelpDesk.Application.Services.Background;
using HostelHelpDesk.Persistence.Data;
using HostelHelpDesk.Persistence.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization  header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            /*policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();*/
            builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
        });
});

builder.Services.AddScoped<JwtServices>();
builder.Services.AddScoped<RoomRepository>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<HostelRepository>();
builder.Services.AddScoped<HostelService>();
builder.Services.AddScoped<ITimeslotRepository, TimeslotRepository>();
builder.Services.AddScoped<ITimeslotService, TimeslotService>();
builder.Services.AddScoped<IComplaintTypeRepository, ComplaintTypeRepository>();
builder.Services.AddScoped<ComplaintTypeWorkerTypeRepository>();
builder.Services.AddScoped<ComplaintTypeWorkerTypeService>();
builder.Services.AddScoped<ComplaintRepository>();
builder.Services.AddScoped<ComplaintService>();
builder.Services.AddHostedService<ComplaintAssignmentService>();


builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
//builder.WebHost.UseUrls("http://*:5000");

builder.Services.AddDbContext<HostelComplaintsDB>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("con"))
    options.UseMySql(
        builder.Configuration.GetConnectionString("con"), 
        new MySqlServerVersion(new Version(8, 0, 41))
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// comment the above if block code and uncomment the below two lines before publishing for using it on old lenovo laptop
//app.UseSwagger();
//app.UseSwaggerUI();

// Comment the below line before publishing for using it on old lenovo laptop
//app.UseHttpsRedirection();


app.UseCors();
app.UseAuthorization();
app.MapControllers();
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<HostelComplaintsDB>();
//    db.Database.Migrate();
//}

app.Run();
