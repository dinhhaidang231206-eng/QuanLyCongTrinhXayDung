using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCongTrinhXayDung.Models
{
    public class DichVu
    {
        public int Id { get; set; }
        
        [Display(Name = "Loại dịch vụ")]
        public LoaiDichVu LoaiDichVu { get; set; } = LoaiDichVu.Khac;
        
        [StringLength(255)]
        [Display(Name = "Loại dịch vụ khác")]
        public string? LoaiDichVuKhac { get; set; }
        
        [StringLength(1000)]
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string? MoTa { get; set; }
        
        [Display(Name = "Giai đoạn")]
        public int GiaiDoanId { get; set; }
        [ForeignKey("GiaiDoanId")]
        public virtual GiaiDoan? GiaiDoan { get; set; }
        
        [StringLength(255)]
        [Display(Name = "Nhà cung cấp")]
        public string? NhaCungCap { get; set; }
        
        [Display(Name = "Đơn giá")]
        public decimal DonGia { get; set; }
        
        [Display(Name = "Số lượng")]
        public double SoLuong { get; set; }
        
        [Display(Name = "Thời gian bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime? ThoiGianBatDau { get; set; }
        
        [Display(Name = "Thời gian kết thúc")]
        [DataType(DataType.Date)]
        public DateTime? ThoiGianKetThuc { get; set; }
        
        [Display(Name = "Trạng thái")]
        public TrangThaiChung TrangThai { get; set; } = TrangThaiChung.ChuaBatDau;
        
        [StringLength(500)]
        [Display(Name = "Mục đích sử dụng")]
        public string? MucDichSuDung { get; set; }
    }
}
