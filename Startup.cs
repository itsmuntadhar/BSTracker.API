using BSTracker.Data;
using BSTracker.Interfaces;
using BSTracker.Repositories;
using BSTracker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;

namespace BSTracker
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddSingleton(Configuration);

            ConfigureDbContext(services);
            AddRepositories(services);
            AddServices(services);

            services.AddControllers();
            services.AddSwaggerGen(AddSwaggerGen);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbContext context)
        {
            app.UseCors("AllowAnyOrigin");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(SetSwaggerUI);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            context.Database.Migrate();
        }

        private void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<SQLiteDbContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseSqlite(Configuration["ConnectionString"]);
            });
            services.AddScoped<IDbContext, SQLiteDbContext>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<BullshitsRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IBullshitService, BullshitService>(x => new BullshitService(x.GetRequiredService<BullshitsRepository>(), false));
        }

        private static void AddSwaggerGen(SwaggerGenOptions options)
        {
            SwaggerAddDoc(options);
            SwaggerAddSecurityDefinitions(options);
        }

        private static void SwaggerAddDoc(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "BullshitTracker.API", Version = "v1" });
        }

        private static void SwaggerAddSecurityDefinitions(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Auth Token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
        }

        private void SetSwaggerUI(SwaggerUIOptions options)
        {
            options.DefaultModelsExpandDepth(-1);
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bullshit Tracker v1");
        }
    }
}
