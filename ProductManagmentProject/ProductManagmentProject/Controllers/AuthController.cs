using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagmentProject.Models;

namespace ProductManagmentProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly FoodManagmentContext _foodManagmentContext;

        public AuthController(FoodManagmentContext context)
        {
            _foodManagmentContext = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập email và mật khẩu.";
                return View();
            }

            var user = await _foodManagmentContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.PasswordHash != password) // Không mã hóa
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng.";
                return View();
            }

            // Lưu thông tin user vào session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);
            HttpContext.Session.SetString("UserId", user.UserId.ToString()); // Store userId in session

            switch (user.Role)
            {
                case "Admin":
                    return RedirectToAction("Index", "Admin");
                case "Staff":
                    return RedirectToAction("Index", "Product");
                case "User":
                    return RedirectToAction("Index", "User");
                default:
                    return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _foodManagmentContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ViewBag.Error = "Email không tồn tại trong hệ thống.";
                return View();
            }

            // Tạo token reset mật khẩu (ở đây dùng email làm giả token)
            string resetToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10);

            // Giả sử gửi token qua email (có thể lưu vào DB nếu cần)
            HttpContext.Session.SetString("ResetToken", resetToken);
            HttpContext.Session.SetString("ResetEmail", email);

            ViewBag.Message = $"Token đặt lại mật khẩu của bạn: {resetToken}";
            return View("ForgotPassword");
        }



        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string email, string newPassword)
        {
            string storedToken = HttpContext.Session.GetString("ResetToken");
            string storedEmail = HttpContext.Session.GetString("ResetEmail");

            if (storedToken == null || storedEmail == null || storedToken != token || storedEmail != email)
            {
                ViewBag.Error = "Token không hợp lệ hoặc đã hết hạn.";
                return View();
            }

            var user = await _foodManagmentContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ViewBag.Error = "Không tìm thấy người dùng.";
                return View();
            }

            user.PasswordHash = newPassword; // Lưu mật khẩu mới (nên mã hóa)
            await _foodManagmentContext.SaveChangesAsync();

            ViewBag.Message = "Mật khẩu đã được đặt lại thành công.";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Xóa session
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserRole");

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("Login", "Auth");
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string username, string password, string? phone)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }

            // Kiểm tra email hoặc username đã tồn tại
            var existingUser = await _foodManagmentContext.Users.FirstOrDefaultAsync(u => u.Email == email || u.Username == username);
            if (existingUser != null)
            {
                ViewBag.Error = "Email hoặc tên đăng nhập đã được sử dụng.";
                return View();
            }

            // Tạo user mới
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Username = username,
                PasswordHash = password,
                Phone = phone,
                Role = "User",
                CreatedAt = DateTime.Now,
                Status = true
            };

            _foodManagmentContext.Users.Add(newUser);

            try
            {
                await _foodManagmentContext.SaveChangesAsync();
                ViewBag.Message = "Đăng ký thành công. Vui lòng đăng nhập.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while saving the user: " + ex.Message;
                return View();
            }

            return RedirectToAction("Login", "Auth");

        }
    }
}
