namespace ConectDB.Models
{
    public class ItineViajeSPM
    {
        public List<DetalleViaj>? ItinerarioViajesSPM { get; set; }
        public List<Incidencia>? Incidencias {  get; set; } 
        public string? NR { get; set; }
        public int cvruta { get; set; }
        public DateTime FeSel { get; set; }
        public string? message { get; set; }
    }
    public class DetalleViaj
    {
        public int CVR { get; set; }
        public int SEC { get; set; }
        public string? DIR { get; set; }
        public string? NUC { get; set; }
        public string? CAN { get; set; }
        public string? A { get; set; }
        public string? INCIDENCIA { get; set; }
        public DateTime LLP { get; set; }
        public DateTime FEG { get; set; }
        public bool CLL { get; set; }
        public DateTime SAP { get; set; }
        public DateTime FSG { get; set; }
        public bool CSA { get; set; }
        public DateTime ETA { get; set; }
        public DateTime ESP { get; set; }
        public DateTime ESR { get; set; }
        public DateTime TRS { get; set; }
        public DateTime TRR { get; set; }
        public DateTime DEM { get; set; }
    }
    public class Incidencia
    {
        public string? Codigo { get; set; }
    }

}
