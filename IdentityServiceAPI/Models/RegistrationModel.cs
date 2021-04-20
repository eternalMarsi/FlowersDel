using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IdentityServiceAPI.Models
{
    public class RegistrationModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string PasswordString { get; set; }
        [Required]
        [MinLength(3)]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [AllowNull]
        public string Token { get; set; }
    }
}