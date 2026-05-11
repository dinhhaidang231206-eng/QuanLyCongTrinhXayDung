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
    public class ThanhViensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThanhViensController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ThanhViens
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ThanhViens.Include(t => t.ChucVu);
            return View(await applicationDbContext.ToListAsync());
        }

        // POST: ThanhViens/CapAdmin/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapAdmin(int id)
        {
            var thanhVien = await _context.ThanhViens.FindAsync(id);
            if (thanhVien == null) return NotFound();

            // Tìm hoặc tạo chức vụ Admin
            var adminRole = await _context.ChucVus.FirstOrDefaultAsync(c => c.TenChucVu == "Admin");
            if (adminRole == null)
            {
                adminRole = new ChucVu { TenChucVu = "Admin", MoTa = "Quản trị viên hệ thống" };
                _context.ChucVus.Add(adminRole);
                await _context.SaveChangesAsync();
            }

            thanhVien.ChucVuId = adminRole.Id;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã cấp quyền Admin cho {thanhVien.HoTen}!";
            return RedirectToAction(nameof(Index));
        }

        // POST: ThanhViens/ThuHoiAdmin/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThuHoiAdmin(int id)
        {
            var thanhVien = await _context.ThanhViens.FindAsync(id);
            if (thanhVien == null) return NotFound();

            // Tìm chức vụ thành viên (không phải Admin)
            var memberRole = await _context.ChucVus.FirstOrDefaultAsync(c => c.TenChucVu != "Admin" && c.TenChucVu != "Quản trị viên");
            if (memberRole == null)
            {
                memberRole = new ChucVu { TenChucVu = "Thành viên", MoTa = "Thành viên thông thường" };
                _context.ChucVus.Add(memberRole);
                await _context.SaveChangesAsync();
            }

            thanhVien.ChucVuId = memberRole.Id;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Đã thu hồi quyền Admin của {thanhVien.HoTen}!";
            return RedirectToAction(nameof(Index));
        }

        // GET: ThanhViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var thanhVien = await _context.ThanhViens
                .Include(t => t.ChucVu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thanhVien == null) return NotFound();

            return View(thanhVien);
        }

        // GET: ThanhViens/Create
        public IActionResult Create()
        {
            ViewData["ChucVuId"] = new SelectList(_context.ChucVus, "Id", "TenChucVu");
            return View();
        }

        // POST: ThanhViens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,HoTen,TenDangNhap,MatKhau,ChucVuId,NgayTao,TrangThai")] ThanhVien thanhVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thanhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChucVuId"] = new SelectList(_context.ChucVus, "Id", "TenChucVu", thanhVien.ChucVuId);
            return View(thanhVien);
        }

        // GET: ThanhViens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var thanhVien = await _context.ThanhViens.FindAsync(id);
            if (thanhVien == null) return NotFound();

            ViewData["ChucVuId"] = new SelectList(_context.ChucVus, "Id", "TenChucVu", thanhVien.ChucVuId);
            return View(thanhVien);
        }

        // POST: ThanhViens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,HoTen,TenDangNhap,MatKhau,ChucVuId,NgayTao,TrangThai")] ThanhVien thanhVien)
        {
            if (id != thanhVien.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thanhVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhVienExists(thanhVien.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChucVuId"] = new SelectList(_context.ChucVus, "Id", "TenChucVu", thanhVien.ChucVuId);
            return View(thanhVien);
        }

        // GET: ThanhViens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var thanhVien = await _context.ThanhViens
                .Include(t => t.ChucVu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thanhVien == null) return NotFound();

            return View(thanhVien);
        }

        // POST: ThanhViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thanhVien = await _context.ThanhViens.FindAsync(id);
            if (thanhVien != null)
            {
                _context.ThanhViens.Remove(thanhVien);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThanhVienExists(int id)
        {
            return _context.ThanhViens.Any(e => e.Id == id);
        }
    }
}
