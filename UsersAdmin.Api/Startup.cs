using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using UsersAdmin.Data;
using UsersAdmin.Data.Repositories;
using UsersAdmin.Core.Services;
using UsersAdmin.Services;
using UsersAdmin.Core.Repositories;
using AutoMapper;
using Microsoft.OpenApi.Models;
using UsersAdmin.Api.ExtensionMethods;
using UsersAdmin.Api.Config;
using System;
using System.Linq;
using FluentValidation;
using System.Reflection;
using FluentValidation.AspNetCore;

namespace UsersAdmin.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public Assembly CoreAssembly
        {
            get => AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("UsersAdmin.Core"))
                .FirstOrDefault();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SystemInfoConfig>(Configuration.GetSection("SystemInfo"));

            services.AddDbContext<AuthDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("AuthDb"), x => x.ServerVersion("8.0.19-mysql"))
            );
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(this.CoreAssembly);

            services.AddTransient<IAppCache, AppCache>();
            services.AddTransient<ISystemRepository, SystemRepository>();
            services.AddTransient<ISystemService, SystemService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            services.AddControllers(options =>
            {
                options.Filters.Add<Filters.ModelValidationActionFilter>(1);
                options.Filters.Add<Filters.AnswerExceptionActionFilter>(3);
            })
            .AddFluentValidation(options =>
                options.RegisterValidatorsFromAssembly(this.CoreAssembly)
            )
            .ConfigureApiBehaviorOptions(options =>
                options.SuppressModelStateInvalidFilter = true
            )
            .AddNewtonsoftJson();

            services.AddMemoryCache();
            services.AddStackExchangeRedisCache(options =>
                options.Configuration = "localhost:6379"
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "UsersAdminApi",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsersAdminApi V1")
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseAddHeaderInfo();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
