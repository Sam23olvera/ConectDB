namespace ConectDB.Models
{
    public class DatoTum
    {
        public List<TCAND>? TCAND { get; set; }
        public List<Rutum>? Ruta { get; set; }
    }

    public class Mod_Rutas
    {
        public bool success { get; set; }
        public int status { get; set; }
        public string? message { get; set; }
        public List<DatoTum>? data { get; set; }
        public int total { get; set; }
    }

    public class Rutum
    {
        public int ClaveRuta { get; set; }
        public string? Descripcion { get; set; }
    }

    public class TCAND
    {
        public int ClaveTipoCandidato { get; set; }
        public string? Descripcion { get; set; }
    }


}
