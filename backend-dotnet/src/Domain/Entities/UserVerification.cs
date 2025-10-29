namespace SistemaGastos.Domain.Entities
{
    public class UserVerification
    {
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public User User { get; private set; }
        public string Code { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool Used { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public UserVerification(string id, string userId, string code, DateTime? expiresAt = null)
        {
            Id = id;
            UserId = userId;
            Code = code;
            ExpiresAt = expiresAt ?? DateTime.UtcNow.AddMinutes(5);
            Used = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkUsed() => Used = true;
    }
}
