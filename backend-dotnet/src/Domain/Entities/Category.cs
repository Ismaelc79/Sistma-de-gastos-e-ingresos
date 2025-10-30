namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; } 
        public string UserId { get; private set; }
        public User User { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Category(string name, string type, string userId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre obligatorio");
       //     if (type != "Ingreso" && type != "Gastos") throw new ArgumentException("Tipo inválido");

            Name = name;
            Type = type;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
