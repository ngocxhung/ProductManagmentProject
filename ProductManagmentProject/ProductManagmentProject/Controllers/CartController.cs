using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagmentProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagmentProject.Controllers
{
    public class CartController : Controller
    {
        private readonly FoodManagmentContext _context;
        private readonly int _userId = 1; // Giả lập UserId, sau này lấy từ session

        public CartController(FoodManagmentContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == _userId)
                .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.Stock < quantity)
            {
                return Json(new { success = false, message = "Sản phẩm không đủ hàng!" });
            }

            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == _userId);

            if (cartItem != null)
                cartItem.Quantity += quantity;
            else
                _context.Carts.Add(new Cart { ProductId = productId, Quantity = quantity, UserId = _userId });

            product.Stock -= quantity;
            await _context.SaveChangesAsync();

            return Json(new { success = true, product = new { product.ProductId, product.ProductName, product.Price } });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(int productId, int change) // change = ±1
        {
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == _userId);
            if (cartItem == null) return Json(new { success = false, message = "Sản phẩm không có trong giỏ hàng!" });

            var product = await _context.Products.FindAsync(productId);
            if (product == null) return Json(new { success = false, message = "Sản phẩm không tồn tại!" });

            if (change == -1 && cartItem.Quantity == 1)
                _context.Carts.Remove(cartItem);
            else
                cartItem.Quantity += change;

            product.Stock -= change; // Cộng lại nếu giảm số lượng, trừ nếu tăng
            await _context.SaveChangesAsync();

            return Json(new { success = true, newQuantity = cartItem.Quantity, newStock = product.Stock });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == _userId);
            if (cartItem == null) return Json(new { success = false, message = "Sản phẩm không có trong giỏ hàng!" });

            var product = await _context.Products.FindAsync(productId);
            if (product != null) product.Stock += cartItem.Quantity;

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
