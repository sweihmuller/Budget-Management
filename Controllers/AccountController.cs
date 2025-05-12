using Budget_Management.Models;
using Budget_Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget_Management.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountTypeRepository _accountTypeRepository;
        private readonly IUserServices _userServices;
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountTypeRepository accountTypeRepository, 
                                 IUserServices userServices,
                                 IAccountRepository accountRepository)
        {
            _accountTypeRepository = accountTypeRepository;
            _userServices = userServices;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userServices.RetrieveUserId();
            var accountWithAccountType = await _accountRepository.Search(userId);
            var model = accountWithAccountType
                .GroupBy(x => x.AccountType)
                .Select(group => new IndexAccountViewModel
                {
                    AccountType = group.Key,
                    Accounts = group.AsEnumerable()
                }).ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = _userServices.RetrieveUserId();
            var accountType = await _accountTypeRepository.Retrieve(userId);
            var model = new CreationAccountViewModel
            {
                AccountType = accountType.Select(x => new SelectListItem(x.name, x.Id.ToString()))
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreationAccountViewModel account)
        {
            var userId = _userServices.RetrieveUserId();
            var accountType = await _accountTypeRepository.Retrieve(userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _accountRepository.Create(account);
             return RedirectToAction("Index");
        }
    }
}
