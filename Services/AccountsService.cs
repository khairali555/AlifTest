using AlifTestTask.DB;
using AlifTestTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlifTestTask.Services
{
    public class AccountsService
    {
        AlifDB _db;
        private TransactionsService _trnSvc;
        private UsersService _usrSvc;
        private readonly ILogger<AccountsService> _log;
        public AccountsService(AlifDB db, TransactionsService trnSvc, UsersService usersService, ILogger<AccountsService> log)
        {
            _db = db;
            _trnSvc = trnSvc;
            _usrSvc = usersService;
            _log = log;
        }

        public async Task<Accounts?> GetAccountByUserPhone(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    _log.LogDebug("invalid phone number");
                    return null;
                }                
                var usr =  _db.Users.FirstOrDefaultAsync(u=>u.Phone.Equals(phone)).Result;
                var acc =  _db.Accounts.FirstOrDefaultAsync(a=>a.UserId == usr.Id).Result;
                return acc;
            }catch (Exception ex) 
            {
                _log.LogError(ex.Message);
                return null;
            }
           
        }

        public async Task<BalanceInfo?> GetBalance(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    _log.LogDebug("invalid phone number");
                    return null;
                }

                return await (from acc in _db.Accounts
                              join usr in _db.Users
                              on acc.UserId equals usr.Id
                              where
                              acc.UserId == usr.Id
                              select new BalanceInfo
                              {
                                  Phone = usr.Phone,
                                  Balance = acc.Balance
                              }).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return null;
            }

        }
    }
}
