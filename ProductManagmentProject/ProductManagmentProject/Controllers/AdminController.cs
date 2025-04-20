using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagmentProject.Models;

namespace ProductManagmentProject.Controllers
{
    public class AdminController : Controller

    {
        private readonly FoodManagmentContext _foodManagmentContext;
        public AdminController(FoodManagmentContext context)
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
        //Create product
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _foodManagmentContext.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string filePath = Path.Combine(uploadsFolder, ImageFile.FileName);//Xác định đường dẫn lưu file
                using (var steam = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(steam);
                }
                product.ImageUrl = "images/" + ImageFile.FileName;
            }
            _foodManagmentContext.Products.Add(product);
            await _foodManagmentContext.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }




        //List User
        public async Task<IActionResult> ListUser()
        {
            var user = await _foodManagmentContext.Users.ToListAsync();
            return View(user);
        }
        //Add User
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(User user)
        {
            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.PasswordHash) ||
                string.IsNullOrWhiteSpace(user.FullName) ||
                string.IsNullOrWhiteSpace(user.Role))
            {
                return Content("Thiếu thông tin. Vui lòng nhập đầy đủ Email, Password, FullName và Role.");
            }

            try
            {
                user.Username = user.Email; // Gán Username = Email để tránh lỗi NULL
                user.CreatedAt = DateTime.Now; // Gán ngày tạo

                _foodManagmentContext.Users.Add(user);
                await _foodManagmentContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content($"Lỗi khi thêm user: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        //Edit 
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _foodManagmentContext.Products.FindAsync(id);//tim kiem theo id
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _foodManagmentContext.Categories.ToListAsync();


            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? ImageFile)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            var exsistingProduct = await _foodManagmentContext.Products.FindAsync(id);
            if (exsistingProduct == null)
            {
                return NotFound();
            }
            exsistingProduct.ProductName = product.ProductName;
            exsistingProduct.Description = product.Description;
            exsistingProduct.Price = product.Price;
            exsistingProduct.Stock = product.Stock;
            exsistingProduct.CategoryId = product.CategoryId;

            //UPdate image
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                string filePath = Path.Combine(uploadFolder, ImageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                exsistingProduct.ImageUrl = "images/" + ImageFile.FileName;
            }
            _foodManagmentContext.Products.Update(exsistingProduct);
            await _foodManagmentContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        //Details


        public async Task<IActionResult> Details(int id)
        {
            var product = await _foodManagmentContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _foodManagmentContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _foodManagmentContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _foodManagmentContext.Products.Remove(product);
            await _foodManagmentContext.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }
    }
}


