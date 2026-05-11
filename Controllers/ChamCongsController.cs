using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyCongTrinhXayDung.Data;
using QuanLyCongTrinhXayDung.Models;

namespace QuanLyCongTrinhXayDung.Controllers
{
    [Authorize(Roles = "Admin,Quản trị viên")]
    public class ChamCongsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChamCongsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChamCongs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChamCongs
                .Include(c => c.NhanCong)
                    .ThenInclude(n => n!.GiaiDoan)
                        .ThenInclude(g => g!.CongTrinh)
                .OrderByDescending(c => c.Ngay);

            // Thống kê hôm nay
            var today = DateTime.Today;
            ViewBag.ChamCongHomNay = await _context.ChamCongs.CountAsync(c => c.Ngay.Date == today);
            ViewBag.TongNhanCongHoatDong = await _context.NhanCongs
                .Where(n => n.GiaiDoan != null && n.GiaiDoan.TrangThai == TrangThaiChung.DangThucHien
                         && (n.NgayBatDau == null || n.NgayBatDau <= today)
                         && (n.NgayKetThuc == null || n.NgayKetThuc >= today))
                .CountAsync();
            ViewBag.ChuaChamHomNay = ViewBag.TongNhanCongHoatDong - ViewBag.ChamCongHomNay;
            if (ViewBag.ChuaChamHomNay < 0) ViewBag.ChuaChamHomNay = 0;

            return View(await applicationDbContext.ToListAsync());
        }

        // POST: ChamCongs/ChamCongTuDong - Chạy ngay không cần chờ 17h
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChamCongTuDong()
        {
            var today = DateTime.Today;

            // Lấy tất cả giai đoạn đang thi công
            var activePhaseIds = await _context.GiaiDoans
                .Where(g => g.TrangThai == TrangThaiChung.DangThucHien)
                .Select(g => g.Id)
                .ToListAsync();

            if (!activePhaseIds.Any())
            {
                TempData["WarningMessage"] = "Không có giai đoạn nào đang thi công. Hãy chuyển trạng thái giai đoạn sang 'Đang thi công'.";
                return RedirectToAction(nameof(Index));
            }

            // Nhân công hợp lệ trong các giai đoạn đang thi công
            var activeLaborers = await _context.NhanCongs
                .Where(n => activePhaseIds.Contains(n.GiaiDoanId)
                         && (n.NgayBatDau == null || n.NgayBatDau <= today)
                         && (n.NgayKetThuc == null || n.NgayKetThuc >= today))
                .ToListAsync();

            int added = 0, skipped = 0;
            foreach (var laborer in activeLaborers)
            {
                bool exists = await _context.ChamCongs
                    .AnyAsync(c => c.NhanCongId == laborer.Id && c.Ngay.Date == today);

                if (!exists)
                {
                    _context.ChamCongs.Add(new ChamCong
                    {
                        NhanCongId = laborer.Id,
                        Ngay = today,
                        SoCong = 1.0,
                        GioDen = new TimeSpan(8, 0, 0),
                        GioVe = new TimeSpan(17, 0, 0)
                    });
                    added++;
                }
                else
                {
                    skipped++;
                }
            }

            if (added > 0)
                await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"✅ Chấm công tự động hoàn tất! Đã thêm {added} bản ghi mới" +
                                         (skipped > 0 ? $", bỏ qua {skipped} (đã chấm)" : "") + ".";
            return RedirectToAction(nameof(Index));
        }

        // GET: ChamCongs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var chamCong = await _context.ChamCongs
                .Include(c => c.NhanCong)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chamCong == null) return NotFound();
            return View(chamCong);
        }

        // GET: ChamCongs/Create
        public IActionResult Create()
        {
            ViewData["NhanCongId"] = new SelectList(_context.NhanCongs, "Id", "HoTen");
            return View();
        }

        // POST: ChamCongs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NhanCongId,Ngay,SoCong,GioDen,GioVe")] ChamCong chamCong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chamCong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NhanCongId"] = new SelectList(_context.NhanCongs, "Id", "HoTen", chamCong.NhanCongId);
            return View(chamCong);
        }

        // GET: ChamCongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var chamCong = await _context.ChamCongs.FindAsync(id);
            if (chamCong == null) return NotFound();
            ViewData["NhanCongId"] = new SelectList(_context.NhanCongs, "Id", "HoTen", chamCong.NhanCongId);
            return View(chamCong);
        }

        // POST: ChamCongs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NhanCongId,Ngay,SoCong,GioDen,GioVe")] ChamCong chamCong)
        {
            if (id != chamCong.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chamCong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChamCongExists(chamCong.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NhanCongId"] = new SelectList(_context.NhanCongs, "Id", "HoTen", chamCong.NhanCongId);
            return View(chamCong);
        }

        // GET: ChamCongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var chamCong = await _context.ChamCongs
                .Include(c => c.NhanCong)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chamCong == null) return NotFound();
            return View(chamCong);
        }

        // POST: ChamCongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chamCong = await _context.ChamCongs.FindAsync(id);
            if (chamCong != null)
                _context.ChamCongs.Remove(chamCong);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChamCongExists(int id)
        {
            return _context.ChamCongs.Any(e => e.Id == id);
        }
    }
}
