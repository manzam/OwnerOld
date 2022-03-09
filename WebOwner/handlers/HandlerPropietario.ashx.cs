using BO;
using DM;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebOwner.handlers
{
    /// <summary>
    /// Summary description for HandlerPropietario
    /// </summary>
    /// 
    enum ActionOwnerEnum
    {
        SaveOwner = 0,
        UpdateOwner = 1,
        GetDepto = 2,
        GetCity = 3,
        GetHotel = 4,
        GetSuite = 5,
        GetVariables = 6,
        GetBank = 7,
        LoadOwner = 8,
        ActiveSuite = 9,
        LoadSuite = 10,
        UpdateVariables = 11
    }
    public class HandlerPropietario : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int idUsuario = -1;
            int idPropietario = -1;
            int idSuitPropietario = -1;
            int idSuit = -1;

            if (context.Session["usuarioLogin"] != null)
                idUsuario = ((ObjetoGenerico)context.Session["usuarioLogin"]).Id;

            JavaScriptSerializer js = new JavaScriptSerializer();
            ActionOwnerEnum actionType = (ActionOwnerEnum)Enum.Parse(typeof(ActionOwnerEnum), HttpContext.Current.Request.Params["Action"]);

            ObjetoGenerico objetoResponse = null;
            ObjetoGenerico dataPropietario = null;
            Suit_Propietario suitPropietario = null;
            Propietario propietario = null;

            PropietarioBo propietarioBo = null;
            SuitBo suitBo = null;
            SuitPropietarioBo suitPropietarioBo = null;
            ValorVariableBo ValorVariableBo = null;

            switch (actionType)
            {
                case ActionOwnerEnum.SaveOwner:
                    objetoResponse = new ObjetoGenerico();
                    try
                    {
                        
                        propietarioBo = new PropietarioBo();
                        suitPropietarioBo = new SuitPropietarioBo();
                        ValorVariableBo = new ValorVariableBo();

                        dataPropietario = js.Deserialize<ObjetoGenerico>(HttpContext.Current.Request.Params["data"]);
                        propietario = this.GetPropietarioModel(dataPropietario);

                        Propietario propietarioRef = null;
                        if (propietario.IdPropietario < 0)
                            propietarioRef = propietarioBo.ObtenerPropietario(dataPropietario.NumIdentificacion);

                        if (propietario.IdPropietario < 0)
                            idPropietario = propietarioBo.Guardar(propietario, idUsuario);
                        else
                            idPropietario = propietarioBo.Actualizar(propietario, idUsuario);


                        if (dataPropietario.ListaVariables != null)
                        {
                            dataPropietario.IdPropietario = idPropietario;
                            suitPropietario = GetSuiteModel(dataPropietario);
                            idSuitPropietario = suitPropietarioBo.Guardar(suitPropietario);
                            dataPropietario.IdSuitPropietario = idSuitPropietario;
                            List<Valor_Variable_Suit> listValorVariableSuit = GetVariableModel(dataPropietario);
                            ValorVariableBo.Guardar(listValorVariableSuit);
                        }

                        objetoResponse.Ok = true;
                        objetoResponse.Succes = "Guardado con exito";
                        context.Response.ContentType = "application/json";
                        context.Response.Write(js.Serialize(objetoResponse));
                    }
                    catch (Exception ex)
                    {
                        objetoResponse.Ok = false;
                        objetoResponse.Error = "Error al guardar.";
                        objetoResponse.ErrorExeption = $"Message: {ex.Message} InnerException: {ex.InnerException} StackTrace: {ex.StackTrace}";

                        propietarioBo.EliminarPropietario(idPropietario);

                        context.Response.ContentType = "application/json";
                        context.Response.Write(js.Serialize(objetoResponse));
                    }

                    break;
                case ActionOwnerEnum.UpdateOwner:
                    dataPropietario = js.Deserialize<ObjetoGenerico>(HttpContext.Current.Request.Params["data"]);
                    propietario = this.GetPropietarioModel(dataPropietario);



                    break;
                case ActionOwnerEnum.GetDepto:
                    DeptoBo deptoBo = new DeptoBo();
                    List<Departamento> listDepto = deptoBo.ObtenerTodos();
                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(listDepto.Select(D => new { IdDepto = D.IdDepartamento, Name = D.Nombre })));
                    break;
                case ActionOwnerEnum.GetCity:
                    int idDepto = int.Parse(HttpContext.Current.Request.Params["data"]);
                    CiudadBo ciudadBo = new CiudadBo();
                    List<Ciudad> listCiudad = ciudadBo.ObtenerPorDepto(idDepto);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(listCiudad.Select(C => new { IdCity = C.IdCiudad, Name = C.Nombre })));
                    break;
                case ActionOwnerEnum.GetHotel:
                    HotelBo hotelBo = new HotelBo();
                    List<DM.Hotel> listHotel = hotelBo.VerTodos(idUsuario);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(listHotel.Select(H => new { IdHotel = H.IdHotel, Name = H.Nombre })));
                    break;
                case ActionOwnerEnum.GetSuite:
                    int idHotel = int.Parse(HttpContext.Current.Request.Params["data"]);
                    suitBo = new SuitBo();
                    List<Suit> listSuite = suitBo.ObtenerSuitsPorHotel(idHotel);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(listSuite.Select(S => new { IdSuite = S.IdSuit, NumSuite = S.NumSuit, NumEsc = S.NumEscritura })));
                    break;
                case ActionOwnerEnum.GetVariables:
                    ObjetoGenerico data = js.Deserialize<ObjetoGenerico>(HttpContext.Current.Request.Params["data"]);
                    VariableBo variablesSuiteBoTmp = new VariableBo();
                    suitBo = new SuitBo();                    
                    string desc = suitBo.ObtenerSuit(data.IdSuit).Descripcion;                    
                    List<Variable> listaVariables = variablesSuiteBoTmp.VerTodos(data.IdHotel, true, "P");
                    
                    objetoResponse = new ObjetoGenerico();
                    objetoResponse.NombreSuit = desc;
                    objetoResponse.ListaVariables = (from V in listaVariables select new ObjetoGenerico() { IdVariable = V.IdVariable, Nombre = V.Nombre, Valor = 0 }).ToList();

                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(objetoResponse));
                    break;
                case ActionOwnerEnum.GetBank:
                    BancoBo bancoBo = new BancoBo();
                    List<Banco> listBanco = bancoBo.VerTodos();
                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(listBanco.Select(B => new { IdBanco = B.IdBanco, Name = B.Nombre })));
                    break;
                case ActionOwnerEnum.LoadOwner:
                    idPropietario = int.Parse(HttpContext.Current.Request.Params["data"]);
                    propietarioBo = new PropietarioBo();
                    ObjetoGenerico propietarioTmp = propietarioBo.ObtenerPropietario(idPropietario);

                    suitBo = new SuitBo();
                    propietarioTmp.ListaSuite = suitBo.ObtenerSuitsPorPropietario(idPropietario);

                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(propietarioTmp));
                    break;
                case ActionOwnerEnum.ActiveSuite:
                    idSuitPropietario = int.Parse(HttpContext.Current.Request.Params["data"]);
                    suitPropietarioBo = new SuitPropietarioBo();
                    bool isActive = suitPropietarioBo.Activar(idSuitPropietario);

                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(isActive));
                    break;
                case ActionOwnerEnum.LoadSuite:
                    idSuitPropietario = int.Parse(HttpContext.Current.Request.Params["data"]);
                    suitPropietarioBo = new SuitPropietarioBo();
                    objetoResponse = suitPropietarioBo.Obtener(idSuitPropietario);

                    context.Response.ContentType = "application/json";
                    context.Response.Write(js.Serialize(objetoResponse));
                    break;
                case ActionOwnerEnum.UpdateVariables:
                    context.Response.ContentType = "application/json";
                    try
                    {
                        dataPropietario = js.Deserialize<ObjetoGenerico>(HttpContext.Current.Request.Params["data"]);
                        suitPropietarioBo = new SuitPropietarioBo();
                        suitPropietarioBo.Actualizar(dataPropietario.IdSuitPropietario, dataPropietario.IdBanco, dataPropietario.NumCuenta, dataPropietario.NumEstadias, dataPropietario.Titular, dataPropietario.TipoCuenta, "", "", idUsuario);
                        ValorVariableBo = new ValorVariableBo();
                        ValorVariableBo.Actualizar(dataPropietario.ListaVariables);
                        
                        context.Response.Write(js.Serialize(objetoResponse));
                    }
                    catch (Exception ex)
                    {
                        context.Response.Write(js.Serialize(ex.Message));
                    }
                    
                    break;

                default:
                    break;
            }
        }

        public Propietario GetPropietarioModel(ObjetoGenerico owner)
        {
            string clave = Utilities.EncodePassword(String.Concat(owner.NumIdentificacion, owner.NumIdentificacion));

            Propietario propietarioTmp = new Propietario();
            propietarioTmp.IdPropietario = owner.IdPropietario;
            propietarioTmp.NombrePrimero = owner.PrimeroNombre.Trim();
            propietarioTmp.NombreSegundo = owner.SegundoNombre.Trim();
            propietarioTmp.ApellidoPrimero = owner.PrimerApellido.Trim();
            propietarioTmp.ApellidoSegundo = owner.SegundoApellido.Trim();
            propietarioTmp.TipoPersona = owner.TipoPersona;
            propietarioTmp.TipoDocumento = owner.TipoDocumento;
            propietarioTmp.FechaIngreso = DateTime.Now;
            propietarioTmp.NumIdentificacion = owner.NumIdentificacion.Trim();
            propietarioTmp.Login = owner.NumIdentificacion.Trim();
            propietarioTmp.Pass = clave;
            propietarioTmp.Activo = owner.Activo;
            propietarioTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", owner.IdCiudad);
            propietarioTmp.PerfilReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Perfil", "IdPerfil", 2);
            propietarioTmp.Correo = owner.Correo.Trim();
            propietarioTmp.Correo2 = owner.Correo2.Trim();
            propietarioTmp.Correo3 = owner.Correo3.Trim();
            propietarioTmp.Direccion = owner.Direccion;
            propietarioTmp.Telefono_1 = owner.Telefono1;
            propietarioTmp.Telefono_2 = owner.Telefono2;
            propietarioTmp.Telefono_3 = owner.Telefono3;
            propietarioTmp.NombreContacto = owner.NombreContacto;
            propietarioTmp.TelefonoContacto = owner.TelContacto;
            propietarioTmp.CorreoContacto = owner.CorreoContacto.Trim();
            propietarioTmp.EsRetenedor = owner.EsRetenedor;
            propietarioTmp.Cambio = true;

            return propietarioTmp;
        }

        public Suit_Propietario GetSuiteModel(ObjetoGenerico owner)
        {

            Suit_Propietario SuitPropietarioTmp = new Suit_Propietario();
            SuitPropietarioTmp.SuitReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit", "IdSuit", owner.IdSuit);
            SuitPropietarioTmp.PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Propietario", "IdPropietario", owner.IdPropietario);
            SuitPropietarioTmp.BancoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Banco", "IdBanco", owner.IdBanco);
            SuitPropietarioTmp.NumCuenta = owner.NumCuenta;
            SuitPropietarioTmp.NumEstadias = owner.NumEstadias;
            SuitPropietarioTmp.TipoCuenta = owner.TipoCuenta;
            SuitPropietarioTmp.Titular = owner.Titular;
            SuitPropietarioTmp.EsActivo = owner.Activo;

            return SuitPropietarioTmp;
        }

        public List<Valor_Variable_Suit> GetVariableModel(ObjetoGenerico owner)
        {

            List<Valor_Variable_Suit> listValorVariableSuit = new List<Valor_Variable_Suit>();
            foreach (var item in owner.ListaVariables)
            {
                Valor_Variable_Suit valorVariableSuit = new Valor_Variable_Suit();
                valorVariableSuit.Suit_PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit_Propietario", "IdSuitPropietario", owner.IdSuitPropietario);
                valorVariableSuit.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", item.IdVariable);
                valorVariableSuit.Valor = item.Valor;
                listValorVariableSuit.Add(valorVariableSuit);
            }
            return listValorVariableSuit;
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