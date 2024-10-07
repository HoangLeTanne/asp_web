using BaiKiemTra03_01.Data;
using BaiKiemTra03_01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BaiKiemTra03_01.Controllers
{
    [Area("Admin")]
    public class NhanVienController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NhanVienController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<NhanVien> nhanvien = _db.NhanVien.Include("PhongBan").ToList();
            return View(nhanvien);
        }
        
        [HttpPost]
        public IActionResult Upsert(NhanVien nhanvien)
        {
            if (ModelState.IsValid)
            {
                if (nhanvien.Id == 0)
                {
                    _db.NhanVien.Add(nhanvien);
                }
                else
                {
                    _db.NhanVien.Update(nhanvien);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {

            var nhanvien = _db.NhanVien.FirstOrDefault(nv => nv.Id == id);
            if (nhanvien == null)
            {
                return NotFound();
            }
            _db.NhanVien.Remove(nhanvien);
            _db.SaveChanges();
            return Json(new { success = true });

        }
    }
}
