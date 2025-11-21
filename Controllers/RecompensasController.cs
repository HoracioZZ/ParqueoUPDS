using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueoUPDS.Data;
using ParqueoUPDS.Models;

namespace ParqueoUPDS.Controllers
{
    public class RecompensasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecompensasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var recompensas = await _context.Recompensas
                .Include(r => r.Vehiculo)
                .OrderByDescending(r => r.FechaEntrega)
                .ToListAsync();

            return View(recompensas);
        }

        public async Task<IActionResult> Details(int id)
        {
            var recompensa = await _context.Recompensas
                .Include(r => r.Vehiculo)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recompensa == null)
                return NotFound();

            return View(recompensa);
        }
    }
}
