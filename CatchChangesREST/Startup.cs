using DataModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CatchChangesREST
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CatchChangesREST", Version = "v1"});
            });

            var assemblies = new[] {typeof(Startup).Assembly};
            services.RegisterAll<IClient>(assemblies);
            services.RegisterAll<IDataSource>(assemblies);
            services.AddSingleton<SubscriptionService>();

            var host = _configuration["Host"] ?? "postgres";
            var port = _configuration["Port"] ?? "5432";
            var name = _configuration["DbName"] ?? "catch_changes_db";
            var user = _configuration["UserId"] ?? "admin";
            var password = _configuration["Password"] ?? "password";
            services.AddEntityFrameworkNpgsql().AddDbContext<SourceContext>(opt =>
                opt.UseNpgsql($"Host={host};Port={port};Database={name};User ID={user};Password={password}"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            SubscriptionService service,
            SourceContext sourceContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatchChangesREST v1"); });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            sourceContext.Database.Migrate();
        }
    }
}