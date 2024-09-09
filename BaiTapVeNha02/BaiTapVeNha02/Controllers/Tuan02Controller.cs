using Microsoft.AspNetCore.Mvc;

namespace BaiTapVeNha02.Controllers
{
    public class Tuan02Controller : Controller
    {
        public IActionResult Index()
        {
            ViewBag.HoTen = "Hoàng Lê Tấn";
            ViewBag.MSSV = "1822040193";
            ViewBag.Nam = "Năm 2024";
            return View();
        }
    }
}
