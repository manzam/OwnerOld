using System;
using System.Web.Script.Serialization;
using System.Web;
using System.Collections.Generic;
//using BO;
using BO;
using System.Web.SessionState;

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

            try
            {
                switch (actionType)
                {
                    case ActionEnum.Update:
                        DataSuite dataSave = js.Deserialize<DataSuite>(HttpContext.Current.Request.Params["data"]);

                        if (dataSave != null)
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
                        }
                        result.OK = true;
                        result.ERROR = "";
                        context.Response.ContentType = "application/json";
                        context.Response.Write(js.Serialize(result));
                        break;

                    case ActionEnum.Save:
                        Propietario dataPropietario = js.Deserialize<Propietario>(HttpContext.Current.Request.Params["data"]);
                        if (dataPropietario != null)
                        {
                            PropietarioBo propietarioBo = new PropietarioBo();
                            SuitPropietarioBo suitPropietarioBo = new SuitPropietarioBo();

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
                                suitPropietarioBo.Guardar(listaSuitPropietario, idPropi);
                            }
                        }
                        result.OK = true;
                        result.ERROR = "";
                        context.Response.ContentType = "application/json";
                        context.Response.Write(js.Serialize(result));
                        break;

                    case ActionEnum.SaveNuevaSuite:
                        Propietario dataNuevaSuite = js.Deserialize<Propietario>(HttpContext.Current.Request.Params["data"]);
                        if (dataNuevaSuite.IdPropietario < 0) {
                            result.OK = true;
                            result.ERROR = "";
                            context.Response.ContentType = "application/json";
                            context.Response.Write(js.Serialize(result));
                            break;
                        }
                            

                        if (dataNuevaSuite != null)
                        {
                            SuitPropietarioBo suitPropietarioBo = new SuitPropietarioBo();

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

                        }
                        result.OK = true;
                        result.ERROR = "";
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
            public int IdUsuario { get; set; }
            public List<DataVariable> ListDataVariable { get; set; }
        }

        public class DataVariable {
            public int IdValorVariableSuit { get; set; }
            public int IdVariable { get; set; }
            public double Valor { get; set; }
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