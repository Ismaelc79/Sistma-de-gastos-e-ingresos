using Domain.ValueObjects;

namespace Domain.Entities
{
    public class User
    {
        public Ulid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public Password PasswordHash { get; private set; } = null!;
        public PhoneNumber? Phone { get; private set; }
        public Currency Currency { get; private set; } = null!;
        public string Language { get; private set; } = null!;
        public string? Avatar { get; private set; }
        public bool IsVerified { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Relaciones
        public List<Category> Categories { get; private set; } = new();
        public List<Transaction> Transactions { get; private set; } = new();
        public List<UserVerification> Verifications { get; private set; } = new();
        public List<RefreshToken> RefreshTokens { get; private set; } = new();

        public User() { }

        public User(Ulid id, string name, Email email, Password password,
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

        public void Verify()
        {
            IsVerified = true;
            UpdatedAt = DateTime.Now;
        }

        public void ChangePassword(Password password)
        {
            if (password is null) throw new ArgumentNullException(nameof(password));

            PasswordHash = password;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
