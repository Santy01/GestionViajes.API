using System.ComponentModel.DataAnnotations;

namespace GestionViajes.API.DTOs
{
    public class DestinoDto
    {
        public int DestinoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Costo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }

    public class DestinoCreateDto
    {
        [Required(ErrorMessage = "El nombre del destino es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El país es obligatorio")]
        [StringLength(50, ErrorMessage = "El país no puede exceder los 50 caracteres")]
        public string Pais { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El costo es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        public decimal Costo { get; set; }
    }

    public class DestinoUpdateDto
    {
        [Required(ErrorMessage = "El nombre del destino es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El país es obligatorio")]
        [StringLength(50, ErrorMessage = "El país no puede exceder los 50 caracteres")]
        public string Pais { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El costo es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
        public decimal Costo { get; set; }
    }
}