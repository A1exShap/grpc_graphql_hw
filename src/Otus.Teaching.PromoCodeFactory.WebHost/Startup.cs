using GrpcServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
//using NSwag.AspNetCore;
using Microsoft.OpenApi.Models;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Gateways;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using Otus.Teaching.PromoCodeFactory.Integration;
using System;
using System.IO;
using System.Reflection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

// References 
// gRPC
// https://gitlab.com/aa.gerasimenko/Otus.gRPC 
// https://github.com/grpc/grpc-dotnet/tree/master/examples
// https://learn.microsoft.com/ru-ru/aspnet/core/grpc/protobuf?view=aspnetcore-7.0 
// https://protobuf.dev/programming-guides/proto3/ 
// https://grpc.io/docs/guides/error/
// https://learn.microsoft.com/ru-ru/aspnet/core/grpc/json-transcoding-openapi?view=aspnetcore-7.0

namespace Otus.Teaching.PromoCodeFactory.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddMvcOptions(x=> 
                x.SuppressAsyncSuffixInActionNames = false);
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<INotificationGateway, NotificationGateway>();
            services.AddScoped<IDbInitializer, EfDbInitializer>();
            services.AddDbContext<DataContext>(x =>
            {
                x.UseSqlite("Filename=PromoCodeFactoryDb.sqlite");
                //x.UseNpgsql(Configuration.GetConnectionString("PromoCodeFactoryDb"));
                x.UseSnakeCaseNamingConvention();
                x.UseLazyLoadingProxies();
            });

            //services.AddOpenApiDocument(options =>
            //{
            //    options.Title = "PromoCode Factory API Doc";
            //    options.Version = "1.0";
            //});

            // gRPC
            services.AddGrpc();

            services.AddGrpcReflection();

            services.AddGrpcHttpApi();

            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PromoCode Factory API", Version = "v2" });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var filePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                c.IncludeXmlComments(filePath);
                c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
            });

            services.AddGrpcSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseOpenApi();
            //app.UseSwaggerUi3(x =>
            //{
            //    x.Path = "/nswag";
            //    x.DocExpansion = "list";
            //});

            //only serve .proto files
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Clear();
            provider.Mappings[".proto"] = "text/plain";

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Protos")),
                RequestPath = "/proto",
                ContentTypeProvider = provider
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Protos")),
                RequestPath = "/proto"
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PromoCode Factory API V2"));

            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGrpcService<CustomerService>();

                if (env.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }

                endpoints.MapGet("/",
                async context =>
                {
                    await context.Response.WriteAsync(
                "Communication with gRPC endpoints must be made through a gRPC client. " +
                "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

            dbInitializer.InitializeDb();
        }
    }
}