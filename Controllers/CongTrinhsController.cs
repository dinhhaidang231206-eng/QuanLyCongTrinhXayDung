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
    public class CongTrinhsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CongTrinhsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CongTrinhs
        public async Task<IActionResult> Index()
        {
            return View(await _context.CongTrinhs.ToListAsync());
        }

        // GET: CongTrinhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congTrinh = await _context.CongTrinhs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (congTrinh == null)
            {
                return NotFound();
            }

            return View(congTrinh);
        }

        // GET: CongTrinhs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CongTrinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenCongTrinh,DiaDiem,ThoiGianBatDau,ThoiGianKetThuc,ThoiGianHoanThanh,NguoiPhuTrach,NguoiGiamSat,TrangThai,ChiPhiDuKien,ChiPhiThucTe")] CongTrinh congTrinh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(congTrinh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(congTrinh);
        }

        // GET: CongTrinhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congTrinh = await _context.CongTrinhs.FindAsync(id);
            if (congTrinh == null)
            {
                return NotFound();
            }
            return View(congTrinh);
        }

        // POST: CongTrinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenCongTrinh,DiaDiem,ThoiGianBatDau,ThoiGianKetThuc,ThoiGianHoanThanh,NguoiPhuTrach,NguoiGiamSat,TrangThai,ChiPhiDuKien,ChiPhiThucTe")] CongTrinh congTrinh)
        {
            if (id != congTrinh.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(congTrinh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CongTrinhExists(congTrinh.Id))
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
            return View(congTrinh);
        }

        // GET: CongTrinhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var congTrinh = await _context.CongTrinhs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (congTrinh == null)
            {
                return NotFound();
            }

            return View(congTrinh);
        }

        // POST: CongTrinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var congTrinh = await _context.CongTrinhs.FindAsync(id);
            if (congTrinh != null)
            {
                _context.CongTrinhs.Remove(congTrinh);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CongTrinhExists(int id)
        {
            return _context.CongTrinhs.Any(e => e.Id == id);
        }
    }
}

