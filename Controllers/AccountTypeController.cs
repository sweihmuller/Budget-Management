using Budget_Management.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Budget_Management.Controllers
{
    public class AccountTypeController(IConfiguration configuration) : Controller
    {
        private readonly string connectionString = configuration.GetConnectionString("DefaultConnection");

        [HttpGet]
        public IActionResult Create()
        {
                return View();
        }

        [HttpPost]
        public IActionResult Create(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                // Save the account type to the database}
                return View(accountType);
            }
            return View();
        }
    }
}
