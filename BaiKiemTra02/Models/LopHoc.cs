using System.ComponentModel.DataAnnotations;

namespace BaiKiemTra02.Models
{
    public class LopHoc
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TenLopHoc { get; set; }
        public int NamNhapHoc { get; set; }
        public int NamRaTruong { get; set; }
        public int SoLuongSinhVien { get; set; }

    }
}
