using System.Security.Cryptography;

namespace IdentityServiceAPI.Models.ServiceResults
{
    public class JoinResult
    {
        public User User { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }

        public JoinResult() { }
        public JoinResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
