namespace Entities.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string? Surname { get; private set; }
        public string? Name { get; private set; }
        public string? Patronymic { get; private set; }
        public string Email { get; private set; }
        public bool IsEmailChecked { get; private set; }
        public string Role { get; private set; }

        public User(string name, string surname, string patronymic, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Email = email;
            IsEmailChecked = false;
            Role = "user";
        }
        
        public User(string email)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Email = email;
            IsEmailChecked = false;
            Role = "user";
        }
        
        public User(string email, bool isAdmin)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Email = email;
            IsEmailChecked = isAdmin;
            Role = isAdmin ? "admin" : "user";
        }

        public void CheckEmail()
        {
            IsEmailChecked = true;
        }

        public void ChangeName(string surname, string name, string patronymic)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
        }
    }
}
