using Infrastructure.Persistence.Context;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Test
{
    public class DapperContextTests
    {
        [Fact]
        public void CreateConnection_WithValidConfig_ShouldReturnExpectedConnectionString()
        {
            // Arrange
            var connectionString = new Dictionary<string, string> {
                { "ConnectionStrings:GastosIngresosDb", "Server=localhost;Database=test;Trusted_Connection=true;" }
            };


            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(connectionString!).Build();

            // Act
            var dapperContext = new DapperContext(configuration);
            using var connection = dapperContext.CreateConnection();

            // Assert
            Assert.Contains("Server=localhost;Database=test;Trusted_Connection=true;", connection.ConnectionString);
            Assert.IsAssignableFrom<IDbConnection>(connection);
        }
    }
}