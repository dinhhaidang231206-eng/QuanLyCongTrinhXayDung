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
    public class GiaiDoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GiaiDoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GiaiDoans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GiaiDoans.Include(g => g.CongTrinh);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GiaiDoans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giaiDoan = await _context.GiaiDoans
                .Include(g => g.CongTrinh)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giaiDoan == null)
            {
                return NotFound();
            }

            return View(giaiDoan);
        }

        // GET: GiaiDoans/Create
        public IActionResult Create()
        {
            ViewData["CongTrinhId"] = new SelectList(_context.CongTrinhs, "Id", "TenCongTrinh");
            return View();
        }

        // POST: GiaiDoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenGiaiDoan,CongTrinhId,ThoiGianBatDau,ThoiGianKetThuc,ThoiGianHoanThanh,TrangThai,ChiPhiDuKien,ChiPhiThucTe")] GiaiDoan giaiDoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giaiDoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CongTrinhId"] = new SelectList(_context.CongTrinhs, "Id", "TenCongTrinh", giaiDoan.CongTrinhId);
            return View(giaiDoan);
        }

        // GET: GiaiDoans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giaiDoan = await _context.GiaiDoans.FindAsync(id);
            if (giaiDoan == null)
            {
                return NotFound();
            }
            ViewData["CongTrinhId"] = new SelectList(_context.CongTrinhs, "Id", "TenCongTrinh", giaiDoan.CongTrinhId);
            return View(giaiDoan);
        }

        // POST: GiaiDoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenGiaiDoan,CongTrinhId,ThoiGianBatDau,ThoiGianKetThuc,ThoiGianHoanThanh,TrangThai,ChiPhiDuKien,ChiPhiThucTe")] GiaiDoan giaiDoan)
        {
            if (id != giaiDoan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giaiDoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiaiDoanExists(giaiDoan.Id))
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
            ViewData["CongTrinhId"] = new SelectList(_context.CongTrinhs, "Id", "TenCongTrinh", giaiDoan.CongTrinhId);
            return View(giaiDoan);
        }

        // GET: GiaiDoans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giaiDoan = await _context.GiaiDoans
                .Include(g => g.CongTrinh)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giaiDoan == null)
            {
                return NotFound();
            }

            return View(giaiDoan);
        }

        // POST: GiaiDoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giaiDoan = await _context.GiaiDoans.FindAsync(id);
            if (giaiDoan != null)
            {
                _context.GiaiDoans.Remove(giaiDoan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiaiDoanExists(int id)
        {
            return _context.GiaiDoans.Any(e => e.Id == id);
        }
    }
}

