using AlifTestTask.DB;
using AlifTestTask.Models;
using AlifTestTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlifTestTask.Controllers
{
    [Authorize]
    [ApiController]    
    [Route("api/[controller]")]
    public class ApiController : Controller
    {
        private AlifDB _db;
        private UsersService _usersService;
        private TransactionsService _transactionsService;
        private AccountsService _accountsService;
        private readonly ILogger<ApiController> _log;
        public ApiController(AlifDB db,UsersService usersService,TransactionsService transactionsService,AccountsService accountsService, ILogger<ApiController> log) 
        {
            _db= db;
            _usersService= usersService;
            _transactionsService= transactionsService;
            _accountsService= accountsService;
            _log = log;
        }
        [AllowAnonymous]
        [HttpPost("add_user")]
        public async Task<ActionResult> AddUser(UserWithAccount data)
        {
            if (data == null) 
            {
                _log.LogDebug("invalid userDto");
                return BadRequest("invalid userDto"); 
            }
            try
            {
               var res = await _usersService.CreateUser(data);
                return Ok(res);
            }catch (Exception ex) 
            {
                _log.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get_user")]
        //[Authorize(AuthenticationSchemes = "Digest")]
        public async Task<ActionResult> GetUser(string phone)
        {
            var user =  await _usersService.GetUser(phone);
            if (user == null) return BadRequest("User not Found");
            return Ok(user);
        }

        [HttpPost("credit")]
        public async Task<ActionResult> Credit(string phone, decimal amount)
        {
            try
            {
                var acc = await _accountsService.GetAccountByUserPhone(phone);
                var trn = await _transactionsService.SendPayment(acc.Id, amount, (int)Enums.TransactionTypes.Credit);
                return Ok(((Enums.TransactionerrorTypes)trn.Status).GetDescription());
            }
            catch (Exception ex)
            {
                return BadRequest(Enums.TransactionerrorTypes.CreditError.GetDescription());
            }

        }

        [HttpPost("get_balance")]
        public async Task<ActionResult> GetBalanceInfo(string phone)
        {
            try
            {
                var balance = await _accountsService.GetBalance(phone);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return BadRequest("Balance not found.");
            }

        }

        [HttpPost("get_tran_info")]
        public async Task<ActionResult> GetTransactionInfo(string phone)
        {
            try
            {
                var acc = await _accountsService.GetAccountByUserPhone(phone);
                var trnInfo = await _transactionsService.GetTranInfoByAccountId(acc.Id);
                return Ok(trnInfo);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                return BadRequest("Error on Get Info.");
            }

        }
    }
}
