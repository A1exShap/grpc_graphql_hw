using Otus.Teaching.PromoCodeFactory.DataAccess;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.GraphQL.Customers;
using Otus.Teaching.PromoCodeFactory.GraphQL.DataLoaders;
using HotChocolate.AspNetCore;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.GraphQL.Common;

namespace Otus.Teaching.PromoCodeFactory.GraphQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(o =>
                    o.AddDefaultPolicy(b =>
                        b.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()))

            .AddPooledDbContextFactory<DataContext>((s, o) =>
                o.UseNpgsql(builder.Configuration.GetConnectionString("PromoCodeFactoryDb"))
            )

            .AddScoped<IDbInitializer, DbInitializer>()

            .AddGraphQLServer()

            .AddQueryType()
            .AddMutationType()
            .AddSubscriptionType()

            .AddTypeExtension<CustomerQueries>()
            .AddTypeExtension<CustomerMutations>()
            .AddTypeExtension<CustomerSubsciptions>()
            .AddTypeExtension<CustomerNode>()
            .AddDataLoader<CustomerByIdDataLoader>()
            .AddDataLoader<PreferenceByCustomerIdDataLoader>()

            .AddFiltering()
            .AddSorting()
            .AddGlobalObjectIdentification()

            .AddFileSystemQueryStorage("./persisted_queries")
            .UsePersistedQueryPipeline();

            builder.Services.AddAuthorization();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.UseCors();

            app.UseWebSockets();
            app.UseRouting();

            app.MapGraphQL().WithOptions(new GraphQLServerOptions
            {
                Tool =
                {
                    GaTrackingId = "G-2Y04SFDV8F"
                }
            });

            app.MapGet("/", context =>
            {
                context.Response.Redirect("/graphql", true);
                return Task.CompletedTask;
            });

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.InitializeDb();
            }

            app.Run();
        }
    }
}
