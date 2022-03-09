using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    [Serializable()]
    public class ObjetoGenerico
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdCuentaContable { get; set; }
        public int IdCuentaContable2 { get; set; }
        public int IdValorVariableSuit { get; set; }
        public int IdVariableSuite { get; set; }
        public int IdReserva { get; set; }
        public int IdSuit { get; set; }
        public int IdHotel { get; set; }
        public int IdCiudad { get; set; }
        public int IdDepto { get; set; }
        public int IdPropietario { get; set; }
        public int IdBanco { get; set; }
        public int IdSuitPropietario { get; set; }
        public int IdPerfil { get; set; }
        public int NumEstadias { get; set; }
        public int NumAdultos { get; set; }
        public int NumNinos { get; set; }
        public int? IdVariable { get; set; }
        public int? IdVariableCondicion { get; set; }
        public int IdValorVariable { get; set; }
        public int? IdConcepto { get; set; }
        public int IdVariableConcepto { get; set; }
        public int Orden { get; set; }
        public int IdConceptoPadre { get; set; }
        public int NivelConcepto { get; set; }
        public int IdCentroCosto { get; set; }
        public int IdCentroCosto_Hotel { get; set; }
        public int IdTipoCuentaContable { get; set; }
        public int IdInformacionEstadistica { get; set; }
        public int IdHistorialInformacionEstadistica { get; set; }
        public int IdCierre { get; set; }
        public int NumDecimales { get; set; }
        public int IdReporteDetalle { get; set; }
        public int IdGrupo { get; set; }
        public int IdReporteGrupoDetalle { get; set; }
        public int OrdenConcepto { get; set; }
        public int OrdenEjecucion { get; set; }

        public string Operador { get; set; }
        public string NomClass { get; set; }
        public string NombrePerfil { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public string PrimeroNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string TipoPersona { get; set; }
        public string NombreCiudad { get; set; }
        public string NombreHotel { get; set; }
        public string NombreBanco { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreExtracto { get; set; }
        public string Apellido { get; set; }
        public string Nit { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Correo2 { get; set; }
        public string Correo3 { get; set; }
        public string Login { get; set; }
        public string NumIdentificacion { get; set; }
        public string NumCuenta { get; set; }
        public string NumSuit { get; set; }
        public string Pass { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Telefono3 { get; set; }
        public string Titular { get; set; }
        public string TipoCuenta { get; set; }
        public string NombreVariable { get; set; }
        public string NombreSuit { get; set; }
        public string NombreConcepto { get; set; }
        public string Regla { get; set; }
        public string Tipo { get; set; }
        public string RutaLogo { get; set; }
        public string NombreContacto { get; set; }
        public string TelContacto { get; set; }
        public string CorreoContacto { get; set; }
        public string Codigo { get; set; }
        public string NombreCuentaContable { get; set; }
        public string NombreCentroCosto { get; set; }
        public string NombreTipoCuentaContable { get; set; }
        public string EncabezadoDocCruce { get; set; }
        public string CodigoCuentaContable { get; set; }
        public string NaturalezaCuenta { get; set; }
        public string CodigoTercero { get; set; }
        public string RegistroNotaria { get; set; }
        public string UnidadNegocioHotel { get; set; }
        public string UnidadNegocioCuentaContable { get; set; }
        public string NumEscritura { get; set; }
        public string Periodo { get; set; }
        public string TipoDocumento { get; set; }
        public string ValorEstadistica { get; set; }
        public string Estado { get; set; }
        public string Condicion { get; set; }
        public string TipoVariableCondicion { get; set; }
        public string UnidadNegocio { get; set; }
        public string Sufijo { get; set; }
        public string Succes { get; set; }
        public string Error { get; set; }
        public string ErrorExeption { get; set; }

        public bool EsRetenedor { get; set; }
        public bool Activo { get; set; }
        public bool Cambio { get; set; }
        public bool EsLiquidacionHotel { get; set; }
        public bool EsCentroCostoVariable { get; set; }
        public bool EsTerceroVariable { get; set; }
        public bool IncluyeCeCo { get; set; }
        public bool EsVarAcumulada { get; set; }
        public bool EsValidacion { get; set; }
        public bool EsConSegundaCuenta { get; set; }
        public bool Ok { get; set; }

        public double PorcentajeParticipacion { get; set; }
        public double Valor { get; set; }
        public double ValorCondicion { get; set; }
        public double ValorSuite { get; set; }


        public DateTime Fecha { get; set; }
        public DateTime FechaLlegada { get; set; }
        public DateTime FechaSalida { get; set; }

        public short ValMax { get; set; }
        public short ValorAcumulado { get; set; }
        public short Anio { get; set; }
        public short TipoValidacion { get; set; }

        public List<ObjetoGenerico> ListaVariables { get; set; }
        public List<string> ListaConceptos { get; set; }
        public List<ObjetoGenerico> ListaSuite { get; set; }
    }

    public class Response
    {
        public Response()
        {
            this.Ok = true;
        }
        public bool Ok { get; set; }
        public short TipoValidacion { get; set; }
        public List<ObjetoGenerico> Lista { get; set; }

        private string Error_;
        public string Error
        {
            get
            {
                return Error_;
            }
            set
            {
                Error_ = value;
                this.Ok = (value != string.Empty) ? false : true;
            }
        }
    }
}
