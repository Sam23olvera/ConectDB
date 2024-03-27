using System.ComponentModel.DataAnnotations;

namespace ConectDB.Models
{
    public class Reclutador
    {
        public int ClaveReclutador { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El campo Activo es obligatorio")]
        public bool? Activo { get; set; }
        public string? Telefono { get; set; }
        public string? Telefonodos { get; set; }
        public string? user { get; set; }
        public string? contra { get; set; }
        public int cveEmp { get; set; }
    }
}