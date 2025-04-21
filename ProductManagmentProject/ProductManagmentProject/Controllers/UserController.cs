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
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            if (request == null || request.CartItems == null || !request.CartItems.Any())
            {
                return Json(new { success = false, message = "Giỏ hàng trống hoặc dữ liệu không hợp lệ." });
            }

            try
            {
                // Tạo đơn hàng mới
                var order = new Order
                {
                    UserId = 1, // Thay bằng UserId thực tế (nếu có hệ thống đăng nhập)
                    OrderDate = DateTime.Now,
                    TotalAmount = request.Total,
                    Status = "Pending",
                    OrderDetails = request.CartItems.Select(item => new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = _foodManagmentContext.Products
                            .Where(p => p.ProductId == item.ProductId)
                            .Select(p => p.Price)
                            .FirstOrDefault()
                    }).ToList()
                };

                // Lưu đơn hàng vào cơ sở dữ liệu
                _foodManagmentContext.Orders.Add(order);
                await _foodManagmentContext.SaveChangesAsync();

                return Json(new { success = true, message = "Thanh toán thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi khi lưu đơn hàng: {ex.Message}" });
            }
        }
    }
    public class CheckoutRequest
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Giá trị mặc định
        public decimal Total { get; set; }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
