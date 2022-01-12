using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Servicios;
using BO;
using System.Text;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserCargaMasiva : System.Web.UI.UserControl
    {
        CargaMasivaBo cargaMasivaBoTmp = new CargaMasivaBo();

        protected void Page_Load(object sender, EventArgs e)
        {
            divError.Visible = false;
            divExito.Visible = false;                        

            if (!IsPostBack)
            {
                this.NumLineasCargadas = 0;
                pnlErrores.Visible = false;
                btnAceptar.Visible = false;
                ddlTabla_SelectedIndexChanged(null, null);
            }
        }

        #region propiedades
        public string RutaArchivo 
        {
            get { return (string)Session["RutaArchivo"]; }
            set { Session["RutaArchivo"] = value; }
        }
        public int NumLineas 
        {
            get { return (int)Session["NumLineas"]; }
            set { Session["NumLineas"] = value; }
        }
        public int NumLineasCargadas 
        {
            get { return (int)ViewState["NumLineasCargadas"]; }
            set { ViewState["NumLineasCargadas"] = value; }
        }
        public string Estructura 
        {
            get { return (string)ViewState["Estructura"]; }
            set { ViewState["Estructura"] = value; } 
        }
        public string InfoCargue 
        {
            get { return (string)ViewState["InfoCargue"]; }
            set { ViewState["InfoCargue"] = value; }
        }
        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)Session["usuarioLogin"]; }
        }
        #endregion

        #region Evento

        protected void ddlTabla_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tabla = ddlTabla.SelectedValue;

            switch (tabla)
            {
                case "HS":
                    lblInfoEstructura.Text = " [ Nombre Hotel, Nit Hotel, Dirección Hotel, Correo Hotel, Correo de reservas, Código Hotel, Ciudad Hotel, Numero Suite, Numero Suite Escritura, Registro Notaria, Descripción suite ]";
                    break;
                case "P":
                    lblInfoEstructura.Text = " [ PRIMER NOMBRE,	SEGUNDO NOMBRE,	PRIMER APELLIDO, SEGUNDO APELLIDO, TIPO PERSONA, TIPO DOCUMENTO, NUMERO IDENTIFICACIÓN Ó NIT	CORREO,	DIRECCIÓN,	CIUDAD PROPIETARIO,	TELÉFONO 1,	TELÉFONO 2,	NOMBRE CONTACTO, TELÉFONO CONTACTO, CORREO CONTACTO, CÓDIGO HOTEL, SUITE ESCRITURA,	NOMBRE BANCO, NUMERO DE CUENTA,	TIPO DE CUENTA,	NOMBRE TITULAR,	NUMERO DE ESTADÍAS ]";
                    break;
                case "VS":
                    lblInfoEstructura.Text = " [ Nit ó Cedula Propietario, Código Hotel, Suite Escritura, Nombre Variable, Descripción Variable, Valor Variable ]";
                    break;
                case "VH":
                    lblInfoEstructura.Text = " [ Código Hotel, Nombre Variable, Descripción Variable, Valor ]";
                    break;

                case "IC":
                    lblInfoEstructura.Text = " [ Nit ó Cedula Propietario, Código Hotel, Año, Nombre Concepto, Valor ]";
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Boton

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Response.Redirect("CargaMasiva.aspx", true);
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreArchivoTipo = string.Empty;

                if (this.AsyncFileUpload.HasFile)
                {
                    List<string> listaError = new List<string>();
                    string filename = System.IO.Path.GetFileName(AsyncFileUpload.FileName);
                    string rutaFile = Server.MapPath("~/upload/") + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + filename + ".txt";
                    AsyncFileUpload.SaveAs(rutaFile);
                    nombreArchivoTipo = "Nombre Archivo : " + filename;

                    ControlEnable(false);

                    cargaMasivaBoTmp.Ruta = rutaFile; //AsyncFileUpload.FileName;//this.RutaArchivo;
                    cargaMasivaBoTmp.IdPerfilPropietario = Properties.Settings.Default.IdPropietario;
                    cargaMasivaBoTmp.IdCiudadDefault = Properties.Settings.Default.IdCiudadDefault;
                    cargaMasivaBoTmp.NumEstadias = Properties.Settings.Default.NumEstadias;
                    cargaMasivaBoTmp.IdBancoDefault = Properties.Settings.Default.IdBancoDefault;
                    cargaMasivaBoTmp.NombreBancoDefault = Properties.Settings.Default.NombreBancoDefault;
                    cargaMasivaBoTmp.IdUsuario = this.UsuarioLogin.Id;

                    switch (ddlTabla.SelectedValue)
                    {
                        case "HS":
                            nombreArchivoTipo += " Modulo : Hotel - Suite";
                            listaError = cargaMasivaBoTmp.Cargar_HotelSuit();

                            if (listaError.Count > 0)
                                this.MostrarErrores(listaError);
                            else
                            {
                                pnlErrores.Visible = false;
                                lblEstadoExito.Visible = true;

                                btnAceptar.Visible = true;
                                lblEstadoExito.Text = "El cargue fue exitoso.";
                            }

                            break;
                            
                        case "P":
                            nombreArchivoTipo += " Modulo : Propietario";
                            listaError = cargaMasivaBoTmp.Cargar_Propietario();

                            if (listaError.Count > 0)
                                this.MostrarErrores(listaError);
                            else
                            {
                                pnlErrores.Visible = false;
                                lblEstadoExito.Visible = true;

                                btnAceptar.Visible = true;
                                lblEstadoExito.Text = "El cargue fue exitoso.";
                            }

                            break;

                        case "VS":
                            nombreArchivoTipo += " Modulo : Variable - Suite";
                            listaError = cargaMasivaBoTmp.Cargar_VariableSuit();

                            if (listaError.Count > 0)
                                this.MostrarErrores(listaError);                            
                            else
                            {
                                pnlErrores.Visible = false;
                                lblEstadoExito.Visible = true;

                                btnAceptar.Visible = true;
                                lblEstadoExito.Text = "El cargue fue exitoso.";
                            }
                            break;

                        case "VH":
                            nombreArchivoTipo += " Modulo : Variable - Hotel";
                            listaError = cargaMasivaBoTmp.Cargar_VariableHotel();

                            if (listaError.Count > 0)
                                this.MostrarErrores(listaError);
                            else
                            {
                                pnlErrores.Visible = false;
                                lblEstadoExito.Visible = true;

                                btnAceptar.Visible = true;
                                lblEstadoExito.Text = "El cargue fue exitoso.";
                            }
                            break;

                        case "IC":
                            nombreArchivoTipo += " Modulo : Informacion Certificado";
                            listaError = cargaMasivaBoTmp.Cargar_InformacionCertificado();

                            if (listaError.Count > 0)
                                this.MostrarErrores(listaError);
                            else
                            {
                                pnlErrores.Visible = false;
                                lblEstadoExito.Visible = true;

                                btnAceptar.Visible = true;
                                lblEstadoExito.Text = "El cargue fue exitoso.";
                            }
                            break;

                        default:
                            break;
                    }

                    #region Auditoria
                    AuditoriaBo auditoriaBo = new AuditoriaBo();
                    auditoriaBo.Guardar("Carga Masiva : Descripcion Archivo : " + nombreArchivoTipo, "", "Carga Masiva", "Insertar", "Carga Masiva", DateTime.Now, this.UsuarioLogin.Id);
                    #endregion
                }
                else
                {
                    this.divExito.Visible = false;
                    this.divError.Visible = true;
                    this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;
                }
            }
            catch (Exception ex)
            {
                cargaMasivaBoTmp.CerrarArchivo();

                Utilities.Log(ex);

                this.divExito.Visible = false;
                this.divError.Visible = true;
                this.lbltextoError.Text = Resources.Resource.lblMensajeError_6;

                btnAceptar.Visible = true;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                Utilities.EliminarArchivo(this.RutaArchivo);

                AsyncFileUpload.Enabled = true;
                ddlTabla.Enabled = true;

                btnCargar.Enabled = false;

                Response.Redirect("CargaMasiva.aspx", true);
            }
            catch (Exception ex)
            {
            }
            
        }

        #endregion

        #region Metodos

        private void MostrarErrores(List<string> listaError)
        {
            pnlErrores.Visible = true;
            lblEstadoExito.Visible = false;
            btnAceptar.Visible = true;

            BulletedList bullet = new BulletedList();
            foreach (string itemError in listaError)
            {
                bullet.Items.Add(itemError);
            }

            pnlErrores.Controls.Add(bullet);
            pnlErrores.Visible = true;
        }

        private void ControlEnable(bool enable)
        {
            ddlTabla.Enabled = enable;
            AsyncFileUpload.Enabled = enable;
            btnCargar.Enabled = enable;
            btnCancelar.Enabled = enable;
        }

        #endregion
    }
}