namespace SistemaGastos.Domain.Entities
{
    public class User
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Password PasswordHash { get; private set; }
        public PhoneNumber? Phone { get; private set; }
        public Currency Currency { get; private set; }
        public string Language { get; private set; }
        public string? Avatar { get; private set; }
        public bool IsVerified { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Relaciones
        public List<Category> Categories { get; private set; } = new();
        public List<Transaction> Transactions { get; private set; } = new();
        public List<UserVerification> Verifications { get; private set; } = new();
        public List<RefreshToken> RefreshTokens { get; private set; } = new();

        public User(string id, string name, Email email, Password password,
                    PhoneNumber? phone = null, Currency? currency = null,
                    string language = "English", string? avatar = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre obligatorio");

            Id = id;
            Name = name;
            Email = email;
            PasswordHash = password;
            Phone = phone;
            Currency = currency ?? new Currency("DO");
            Language = language;
            Avatar = avatar;
            IsVerified = false;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateProfile(string? name = null, PhoneNumber? phone = null,
                                  Currency? currency = null, string? language = null,
                                  string? avatar = null)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
            if (phone != null) Phone = phone;
            if (currency != null) Currency = currency;
            if (!string.IsNullOrWhiteSpace(language)) Language = language;
            if (!string.IsNullOrWhiteSpace(avatar)) Avatar = avatar;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Verify() => IsVerified = true;
    }

    public class Currency
    {
        private string v;

        public Currency(string v)
        {
            this.v = v;
        }
    }

    public class PhoneNumber
    {
    }

    public class Password
    {
    }

    public class Email
    {
    }
}
