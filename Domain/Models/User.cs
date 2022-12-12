namespace Domain.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string? Surname { get; private set; }
        public string? Name { get; private set; }
        public string? Patronymic { get; private set; }
        public string Email { get; private set; }
        public bool IsEmailChecked { get; private set; }
        public Role Role { get; private set; }

        public User(string name, string surname, string patronymic, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Email = email;
            IsEmailChecked = false;
            Role = Role.User;
        }
        
        public User(string email)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Email = email;
            IsEmailChecked = false;
            Role = Role.User;
        }
        
        public User(string email, bool isAdmin)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Email = email;
            IsEmailChecked = isAdmin;
            Role = isAdmin ? Role.Admin : Role.User;
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
