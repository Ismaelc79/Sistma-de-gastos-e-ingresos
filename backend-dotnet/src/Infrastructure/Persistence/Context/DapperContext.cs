using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.Persistence.TypeHandlers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Context
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration) 
        {
            SqlMapper.AddTypeHandler(new UlidTypeHandler());
            SqlMapper.AddTypeHandler(new EmailTypeHandler());
            SqlMapper.AddTypeHandler(new CurrencyTypeHandler());
            SqlMapper.AddTypeHandler(new PasswordTypeHandler());
            SqlMapper.AddTypeHandler(new PhoneNumberTypeHandler());
            _connectionString = configuration.GetConnectionString("GastosIngresosDb")!;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
