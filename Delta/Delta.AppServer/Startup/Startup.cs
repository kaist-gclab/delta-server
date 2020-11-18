using System.Linq;
using System.Reflection;
using System.Text;
using Delta.AppServer.Assets;
using Delta.AppServer.Core.Schedule;
using Delta.AppServer.Core.Security;
using Delta.AppServer.Jobs;
using Delta.AppServer.ObjectStorage;
using Delta.AppServer.Processors;
using Delta.AppServer.Encryption;
using Delta.AppServer.Stats;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddResponseCompression();
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins("http://localhost:3000", "https://delta-test.cqcqcqde.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
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
                .AddNewtonsoftJson(options => { options.SerializerSettings.ConfigureJsonSerializerSettings(); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
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
            services.AddScoped<JobExecutionConfig>();

            services.AddScoped<AuthService>();
            services.AddScoped<TokenService>();
            services.AddScoped<JobService>();
            services.AddScoped<ProcessorService>();
            services.AddScoped<AssetService>();
            services.AddScoped<AssetMetadataService>();
            services.AddScoped<IObjectStorageService, S3CompatibleObjectStorageService>();
            services.AddScoped<CompressionService>();
            services.AddScoped<EncryptionService>();
            services.AddScoped<IObjectStorageKeyConverter, PrefixFourObjectStorageKeyConverter>();
            services.AddScoped<StatsService>();

            services.AddSingleton<ScheduleHelper>();
            services.AddSingleton(DateTimeZoneProviders.Tzdb[_configuration["Time:DateTimeZone"]]);
        }

        protected virtual void ConfigureDbContext(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(
                _configuration.GetConnectionString("DeltaDatabase"),
                o => o.UseNodaTime());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
