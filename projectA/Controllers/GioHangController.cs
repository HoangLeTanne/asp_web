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
        public IActionResult Index()
        {
            //Lấy thông tin tài khoản
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            //Lấy danh sách các sản phẩm trong gio hang của User
            IEnumerable<GioHang> dsGioHang = _db.GioHang.Include("SanPham").
                    Where(gh => gh.ApplicationUserId == claim.Value).ToList();


            return View(dsGioHang);
        }
    }
}
