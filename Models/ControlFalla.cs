namespace ConectDB.Models
{

    public class ControlFalla
    {
        private List<Solicitude> zVarcio = new List<Solicitude>();
        private List<TBCATTipoFalla> zVaciTBCATFALLAS = new List<TBCATTipoFalla>();
        private List<TBCATTipoTicket> zVaciTBCATTipoTicket = new List<TBCATTipoTicket>();
        private List<TBCATTipoApoyo> ZVaciTBCATTipApoyo = new List<TBCATTipoApoyo>();
        private List<TBCATTipoClasificacion> zVaciTBCATTipoClasificacion = new List<TBCATTipoClasificacion>();
        private List<TBCATUserAsignaReparacion> zVaciTBCATUserAsignaReparacion = new List<TBCATUserAsignaReparacion>();
        private List<soliGeneral> soliGenerals = new List<soliGeneral>();
        public int? TotalSolicitudes { get; set; }
        public int? status { get; set; }
        public string? message { get; set; }
        public List<soliGeneral> SolicitudesGenerales { get { return soliGenerals; } set { soliGenerals = value; } }
        public List<Solicitude> Solicitudes { get { return zVarcio; } set { zVarcio = value; } }
        public List<TBCATTipoTicket> TBCAT_TipoTicket { get { return zVaciTBCATTipoTicket; } set { zVaciTBCATTipoTicket = value; } }
        public List<TBCATTipoFalla> TBCAT_TipoFalla { get { return zVaciTBCATFALLAS; } set { zVaciTBCATFALLAS = value; } }
        public List<TBCATTipoApoyo> TBCAT_TipoApoyo { get { return ZVaciTBCATTipApoyo; } set { ZVaciTBCATTipApoyo = value; } }
        public List<TBCATTipoClasificacion> TBCAT_TipoClasificacion { get { return zVaciTBCATTipoClasificacion; } set { zVaciTBCATTipoClasificacion = value; } }
        public List<TBCATUserAsignaReparacion> TBCAT_UserAsignaReparacion { get { return zVaciTBCATUserAsignaReparacion; } set { zVaciTBCATUserAsignaReparacion = value; } }
        

    }
    public class archivo 
    {
        public int carpet { get; set; }
        public string rutFile { get; set; }

    }
    public class TBCATUserAsignaReparacion
    {
        public int idusuario { get; set; }
        public string? nombre { get; set; }
        public string? correo { get; set; }
    }
    public class soliGeneral
    {
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public int ClaveEstatus { get; set; }
    }
    public class Solicitude
    {
        private List<archivo> RutArchi = new List<archivo>();
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public int ClaveEstatus { get; set; }
        public string? Estatus { get; set; }
        public int ClaveTipoTicket { get; set; }
        public string? TipoTicket { get; set; }
        public int NumTicket { get; set; }
        public int Unidad { get; set; }
        public string? Alias { get; set; }
        public string? NumOp { get; set; }
        public string? Operador { get; set; }
        public string? Remolque { get; set; }
        public int NumRuta { get; set; }
        public string? Ruta { get; set; }
        public string? TipoCarga { get; set; }
        public string? TipoFalla { get; set; }
        public string? TipoApoyo { get; set; }
        public string? TipoClasificacion { get; set; }
        public int ClaveTipoClasificacion { get; set; }
        public string? TipoEquipo { get; set; }
        public DateTime? FechaHoraVencimiento { get; set; }
        public string? MotivoVencimiento { get; set; }
        public string? Tel_Operador { get; set; }
        public string? Usuario { get; set; }
        public string? UsuarioCreacion { get; set; }
        public int CveUsuarioAsignado { get; set; }
        public string? UbicacionReportada { get; set; }
        public DateTime? FechaUltPosicion { get; set; }
        public string? UltimaPosicionGps { get; set; }
        public string? TramoCarretero { get; set; }
        public string? ObsOperacion { get; set; }
        public DateTime? FechaEvento { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public bool Diesel { get; set; }
        public bool Grua { get; set; }
        public string? DOT { get; set; }
        public string? MARCA { get; set; }
        public string? MEDIDA { get; set; }
        public int? POSICION { get; set; }
        public int ClaveTipoFalla { get; set; }
        public List<archivo> PathArchivo { get { return RutArchi; } set { RutArchi = value; } }
    }
}
