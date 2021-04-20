namespace IdentityServiceAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] PasswordBytes { get; set; }
        public byte[] LocalHashBytes { get; set; }

        public Role Role { get; set; }
        public int RoleId { get; set; }

    }
}
