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
        public string Password { get; private set; }
        public IReadOnlyCollection<Group> Groups => _groups;
        private List<Group> _groups = new();

        public User(string name, string surname, string patronymic, string email, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Email = email;
            IsEmailChecked = false;
            Role = Role.User;
            Password = password;
        }
        
        public User(string email, string password)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Email = email;
            IsEmailChecked = false;
            Role = Role.User;
            Password = password;
        }
        
        public User(string email, string password, bool isAdmin)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Email = email;
            IsEmailChecked = false;
            Role = isAdmin ? Role.Admin : Role.User;
            Password = password;
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

        // public void AddInGroup(Group group)
        // {
        //     _groups.Add(group);
        // }
    }
}
