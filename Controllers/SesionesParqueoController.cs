using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueoUPDS.Data;
using ParqueoUPDS.Models;

namespace ParqueoUPDS.Controllers
{
    public class SesionesParqueoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SesionesParqueoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTADO
        public async Task<IActionResult> Index()
        {
            var sesiones = await _context.SesionesParqueo
                .Include(s => s.Vehiculo)
                .OrderByDescending(s => s.HoraEntrada)
                .ToListAsync();

            return View(sesiones);
        }

        // REGISTRAR ENTRADA
        public IActionResult RegistrarEntrada()
        {
            ViewBag.Vehiculos = _context.Vehiculos.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarEntrada(int vehiculoId)
        {
            var sesion = new SesionParqueo
            {
                VehiculoId = vehiculoId,
                HoraEntrada = DateTime.UtcNow, // <-- ahora usamos UTC
                Estado = "Dentro"
            };

            _context.SesionesParqueo.Add(sesion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // REGISTRAR SALIDA
        public async Task<IActionResult> RegistrarSalida(int id)
        {
            var sesion = await _context.SesionesParqueo
                .Include(s => s.Vehiculo)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sesion == null || sesion.Estado == "Fuera")
                return NotFound();

            sesion.HoraSalida = DateTime.UtcNow; // <-- ahora usamos UTC
            sesion.DuracionHoras = (decimal)(sesion.HoraSalida.Value - sesion.HoraEntrada).TotalHours;
            sesion.Estado = "Fuera";

            // **Acumular horas**
            sesion.Vehiculo.HorasAcumuladas += sesion.DuracionHoras ?? 0;

            // **Registrar recompensa si pasó las 10 horas**
            if (sesion.Vehiculo.HorasAcumuladas >= 10)
            {
                _context.Recompensas.Add(new Recompensa
                {
                    VehiculoId = sesion.VehiculoId,
                    FechaEntrega = DateTime.UtcNow, // <-- ahora usamos UTC
                    TipoPremio = "Lavado Gratis",
                    HorasAcumuladasAlMomento = sesion.Vehiculo.HorasAcumuladas
                });

                // Reset de horas acumuladas
                sesion.Vehiculo.HorasAcumuladas = 0;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
