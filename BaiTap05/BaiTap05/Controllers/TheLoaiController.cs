using BaiTap05.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTap05.Controllers
{
    public class TheLoaiController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Ngay = "Ngày 02";
            ViewBag.Thang = "Tháng 11";
            ViewData["Nam"] = "Năm 2040";           
            return View();
        }
        public IActionResult Index2()
        {
            var theloai = new TheLoaiViewModel
            {
                Id = 1,
                Name = "Trinh thám"
            };

            return View(theloai);
        }
    }
}
