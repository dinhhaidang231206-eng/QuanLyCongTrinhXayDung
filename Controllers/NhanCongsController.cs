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
    public class NhanCongsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhanCongsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NhanCongs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.NhanCongs.Include(n => n.ChucVu).Include(n => n.GiaiDoan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NhanCongs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var nhanCong = await _context.NhanCongs
                .Include(n => n.ChucVu)
                .Include(n => n.GiaiDoan)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (nhanCong == null) return NotFound();
            return View(nhanCong);
        }

        // GET: NhanCongs/Create
        public IActionResult Create()
        {
            ViewData["ChucVuId"] = new SelectList(_context.ChucVus, "Id", "TenChucVu");
            ViewData["CongTrinhId"] = new SelectList(_context.CongTrinhs, "Id", "TenCongTrinh");
            ViewData["GiaiDoanId"] = new SelectList(new List<GiaiDoan>(), "Id", "TenGiaiDoan");
            ViewData["ThanhVienList"] = new SelectList(
                _context.ThanhViens.Where(t => t.TrangThai).OrderBy(t => t.HoTen),
                "HoTen", "HoTen");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetGiaiDoans(int congTrinhId)
        {
            var giaiDoans = await _context.GiaiDoans
                .Where(g => g.CongTrinhId == congTrinhId)
                .Select(g => new { value = g.Id, text = g.TenGiaiDoan })
                .ToListAsync();
            return Json(giaiDoans);
        }

        // POST: NhanCongs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HoTen,GiaiDoanId,ChucVuId,Luong,NgayBatDau,NgayKetThuc")] NhanCong nhanCong)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhanCong);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChucVuId"] = new SelectList(_context.ChucVus, "Id", "TenChucVu", nhanCong.ChucVuId);
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", nhanCong.GiaiDoanId);
            ViewData["ThanhVienList"] = new SelectList(
                _context.ThanhViens.Where(t => t.TrangThai).OrderBy(t => t.HoTen),
                "HoTen", "HoTen", nhanCong.HoTen);
            return View(nhanCong);
        }

        // GET: NhanCongs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nhanCong = await _context.NhanCongs.FindAsync(id);
            if (nhanCong == null) return NotFound();

            ViewData["ChucVuId"] = new SelectList(_context.ChucVus, "Id", "TenChucVu", nhanCong.ChucVuId);
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", nhanCong.GiaiDoanId);
            ViewData["ThanhVienList"] = new SelectList(
                _context.ThanhViens.Where(t => t.TrangThai).OrderBy(t => t.HoTen),
                "HoTen", "HoTen", nhanCong.HoTen);
            return View(nhanCong);
        }

        // POST: NhanCongs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HoTen,GiaiDoanId,ChucVuId,Luong,NgayBatDau,NgayKetThuc")] NhanCong nhanCong)
        {
            if (id != nhanCong.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanCong);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanCongExists(nhanCong.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChucVuId"] = new SelectList(_context.ChucVus, "Id", "TenChucVu", nhanCong.ChucVuId);
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", nhanCong.GiaiDoanId);
            ViewData["ThanhVienList"] = new SelectList(
                _context.ThanhViens.Where(t => t.TrangThai).OrderBy(t => t.HoTen),
                "HoTen", "HoTen", nhanCong.HoTen);
            return View(nhanCong);
        }

        // GET: NhanCongs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var nhanCong = await _context.NhanCongs
                .Include(n => n.ChucVu)
                .Include(n => n.GiaiDoan)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (nhanCong == null) return NotFound();
            return View(nhanCong);
        }

        // POST: NhanCongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanCong = await _context.NhanCongs.FindAsync(id);
            if (nhanCong != null)
            {
                _context.NhanCongs.Remove(nhanCong);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanCongExists(int id)
        {
            return _context.NhanCongs.Any(e => e.Id == id);
        }
    }
}
