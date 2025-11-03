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
    public class CurrencyTypeHandler : SqlMapper.TypeHandler<Currency>
    {
        public override void SetValue(IDbDataParameter parameter, Currency? value)
        {
            if (value is null) throw new InvalidOperationException("La moneda debe de estar definida");
            parameter.Value = value.Code;
        }

        public override Currency Parse(object value)
        {
            return new Currency(value.ToString()!);
        }
    }
}
