using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TestCoreWebApi.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Net.Http.Headers;

namespace TestCoreWebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
             services.AddCors();
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);



            var connection = @"Data Source=WSD001\SQLEXPRESS;Initial Catalog=Practice;User ID=sa;Password=Newuser@123;";
                //@"Data Source = 192.168.0.183; Initial Catalog = test; Integrated Security = True; Persist Security Info = True; User ID = sa; Password = ROOT#123;Trusted_Connection=True;MultipleActiveResultSets=true;";
            services.AddDbContext<Employee>(options => options.UseSqlServer(connection));


          //  var corsBuilder = new CorsPolicyBuilder();
          //  corsBuilder.AllowAnyHeader();
          //  corsBuilder.AllowAnyMethod();
          //  corsBuilder.AllowAnyOrigin();
          //  corsBuilder.AllowCredentials();

         services.ConfigureCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                        .AllowAnyMethod()
                                                                         .AllowAnyHeader().AllowAnyHeader().AllowCredentials());

            services.AddMvc(options=> 
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", new MediaTypeHeaderValue("application/json"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", new MediaTypeHeaderValue("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("txt", new MediaTypeHeaderValue("text/plain"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            

            app.UseMvcWithDefaultRoute();
            app.UseCors("AllowAll");
        }
    }
}
