using System.Text;
using PrettyNeatGenericAPI.Data.Repos;
using PrettyNeatGenericAPI.Data.Seed;
using PrettyNeatGenericAPI.Handlers;
using PrettyNeatGenericAPI.Interfaces;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Providers;
using PrettyNeatGenericAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SendGrid.Extensions.DependencyInjection;

namespace PrettyNeatGenericAPI
{

    public class Program
    {
        public static void Main(string[] args)
        {

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "PrettyNeatGenericAPI", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            builder.Services.AddDbContext<PrettyNeatGenericAPIDbContext>(options =>
                            options.UseNpgsql(config.GetValue<string>("ConnectionString")));


            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<SharePointProvider>();

            //here add the repo which you'll create in the future
            builder.Services.AddScoped<PatientRepo>();
            builder.Services.AddScoped<UserRepo>();
            builder.Services.AddScoped<EmployeeRepo>();
            builder.Services.AddScoped<BagInventoryRepo>();
            builder.Services.AddScoped<DeliveryNoteRepo>();
            builder.Services.AddScoped<ReturnLogsRepo>();
            builder.Services.AddScoped<DeliveryNotePrintedRepo>();
            builder.Services.AddScoped<ReturnedBagsRepo>();
            builder.Services.AddScoped<QualifiersRepo>();
            builder.Services.AddScoped<CheckedBagsLogsRepo>();

            builder.Services.AddSendGrid(c =>
            {
                c.ApiKey = config.GetSection("sendGrid").GetValue<string>("apiKey");
            });
            builder.Services.AddScoped<IMailer, SendGridHandler>();

            // Include the service as a Singleton - ensure one copy only exists
            // so one (concurrent) run as well

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                };
            });

            var app = builder.Build();

            app.UseCors(builder =>
            {
                builder
                    .WithOrigins("http://localhost:5173", "https://127.0.0.1:5173", "http://127.0.0.1:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed((host) => true);
        }
);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.MapControllers();

            var nukeAndSeed = config.GetValue<bool>("NukeAndSeedDb"); //WARNING This will destroy and reconstruct the db on every run
            if (nukeAndSeed)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var service = scope.ServiceProvider;
                    var context = service.GetService<PrettyNeatGenericAPIDbContext>();
                    if (context!= null)
                    {
                        DataSeeder.Nuke(context);
                        DataSeeder.Seed(context);
                    }
                }
            }
            ////run the queue
            //using (var scope = app.Services.CreateScope())
            //{
            //    var service = scope.ServiceProvider;
            //    var context = service.GetService<PrettyNeatGenericAPIDbContext>();
            //    var mailer = service.GetService<IMailer>();
            //    var signalrProvider = service.GetService<SignalRProvider>();

            //    var pipelineRepo = service.GetService<PipelineRepo>();
            //    if (context != null)
            //    {

            //        Queues.PipelineRunQueue pipelineRunQueue = new Queues.PipelineRunQueue(context, config, pipelineRepo, mailer, signalrProvider);

            //        Task.Run(async () => await pipelineRunQueue.Run());
            //    }
            //}

            app.UseDeveloperExceptionPage();

            app.Run();


        }
    }
}