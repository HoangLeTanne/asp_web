using BaiTapKiemTra01.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTapKiemTra01.Controllers
{
    public class TaiKhoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DangKy(TaiKhoanViewModel model)
        {
            if (model.Username != null )
            {
                return Content(model.Username);
            }
            return View(model);
        }
        public IActionResult BaiTap2()
        {
            var sanpham = new SanPhamViewModel();
            {

            }
            return View(sanpham);
        }

    }
}
