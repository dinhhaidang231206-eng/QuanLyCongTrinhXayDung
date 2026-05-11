using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCongTrinhXayDung.Models
{
    public class ChamCong
    {
        public int Id { get; set; }
        
        [Display(Name = "Nhân công")]
        public int NhanCongId { get; set; }
        [ForeignKey("NhanCongId")]
        public virtual NhanCong? NhanCong { get; set; }
        
        [Display(Name = "Ngày chấm công")]
        [DataType(DataType.Date)]
        public DateTime Ngay { get; set; } = DateTime.Today;
        
        [Display(Name = "Số công")]
        public double SoCong { get; set; } = 1.0;

        [Display(Name = "Giờ đến")]
        [DataType(DataType.Time)]
        public TimeSpan? GioDen { get; set; }

        [Display(Name = "Giờ về")]
        [DataType(DataType.Time)]
        public TimeSpan? GioVe { get; set; }
    }
}
