using System;
using System.Web.Script.Serialization;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using BO;
using System.Web.SessionState;
using DM;

namespace WebOwner.handlers
{
    /// <summary>
    /// Summary description for HandlerVariable
    /// </summary>
    /// 
    enum ActionEnum
    {
        Update = 0,
        Save = 1,
        SaveNuevaSuite = 2
    }
    public class HandlerVariable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ActionEnum actionType = (ActionEnum)Enum.Parse(typeof(ActionEnum), HttpContext.Current.Request.Params["Action"]);
            WebOwner.handlers.HandlerLiquidacion.ResultGuardado result = new WebOwner.handlers.HandlerLiquidacion.ResultGuardado();
            LiquidadorBo oLiquidadorBo = null;
            SuitBo SuitBoTmp = null;
            SuitPropietarioBo SuitPropietarioBoTmp = null;
            List<ResponseValidateParticipacion> listError = null;
            int[] idhotles = { 365 };
            int idHotel = -1;

            try
            {
                switch (actionType)
                {
                    case ActionEnum.Update:
                        DataSuite dataSave = js.Deserialize<DataSuite>(HttpContext.Current.Request.Params["data"]);
                        result.OK = true;
                        context.Response.ContentType = "application/json";
                        result.ERROR = "";                        
                        SuitBoTmp = new SuitBo();
                        idHotel = SuitBoTmp.ObtenerSuitElHotel(dataSave.IdSuite);

                        if (dataSave != null)
                        {

                            List<DataVariable> listVariableVal = dataSave.ListDataVariable.Where(V => V.EsValidacion == true).ToList();
                            oLiquidadorBo = new LiquidadorBo();
                            foreach (var item in listVariableVal)
                            {
                                if (!idhotles.Contains(idHotel))
                                {
                                    listError = oLiquidadorBo.ValidateParticipationByHotel(dataSave.IdSuite, item.IdVariable, dataSave.IdPropietarioSeleccionado, item.Valor);
                                    if (listError.Count() > 1)
                                    {
                                        result.OK = false;
                                        result.ERROR = "La participación supera el 100%";
                                        result.ErrorDescripcion = js.Serialize(listError);
                                        result.TipoValidacion = 1;
                                        break;
                                    }

                                    if (listError.Count() < 1)
                                    {
                                        result.ERROR = "La participación no cumple con el 100% aun";
                                        result.ErrorDescripcion = js.Serialize(listError);
                                        result.TipoValidacion = 1;
                                        break;
                                    }
                                }                                

                                listError = null;
                                listError = oLiquidadorBo.ValidatePesoParticipationConSuite(dataSave.IdSuitPropietarioSeleccionado, item.IdVariable, item.Valor);
                                if (listError.Count() > 0)
                                {
                                    result.OK = false;
                                    result.ERROR = "La sumatoria de los coeficientes de propietario, supera el coeficiente de la suite";
                                    result.ErrorDescripcion = js.Serialize(listError);
                                    result.TipoValidacion = 3;
                                    break;
                                }
                            }

                            if (result.OK)
                            {
                                List<ObjetoGenerico> itemVariable = new List<ObjetoGenerico>();
                                foreach (var item in dataSave.ListDataVariable)
                                {
                                    ObjetoGenerico oItem = new ObjetoGenerico();
                                    oItem.IdValorVariableSuit = item.IdValorVariableSuit;
                                    oItem.Valor = item.Valor;
                                    itemVariable.Add(oItem);
                                }
                                SuitPropietarioBo suitPropietarioBoTmp = new SuitPropietarioBo();
                                ValorVariableBo valorVariableBo = new ValorVariableBo();
                                suitPropietarioBoTmp.Actualizar(dataSave.IdSuitPropietarioSeleccionado, dataSave.IdBanco, dataSave.NumCuenta, dataSave.NumEstadias, dataSave.TitularBanco, dataSave.TipoCuenta, "", "", dataSave.IdUsuario);
                                valorVariableBo.Actualizar(itemVariable, "", dataSave.IdUsuario);

                                result.ERROR = "Guardado con exito.";
                            }
                        }                        
                        context.Response.Write(js.Serialize(result));
                        break;

                    case ActionEnum.Save:
                        Propietario dataPropietario = js.Deserialize<Propietario>(HttpContext.Current.Request.Params["data"]);
                        if (dataPropietario != null)
                        {
                            PropietarioBo propietarioBo = new PropietarioBo();
                            SuitPropietarioBoTmp = new SuitPropietarioBo();

                            if (dataPropietario.IdPropietario > 0)
                            {
                                propietarioBo.Actualizar(dataPropietario.IdPropietario, dataPropietario.Nombre1, dataPropietario.Nombre2, dataPropietario.Apellido1, dataPropietario.Apellido2, dataPropietario.TipoPersona, dataPropietario.NumIdentificacion, dataPropietario.Correo1, dataPropietario.Correo2, dataPropietario.Correo3, dataPropietario.Activo, dataPropietario.IdCiudad, 2, dataPropietario.Direccion, dataPropietario.Tel1, dataPropietario.Tel2, dataPropietario.NomContacto, dataPropietario.TelContacto, dataPropietario.CorreoContacto, dataPropietario.TipoDoc, dataPropietario.Retencion, dataPropietario.IdUsuario);
                            }
                            else
                            {
                                int idPropi = propietarioBo.Guardar(dataPropietario.Nombre1, dataPropietario.Nombre2, dataPropietario.Apellido1, dataPropietario.Apellido2, dataPropietario.TipoPersona, dataPropietario.NumIdentificacion, dataPropietario.NumIdentificacion, dataPropietario.NumIdentificacion, dataPropietario.Correo1, dataPropietario.Correo2, dataPropietario.Correo3, dataPropietario.Activo, dataPropietario.IdCiudad, 2, dataPropietario.Direccion, dataPropietario.Tel1, dataPropietario.Tel2, "", dataPropietario.NomContacto, dataPropietario.TelContacto, dataPropietario.CorreoContacto, dataPropietario.TipoDoc, dataPropietario.Retencion);
                                List<ObjetoGenerico> listaSuitPropietario = new List<ObjetoGenerico>();
                                foreach (DataSuite item in dataPropietario.ListaDataSuite)
                                {
                                    ObjetoGenerico oObjetoGenerico = new ObjetoGenerico();
                                    oObjetoGenerico.NumCuenta = item.NumCuenta;
                                    oObjetoGenerico.NumEstadias = item.NumEstadias;
                                    oObjetoGenerico.TipoCuenta = item.TipoCuenta;
                                    oObjetoGenerico.Titular = item.TitularBanco;
                                    oObjetoGenerico.IdSuit = item.IdSuite;
                                    oObjetoGenerico.IdBanco = item.IdBanco;
                                    oObjetoGenerico.ListaVariables = new List<ObjetoGenerico>();

                                    foreach (DataVariable itemVariable in item.ListDataVariable)
                                    {
                                        ObjetoGenerico oVariable = new ObjetoGenerico();
                                        oVariable.IdVariable = itemVariable.IdVariable;
                                        oVariable.Valor = itemVariable.Valor;
                                        oObjetoGenerico.ListaVariables.Add(oVariable);
                                    }
                                    listaSuitPropietario.Add(oObjetoGenerico);
                                }
                                SuitPropietarioBoTmp.Guardar(listaSuitPropietario, idPropi);
                            }
                        }
                        result.OK = true;
                        result.ERROR = "";
                        context.Response.ContentType = "application/json";
                        context.Response.Write(js.Serialize(result));
                        break;

                    case ActionEnum.SaveNuevaSuite:
                        Propietario dataNuevaSuite = js.Deserialize<Propietario>(HttpContext.Current.Request.Params["data"]);

                        try
                        {
                            if (dataNuevaSuite != null)
                            {
                                if (dataNuevaSuite.IdPropietario < 0)
                                {
                                    PropietarioBo propietarioBo = new PropietarioBo();
                                    dataNuevaSuite.IdPropietario = propietarioBo.Guardar(dataNuevaSuite.Nombre1, dataNuevaSuite.Nombre2, dataNuevaSuite.Apellido1, dataNuevaSuite.Apellido2, dataNuevaSuite.TipoPersona, dataNuevaSuite.NumIdentificacion, dataNuevaSuite.NumIdentificacion, dataNuevaSuite.NumIdentificacion, dataNuevaSuite.Correo1, dataNuevaSuite.Correo2, dataNuevaSuite.Correo3, dataNuevaSuite.Activo, dataNuevaSuite.IdCiudad, 2, dataNuevaSuite.Direccion, dataNuevaSuite.Tel1, dataNuevaSuite.Tel2, "", dataNuevaSuite.NomContacto, dataNuevaSuite.TelContacto, dataNuevaSuite.CorreoContacto, dataNuevaSuite.TipoDoc, dataNuevaSuite.Retencion);
                                    result.IdPropietario = dataNuevaSuite.IdPropietario;
                                }

                                SuitPropietarioBo suitPropietarioBo = new SuitPropietarioBo();
                                oLiquidadorBo = new LiquidadorBo();
                                SuitBoTmp = new SuitBo();
                                SuitPropietarioBoTmp = new SuitPropietarioBo();
                                result.OK = true;
                                result.ERROR = "";

                                List<ObjetoGenerico> listaSuitPropietario = new List<ObjetoGenerico>();
                                foreach (DataSuite item in dataNuevaSuite.ListaDataSuite)
                                {
                                    ObjetoGenerico oObjetoGenerico = new ObjetoGenerico();
                                    oObjetoGenerico.NumCuenta = item.NumCuenta;
                                    oObjetoGenerico.NumEstadias = item.NumEstadias;
                                    oObjetoGenerico.TipoCuenta = item.TipoCuenta;
                                    oObjetoGenerico.Titular = item.TitularBanco;
                                    oObjetoGenerico.IdSuit = item.IdSuite;
                                    oObjetoGenerico.IdBanco = item.IdBanco;
                                    oObjetoGenerico.ListaVariables = new List<ObjetoGenerico>();
                                    
                                    foreach (DataVariable itemVariable in item.ListDataVariable)
                                    {
                                        ObjetoGenerico oVariable = new ObjetoGenerico();
                                        oVariable.IdVariable = itemVariable.IdVariable;
                                        oVariable.Valor = itemVariable.Valor;
                                        oObjetoGenerico.ListaVariables.Add(oVariable);
                                    }

                                    listaSuitPropietario.Add(oObjetoGenerico);
                                }
                                suitPropietarioBo.Guardar(listaSuitPropietario, dataNuevaSuite.IdPropietario);

                                // Validamos las variables
                                foreach (DataSuite item in dataNuevaSuite.ListaDataSuite)
                                {
                                    idHotel = SuitBoTmp.ObtenerSuitElHotel(item.IdSuite);

                                    foreach (DataVariable itemVariable in item.ListDataVariable)
                                    {
                                        if (!idhotles.Contains(idHotel))
                                        {
                                            listError = oLiquidadorBo.ValidateParticipationByHotel(item.IdSuite, itemVariable.IdVariable, dataNuevaSuite.IdPropietario, itemVariable.Valor);
                                            if (listError.Count() > 1)
                                            {
                                                result.OK = false;
                                                result.ERROR = "La participación supera el 100%";
                                                result.ErrorDescripcion = js.Serialize(listError);
                                                break;
                                            }

                                            if (listError.Count() < 1)
                                            {
                                                result.ERROR = "La participación no cumple con el 100% aun";
                                                result.ErrorDescripcion = js.Serialize(listError);
                                                break;
                                            }
                                        }

                                        listError = null;
                                        Suit_Propietario suit_PropietarioTmp = SuitPropietarioBoTmp.Obtener(dataNuevaSuite.IdPropietario, item.IdSuite);
                                        listError = oLiquidadorBo.ValidatePesoParticipationConSuite(suit_PropietarioTmp.IdSuitPropietario, itemVariable.IdVariable, itemVariable.Valor);
                                        if (listError.Count() > 0)
                                        {
                                            result.OK = false;
                                            result.ERROR = "La sumatoria de los coeficientes de propietario, supera el coeficiente de la suite";
                                            result.ErrorDescripcion = js.Serialize(listError);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            result.OK = false;
                            result.ERROR = "Error inesperado.";
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.Write(js.Serialize(result));
                        break;

                    default:
                        break;
                }                
            }
            catch (Exception ex)
            {
                result.OK = false;
                result.ERROR = ex.ToString();
                context.Response.ContentType = "application/json";
                context.Response.Write(js.Serialize(result));
            }            
        }

        public class Propietario
        {
            public int IdUsuario { get; set; }
            public int IdPropietario { get; set; }
            public string TipoPersona { get; set; }
            public bool Activo { get; set; }
            public bool Retencion { get; set; }
            public string Nombre1 { get; set; }
            public string Nombre2 { get; set; }
            public string Apellido1 { get; set; }
            public string Apellido2 { get; set; }
            public string NumIdentificacion { get; set; }
            public string TipoDoc { get; set; }
            public int IdDepto { get; set; }
            public int IdCiudad { get; set; }
            public string Direccion { get; set; }
            public string Correo1 { get; set; }
            public string Correo2 { get; set; }
            public string Correo3 { get; set; }
            public string Tel1 { get; set; }
            public string Tel2 { get; set; }
            public string NomContacto { get; set; }
            public string TelContacto { get; set; }
            public string CorreoContacto { get; set; }
            public List<DataSuite> ListaDataSuite { get; set; }

        }

        public class DataSuite
        {
            public int IdSuite { get; set; }
            public int IdBanco { get; set; }
            public string TitularBanco { get; set; }
            public string TipoCuenta { get; set; }
            public string NumCuenta { get; set; }
            public int NumEstadias { get; set; }
            public int IdSuitPropietarioSeleccionado { get; set; }
            public int IdPropietarioSeleccionado { get; set; }
            public int IdUsuario { get; set; }
            public int IdHotel { get; set; }
            public List<DataVariable> ListDataVariable { get; set; }
        }

        public class DataVariable {
            public int IdValorVariableSuit { get; set; }
            public int IdVariable { get; set; }
            public double Valor { get; set; }
            public bool EsValidacion { get; set; }
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