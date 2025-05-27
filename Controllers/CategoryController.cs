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
        public async Task<IActionResult> Index()
        {
            var userId = _userServices.RetrieveUserId();
            var categories = await _categoryRepository.GetAll(userId);
            return View(categories);
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
        public async Task<IActionResult> Update(int id)
        {
            var userId = _userServices.RetrieveUserId();
            var category = await _categoryRepository.GetById(id, userId);
            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            var userId = _userServices.RetrieveUserId();
            var retrievedCategory = await _categoryRepository.GetById(category.Id, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Index");
            }

            category.userId = userId;
            await _categoryRepository.Update(category);
            return RedirectToAction("Index", "Home");
        }
    }
}
