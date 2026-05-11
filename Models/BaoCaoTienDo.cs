using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCongTrinhXayDung.Models
{
    public class BaoCaoTienDo
    {
        public int Id { get; set; }
        
        [Display(Name = "Giai đoạn")]
        public int GiaiDoanId { get; set; }
        [ForeignKey("GiaiDoanId")]
        public virtual GiaiDoan? GiaiDoan { get; set; }
        
        [Display(Name = "Ngày báo cáo")]
        [DataType(DataType.Date)]
        public DateTime NgayBaoCao { get; set; } = DateTime.Now;
        
        [Required, StringLength(255)]
        [Display(Name = "Tiêu đề")]
        public string TieuDe { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nội dung")]
        [DataType(DataType.MultilineText)]
        public string NoiDung { get; set; } = string.Empty;

        [Display(Name = "Độ ưu tiên")]
        public DoUuTien UuTien { get; set; } = DoUuTien.TrungBinh;

        [Display(Name = "Phản hồi từ Admin")]
        [DataType(DataType.MultilineText)]
        public string? TraLoi { get; set; }
        
        public virtual System.Collections.Generic.ICollection<HinhAnhBaoCao> HinhAnhs { get; set; } = new System.Collections.Generic.List<HinhAnhBaoCao>();
        
        [Display(Name = "Người báo cáo")]
        public int? ThanhVienId { get; set; }
        [ForeignKey("ThanhVienId")]
        [Display(Name = "Người báo cáo")]
        public virtual ThanhVien? NguoiBaoCao { get; set; }
    }
}

