namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Type { get; private set; } = null!;
        public Ulid UserId { get; private set; }
        public User? User { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Category() { }

        public Category(string name, string type, Ulid userId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre obligatorio");

            Name = name;
            Type = type;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public void EditCategory(string name, string type)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                Type = type;
            }
        }
    }
}
