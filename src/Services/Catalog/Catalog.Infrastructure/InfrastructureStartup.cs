﻿
namespace eShop.Services.Catalog.Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration Configuration)
    {
       services.AddCustomDbContext(Configuration);

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();
        return services;
    }
    
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkSqlServer()
            .AddDbContext<CatalogContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString"],
                                        sqlServerOptionsAction: sqlOptions =>
                                        {
                                            sqlOptions.MigrationsAssembly(typeof(InfrastructureStartup).Assembly.GetName().Name);
                                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                        });
            });


        services.AddDbContext<IntegrationEventLogContext>(options =>
        {
            options.UseSqlServer(configuration["ConnectionString"],
                                    sqlServerOptionsAction: sqlOptions =>
                                    {
                                        sqlOptions.MigrationsAssembly(typeof(InfrastructureStartup).Assembly.GetName().Name);
                                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                    });
        });


        return services;
    }
}
