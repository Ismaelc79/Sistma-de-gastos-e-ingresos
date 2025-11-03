using Dapper;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.TypeHandlers
{
    public class PasswordTypeHandler : SqlMapper.TypeHandler<Password>
    {
        public override Password Parse(object value)
        {
            return Password.FromHash(value.ToString()!);
        }

        public override void SetValue(IDbDataParameter parameter, Password? value)
        {
            if (value is null) throw new InvalidOperationException("La clave no puede estar vacía");
            parameter.Value = value.Hash;
        }
    }
}
