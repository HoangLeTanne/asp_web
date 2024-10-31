using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectA.Data;
using projectA.Models;
using System.Security.Claims;

namespace projectA.Controllers
{
    [Area("Customer")]
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _db;
        public GioHangController(ApplicationDbContext db)
        { 
            _db = db; 
        }

        [Authorize]
        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            //IEnumerable<Giohang> dsGioHang = _db.Giohang.Include("SanPham")
            //.Where(gh=>gh.ApplicationUserId == claim.Value).ToList();

            //return View(dsGioHang);
            GioHangViewModel giohang = new GioHangViewModel
            {
                DsGioHang = _db.GioHang.Include("SanPham")
                .Where(gh => gh.ApplicationUserId == claim.Value).ToList(),
                HoaDon = new HoaDon()
            };

            foreach (var item in giohang.DsGioHang)
            {
                double prodcutprice = item.Quantity * item.SanPham.price;
                giohang.HoaDon.Total += prodcutprice;
            }
            return View(giohang);
        }
        [Authorize]
        public IActionResult ThanhToan()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            //IEnumerable<Giohang> dsGioHang = _db.Giohang.Include("SanPham")
            //.Where(gh=>gh.ApplicationUserId == claim.Value).ToList();

            //return View(dsGioHang);
            GioHangViewModel giohang = new GioHangViewModel
            {
                DsGioHang = _db.GioHang.Include("SanPham")
                .Where(gh => gh.ApplicationUserId == claim.Value).ToList(),
                HoaDon = new HoaDon()
            };

            giohang.HoaDon.ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value);

            giohang.HoaDon.Name = giohang.HoaDon.ApplicationUser.Name;
            giohang.HoaDon.Address = giohang.HoaDon.ApplicationUser.Address;
            giohang.HoaDon.PhoneNumber = giohang.HoaDon.ApplicationUser.PhoneNumber;

            foreach (var item in giohang.DsGioHang)
            {
                double prodcutprice = item.Quantity * item.SanPham.price;
                giohang.HoaDon.Total += prodcutprice;
            }
            return View(giohang);
        }
        [HttpPost]
        [Authorize]

        public IActionResult ThanhToan(GioHangViewModel giohang)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            giohang.DsGioHang = _db.GioHang.Include("SanPham").Where(gh => gh.ApplicationUserId == claim.Value).ToList();


            giohang.HoaDon.ApplicationUserId = claim.Value;
            giohang.HoaDon.OrderDate = DateTime.Now;
            giohang.HoaDon.OrderStatus = "Đang xác nhận";

            foreach (var item in giohang.DsGioHang)
            {
                double prodcutprice = item.Quantity * item.SanPham.price;
                giohang.HoaDon.Total += prodcutprice;
            }

            _db.HoaDon.Add(giohang.HoaDon);
            _db.SaveChanges();

            //Thêm thông tin chi tiết hóa đơn
            foreach (var item in giohang.DsGioHang)
            {
                ChiTietHoaDon chitiethoadon = new ChiTietHoaDon()
                {
                    SanPhamId = item.SanPhamId,
                    HoaDonId = giohang.HoaDon.Id,
                    ProductPrice = item.SanPham.price * item.Quantity,
                    Quantity = item.Quantity
                };
                _db.ChiTietHoaDon.Add(chitiethoadon);
                _db.SaveChanges();
            }

            //Xoa thong tin trong gio hang
            _db.GioHang.RemoveRange(giohang.DsGioHang);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize]


        public IActionResult Xoa(int giohangId)
        {
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);

            _db.GioHang.Remove(giohang); 

            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]

        public IActionResult Tang(int giohangId)
        {
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);

            giohang.Quantity = giohang.Quantity + 1;

            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]

        public IActionResult Giam(int giohangId)
        {
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);

            giohang.Quantity = giohang.Quantity - 1;

            if (giohang.Quantity == 0)
            {
                _db.GioHang.Remove(giohang);
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
