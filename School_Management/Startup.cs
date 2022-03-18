using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using School_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School_Management.BL;

namespace School_Management
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "School_Management", Version = "v1" });
            });

            services.AddDbContext<SchoolContext>(options =>
            {
                string connectionString = Configuration.GetConnectionString("Database");
                options.UseMySQL(connectionString);
            });

            services.AddCors(o => o.AddPolicy("TCors", builder =>
            {
                builder.AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin();
            }));

            services.AddControllers()
                .AddJsonOptions(
                x =>
                {
                    x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    x.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            AddInjection(ref services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "School_Management v1"));
            }

            app.UseCors("TCors");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddInjection(ref IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseBL<>), typeof(BaseBL<>));
            services.AddTransient<IStudentBL, StudentBL>();
            services.AddTransient<IParentBL, ParentBL>();
            services.AddTransient<ITeacherBL, TeacherBL>();
        }
    }
}
