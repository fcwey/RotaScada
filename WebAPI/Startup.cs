using Business.DependencyResolvers;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encyption;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using WebAPI.Hubs;
using WebAPI.Hubs.WebAPI.Hubs;
using WebAPI.Services;
using WebAPI.Services.WebAPI.Services;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // 1. SignalR Servisi
            services.AddSignalR();

            // 2. Arka Plan PLC Polling Servisi
            services.AddHostedService<PlcPollingBackgroundService>();


            // 3. CORS Ayarý (React / Vite / HMI arayüzleri için)
            services.AddCors(option =>
            {
                option.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("http://localhost:3000", "http://localhost:5173")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials()); // SignalR WebSocket el sýkýþmasý için þarttýr
            });

            // 4. JWT Yetkilendirme ve SignalR WebSocket Token Entegrasyonu
            var tokenOptions = Configuration.GetSection("TokenOption").Get<TokenOption>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };

                    // WebSocket üzerinden JWT Token okuma mekanizmasý
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            // Ýstenen adres SignalR Hub'ý ise token'ý sorgu parametresinden çek
                            if (!string.IsNullOrEmpty(accessToken) &&
                               (path.StartsWithSegments("/scadaHub") || path.StartsWithSegments("/alarmHub")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // 5. Core Baðýmlýlýklarý
            services.AddDependencyResolvers(new ICoreModule[]
            {
               new CoreModule()
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureCustomExceptionMiddleware();

            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // 6. Endpoint Yönlendirmeleri
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ScadaHub>("/scadaHub");   // Canlý Telemetri WebSocket Adresi
                endpoints.MapHub<AlarmHub>("/alarmHub");   // Anlýk Alarm WebSocket Adresi
            });
        }
    }
}