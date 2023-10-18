using AlifTestTask.DB;

namespace AlifTestTask.Services
{
    public class AuthService
    {
        AlifDB _db;
        public AuthService(AlifDB db) 
        {
            _db = db;
        }
        public bool CheckUser(string phone, string hash)
        {
           
                var user = _db.Users.FirstOrDefault(x => x.Phone == phone);

                var userhash = Helper.CreateSHA1Hash(user.Phone+user.Password);
                if (userhash == hash) return true;
                return false;
            
        }
    }
}
