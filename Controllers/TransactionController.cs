using Budget_Management.Models;
using Budget_Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Budget_Management.Controllers
{
    public class TransactionController : Controller
    {
        private IUserServices _userServices;
        private IAccountRepository _accountRepository;
        private ICategoryRepository _categoryRepository;
        public TransactionController(IUserServices userServices, 
                                     IAccountRepository accountRepository, 
                                     ICategoryRepository categoryRepository)
        {
            _userServices = userServices;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Create()
        {
            var userId = _userServices.RetrieveUserId();
            var model = new TransactionCreationViewModel();
            model.Account = await GetAccount(userId);
            model.Category = await GetCategory(userId, model.OperationTypeId);
            return View(model);
        }

        private async Task<IEnumerable<SelectListItem>> GetAccount(int userId)
        {
            var accounts = await _accountRepository.Search(userId);
            return accounts.Select(a => new SelectListItem(a.Name, a.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetCategory(int userId, OperationType operationType)
        {
            var category = await _categoryRepository.GetAll(userId, operationType);
            return category.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        [HttpPost]
        public async Task<IActionResult> GetCategory([FromBody] OperationType operationType)
        {
            var userId = _userServices.RetrieveUserId();
            var category = await GetCategory(userId, operationType);
            return Ok(category); 
        }  
    }
}
