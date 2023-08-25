using Infrastructure.MiddleWare;
using LeaveManagmentSystemAPI;
using LeaveManagmentSystemAPI.Data;
using LeaveManagmentSystemAPI.Mappings;
using LeaveManagmentSystemAPI.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LeaveManagmentSystemAPI.Helpers;
using LeaveManagmentSystemAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);


//policy.WithOrigins("http://localhost:3000")

builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader())
);

builder.Services.ConfigureSqlContext(builder.Configuration);

builder.Services.AddIdentity<Employee, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 7;
    opt.Password.RequireDigit = false;
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings.GetSection("securityKey").Value))
    };
});

builder.Services.AddScoped<JwtHandler>();

builder.Services.ConfigureRepositoryManager();

// Add services to the container.


builder.Services.AddHttpContextAccessor();

//filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogActionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(Maps));

builder.Services.AddSwaggerGen();

var app = builder.Build();

var supportedCultures = new string[] { "en-GB" };

// DEFAULT CULTURE SETUP
app.UseRequestLocalization(options =>
                options
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures)
                .SetDefaultCulture("en-GB")
                .RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    return Task.FromResult(new ProviderCultureResult("en-GB") ?? null);
                }))
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
ILogger logger = app.Logger;

IWebHostEnvironment env = app.Environment;

//IService service = app.Services.GetRequiredService<IService>();

IHostApplicationLifetime lifetime = app.Lifetime;

//lifetime.ApplicationStopped.Register(() =>
//    logger.LogInformation(
//        $"The application {env.ApplicationName} started" +
//        $" with injected {service}"));

app.UseRequestCulture();

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<Employee>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        SeedData.Seed(userManager, roleManager);
    }
    catch (Exception)
    {
        throw;
    }
}

app.UseEndpoints(endpoints =>
{
    //endpoints.MapControllerRoute(
    //    name: "defaultArea",
    //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();



 