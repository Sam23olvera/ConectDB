namespace ConectDB.Models
{
    public class Respuesta
    {
        public bool success { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        //public List<Rutas> data { get; set; }
        public List<object>? data { get; set; }
        public int total { get; set; }
    }

}
