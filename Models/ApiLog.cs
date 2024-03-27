namespace ConectDB.Models
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public int idus { get; set; }
        public int estatus { get; set; }
        public int nop_emp { get; set; }
        public string? nom { get; set; }
        public int idrol { get; set; }
        public string? nomrol { get; set; }
        public string? message { get; set; }
        public List<Emp>? empS { get; set; }
    }

    public class Emp
    {
        public int cveEmp { get; set; }
        public string? nomb { get; set; }
        public string? alias { get; set; }
        public string? prefijo { get; set; }
    }

    public class ApiLog
    {
        public string? token { get; set; }
        public int status { get; set; }
        public bool success { get; set; }
        public string? message { get; set; }
        public List<Datum>? data { get; set; }
    }

}