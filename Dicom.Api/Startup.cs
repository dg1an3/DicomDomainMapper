using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper.EquivalencyExpression;
using DomainModel = Elektrum.Capability.Dicom.Domain.Model;
using Elektrum.Capability.Dicom.Application.Repositories;
using Elektrum.Capability.Dicom.Application.Services;

namespace Elektrum.Capability.Dicom.Api
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
            services.AddDbContext<Infrastructure.EFModel.DicomDbContext>(options =>
            {
                options.UseSqlServer(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=MyStoreDB;");
            },
            ServiceLifetime.Scoped);

            services.AddAutoMapper(cfg => 
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile<Mappers.AbstractionMapperProfile>();
                cfg.AddProfile<Infrastructure.Mappers.DomainMapperProfile>();
            });

            services.AddScoped<IAggregateRepository<DomainModel.DicomSeries, DomainModel.DicomUid>,
                Infrastructure.Repositories.DicomSeriesRepository>();
            services.AddScoped<Application.Helpers.IDicomParser, Application.Helpers.DicomParser>();
            services.AddScoped<Application.Messaging.IMessaging, Messaging.Messaging>();
            services.AddScoped<IDicomApplicationService, DicomApplicationService>();

            services.AddControllers();

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
