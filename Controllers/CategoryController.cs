using Budget_Management.Models;
using Budget_Management.Services;
using Microsoft.AspNetCore.Mvc;

namespace Budget_Management.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserServices _userServices;
        public CategoryController(ICategoryRepository categoryRepository,
                                  IUserServices userServices)
        {
            _categoryRepository = categoryRepository;
            _userServices = userServices;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Create(Category category)
        {
            var userId = _userServices.RetrieveUserId();
            category.userId = userId;

            await _categoryRepository.Create(category);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userServices.RetrieveUserId();
            var categories = await _categoryRepository.GetAll(userId);
            return View(categories);
        }
    }
}
