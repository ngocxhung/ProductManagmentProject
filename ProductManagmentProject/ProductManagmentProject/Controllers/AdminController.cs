using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
        public async Task<IActionResult> MonthlySales()
        {
            var salesData = await _foodManagmentContext.Orders
                .Where(o => o.OrderDate.HasValue) // Chỉ lấy các đơn hàng có ngày đặt
                .GroupBy(o => new
                {
                    Year = o.OrderDate.Value.Year,
                    Month = o.OrderDate.Value.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSales = g.Sum(o => o.TotalAmount), // Tính tổng doanh số từ TotalAmount
                    TopProduct = g.SelectMany(o => o.OrderDetails)
                                  .GroupBy(od => od.ProductId)
                                  .Select(pg => new
                                  {
                                      ProductId = pg.Key,
                                      ProductName = pg.FirstOrDefault().Product.ProductName,
                                      ImageUrl = pg.FirstOrDefault().Product.ImageUrl, // Lấy đường dẫn ảnh
                                      TotalQuantity = pg.Sum(od => od.Quantity)
                                  })
                                  .OrderByDescending(pg => pg.TotalQuantity)
                                  .FirstOrDefault() // Lấy sản phẩm bán chạy nhất
                })
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToListAsync();

            return View(salesData);
        }
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _foodManagmentContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Role == "Admin")
            {
                ViewData["StatusError_" + id] = "Không thể thay đổi trạng thái của tài khoản Admin.";

                var userList = await _foodManagmentContext.Users.ToListAsync();
                return View("ListUser", userList); // Render lại view với lỗi
            }

            user.Status = !(user.Status ?? false);
            _foodManagmentContext.Users.Update(user);
            await _foodManagmentContext.SaveChangesAsync();
            return RedirectToAction("ListUser");
        }
        public async Task<IActionResult> DownloadMonthlySales()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var salesData = await _foodManagmentContext.Orders
                .Where(o => o.OrderDate.HasValue)
                .GroupBy(o => new
                {
                    Year = o.OrderDate.Value.Year,
                    Month = o.OrderDate.Value.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalSales = g.Sum(o => o.TotalAmount),
                    TopProduct = g.SelectMany(o => o.OrderDetails)
                                .GroupBy(od => od.ProductId)
                                .Select(pg => new
                                {
                                    ProductName = pg.FirstOrDefault().Product.ProductName,
                                    TotalQuantity = pg.Sum(od => od.Quantity)
                                })
                                .OrderByDescending(pg => pg.TotalQuantity)
                                .FirstOrDefault()
                })
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("MonthlySales");

                // Tạo tiêu đề cột
                worksheet.Cells[1, 1].Value = "Năm";
                worksheet.Cells[1, 2].Value = "Tháng";
                worksheet.Cells[1, 3].Value = "Doanh số (VNĐ)";
                worksheet.Cells[1, 4].Value = "Sản phẩm bán chạy nhất";
                worksheet.Cells[1, 5].Value = "Số lượng bán";

                // Đổ dữ liệu vào file Excel
                int row = 2;
                foreach (var item in salesData)
                {
                    worksheet.Cells[row, 1].Value = item.Year;
                    worksheet.Cells[row, 2].Value = item.Month;
                    worksheet.Cells[row, 3].Value = item.TotalSales;
                    worksheet.Cells[row, 4].Value = item.TopProduct?.ProductName ?? "Không có dữ liệu";
                    worksheet.Cells[row, 5].Value = item.TopProduct?.TotalQuantity ?? 0;
                    row++;
                }

                // Định dạng cột tự động điều chỉnh kích thước
                worksheet.Cells.AutoFitColumns();

                // Xuất file Excel ra stream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Trả về file Excel
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MonthlySales.xlsx");
            }
        }
    }
}


