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

        public AccountController(IAccountTypeRepository accountTypeRepository, IUserServices userServices)
        {
            _accountTypeRepository = accountTypeRepository;
            _userServices = userServices;
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
    }
}
