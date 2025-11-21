using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParqueoUPDS.Models
{
    public class SesionParqueo
    {
        public int Id { get; set; }

        [Required]
        public int VehiculoId { get; set; }

        [ForeignKey("VehiculoId")]
        public Vehiculo Vehiculo { get; set; } = null!;

        [Required]
        public DateTime HoraEntrada { get; set; }

        public DateTime? HoraSalida { get; set; }

        public decimal? DuracionHoras { get; set; }

        [Required]
        [StringLength(10)]
        public string Estado { get; set; } = null!; // "Dentro" o "Fuera"
    }
}
