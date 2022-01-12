using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;
using System.Globalization;

namespace BO
{
    public class ValorVariableBo
    {
        public void GuardarSuit(int idSuitPropietario, int idVariable, double valor)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Valor_Variable_Suit valorVariableSuitTmp = new Valor_Variable_Suit();
                valorVariableSuitTmp.Suit_PropietarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Suit_Propietario", "IdSuitPropietario", idSuitPropietario);
                valorVariableSuitTmp.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", idVariable);
                valorVariableSuitTmp.Valor = valor;

                Contexto.AddToValor_Variable_Suit(valorVariableSuitTmp);
                Contexto.SaveChanges();
            }
        }

        public List<ObjetoGenerico> ObtenerValoresVariables(int idSuitPropietario, bool esActivo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                listaVariables = (from V in Contexto.Variable
                                  join VV in Contexto.Valor_Variable_Suit on V.IdVariable equals VV.Variable.IdVariable
                                  where VV.Suit_Propietario.IdSuitPropietario == idSuitPropietario && V.Activo == esActivo
                                  select new ObjetoGenerico()
                                  {
                                      IdValorVariableSuit = VV.IdValorVariableSuit,
                                      IdSuitPropietario = VV.Suit_Propietario.IdSuitPropietario,
                                      IdVariableSuite = V.IdVariable,
                                      Nombre = V.Nombre,
                                      Valor = VV.Valor,
                                      EsValidacion = V.EsConValidacion,
                                      ValMax = V.MaxNumero
                                  }).ToList();

                return listaVariables;
            }
        }

        public void Actualizar(List<ObjetoGenerico> listaSuitVariables, string propietario, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                DateTime fecha = DateTime.Now;
                foreach (ObjetoGenerico suitVariableTmp in listaSuitVariables)
                {
                    Valor_Variable_Suit valorVariableSuitTmp = Contexto.Valor_Variable_Suit.Include("Variable").Include("Suit_Propietario").Where(SP_VS => SP_VS.IdValorVariableSuit == suitVariableTmp.IdValorVariableSuit).FirstOrDefault();

                    #region auditoria
                    string nomVariable = Contexto.Variable.Where(V => V.IdVariable == valorVariableSuitTmp.Variable.IdVariable).Select(V => V.Nombre).FirstOrDefault();
                    var nombreHotelSuite = Contexto.Suit_Propietario.Where(SP => SP.IdSuitPropietario == valorVariableSuitTmp.Suit_Propietario.IdSuitPropietario).Select(SP => new { SP.Suit.NumSuit, SP.Suit.Hotel.Nombre }).FirstOrDefault();

                    Auditoria auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = "Propietario: " + propietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : " + nomVariable + " = " + suitVariableTmp.Valor.ToString("N");
                    auditoriaTmp.ValorAnterior = "Propietario: " + propietario + " Hotel : " + nombreHotelSuite.Nombre + " Num. Suite : " + nombreHotelSuite.NumSuit + " : " + nomVariable + " = " + valorVariableSuitTmp.Valor.ToString("N");
                    auditoriaTmp.NombreTabla = "Valor Variable Suite";
                    auditoriaTmp.Accion = "Actualizar";
                    auditoriaTmp.Campo = "Valor";
                    auditoriaTmp.Fechahora = fecha;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);
                    #endregion

                    valorVariableSuitTmp.Valor = suitVariableTmp.Valor;
                }
                Contexto.SaveChanges();
            }
        }

        public void Guardar(List<Valor_Variable_Suit> listaValorVariables)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                foreach (Valor_Variable_Suit valorVariableTmp in listaValorVariables)
                {
                    Contexto.AddToValor_Variable_Suit(valorVariableTmp);
                }
                Contexto.SaveChanges();
            }
        }

        public int Guardar(Valor_Variable_Suit valorVariableSuit)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Contexto.AddToValor_Variable_Suit(valorVariableSuit);
                Contexto.SaveChanges();

                return valorVariableSuit.IdValorVariableSuit;
            }
        }

        public int Guardar(Valor_Variable valorVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Contexto.AddToValor_Variable(valorVariable);
                Contexto.SaveChanges();

                return valorVariable.IdValorVariable;
            }
        }

        public double Obtener(int idVariable, DateTime fechaDesde, DateTime fechaHasta)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                double valor = 0;

                try
                {
                    valor = Contexto.
                        Valor_Variable.
                        Where(VV => VV.Variable.IdVariable == idVariable &&
                              VV.Fecha.Month >= fechaDesde.Month &&
                              VV.Fecha.Year >= fechaDesde.Year &&
                              VV.Fecha.Month <= fechaHasta.Month &&
                              VV.Fecha.Year <= fechaHasta.Year).
                              Sum(VV => VV.Valor);
                }
                catch (Exception)
                {
                }                

                return valor;
            }
        }

        public List<ObjetoGenerico> ValidarValorVariable(int idVariable, int idSuite, double valMaxVariable, ref double valMax)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> lista = ((from VVS in Contexto.Valor_Variable_Suit
                                               join SP in Contexto.Suit_Propietario on VVS.Suit_Propietario.IdSuitPropietario equals SP.IdSuitPropietario
                                               join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                               where SP.Suit.IdSuit == idSuite && VVS.Variable.IdVariable == idVariable
                                               select new ObjetoGenerico()
                                               {
                                                   Nombre = P.NombrePrimero + " " + P.ApellidoPrimero,
                                                   Valor = VVS.Valor
                                               }).ToList());

                valMax = (Math.Abs(valMaxVariable - (lista.Sum(S => S.Valor) * 100)) / 100);

                return lista;
            }
        }

        public List<ObjetoGenerico> ObtenerVariableValorPorHotel(int idHotel, string tipo, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                listaVariables = (from V in Contexto.Variable
                                  join VV in Contexto.Valor_Variable on V.IdVariable equals VV.Variable.IdVariable
                                  where V.Hotel.IdHotel == idHotel &&
                                        VV.Fecha.Month == fecha.Month &&
                                        VV.Fecha.Year == fecha.Year &&
                                        V.Activo == true
                                  select new ObjetoGenerico()
                                  {
                                      NombreVariable = V.Nombre,
                                      Valor = VV.Valor
                                  }).ToList();
                return listaVariables;
            }
        }

        public List<ObjetoGenerico> ObtenerVariableValorPorPropietario(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                listaVariables = (from SP in Contexto.Suit_Propietario
                                  join VVS in Contexto.Valor_Variable_Suit on SP.IdSuitPropietario equals VVS.Suit_Propietario.IdSuitPropietario
                                  join V in Contexto.Variable on VVS.Variable.IdVariable equals V.IdVariable
                                  join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                  join UP in Contexto.Propietario on SP.Propietario.IdPropietario equals UP.IdPropietario
                                  where S.Hotel.IdHotel == idHotel && V.Tipo == "P"
                                  select new ObjetoGenerico()
                                  {
                                      PrimeroNombre = UP.NombrePrimero,
                                      SegundoNombre = UP.NombreSegundo,
                                      PrimerApellido = UP.ApellidoPrimero,
                                      SegundoApellido = UP.ApellidoSegundo,
                                      TipoPersona = UP.TipoPersona,
                                      NumSuit = S.NumSuit,
                                      NumEscritura = S.NumEscritura,
                                      NombreVariable = V.Nombre,
                                      Valor = VVS.Valor
                                  }).OrderBy(X => new { X.NombreVariable, X.NumSuit }).ToList();


                return listaVariables;
            }
        }

        public bool ObtenerValorVariableSuite(int idSuitPropietario, int idVariable, string valorTmp)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Valor_Variable_Suit ValorVariableSuitTmp = Contexto.Valor_Variable_Suit.Where(VVS => VVS.Suit_Propietario.IdSuitPropietario == idSuitPropietario && VVS.Variable.IdVariable == idVariable).FirstOrDefault();
                if (ValorVariableSuitTmp != null)
                {
                    string culture = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                    valorTmp = valorTmp.Replace(".", culture);
                    ValorVariableSuitTmp.Valor = Convert.ToDouble(valorTmp);
                    Contexto.SaveChanges();

                    return true;
                }
                else
                    return false;
            }
        }

        public double ObtenerValorTipoPropietario(int idPropietario, int idsuite, int idVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                double valor = (from VVS in Contexto.Valor_Variable_Suit
                                join SP in Contexto.Suit_Propietario on VVS.Suit_Propietario.IdSuitPropietario equals SP.IdSuitPropietario
                                where SP.Propietario.IdPropietario == idPropietario &&
                                      SP.Suit.IdSuit == idsuite &&
                                      VVS.Variable.IdVariable == idVariable
                                select VVS.Valor).FirstOrDefault();
                return valor;
            }
        }

        public double ObtenerValorTipoHotel(int idVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                double valor = (from VV in Contexto.Valor_Variable
                                where VV.Variable.IdVariable == idVariable
                                select VV.Valor).FirstOrDefault();
                return valor;
            }
        }

        public List<ObjetoGenerico> GetInforVariablesValidacion(int idVariable, int idSuite)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> items = (from VVS in Contexto.Valor_Variable_Suit
                                              join SP in Contexto.Suit_Propietario on VVS.Suit_Propietario.IdSuitPropietario equals SP.IdSuitPropietario
                                              join P in Contexto.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                              where VVS.Variable.IdVariable == idVariable && SP.Suit.IdSuit == idSuite
                                              select new ObjetoGenerico()
                                              {
                                                  Nombre = P.NombrePrimero,
                                                  Apellido = P.ApellidoPrimero,
                                                  NumIdentificacion = P.NumIdentificacion,
                                                  Valor = VVS.Valor,
                                                  IdSuitPropietario = SP.IdSuitPropietario
                                              }).ToList();
                return items;
            }
        }
    }
}
