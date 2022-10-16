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
        var eventStoreOptions = configuration.GetSection(nameof(EventStoreOptions));
        services.Configure<EventStoreOptions>(eventStoreOptions);
        services.AddScoped<IEventStore, EventStore>();

        return services;
    }

    public static IServiceCollection AddConnectionFactory(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(nameof(Npgsql));
        services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        services.AddScoped<IConnectionFactory, ConnectionFactory>();

        return services;
    }

    public static IServiceCollection AddSnapshotRepository(this IServiceCollection services, IConfiguration configuration)
    {
        var snapshot = configuration.GetSection(nameof(SnapshotOptions));
        services.Configure<SnapshotOptions>(snapshot);

        services.AddScoped(typeof(ISnapshotRepository<,>), typeof(SnapshotRepository<,>));

        return services;
    }

    public static IServiceCollection AddPayrollLoanService(this IServiceCollection services)
    {
        services.AddScoped<IPayrollLoanService, PayrollLoanService>();

        return services;
    }
}
