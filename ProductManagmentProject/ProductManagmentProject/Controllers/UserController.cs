using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagmentProject.Models;

namespace ProductManagmentProject.Controllers
{
    public class UserController : Controller
    {
        private readonly FoodManagmentContext _foodManagmentContext;
        public UserController(FoodManagmentContext context)
        {
            _foodManagmentContext = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _foodManagmentContext.Products
                .Include(p => p.Category)
                .ToListAsync();
            return View(products);
        }
    }
}
