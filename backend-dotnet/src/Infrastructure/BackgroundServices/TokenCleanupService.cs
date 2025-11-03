using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    // Remove tokens that expired 7 days ago
                    await _unitOfWork.RefreshToken.DeleteExpiredTokensAsync();
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
