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
    public class ChucVusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChucVusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChucVus
        public async Task<IActionResult> Index()
        {
            var chucVus = await _context.ChucVus
                .Include(c => c.ThanhViens)
                .Include(c => c.NhanCongs)
                .ToListAsync();

            // Đếm thành viên và nhân công cho mỗi chức vụ
            ViewBag.ThanhVienCounts = chucVus.ToDictionary(c => c.Id, c => c.ThanhViens.Count);
            ViewBag.NhanCongCounts = chucVus.ToDictionary(c => c.Id, c => c.NhanCongs.Count);

            return View(chucVus);
        }

        // GET: ChucVus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var chucVu = await _context.ChucVus.FirstOrDefaultAsync(m => m.Id == id);
            if (chucVu == null) return NotFound();
            return View(chucVu);
        }

        // GET: ChucVus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChucVus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenChucVu,MoTa")] ChucVu chucVu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chucVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chucVu);
        }

        // GET: ChucVus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var chucVu = await _context.ChucVus.FindAsync(id);
            if (chucVu == null) return NotFound();
            return View(chucVu);
        }

        // POST: ChucVus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenChucVu,MoTa")] ChucVu chucVu)
        {
            if (id != chucVu.Id) return NotFound();

            // Lấy tên chức vụ gốc từ DB, không cho phép thay đổi tên
            var original = await _context.ChucVus.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (original != null)
            {
                chucVu.TenChucVu = original.TenChucVu;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chucVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChucVuExists(chucVu.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chucVu);
        }

        // GET: ChucVus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var chucVu = await _context.ChucVus.FirstOrDefaultAsync(m => m.Id == id);
            if (chucVu == null) return NotFound();
            return View(chucVu);
        }

        // POST: ChucVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chucVu = await _context.ChucVus.FindAsync(id);
            if (chucVu != null)
            {
                _context.ChucVus.Remove(chucVu);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChucVuExists(int id)
        {
            return _context.ChucVus.Any(e => e.Id == id);
        }
    }
}
