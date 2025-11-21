using System.ComponentModel.DataAnnotations;

namespace ParqueoUPDS.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Placa { get; set; } = null!;

        [StringLength(100)]
        public string? Marca { get; set; }

        [StringLength(100)]
        public string? Modelo { get; set; }

        public decimal HorasAcumuladas { get; set; }

        // RowVersion para manejo de concurrencia
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        // Relación: un vehículo tiene muchas sesiones
        public ICollection<SesionParqueo> Sesiones { get; set; } = new List<SesionParqueo>();

        // Relación: un vehículo puede tener múltiples recompensas
        public ICollection<Recompensa> Recompensas { get; set; } = new List<Recompensa>();
    }
}
