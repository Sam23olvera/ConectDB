using System.ComponentModel.DataAnnotations;

namespace ConectDB.Models
{
    public class LogUser
    {
        public Data? data { get; set; }
        public Filter? filter { get; set; }

        public class Data
        {
            public int bdCc { get; set; }
            public string? bdSch { get; set; }
            public string? bdSp { get; set; }
        }

        public class Filter
        {
            [Required(ErrorMessage = " Se necesita el usuario")]
            public string? usr { get; set; }
            [Required(ErrorMessage = " Se necesita la contraseña")]
            public string? pwd { get; set; }
            public int idempresa { get; set; }
        }
    }
}