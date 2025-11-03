using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.TypeHandlers
{
    public class UlidTypeHandler : SqlMapper.TypeHandler<Ulid>
    {
        public override void SetValue(IDbDataParameter parameter, Ulid value)
        {
            parameter.Value = value.ToString();
        }

        public override Ulid Parse(object value)
        {
            return Ulid.Parse(value.ToString()!);
        }
    }
}
