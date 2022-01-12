using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class LiquidadorObjetoGenerico
    {
    }

    public class LiquidadorOwners
    {
        public string FullNombre { get; set; }
        public string Nombre { get; set; }
        public string NumSuite { get; set; }
        public string NumEscritura { get; set; }
        public int IdOwner { get; set; }
        public int IdSuite { get; set; }
        public string NumIdentificacion { get; set; }
        public string NomConcepto { get; set; }

    }

    public class LiquidadorRegla
    {
        public int IdConcepto { get; set; }
        public int NumDecimales { get; set; }
        public string NombreConcepto { get; set; }
        public string Regla { get; set; }
        public int Orden { get; set; }
        public List<LiquidadorReglaConceptos> ListaConceptos { get; set; }
    }

    public class LiquidadorReglaConceptos
    {
        public int? IdVariable { get; set; }
        public int? IdConcepto { get; set; }
        public string NomClass { get; set; }
        public string Operador { get; set; }
        public string NomVariable { get; set; }
    }

    public class LiquidadorValorVariable
    {
        public int IdOwner { get; set; }
        public int IdSuit { get; set; }
        public int IdVariable { get; set; }
        public double Valor { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }

    public class LiquidacionHotel
    {
        public int IdConcepto { get; set; }
        public int IdHotel { get; set; }
        public string Regla { get; set; }
        public double Valor { get; set; }
        public DateTime FechaPeriodoLiquidado { get; set; }
    }

    public class LiquidacionProp
    {
        public int IdHotel { get; set; }
        public int IdSuit { get; set; }
        public int IdPropietario { get; set; }
        public DateTime FechaPeriodoLiquidado { get; set; }
        public List<LiquidacionConceptoProp> ListaConceptos { get; set; }
    }

    public class LiquidacionConceptoProp
    {
        public int IdConcepto { get; set; }
        public string Regla { get; set; }
        public double Valor { get; set; }
    }
}
