using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SportsSocialNetwork.DataAccessLayer;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Helpers;
using SportsSocialNetwork.Interfaces;
using SportsSocialNetwork.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SportsSocialNetwork
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataBaseContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataBaseContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                    ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });
            services.AddControllers();
            services.AddAutoMapper();
            services.AddMvc()
                //(o =>
                //{
                //    o.ModelMetadataDetailsProviders.Add(new CustonValidationMetadataProvider());
                //    o.EnableEndpointRouting = false;
                //})
                //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(x =>
                {
                    //x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    //x.SerializerSettings.Culture = new CultureInfo("ru-Ru");
                    //x.SerializerSettings.Culture.NumberFormat.NumberGroupSeparator = "";
                    x.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
                })
            //    .AddJsonOptions(opts =>
            //{
            //    opts.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            //})
            ;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "beta",
                    Title = "SportSocialNetwork",
                    Description = "API for sports social network",
                    TermsOfService = new Uri("https://github.com/Seeyouinthespring/MEMAnalyzer_Backend/tree/main/MEMAnalyzer_Backend"),
                    Contact = new OpenApiContact
                    {
                        Name = "Nick Zhuravlyov",
                        Email = "colya.juravlyov2011@ya.ru",
                        Url = new Uri("https://vk.com/id118971987"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://vk.com/id118971987"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
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
                        new string[] { }
                    }
                });
                

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            AddOwnServices(services);

            services.AddControllers();
        }

        private void AddOwnServices(IServiceCollection services)
        {
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISportsService, SportsService>();
            services.AddScoped<IPlaygroundService, PlaygroundService>();
            services.AddScoped<IVisitingsService, VisitingsService>();
            services.AddScoped<IAppointmentsService, AppointmentsService>();
            services.AddScoped<IRentService, RentService>();
            services.AddScoped<IMessagesService, MessagesService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IContactInformationService, ContactInformationService>();
            services.AddScoped<IPersonalActivityService, PersonalActivityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();;

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
