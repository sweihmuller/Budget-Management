using Budget_Management.Models;
using Budget_Management.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly IAccountTypeRepository _accountTypeRepository;
        public AccountTypeController(IAccountTypeRepository accountTypeRepository)
        {
            _accountTypeRepository = accountTypeRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
           /* if (!ModelState.IsValid)
            {
                // Save the account type to the database}
                return View(accountType);
            }*/

            accountType.userId = 1;
           await _accountTypeRepository.Create(accountType);

            return View();
        }
    }
}
