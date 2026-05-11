using System.ComponentModel.DataAnnotations;

namespace QuanLyCongTrinhXayDung.Models
{
    public enum TrangThaiCongTrinh
    {
        [Display(Name = "Khởi tạo")] KhoiTao,
        [Display(Name = "Đang thi công")] DangThiCong,
        [Display(Name = "Tạm dừng")] TamDung,
        [Display(Name = "Đã hoàn thành")] HoanThien
    }

    public enum TrangThaiChung
    {
        [Display(Name = "Chưa bắt đầu")] ChuaBatDau,
        [Display(Name = "Đang thi công")] DangThucHien,
        [Display(Name = "Đã hoàn thành")] HoanThanh,
        [Display(Name = "Hủy bỏ")] HuyBo
    }

    public enum LoaiDichVu
    {
        [Display(Name = "Vận chuyển")] VanChuyen,
        [Display(Name = "Vệ sinh")] VeSinh,
        [Display(Name = "Tư vấn")] TuVan,
        [Display(Name = "Khác")] Khac
    }
    public enum DoUuTien
    {
        [Display(Name = "Thấp")] Thap,
        [Display(Name = "Trung bình")] TrungBinh,
        [Display(Name = "Cao")] Cao,
        [Display(Name = "Khẩn cấp")] KhanCap
    }
}
