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
        private readonly IUserServices _userServices;

        public AccountTypeController(IAccountTypeRepository accountTypeRepository,
                                     IUserServices userServices)
        {
            _accountTypeRepository = accountTypeRepository;
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userServices.RetrieveUserId();
            var accountType = await _accountTypeRepository.Retrieve(userId);
            return View(accountType);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userServices.RetrieveUserId();
            var accountType = await _accountTypeRepository.RetrieveById(id, userId);
            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(accountType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountType accountType)
        {
           /* if (!ModelState.IsValid)
            {
                return View(accountType);
            }*/
            var userId = _userServices.RetrieveUserId();
            var doesAlreadyExist = await _accountTypeRepository.RetrieveById(accountType.Id, userId);
            if (doesAlreadyExist is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await _accountTypeRepository.Update(accountType);
            return RedirectToActionPermanent("Index");
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

           accountType.userId = _userServices.RetrieveUserId();
           var doesAlreadyExist = await _accountTypeRepository.DoesExist(accountType.name, accountType.userId);
            if(doesAlreadyExist)
            {
                ModelState.AddModelError(nameof(accountType.name), $"El nombre {accountType.name} ya existe.");
                return View(accountType);
            }
            await _accountTypeRepository.Create(accountType);

            return RedirectToActionPermanent("Index");
        }
    }
}
