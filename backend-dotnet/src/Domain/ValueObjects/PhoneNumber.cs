using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class PhoneNumber
    {
        private static readonly Regex _regex = new(@"^\+?[0-9]{8,15}$", RegexOptions.Compiled);
        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El número de teléfono es obligatorio.");

            value = value.Replace(" ", "").Replace("-", "");

            if (!_regex.IsMatch(value))
                throw new ArgumentException("El formato del número de teléfono no es válido.");

            Value = value;
        }

        public override string ToString() => Value;

        // Igualdad estructural (para que Dapper, EF o HashSet lo manejen correctamente)
        public override bool Equals(object? obj)
            => obj is PhoneNumber other && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }
}
}
