using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Servicios;
using DM;

namespace BO
{
    public class CargaMasivaBo
    {
        CiudadBo ciudadBoTmp = null;
        PropietarioBo propietarioBoTmp = null;
        SuitPropietarioBo suitPropietarioBoTmp = null;
        ConfiguracionCertificadoBo configCertificadoBoTmp = null;
        SuitBo suitBoTmp = null;
        BancoBo bancoBoTmp = null;
        HotelBo hotelBoTmp = null;

        public int Cont { get; set; }
        public string Ruta { get; set; }
        public int IdPerfilPropietario { get; set; }
        public int IdCiudadDefault { get; set; }
        public int IdBancoDefault { get; set; }
        public string NombreBancoDefault { get; set; }
        private List<string> listaError;
        public StringBuilder ListaLineas { get; set; }
        public StreamReader objReader { get; set; }
        public int NumEstadias { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Fecha { get; set; }

        public CargaMasivaBo()
        {
            listaError = new List<string>();
            Fecha = DateTime.Now;
        }

        public void CerrarArchivo()
        {
            try
            {
                this.objReader.Close();
            }
            catch (Exception ex)
            {
            }
        }

        #region Hotel Suit

        public List<string> Cargar_HotelSuit()
        {
            try
            {

                if (this.ValidarLineasHotel())
                {
                    HotelBo hotelBoTmp = new HotelBo();
                    CiudadBo ciudadBoTmp = new CiudadBo();
                    SuitBo suitBoTmp = new SuitBo();

                    objReader = new StreamReader(this.Ruta, Encoding.Default);
                    string lineaTmp = "";

                    // [ Nombre Hotel, Nit Hotel, Dirección Hotel, Correo Hotel, Correo de reservas, Código Hotel, UNIDAD DE NEGOCIO, Ciudad Hotel,
                    //   Numero Suite Hotel, Numero Suite Escritura, Escritura No, Descripción suite ]

                    // List<Hotel> listaHotelTmp = new List<Hotel>();
                    List<Suit> listaSuitTmp = new List<Suit>();

                    List<Hotel> listaHotelLocal = hotelBoTmp.VerTodos();
                    List<Ciudad> listaCiudadLocal = ciudadBoTmp.ObtenerTodos();

                    while (lineaTmp != null)
                    {
                        lineaTmp = objReader.ReadLine();

                        if (lineaTmp != null)
                        {
                            string[] linea = lineaTmp.Split(';');
                            string codigo = linea[5].Trim();

                            // cargamos todos los hoteles
                            Hotel hotelLocalTmp = listaHotelLocal.Where(H => H.Codigo == codigo).FirstOrDefault();
                            if (hotelLocalTmp == null)
                            {
                                Hotel hotelTmp = new Hotel();
                                hotelTmp.Nombre = (Utilities.QuitarAcentuaciones(linea[0].Trim())).ToUpper();
                                hotelTmp.Nit = (Utilities.QuitarAcentuaciones(linea[1].Trim()));
                                hotelTmp.Codigo = linea[5].Trim();
                                hotelTmp.Correo = linea[3].Trim();
                                hotelTmp.CorreoReservas = linea[4].Trim();
                                hotelTmp.Secuencial = 0;
                                hotelTmp.UnidadNegocio = linea[6].Trim();

                                int idCiudad = this.IdCiudadDefault;
                                string nombreCiudad = Utilities.QuitarAcentuaciones(linea[7].Trim());

                                Ciudad ciudadTmpLocal = listaCiudadLocal.Where(C => C.Nombre.StartsWith(nombreCiudad)).FirstOrDefault();
                                if (ciudadTmpLocal != null)
                                    idCiudad = ciudadTmpLocal.IdCiudad;

                                hotelTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", idCiudad);

                                string direccion = Utilities.QuitarAcentuaciones(linea[2].Trim());
                                direccion = Utilities.SoloNumeroLetras(direccion);
                                hotelTmp.Direccion = direccion.ToUpper();

                                hotelBoTmp.Guardar(hotelTmp, this.IdUsuario);
                                listaHotelLocal.Add(hotelTmp);
                            }

                            // se va agregandolas suite a una lista temporal
                            Suit suitTmp = new Suit();
                            suitTmp.NumSuit = linea[8].Trim();
                            suitTmp.NumEscritura = linea[9].Trim() + "%" + codigo;
                            suitTmp.RegistroNotaria = linea[10].Trim();
                            suitTmp.Descripcion = linea[11].Trim();
                            suitTmp.Activo = true;

                            listaSuitTmp.Add(suitTmp);
                        }
                    }

                    objReader.Close();

                    foreach (Hotel itemHotel in listaHotelLocal)
                    {
                        List<Suit> listaSuit = listaSuitTmp.Where(S => S.NumEscritura.Split('%')[1] == itemHotel.Codigo).ToList();
                        List<ObjetoGenerico> listaSuiteLocal = suitBoTmp.ObtenerSuitsPorHotelCargaMasiva(itemHotel.IdHotel);

                        foreach (Suit itemSuite in listaSuit)
                        {
                            ObjetoGenerico suiteTmpLocal = listaSuiteLocal.
                                                           Where(S => S.NumEscritura == itemSuite.NumEscritura.Split('%')[0] && S.Codigo == itemHotel.Codigo).
                                                           FirstOrDefault();
                            if (suiteTmpLocal == null)
                            {
                                Suit suiteNueva = new Suit();
                                suiteNueva.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", itemHotel.IdHotel);
                                suiteNueva.NumSuit = itemSuite.NumSuit;
                                suiteNueva.Descripcion = itemSuite.Descripcion;
                                suiteNueva.NumEscritura = itemSuite.NumEscritura.Split('%')[0];
                                suiteNueva.RegistroNotaria = itemSuite.RegistroNotaria;
                                suiteNueva.Activo = true;

                                listaSuiteLocal.Add(new ObjetoGenerico() { Codigo = itemHotel.Codigo, NumEscritura = itemSuite.NumEscritura.Split('%')[0] });
                                suitBoTmp.Guardar(suiteNueva, this.IdUsuario);
                            }
                            else 
                            {
                                Suit oSuit = suitBoTmp.ObtenerSuitByEscritura(itemSuite.NumEscritura.Split('%')[0], itemHotel.Codigo);
                                suitBoTmp.Actualizar(oSuit.IdSuit, itemSuite.Descripcion, oSuit.Activo, itemSuite.NumSuit, oSuit.NumEscritura, itemSuite.RegistroNotaria, itemHotel.IdHotel, this.IdUsuario);
                            }
                        }
                    }
                }
                return listaError;
            }
            finally
            {
                objReader.Close();
            }
        }

        #endregion

        #region Propietario

        public List<string> Cargar_Propietario()
        {
            try
            {
                if (this.ValidarLineasPropietario())
                {
                    ciudadBoTmp = new CiudadBo();
                    propietarioBoTmp = new PropietarioBo();
                    suitPropietarioBoTmp = new SuitPropietarioBo();
                    suitBoTmp = new SuitBo();
                    bancoBoTmp = new BancoBo();

                    StreamReader objReader = new StreamReader(this.Ruta, Encoding.Default);
                    string lineaTmp = "";
                    this.Cont = 0;

                    // [ PRIMER NOMBRE, SEGUNDO NOMBRE, PRIMER APELLIDO, SEGUNDO APELLIDO, TIPO PERSONA, TIPO DOCUMENTO,
                    //   Numero Identificación ó Nit, Correo, Dirección, Ciudad, Teléfono1, Teléfono 2,
                    //   Nombre Contacto, Teléfono Contacto, Correo Contacto, Código Hotel,	Numero Suite,
                    //   Nombre Banco, Numero de Cuenta, Tipo de Cuenta, Nombre Titular, Numero de Estadías ]

                    List<Banco> listaBancoLocal = bancoBoTmp.VerTodos();
                    List<Propietario> listaPropietarioLocal = propietarioBoTmp.VerTodos2();
                    List<Ciudad> listaCiudadLocal = ciudadBoTmp.ObtenerTodos();

                    while (lineaTmp != null)
                    {
                        lineaTmp = objReader.ReadLine();

                        if (lineaTmp != null)
                        {
                            string[] linea = lineaTmp.Split(';');

                            Propietario propietarioTmp = new Propietario();
                            propietarioTmp.NombrePrimero = linea[0].Trim().ToUpper();
                            propietarioTmp.NombreSegundo = linea[1].Trim();
                            propietarioTmp.ApellidoPrimero = linea[2].Trim();
                            propietarioTmp.ApellidoSegundo = linea[3].Trim();
                            propietarioTmp.TipoPersona = linea[4].Trim().ToUpper();
                            propietarioTmp.TipoDocumento = linea[5].Trim().ToUpper();
                            propietarioTmp.NumIdentificacion = linea[6].Trim();
                            propietarioTmp.Correo = linea[7].Trim();
                            propietarioTmp.Telefono_1 = linea[10].Trim();
                            propietarioTmp.Telefono_2 = linea[11].Trim();
                            propietarioTmp.NombreContacto = linea[12].Trim();
                            propietarioTmp.TelefonoContacto = linea[13].Trim();
                            propietarioTmp.CorreoContacto = linea[14].Trim();
                            propietarioTmp.Login = linea[6].Trim();
                            propietarioTmp.Pass = Utilities.EncodePassword(string.Concat(linea[6].Trim(), linea[6].Trim()));
                            propietarioTmp.Cambio = true;
                            propietarioTmp.Activo = true;
                            propietarioTmp.FechaIngreso = DateTime.Now;
                            propietarioTmp.PerfilReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Perfil", "IdPerfil", this.IdPerfilPropietario);

                            if (propietarioTmp.NumIdentificacion == "1007475958")
                            {

                            }

                            int idCiudad = this.IdCiudadDefault;
                            string nombreCiudad = Utilities.QuitarAcentuaciones(linea[9].Trim());
                            Ciudad ciudadTmp = listaCiudadLocal.Where(C => C.Nombre.StartsWith(nombreCiudad)).FirstOrDefault();

                            if (ciudadTmp != null) // Ciudad por defeto si viene vacia o no existe na la base de datos.
                                idCiudad = ciudadTmp.IdCiudad;

                            propietarioTmp.CiudadReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Ciudad", "IdCiudad", idCiudad);

                            string direccion = Utilities.QuitarAcentuaciones(linea[8].Trim());
                            direccion = Utilities.SoloNumeroLetras(direccion);
                            propietarioTmp.Direccion = direccion.ToUpper();

                            Propietario propietario = listaPropietarioLocal.
                                                              Where(P => P.NumIdentificacion == propietarioTmp.NumIdentificacion).
                                                              FirstOrDefault();
                            if (propietario == null)
                            {
                                propietarioBoTmp.Guardar(propietarioTmp, this.IdUsuario);
                                listaPropietarioLocal.Add(propietarioTmp);
                            }

                            Banco banco = null;
                            if (linea[18].Trim() != string.Empty)
                            {
                                banco = listaBancoLocal.Where(B => B.Nombre.ToUpper().StartsWith(linea[18].Trim().ToUpper())).FirstOrDefault();
                                if (banco == null)
                                {
                                    Banco bancoTmp = bancoBoTmp.Guardar(linea[17].Trim().ToUpper());
                                    listaBancoLocal.Add(bancoTmp);
                                    banco = bancoTmp;
                                }
                            }

                            Suit suit = suitBoTmp.ObtenerSuitByEscritura(linea[16].Trim(), linea[15].Trim());

                            if (suit != null)
                            {
                                string tipoCuenta=string.Empty;
                                switch (linea[19].Trim().ToUpper())
                                    {
                                        case "AHORROS":
                                            tipoCuenta = "CH";
                                            break;

                                        case "CORRIENTE":
                                            tipoCuenta = "CC";
                                            break;

                                        case "CHEQUE":
                                            tipoCuenta = "HH";
                                            break;

                                        default:
                                            tipoCuenta = "-1";
                                            break;
                                    }
                                int numDias = (linea[21].Trim() != string.Empty)?int.Parse(linea[21].Trim()):this.NumEstadias;
                                int idBanco = (banco != null) ? banco.IdBanco : this.IdBancoDefault;
                                propietarioTmp = listaPropietarioLocal.Where(P => P.NumIdentificacion == propietarioTmp.NumIdentificacion).FirstOrDefault();


                                Suit_Propietario suitPropietarioTmp = suitPropietarioBoTmp.Obtener(propietarioTmp.IdPropietario, suit.IdSuit);
                                if (suitPropietarioTmp == null)
                                {
                                    suitPropietarioTmp = new Suit_Propietario();
                                    suitPropietarioTmp.NumEstadias = numDias;
                                    suitPropietarioTmp.TipoCuenta = tipoCuenta;
                                    suitPropietarioTmp.Titular = linea[20].Trim().ToUpper();
                                    suitPropietarioTmp.NumCuenta = linea[18].Trim();
                                    suitPropietarioTmp.EsActivo = true;
                                    suitPropietarioTmp.BancoReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Banco", "IdBanco", idBanco);
                                    suitPropietarioTmp.SuitReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit", "IdSuit", suit.IdSuit);
                                    suitPropietarioTmp.PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Propietario", "IdPropietario", propietarioTmp.IdPropietario);
                                    suitPropietarioBoTmp.Guardar(suitPropietarioTmp);
                                }
                                else {
                                    suitPropietarioBoTmp.Actualizar(suitPropietarioTmp.IdSuitPropietario, idBanco, linea[18].Trim(), numDias, linea[20].Trim().ToUpper(), tipoCuenta, "", (propietarioTmp.NombrePrimero + " " + propietarioTmp.ApellidoPrimero), IdUsuario);
                                }

                                
                            }
                        }
                    }


                    objReader.Close();
                }
                return listaError;
            }
            finally
            {
                objReader.Close();
            }
        }

        #endregion

        #region Variable Suit

        public List<string> Cargar_VariableSuit()
        {
            try
            {
                if (this.ValidarLineasVariablesSuite())
                {
                    PropietarioBo propietarioBoTmp = new PropietarioBo();
                    HotelBo hotelBoTmp = new HotelBo();
                    SuitBo suitBoTmp = new SuitBo();
                    VariableBo variableBoTmp = new VariableBo();
                    ValorVariableBo valorVariableBoTmp = new ValorVariableBo();
                    SuitPropietarioBo suitPropietarioBoTmp = new SuitPropietarioBo();

                    StreamReader objReader = new StreamReader(this.Ruta, Encoding.Default);
                    string lineaTmp = "";

                    // [ Nit ó Cedula Propietario, Código Hotel, Numero Escritura Suite, Nombre Variable, Descripción Variable, Valor Variable ]

                    List<Propietario> listaPropietarioLocal = propietarioBoTmp.VerTodos2();
                    List<Hotel> listaHotelLocal = hotelBoTmp.VerTodos();
                    List<Suit> listaSuitLocal = suitBoTmp.VerTodos();
                    List<Variable> listaVariableLocal = variableBoTmp.VerTodos2();

                    while (lineaTmp != null)
                    {
                        lineaTmp = objReader.ReadLine();

                        if (lineaTmp != null)
                        {
                            string[] linea = lineaTmp.Split(';');

                            if (linea.Length != 0)
                            {
                                Hotel hotelTmp = listaHotelLocal.Where(H => H.Codigo == linea[1].Trim()).FirstOrDefault();

                                Suit suitTmp = listaSuitLocal.
                                               Where(S => S.NumEscritura == linea[2].Trim() && S.Hotel.Codigo == linea[1].Trim()).
                                               FirstOrDefault();

                                Propietario propietario = listaPropietarioLocal.
                                                          Where(U => U.NumIdentificacion == linea[0]).
                                                          FirstOrDefault();

                                Suit_Propietario suitPropietarioTmp = suitPropietarioBoTmp.Obtener(propietario.IdPropietario, suitTmp.IdSuit);

                                string nombreVariable = Utilities.QuitarCaracteresEspeciales(Utilities.QuitarAcentuaciones(linea[3].Trim().ToUpper().Replace(' ', '_')));
                                Variable variable = listaVariableLocal.
                                                    Where(V => V.Nombre.ToUpper() == nombreVariable && V.Hotel.Codigo == linea[1].Trim()).
                                                    FirstOrDefault();

                                if (variable == null) // Si la variable no existe
                                {
                                    Variable variableTmp = new Variable();
                                    variableTmp.Nombre = nombreVariable.ToUpper();
                                    variableTmp.Activo = true;
                                    variableTmp.Descripcion = linea[4].Trim();
                                    variableTmp.Tipo = "P";
                                    variableTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", hotelTmp.IdHotel);

                                    Valor_Variable_Suit valorVariableSuitTmp = new Valor_Variable_Suit();

                                    if (linea[5].Contains(".")) // si tiene un punto la caja de texto, usa configuracion regional
                                    {
                                        valorVariableSuitTmp.Valor = Convert.ToDouble(linea[5], System.Globalization.CultureInfo.InvariantCulture);

                                    }
                                    else // aca quiere decir que puso una coma y lo reemplaza por un punto
                                    {
                                        linea[5].Replace(',', '.');
                                        valorVariableSuitTmp.Valor = Convert.ToDouble(linea[5]);
                                    }

                                    //valorVariableSuitTmp.Valor = Utilities.PasarPorcentaje(linea[5]);
                                    valorVariableSuitTmp.Suit_PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit_Propietario", "IdSuitPropietario", suitPropietarioTmp.IdSuitPropietario);

                                    variableTmp.Valor_Variable_Suit.Add(valorVariableSuitTmp);

                                    variableBoTmp.Guardar(variableTmp);

                                    variableTmp.Hotel = new Hotel() { IdHotel = hotelTmp.IdHotel, Codigo = hotelTmp.Codigo };
                                    listaVariableLocal.Add(variableTmp);
                                }
                                else // Si la variable no existe
                                {
                                    // Valido si esa variable ya existe, si existe modifico el valor sino lo creo nuevo
                                    if (!valorVariableBoTmp.ObtenerValorVariableSuite(suitPropietarioTmp.IdSuitPropietario, variable.IdVariable, linea[5]))
                                    {
                                        Valor_Variable_Suit valorVariableSuitTmp = new Valor_Variable_Suit();

                                        if (linea[5].Contains(".")) // si tiene un punto la caja de texto, usa configuracion regional
                                        {
                                            valorVariableSuitTmp.Valor = Convert.ToDouble(linea[5], System.Globalization.CultureInfo.InvariantCulture);

                                        }
                                        else // aca quiere decir que puso una coma y lo reemplaza por un punto
                                        {
                                            linea[5].Replace(',', '.');
                                            valorVariableSuitTmp.Valor = Convert.ToDouble(linea[5]);
                                        }

                                        //valorVariableSuitTmp.Valor = Utilities.PasarPorcentaje(linea[5]);
                                        valorVariableSuitTmp.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", variable.IdVariable);
                                        valorVariableSuitTmp.Suit_PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit_Propietario", "IdSuitPropietario", suitPropietarioTmp.IdSuitPropietario);

                                        valorVariableBoTmp.Guardar(valorVariableSuitTmp);
                                    }
                                }
                            }
                        }
                    }

                    objReader.Close();
                }

                return listaError;
            }
            finally
            {
                objReader.Close();
            }
        }

        #endregion

        #region Variable Hotel

        public List<string> Cargar_VariableHotel()
        {
            try
            {
                if (this.ValidarLineasVariablesHotel())
                {
                    //  [ Código Hotel, Nombre Variable, Descripción Variable, Valor ]
                    StreamReader objReader = new StreamReader(this.Ruta, Encoding.Default);
                    string lineaTmp = "";

                    ValorVariableBo valorVariableBoTmp = new ValorVariableBo();
                    VariableBo variableBoTmp = new VariableBo();
                    HotelBo hotelBoTmp = new HotelBo();

                    List<Variable> listaVariableLocal = variableBoTmp.VerTodos2();

                    while (lineaTmp != null)
                    {
                        lineaTmp = objReader.ReadLine();

                        if (lineaTmp != null)
                        {
                            string[] linea = lineaTmp.Split(';');

                            string nombre = Utilities.QuitarCaracteresEspeciales(linea[1].Trim().Replace(' ', '_'));
                            Variable variable = listaVariableLocal.
                                                Where(V => V.Nombre.ToUpper() == nombre &&
                                                      V.Hotel.Codigo == linea[0].Trim()).
                                                FirstOrDefault();

                            Valor_Variable valorVariableTmp = new Valor_Variable();
                            Hotel hotel = hotelBoTmp.Obtener(linea[0].Trim());

                            if (variable == null)
                            {
                                Variable variableTmp = new Variable();
                                variableTmp.Activo = true;
                                variableTmp.Tipo = "H";
                                variableTmp.Descripcion = linea[3].Trim();
                                variableTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", hotel.IdHotel);
                                variableTmp.Nombre = nombre;

                                if (linea[2].Contains(".")) // si tiene un punto la caja de texto, usa configuracion regional
                                {
                                    linea[2] = linea[2].Replace(".", "");
                                    valorVariableTmp.Valor = Convert.ToDouble(linea[2], System.Globalization.CultureInfo.InvariantCulture);

                                }
                                else // aca quiere decir que puso una coma y lo reemplaza por un punto
                                {
                                    linea[2].Replace(',', '.');
                                    valorVariableTmp.Valor = Convert.ToDouble(linea[2]);
                                }

                                //valorVariableTmp.Valor = Utilities.PasarPorcentaje(linea[2]);
                                valorVariableTmp.Fecha = DateTime.Parse(linea[4]);// DateTime.Now;// (linea.Length > 3) ? DateTime.Parse(linea[4].Trim()) : DateTime.Now;

                                variableTmp.Valor_Variable.Add(valorVariableTmp);
                                variableBoTmp.Guardar(variableTmp);
                                listaVariableLocal.Add(variableTmp);

                            }
                            else
                                variableBoTmp.ActualizarValorVariable(variable.IdVariable, double.Parse(linea[2]), DateTime.Parse(linea[4]), hotel.Nombre, variable.Nombre, IdUsuario);
                        }
                    }

                    objReader.Close();
                    //File.Delete(this.Ruta);
                }

                return listaError;
            }
            finally
            {
                objReader.Close();
            }
        }

        #endregion

        #region Informacion Certificado

        public List<string> Cargar_InformacionCertificado()
        {
            try
            {
                if (this.ValidarLineasInformacionCertificado())
                {
                    //  [ Nit ó Cedula Propietario, Código Hotel, Año, Nombre Concepto, Valor ]
                    StreamReader objReader = new StreamReader(this.Ruta, Encoding.Default);
                    string lineaTmp = "";

                    HotelBo hotelBoTmp = new HotelBo();
                    suitBoTmp = new SuitBo();

                    lineaTmp = objReader.ReadLine();
                    string codHotel = lineaTmp.Split(';')[1];
                    Hotel hotelTmp = hotelBoTmp.Obtener(codHotel);

                    List<ObjetoGenerico> listaPropietario = propietarioBoTmp.VerTodosPorHotelPropietarioConDistinc(hotelTmp.IdHotel);
                    List<Suit> listaSuite = suitBoTmp.ObtenerSuitsPorHotel(hotelTmp.IdHotel);
                    List<ObjetoGenerico> listaInfoCertificado = new List<ObjetoGenerico>();
                    ObjetoGenerico infoCertificadoTmp = null;

                    while (lineaTmp != null)
                    {
                        if (lineaTmp != null)
                        {
                            string[] linea = lineaTmp.Split(';');
                            string cc_nit = linea[0].Replace(".", "");

                            infoCertificadoTmp = new ObjetoGenerico();
                            infoCertificadoTmp.Anio = short.Parse(linea[2].Replace(".", ""));
                            infoCertificadoTmp.Valor = double.Parse(linea[4].Replace(".", ""));
                            infoCertificadoTmp.NombreConcepto = linea[3];
                            infoCertificadoTmp.IdPropietario = listaPropietario.Where(P => P.NumIdentificacion == cc_nit).Select(P => P.IdPropietario).FirstOrDefault();
                            infoCertificadoTmp.IdHotel = hotelTmp.IdHotel;

                            listaInfoCertificado.Add(infoCertificadoTmp);
                        }
                        lineaTmp = objReader.ReadLine();
                    }

                    ConfiguracionCertificadoBo configCertificadoBoTmp = new ConfiguracionCertificadoBo();
                    if (configCertificadoBoTmp.Guardar(listaInfoCertificado) == false)
                        listaError.Add("Error de guardado");

                    objReader.Close();
                    //File.Delete(this.Ruta);
                }

                return listaError;
            }
            catch (Exception ex) {
                objReader.Close();
                return listaError;
            }
            finally
            {
                objReader.Close();
            }
        }        

        #endregion

        #region Validaciones
        public bool ValidarLineasHotel()
        {
            bool esValido = true;

            objReader = new StreamReader(this.Ruta, Encoding.Default);
            string lineaTmp = "";
            this.Cont = 1;

            while (lineaTmp != null)
            {
                lineaTmp = objReader.ReadLine();

                if (lineaTmp != null)
                {
                    string[] linea = lineaTmp.Split(';');

                    if (this.Cont == 1) // Validando que el archivo no venga con cabeceras.
                    {
                        if (linea.Length == 0)
                        {
                            listaError.Add("Fila " + this.Cont + ": el archivo tiene cabecera ó los delimitadores no estan con punto y coma (;).");
                            return false;
                        }
                    }

                    if (linea.Length != 12)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": No tiene el numero de campos requeridos.");
                        this.Cont++;
                        continue;
                    }

                    if (!Utilities.EsCorreoValido(linea[3].Trim()))
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": El correo no tiene el formato correcto.");
                    }

