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
    public class PhoneNumberTypeHandler : SqlMapper.TypeHandler<PhoneNumber>
    {
        public override PhoneNumber? Parse(object value)
        {
            if (value is null || value is DBNull) return null;

            return new PhoneNumber(value.ToString()!);
        }

        public override void SetValue(IDbDataParameter parameter, PhoneNumber? value)
        {
            parameter.Value = (object?)value?.Value ?? DBNull.Value;
        }
    }
}
