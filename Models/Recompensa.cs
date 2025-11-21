using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParqueoUPDS.Models
{
    public class Recompensa
    {
        public int Id { get; set; }

        [Required]
        public int VehiculoId { get; set; }

        [ForeignKey("VehiculoId")]
        public Vehiculo Vehiculo { get; set; } = null!;

        [Required]
        public DateTime FechaEntrega { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoPremio { get; set; } = null!;

        public decimal? HorasAcumuladasAlMomento { get; set; }
    }
}
