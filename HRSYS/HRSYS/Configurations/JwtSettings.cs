 namespace HRSYS.API.Configurations {
    public class JwtSettings {
        public string Key { get;  init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiresMinutes { get; init; }
    }
}
