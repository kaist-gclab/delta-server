using System.Linq;
using System.Reflection;
using System.Text;
using Delta.AppServer.Core.Security;
using Delta.AppServer.ObjectStorage;
using Delta.AppServer.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NodaTime;

namespace Delta.AppServer.Startup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])),
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Issuer"]
                    };
                });
            services.AddCors();
            services.AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(context =>
                        {
                            var json = (from c in context.User.Claims
                                        where c.Type == "authInfo"
                                        select c.Value).FirstOrDefault();

                            if (json == null)
                            {
                                return false;
                            }

                            try
                            {
                                var authInfo = JsonConvert.DeserializeObject<AuthInfo>(json);
                                return authInfo.Role == "Admin";
                            }
                            catch
                            {
                                return false;
                            }
                        }).Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddJsonOptions(options => { options.SerializerSettings.ConfigureJsonSerializerSettings(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApplicationPart(Assembly.GetAssembly(typeof(Startup)));
            services.AddScoped<EncryptionService>();
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddLogging(builder => builder.AddConsole());
            services.AddDbContext<DeltaContext>(options =>
            {
                options.UseLazyLoadingProxies();
                ConfigureDbContext(options);
            });
            
            services.AddScoped<AuthConfig>();
            services.AddScoped<ObjectStorageConfig>();

            services.AddScoped<AuthService>();
            services.AddScoped<TokenService>();
            services.AddScoped<IObjectStorageService, S3CompatibleObjectStorageService>();

            services.AddSingleton(DateTimeZoneProviders.Tzdb[_configuration["Time:DateTimeZone"]]);
        }

        protected virtual void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(
                _configuration.GetConnectionString("DeltaDatabase"),
                o => o.UseNodaTime());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            
            app.UseMvc();
        }
    }
}