using AutoMapper;
using Clinic.Helper;
using clinic_Core.Managers.Interfaces;
using Clinic_Core.Managers;
using Clinic_Core.Managers.Interfaces;
using Clinic_Core.Managers.Services;
using Clinic_Core.Mapper;
using Clinic_DbModel.Models;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Clinic
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _mapperConfiguration = new MapperConfiguration(a => {
                a.AddProfile(new Mapping());
            });
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<clinic_dbContext>(op =>
                op.UseMySQL(Configuration.GetConnectionString("Default"))
                );
          
            services.AddOptions();
            services.Configure<JWT>(Configuration.GetSection("JWTOken"));
            services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddDefaultTokenProviders()
             .AddEntityFrameworkStores<clinic_dbContext>();
            services.AddSignalR();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetValue<string>("JWTOken:Issuer"),
                    ValidAudience = Configuration.GetValue<string>("JWTOken:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWTOken:Key")))
                };
            });
         
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            ); 
            services.AddHttpContextAccessor();
            services.AddSingleton(sp => _mapperConfiguration.CreateMapper());
            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            services.AddScoped<IPatientManager, PatientManager>();
            services.AddScoped<IAppointmentManager, AppointmentManager>();
            services.AddScoped<IDoctorManager, DoctorManager>();
            services.AddScoped<ISpecializationManager, SpecializationManager>(); 
            services.AddScoped<IScheduletimingManager, ScheduletimingManager>();
            services.AddScoped<IBlogManager, BlogManager>();
            services.AddScoped<ISocialMediaUrlManager, SocialMediaUrlManager>();
            services.AddScoped<IAdminManager, AdminManager>();
            services.AddScoped<IReviewManager, ReviewManager>();
            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CliniPlus , A Platform to link between Doctors & Patients", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
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
            });
       
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clinic v1"));
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
