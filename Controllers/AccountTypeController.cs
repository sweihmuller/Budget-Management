using Budget_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budget_Management.Controllers
{
    public class AccountTypeController : Controller
    {
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
