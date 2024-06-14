namespace ConectDB.Models
{
    public class UsuarioModel
    {
        private string Tok = string.Empty;
        public string Token { get { return Tok; } set { Tok = value; } }
        public string? XT { get; set; }
        public int Status { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int? idsub { get; set; }
        public List<UserData>? Data { get; set; }
        public List<Reclutador>? recluta { get; set; }
        public string? usuario { get; set; }
        public string? contraseña { get; set; }

    }
    public class UserData
    {
        public int idus { get; set; }
        public int estatus { get; set; }
        public int nop_emp { get; set; }
        public string? nom { get; set; }
        public int idrol { get; set; }
        public string? nomrol { get; set; }
        public string? message { get; set; }
        public List<Empresa>? EmpS { get; set; }
        public List<SistmsModel>? sistms { get; set; }
        public List<UserModstms>? mods { get; set; }
        public List<Nvacc>? nvaccs { get; set; }
        public List<per>? permisos { get; set; } 
        public string? usuario { get; set; }
        public string? contraseña { get; set; }

    }

    public class Empresa
    {
        public int cveEmp { get; set; }
        public string? nomb { get; set; }
        public string? alias { get; set; }
        public string? prefijo { get; set; }
    }
    public class Nvacc
    {
        public int idsub { get; set; }
        public int idopsub { get; set; }
        public string? nomop { get; set; }
    }
    public class per 
    {
        public int idsub { get; set; }
        public int idopsub { get; set; }
        public string? permiso { get; set; }
    }
}