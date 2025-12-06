namespace WEB_MCV.Services
{
    public class JwtService  {
        private readonly IHttpContextAccessor _http;

        public JwtService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public void Save(string token)
        {
            _http.HttpContext ?.Session.SetString("jwt", token);
        }

        public string? Get()
        {
            return _http.HttpContext ? .Session.GetString("jwt");
        }

        public void Clear()
        {
            _http.HttpContext ? .Session.Remove("jwt");
        }
    }
}
