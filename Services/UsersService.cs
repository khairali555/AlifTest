using AlifTestTask.DB;
using AlifTestTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlifTestTask.Services
{
    public class UsersService
    {
        AlifDB _db;
        private readonly ILogger<UsersService> _log;
        public UsersService(AlifDB db, ILogger<UsersService> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<Users> GetUser(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                _log.LogDebug("invalid phone number");
                return null;
            }
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Phone == phone);
            if (user == null) return null;
            return user;
        }

        public async Task<UserWithAccount> CreateUser(UserWithAccount data)
        {
            try
            {
                if (data == null)                
                {
                    _log.LogDebug("invalid userDto");
                    return null;
                }

                var usr = new Users();
                usr.Phone = data.Phone;
                usr.UserType = data.UserType;
                usr.Password = data.Password;
                var userResult = await _db.Users.AddAsync(usr);
                await _db.SaveChangesAsync();

                var acc = new Accounts();
                acc.UserId = userResult.Entity.Id;
                acc.Account = data.Account;
                acc.Balance = 0;
                acc.CreatedAt = DateTime.Now;                                
                await _db.Accounts.AddAsync(acc);

                await _db.SaveChangesAsync();

                return data;

            }catch (Exception ex) 
            {
                _log.LogError(ex.Message);
                return null;
            }
            
        }
    }
}
