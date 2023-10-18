using AlifTestTask.DB;
using FlakeyBit.DigestAuthentication.Implementation;
using Microsoft.EntityFrameworkCore;

namespace AlifTestTask.Services
{
    internal class UserAuthHash : IUsernameHashedSecretProvider
    {
        
        public async Task<string> GetA1Md5HashForUsernameAsync(string phone, string password)
        {
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password)) return await Task.FromResult<string>(null);

            try
            {
                using (AlifDB db = new AlifDB())
                {
                    var usr = await db.Users.FirstOrDefaultAsync(u=>u.Phone.Equals(phone));
                    if (usr != null && usr.Password == password)
                    {
                        string hash = Helper.CreateSHA1Hash(phone + password);

                        return await Task.FromResult(hash);
                    }
                    return await Task.FromResult<string>(null);
                }
                    
            }
            catch (Exception ex)
            {
                return await Task.FromResult<string>(null);
            }
        }        
    }
    
}
