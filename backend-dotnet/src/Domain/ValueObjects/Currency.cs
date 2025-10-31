using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Currency
    {
        private static readonly Regex _regex = new(@"^[A-Z]{2,3}$", RegexOptions.Compiled);
        public string Code { get; }

        public Currency(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("El código de moneda es obligatorio.");

            code = code.ToUpperInvariant();

            if (!_regex.IsMatch(code))
                throw new ArgumentException("El código de moneda debe tener 2 o 3 letras en mayúscula (ej: USD, DOP, EUR).");

            Code = code;
        }

        public override string ToString() => Code;

        public override bool Equals(object? obj)
            => obj is Currency other && Code == other.Code;

        public override int GetHashCode() => Code.GetHashCode();
    }
}
