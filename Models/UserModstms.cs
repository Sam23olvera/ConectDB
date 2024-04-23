namespace ConectDB.Models
{
    public class UserModstms
    {
        public int idsist { get; set; }
        public int idmod { get; set; }
        public string? nommod { get; set; }
        public int secu { get; set; }
        public List<UserSubmod>? submods { get; set; }
    }
    public class UserSubmod
    {
        public int idmod { get; set; }
        public int idsub { get; set; }
        public string? nomsub { get; set; }
        public int secu { get; set; }
    }
}