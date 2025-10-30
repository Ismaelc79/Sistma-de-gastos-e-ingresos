using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Context
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration) 
        {
            _connectionString = configuration.GetConnectionString("GastosIngresosDb")!;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
