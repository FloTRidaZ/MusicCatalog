namespace Client01.ru.kso.Database.Datatype
{
    public sealed class User
    {
        private static User _instance;

        public string Email { get; }
        
        public string Name { get; }

        public User(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public static void CreateInstance(string email, string name)
        {
            if (_instance != null)
            {
                return;
            }
            _instance = new User(email, name);
        }

        public static bool IsLogIn()
        {
            return _instance != null;
        }

        public static void ToLogOut()
        {
            _instance = null;
        }
    }
}
