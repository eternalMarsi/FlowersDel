namespace IdentityServiceAPI
{
    public class TokenSettings
    {
        public string Issuer { get; set; }
        public bool VerifyIssuer { get; set; }
        public string Audience { get; set; }
        public bool VerifyAudience { get; set; }
        public int LifeTime { get; set; }
        public bool VerifyLifeTime { get; set; }
    }
}
