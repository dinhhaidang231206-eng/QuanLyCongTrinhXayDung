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
    public class VatLieusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VatLieusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VatLieus
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.VatLieus.Include(v => v.GiaiDoan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VatLieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vatLieu = await _context.VatLieus
                .Include(v => v.GiaiDoan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vatLieu == null)
            {
                return NotFound();
            }

            return View(vatLieu);
        }

        // GET: VatLieus/Create
        public IActionResult Create()
        {
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan");
            return View();
        }

        // POST: VatLieus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenVatLieu,GiaiDoanId,SoLuong,DonGia")] VatLieu vatLieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vatLieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", vatLieu.GiaiDoanId);
            return View(vatLieu);
        }

        // GET: VatLieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vatLieu = await _context.VatLieus.FindAsync(id);
            if (vatLieu == null)
            {
                return NotFound();
            }
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", vatLieu.GiaiDoanId);
            return View(vatLieu);
        }

        // POST: VatLieus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenVatLieu,GiaiDoanId,SoLuong,DonGia")] VatLieu vatLieu)
        {
            if (id != vatLieu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vatLieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VatLieuExists(vatLieu.Id))
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
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", vatLieu.GiaiDoanId);
            return View(vatLieu);
        }

        // GET: VatLieus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vatLieu = await _context.VatLieus
                .Include(v => v.GiaiDoan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vatLieu == null)
            {
                return NotFound();
            }

            return View(vatLieu);
        }

        // POST: VatLieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vatLieu = await _context.VatLieus.FindAsync(id);
            if (vatLieu != null)
            {
                _context.VatLieus.Remove(vatLieu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VatLieuExists(int id)
        {
            return _context.VatLieus.Any(e => e.Id == id);
        }
    }
}

