using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProductManagmentProject.Hubs;
using ProductManagmentProject.Models;

namespace ProductManagmentProject.Controllers
{
    public class OrderController : Controller
    {
        private readonly FoodManagmentContext _foodManagmentContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<NotificationHub> _hubContext;

        public OrderController(FoodManagmentContext context, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContext)
        {
            _foodManagmentContext = context;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Orders")]
        public async Task<IActionResult> Orders()
        {
            var userRole = _httpContextAccessor.HttpContext?.Session.GetString("UserRole");
            if (userRole != "Admin" && userRole != "Staff")
            {
                return RedirectToAction("Login", "Auth");
            }

            var orders = await _foodManagmentContext.Orders
                .Include(o => o.User)
                .ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var userRole = _httpContextAccessor.HttpContext?.Session.GetString("UserRole");
            if (userRole != "Admin" && userRole != "Staff")
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = await _foodManagmentContext.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveOrder(int id)
        {
            var userRole = _httpContextAccessor.HttpContext?.Session.GetString("UserRole");
            if (userRole != "Admin" && userRole != "Staff")
            {
                return Json(new { success = false, message = "Bạn không có quyền duyệt đơn hàng." });
            }

            try
            {
                Console.WriteLine($"ApproveOrder called with id: {id}");
                var order = await _foodManagmentContext.Orders
                    .FirstOrDefaultAsync(o => o.OrderId == id);
                if (order == null)
                {
                    Console.WriteLine($"Order not found for ID: {id}");
                    return Json(new { success = false, message = $"Không tìm thấy đơn hàng với ID: {id}." });
                }

                if (order.Status != "Pending")
                {
                    return Json(new { success = false, message = "Chỉ có thể duyệt đơn hàng ở trạng thái Pending." });
                }

                if (order.UserId == 0 || order.OrderDate == null)
                {
                    Console.WriteLine($"Invalid order data: UserId={order.UserId}, OrderDate={order.OrderDate}");
                    return Json(new { success = false, message = "Dữ liệu đơn hàng không hợp lệ." });
                }

                const string newStatus = "Approved";
                order.Status = newStatus;
                await _foodManagmentContext.SaveChangesAsync();
                Console.WriteLine($"Order {id} approved successfully.");

                // Gửi thông báo SignalR đến người dùng
                var notificationData = new
                {
                    orderId = order.OrderId,
                    status = order.Status,
                    message = $"Đơn hàng #{order.OrderId} của bạn đã được duyệt."
                };
                Console.WriteLine($"Sending SignalR notification to UserId: {order.UserId}, Data: {System.Text.Json.JsonSerializer.Serialize(notificationData)}");
                await _hubContext.Clients.Group(order.UserId.ToString()).SendAsync(
                    "ReceiveOrderNotification",
                    notificationData
                );

                return Json(new { success = true, message = "Đơn hàng đã được duyệt thành công." });
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine($"DbUpdateException: {errorMessage}");
                if (errorMessage.Contains("CHECK constraint"))
                {
                    return Json(new { success = false, message = "Trạng thái 'Approved' không được phép. Vui lòng kiểm tra ràng buộc cơ sở dữ liệu." });
                }
                return Json(new { success = false, message = $"Lỗi khi lưu đơn hàng: {errorMessage}" });
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Console.WriteLine($"General error: {errorMessage}");
                return Json(new { success = false, message = $"Lỗi khi duyệt đơn hàng: {errorMessage}" });
            }
        }
    }
}