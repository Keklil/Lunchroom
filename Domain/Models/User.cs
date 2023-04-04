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

        private User(string name, string surname, string patronymic, string email, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Email = email;
            IsEmailChecked = false;
            Role = Role.Customer;
            Password = password;
        }

        private User(string email, string password, Role role)
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Surname = string.Empty;
            Patronymic = string.Empty;
            Email = email;
            IsEmailChecked = false;
            Role = role;
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

        public static User CreateCustomer(string email, string password)
        {
            return new User(email, password, Role.Customer);
        }
        
        public static User CreateKitchenOperator(string email, string password)
        {
            return new User(email, password, Role.KitchenOperator);
        }
        public static User CreateAdmin(string email, string password)
        {
            return new User(email, password, Role.Admin);
        }
    }
}
