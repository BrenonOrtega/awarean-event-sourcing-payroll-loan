using System.Data;
using Awarean.EventSourcing.PayrollLoans.Api.Configurations;
using Awarean.EventSourcing.PayrollLoans.Api.Repositories;
using Awarean.EventSourcing.PayrollLoans.Api.Repositories.Abstractions;
using Awarean.EventSourcing.PayrollLoans.Api.Services;
using Awarean.EventSourcing.PayrollLoans.Api.Services.Abstractions;
using Npgsql;

namespace Awarean.EventSourcing.PayrollLoans.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventStore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEventStore, EventStore>();
        services.Configure<EventStoreOptions>(configuration.GetSection(nameof(EventStoreOptions)));
        services.AddConnectionFactory(configuration);

        return services;
    }

    public static IServiceCollection AddConnectionFactory(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IConnectionFactory, ConnectionFactory>();
        services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(configuration.GetConnectionString(nameof(Npgsql))));

        return services;
    }

    public static IServiceCollection AddSnapshotRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISnapshotRepository<,>), typeof(SnapshotRepository<,>));

        return services;
    }

    public static IServiceCollection AddPayrollLoanService(this IServiceCollection services)
    {
        services.AddScoped<IPayrollLoanService, PayrollLoanService>();

        return services;
    }

}
