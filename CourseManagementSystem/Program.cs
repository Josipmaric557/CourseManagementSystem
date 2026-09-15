using System;
using System.Text;
using CourseManagementSystem.DbContextModel;
using CourseManagementSystem.Middleware;
using CourseManagementSystem.Repository.Implementation;
using CourseManagementSystem.Repository.Interfaces;
using CourseManagementSystem.Services.Implementation;
using CourseManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateBootstrapLogger();

try
{

    Log.Information("Pokretanje CourseManagementSystem aplikacije");

    var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICourseRepository, CourseRepository>();
    builder.Services.AddScoped<ILessonRepository, LessonRepository>();
    builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
    builder.Services.AddScoped<IUserCourseRepository, UserCourseRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<ITestRepository, TestRepository>();
    builder.Services.AddScoped<ITestQuestionRepository, TestQuestionRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();



    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<ICourseService, CourseService>();
    builder.Services.AddScoped<ILessonService, LessonService>();
    builder.Services.AddScoped<IReviewService, ReviewService>();
    builder.Services.AddScoped<IUserCourseService, UserCourseService>();
    builder.Services.AddScoped<IRoleService, RoleService>();
    builder.Services.AddScoped<ITestService, TestService>();
    builder.Services.AddScoped<ITestQuestionService, TestQuestionService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthService, AuthService>();


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

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

                ClockSkew = TimeSpan.Zero
            };
        });


    builder.Services.AddAuthorization();


    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "CourseManagementSystem API",
            Version = "v1"
        });


        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter: Bearer {your JWT token}"
        });


        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });


    builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();


    var app = builder.Build();


    app.UseMiddleware<ExceptionMiddleware>();


    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "CourseManagementSystem API v1");

        options.RoutePrefix = "swagger";
    });


    app.UseHttpsRedirection();


    app.UseAuthentication();

    app.UseAuthorization();


    app.MapControllers();


    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplikacije se srusila pri pokretanju, ispraviti greske!!!");
}
finally{
    Log.CloseAndFlush();
}