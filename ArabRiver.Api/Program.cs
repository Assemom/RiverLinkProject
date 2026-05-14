using ArabRiver.Api.Jobs;
using ArabRiver.Api.Seed;
using ArabRiver.Api.Settings;
using ArabRiver.Repository.Data;
using ArabRiver.Repository.Interfaces;
using ArabRiver.Repository.Repositories;
using ArabRiver.Service.Helpers;
using ArabRiver.Service.Interfaces;
using ArabRiver.Service.Mapping;
using ArabRiver.Service.Responses;
using ArabRiver.Service.Services;
using ArabRiver.Service.Validations.Catalog;
using AutoMapper.Internal;
using FluentValidation;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.RateLimiting;

namespace ArabRiver.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =========================================
            // Controllers
            // =========================================

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            // =========================================
            // Database
            // =========================================

            builder.Services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlServer(
                        builder.Configuration
                            .GetConnectionString("DbConn")));

            builder.Services.Configure<EmailSettings>(
            builder.Configuration
                .GetSection("EmailSettings"));

            builder.Services.Configure<WeeklyReportSettings>(
            builder.Configuration
                .GetSection("WeeklyReportSettings"));

            builder.Services.AddHangfire(configuration =>
                configuration.UseSqlServerStorage(
                    builder.Configuration.GetConnectionString("DbConn"),
                    new SqlServerStorageOptions
                    {
                        PrepareSchemaIfNecessary = true
                    }));

            builder.Services.AddHangfireServer();

            // =========================================
            // AutoMapper
            // =========================================

            builder.Services.AddAutoMapper(options =>
            {
                options.Internal().ForAllMaps((_, mapExpr) =>
                {
                    mapExpr.MaxDepth(64);
                });
            });

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                cfg.Internal().ForAllMaps((_, mapExpr) =>
                {
                    mapExpr.MaxDepth(64);
                });
            });

            // =========================================
            // FluentValidation
            // =========================================
            builder.Services.AddValidatorsFromAssemblyContaining<
                CreateCatalogDtoValidator>();

            // =========================================
            // JWT Settings
            // =========================================

            builder.Services.Configure<JwtSettings>(
                builder.Configuration
                    .GetSection("JwtSettings"));

            // =========================================
            // Authentication
            // =========================================

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var jwtSettings =
                        builder.Configuration
                            .GetSection("JwtSettings")
                            .Get<JwtSettings>();

                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,

                            ValidateAudience = true,

                            ValidateLifetime = true,

                            ValidateIssuerSigningKey = true,

                            ValidIssuer =
                                jwtSettings!.Issuer,

                            ValidAudience =
                                jwtSettings.Audience,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        jwtSettings.Key))
                        };
                });

            // =========================================
            // Authorization
            // =========================================

            builder.Services.AddAuthorization();

            // =========================================
            // CORS
            // =========================================
            //in development

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });

            //in Prodoction

            //var frontendUrl =
            //builder.Configuration
            //    ["FrontendSettings:BaseUrl"];

            //        builder.Services.AddCors(options =>
            //        {
            //            options.AddPolicy("AllowFrontend",
            //                policy =>
            //                {
            //                    policy
            //                        .WithOrigins(frontendUrl!)
            //                        .AllowAnyHeader()
            //                        .AllowAnyMethod();
            //                });
            //        });




            // =========================================
            // Repository Registration
            // =========================================

            builder.Services.AddScoped<
                ICatalogRepository,
                CatalogRepository>();

            builder.Services.AddScoped<
                ILeadRepository,
                LeadRepository>();

            builder.Services.AddScoped<
                IContactMessageRepository,
                ContactMessageRepository>();

            builder.Services.AddScoped<
                IAdminRepository,
                AdminRepository>();
            builder.Services.AddScoped<
                IGeolocationService,
                GeolocationService>();

            builder.Services.AddScoped<
                IOutsideEgyptVisitorRepository,
                OutsideEgyptVisitorRepository>();

            builder.Services.AddScoped<
                IPartnerRepository,
                PartnerRepository>();

            // =========================================
            // Service Registration
            // =========================================

            builder.Services.AddScoped<
                IJwtService,
                JwtService>();

            builder.Services.AddScoped<
                ICsvExportService,
                CsvExportService>();

            builder.Services.AddScoped<
                IAuthService,
                AuthService>();

            builder.Services.AddScoped<
                ICatalogService,
                CatalogService>();

            builder.Services.AddScoped<
                ILeadService,
                LeadService>();

            builder.Services.AddScoped<
                IEmailService,
                EmailService>();

            builder.Services.AddScoped<
                IContactService,
                ContactService>();

            builder.Services.AddScoped<
                IOutsideEgyptVisitorService,
                OutsideEgyptVisitorService>();

            builder.Services.AddScoped<
                IPartnerService,
                PartnerService>();


            // =========================================
            // Swagger
            // =========================================

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter JWT Token"
                    });

                var securityRequirement = new OpenApiSecurityRequirement();
                securityRequirement.Add(
                    new OpenApiSecuritySchemeReference("Bearer"),
                    new List<string>()
                );
                options.AddSecurityRequirement(doc => securityRequirement);
            });


            //Rate limiter
            builder.Services.AddRateLimiter(options =>
            {
                // =========================================
                // Login Limiter
                // =========================================

                options.AddFixedWindowLimiter(
                    "LoginPolicy",
                    limiterOptions =>
                    {
                        limiterOptions.PermitLimit = 5;

                        limiterOptions.Window =
                            TimeSpan.FromMinutes(1);

                        limiterOptions.QueueProcessingOrder =
                            QueueProcessingOrder
                                .OldestFirst;

                        limiterOptions.QueueLimit = 0;
                    });

                // =========================================
                // Contact Form Limiter
                // =========================================

                options.AddFixedWindowLimiter(
                    "ContactPolicy",
                    limiterOptions =>
                    {
                        limiterOptions.PermitLimit = 10;

                        limiterOptions.Window =
                            TimeSpan.FromMinutes(1);

                        limiterOptions.QueueProcessingOrder =
                            QueueProcessingOrder
                                .OldestFirst;

                        limiterOptions.QueueLimit = 0;
                    });

                // =========================================
                // Lead Limiter
                // =========================================

                options.AddFixedWindowLimiter(
                    "LeadPolicy",
                    limiterOptions =>
                    {
                        limiterOptions.PermitLimit = 15;

                        limiterOptions.Window =
                            TimeSpan.FromMinutes(1);

                        limiterOptions.QueueProcessingOrder =
                            QueueProcessingOrder
                                .OldestFirst;

                        limiterOptions.QueueLimit = 0;
                    });

                // =========================================
                // Rejected Requests
                // =========================================

                options.RejectionStatusCode = 429;
            });

            builder.Services.Configure<ApiBehaviorOptions>(
                options =>
                {
                    options.InvalidModelStateResponseFactory =
                        context =>
                        {
                            var errors =
                                context.ModelState
                                    .Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage)
                                    .ToList();

                            var response =
                                new ValidationErrorResponse(
                                    errors);

                            return new BadRequestObjectResult(
                                response);
                        };
                });

            var app = builder.Build();

            var weeklyReportSettings =
                app.Services
                    .GetRequiredService<IOptions<WeeklyReportSettings>>()
                    .Value;

            var reportTimeZone =
                WeeklyReportsJob.ResolveTimeZone(
                    weeklyReportSettings.TimeZoneId);

            var weeklyCron =
                Cron.Weekly(
                    weeklyReportSettings.RunDayOfWeek,
                    weeklyReportSettings.RunTime.Hours,
                    weeklyReportSettings.RunTime.Minutes);

            RecurringJob.AddOrUpdate<WeeklyReportsJob>(
                "weekly-reports",
                job => job.SendReportsAsync(),
                weeklyCron,
                reportTimeZone);

            //Exception handling middleware
            //app.UseMiddleware<ExceptionMiddleware>();


            // =========================================
            // Middleware Pipeline
            // =========================================

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("AllowFrontend");

            app.UseRateLimiter();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context =
                    services.GetRequiredService<
                        ApplicationDbContext>();

                await AdminSeeder.SeedAdminAsync(
                    context);
            }

            app.Run();
        }
    }
}