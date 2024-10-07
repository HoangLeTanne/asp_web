using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BaiKiemTra03_01.Models
{
    public class PhongBan
    {
        [Key]
        public int PhongBanId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Numberofemployees { get; set; }

        public int ManagerId { get; set; }     
    }
}
