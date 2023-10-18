using AlifTestTask.DB;
using AlifTestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace AlifTestTask.Services
{
    public class TransactionsService
    {
        AlifDB _db;
        public TransactionsService(AlifDB db) 
        { 
            _db = db;
        }

        public async Task<Transactions> SendPayment(int accountId,decimal amount,int tranType)
        {
            var tran = new Transactions();
            try
            {
                
                tran.AccountId = accountId;
                tran.Amount = amount;
                tran.TranType = tranType;
                tran.TranDate = DateTime.Now;

                var account = await _db.Accounts.FirstOrDefaultAsync(a=>a.Id == accountId);
                var user = await _db.Users.FirstOrDefaultAsync(u=>u.Id == account.UserId);
                
                if (user.UserType == (int)Enums.UserTypes.Identified && (account.Balance + amount) > 100000)
                {
                    tran.ErrorStatusCode = (int)Enums.TransactionerrorTypes.BalanceError;
                    tran.Error = Enums.TransactionerrorTypes.BalanceError.GetDescription();
                    tran.Status = (int)Enums.TransactionerrorTypes.CreditError;
                    return tran;
                }
                if (user.UserType == (int)Enums.UserTypes.UnnIdentified && (account.Balance + amount) > 10000)
                {
                    tran.ErrorStatusCode = (int)Enums.TransactionerrorTypes.BalanceError;
                    tran.Error = Enums.TransactionerrorTypes.BalanceError.GetDescription();
                    tran.Status = (int)Enums.TransactionerrorTypes.CreditError;
                    return tran;
                }

                account.Balance += amount;

                tran.ErrorStatusCode = (int)Enums.TransactionerrorTypes.Ok;
                tran.Error = Enums.TransactionerrorTypes.Ok.GetDescription();
                tran.Status = (int)Enums.TransactionerrorTypes.Ok;

                _db.Accounts.Update(account);
                await _db.Transactions.AddAsync(tran);
                await _db.SaveChangesAsync();
                return  tran;
            }catch (Exception ex) 
            {
                tran.ErrorStatusCode = (int)Enums.TransactionerrorTypes.BalanceError;
                tran.Error = Enums.TransactionerrorTypes.BalanceError.GetDescription();
                tran.Status = (int)Enums.TransactionerrorTypes.CreditError;
                return tran;
            }
            
        }

        public async Task<FullTranInfo> GetTranInfoByAccountId(int accountId)
        {
            try
            {                
                var tranInfo = await (from trn in _db.Transactions
                                 join acc in _db.Accounts
                                 on trn.AccountId equals acc.Id
                                 join usr in _db.Users
                                 on acc.UserId equals usr.Id 
                                 where trn.AccountId == accountId &&
                                 trn.TranDate.Month == DateTime.Now.Month
                                 select new TranInfo
                                 {
                                     Account = acc.Account,
                                     Amount = trn.Amount,
                                     Phone = usr.Phone,
                                     TranType = trn.TranType,
                                     Date = trn.TranDate
                                 }).ToListAsync();

                var Fulltraninfo = new FullTranInfo();
                Fulltraninfo.TranInfo = tranInfo;
                Fulltraninfo.Count = tranInfo.Count;

                return Fulltraninfo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
