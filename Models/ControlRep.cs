namespace ConectDB.Models
{
    public class ControlRep
    {
        public List<ControlRepara>? Solicitudes { get; set; }
        public List<TBCATTipoFalla>? TBCAT_TipoFalla { get; set; }
    }
    public class ControlRepara 
    {
        public int ClaveControlReparaciones { get; set; }
        public int ClaveEmpresa { get; set; }
        public string? Empresa { get; set; }
        public DateTime InicioReporte { get; set; }
        public int ClaveUnidad { get; set; }
        public int Numero { get; set; }
        public string? Alias { get; set; }
        public int ClaveOperador { get; set; }
        public string? Nombre { get; set; }
        public string? NumOP { get; set; }
        public string? Tel_Operador { get; set; }
        public int CveRemolque { get; set; }
        public string? Remolque1 { get; set; }
        public int CveRemolque2 { get; set; }
        public string? Remolque2 { get; set; }
        public int CveRuta { get; set; }
        public string? Ruta { get; set; }
        public string? TramoCarretero { get; set; }
        public int ClaveTipoCarga { get; set; }
        public string? TipoCarga { get; set; }
        public int ClaveTipoFalla { get; set; }
        public string? TipoFalla { get; set; }
        public int ClaveTipoApoyo { get; set; }
        public string? TipoApoyo { get; set; }
        public int ClaveTipoEquipo { get; set; }
        public string? TipoEquipo { get; set; }
        public int IdUsuario { get; set; }
        public string? UsuarioIni { get; set; }
        public string? UbicacionReportada { get; set; }
        public string? UltimaPosicionGps { get; set; }
        public DateTime FechaUltPosicion { get; set; }
        public string? ObsOperacion { get; set; }
        public int ClaveTipoFallaReal { get; set; }
        public string? TipoFallaReal { get; set; }
        public string? DescripcionFallaReal { get; set; }
        public string? ObsMantenimiento { get; set; }
        public DateTime FinReporte { get; set; }
        public int IdUsuarioFin { get; set; }
        public string? UsuarioFin { get; set; }
    }
}
