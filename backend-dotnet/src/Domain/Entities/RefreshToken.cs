namespace Domain.Entities
{
    public class RefreshToken
    {
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public User User { get; private set; }
        public string JwtId { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool Revoked { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public RefreshToken(string id, string userId, string jwtId, DateTime expiresAt)
        {
            Id = id;
            UserId = userId;
            JwtId = jwtId;
            ExpiresAt = expiresAt;
            Revoked = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void Revoke() => Revoked = true;
    }
}
