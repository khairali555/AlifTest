using AlifTestTask.DB;

namespace AlifTestTask.Services
{
    public class AuthService
    {
        AlifDB _db;
        private readonly ILogger<AuthService> _log;
        public AuthService(AlifDB db, ILogger<AuthService> log)
        {
            _db = db;
            _log = log;
        }
        public bool CheckUser(string phone, string hash)
        {
           
                var user = _db.Users.FirstOrDefault(x => x.Phone == phone);

                var userhash = Helper.CreateSHA1Hash(user.Phone+user.Password);
                if (userhash == hash)
                {
                    _log.LogDebug($"User {user.Phone} is checked.");
                    return true;
                }
                _log.LogDebug($"User {user.Phone} ended with error.");
                return false;
            
        }
    }
}
