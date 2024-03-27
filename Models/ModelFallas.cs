namespace ConectDB.Models
{
    
    public class ModelFallas
    {
        public List<TBCATOperador> TBCAT_Operador { get; set; }
        public List<TBCATSeleccionEquipo> TBCAT_SeleccionEquipo { get; set; }
        public List<TBCATTipoApoyo> TBCAT_TipoApoyo { get; set; }
        public List<TBCATTipoTicket> TBCAT_TipoTicket { get; set; }
        public List<TBCATTipoClasificacion> TBCAT_TipoClasificacion { get; set; }
        public List<TBCATTipoCarga> TBCAT_TipoCarga { get; set; }
        public List<TBCATTipoFalla> TBCAT_TipoFalla { get; set; }
        public List<TBCATUnidade> TBCAT_Unidades { get; set; }
        public List<TBCATTipoEquipo> TBCAT_TipoEquipo { get; set; }
        public List<TBCATRutum> TBCAT_Ruta { get; set; }
        public List<UltimaPosicion> UltimaPosicion { get; set; }
        
        public List<TBCATRemolque> TBCAT_Remolques { get; set; }
    }
    public class TBCATTipoClasificacion
    {
        public int ClaveTipoClasificacion { get; set; }
        public string Descripcion { get; set; }
    }
    public class TBCATTipoTicket 
    {
        public int ClaveTipoTicket { get; set; }
        public string Descripcion { get; set; }
}
    public class TBCATOperador
    {
        public int ClaveOperador { get; set; }
        public string NumOP { get; set; }
        public string Nombre { get; set; }
    }
    public class TBCATRutum
    {
        public int CveRuta { get; set; }
        public string Nombre { get; set; }
    }

    public class TBCATSeleccionEquipo
    {
        public int ClaveSeleccion { get; set; }
        public string Descripcion { get; set; }
    }

    public class TBCATTipoApoyo
    {
        public int ClaveTipoApoyo { get; set; }
        public string Descripcion { get; set; }
    }

    public class TBCATTipoCarga
    {
        public int ClaveTipoCarga { get; set; }
        public string Descripcion { get; set; }
    }

    public class TBCATTipoEquipo
    {
        public int ClaveTipoEquipo { get; set; }
        public string Descripcion { get; set; }
    }

    public class TBCATTipoFalla
    {
        public int ClaveTipoFalla { get; set; }
        public string Descripcion { get; set; }
    }

    public class TBCATUnidade
    {
        public int ClaveUnidad_Motora { get; set; }
        public int Numero { get; set; }
        public string Alias { get; set; }
        public int ClaveTipoOperacion { get; set; }
        public int ClaveTipoEquipo { get; set; }
        public string TipoEquipo { get; set; }
        public string TipoOperacion { get; set; }
    }
    public class UltimaPosicion
    {
        public string UnitNum { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Position { get; set; }
        public DateTime SendTime { get; set; }
    }

    public class TBCATRemolque
    {
        public int ClaveUnidad_Arrastre { get; set; }
        public string Numero { get; set; }
    }
}
