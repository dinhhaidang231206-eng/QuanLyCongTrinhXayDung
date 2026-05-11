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
    public class MayMocsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MayMocsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MayMocs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MayMocs.Include(m => m.GiaiDoan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MayMocs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mayMoc = await _context.MayMocs
                .Include(m => m.GiaiDoan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mayMoc == null)
            {
                return NotFound();
            }

            return View(mayMoc);
        }

        // GET: MayMocs/Create
        public IActionResult Create()
        {
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan");
            return View();
        }

        // POST: MayMocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenMay,GiaiDoanId,SoLuong,DonGia,ThoiGianBatDau,ThoiGianKetThuc,TrangThai")] MayMoc mayMoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mayMoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", mayMoc.GiaiDoanId);
            return View(mayMoc);
        }

        // GET: MayMocs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mayMoc = await _context.MayMocs.FindAsync(id);
            if (mayMoc == null)
            {
                return NotFound();
            }
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", mayMoc.GiaiDoanId);
            return View(mayMoc);
        }

        // POST: MayMocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenMay,GiaiDoanId,SoLuong,DonGia,ThoiGianBatDau,ThoiGianKetThuc,TrangThai")] MayMoc mayMoc)
        {
            if (id != mayMoc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mayMoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MayMocExists(mayMoc.Id))
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
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", mayMoc.GiaiDoanId);
            return View(mayMoc);
        }

        // GET: MayMocs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mayMoc = await _context.MayMocs
                .Include(m => m.GiaiDoan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mayMoc == null)
            {
                return NotFound();
            }

            return View(mayMoc);
        }

        // POST: MayMocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mayMoc = await _context.MayMocs.FindAsync(id);
            if (mayMoc != null)
            {
                _context.MayMocs.Remove(mayMoc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MayMocExists(int id)
        {
            return _context.MayMocs.Any(e => e.Id == id);
        }
    }
}