                    if (!Utilities.EsCorreoValido(linea[4].Trim()))
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": El correo no tiene el formato correcto.");
                    }

                    if (linea[8].Trim().Length > 10) // Num Suite
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Numero suite no debe ser mayor a 10 caracteres.");
                    }

                    if (linea[9].Trim().Length > 10) // Num Escritura
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Numero suite no debe ser mayor a 10 caracteres.");
                    }

                    if (linea[10].Trim().Length > 10) // Registro Notaria
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Numero suite no debe ser mayor a 10 caracteres.");
                    }

                    if (linea[8].Trim() == string.Empty) // Num Suite
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Numero Suite es campo obligatorio.");
                    }

                    if (linea[9].Trim() == string.Empty) // Num Escritura
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Numero Suite Escritura es campo obligatorio.");
                    }

                    this.Cont++;
                }
            }

            return esValido;        
        }

        public bool ValidarLineasPropietario()
        {
            bool esValido = true;
            suitBoTmp = new SuitBo();
            hotelBoTmp = new HotelBo();

            objReader = new StreamReader(this.Ruta, Encoding.Default);
            string lineaTmp = "";
            this.Cont = 1;

            while (lineaTmp != null)
            {
                lineaTmp = objReader.ReadLine();

                if (lineaTmp != null)
                {
                    string[] linea = lineaTmp.Split(';');

                    if (this.Cont == 1) // Validando que el archivo no venga con cabeceras.
                    {
                        if (linea.Length == 0)
                        {
                            listaError.Add("Fila " + this.Cont + ": el archivo tiene cabecera ó los delimitadores no estan con punto y coma (;).");
                            return false;
                        }
                    }

                    if (linea.Length != 22) // 22/09/2018[23] // 1/14/2016 [22]
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": No tiene el numero de campos requeridos.");
                        this.Cont++;
                        continue;
                    }

                    int num = -1;
                    if (!(int.TryParse(linea[(linea.Length - 1)].ToString(), out num)))
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": El número de estadias debe ser un número.");
                    }

                    if (linea[0].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Primer Nombre debe ser obligatorio.");
                    }
                    // depronto si es con ó
                    if (linea[4].Trim().ToUpper() != "JURIDICO" && linea[4].Trim().ToUpper() != "NATURAL")
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": El valor debe ser JURIDICO ó NATURAL.");
                    }

                    //if (linea[5].Trim().Length != 3)
                    //{
                    //    esValido = false;
                    //    listaError.Add("Fila " + this.Cont + ": Tipo Documento debe ser de 3 caracteres.");
                    //}

                    if (linea[5].Trim().ToUpper() != "NIT" && linea[5].Trim().ToUpper() != "CC" &&
                        linea[5].Trim().ToUpper() != "CE" && linea[5].Trim().ToUpper() != "TI")
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Tipo Documento debe ser NIT,CC,CE ó TI.");
                    }                    

                    if (linea[6].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Numero de identificacion ó Nit, debe ser obligatorio.");
                    }

                    if (linea[7].Trim() != string.Empty)
                    {
                        if (!Utilities.EsCorreoValido(linea[7].Trim()))
                        {
                            esValido = false;
                            listaError.Add("Fila " + this.Cont + ": El correo del propietario, no tiene el formato correcto.");
                        }
                    }

                    //if (linea[15].Trim() != string.Empty)
                    //{
                    //    if (!Utilities.EsCorreoValido(linea[15].Trim()))
                    //    {
                    //        esValido = false;
                    //        listaError.Add("Fila " + this.Cont + ": El correo del contacto, no tiene el formato correcto.");
                    //    }
                    //}

                    if (linea[15].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Codigo hotel es un campo obligatorio.");
                    }

                    if (linea[16].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Numero Suite Escritura es un campo obligatorio.");
                    }

                    Hotel hotelTmp = hotelBoTmp.Obtener(linea[15].Trim());
                    if (hotelTmp == null)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": El código del Hotel no existe.");
                    }

                    Suit suitTmp = suitBoTmp.ObtenerSuitByEscritura(linea[16].Trim(), linea[15].Trim());
                    if (suitTmp == null)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": La suite no existe.");
                    }

                    //if (linea[19].Trim() == string.Empty)
                    //{
                    //    esValido = false;
                    //    listaError.Add("Fila " + this.Cont + ": Numero de Cuenta es un campo obligatorio.");
                    //}

                    //if (linea[20].Trim() == string.Empty)
                    //{
                    //    esValido = false;
                    //    listaError.Add("Fila " + this.Cont + ": Tipo de Cuenta es un campo obligatorio.");
                    //}

                    //if (linea[20].Trim().ToUpper() != "CORRIENTE" && linea[20].Trim().ToUpper() != "AHORROS")
                    //{
                    //    esValido = false;
                    //    listaError.Add("Fila " + this.Cont + ": Tipo de Cuenta debe ser CORRIENTE ó AHORROS.");
                    //}

                    //if (linea[22].Trim() == string.Empty)
                    //{
                    //    esValido = false;
                    //    listaError.Add("Fila " + this.Cont + ": Nombre Titular es un campo obligatorio.");
                    //}
                }

                this.Cont++;
            }

            return esValido;
        }

        public bool ValidarLineasVariablesSuite()
        {
            bool esValido = true;
            suitBoTmp = new SuitBo();
            hotelBoTmp = new HotelBo();
            propietarioBoTmp = new PropietarioBo();
            suitPropietarioBoTmp = new SuitPropietarioBo();

            objReader = new StreamReader(this.Ruta, Encoding.Default);
            string lineaTmp = "";
            this.Cont = 1;

            List<ObjetoGenerico> listaPropietarios = propietarioBoTmp.VerTodos();
            List<Hotel> listaHotel = hotelBoTmp.VerTodos();

            while (lineaTmp != null)
            {
                lineaTmp = objReader.ReadLine();

                if (lineaTmp != null)
                {
                    string[] linea = lineaTmp.Split(';');

                    if (this.Cont == 1) // Validando que el archivo no venga con cabeceras.
                    {
                        if (linea.Length == 0)
                        {
                            listaError.Add("Fila " + this.Cont + ": el archivo tiene cabecera ó los delimitadores no estan con punto y coma (;).");
                            return false;
                        }
                    }

                    if (linea.Length != 6)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": No tiene el numero de campos requeridos.");
                        this.Cont++;
                        continue;
                    }

                    if (linea[0].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Nit ó Numero de identificación, debe ser obligatorio.");
                    }

                    if (listaPropietarios.Where(P => P.NumIdentificacion == Utilities.QuitarCaracteresEspeciales(linea[0])).Count() == 0)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Nit ó Numero de identificación, no existe en la base de datos.");
                    }

                    if (linea[1].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Codigo Hotel, debe ser obligatorio.");
                    }

                    if (listaHotel.Where(H => H.Codigo == linea[1].Trim()).Count() == 0)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Código hotel, no existe en la base de datos.");
                    }

                    if (linea[2].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Número suite escritura, debe ser obligatorio.");
                    }

                    Suit suitTmp = suitBoTmp.ObtenerSuitByEscritura(linea[2].Trim(), linea[1].Trim());
                    if (suitTmp == null)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": La suite no existe.");
                    }

                    if (linea[3].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Nombre variable, debe ser obligatorio.");
                    }

                    if (linea[5].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Valor variable, debe ser obligatorio.");
                    }

                    if (linea[5].Trim() != string.Empty)
                    {
                        double num = 0;
                        if (!double.TryParse(linea[5].Trim(), out num))
                        {
                            esValido = false;
                            listaError.Add("Fila " + this.Cont + ": El Valor no es valido.");
                        }
                    }

                    if (linea[0].Trim() != string.Empty && linea[2].Trim() != string.Empty && linea[1].Trim() != string.Empty)
                    {
                        Suit_Propietario suitPropietarioTmp = suitPropietarioBoTmp.Obtener(linea[0], linea[2], linea[1]);
                        if (suitPropietarioTmp == null)
                        {
                            esValido = false;
                            listaError.Add("Fila " + this.Cont + ": La suite no pertenece a ese propietario.");
                        }
                    }
                }

                this.Cont++;
            }

            return esValido;
        }

        public bool ValidarLineasVariablesHotel()
        {
            bool esValido = true;
            hotelBoTmp = new HotelBo();

            objReader = new StreamReader(this.Ruta, Encoding.Default);
            string lineaTmp = "";
            this.Cont = 1;

            List<Hotel> listaHotel = hotelBoTmp.VerTodos();

            while (lineaTmp != null)
            {
                lineaTmp = objReader.ReadLine();

                if (lineaTmp != null)
                {
                    string[] linea = lineaTmp.Split(';');

                    if (this.Cont == 1) // Validando que el archivo no venga con cabeceras.
                    {
                        if (linea.Length == 0)
                        {
                            listaError.Add("Fila " + this.Cont + ": el archivo tiene cabecera ó los delimitadores no estan con punto y coma (;).");
                            return false;
                        }
                    }

                    if (linea.Length != 5)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": No tiene el numero de campos requeridos.");
                        this.Cont++;
                        continue;
                    }

                    if (linea[0].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Codigo Hotel, debe ser obligatorio.");
                    }

                    if (listaHotel.Where(H => H.Codigo == linea[0].Trim()).Count() == 0)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Código hotel, no existe en la base de datos.");
                    }

                    if (linea[1].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Nombre Variable, debe ser obligatorio.");
                    }

                    if (linea[2].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Valor, debe ser obligatorio.");
                    }

                    if (linea[2].Trim() != string.Empty)
                    {
                        double num = 0;
                        if (!double.TryParse(linea[2].Trim(), out num))
                        {
                            esValido = false;
                            listaError.Add("Fila " + this.Cont + ": El Valor no es valido.");
                        }
                    }

                    if (linea[4].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Fecha, debe ser obligatorio.");
                    }

                    if (linea[4].Trim() != string.Empty)
                    {
                        DateTime fecha;
                        if (!DateTime.TryParse(linea[4].Trim(), out fecha))
                        {
                            esValido = false;
                            listaError.Add("Fila " + this.Cont + ": La Fecha no es valida.");
                        }
                    }
                }
                this.Cont++;
            }
            return esValido;
        }

        private bool ValidarLineasInformacionCertificado()
        {
            bool esValido = true;
            hotelBoTmp = new HotelBo();

            objReader = new StreamReader(this.Ruta, Encoding.Default);
            string lineaTmp = "";
            this.Cont = 1;

            lineaTmp = objReader.ReadLine();
            string codHotel = lineaTmp.Split(';')[1];
            Hotel hotelTmp = hotelBoTmp.Obtener(codHotel);
            if(hotelTmp == null)
            {
                listaError.Add("Fila " + this.Cont + ": Código hotel, no existe en la base de datos..");
                return false;
            }

            propietarioBoTmp = new PropietarioBo();
            List<ObjetoGenerico> listaPropietario = propietarioBoTmp.VerTodosPorHotelPropietarioConDistinc(hotelTmp.IdHotel); 

            while (lineaTmp != null)
            {
                if (lineaTmp != null)
                {
                    string[] linea = lineaTmp.Split(';');

                    if (this.Cont == 1) // Validando que el archivo no venga con cabeceras.
                    {
                        if (linea.Length == 0)
                        {
                            listaError.Add("Fila " + this.Cont + ": el archivo tiene cabecera ó los delimitadores no estan con punto y coma (;).");
                            return false;
                        }
                    }

                    if (linea.Length != 5)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": No tiene el numero de campos requeridos.");
                        this.Cont++;
                        continue;
                    }

                    if (linea[0].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Nit ó Cedula Propietario, debe ser obligatorio.");
                    }

                    if (linea[1].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Codigo Hotel, debe ser obligatorio.");
                    }

                    if (linea[2].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Año, debe ser obligatorio.");
                    }

                    if (linea[3].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Nombre Concepto, debe ser obligatorio.");
                    }

                    if (linea[4].Trim() == string.Empty)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Valor, debe ser obligatorio.");
                    }                    

                    if (listaPropietario.Where(P => P.NumIdentificacion == linea[0].Replace(".","").Trim()).Count() == 0)
                    {
                        esValido = false;
                        listaError.Add("Fila " + this.Cont + ": Propietario no existente.");
                    }

                    if (linea[4].Trim() != string.Empty)
                    {
                        float num = 0;
                        if (!float.TryParse(linea[2].Trim(), out num))
                        {
                            esValido = false;
                            listaError.Add("Fila " + this.Cont + ": El Valor no es valido.");
                        }
                    }

                    lineaTmp = objReader.ReadLine();
                    this.Cont++;
                }                
            }
            return esValido;
        }
        #endregion
    }
}
