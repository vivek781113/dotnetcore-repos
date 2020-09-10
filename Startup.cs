using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using WebAPI3_1.CustomMiddleware;
using WebAPI3_1.Models;
using WebAPI3_1.Services.Implementations;
using WebAPI3_1.Services.Interfaces;

namespace WebAPI3_1
{
    public class Startup
    {

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                    .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            services.Configure<JwtBearerOptions>(AzureADDefaults.JwtBearerAuthenticationScheme, options =>
            {
                options.Authority = $"{Configuration.GetValue<string>("AzureAd:Instance")}{Configuration.GetValue<string>("AzureAd:TenantId")}";
                options.Audience = Configuration.GetValue<string>("AzureAd:Audience");

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //ValidIssuer = Configuration.GetValue<string>("AzureAd:Issuer"),
                    ValidAudience = Configuration.GetValue<string>("AzureAd:Audience"),
                    ValidateIssuer = false,
                };
            });

            services.AddTransient<IOperationTransient, Operation>();
            services.AddScoped<IOperationScoped, Operation>();
            services.AddSingleton<IOperationSingleton, Operation>();

            services.AddDbContext<ShopContext>(options =>
                options.UseInMemoryDatabase("Shop")
            );

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:4200/", "https://ngazwebapp.azurewebsites.net/")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
            });

            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    })
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        //default behaviour -- if this set to tru, model validation wont take place
                        options.SuppressModelStateInvalidFilter = false;
                    });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseSerilogRequestLogging();

            /*Builtin & custom middleware for excption handling */
            app.ConfigureExceptionHandler();

            ///*Commented out if we are running under docker */
            //app.UseHttpsRedirection();

            app.UseRouting();

            /*
            Calls the UseCors extension method and specifies the _myAllowSpecificOrigins CORS policy. UseCors adds the CORS middleware. The call to UseCors must be placed after UseRouting, but before UseAuthorization. For more information, see Middleware order.
            */
            app.UseCors(MyAllowSpecificOrigins);


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
