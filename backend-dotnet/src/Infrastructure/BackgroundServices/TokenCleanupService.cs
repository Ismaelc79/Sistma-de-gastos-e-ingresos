using Dapper;
using Infrastructure.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.BackgroundServices
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public TokenCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<DapperContext>();

                    using var connection = context.CreateConnection();

                    const string sql = @"
                        DELETE FROM [dbo].[RefreshToken]
                        WHERE ExpiresAt < DATEADD(day, -7, GETUTCDATE());
                    ";

                    await connection.ExecuteAsync(sql);

                    Console.WriteLine("[TokenCleanupService] ✔ Expired tokens removed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[TokenCleanupService] Error: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
