namespace Airflights.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name {get; set;} = string.Empty;
        public UserRoles Role { get; set;} = UserRoles.User;

    }

    public class UserCreateModel
    {
        public string Name {get; set;} = string.Empty;
        public string Login {get; set;} = string.Empty;
        public string Password {get; set;} = string.Empty;
        public UserRoles Role { get; set;} = UserRoles.User;
    }


}