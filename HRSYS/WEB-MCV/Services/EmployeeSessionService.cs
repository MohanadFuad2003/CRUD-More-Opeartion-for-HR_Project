namespace WEB_MCV.Services
{
    public class EmployeeSessionService
    {
        private readonly IHttpContextAccessor _http;
        private const string SessionKey = "employee_jwt";

        public EmployeeSessionService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public void Save(string token)
        {
            _http.HttpContext?.Session.SetString(SessionKey, token);
        }

        public string? Get()
        {
            return _http.HttpContext?.Session.GetString(SessionKey);
        }

        public void Clear()
        {
            _http.HttpContext?.Session.Remove(SessionKey);
        }
    }
}

