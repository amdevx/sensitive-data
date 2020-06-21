using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sensitiveAPI.Configuration;
using sensitiveAPI.Data;
using sensitiveAPI.Repository;

namespace sensitiveAPI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            HostingEnvironment = env;
            var envName = env.EnvironmentName.ToLowerInvariant();
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sensitiveConfig = new SensitiveDataConfiguration(Configuration);
            services.AddSingleton<IDatabaseConfig>(c => c.GetRequiredService<SensitiveDataConfiguration>());
            services.AddControllers();
            services.AddDbContext<SensitiveContext>(x => x.UseSqlServer(sensitiveConfig.DatabaseConnectionString));
            services.AddScoped<ISensitiveRepository, SensitiveRepository>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}