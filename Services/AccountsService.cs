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
        public AccountsService(AlifDB db, TransactionsService trnSvc, UsersService usersService)
        {
            _db = db;
            _trnSvc = trnSvc;
            _usrSvc = usersService;
        }

        public async Task<Accounts?> GetAccountByUserPhone(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone)) return null;
                var usr =  _db.Users.FirstOrDefaultAsync(u=>u.Phone.Equals(phone)).Result;
                var acc =  _db.Accounts.FirstOrDefaultAsync(a=>a.UserId == usr.Id).Result;
                return acc;
            }catch (Exception ex) 
            {
                return null;
            }
           
        }

        public async Task<BalanceInfo?> GetBalance(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone)) return null;

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
                return null;
            }

        }
    }
}
