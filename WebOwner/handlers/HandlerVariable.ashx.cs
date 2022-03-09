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
        IsValid = 0,
        Save = 1,
        SaveNuevaSuite = 2
    }
    public class HandlerVariable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ActionEnum actionType = (ActionEnum)Enum.Parse(typeof(ActionEnum), HttpContext.Current.Request.Params["Action"]);
            Response result = new Response();
            VariablesValidacionBo variablesValidacionBo = null;
            context.Response.ContentType = "application/json";

            try
            {
                switch (actionType)
                {
                    case ActionEnum.IsValid:
                        List<ObjetoGenerico> listVariable = js.Deserialize<List<ObjetoGenerico>>(HttpContext.Current.Request.Params["data"]);
                        
                        
                        variablesValidacionBo = new VariablesValidacionBo();
                        result = variablesValidacionBo.Validar(listVariable);

                        /*
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
                        */
                        context.Response.Write(js.Serialize(result));
                        break;

                    case ActionEnum.Save:
                        /*
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
                        */
                        break;

                    case ActionEnum.SaveNuevaSuite:
                        Propietario dataNuevaSuite = js.Deserialize<Propietario>(HttpContext.Current.Request.Params["data"]);
                        /*
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
                        */
                        context.Response.ContentType = "application/json";
                        context.Response.Write(js.Serialize(result));
                        break;

                    default:
                        break;
                }                
            }
            catch (Exception ex)
            {
                result.Error = ex.ToString();
                context.Response.ContentType = "application/json";
                context.Response.Write(js.Serialize(result));
            }            
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