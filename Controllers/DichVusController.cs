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
    public class DichVusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DichVusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DichVus
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DichVus.Include(d => d.GiaiDoan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DichVus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .Include(d => d.GiaiDoan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // GET: DichVus/Create
        public IActionResult Create()
        {
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan");
            var dichVu = new DichVu
            {
                TrangThai = TrangThaiChung.DangThucHien,
                LoaiDichVu = LoaiDichVu.VanChuyen, // example default
                ThoiGianBatDau = DateTime.Today,
                SoLuong = 1
            };
            return View(dichVu);
        }

        // POST: DichVus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LoaiDichVu,LoaiDichVuKhac,MoTa,GiaiDoanId,NhaCungCap,DonGia,SoLuong,ThoiGianBatDau,ThoiGianKetThuc,TrangThai,MucDichSuDung")] DichVu dichVu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", dichVu.GiaiDoanId);
            return View(dichVu);
        }

        // GET: DichVus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null)
            {
                return NotFound();
            }
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", dichVu.GiaiDoanId);
            return View(dichVu);
        }

        // POST: DichVus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LoaiDichVu,LoaiDichVuKhac,MoTa,GiaiDoanId,NhaCungCap,DonGia,SoLuong,ThoiGianBatDau,ThoiGianKetThuc,TrangThai,MucDichSuDung")] DichVu dichVu)
        {
            if (id != dichVu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dichVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DichVuExists(dichVu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", dichVu.GiaiDoanId);
            return View(dichVu);
        }

        // GET: DichVus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dichVu = await _context.DichVus
                .Include(d => d.GiaiDoan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dichVu == null)
            {
                return NotFound();
            }

            return View(dichVu);
        }

        // POST: DichVus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu != null)
            {
                _context.DichVus.Remove(dichVu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DichVuExists(int id)
        {
            return _context.DichVus.Any(e => e.Id == id);
        }
    }
}

