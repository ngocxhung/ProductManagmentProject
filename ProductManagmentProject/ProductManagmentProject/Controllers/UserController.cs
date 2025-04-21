using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductManagmentProject.Models;

namespace ProductManagmentProject.Controllers
{
    public class UserController : Controller
    {
        private readonly FoodManagmentContext _foodManagmentContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(FoodManagmentContext context, IHttpContextAccessor httpContextAccessor)
        {
            _foodManagmentContext = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _foodManagmentContext.Products
                .Include(p => p.Category)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Profile()
        {
            var userEmail = _httpContextAccessor.HttpContext?.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _foodManagmentContext.Users
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            return View(user);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var userEmail = _httpContextAccessor.HttpContext?.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = await _foodManagmentContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id && o.User.Email == userEmail);
            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng hoặc bạn không có quyền xem.");
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userEmail = _httpContextAccessor.HttpContext?.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để hủy đơn hàng." });
            }

            var order = await _foodManagmentContext.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id && o.User.Email == userEmail);
            if (order == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn hàng hoặc bạn không có quyền hủy." });
            }

            // Kiểm tra trạng thái đơn hàng
            if (order.Status != "Pending" && order.Status != "Processing")
            {
                return Json(new { success = false, message = "Không thể hủy đơn hàng ở trạng thái này." });
            }

            try
            {
                // Xóa OrderDetails liên quan
                var orderDetails = await _foodManagmentContext.OrderDetails
                    .Where(od => od.OrderId == id)
                    .ToListAsync();
                _foodManagmentContext.OrderDetails.RemoveRange(orderDetails);

                // Xóa Payments liên quan (nếu có)
                var payments = await _foodManagmentContext.Payments
                    .Where(p => p.OrderId == id)
                    .ToListAsync();
                _foodManagmentContext.Payments.RemoveRange(payments);

                // Xóa Order
                _foodManagmentContext.Orders.Remove(order);
                await _foodManagmentContext.SaveChangesAsync();

                return Json(new { success = true, message = "Đơn hàng đã được hủy thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi khi hủy đơn hàng: {ex.Message}" });
            }
        }
    }
}