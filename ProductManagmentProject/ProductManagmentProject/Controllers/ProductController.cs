using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagmentProject.Models; // Thay bằng namespace của bạn

public class ProductController : Controller
{
    private readonly FoodManagmentContext _foodManagmentContext;

    public ProductController(FoodManagmentContext context)
    {
        _foodManagmentContext = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _foodManagmentContext.Products
            .Include(p => p.Category) // Load thêm dữ liệu Category
            .ToListAsync();
        return View(products);
    }
    
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories=await _foodManagmentContext.Categories.ToListAsync();
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


    // Phương thức GET để hiển thị form chỉnh sửa
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _foodManagmentContext.Products.FindAsync(id);
        if (product == null) return NotFound();

        ViewBag.Categories = await _foodManagmentContext.Categories.ToListAsync();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product, IFormFile? ImageFile)
    {
        var existingProduct = await _foodManagmentContext.Products.FindAsync(id);
        if (existingProduct == null) return NotFound();

        if (ImageFile != null)
        {
            string filePath = Path.Combine("wwwroot/images", ImageFile.FileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await ImageFile.CopyToAsync(stream);
            product.ImageUrl = "images/" + ImageFile.FileName;
        }
        else product.ImageUrl = existingProduct.ImageUrl;

        _foodManagmentContext.Entry(existingProduct).CurrentValues.SetValues(product);
        await _foodManagmentContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // Kiểm tra sản phẩm có tồn tại không
    private bool ProductExist(int id)
    {
        return _foodManagmentContext.Products
            .AsNoTracking()
            .Any(p => p.ProductId == id);
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchString)
    {
        // Lấy danh sách sản phẩm và bao gồm thông tin danh mục
        var productsQuery = _foodManagmentContext.Products
            .Include(p => p.Category)
            .AsQueryable();

        // Nếu có từ khóa tìm kiếm, lọc sản phẩm
        if (!string.IsNullOrEmpty(searchString))
        {
            var loweredSearchString = searchString.ToLower();
            productsQuery = productsQuery
                .Where(p => p.ProductName.ToLower().Contains(loweredSearchString));
        }

        // Lấy danh sách sản phẩm đã lọc (hoặc toàn bộ nếu không có từ khóa)
        var products = await productsQuery.ToListAsync();

        // Truyền từ khóa tìm kiếm vào ViewBag để hiển thị lại trên giao diện
        ViewBag.SearchString = searchString;

        return View(products);
    }


    // Hiển thị trang xác nhận xóa sản phẩm
    //public async Task<IActionResult> Delete(int id)
    //{
    //    var product = await _foodManagmentContext.Products
    //        .Include(p => p.Category) // Load dữ liệu danh mục
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync(p => p.ProductId == id);

    //    if (product == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(product);
    //}

    //// Xóa sản phẩm (POST)
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(int id)
    //{
    //    var product = await _foodManagmentContext.Products.FindAsync(id);
    //    if (product == null)
    //    {
    //        return NotFound();
    //    }

    //    try
    //    {
    //        _foodManagmentContext.Products.Remove(product);
    //        await _foodManagmentContext.SaveChangesAsync();
    //        Console.WriteLine($"✅ Product {id} deleted successfully!");

    //        return RedirectToAction(nameof(Index));
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"❌ Error deleting product: {ex.Message}");
    //        return View(product);
    //    }
    //}



}