using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DM;
using BO;
using Servicios;
using System.Web.Security;

namespace WebOwner.Templates
{
    public partial class TemplatePrincipal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.hfIsPostback.Value = "F";
            try
            {
                lblUsuarioLogin.Text = "Bienvenido : " + ((ObjetoGenerico)Session["usuarioLogin"]).PrimeroNombre + " " + ((ObjetoGenerico)Session["usuarioLogin"]).SegundoNombre + " " + ((ObjetoGenerico)Session["usuarioLogin"]).PrimerApellido + " " + ((ObjetoGenerico)Session["usuarioLogin"]).SegundoApellido;
                CargarModulos();                
            }
            catch (Exception ex)
            {
                //Response.Redirect("http://www.hotelesestelar.com/", true);
                Response.Redirect("~/Default.aspx", true);
            }

            if (!IsPostBack)
            {
                Utilities.RutaServidor = this.Server.MapPath("~");
                this.hfIsPostback.Value = "T";
            }            
        }

        private void CargarModulos()
        {
            ModuloBo moduloBoTmp = new ModuloBo();

            string grupo = "";
            short id = 0;
            HtmlGenericControl controlHtml = null;
            HtmlGenericControl controlHtmlDiv = null;
            HtmlGenericControl controlHtmlDivModulos = null;
            HtmlGenericControl controlHtmlUl = null;
            HtmlGenericControl controlHtmlLi = null;

            foreach (Modulo moduloTmp in moduloBoTmp.VerTodos(((ObjetoGenerico)Session["usuarioLogin"]).IdPerfil))
            {
                if (moduloTmp.Grupo == null || moduloTmp.Grupo == string.Empty)
                    continue;

                if (grupo != moduloTmp.Grupo)
                {
                    id++;

                    if (controlHtmlDivModulos != null)
                        this.modulos.Controls.Add(controlHtmlDivModulos);

                    #region Nombre del grupo

                    controlHtml = new HtmlGenericControl();
                    controlHtml.TagName = "h3";
                    controlHtml.InnerText = moduloTmp.Grupo;

                    controlHtmlDiv = new HtmlGenericControl();
                    controlHtmlDiv.TagName = "div";
                    controlHtmlDiv.Style.Add(System.Web.UI.HtmlTextWriterStyle.Cursor, "pointer");
                    controlHtmlDiv.Attributes.Add("onclick", "SlideDown('ctl00_grupo_" + id + "');");
                    controlHtmlDiv.Controls.Add(controlHtml);

                    controlHtmlLi = new HtmlGenericControl();
                    controlHtmlLi.TagName = "li";
                    controlHtmlLi.Controls.Add(controlHtmlDiv);

                    controlHtmlUl = new HtmlGenericControl();
                    controlHtmlUl.TagName = "ul";
                    controlHtmlUl.Controls.Add(controlHtmlLi);

                    this.modulos.Controls.Add(controlHtmlUl);                   

                    #endregion

                    #region Modulos

                    controlHtmlDivModulos = new HtmlGenericControl();
                    controlHtmlDivModulos.TagName = "div";
                    controlHtmlDivModulos.ID = "grupo_" + id;
                    controlHtmlDivModulos.Attributes.Add("class", "subMenu");
                    controlHtmlDivModulos.Style.Add(System.Web.UI.HtmlTextWriterStyle.Display, "none");
                    controlHtmlDivModulos.Style.Add(System.Web.UI.HtmlTextWriterStyle.Width, "auto");
                    controlHtmlDivModulos.Style.Add(System.Web.UI.HtmlTextWriterStyle.TextAlign, "left");

                    HyperLink linkTmp = new HyperLink();
                    linkTmp.Text = GetGlobalResourceObject("Resource", moduloTmp.NombreRecuros).ToString();
                    linkTmp.NavigateUrl = moduloTmp.Ruta + "?idMenu=ctl00_grupo_" + id;

                    controlHtmlLi = new HtmlGenericControl();
                    controlHtmlLi.TagName = "li";
                    controlHtmlLi.Controls.Add(linkTmp);

                    controlHtmlUl = new HtmlGenericControl();
                    controlHtmlUl.TagName = "ul";
                    controlHtmlUl.Controls.Add(controlHtmlLi);

                    controlHtmlDivModulos.Controls.Add(controlHtmlUl);                    

                    #endregion

                    grupo = moduloTmp.Grupo;                    
                }
                else
                {
                    HyperLink linkTmp = new HyperLink();
                    linkTmp.Text = GetGlobalResourceObject("Resource", moduloTmp.NombreRecuros).ToString();
                    linkTmp.NavigateUrl = moduloTmp.Ruta + "?idMenu=ctl00_grupo_" + id;

                    controlHtmlLi = new HtmlGenericControl();
                    controlHtmlLi.TagName = "li";
                    controlHtmlLi.Controls.Add(linkTmp);

                    controlHtmlUl.Controls.Add(controlHtmlLi);
                    controlHtmlDivModulos.Controls.Add(controlHtmlUl);
                }                
            }

            if (controlHtmlDivModulos != null)
                this.modulos.Controls.Add(controlHtmlDivModulos);
        }
    }
}
