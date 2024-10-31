using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projectA.Data;
using projectA.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace projectA.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult GioiThieu()
        {
            return View();
        }

        public IActionResult LienHe()
        {
            return View();
        }
        public IActionResult Index()
        {
            IEnumerable<SanPham> sanpham = _db.SanPham.Include("TheLoai").ToList();
            return View(sanpham);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Details(int sanphamId)
        {
            GioHang giohang = new GioHang()
            {
                SanPhamId = sanphamId,
                SanPham = _db.SanPham.Include(sp => sp.TheLoai)
                .FirstOrDefault(sp => sp.Id == sanphamId),
                Quantity = 1
            };
            return View(giohang);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(GioHang giohang)
        {
            // Lay thong tin tai khoan
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            giohang.ApplicationUserId = claim.Value;


            // Ki?m tra s?n ph?m ?ã có trong c? s? d? li?u hay ch?a?
            var giohangdb = _db.GioHang.FirstOrDefault(gh => gh.SanPhamId == giohang.SanPhamId
       && gh.ApplicationUserId == giohang.ApplicationUserId);

            if (giohangdb == null)
            {
                _db.GioHang.Add(giohang); // Them san pham vao gio hang
            }
            else
            {
                giohangdb.Quantity += giohang.Quantity;
            }
            // Them san pham vao gio hang
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult FilterByTheLoai(int id)
        {
            IEnumerable<SanPham> sanpham = _db.SanPham.Include("TheLoai").Where(sp => sp.TheLoai.Id == id).ToList();
            return View("Index", sanpham);
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(int sanphamId)
        {
            // Lấy thông tin người dùng
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            // Tạo đối tượng GioHang
            GioHang giohang = new GioHang
            {
                SanPhamId = sanphamId,
                ApplicationUserId = claim.Value,
                Quantity = 1 // Giả sử mỗi lần thêm là 1 sản phẩm
            };

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            var giohangdb = _db.GioHang.FirstOrDefault(gh => gh.SanPhamId == sanphamId && gh.ApplicationUserId == giohang.ApplicationUserId);

            if (giohangdb == null)
            {
                _db.GioHang.Add(giohang); // Thêm sản phẩm vào giỏ hàng
                TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng!";
            }
            else
            {
                giohangdb.Quantity += giohang.Quantity; // Tăng số lượng nếu sản phẩm đã có
                TempData["SuccessMessage"] = "Sản phẩm đã được cập nhật trong giỏ hàng!";
            }

            // Lưu thay đổi vào cơ sở dữ liệu
            _db.SaveChanges();

            // Quay lại trang trước đó
            return RedirectToAction("Index");
        }





    }
}
