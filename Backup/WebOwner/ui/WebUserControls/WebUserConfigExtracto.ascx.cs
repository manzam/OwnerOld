using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using DM;
using Servicios;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserConfigExtracto : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;

            if (!IsPostBack)
            {
                CargarCombos();
                ddlHotel_SelectedIndexChanged(null, null);
                ddlTipoExtracto_SelectedIndexChanged(null, null);
            }
        }

        #region Metodos

        private void Limpiar()
        {
            imgLogo.ImageUrl = string.Empty;
            txtDescExtracto.Text = string.Empty;
            txtPieExtracto.Text = string.Empty;
        }

        private void CargarCombos()
        {
            HotelBo hotelBoTmp = new HotelBo();
            List<DM.Hotel> listaHotel = new List<DM.Hotel>();

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                listaHotel = hotelBoTmp.VerTodos();
            else
                listaHotel = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

            ddlHotel.DataSource = listaHotel;
            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();            
        }

        private void CargarCombosVariable()
        {
            ConfigurarExtractoBo configExtractoBoTmp = new ConfigurarExtractoBo();
            List<ObjetoGenerico> listaVariable = configExtractoBoTmp.ObtenerListaVariables(int.Parse(ddlHotel.SelectedValue));
           
            listaVariable.Insert(0, new ObjetoGenerico() { Nombre = "Ninguno - Ninguno", IdVariable = -1 });
            //Extracto Uno

            ddlPorcentajeIngresoPropietario.DataSource = listaVariable;
            ddlPorcentajeIngresoPropietario.DataTextField = "Nombre";
            ddlPorcentajeIngresoPropietario.DataValueField = "IdVariable";
            ddlPorcentajeIngresoPropietario.DataBind();

            ddlRetencionH.DataSource = listaVariable;
            ddlRetencionH.DataTextField = "Nombre";
            ddlRetencionH.DataValueField = "IdVariable";
            ddlRetencionH.DataBind();
            ddlRetencionS.DataSource = listaVariable;
            ddlRetencionS.DataTextField = "Nombre";
            ddlRetencionS.DataValueField = "IdVariable";
            ddlRetencionS.DataBind();

            ddlCoeficiente.DataSource = listaVariable;
            ddlCoeficiente.DataTextField = "Nombre";
            ddlCoeficiente.DataValueField = "IdVariable";
            ddlCoeficiente.DataBind();
            ddlParticipacionPropietario.DataSource = listaVariable;
            ddlParticipacionPropietario.DataTextField = "Nombre";
            ddlParticipacionPropietario.DataValueField = "IdVariable";
            ddlParticipacionPropietario.DataBind();            

            ddlIngresoTotalH.DataSource = listaVariable;
            ddlIngresoTotalH.DataTextField = "Nombre";
            ddlIngresoTotalH.DataValueField = "IdVariable";
            ddlIngresoTotalH.DataBind();
            ddlIngresoTotalS.DataSource = listaVariable;
            ddlIngresoTotalS.DataTextField = "Nombre";
            ddlIngresoTotalS.DataValueField = "IdVariable";
            ddlIngresoTotalS.DataBind();

            ddlTotalCostosH.DataSource = listaVariable;
            ddlTotalCostosH.DataTextField = "Nombre";
            ddlTotalCostosH.DataValueField = "IdVariable";
            ddlTotalCostosH.DataBind();
            ddlTotalCostosS.DataSource = listaVariable;
            ddlTotalCostosS.DataTextField = "Nombre";
            ddlTotalCostosS.DataValueField = "IdVariable";
            ddlTotalCostosS.DataBind();

            ddlTotalGastosH.DataSource = listaVariable;
            ddlTotalGastosH.DataTextField = "Nombre";
            ddlTotalGastosH.DataValueField = "IdVariable";
            ddlTotalGastosH.DataBind();
            ddlTotalGastosS.DataSource = listaVariable;
            ddlTotalGastosS.DataTextField = "Nombre";
            ddlTotalGastosS.DataValueField = "IdVariable";
            ddlTotalGastosS.DataBind();

            ddlIngresosDispoH.DataSource = listaVariable;
            ddlIngresosDispoH.DataTextField = "Nombre";
            ddlIngresosDispoH.DataValueField = "IdVariable";
            ddlIngresosDispoH.DataBind();
            ddlIngresosDispoS.DataSource = listaVariable;
            ddlIngresosDispoS.DataTextField = "Nombre";
            ddlIngresosDispoS.DataValueField = "IdVariable";
            ddlIngresosDispoS.DataBind();

            ddlIngrePropiH.DataSource = listaVariable;
            ddlIngrePropiH.DataTextField = "Nombre";
            ddlIngrePropiH.DataValueField = "IdVariable";
            ddlIngrePropiH.DataBind();
            ddlIngrePropiS.DataSource = listaVariable;
            ddlIngrePropiS.DataTextField = "Nombre";
            ddlIngrePropiS.DataValueField = "IdVariable";
            ddlIngrePropiS.DataBind();

            ddlDescFaraH.DataSource = listaVariable;
            ddlDescFaraH.DataTextField = "Nombre";
            ddlDescFaraH.DataValueField = "IdVariable";
            ddlDescFaraH.DataBind();
            ddlDescFaraS.DataSource = listaVariable;
            ddlDescFaraS.DataTextField = "Nombre";
            ddlDescFaraS.DataValueField = "IdVariable";
            ddlDescFaraS.DataBind();

            ddlDesSegAnoH.DataSource = listaVariable;
            ddlDesSegAnoH.DataTextField = "Nombre";
            ddlDesSegAnoH.DataValueField = "IdVariable";
            ddlDesSegAnoH.DataBind();
            ddlDesSegAnoS.DataSource = listaVariable;
            ddlDesSegAnoS.DataTextField = "Nombre";
            ddlDesSegAnoS.DataValueField = "IdVariable";
            ddlDesSegAnoS.DataBind();

            ddlPartiDistH.DataSource = listaVariable;
            ddlPartiDistH.DataTextField = "Nombre";
            ddlPartiDistH.DataValueField = "IdVariable";
            ddlPartiDistH.DataBind();
            ddlPartiDistS.DataSource = listaVariable;
            ddlPartiDistS.DataTextField = "Nombre";
            ddlPartiDistS.DataValueField = "IdVariable";
            ddlPartiDistS.DataBind();

            ddlRentaPropH.DataSource = listaVariable;
            ddlRentaPropH.DataTextField = "Nombre";
            ddlRentaPropH.DataValueField = "IdVariable";
            ddlRentaPropH.DataBind();
            ddlRentaPropS.DataSource = listaVariable;
            ddlRentaPropS.DataTextField = "Nombre";
            ddlRentaPropS.DataValueField = "IdVariable";
            ddlRentaPropS.DataBind();

            ddlEficienciaOperativaH.DataSource = listaVariable;
            ddlEficienciaOperativaH.DataTextField = "Nombre";
            ddlEficienciaOperativaH.DataValueField = "IdVariable";
            ddlEficienciaOperativaH.DataBind();
            ddlEficienciaOperativaS.DataSource = listaVariable;
            ddlEficienciaOperativaS.DataTextField = "Nombre";
            ddlEficienciaOperativaS.DataValueField = "IdVariable";
            ddlEficienciaOperativaS.DataBind();

            ddlUtilidadOperacional.DataSource = listaVariable;
            ddlUtilidadOperacional.DataTextField = "Nombre";
            ddlUtilidadOperacional.DataValueField = "IdVariable";
            ddlUtilidadOperacional.DataBind();

            // Extracto Dos
            ddlUtilidadOperativaCop.DataSource = listaVariable;
            ddlUtilidadOperativaCop.DataTextField = "Nombre";
            ddlUtilidadOperativaCop.DataValueField = "IdVariable";
            ddlUtilidadOperativaCop.DataBind();
            ddlUtilidadOperativaSuite.DataSource = listaVariable;
            ddlUtilidadOperativaSuite.DataTextField = "Nombre";
            ddlUtilidadOperativaSuite.DataValueField = "IdVariable";
            ddlUtilidadOperativaSuite.DataBind();

            ddlFaraContractualCop.DataSource = listaVariable;
            ddlFaraContractualCop.DataTextField = "Nombre";
            ddlFaraContractualCop.DataValueField = "IdVariable";
            ddlFaraContractualCop.DataBind();
            ddlFaraContractualSuite.DataSource = listaVariable;
            ddlFaraContractualSuite.DataTextField = "Nombre";
            ddlFaraContractualSuite.DataValueField = "IdVariable";
            ddlFaraContractualSuite.DataBind();

            ddlFaraAdicionalCop.DataSource = listaVariable;
            ddlFaraAdicionalCop.DataTextField = "Nombre";
            ddlFaraAdicionalCop.DataValueField = "IdVariable";
            ddlFaraAdicionalCop.DataBind();
            ddlFaraAdicionalSuite.DataSource = listaVariable;
            ddlFaraAdicionalSuite.DataTextField = "Nombre";
            ddlFaraAdicionalSuite.DataValueField = "IdVariable";
            ddlFaraAdicionalSuite.DataBind();

            ddlRetencionFuenteCop.DataSource = listaVariable;
            ddlRetencionFuenteCop.DataTextField = "Nombre";
            ddlRetencionFuenteCop.DataValueField = "IdVariable";
            ddlRetencionFuenteCop.DataBind();
            ddlRetencionFuenteSuite.DataSource = listaVariable;
            ddlRetencionFuenteSuite.DataTextField = "Nombre";
            ddlRetencionFuenteSuite.DataValueField = "IdVariable";
            ddlRetencionFuenteSuite.DataBind();

            ddlFaraAdicional.DataSource = listaVariable;
            ddlFaraAdicional.DataTextField = "Nombre";
            ddlFaraAdicional.DataValueField = "IdVariable";
            ddlFaraAdicional.DataBind();

            ddlNochesVendidas.DataSource = listaVariable;
            ddlNochesVendidas.DataTextField = "Nombre";
            ddlNochesVendidas.DataValueField = "IdVariable";
            ddlNochesVendidas.DataBind();

            ddlOcupacion.DataSource = listaVariable;
            ddlOcupacion.DataTextField = "Nombre";
            ddlOcupacion.DataValueField = "IdVariable";
            ddlOcupacion.DataBind();

            ddlAcumulado.DataSource = listaVariable;
            ddlAcumulado.DataTextField = "Nombre";
            ddlAcumulado.DataValueField = "IdVariable";
            ddlAcumulado.DataBind();

            ddlRenta.DataSource = listaVariable;
            ddlRenta.DataTextField = "Nombre";
            ddlRenta.DataValueField = "IdVariable";
            ddlRenta.DataBind();

            ddlCoeficienteSuite.DataSource = listaVariable;
            ddlCoeficienteSuite.DataTextField = "Nombre";
            ddlCoeficienteSuite.DataValueField = "IdVariable";
            ddlCoeficienteSuite.DataBind();

            ddlCoeficienteParticipacion.DataSource = listaVariable;
            ddlCoeficienteParticipacion.DataTextField = "Nombre";
            ddlCoeficienteParticipacion.DataValueField = "IdVariable";
            ddlCoeficienteParticipacion.DataBind();

            // Extracto tres
            ddlCoeficiente3.DataSource = listaVariable;
            ddlCoeficiente3.DataTextField = "Nombre";
            ddlCoeficiente3.DataValueField = "IdVariable";
            ddlCoeficiente3.DataBind();
            ddlParticipacionPropietario3.DataSource = listaVariable;
            ddlParticipacionPropietario3.DataTextField = "Nombre";
            ddlParticipacionPropietario3.DataValueField = "IdVariable";
            ddlParticipacionPropietario3.DataBind();

            ddlVentaTotalH.DataSource = listaVariable;
            ddlVentaTotalH.DataTextField = "Nombre";
            ddlVentaTotalH.DataValueField = "IdVariable";
            ddlVentaTotalH.DataBind();
            ddlVentaTotalS.DataSource = listaVariable;
            ddlVentaTotalS.DataTextField = "Nombre";
            ddlVentaTotalS.DataValueField = "IdVariable";
            ddlVentaTotalS.DataBind();            

            ddlAlojamientoH.DataSource = listaVariable;
            ddlAlojamientoH.DataTextField = "Nombre";
            ddlAlojamientoH.DataValueField = "IdVariable";
            ddlAlojamientoH.DataBind();
            ddlAlojamientoS.DataSource = listaVariable;
            ddlAlojamientoS.DataTextField = "Nombre";
            ddlAlojamientoS.DataValueField = "IdVariable";
            ddlAlojamientoS.DataBind();

            ddlServiciosCompleH.DataSource = listaVariable;
            ddlServiciosCompleH.DataTextField = "Nombre";
            ddlServiciosCompleH.DataValueField = "IdVariable";
            ddlServiciosCompleH.DataBind();
            ddlServiciosCompleS.DataSource = listaVariable;
            ddlServiciosCompleS.DataTextField = "Nombre";
            ddlServiciosCompleS.DataValueField = "IdVariable";
            ddlServiciosCompleS.DataBind();

            ddlTotalIngresosH.DataSource = listaVariable;
            ddlTotalIngresosH.DataTextField = "Nombre";
            ddlTotalIngresosH.DataValueField = "IdVariable";
            ddlTotalIngresosH.DataBind();
            ddlTotalIngresosS.DataSource = listaVariable;
            ddlTotalIngresosS.DataTextField = "Nombre";
            ddlTotalIngresosS.DataValueField = "IdVariable";
            ddlTotalIngresosS.DataBind();

            ddlRemanenteH.DataSource = listaVariable;
            ddlRemanenteH.DataTextField = "Nombre";
            ddlRemanenteH.DataValueField = "IdVariable";
            ddlRemanenteH.DataBind();
            ddlRemanenteS.DataSource = listaVariable;
            ddlRemanenteS.DataTextField = "Nombre";
            ddlRemanenteS.DataValueField = "IdVariable";
            ddlRemanenteS.DataBind();

            ddlCostosGastosH.DataSource = listaVariable;
            ddlCostosGastosH.DataTextField = "Nombre";
            ddlCostosGastosH.DataValueField = "IdVariable";
            ddlCostosGastosH.DataBind();
            ddlCostosGastosS.DataSource = listaVariable;
            ddlCostosGastosS.DataTextField = "Nombre";
            ddlCostosGastosS.DataValueField = "IdVariable";
            ddlCostosGastosS.DataBind();

            ddlExedenteH.DataSource = listaVariable;
            ddlExedenteH.DataTextField = "Nombre";
            ddlExedenteH.DataValueField = "IdVariable";
            ddlExedenteH.DataBind();
            ddlExedenteS.DataSource = listaVariable;
            ddlExedenteS.DataTextField = "Nombre";
            ddlExedenteS.DataValueField = "IdVariable";
            ddlExedenteS.DataBind();

            ddlNotaCreditoH.DataSource = listaVariable;
            ddlNotaCreditoH.DataTextField = "Nombre";
            ddlNotaCreditoH.DataValueField = "IdVariable";
            ddlNotaCreditoH.DataBind();
            ddlNotaCreditoS.DataSource = listaVariable;
            ddlNotaCreditoS.DataTextField = "Nombre";
            ddlNotaCreditoS.DataValueField = "IdVariable";
            ddlNotaCreditoS.DataBind();

            ddlAdminH.DataSource = listaVariable;
            ddlAdminH.DataTextField = "Nombre";
            ddlAdminH.DataValueField = "IdVariable";
            ddlAdminH.DataBind();
            ddlAdminS.DataSource = listaVariable;
            ddlAdminS.DataTextField = "Nombre";
            ddlAdminS.DataValueField = "IdVariable";
            ddlAdminS.DataBind();

            ddlDescFaraH3.DataSource = listaVariable;
            ddlDescFaraH3.DataTextField = "Nombre";
            ddlDescFaraH3.DataValueField = "IdVariable";
            ddlDescFaraH3.DataBind();
            ddlDescFaraS3.DataSource = listaVariable;
            ddlDescFaraS3.DataTextField = "Nombre";
            ddlDescFaraS3.DataValueField = "IdVariable";
            ddlDescFaraS3.DataBind();

            ddlReteH.DataSource = listaVariable;
            ddlReteH.DataTextField = "Nombre";
            ddlReteH.DataValueField = "IdVariable";
            ddlReteH.DataBind();
            ddlReteS.DataSource = listaVariable;
            ddlReteS.DataTextField = "Nombre";
            ddlReteS.DataValueField = "IdVariable";
            ddlReteS.DataBind();

            ddlCuotaH.DataSource = listaVariable;
            ddlCuotaH.DataTextField = "Nombre";
            ddlCuotaH.DataValueField = "IdVariable";
            ddlCuotaH.DataBind();
            ddlCuotaS.DataSource = listaVariable;
            ddlCuotaS.DataTextField = "Nombre";
            ddlCuotaS.DataValueField = "IdVariable";
            ddlCuotaS.DataBind();

            ddlSubTotalDesH.DataSource = listaVariable;
            ddlSubTotalDesH.DataTextField = "Nombre";
            ddlSubTotalDesH.DataValueField = "IdVariable";
            ddlSubTotalDesH.DataBind();
            ddlSubTotalDesS.DataSource = listaVariable;
            ddlSubTotalDesS.DataTextField = "Nombre";
            ddlSubTotalDesS.DataValueField = "IdVariable";
            ddlSubTotalDesS.DataBind();

            ddlTotalH.DataSource = listaVariable;
            ddlTotalH.DataTextField = "Nombre";
            ddlTotalH.DataValueField = "IdVariable";
            ddlTotalH.DataBind();
            ddlTotalS.DataSource = listaVariable;
            ddlTotalS.DataTextField = "Nombre";
            ddlTotalS.DataValueField = "IdVariable";
            ddlTotalS.DataBind();

            // Extracto 4
            ddlPorcentajeIngresoPropietario4.DataSource = listaVariable;
            ddlPorcentajeIngresoPropietario4.DataTextField = "Nombre";
            ddlPorcentajeIngresoPropietario4.DataValueField = "IdVariable";
            ddlPorcentajeIngresoPropietario4.DataBind();

            ddlParticipacionPropietario4.DataSource = listaVariable;
            ddlParticipacionPropietario4.DataTextField = "Nombre";
            ddlParticipacionPropietario4.DataValueField = "IdVariable";
            ddlParticipacionPropietario4.DataBind();

            ddlIngresoTotalH4.DataSource = listaVariable;
            ddlIngresoTotalH4.DataTextField = "Nombre";
            ddlIngresoTotalH4.DataValueField = "IdVariable";
            ddlIngresoTotalH4.DataBind();
            ddlIngresoTotalS4.DataSource = listaVariable;
            ddlIngresoTotalS4.DataTextField = "Nombre";
            ddlIngresoTotalS4.DataValueField = "IdVariable";
            ddlIngresoTotalS4.DataBind();

            ddlTotalCostosH4.DataSource = listaVariable;
            ddlTotalCostosH4.DataTextField = "Nombre";
            ddlTotalCostosH4.DataValueField = "IdVariable";
            ddlTotalCostosH4.DataBind();
            ddlTotalCostosS4.DataSource = listaVariable;
            ddlTotalCostosS4.DataTextField = "Nombre";
            ddlTotalCostosS4.DataValueField = "IdVariable";
            ddlTotalCostosS4.DataBind();

            ddlTotalGastosH4.DataSource = listaVariable;
            ddlTotalGastosH4.DataTextField = "Nombre";
            ddlTotalGastosH4.DataValueField = "IdVariable";
            ddlTotalGastosH4.DataBind();
            ddlTotalGastosS4.DataSource = listaVariable;
            ddlTotalGastosS4.DataTextField = "Nombre";
            ddlTotalGastosS4.DataValueField = "IdVariable";
            ddlTotalGastosS4.DataBind();

            ddlIngresosDispoH4.DataSource = listaVariable;
            ddlIngresosDispoH4.DataTextField = "Nombre";
            ddlIngresosDispoH4.DataValueField = "IdVariable";
            ddlIngresosDispoH4.DataBind();
            ddlIngresosDispoS4.DataSource = listaVariable;
            ddlIngresosDispoS4.DataTextField = "Nombre";
            ddlIngresosDispoS4.DataValueField = "IdVariable";
            ddlIngresosDispoS4.DataBind();

            ddlIngrePropiH4.DataSource = listaVariable;
            ddlIngrePropiH4.DataTextField = "Nombre";
            ddlIngrePropiH4.DataValueField = "IdVariable";
            ddlIngrePropiH4.DataBind();
            ddlIngrePropiS4.DataSource = listaVariable;
            ddlIngrePropiS4.DataTextField = "Nombre";
            ddlIngrePropiS4.DataValueField = "IdVariable";
            ddlIngrePropiS4.DataBind();

            ddlDescFaraH4.DataSource = listaVariable;
            ddlDescFaraH4.DataTextField = "Nombre";
            ddlDescFaraH4.DataValueField = "IdVariable";
            ddlDescFaraH4.DataBind();
            ddlDescFaraS4.DataSource = listaVariable;
            ddlDescFaraS4.DataTextField = "Nombre";
            ddlDescFaraS4.DataValueField = "IdVariable";
            ddlDescFaraS4.DataBind();

            ddlPartiDistH4.DataSource = listaVariable;
            ddlPartiDistH4.DataTextField = "Nombre";
            ddlPartiDistH4.DataValueField = "IdVariable";
            ddlPartiDistH4.DataBind();
            ddlPartiDistS4.DataSource = listaVariable;
            ddlPartiDistS4.DataTextField = "Nombre";
            ddlPartiDistS4.DataValueField = "IdVariable";
            ddlPartiDistS4.DataBind();

            ddlRentaPropH4.DataSource = listaVariable;
            ddlRentaPropH4.DataTextField = "Nombre";
            ddlRentaPropH4.DataValueField = "IdVariable";
            ddlRentaPropH4.DataBind();
            ddlRentaPropS4.DataSource = listaVariable;
            ddlRentaPropS4.DataTextField = "Nombre";
            ddlRentaPropS4.DataValueField = "IdVariable";
            ddlRentaPropS4.DataBind();

            ddlUtilidadOperacional4.DataSource = listaVariable;
            ddlUtilidadOperacional4.DataTextField = "Nombre";
            ddlUtilidadOperacional4.DataValueField = "IdVariable";
            ddlUtilidadOperacional4.DataBind();

            ddlRete4H.DataSource = listaVariable;
            ddlRete4H.DataTextField = "Nombre";
            ddlRete4H.DataValueField = "IdVariable";
            ddlRete4H.DataBind();
            ddlRete4S.DataSource = listaVariable;
            ddlRete4S.DataTextField = "Nombre";
            ddlRete4S.DataValueField = "IdVariable";
            ddlRete4S.DataBind();

            ddlOtrosDes4H.DataSource = listaVariable;
            ddlOtrosDes4H.DataTextField = "Nombre";
            ddlOtrosDes4H.DataValueField = "IdVariable";
            ddlOtrosDes4H.DataBind();
            ddlOtrosDes4S.DataSource = listaVariable;
            ddlOtrosDes4S.DataTextField = "Nombre";
            ddlOtrosDes4S.DataValueField = "IdVariable";
            ddlOtrosDes4S.DataBind();            

            // Extracto 5
            ddlPorcentajeIngresoPropietario5.DataSource = listaVariable;
            ddlPorcentajeIngresoPropietario5.DataTextField = "Nombre";
            ddlPorcentajeIngresoPropietario5.DataValueField = "IdVariable";
            ddlPorcentajeIngresoPropietario5.DataBind();

            ddlParticipacionPropietario5.DataSource = listaVariable;
            ddlParticipacionPropietario5.DataTextField = "Nombre";
            ddlParticipacionPropietario5.DataValueField = "IdVariable";
            ddlParticipacionPropietario5.DataBind();

            ddlIngresoTotalH5.DataSource = listaVariable;
            ddlIngresoTotalH5.DataTextField = "Nombre";
            ddlIngresoTotalH5.DataValueField = "IdVariable";
            ddlIngresoTotalH5.DataBind();
            ddlIngresoTotalS5.DataSource = listaVariable;
            ddlIngresoTotalS5.DataTextField = "Nombre";
            ddlIngresoTotalS5.DataValueField = "IdVariable";
            ddlIngresoTotalS5.DataBind();

            ddlTotalCostosH5.DataSource = listaVariable;
            ddlTotalCostosH5.DataTextField = "Nombre";
            ddlTotalCostosH5.DataValueField = "IdVariable";
            ddlTotalCostosH5.DataBind();
            ddlTotalCostosS5.DataSource = listaVariable;
            ddlTotalCostosS5.DataTextField = "Nombre";
            ddlTotalCostosS5.DataValueField = "IdVariable";
            ddlTotalCostosS5.DataBind();

            ddlTotalGastosH5.DataSource = listaVariable;
            ddlTotalGastosH5.DataTextField = "Nombre";
            ddlTotalGastosH5.DataValueField = "IdVariable";
            ddlTotalGastosH5.DataBind();
            ddlTotalGastosS5.DataSource = listaVariable;
            ddlTotalGastosS5.DataTextField = "Nombre";
            ddlTotalGastosS5.DataValueField = "IdVariable";
            ddlTotalGastosS5.DataBind();

            ddlIngresosDispoH5.DataSource = listaVariable;
            ddlIngresosDispoH5.DataTextField = "Nombre";
            ddlIngresosDispoH5.DataValueField = "IdVariable";
            ddlIngresosDispoH5.DataBind();
            ddlIngresosDispoS5.DataSource = listaVariable;
            ddlIngresosDispoS5.DataTextField = "Nombre";
            ddlIngresosDispoS5.DataValueField = "IdVariable";
            ddlIngresosDispoS5.DataBind();

            ddlIngrePropiH5.DataSource = listaVariable;
            ddlIngrePropiH5.DataTextField = "Nombre";
            ddlIngrePropiH5.DataValueField = "IdVariable";
            ddlIngrePropiH5.DataBind();
            ddlIngrePropiS5.DataSource = listaVariable;
            ddlIngrePropiS5.DataTextField = "Nombre";
            ddlIngrePropiS5.DataValueField = "IdVariable";
            ddlIngrePropiS5.DataBind();

            ddlDescFaraH5.DataSource = listaVariable;
            ddlDescFaraH5.DataTextField = "Nombre";
            ddlDescFaraH5.DataValueField = "IdVariable";
            ddlDescFaraH5.DataBind();
            ddlDescFaraS5.DataSource = listaVariable;
            ddlDescFaraS5.DataTextField = "Nombre";
            ddlDescFaraS5.DataValueField = "IdVariable";
            ddlDescFaraS5.DataBind();

            ddlRentaPropH5.DataSource = listaVariable;
            ddlRentaPropH5.DataTextField = "Nombre";
            ddlRentaPropH5.DataValueField = "IdVariable";
            ddlRentaPropH5.DataBind();
            ddlRentaPropS5.DataSource = listaVariable;
            ddlRentaPropS5.DataTextField = "Nombre";
            ddlRentaPropS5.DataValueField = "IdVariable";
            ddlRentaPropS5.DataBind();

            ddlUtilidadOperacional5.DataSource = listaVariable;
            ddlUtilidadOperacional5.DataTextField = "Nombre";
            ddlUtilidadOperacional5.DataValueField = "IdVariable";
            ddlUtilidadOperacional5.DataBind();

            ddlRete5H.DataSource = listaVariable;
            ddlRete5H.DataTextField = "Nombre";
            ddlRete5H.DataValueField = "IdVariable";
            ddlRete5H.DataBind();
            ddlRete5S.DataSource = listaVariable;
            ddlRete5S.DataTextField = "Nombre";
            ddlRete5S.DataValueField = "IdVariable";
            ddlRete5S.DataBind();

            ddlOtrosConceptosH5.DataSource = listaVariable;
            ddlOtrosConceptosH5.DataTextField = "Nombre";
            ddlOtrosConceptosH5.DataValueField = "IdVariable";
            ddlOtrosConceptosH5.DataBind();
            ddlOtrosConceptosS5.DataSource = listaVariable;
            ddlOtrosConceptosS5.DataTextField = "Nombre";
            ddlOtrosConceptosS5.DataValueField = "IdVariable";
            ddlOtrosConceptosS5.DataBind();

            // Extracto 6
            ddlRetencionH6.DataSource = listaVariable;
            ddlRetencionH6.DataTextField = "Nombre";
            ddlRetencionH6.DataValueField = "IdVariable";
            ddlRetencionH6.DataBind();
            ddlRetencionS6.DataSource = listaVariable;
            ddlRetencionS6.DataTextField = "Nombre";
            ddlRetencionS6.DataValueField = "IdVariable";
            ddlRetencionS6.DataBind();

            ddlCoeficiente6.DataSource = listaVariable;
            ddlCoeficiente6.DataTextField = "Nombre";
            ddlCoeficiente6.DataValueField = "IdVariable";
            ddlCoeficiente6.DataBind();
            ddlParticipacionPropietario6.DataSource = listaVariable;
            ddlParticipacionPropietario6.DataTextField = "Nombre";
            ddlParticipacionPropietario6.DataValueField = "IdVariable";
            ddlParticipacionPropietario6.DataBind();

            ddlIngresoTotalH6.DataSource = listaVariable;
            ddlIngresoTotalH6.DataTextField = "Nombre";
            ddlIngresoTotalH6.DataValueField = "IdVariable";
            ddlIngresoTotalH6.DataBind();
            ddlIngresoTotalS6.DataSource = listaVariable;
            ddlIngresoTotalS6.DataTextField = "Nombre";
            ddlIngresoTotalS6.DataValueField = "IdVariable";
            ddlIngresoTotalS6.DataBind();

            ddlTotalCostosH6.DataSource = listaVariable;
            ddlTotalCostosH6.DataTextField = "Nombre";
            ddlTotalCostosH6.DataValueField = "IdVariable";
            ddlTotalCostosH6.DataBind();
            ddlTotalCostosS6.DataSource = listaVariable;
            ddlTotalCostosS6.DataTextField = "Nombre";
            ddlTotalCostosS6.DataValueField = "IdVariable";
            ddlTotalCostosS6.DataBind();

            ddlTotalGastosH6.DataSource = listaVariable;
            ddlTotalGastosH6.DataTextField = "Nombre";
            ddlTotalGastosH6.DataValueField = "IdVariable";
            ddlTotalGastosH6.DataBind();
            ddlTotalGastosS6.DataSource = listaVariable;
            ddlTotalGastosS6.DataTextField = "Nombre";
            ddlTotalGastosS6.DataValueField = "IdVariable";
            ddlTotalGastosS6.DataBind();

            ddlIngresosDispoH6.DataSource = listaVariable;
            ddlIngresosDispoH6.DataTextField = "Nombre";
            ddlIngresosDispoH6.DataValueField = "IdVariable";
            ddlIngresosDispoH6.DataBind();
            ddlIngresosDispoS6.DataSource = listaVariable;
            ddlIngresosDispoS6.DataTextField = "Nombre";
            ddlIngresosDispoS6.DataValueField = "IdVariable";
            ddlIngresosDispoS6.DataBind();

            ddlIngrePropiH6.DataSource = listaVariable;
            ddlIngrePropiH6.DataTextField = "Nombre";
            ddlIngrePropiH6.DataValueField = "IdVariable";
            ddlIngrePropiH6.DataBind();
            ddlIngrePropiS6.DataSource = listaVariable;
            ddlIngrePropiS6.DataTextField = "Nombre";
            ddlIngrePropiS6.DataValueField = "IdVariable";
            ddlIngrePropiS6.DataBind();

            ddlDescFaraH6.DataSource = listaVariable;
            ddlDescFaraH6.DataTextField = "Nombre";
            ddlDescFaraH6.DataValueField = "IdVariable";
            ddlDescFaraH6.DataBind();
            ddlDescFaraS6.DataSource = listaVariable;
            ddlDescFaraS6.DataTextField = "Nombre";
            ddlDescFaraS6.DataValueField = "IdVariable";
            ddlDescFaraS6.DataBind();

            ddlDesSegAnoH6.DataSource = listaVariable;
            ddlDesSegAnoH6.DataTextField = "Nombre";
            ddlDesSegAnoH6.DataValueField = "IdVariable";
            ddlDesSegAnoH6.DataBind();
            ddlDesSegAnoS6.DataSource = listaVariable;
            ddlDesSegAnoS6.DataTextField = "Nombre";
            ddlDesSegAnoS6.DataValueField = "IdVariable";
            ddlDesSegAnoS6.DataBind();

            ddlPartiDistH6.DataSource = listaVariable;
            ddlPartiDistH6.DataTextField = "Nombre";
            ddlPartiDistH6.DataValueField = "IdVariable";
            ddlPartiDistH6.DataBind();
            ddlPartiDistS6.DataSource = listaVariable;
            ddlPartiDistS6.DataTextField = "Nombre";
            ddlPartiDistS6.DataValueField = "IdVariable";
            ddlPartiDistS6.DataBind();

            ddlRentaPropH6.DataSource = listaVariable;
            ddlRentaPropH6.DataTextField = "Nombre";
            ddlRentaPropH6.DataValueField = "IdVariable";
            ddlRentaPropH6.DataBind();
            ddlRentaPropS6.DataSource = listaVariable;
            ddlRentaPropS6.DataTextField = "Nombre";
            ddlRentaPropS6.DataValueField = "IdVariable";
            ddlRentaPropS6.DataBind();

            ddlCuotaH6.DataSource = listaVariable;
            ddlCuotaH6.DataTextField = "Nombre";
            ddlCuotaH6.DataValueField = "IdVariable";
            ddlCuotaH6.DataBind();
            ddlCuotaS6.DataSource = listaVariable;
            ddlCuotaS6.DataTextField = "Nombre";
            ddlCuotaS6.DataValueField = "IdVariable";
            ddlCuotaS6.DataBind();

            ddlUtilidadOperacional6.DataSource = listaVariable;
            ddlUtilidadOperacional6.DataTextField = "Nombre";
            ddlUtilidadOperacional6.DataValueField = "IdVariable";
            ddlUtilidadOperacional6.DataBind();

            //Extracto Siete

            ddlRetencionH7.DataSource = listaVariable;
            ddlRetencionH7.DataTextField = "Nombre";
            ddlRetencionH7.DataValueField = "IdVariable";
            ddlRetencionH7.DataBind();
            ddlRetencionS7.DataSource = listaVariable;
            ddlRetencionS7.DataTextField = "Nombre";
            ddlRetencionS7.DataValueField = "IdVariable";
            ddlRetencionS7.DataBind();

            ddlCoeficiente7.DataSource = listaVariable;
            ddlCoeficiente7.DataTextField = "Nombre";
            ddlCoeficiente7.DataValueField = "IdVariable";
            ddlCoeficiente7.DataBind();
            ddlParticipacionPropietario7.DataSource = listaVariable;
            ddlParticipacionPropietario7.DataTextField = "Nombre";
            ddlParticipacionPropietario7.DataValueField = "IdVariable";
            ddlParticipacionPropietario7.DataBind();

            ddlIngresoTotalH7.DataSource = listaVariable;
            ddlIngresoTotalH7.DataTextField = "Nombre";
            ddlIngresoTotalH7.DataValueField = "IdVariable";
            ddlIngresoTotalH7.DataBind();
            ddlIngresoTotalS7.DataSource = listaVariable;
            ddlIngresoTotalS7.DataTextField = "Nombre";
            ddlIngresoTotalS7.DataValueField = "IdVariable";
            ddlIngresoTotalS7.DataBind();

            ddlTotalCostosH7.DataSource = listaVariable;
            ddlTotalCostosH7.DataTextField = "Nombre";
            ddlTotalCostosH7.DataValueField = "IdVariable";
            ddlTotalCostosH7.DataBind();
            ddlTotalCostosS7.DataSource = listaVariable;
            ddlTotalCostosS7.DataTextField = "Nombre";
            ddlTotalCostosS7.DataValueField = "IdVariable";
            ddlTotalCostosS7.DataBind();

            ddlApoyoH7.DataSource = listaVariable;
            ddlApoyoH7.DataTextField = "Nombre";
            ddlApoyoH7.DataValueField = "IdVariable";
            ddlApoyoH7.DataBind();
            ddlApoyoS7.DataSource = listaVariable;
            ddlApoyoS7.DataTextField = "Nombre";
            ddlApoyoS7.DataValueField = "IdVariable";
            ddlApoyoS7.DataBind();

            ddlTotalGastosH7.DataSource = listaVariable;
            ddlTotalGastosH7.DataTextField = "Nombre";
            ddlTotalGastosH7.DataValueField = "IdVariable";
            ddlTotalGastosH7.DataBind();
            ddlTotalGastosS7.DataSource = listaVariable;
            ddlTotalGastosS7.DataTextField = "Nombre";
            ddlTotalGastosS7.DataValueField = "IdVariable";
            ddlTotalGastosS7.DataBind();

            ddlIngrePropiH7.DataSource = listaVariable;
            ddlIngrePropiH7.DataTextField = "Nombre";
            ddlIngrePropiH7.DataValueField = "IdVariable";
            ddlIngrePropiH7.DataBind();
            ddlIngrePropiS7.DataSource = listaVariable;
            ddlIngrePropiS7.DataTextField = "Nombre";
            ddlIngrePropiS7.DataValueField = "IdVariable";
            ddlIngrePropiS7.DataBind();

            ddlGestorH7.DataSource = listaVariable;
            ddlGestorH7.DataTextField = "Nombre";
            ddlGestorH7.DataValueField = "IdVariable";
            ddlGestorH7.DataBind();
            ddlGestorS7.DataSource = listaVariable;
            ddlGestorS7.DataTextField = "Nombre";
            ddlGestorS7.DataValueField = "IdVariable";
            ddlGestorS7.DataBind();

            ddlDescFaraH7.DataSource = listaVariable;
            ddlDescFaraH7.DataTextField = "Nombre";
            ddlDescFaraH7.DataValueField = "IdVariable";
            ddlDescFaraH7.DataBind();
            ddlDescFaraS7.DataSource = listaVariable;
            ddlDescFaraS7.DataTextField = "Nombre";
            ddlDescFaraS7.DataValueField = "IdVariable";
            ddlDescFaraS7.DataBind();

            ddlDesSegAnoH7.DataSource = listaVariable;
            ddlDesSegAnoH7.DataTextField = "Nombre";
            ddlDesSegAnoH7.DataValueField = "IdVariable";
            ddlDesSegAnoH7.DataBind();
            ddlDesSegAnoS7.DataSource = listaVariable;
            ddlDesSegAnoS7.DataTextField = "Nombre";
            ddlDesSegAnoS7.DataValueField = "IdVariable";
            ddlDesSegAnoS7.DataBind();

            ddlSeguroH7.DataSource = listaVariable;
            ddlSeguroH7.DataTextField = "Nombre";
            ddlSeguroH7.DataValueField = "IdVariable";
            ddlSeguroH7.DataBind();
            ddlSeguroS7.DataSource = listaVariable;
            ddlSeguroS7.DataTextField = "Nombre";
            ddlSeguroS7.DataValueField = "IdVariable";
            ddlSeguroS7.DataBind();

            ddlPartiDistH7.DataSource = listaVariable;
            ddlPartiDistH7.DataTextField = "Nombre";
            ddlPartiDistH7.DataValueField = "IdVariable";
            ddlPartiDistH7.DataBind();
            ddlPartiDistS7.DataSource = listaVariable;
            ddlPartiDistS7.DataTextField = "Nombre";
            ddlPartiDistS7.DataValueField = "IdVariable";
            ddlPartiDistS7.DataBind();

            ddlRentaPropH7.DataSource = listaVariable;
            ddlRentaPropH7.DataTextField = "Nombre";
            ddlRentaPropH7.DataValueField = "IdVariable";
            ddlRentaPropH7.DataBind();
            ddlRentaPropS7.DataSource = listaVariable;
            ddlRentaPropS7.DataTextField = "Nombre";
            ddlRentaPropS7.DataValueField = "IdVariable";
            ddlRentaPropS7.DataBind();

            ddlIngrsosDisponiblesH7.DataSource = listaVariable;
            ddlIngrsosDisponiblesH7.DataTextField = "Nombre";
            ddlIngrsosDisponiblesH7.DataValueField = "IdVariable";
            ddlIngrsosDisponiblesH7.DataBind();

            ddlIngrsosDisponiblesS7.DataSource = listaVariable;
            ddlIngrsosDisponiblesS7.DataTextField = "Nombre";
            ddlIngrsosDisponiblesS7.DataValueField = "IdVariable";
            ddlIngrsosDisponiblesS7.DataBind();

            // Extracto 8
            ddlCoeficiente8.DataSource = listaVariable;
            ddlCoeficiente8.DataTextField = "Nombre";
            ddlCoeficiente8.DataValueField = "IdVariable";
            ddlCoeficiente8.DataBind();

            ddlParticipacionPropietario8.DataSource = listaVariable;
            ddlParticipacionPropietario8.DataTextField = "Nombre";
            ddlParticipacionPropietario8.DataValueField = "IdVariable";
            ddlParticipacionPropietario8.DataBind();

            ddlIngresoTotalH8.DataSource = listaVariable;
            ddlIngresoTotalH8.DataTextField = "Nombre";
            ddlIngresoTotalH8.DataValueField = "IdVariable";
            ddlIngresoTotalH8.DataBind();
            ddlIngresoTotalS8.DataSource = listaVariable;
            ddlIngresoTotalS8.DataTextField = "Nombre";
            ddlIngresoTotalS8.DataValueField = "IdVariable";
            ddlIngresoTotalS8.DataBind();

            ddlTotalCostosH8.DataSource = listaVariable;
            ddlTotalCostosH8.DataTextField = "Nombre";
            ddlTotalCostosH8.DataValueField = "IdVariable";
            ddlTotalCostosH8.DataBind();
            ddlTotalCostosS8.DataSource = listaVariable;
            ddlTotalCostosS8.DataTextField = "Nombre";
            ddlTotalCostosS8.DataValueField = "IdVariable";
            ddlTotalCostosS8.DataBind();

            ddlTotalGastosH8.DataSource = listaVariable;
            ddlTotalGastosH8.DataTextField = "Nombre";
            ddlTotalGastosH8.DataValueField = "IdVariable";
            ddlTotalGastosH8.DataBind();
            ddlTotalGastosS8.DataSource = listaVariable;
            ddlTotalGastosS8.DataTextField = "Nombre";
            ddlTotalGastosS8.DataValueField = "IdVariable";
            ddlTotalGastosS8.DataBind();

            ddlIngrsosDisponiblesH8.DataSource = listaVariable;
            ddlIngrsosDisponiblesH8.DataTextField = "Nombre";
            ddlIngrsosDisponiblesH8.DataValueField = "IdVariable";
            ddlIngrsosDisponiblesH8.DataBind();
            ddlIngrsosDisponiblesS8.DataSource = listaVariable;
            ddlIngrsosDisponiblesS8.DataTextField = "Nombre";
            ddlIngrsosDisponiblesS8.DataValueField = "IdVariable";
            ddlIngrsosDisponiblesS8.DataBind();

            ddlIngrePropiH8.DataSource = listaVariable;
            ddlIngrePropiH8.DataTextField = "Nombre";
            ddlIngrePropiH8.DataValueField = "IdVariable";
            ddlIngrePropiH8.DataBind();
            ddlIngrePropiS8.DataSource = listaVariable;
            ddlIngrePropiS8.DataTextField = "Nombre";
            ddlIngrePropiS8.DataValueField = "IdVariable";
            ddlIngrePropiS8.DataBind();

            ddlRetencionH8.DataSource = listaVariable;
            ddlRetencionH8.DataTextField = "Nombre";
            ddlRetencionH8.DataValueField = "IdVariable";
            ddlRetencionH8.DataBind();
            ddlRetencionS8.DataSource = listaVariable;
            ddlRetencionS8.DataTextField = "Nombre";
            ddlRetencionS8.DataValueField = "IdVariable";
            ddlRetencionS8.DataBind();

            ddlDescFaraH8.DataSource = listaVariable;
            ddlDescFaraH8.DataTextField = "Nombre";
            ddlDescFaraH8.DataValueField = "IdVariable";
            ddlDescFaraH8.DataBind();
            ddlDescFaraS8.DataSource = listaVariable;
            ddlDescFaraS8.DataTextField = "Nombre";
            ddlDescFaraS8.DataValueField = "IdVariable";
            ddlDescFaraS8.DataBind();

            ddlDesSegAnoH8.DataSource = listaVariable;
            ddlDesSegAnoH8.DataTextField = "Nombre";
            ddlDesSegAnoH8.DataValueField = "IdVariable";
            ddlDesSegAnoH8.DataBind();
            ddlDesSegAnoS8.DataSource = listaVariable;
            ddlDesSegAnoS8.DataTextField = "Nombre";
            ddlDesSegAnoS8.DataValueField = "IdVariable";
            ddlDesSegAnoS8.DataBind();

            ddlPartiDistH8.DataSource = listaVariable;
            ddlPartiDistH8.DataTextField = "Nombre";
            ddlPartiDistH8.DataValueField = "IdVariable";
            ddlPartiDistH8.DataBind();
            ddlPartiDistS8.DataSource = listaVariable;
            ddlPartiDistS8.DataTextField = "Nombre";
            ddlPartiDistS8.DataValueField = "IdVariable";
            ddlPartiDistS8.DataBind();

            ddlRentaPropH8.DataSource = listaVariable;
            ddlRentaPropH8.DataTextField = "Nombre";
            ddlRentaPropH8.DataValueField = "IdVariable";
            ddlRentaPropH8.DataBind();
            ddlRentaPropS8.DataSource = listaVariable;
            ddlRentaPropS8.DataTextField = "Nombre";
            ddlRentaPropS8.DataValueField = "IdVariable";
            ddlRentaPropS8.DataBind();

            //Extracto Uno

            ddlCoeficiente9.DataSource = listaVariable;
            ddlCoeficiente9.DataTextField = "Nombre";
            ddlCoeficiente9.DataValueField = "IdVariable";
            ddlCoeficiente9.DataBind();
            ddlParticipacionPropietario9.DataSource = listaVariable;
            ddlParticipacionPropietario9.DataTextField = "Nombre";
            ddlParticipacionPropietario9.DataValueField = "IdVariable";
            ddlParticipacionPropietario9.DataBind();

            ddlAlojamientoH9.DataSource = listaVariable;
            ddlAlojamientoH9.DataTextField = "Nombre";
            ddlAlojamientoH9.DataValueField = "IdVariable";
            ddlAlojamientoH9.DataBind();
            ddlAlojamientoS9.DataSource = listaVariable;
            ddlAlojamientoS9.DataTextField = "Nombre";
            ddlAlojamientoS9.DataValueField = "IdVariable";
            ddlAlojamientoS9.DataBind();

            ddlServiciosH9.DataSource = listaVariable;
            ddlServiciosH9.DataTextField = "Nombre";
            ddlServiciosH9.DataValueField = "IdVariable";
            ddlServiciosH9.DataBind();
            ddlServiciosS9.DataSource = listaVariable;
            ddlServiciosS9.DataTextField = "Nombre";
            ddlServiciosS9.DataValueField = "IdVariable";
            ddlServiciosS9.DataBind();

            ddlIngresosDispoH9.DataSource = listaVariable;
            ddlIngresosDispoH9.DataTextField = "Nombre";
            ddlIngresosDispoH9.DataValueField = "IdVariable";
            ddlIngresosDispoH9.DataBind();
            ddlIngresosDispoS9.DataSource = listaVariable;
            ddlIngresosDispoS9.DataTextField = "Nombre";
            ddlIngresosDispoS9.DataValueField = "IdVariable";
            ddlIngresosDispoS9.DataBind();

            ddlVentasAlojamientoH9.DataSource = listaVariable;
            ddlVentasAlojamientoH9.DataTextField = "Nombre";
            ddlVentasAlojamientoH9.DataValueField = "IdVariable";
            ddlVentasAlojamientoH9.DataBind();
            ddlVentasAlojamientoS9.DataSource = listaVariable;
            ddlVentasAlojamientoS9.DataTextField = "Nombre";
            ddlVentasAlojamientoS9.DataValueField = "IdVariable";
            ddlVentasAlojamientoS9.DataBind();

            ddlVentasServicioH9.DataSource = listaVariable;
            ddlVentasServicioH9.DataTextField = "Nombre";
            ddlVentasServicioH9.DataValueField = "IdVariable";
            ddlVentasServicioH9.DataBind();
            ddlVentasServicioS9.DataSource = listaVariable;
            ddlVentasServicioS9.DataTextField = "Nombre";
            ddlVentasServicioS9.DataValueField = "IdVariable";
            ddlVentasServicioS9.DataBind();

            ddlParticipacionH9.DataSource = listaVariable;
            ddlParticipacionH9.DataTextField = "Nombre";
            ddlParticipacionH9.DataValueField = "IdVariable";
            ddlParticipacionH9.DataBind();
            ddlParticipacionS9.DataSource = listaVariable;
            ddlParticipacionS9.DataTextField = "Nombre";
            ddlParticipacionS9.DataValueField = "IdVariable";
            ddlParticipacionS9.DataBind();

            ddlFaraH9.DataSource = listaVariable;
            ddlFaraH9.DataTextField = "Nombre";
            ddlFaraH9.DataValueField = "IdVariable";
            ddlFaraH9.DataBind();
            ddlFaraS9.DataSource = listaVariable;
            ddlFaraS9.DataTextField = "Nombre";
            ddlFaraS9.DataValueField = "IdVariable";
            ddlFaraS9.DataBind();

            ddlHonorariosH9.DataSource = listaVariable;
            ddlHonorariosH9.DataTextField = "Nombre";
            ddlHonorariosH9.DataValueField = "IdVariable";
            ddlHonorariosH9.DataBind();
            ddlHonorariosS9.DataSource = listaVariable;
            ddlHonorariosS9.DataTextField = "Nombre";
            ddlHonorariosS9.DataValueField = "IdVariable";
            ddlHonorariosS9.DataBind();

            ddlRetencionS9.DataSource = listaVariable;
            ddlRetencionS9.DataTextField = "Nombre";
            ddlRetencionS9.DataValueField = "IdVariable";
            ddlRetencionS9.DataBind();

            ddlParticipacionDistriH9.DataSource = listaVariable;
            ddlParticipacionDistriH9.DataTextField = "Nombre";
            ddlParticipacionDistriH9.DataValueField = "IdVariable";
            ddlParticipacionDistriH9.DataBind();
            ddlParticipacionDistriS9.DataSource = listaVariable;
            ddlParticipacionDistriS9.DataTextField = "Nombre";
            ddlParticipacionDistriS9.DataValueField = "IdVariable";
            ddlParticipacionDistriS9.DataBind();

            ddlRentaPropS9.DataSource = listaVariable;
            ddlRentaPropS9.DataTextField = "Nombre";
            ddlRentaPropS9.DataValueField = "IdVariable";
            ddlRentaPropS9.DataBind();
        }

        private void CargarDatos()
        {
            ConfigurarExtractoBo configExtractoBoTmp = new ConfigurarExtractoBo();
            Extracto extractoTmp = configExtractoBoTmp.Obtener(int.Parse(ddlHotel.SelectedValue));

            ContentPlaceHolder ph = (ContentPlaceHolder)Page.Master.FindControl("Contenidoprincipal");
            UserControl uc = (UserControl)ph.FindControl("WebUserConfigExtracto");

            if (extractoTmp != null)
            {
                ddlTipoExtracto.SelectedValue = extractoTmp.TipoExtracto.ToString();
                ddlTipoExtracto_SelectedIndexChanged(null, null);

                switch (ddlTipoExtracto.SelectedValue)
                {
                    case "1":
                        txtDescExtracto.Text = extractoTmp.DescripcionExtracto;
                        break;
                    case "3":
                        txtDescExtracto3.Text = extractoTmp.DescripcionExtracto;
                        break;
                    case "4":
                        txtDescExtracto4.Text = extractoTmp.DescripcionExtracto;
                        break;
                    case "5":
                        txtDescExtracto5.Text = extractoTmp.DescripcionExtracto;
                        break;
                    case "6":
                        txtDescExtracto6.Text = extractoTmp.DescripcionExtracto;
                        break;
                    case "7":
                        txtDescExtracto7.Text = extractoTmp.DescripcionExtracto;
                        break;
                    case "8":
                        txtDescExtracto8.Text = extractoTmp.DescripcionExtracto;
                        break;
                    case "9":
                        txtDescExtracto9.Text = extractoTmp.DescripcionExtracto;
                        break;
                    default:
                        break;
                }
                    

                //txtDescExtracto.Text = extractoTmp.DescripcionExtracto;
                txtPieExtracto.Text = extractoTmp.PieExtracto;
                imgLogo.ImageUrl = extractoTmp.FirmaLogo;                

                foreach (Detalle_Extracto itemDetalle in extractoTmp.Detalle_Extracto)
                {
                    DropDownList ddlTmp = (DropDownList)uc.FindControl(itemDetalle.IdControl);
                    if (ddlTmp != null)
                    {
                        ddlTmp.SelectedValue = itemDetalle.IdVariable.ToString();
                    }                    
                }                
            }
        }

        #endregion

        #region Evento
        protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Limpiar();
            this.CargarCombosVariable();
            this.CargarDatos();
        }

        protected void ddlTipoExtracto_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlTipoExtracto.SelectedValue)
            {
                case "1":
                    pnlFormatoNueve.Visible = false;
                    pnlFormatoOcho.Visible = false;
                    pnlFormatoSiete.Visible = false;
                    pnlFormatoSeis.Visible = false;
                    pnlFormatoCinco.Visible = false;
                    pnlFormatoCuatro.Visible = false;
                    pnlFormatoTres.Visible = false;
                    pnlFormatoDos.Visible = false;
                    pnlFormatoUno.Visible = true;
                    break;

                case "2":
                    pnlFormatoNueve.Visible = false;
                    pnlFormatoOcho.Visible = false;
                    pnlFormatoSiete.Visible = false;
                    pnlFormatoSeis.Visible = false;
                    pnlFormatoCinco.Visible = false;
                    pnlFormatoCuatro.Visible = false;
                    pnlFormatoTres.Visible = false;
                    pnlFormatoDos.Visible = true;
                    pnlFormatoUno.Visible = false;
                    break;

                case "3":
                    pnlFormatoNueve.Visible = false;
                    pnlFormatoOcho.Visible = false;
                    pnlFormatoSiete.Visible = false;
                    pnlFormatoSeis.Visible = false;
                    pnlFormatoCinco.Visible = false;
                    pnlFormatoCuatro.Visible = false;
                    pnlFormatoTres.Visible = true;
                    pnlFormatoDos.Visible = false;
                    pnlFormatoUno.Visible = false;
                    break;

                case "4":
                    pnlFormatoNueve.Visible = false;
                    pnlFormatoOcho.Visible = false;
                    pnlFormatoSiete.Visible = false;
                    pnlFormatoSeis.Visible = false;
                    pnlFormatoCinco.Visible = false;
                    pnlFormatoCuatro.Visible = true;
                    pnlFormatoTres.Visible = false;
                    pnlFormatoDos.Visible = false;
                    pnlFormatoUno.Visible = false;
                    break;

                case "5":
                    pnlFormatoNueve.Visible = false;
                    pnlFormatoOcho.Visible = false;
                    pnlFormatoSiete.Visible = false;
                    pnlFormatoSeis.Visible = false;
                    pnlFormatoCinco.Visible = true;
                    pnlFormatoCuatro.Visible = false;
                    pnlFormatoTres.Visible = false;
                    pnlFormatoDos.Visible = false;
                    pnlFormatoUno.Visible = false;
                    break;

                case "6":
                    pnlFormatoSiete.Visible = false;
                    pnlFormatoSeis.Visible = true;
                    pnlFormatoCinco.Visible = false;
                    pnlFormatoCuatro.Visible = false;
                    pnlFormatoTres.Visible = false;
                    pnlFormatoDos.Visible = false;
                    pnlFormatoUno.Visible = false;
                    break;

                case "7":
                    pnlFormatoNueve.Visible = false;
                    pnlFormatoOcho.Visible = false;
                    pnlFormatoSiete.Visible = true;
                    pnlFormatoSeis.Visible = false;
                    pnlFormatoCinco.Visible = false;
                    pnlFormatoCuatro.Visible = false;
                    pnlFormatoTres.Visible = false;
                    pnlFormatoDos.Visible = false;
                    pnlFormatoUno.Visible = false;
                    break;

                case "8":
                    pnlFormatoNueve.Visible = false;
                    pnlFormatoOcho.Visible = true;
                    pnlFormatoSiete.Visible = false;
                    pnlFormatoSeis.Visible = false;
                    pnlFormatoCinco.Visible = false;
                    pnlFormatoCuatro.Visible = false;
                    pnlFormatoTres.Visible = false;
                    pnlFormatoDos.Visible = false;
                    pnlFormatoUno.Visible = false;
                    break;

                case "9":
                    pnlFormatoNueve.Visible = true;
                    pnlFormatoOcho.Visible = false;
                    pnlFormatoSiete.Visible = false;
                    pnlFormatoSeis.Visible = false;
                    pnlFormatoCinco.Visible = false;
                    pnlFormatoCuatro.Visible = false;
                    pnlFormatoTres.Visible = false;
                    pnlFormatoDos.Visible = false;
                    pnlFormatoUno.Visible = false;
                    break;

                default:
                    break;
            }
        }

        protected void AsyncFileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            string filename = System.IO.Path.GetFileName(AsyncFileUpload1.FileName);
            AsyncFileUpload1.SaveAs(Server.MapPath("../../img/imgFirmas/") + filename);

            HotelBo hotelBoTmp = new HotelBo();
            //hotelBoTmp.GuardarRutaLogo(this.IdHotelSeleccionado, "~/img/imgLogo/" + filename);

            imgLogo.ImageUrl = Server.MapPath("~/img/imgLogo/") + filename;
        }
        #endregion

        #region Boton
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjetoGenerico detalleExt = null;
                List<ObjetoGenerico> listaDetalleExt = new List<ObjetoGenerico>();
                string textoExtracto = string.Empty;

                switch (ddlTipoExtracto.SelectedValue)
                {
                    case "1":
                        #region
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPorcentajeIngresoPropietario.SelectedValue);
                        detalleExt.NombreVariable = ddlPorcentajeIngresoPropietario.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPorcentajeIngresoPropietario.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCoeficiente.SelectedValue);
                        detalleExt.NombreVariable = ddlCoeficiente.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCoeficiente.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionPropietario.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionPropietario.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionPropietario.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalH.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalS.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosH.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosS.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosH.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosS.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoH.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoS.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiH.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiS.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiS.ID;
                        listaDetalleExt.Add(detalleExt);
                        
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionH.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionS.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraH.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraS.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDesSegAnoH.SelectedValue);
                        detalleExt.NombreVariable = ddlDesSegAnoH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDesSegAnoH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDesSegAnoS.SelectedValue);
                        detalleExt.NombreVariable = ddlDesSegAnoS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDesSegAnoS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistH.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistS.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropH.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropS.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlEficienciaOperativaH.SelectedValue);
                        detalleExt.NombreVariable = ddlEficienciaOperativaH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlEficienciaOperativaH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlEficienciaOperativaS.SelectedValue);
                        detalleExt.NombreVariable = ddlEficienciaOperativaS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlEficienciaOperativaS.ID;
                        listaDetalleExt.Add(detalleExt);                        

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlUtilidadOperacional.SelectedValue);
                        detalleExt.NombreVariable = ddlUtilidadOperacional.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlUtilidadOperacional.ID;
                        listaDetalleExt.Add(detalleExt);

                        textoExtracto = txtDescExtracto.Text;
                        #endregion
                        break;

                    case "2":
                        #region
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlUtilidadOperativaCop.SelectedValue);
                        detalleExt.NombreVariable = ddlUtilidadOperativaCop.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlUtilidadOperativaCop.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlUtilidadOperativaSuite.SelectedValue);
                        detalleExt.NombreVariable = ddlUtilidadOperativaSuite.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlUtilidadOperativaSuite.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlFaraContractualCop.SelectedValue);
                        detalleExt.NombreVariable = ddlFaraContractualCop.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlFaraContractualCop.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlFaraContractualSuite.SelectedValue);
                        detalleExt.NombreVariable = ddlFaraContractualSuite.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlFaraContractualSuite.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlFaraAdicionalCop.SelectedValue);
                        detalleExt.NombreVariable = ddlFaraAdicionalCop.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlFaraAdicionalCop.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlFaraAdicionalSuite.SelectedValue);
                        detalleExt.NombreVariable = ddlFaraAdicionalSuite.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlFaraAdicionalSuite.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionFuenteCop.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionFuenteCop.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionFuenteCop.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionFuenteSuite.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionFuenteSuite.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionFuenteSuite.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlFaraAdicional.SelectedValue);
                        detalleExt.NombreVariable = ddlFaraAdicional.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlFaraAdicional.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlNochesVendidas.SelectedValue);
                        detalleExt.NombreVariable = ddlNochesVendidas.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlNochesVendidas.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlOcupacion.SelectedValue);
                        detalleExt.NombreVariable = ddlOcupacion.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlOcupacion.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlAcumulado.SelectedValue);
                        detalleExt.NombreVariable = ddlAcumulado.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlAcumulado.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRenta.SelectedValue);
                        detalleExt.NombreVariable = ddlRenta.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRenta.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCoeficienteSuite.SelectedValue);
                        detalleExt.NombreVariable = ddlCoeficienteSuite.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCoeficienteSuite.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCoeficienteParticipacion.SelectedValue);
                        detalleExt.NombreVariable = ddlCoeficienteParticipacion.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCoeficienteParticipacion.ID;
                        listaDetalleExt.Add(detalleExt);
                        
                        #endregion
                        break;

                    case "3":
                        #region
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCoeficiente3.SelectedValue);
                        detalleExt.NombreVariable = ddlCoeficiente3.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCoeficiente3.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionPropietario3.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionPropietario3.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionPropietario3.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlVentaTotalH.SelectedValue);
                        detalleExt.NombreVariable = ddlVentaTotalH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlVentaTotalH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlVentaTotalS.SelectedValue);
                        detalleExt.NombreVariable = ddlVentaTotalS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlVentaTotalS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlAlojamientoH.SelectedValue);
                        detalleExt.NombreVariable = ddlAlojamientoH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlAlojamientoH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlAlojamientoS.SelectedValue);
                        detalleExt.NombreVariable = ddlAlojamientoS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlAlojamientoS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlServiciosCompleH.SelectedValue);
                        detalleExt.NombreVariable = ddlServiciosCompleH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlServiciosCompleH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlServiciosCompleS.SelectedValue);
                        detalleExt.NombreVariable = ddlServiciosCompleS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlServiciosCompleS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalIngresosH.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalIngresosH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalIngresosH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalIngresosS.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalIngresosS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalIngresosS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRemanenteH.SelectedValue);
                        detalleExt.NombreVariable = ddlRemanenteH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRemanenteH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRemanenteS.SelectedValue);
                        detalleExt.NombreVariable = ddlRemanenteS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRemanenteS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCostosGastosH.SelectedValue);
                        detalleExt.NombreVariable = ddlCostosGastosH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCostosGastosH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCostosGastosS.SelectedValue);
                        detalleExt.NombreVariable = ddlCostosGastosS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCostosGastosS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlExedenteH.SelectedValue);
                        detalleExt.NombreVariable = ddlExedenteH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlExedenteH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlExedenteS.SelectedValue);
                        detalleExt.NombreVariable = ddlExedenteS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlExedenteS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlNotaCreditoH.SelectedValue);
                        detalleExt.NombreVariable = ddlNotaCreditoH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlNotaCreditoH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlNotaCreditoS.SelectedValue);
                        detalleExt.NombreVariable = ddlNotaCreditoS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlNotaCreditoS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlAdminH.SelectedValue);
                        detalleExt.NombreVariable = ddlAdminH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlAdminH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlAdminS.SelectedValue);
                        detalleExt.NombreVariable = ddlAdminS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlAdminS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraH3.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraH3.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraH3.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraS3.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraS3.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraS3.ID;
                        listaDetalleExt.Add(detalleExt);                        

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlReteH.SelectedValue);
                        detalleExt.NombreVariable = ddlReteH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlReteH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlReteS.SelectedValue);
                        detalleExt.NombreVariable = ddlReteS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlReteS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCuotaH.SelectedValue);
                        detalleExt.NombreVariable = ddlCuotaH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCuotaH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCuotaS.SelectedValue);
                        detalleExt.NombreVariable = ddlCuotaS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCuotaS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlSubTotalDesH.SelectedValue);
                        detalleExt.NombreVariable = ddlSubTotalDesH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlSubTotalDesH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlSubTotalDesS.SelectedValue);
                        detalleExt.NombreVariable = ddlSubTotalDesS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlSubTotalDesS.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalH.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalH.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalH.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalS.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalS.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalS.ID;
                        listaDetalleExt.Add(detalleExt);

                        textoExtracto = txtDescExtracto3.Text;
                        #endregion
                        break;

                    case "4":
                        #region
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPorcentajeIngresoPropietario4.SelectedValue);
                        detalleExt.NombreVariable = ddlPorcentajeIngresoPropietario4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPorcentajeIngresoPropietario4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionPropietario4.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionPropietario4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionPropietario4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalH4.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalH4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalH4.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalS4.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalS4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalS4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosH4.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosH4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosH4.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosS4.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosS4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosS4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosH4.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosH4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosH4.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosS4.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosS4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosS4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoH4.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoH4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoH4.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoS4.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoS4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoS4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiH4.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiH4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiH4.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiS4.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiS4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiS4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraH4.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraH4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraH4.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraS4.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraS4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraS4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistH4.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistH4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistH4.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistS4.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistS4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistS4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropH4.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropH4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropH4.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropS4.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropS4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropS4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlUtilidadOperacional4.SelectedValue);
                        detalleExt.NombreVariable = ddlUtilidadOperacional4.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlUtilidadOperacional4.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRete4H.SelectedValue);
                        detalleExt.NombreVariable = ddlRete4H.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRete4H.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRete4S.SelectedValue);
                        detalleExt.NombreVariable = ddlRete4S.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRete4S.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlOtrosDes4H.SelectedValue);
                        detalleExt.NombreVariable = ddlOtrosDes4H.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlOtrosDes4H.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlOtrosDes4S.SelectedValue);
                        detalleExt.NombreVariable = ddlOtrosDes4S.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlOtrosDes4S.ID;
                        listaDetalleExt.Add(detalleExt);

                        textoExtracto = txtDescExtracto4.Text;
                        #endregion
                        break;

                    case "5":
                        #region
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPorcentajeIngresoPropietario5.SelectedValue);
                        detalleExt.NombreVariable = ddlPorcentajeIngresoPropietario5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPorcentajeIngresoPropietario5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionPropietario5.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionPropietario5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionPropietario5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalH5.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalH5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalH5.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalS5.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalS5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalS5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosH5.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosH5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosH5.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosS5.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosS5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosS5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosH5.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosH5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosH5.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosS5.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosS5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosS5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoH5.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoH5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoH5.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoS5.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoS5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoS5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiH5.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiH5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiH5.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiS5.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiS5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiS5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraH5.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraH5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraH5.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraS5.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraS5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraS5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropH5.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropH5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropH5.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropS5.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropS5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropS5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlUtilidadOperacional5.SelectedValue);
                        detalleExt.NombreVariable = ddlUtilidadOperacional5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlUtilidadOperacional5.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRete5H.SelectedValue);
                        detalleExt.NombreVariable = ddlRete5H.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRete5H.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRete5S.SelectedValue);
                        detalleExt.NombreVariable = ddlRete5S.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRete5S.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlOtrosConceptosH5.SelectedValue);
                        detalleExt.NombreVariable = ddlOtrosConceptosH5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlOtrosConceptosH5.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlOtrosConceptosS5.SelectedValue);
                        detalleExt.NombreVariable = ddlOtrosConceptosS5.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlOtrosConceptosS5.ID;
                        listaDetalleExt.Add(detalleExt);

                        textoExtracto = txtDescExtracto5.Text;
                        #endregion
                        break;

                    case "6":
                        #region
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCoeficiente6.SelectedValue);
                        detalleExt.NombreVariable = ddlCoeficiente6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCoeficiente6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionPropietario6.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionPropietario6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionPropietario6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalH6.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalS6.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosH6.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosS6.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosH6.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosS6.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoH6.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoS6.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiH6.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiS6.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionH6.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionS6.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraH6.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraS6.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDesSegAnoH6.SelectedValue);
                        detalleExt.NombreVariable = ddlDesSegAnoH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDesSegAnoH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDesSegAnoS6.SelectedValue);
                        detalleExt.NombreVariable = ddlDesSegAnoS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDesSegAnoS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistH6.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistS6.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropH6.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropS6.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCuotaH6.SelectedValue);
                        detalleExt.NombreVariable = ddlCuotaH6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCuotaH6.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCuotaS6.SelectedValue);
                        detalleExt.NombreVariable = ddlCuotaS6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCuotaS6.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlUtilidadOperacional6.SelectedValue);
                        detalleExt.NombreVariable = ddlUtilidadOperacional6.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlUtilidadOperacional6.ID;
                        listaDetalleExt.Add(detalleExt);

                        textoExtracto = txtDescExtracto6.Text;
                        #endregion
                        break;

                    case "7":
                        #region

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCoeficiente7.SelectedValue);
                        detalleExt.NombreVariable = ddlCoeficiente7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCoeficiente7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionPropietario7.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionPropietario7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionPropietario7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalH7.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalS7.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosH7.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosS7.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlApoyoH7.SelectedValue);
                        detalleExt.NombreVariable = ddlApoyoH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlApoyoH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlApoyoS7.SelectedValue);
                        detalleExt.NombreVariable = ddlApoyoS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlApoyoS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosH7.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosS7.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiH7.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiS7.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlGestorH7.SelectedValue);
                        detalleExt.NombreVariable = ddlGestorH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlGestorH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlGestorS7.SelectedValue);
                        detalleExt.NombreVariable = ddlGestorS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlGestorS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionH7.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionS7.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraH7.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraS7.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDesSegAnoH7.SelectedValue);
                        detalleExt.NombreVariable = ddlDesSegAnoH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDesSegAnoH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDesSegAnoS7.SelectedValue);
                        detalleExt.NombreVariable = ddlDesSegAnoS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDesSegAnoS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlSeguroH7.SelectedValue);
                        detalleExt.NombreVariable = ddlSeguroH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlSeguroH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlSeguroS7.SelectedValue);
                        detalleExt.NombreVariable = ddlSeguroS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlSeguroS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistH7.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistS7.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropH7.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropS7.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrsosDisponiblesH7.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrsosDisponiblesH7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrsosDisponiblesH7.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrsosDisponiblesS7.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrsosDisponiblesS7.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrsosDisponiblesS7.ID;
                        listaDetalleExt.Add(detalleExt);

                        textoExtracto = txtDescExtracto7.Text;
                        #endregion
                        break;

                    case "8":
                        #region

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCoeficiente8.SelectedValue);
                        detalleExt.NombreVariable = ddlCoeficiente8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCoeficiente8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionPropietario8.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionPropietario8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionPropietario8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalH8.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresoTotalS8.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresoTotalS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresoTotalS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosH8.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalCostosS8.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalCostosS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalCostosS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosH8.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlTotalGastosS8.SelectedValue);
                        detalleExt.NombreVariable = ddlTotalGastosS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlTotalGastosS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrsosDisponiblesH8.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrsosDisponiblesH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrsosDisponiblesH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrsosDisponiblesS8.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrsosDisponiblesS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrsosDisponiblesS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiH8.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngrePropiS8.SelectedValue);
                        detalleExt.NombreVariable = ddlIngrePropiS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngrePropiS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionH8.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionS8.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraH8.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDescFaraS8.SelectedValue);
                        detalleExt.NombreVariable = ddlDescFaraS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDescFaraS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDesSegAnoH8.SelectedValue);
                        detalleExt.NombreVariable = ddlDesSegAnoH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDesSegAnoH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlDesSegAnoS8.SelectedValue);
                        detalleExt.NombreVariable = ddlDesSegAnoS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlDesSegAnoS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistH8.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlPartiDistS8.SelectedValue);
                        detalleExt.NombreVariable = ddlPartiDistS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlPartiDistS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropH8.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropH8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropH8.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropS8.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropS8.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropS8.ID;
                        listaDetalleExt.Add(detalleExt);

                        textoExtracto = txtDescExtracto8.Text;
                        #endregion
                        break;

                    case "9":
                        #region
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlCoeficiente9.SelectedValue);
                        detalleExt.NombreVariable = ddlCoeficiente9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlCoeficiente9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionPropietario9.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionPropietario9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionPropietario9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlAlojamientoH9.SelectedValue);
                        detalleExt.NombreVariable = ddlAlojamientoH9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlAlojamientoH9.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlAlojamientoS9.SelectedValue);
                        detalleExt.NombreVariable = ddlAlojamientoS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlAlojamientoS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlServiciosH9.SelectedValue);
                        detalleExt.NombreVariable = ddlServiciosH9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlServiciosH9.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlServiciosS9.SelectedValue);
                        detalleExt.NombreVariable = ddlServiciosS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlServiciosS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoH9.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoH9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoH9.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlIngresosDispoS9.SelectedValue);
                        detalleExt.NombreVariable = ddlIngresosDispoS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlIngresosDispoS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlVentasAlojamientoH9.SelectedValue);
                        detalleExt.NombreVariable = ddlVentasAlojamientoH9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlVentasAlojamientoH9.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlVentasAlojamientoS9.SelectedValue);
                        detalleExt.NombreVariable = ddlVentasAlojamientoS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlVentasAlojamientoS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlVentasServicioH9.SelectedValue);
                        detalleExt.NombreVariable = ddlVentasServicioH9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlVentasServicioH9.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlVentasServicioS9.SelectedValue);
                        detalleExt.NombreVariable = ddlVentasServicioS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlVentasServicioS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionH9.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionH9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionH9.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionS9.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlFaraH9.SelectedValue);
                        detalleExt.NombreVariable = ddlFaraH9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlFaraH9.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlFaraS9.SelectedValue);
                        detalleExt.NombreVariable = ddlFaraS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlFaraS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlHonorariosH9.SelectedValue);
                        detalleExt.NombreVariable = ddlHonorariosH9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlHonorariosH9.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlHonorariosS9.SelectedValue);
                        detalleExt.NombreVariable = ddlHonorariosS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlHonorariosS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRetencionS9.SelectedValue);
                        detalleExt.NombreVariable = ddlRetencionS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRetencionS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionDistriH9.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionDistriH9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionDistriH9.ID;
                        listaDetalleExt.Add(detalleExt);
                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlParticipacionDistriS9.SelectedValue);
                        detalleExt.NombreVariable = ddlParticipacionDistriS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlParticipacionDistriS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        detalleExt = new ObjetoGenerico();
                        detalleExt.IdVariable = int.Parse(ddlRentaPropS9.SelectedValue);
                        detalleExt.NombreVariable = ddlRentaPropS9.SelectedItem.Text.Split('-')[1].Trim();
                        detalleExt.Nombre = ddlRentaPropS9.ID;
                        listaDetalleExt.Add(detalleExt);

                        textoExtracto = txtDescExtracto9.Text;
                        #endregion
                        break;
                    default:
                        break;
                }

                string rutaLogoFirma = imgLogo.ImageUrl; ;
                if (AsyncFileUpload1.HasFile)
                {
                    rutaLogoFirma = "~/img/imgFirmas/" + AsyncFileUpload1.FileName;
                }                

                ConfigurarExtractoBo configExtBoTmp = new ConfigurarExtractoBo();
                configExtBoTmp.GuardarActualizar(int.Parse(ddlHotel.SelectedValue), textoExtracto, txtPieExtracto.Text, listaDetalleExt, rutaLogoFirma, short.Parse(ddlTipoExtracto.SelectedValue));
                this.CargarDatos();

                this.divExito.Visible = true;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeGuardar;
                this.divError.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
            }            
        }
        #endregion
    }
}