namespace ConectDB.Models
{
    public class ViajesSep
    {
        public List<ViajesSepome> ViajesSepomex { get; set; }
        public DateTime Date { get; set; }
        public int cvruta { get; set; }
        public string? NR { get; set; }
    }
    public class ViajesSepome
    {
        public int CV { get; set; }
        public string? FO { get; set; }
        public string? UN { get; set; }
        public DateTime FV { get; set; }
        public int NR { get; set; }
        public string? RU { get; set; }
        public int ET { get; set; }
        public string? OP { get; set; }
    }
}
