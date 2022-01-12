using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Servicios;
using System.IO;
using System.Text;
using BO;
using AjaxControlToolkit;

namespace WebOwner.ui.WebUserControls
{
    public partial class WebUserInterfaz : System.Web.UI.UserControl
    {
        HotelBo hotelBoTmp;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 10; i--)
                {
                    ddlAno.Items.Add(new ListItem() { Text = i.ToString(), Value = i.ToString() });
                }

                //ParametroBo parametroBoTmp = new ParametroBo();
                //gvwPropietarios.PageSize = int.Parse(parametroBoTmp.ObtenerValor("PageSize"));

                //List<ObjetoGenerico> listaIdSuiteIdPropietario = new List<ObjetoGenerico>();
                //Session["listaIdSuiteIdPropietario"] = listaIdSuiteIdPropietario;

                this.CargarCombos();
                //this.CargarPropietarios();
            }
        }

        #region Propiedades
        public ObjetoGenerico UsuarioLogin
        {
            get { return (ObjetoGenerico)Session["usuarioLogin"]; }
        }
        #endregion

        #region Metodo

        private void CargarCombos()
        {
            HotelBo hotelBoTmp = new HotelBo();

            if (((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil == Properties.Settings.Default.IdSuperUsuario)
                ddlHotel.DataSource = hotelBoTmp.VerTodos();            
            else
                ddlHotel.DataSource = hotelBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).Id);

            ddlHotel.DataTextField = "Nombre";
            ddlHotel.DataValueField = "IdHotel";
            ddlHotel.DataBind();
        }

        private void CrearArchivo(DateTime fechaPeriodo, StringBuilder texto)
        {
            DateTime fecha = DateTime.Now;
            string nombreArchivo = fechaPeriodo.Month + "_" + fechaPeriodo.Year + "_" + fechaPeriodo.ToString("hh_mm_ss") + ".txt";
            string ruta = Server.MapPath("../../interfases/Interfaz_" + nombreArchivo);

            #region Auditoria
            AuditoriaBo auditoriaBo = new AuditoriaBo();
            auditoriaBo.Guardar("Interfaz generada Nombre Hotel : " + ddlHotel.SelectedItem.Text + " Nombre Archivo : " + nombreArchivo, "", "Interfaz", "Insertar", "Interfaz generada", DateTime.Now, this.UsuarioLogin.Id);
            #endregion

            System.IO.StreamWriter sw = new System.IO.StreamWriter(ruta);
            string miTexto = texto.ToString().Remove(texto.ToString().Length - 1);

            sw.WriteLine(miTexto.TrimEnd());            
            sw.Close();
            FileInfo fileInfo = new FileInfo(ruta);

            Response.Clear();
            Response.ClearHeaders();
            Response.ContentType = "text/plain";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
            //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileInfo.Name);
            Response.AppendHeader("Content-Length", fileInfo.Length.ToString());

            Response.WriteFile(ruta);
            
            Response.Flush();    
            //Response.End();
            //Response.Close();

            //Session["fileInfo"] = fileInfo;
            //Session["ruta"] = ruta;           
        }

        #endregion

        #region Boton

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaPeriodo = new DateTime(int.Parse(ddlAno.SelectedValue), int.Parse(ddlMes.SelectedValue), 1, 00, 00, 00);
                int idHotel = int.Parse(ddlHotel.SelectedValue);

                hotelBoTmp = new HotelBo();
                int consecutivoPlano = 0; //hotelBoTmp.ObtenerConsecutivo(idHotel);

                //List<ObjetoGenerico> listaIdSuiteIdPropietario = (List<ObjetoGenerico>)Session["listaIdSuiteIdPropietario"];
                List<ObjetoGenerico> listaIdSuiteIdPropietario = new List<ObjetoGenerico>();
                InterfazBo interfazBoTmp = new InterfazBo(fechaPeriodo, idHotel, listaIdSuiteIdPropietario, string.Empty);

                interfazBoTmp.F_TIPO_REG_Inicio = Properties.Settings.Default.F_TIPO_REG_Inicio;
                interfazBoTmp.F_SUBTIPO_REG_Inicio = Properties.Settings.Default.F_SUBTIPO_REG_Inicio;
                interfazBoTmp.F_VERSION_REG_Inicio = Properties.Settings.Default.F_VERSION_REG_Inicio;
                interfazBoTmp.F_CIA_Inicio = Properties.Settings.Default.F_CIA_Inicio;
                interfazBoTmp.F_TIPO_REG_Final = Properties.Settings.Default.F_TIPO_REG_Final;
                interfazBoTmp.F_SUBTIPO_REG_Final = Properties.Settings.Default.F_SUBTIPO_REG_Final;
                interfazBoTmp.F_VERSION_REG_Final = Properties.Settings.Default.F_VERSION_REG_Final;
                interfazBoTmp.F_CIA_Final = Properties.Settings.Default.F_CIA_Final;
                interfazBoTmp.F_TIPO_REG = Properties.Settings.Default.F_TIPO_REG;
                interfazBoTmp.F_SUBTIPO_REG = Properties.Settings.Default.F_SUBTIPO_REG;
                interfazBoTmp.F_VERSION_REG = Properties.Settings.Default.F_VERSION_REG;
                interfazBoTmp.F_CIA = Properties.Settings.Default.F_CIA;
                interfazBoTmp.F351_ID_UN = Properties.Settings.Default.F351_ID_UN;
                interfazBoTmp.F353_ID_SUCURSAL = Properties.Settings.Default.F353_ID_SUCURSAL;
                interfazBoTmp.F353_NRO_CUOTA_CRUCE = Properties.Settings.Default.F353_NRO_CUOTA_CRUCE;
                interfazBoTmp.F_SUBTIPO_REG_MXP = Properties.Settings.Default.F_SUBTIPO_REG_MXP;
                interfazBoTmp.F353_ID_FE = Properties.Settings.Default.F353_ID_FE;
                interfazBoTmp.F_CONSEC_AUTO_REG = Properties.Settings.Default.F_CONSEC_AUTO_REG;
                interfazBoTmp.F_TIPO_REG_ENCABEZADO = Properties.Settings.Default.F_TIPO_REG_ENCABEZADO;
                interfazBoTmp.F350_ID_TIPO_DOCTO = Properties.Settings.Default.F350_ID_TIPO_DOCTO;
                interfazBoTmp.CodigoHotel = hotelBoTmp.Obtener(idHotel).Codigo;
                interfazBoTmp.NitHotelEstelar = Properties.Settings.Default.NitHotelEstelar;
                interfazBoTmp.F350_ID_CLASE_DOCTO = Properties.Settings.Default.F350_ID_CLASE_DOCTO;
                interfazBoTmp.F350_IND_ESTADO = Properties.Settings.Default.F350_IND_ESTADO;
                interfazBoTmp.F350_IND_IMPRESION = Properties.Settings.Default.F350_IND_IMPRESION;

                StringBuilder texto = interfazBoTmp.GenerarInterfas(consecutivoPlano, fechaPeriodo);
                
                //this.CargarPropietarios();

                if (interfazBoTmp.ListaErrores.Length > 0)
                {
                    interfazBoTmp.ListaErrores.AppendLine("");
                    interfazBoTmp.ListaErrores.AppendLine("");
                    interfazBoTmp.ListaErrores.AppendLine("Lista de Errores");
                    this.CrearArchivo(fechaPeriodo, interfazBoTmp.ListaErrores);
                }
                else
                    this.CrearArchivo(fechaPeriodo, texto);

                lbDescargarArchivo.Visible = true;

                this.divExito.Visible = true;
                this.lbltextoExito.Text = Resources.Resource.lblMensajeArchivoGenerado;
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

        protected void imgBtnSeleccion_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgBtnTmp = (ImageButton)sender;
            string[] ids = imgBtnTmp.CommandArgument.Split(',');

            List<ObjetoGenerico> listaIdSuiteIdPropietario = (List<ObjetoGenerico>)Session["listaIdSuiteIdPropietario"];

            if (imgBtnTmp.ImageUrl == "~/img/117.png")
            {
                imgBtnTmp.ImageUrl = "~/img/95.png";

                ObjetoGenerico objIdSuiteIdPropietario = new ObjetoGenerico();
                objIdSuiteIdPropietario.IdSuit = int.Parse(ids[0]);
                objIdSuiteIdPropietario.IdPropietario = int.Parse(ids[1]);
                listaIdSuiteIdPropietario.Add(objIdSuiteIdPropietario);
            }
            else
            {
                imgBtnTmp.ImageUrl = "~/img/117.png";
                listaIdSuiteIdPropietario.RemoveAll(I => I.IdSuit == int.Parse(ids[0]) && I.IdPropietario == int.Parse(ids[1]));
            }
        }

        #endregion

        #region Eventos

        //protected void gvwPropietarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvwPropietarios.PageIndex = e.NewPageIndex;
        //    CargarPropietarios();
        //}

        //protected void gvwPropietarios_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        int idSuite = int.Parse(gvwPropietarios.DataKeys[e.Row.RowIndex]["IdSuit"].ToString());
        //        int idPropietario = int.Parse(gvwPropietarios.DataKeys[e.Row.RowIndex]["IdPropietario"].ToString());

        //        ImageButton imgBtnTmp = (ImageButton)e.Row.FindControl("imgBtnSeleccion");

        //        List<ObjetoGenerico> listaIdSuiteIdPropietario = (List<ObjetoGenerico>)Session["listaIdSuiteIdPropietario"];

        //        if (listaIdSuiteIdPropietario.Count > 0)
        //        {
        //            if (listaIdSuiteIdPropietario.Where(I => I.IdSuit == idSuite && I.IdPropietario == idPropietario).Count() > 0)
        //            {
        //                imgBtnTmp.ImageUrl = "~/img/95.png";
        //                ObjetoGenerico objIdSuiteIdPropietario = new ObjetoGenerico();
        //                objIdSuiteIdPropietario.IdSuit = idSuite;
        //                objIdSuiteIdPropietario.IdPropietario = idPropietario;

        //                listaIdSuiteIdPropietario.Add(objIdSuiteIdPropietario);
        //            }
        //            else
        //            {
        //                imgBtnTmp.ImageUrl = "~/img/117.png";
        //                listaIdSuiteIdPropietario.RemoveAll(I => I.IdSuit == idSuite && I.IdPropietario == idPropietario);
        //            }
        //        }
        //    }
        //}

        //protected void ddlHotel_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    List<ObjetoGenerico> listaIdSuiteIdPropietario = new List<ObjetoGenerico>();
        //    Session["listaIdSuiteIdPropietario"] = listaIdSuiteIdPropietario;

        //    this.CargarPropietarios();
        //}

        #endregion
    }
}