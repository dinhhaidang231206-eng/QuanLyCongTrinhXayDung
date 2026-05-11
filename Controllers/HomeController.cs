using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuanLyCongTrinhXayDung.Models;
using Microsoft.AspNetCore.Authorization;
using QuanLyCongTrinhXayDung.Data;
using Microsoft.EntityFrameworkCore;

namespace QuanLyCongTrinhXayDung.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Quản trị viên"))
            {
                var today = DateTime.Today;
                ViewBag.CountCongTrinh = await _context.CongTrinhs.CountAsync(c => c.TrangThai == TrangThaiCongTrinh.DangThiCong);
                ViewBag.CountNhanCong = await _context.NhanCongs.CountAsync();
                ViewBag.CountMayMoc = await _context.MayMocs.CountAsync();
                ViewBag.CountTreTienDo = await _context.GiaiDoans.CountAsync(g => 
                    g.TrangThai == TrangThaiChung.DangThucHien && 
                    g.ThoiGianKetThuc < today);

                return View("AdminDashboard");
            }
            else
            {
                // Logic lấy thông tin chấm công hôm nay cho Member
                var today = DateTime.Today;
                var hoTen = User.Identity.Name;

                // Tìm nhân công khớp với tên đăng nhập (hoặc tên thật) và đang trong giai đoạn thi công
                var nhanCong = await _context.NhanCongs
                    .Include(n => n.GiaiDoan)
                    .FirstOrDefaultAsync(n => n.HoTen == hoTen && 
                                              n.GiaiDoan != null && 
                                              n.GiaiDoan.TrangThai == TrangThaiChung.DangThucHien &&
                                              (n.NgayBatDau == null || n.NgayBatDau <= today) &&
                                              (n.NgayKetThuc == null || n.NgayKetThuc >= today));

                if (nhanCong != null)
                {
                    var chamCongHomNay = await _context.ChamCongs
                        .FirstOrDefaultAsync(c => c.NhanCongId == nhanCong.Id && c.Ngay.Date == today);
                    
                    ViewBag.NhanCongId = nhanCong.Id;
                    ViewBag.ChamCongHomNay = chamCongHomNay;
                    ViewBag.TenCongTrinh = nhanCong.GiaiDoan?.CongTrinh?.TenCongTrinh ?? "N/A";
                    ViewBag.TenGiaiDoan = nhanCong.GiaiDoan?.TenGiaiDoan ?? "N/A";
                }
                else
                {
                    ViewBag.NhanCongId = null;
                }

                return View("MemberDashboard");
            }
        }
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckIn(int nhanCongId)
    {
        var today = DateTime.Today;
        var nowTime = DateTime.Now.TimeOfDay;

        var exists = await _context.ChamCongs
            .AnyAsync(c => c.NhanCongId == nhanCongId && c.Ngay.Date == today);

        if (!exists)
        {
            var chamCong = new ChamCong
            {
                NhanCongId = nhanCongId,
                Ngay = today,
                GioDen = new TimeSpan(nowTime.Hours, nowTime.Minutes, 0),
                SoCong = 0 // Chưa tính công cho đến khi check out
            };
            _context.ChamCongs.Add(chamCong);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã Check-in thành công lúc {chamCong.GioDen:hh\\:mm}!";
        }
        else
        {
            TempData["WarningMessage"] = "Bạn đã check-in hôm nay rồi.";
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(int chamCongId)
    {
        var chamCong = await _context.ChamCongs.FindAsync(chamCongId);
        if (chamCong != null && chamCong.GioVe == null)
        {
            var nowTime = DateTime.Now.TimeOfDay;
            chamCong.GioVe = new TimeSpan(nowTime.Hours, nowTime.Minutes, 0);

            // Tính số công cơ bản (Giả sử 8 tiếng = 1 công)
            if (chamCong.GioDen != null)
            {
                double hours = (chamCong.GioVe.Value - chamCong.GioDen.Value).TotalHours;
                if (hours >= 8) chamCong.SoCong = 1.0;
                else if (hours >= 4) chamCong.SoCong = 0.5;
                else chamCong.SoCong = Math.Round(hours / 8.0, 2);
            }

            _context.Update(chamCong);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã Check-out thành công lúc {chamCong.GioVe:hh\\:mm}!";
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
