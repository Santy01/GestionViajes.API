using System.ComponentModel.DataAnnotations;

namespace GestionViajes.API.DTOs
{
    public class ReservaDto
    {
        public int ReservaId { get; set; }
        public int TuristaId { get; set; }
        public int DestinoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int CantidadPersonas { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class ReservaCreateDto
    {
        [Required]
        public int TuristaId { get; set; }
        [Required]
        public int DestinoId { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Range(1, int.MaxValue)]
        public int CantidadPersonas { get; set; } = 1;
    }

    public class ReservaUpdateDto
    {
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Range(1, int.MaxValue)]
        public int CantidadPersonas { get; set; } = 1;
    }
}