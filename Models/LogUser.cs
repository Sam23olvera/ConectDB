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
            public string? usr { get; set; }
            public string? pwd { get; set; }
            public int idempresa { get; set; }
        }
    }
}