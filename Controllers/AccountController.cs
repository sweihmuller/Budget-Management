using AutoMapper;
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
        private readonly IMapper _iMapper;
        private readonly ITransactionRepository _transactionRepository; 

        public AccountController(IAccountTypeRepository accountTypeRepository,
                                 IUserServices userServices,
                                 IAccountRepository accountRepository,
                                 ITransactionRepository transactionRepository,
                                 IMapper mapper)
        {
            _accountTypeRepository = accountTypeRepository;
            _userServices = userServices;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _iMapper = mapper;
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

        public async Task<IActionResult> Details(int id, int month, int year) 
         {
            var userId = _userServices.RetrieveUserId();
            var account = await _accountRepository.GetById(id, userId);
            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            DateTime startDate;
            DateTime endDate;

            if (month == 0 || month > 12 || year < 1900)
            {
                var today = DateTime.Today;
                startDate = new DateTime(today.Year, today.Month, 1);

            } else
            {
                startDate = new DateTime(year, month, 1);
            }

            endDate = startDate.AddMonths(1).AddDays(-1);

            var getTransactionByAccount = new GetTransactionByAccount
            {
                AccountId = id,
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate
            };

            var transactions = await _transactionRepository.GetAccountById(getTransactionByAccount);

            var model = new DetailedReportTransactions();
            ViewBag.AccountName = account.Name;

            var transactionByDate = transactions.OrderByDescending(x => x.DateTransaction)
                                                .GroupBy(x => x.DateTransaction)
                                                .Select(group => new DetailedReportTransactions.GetTransactionByDate()
                                                {
                                                    DateTransaction = group.Key,
                                                    Transactions = group.AsEnumerable()
                                                });

            model.TransactionsByDate = transactionByDate;
            model.DateStart = startDate;
            model.DateEnd = endDate;

            ViewBag.PreviousMonth = startDate.AddMonths(-1).Month;
            ViewBag.PreviousYear = startDate.AddMonths(-1).Year;

            ViewBag.NextMonth = startDate.AddMonths(1).Month;
            ViewBag.NextYear = startDate.AddMonths(1).Year;

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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userServices.RetrieveUserId();
            var account = await _accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var model = _iMapper.Map<CreationAccountViewModel>(account);

            model.AccountType = await GetAccountTypes(userId);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreationAccountViewModel creationAccountViewModel)
        {
            var userId = _userServices.RetrieveUserId();
            var account = await _accountRepository.GetById(creationAccountViewModel.Id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var accountType = await _accountTypeRepository.RetrieveById(creationAccountViewModel.Id, userId);
            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _accountRepository.Update(creationAccountViewModel);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetAccountTypes(int userId)
        {
            var accountType = await _accountTypeRepository.Retrieve(userId);
            return accountType.Select(x => new SelectListItem(x.name, x.Id.ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userServices.RetrieveUserId();
            var account = await _accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = _userServices.RetrieveUserId();
            var account = await _accountRepository.GetById(id, userId);

            if(account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _accountRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
