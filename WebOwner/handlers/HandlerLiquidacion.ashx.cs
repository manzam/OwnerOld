using System;
using System.Web.Script.Serialization;
using System.Web;
using System.Collections.Generic;
//using BO;
using BO;
using System.Web.SessionState;
using System.Data;
using System.Text;

namespace WebOwner.handlers
{
    /// <summary>
    /// Logica de liquidacion
    /// </summary>
    /// 

    enum ActionTypeEnum
    {
        GetOwnerByHotel = 0,
        GetReglas = 1,
        SaveLiqHotel = 3,
        SaveLiqProp = 4,
        DeleteLiq = 5,
        ActiveConcepto = 6,
        ValidateParticipation = 7,
        ValidateCoeficiente = 8
    }

    public class HandlerLiquidacion : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            ConceptoBo conceptoBoTmp = new ConceptoBo();
            LiquidadorBo LiquidadorBoTmp = new LiquidadorBo();
            JavaScriptSerializer js = new JavaScriptSerializer();
            ActionTypeEnum actionType = (ActionTypeEnum)Enum.Parse(typeof(ActionTypeEnum), HttpContext.Current.Request.Params["ActionType"]);

            int idHotel, yyyy, mm;

            int.TryParse(HttpContext.Current.Request.Params["IdHotel"], out idHotel);
            int idUsuario = ((ObjetoGenerico)HttpContext.Current.Session["usuarioLogin"]).Id;
            int.TryParse(HttpContext.Current.Request.Params["YYYY"], out yyyy);
            int.TryParse(HttpContext.Current.Request.Params["MM"], out mm);            

            string error = string.Empty;
            bool ok = true;
            ResultGuardado ResultGuardadoTmp = null;

            switch (actionType)
            {
                case ActionTypeEnum.GetOwnerByHotel:

                    List<LiquidadorOwners> lstOwners = LiquidadorBoTmp.ObtenerPropietariosConSuiteActivas(idHotel);
                    List<LiquidadorRegla> lstReglaH = LiquidadorBoTmp.ObtenerReglaHotel(idHotel);
                    List<LiquidadorValorVariable> lstVariablesValor = LiquidadorBoTmp.ObtenerVariablesValor(idHotel, yyyy, mm);
                    bool okLiqHotel = LiquidadorBoTmp.LiquidacionHotel(idHotel, yyyy, mm);

                    ResultGetOwnerByHotel ResultGetOwnerByHotelTmp = new ResultGetOwnerByHotel();
                    ResultGetOwnerByHotelTmp.Owners = lstOwners;
                    ResultGetOwnerByHotelTmp.Regla = lstReglaH;
                    ResultGetOwnerByHotelTmp.VariableValor = lstVariablesValor;
                    ResultGetOwnerByHotelTmp.OKLiquidacionHotel = okLiqHotel;

                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(ResultGetOwnerByHotelTmp));
                    break;

                case ActionTypeEnum.GetReglas:

                    List<LiquidadorRegla> lstReglaP = LiquidadorBoTmp.ObtenerReglaPorHotel(idHotel);

                    ResultGetReglaValor ResultGetReglaValorTmp = new ResultGetReglaValor();
                    ResultGetReglaValorTmp.Regla = lstReglaP;

                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(ResultGetReglaValorTmp));
                    break;

                case ActionTypeEnum.SaveLiqHotel:

                    List<LiquidacionHotel> lstLiquidacionHotel = js.Deserialize<List<LiquidacionHotel>>(HttpContext.Current.Request.Params["LiquidacionHotel"]);
                    ok = LiquidadorBoTmp.GuardarLiquidacionHotel(lstLiquidacionHotel, idUsuario, yyyy, mm, idHotel, ref error);

                    ResultGuardadoTmp = new ResultGuardado();
                    ResultGuardadoTmp.ERROR = error;
                    ResultGuardadoTmp.OK = ok;

                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(ResultGuardadoTmp));
                    break;

                case ActionTypeEnum.SaveLiqProp:

                    List<LiquidacionProp> lstLiquidacionProp = js.Deserialize<List<LiquidacionProp>>(HttpContext.Current.Request.Params["LiquidacionProp"]);
                    bool isSel = bool.Parse(HttpContext.Current.Request.Params["IsSel"]);
                    ok = LiquidadorBoTmp.GuardarLiquidacionProp(lstLiquidacionProp, idUsuario, yyyy, mm, idHotel, isSel, ref error);

                    ResultGuardadoTmp = new ResultGuardado();
                    ResultGuardadoTmp.ERROR = error;
                    ResultGuardadoTmp.OK = ok;

                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(ResultGuardadoTmp));
                    break;

                case ActionTypeEnum.DeleteLiq:

                    bool okDeleteLiq = LiquidadorBoTmp.EliminarLiquidacion(yyyy, mm, idHotel, ref error);

                    ResultDelete ResultDeleteTmp = new ResultDelete();
                    ResultDeleteTmp.OK = okDeleteLiq;
                    ResultDeleteTmp.ERROR = error;

                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(ResultDeleteTmp));
                    break;

                case ActionTypeEnum.ActiveConcepto:
                    int idConcepto = int.Parse(HttpContext.Current.Request.Params["idConcepto"]);
                    conceptoBoTmp.ActivarConcepto(idConcepto);
                    break;

                case ActionTypeEnum.ValidateParticipation:
                    List<ResponseValidateParticipacion> listPropPartipacion = LiquidadorBoTmp.ValidateParticipationByHotel(idHotel, ref error);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(listPropPartipacion));
                    break;

                case ActionTypeEnum.ValidateCoeficiente:
                    decimal value = LiquidadorBoTmp.ValidateCoefecienteByHotel(idHotel, ref error);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(value));
                    break;

                default:
                    break;
            }

        }

        public class ResultGuardado
        {
            public bool OK { get; set; }
            public string ERROR { get; set; }
            public string ErrorDescripcion { get; set; }
        }

        public class ResultDelete
        {
            public bool OK { get; set; }
            public string ERROR { get; set; }
        }

        public class ResultGetReglaValor
        {
            public List<LiquidadorRegla> Regla { get; set; }
        }

        public class ResultGetOwnerByHotel
        {
            public List<LiquidadorRegla> Regla { get; set; }
            public List<LiquidadorOwners> Owners { get; set; }
            public List<LiquidadorValorVariable> VariableValor { get; set; }
            public bool OKLiquidacionHotel { get; set; }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
