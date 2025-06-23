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
        private ITransactionRepository _transactionRepository;
        public TransactionController(IUserServices userServices, 
                                     IAccountRepository accountRepository, 
                                     ICategoryRepository categoryRepository,
                                     ITransactionRepository transactionRepository)
        {
            _userServices = userServices;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var userId = _userServices.RetrieveUserId();
            var model = new TransactionCreationViewModel();
            model.Account = await GetAccount(userId);
            model.Category = await GetCategory(userId, model.OperationTypeId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransactionCreationViewModel model)
        {
            var userId = _userServices.RetrieveUserId();
            /*if (!ModelState.IsValid)
            {
                model.Account = await GetAccount(userId);
                model.Category = await GetCategory(userId, model.OperationTypeId);
                return View(model);
            }*/

            var account = await _accountRepository.GetById(model.AccountId, userId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var category = await _categoryRepository.GetById(model.CategoryId, userId);
            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            model.UserId = userId;

            if (model.OperationTypeId == OperationType.Expense)
            {
                model.Amount *= -1;
            }

            await _transactionRepository.Create(model);
            return RedirectToAction("Index", "Home");
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userServices.RetrieveUserId();
            var transaction = await _transactionRepository.GetById(id, userId);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _transactionRepository.Delete(transaction.Id);
            return RedirectToAction("Index");
        }

    }
}
