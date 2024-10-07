using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BaiKiemTra03_01.Models
{
    public class NhanVien
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
           
        public int PhongBanId { get; set; }
        [ForeignKey("PhongBanId")]
        [ValidateNever]
        public PhongBan PhongBan { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

    }
}
