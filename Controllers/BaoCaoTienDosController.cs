using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using QuanLyCongTrinhXayDung.Data;
using QuanLyCongTrinhXayDung.Models;

namespace QuanLyCongTrinhXayDung.Controllers
{
    [Authorize]
    public class BaoCaoTienDosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BaoCaoTienDosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: BaoCaoTienDos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BaoCaoTienDos
                .Include(b => b.GiaiDoan)
                .Include(b => b.HinhAnhs)
                .Include(b => b.NguoiBaoCao);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BaoCaoTienDos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baoCaoTienDo = await _context.BaoCaoTienDos
                .Include(b => b.GiaiDoan)
                .Include(b => b.HinhAnhs)
                .Include(b => b.NguoiBaoCao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baoCaoTienDo == null)
            {
                return NotFound();
            }

            return View(baoCaoTienDo);
        }

        // GET: BaoCaoTienDos/Create
        public IActionResult Create()
        {
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan");
            
            // Only Admins get to pick who the reporter is. 
            // Regular members will have it set automatically.
            if (User.IsInRole("Admin") || User.IsInRole("Quản trị viên"))
            {
                ViewData["ThanhVienId"] = new SelectList(_context.ThanhViens, "Id", "HoTen");
            }
            
            return View();
        }

        // POST: BaoCaoTienDos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GiaiDoanId,NgayBaoCao,TieuDe,NoiDung,UuTien,ThanhVienId")] BaoCaoTienDo baoCaoTienDo, List<IFormFile> files)
        {
            // Auto-assign reporter for non-admin users
            if (!(User.IsInRole("Admin") || User.IsInRole("Quản trị viên")))
            {
                var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(currentUserIdStr, out int userId))
                {
                    baoCaoTienDo.ThanhVienId = userId;
                    // Clear validation error for ThanhVienId since we set it manually
                    ModelState.Remove("ThanhVienId");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(baoCaoTienDo);
                await _context.SaveChangesAsync();

                if (files != null && files.Count > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "baocaotiendo");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            var hinhAnh = new HinhAnhBaoCao
                            {
                                BaoCaoTienDoId = baoCaoTienDo.Id,
                                DuongDan = "/uploads/baocaotiendo/" + uniqueFileName
                            };
                            _context.HinhAnhBaoCaos.Add(hinhAnh);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", baoCaoTienDo.GiaiDoanId);
            ViewData["ThanhVienId"] = new SelectList(_context.ThanhViens, "Id", "HoTen", baoCaoTienDo.ThanhVienId);
            return View(baoCaoTienDo);
        }

        // GET: BaoCaoTienDos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baoCaoTienDo = await _context.BaoCaoTienDos
                .Include(b => b.HinhAnhs)
                .Include(b => b.NguoiBaoCao)
                .FirstOrDefaultAsync(b => b.Id == id);
            
            if (baoCaoTienDo == null)
            {
                return NotFound();
            }
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", baoCaoTienDo.GiaiDoanId);
            ViewData["ThanhVienId"] = new SelectList(_context.ThanhViens, "Id", "HoTen", baoCaoTienDo.ThanhVienId);
            return View(baoCaoTienDo);
        }

        // POST: BaoCaoTienDos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GiaiDoanId,NgayBaoCao,TieuDe,NoiDung,UuTien,ThanhVienId,TraLoi")] BaoCaoTienDo baoCaoTienDo, List<IFormFile> files)
        {
            if (id != baoCaoTienDo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baoCaoTienDo);
                    
                    if (files != null && files.Count > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "baocaotiendo");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                }

                                var hinhAnh = new HinhAnhBaoCao
                                {
                                    BaoCaoTienDoId = baoCaoTienDo.Id,
                                    DuongDan = "/uploads/baocaotiendo/" + uniqueFileName
                                };
                                _context.HinhAnhBaoCaos.Add(hinhAnh);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaoCaoTienDoExists(baoCaoTienDo.Id))
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
            ViewData["GiaiDoanId"] = new SelectList(_context.GiaiDoans, "Id", "TenGiaiDoan", baoCaoTienDo.GiaiDoanId);
            ViewData["ThanhVienId"] = new SelectList(_context.ThanhViens, "Id", "HoTen", baoCaoTienDo.ThanhVienId);
            return View(baoCaoTienDo);
        }

        // POST: BaoCaoTienDos/DeleteImage/5
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var hinhAnh = await _context.HinhAnhBaoCaos.FindAsync(imageId);
            if (hinhAnh != null)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, hinhAnh.DuongDan.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _context.HinhAnhBaoCaos.Remove(hinhAnh);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        // GET: BaoCaoTienDos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baoCaoTienDo = await _context.BaoCaoTienDos
                .Include(b => b.GiaiDoan)
                .Include(b => b.HinhAnhs)
                .Include(b => b.NguoiBaoCao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baoCaoTienDo == null)
            {
                return NotFound();
            }

            return View(baoCaoTienDo);
        }

        // POST: BaoCaoTienDos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baoCaoTienDo = await _context.BaoCaoTienDos.Include(b => b.HinhAnhs).FirstOrDefaultAsync(b => b.Id == id);
            if (baoCaoTienDo != null)
            {
                // Delete associated files
                foreach(var img in baoCaoTienDo.HinhAnhs)
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, img.DuongDan.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.BaoCaoTienDos.Remove(baoCaoTienDo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: BaoCaoTienDos/Reply/5
        [HttpPost]
        [Authorize(Roles = "Admin,Quản trị viên")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int id, string traLoi)
        {
            var baoCao = await _context.BaoCaoTienDos.FindAsync(id);
            if (baoCao == null)
            {
                return NotFound();
            }

            baoCao.TraLoi = traLoi;
            _context.Update(baoCao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }

        private bool BaoCaoTienDoExists(int id)
        {
            return _context.BaoCaoTienDos.Any(e => e.Id == id);
        }
    }
}
