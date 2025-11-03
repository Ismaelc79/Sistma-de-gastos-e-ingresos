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
    public class EmailTypeHandler : SqlMapper.TypeHandler<Email>
    {
        public override void SetValue(IDbDataParameter parameter, Email? email)
        {
            if (email is null) throw new InvalidOperationException("El email no puede ser null");
            parameter.Value = email.Value;
        }

        public override Email Parse(object value)
        {
            return new Email(value.ToString()!);
        }
    }
}
