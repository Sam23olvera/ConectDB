namespace ConectDB.Models
{
    public class Rutas
    {
        public List<CatRutum> CatRuta { get; set; }
        public DateTime fecha { get; set; }
        public int cvruta { get; set; }
    }
    public class CatRutum
    {
        public int CR { get; set; }
        public string? NR { get; set; }
    }
}
