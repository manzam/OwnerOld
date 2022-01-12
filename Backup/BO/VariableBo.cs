using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;

namespace BO
{
    public class VariableBo
    {
        public List<ObjetoGenerico> VerTodos()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                listaVariables = (from V in Contexto.Variable
                                  join H in Contexto.Hotel on V.Hotel.IdHotel equals H.IdHotel
                                  select new ObjetoGenerico()
                                  {
                                      IdVariable = V.IdVariable,
                                      IdHotel = H.IdHotel,
                                      Nombre = V.Nombre,
                                      Descripcion = V.Descripcion,
                                      NombreHotel = H.Nombre,
                                      Activo = V.Activo,
                                      Tipo = V.Tipo
                                  }).OrderBy(V => V.Nombre).ToList();
                return listaVariables;
            }
        }

        public List<Variable> VerTodos2()
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Variable> listaVariables = new List<Variable>();
                listaVariables = Contexto.Variable.Include("Hotel").ToList();
                return listaVariables;
            }
        }

        public List<ObjetoGenerico> VerTodos(string tipo, int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                listaVariables = (from V in Contexto.Variable
                                  join H in Contexto.Hotel on V.Hotel.IdHotel equals H.IdHotel
                                  where V.Tipo == tipo && H.IdHotel == idHotel
                                  select new ObjetoGenerico()
                                  {
                                      IdVariable = V.IdVariable,
                                      IdHotel = H.IdHotel,
                                      Nombre = V.Nombre,
                                      Descripcion = V.Descripcion,
                                      NombreHotel = H.Nombre,
                                      Activo = V.Activo,
                                      Tipo = V.Tipo
                                  }).OrderBy(V => V.Nombre).ToList();
                return listaVariables;
            }
        }

        public List<ObjetoGenerico> VerTodos(int idHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                listaVariables = (from V in Contexto.Variable
                                  join H in Contexto.Hotel on V.Hotel.IdHotel equals H.IdHotel
                                  where H.IdHotel == idHotel
                                  select new ObjetoGenerico()
                                  {
                                      IdVariable = V.IdVariable,
                                      IdHotel = H.IdHotel,
                                      Nombre = V.Nombre,
                                      Descripcion = V.Descripcion,
                                      NombreHotel = H.Nombre,
                                      Activo = V.Activo,
                                      Tipo = V.Tipo,
                                      ValMax = V.MaxNumero,
                                      EsValidacion = V.EsConValidacion,
                                      Valor = V.ValorConstante
                                  }).ToList();

                return listaVariables;
            }
        }

        public List<ObjetoGenerico> VerTodos(bool esActivo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                listaVariables = (from V in Contexto.Variable
                                  join VV in Contexto.Valor_Variable on V.IdVariable equals VV. Variable.IdVariable
                                  join H in Contexto.Hotel on V.Hotel.IdHotel equals H.IdHotel
                                  where V.Activo == esActivo
                                  select new ObjetoGenerico()
                                  {
                                      IdVariable = V.IdVariable,
                                      IdHotel = H.IdHotel,
                                      IdValorVariable = VV.IdValorVariable,
                                      Valor = VV.Valor,
                                      NombreHotel = H.Nombre,
                                      Activo = V.Activo
                                  }).ToList();
                return listaVariables;
            }
        }

        public List<Variable> VerTodos(int idHotel, bool esActivo, string tipo)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Variable> listaVariables = new List<Variable>();
                listaVariables = Contexto.
                                 Variable.Include("Hotel").
                                 Where(V => V.Hotel.IdHotel == idHotel && V.Activo == esActivo && V.Tipo == tipo).OrderBy(V => V.Nombre).
                                 ToList();
                return listaVariables;
            }
        }

        public List<ObjetoGenerico> VerTodosConValor(int idHotel, bool esActivo, string tipo, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Variable> listaVariables = this.VerTodos(idHotel, esActivo, tipo);
                List<ObjetoGenerico> listaValorVariable = new List<ObjetoGenerico>();
                ObjetoGenerico oValorVariable = null;

                foreach (Variable itemVariable in listaVariables)
                {
                    oValorVariable = new ObjetoGenerico();
                    oValorVariable.IdVariable = itemVariable.IdVariable;
                    oValorVariable.Nombre = itemVariable.Nombre;

                    oValorVariable.Valor = Contexto.Valor_Variable.
                                           Where(VV => VV.Variable.IdVariable == itemVariable.IdVariable &&
                                                 VV.Fecha.Month == fecha.Month &&
                                                 VV.Fecha.Year == fecha.Year).
                                           Select(VV => VV.Valor).
                                           FirstOrDefault();

                    listaValorVariable.Add(oValorVariable);
                }

                return listaValorVariable;
            }
        }

        public List<ObjetoGenerico> ListaVariablesSuit(int idSuit)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariables = new List<ObjetoGenerico>();
                listaVariables = (from VVS in Contexto.Valor_Variable_Suit
                                  join SP in Contexto.Suit_Propietario on VVS.Suit_Propietario.IdSuitPropietario equals SP.IdSuitPropietario
                                  join V in Contexto.Variable on VVS.Variable.IdVariable equals V.IdVariable
                                  where SP.Suit.IdSuit == idSuit
                                  select new ObjetoGenerico()
                                  {
                                      NombreVariable = V.Nombre,
                                      Valor = VVS.Valor
                                  }).ToList();
                return listaVariables;
            }
        }

        public bool EsOcupado(string nombreVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int count = Contexto.Variable.Where(V => V.Nombre == nombreVariable).Count();
                return (count > 0) ? true : false;
            }
        }

        public bool EsNombreVariableValido(string nombreVariable, string codigoHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int count = Contexto.Variable.Where(V => V.Nombre == nombreVariable && V.Hotel.Codigo == codigoHotel).Count();
                return (count == 0) ? false : true;
            }
        }

        public void GuardarPlano(List<ObjetoGenerico> listaVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<Hotel> listaHotel = Contexto.Hotel.ToList();

                foreach (ObjetoGenerico itemInfo in listaVariable)
                {
                    Variable variableTmp = Contexto.Variable.Include("Valor_Variable").Where(I => I.Hotel.Codigo == itemInfo.Codigo && I.Nombre == itemInfo.NombreVariable).FirstOrDefault();

                    if (variableTmp != null)
                    {
                        Valor_Variable valorVariableTmp = variableTmp.Valor_Variable.Where(H => H.Fecha.Year == itemInfo.Fecha.Year && H.Fecha.Month == itemInfo.Fecha.Month).FirstOrDefault();
                        
                        if (valorVariableTmp != null)
                        {
                            valorVariableTmp.Fecha = itemInfo.Fecha;
                            valorVariableTmp.Valor = itemInfo.Valor;
                        }
                        else
                        {
                            Valor_Variable valorVariableNuevo = new Valor_Variable();
                            valorVariableNuevo.Fecha = itemInfo.Fecha;
                            valorVariableNuevo.Valor = itemInfo.Valor;
                            variableTmp.Valor_Variable.Add(valorVariableNuevo);
                        }
                    }
                    else
                    {
                        Variable variableNuevoTmp = new Variable();
                        variableNuevoTmp.Nombre = itemInfo.NombreVariable;
                        variableNuevoTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", listaHotel.Where(H => H.Codigo == itemInfo.Codigo).Select(H => H.IdHotel).FirstOrDefault());

                        Valor_Variable valorVariableNuevo = new Valor_Variable();
                        valorVariableNuevo.Fecha = itemInfo.Fecha;
                        valorVariableNuevo.Valor = itemInfo.Valor;
                        variableNuevoTmp.Valor_Variable.Add(valorVariableNuevo);

                        Contexto.AddToVariable(variableNuevoTmp);
                    }
                }
                Contexto.SaveChanges();
            }
        }

        public void Actualizar(int idVariable, string nombreNew, string descripcion, bool esActivo, string tipo, int idHotel, string nombreHotel, string tipoVariable, bool esConValidacion, short valMaximo, int idUsuario, double valorConstante)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Variable variableTmp = Contexto.Variable.Where(V => V.IdVariable == idVariable).FirstOrDefault();

                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = valMaximo + " Hotel : " + nombreHotel;
                auditoriaTmp.ValorAnterior = variableTmp.MaxNumero + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Valor Maximo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = ((esConValidacion) ? "Si" : "No") + " Hotel : " + nombreHotel;
                auditoriaTmp.ValorAnterior = ((variableTmp.EsConValidacion) ? "Si" : "No") + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Es Con Validacion";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreNew + " Hotel : " + nombreHotel;
                auditoriaTmp.ValorAnterior = variableTmp.Nombre + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = descripcion + "Hotel : " + nombreHotel;
                auditoriaTmp.ValorAnterior = variableTmp.Descripcion + "Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Descripcion";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = ((esActivo) ? "Si" : "No") + " Hotel : " + nombreHotel;
                auditoriaTmp.ValorAnterior = ((variableTmp.Activo) ? "Activo" : "Inactivo") + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Estado";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = tipoVariable + " Hotel : " + nombreHotel;
                auditoriaTmp.ValorAnterior = ((variableTmp.Tipo == "P") ? "Variable Propietario" : "Variable Hotel") + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Actualizar";
                auditoriaTmp.Campo = "Tipo Variable";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                if (variableTmp.Tipo != tipo)
                {
                    if (variableTmp.Tipo == "P")
                    {
                        List<Valor_Variable_Suit> listaValorVariableSuite = Contexto.Valor_Variable_Suit.Where(VVS => VVS.Variable.IdVariable == idVariable).ToList();
                        foreach (Valor_Variable_Suit itemValorVariable in listaValorVariableSuite)
                        {
                            Contexto.DeleteObject(itemValorVariable);
                        }
                    }
                }

                #endregion

                variableTmp.Nombre = nombreNew;
                variableTmp.Descripcion = descripcion;
                variableTmp.Activo = esActivo;
                variableTmp.Tipo = tipo;
                variableTmp.EsConValidacion = esConValidacion;
                variableTmp.MaxNumero = valMaximo;
                variableTmp.ValorConstante = valorConstante;                

                //Borro todos los valores de la variable que hallan tenido
                List<Valor_Variable> listaValorVariable = Contexto.Valor_Variable.Where(VV => VV.Variable.IdVariable == idVariable).ToList();
                foreach (Valor_Variable itemValorVariable in listaValorVariable)
                {
                    Contexto.DeleteObject(itemValorVariable);
                }

                Contexto.SaveChanges();
            }
        }

        public int Guardar(string nombreVariable, string descripcion, bool esActivo, string tipo, int idHotel, string nombreHotel, string tipoVariable, bool esConValidacion, short valMaximo, int idUsuario, double valorConstante)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Variable variableTmp = new Variable();
                variableTmp.Nombre = nombreVariable;
                variableTmp.Descripcion = descripcion;
                variableTmp.Activo = esActivo;
                variableTmp.Tipo = tipo;
                variableTmp.EsConValidacion = esConValidacion;
                variableTmp.MaxNumero = valMaximo;
                variableTmp.HotelReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Hotel", "IdHotel", idHotel);
                variableTmp.ValorConstante = valorConstante;

                #region auditoria
                Auditoria auditoriaTmp;
                DateTime fecha = DateTime.Now;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = valMaximo + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Valor Maximo";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = ((esConValidacion) ? "Si" : "No") + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Es Con Validacion";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreVariable + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = descripcion + "Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Descripcion";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = ((esActivo) ? "Activo" : "Inactivo") + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Estado";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = nombreVariable + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Insertar";
                auditoriaTmp.Campo = "Tipo Variable";
                auditoriaTmp.Fechahora = fecha;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                #endregion

                Contexto.AddToVariable(variableTmp);
                Contexto.SaveChanges();

                return variableTmp.IdVariable;
            }
        }

        public int Guardar(Variable variableTmp)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Contexto.AddToVariable(variableTmp);
                Contexto.SaveChanges();

                return variableTmp.IdVariable;
            }
        }

        public void Eliminar(int idVariable, string nombreHotel, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Variable variableTmp = Contexto.Variable.Where(V => V.IdVariable == idVariable).FirstOrDefault();

                #region auditoria
                Auditoria auditoriaTmp;

                auditoriaTmp = new Auditoria();
                auditoriaTmp.ValorNuevo = variableTmp.Nombre + " Hotel : " + nombreHotel;
                auditoriaTmp.NombreTabla = "Variables";
                auditoriaTmp.Accion = "Eliminar";
                auditoriaTmp.Campo = "Nombre";
                auditoriaTmp.Fechahora = DateTime.Now;
                auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                Contexto.AddToAuditoria(auditoriaTmp);

                #endregion
                
                Contexto.DeleteObject(variableTmp);
                Contexto.SaveChanges();
            }
        }

        public bool EsRepetida(string nombreVariable, int idHotel, int idVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int count = 0;

                //if (idVariable != -1)
                //{
                //    List<int> listaId = new List<int>();
                //    listaId.Add(idVariable);

                //    count = Contexto.
                //            Variable.
                //            Where(V => V.Hotel.IdHotel == idHotel && V.Nombre.ToUpper() == nombreVariable.ToUpper()).
                //            Select(V => V.IdVariable).
                //            ToList().
                //            Except(listaId).Count();
                //}
                //else
                    count = Contexto.Variable.Where(V => V.Hotel.IdHotel == idHotel && V.Nombre.ToUpper() == nombreVariable.ToUpper()).Count();

                return (count == 0) ? false : true;
            }
        }

        public bool EsEliminar(int idVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int count = Contexto.Variables_Concepto.Where(VC => VC.Variable.IdVariable == idVariable).Count();
                //count += Contexto.Valor_Variable.Where(VV => VV.Variable.IdVariable == idVariable).Count();
                //count += Contexto.Valor_Variable_Suit.Where(VVS => VVS.Variable.IdVariable == idVariable).Count();

                return (count == 0) ? true : false;
            }
        }

        /// <summary>
        /// Valida si las variables tienen valor
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="idHotel"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<ObjetoGenerico> ValidarVariable(string tipo, int idHotel, DateTime fecha)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariablesTmp = new List<ObjetoGenerico>();
                List<Variable> listaVariable = this.VerTodos(idHotel, true, tipo);

                foreach (Variable item in listaVariable)
                {                    
                    Valor_Variable valorTmp = null;

                    valorTmp = Contexto.Valor_Variable.
                               Where(VV => VV.Variable.IdVariable == item.IdVariable && 
                                           VV.Fecha.Month == fecha.Month &&
                                           VV.Fecha.Year == fecha.Year).
                               FirstOrDefault();

                    if (valorTmp == null)
                    {
                        ObjetoGenerico variableTmp = new ObjetoGenerico();
                        variableTmp.Nombre = item.Nombre;

                        listaVariablesTmp.Add(variableTmp);
                    }
                }
                return listaVariablesTmp;
            }
        }

        public List<ObjetoGenerico> VerDetalleVariable(int idVariable, int ano)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                List<ObjetoGenerico> listaVariable = new List<ObjetoGenerico>();
                listaVariable = (from V in Contexto.Variable
                                 join VV in Contexto.Valor_Variable on V.IdVariable equals VV.Variable.IdVariable
                                 where V.IdVariable == idVariable && VV.Fecha.Year == ano
                                 select new ObjetoGenerico()
                                 {
                                     Fecha = VV.Fecha,
                                     Valor = VV.Valor
                                 }).ToList();
                return listaVariable;
            }
        }

        public void ActualizarValorVariable(int idVariable, double valor, DateTime fecha, string nombreHotel, string nombreVariable, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Valor_Variable valorVariableTmp = (from VV in Contexto.Valor_Variable
                                                   join V in Contexto.Variable on VV.Variable.IdVariable equals V.IdVariable
                                                   where V.IdVariable == idVariable && 
                                                         VV.Fecha.Month == fecha.Month && 
                                                         VV.Fecha.Year == fecha.Year
                                                   select VV).FirstOrDefault();
                if (valorVariableTmp == null)
                {
                    #region auditoria
                    Auditoria auditoriaTmp;

                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = nombreHotel + " : " + " Fecha : " + fecha.ToString("dd-MM") + " : " + nombreVariable + " = " + valor.ToString("N");
                    auditoriaTmp.NombreTabla = "Variables";
                    auditoriaTmp.Accion = "Insertar";
                    auditoriaTmp.Campo = "Valor";
                    auditoriaTmp.Fechahora = DateTime.Now;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);
                    #endregion

                    Valor_Variable valorVariableNuevoTmp = new Valor_Variable();
                    valorVariableNuevoTmp.Fecha = fecha;
                    valorVariableNuevoTmp.Valor = valor;
                    valorVariableNuevoTmp.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", idVariable);

                    Contexto.AddToValor_Variable(valorVariableNuevoTmp);
                }
                else
                {
                    #region auditoria
                    Auditoria auditoriaTmp;

                    auditoriaTmp = new Auditoria();
                    auditoriaTmp.ValorNuevo = nombreHotel + " : " + " Fecha : " + fecha.ToString("dd-MM") + " : " + nombreVariable + " = " + valor.ToString("N"); ;
                    auditoriaTmp.ValorAnterior = nombreHotel + " : " + " Fecha : " + valorVariableTmp.Fecha.ToString("dd-MM") + " : " + nombreVariable + " = " + valorVariableTmp.Valor.ToString("N"); ;
                    auditoriaTmp.NombreTabla = "Variables";
                    auditoriaTmp.Accion = "Actualizar";
                    auditoriaTmp.Campo = "Valor";
                    auditoriaTmp.Fechahora = DateTime.Now;
                    auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                    Contexto.AddToAuditoria(auditoriaTmp);
                    #endregion

                    valorVariableTmp.Valor = valor;                   
                }

                Contexto.SaveChanges();
            }
        }

        public void ActualizarValorVariableLista(List<ObjetoGenerico> listaVariable, DateTime fecha, string nombreHotel, int idUsuario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                foreach (ObjetoGenerico itemVariable in listaVariable)
                {
                    Valor_Variable valorVariableTmp = (from VV in Contexto.Valor_Variable
                                                       join V in Contexto.Variable on VV.Variable.IdVariable equals V.IdVariable
                                                       where V.IdVariable == itemVariable.IdVariable &&
                                                             VV.Fecha.Month == fecha.Month &&
                                                             VV.Fecha.Year == fecha.Year
                                                       select VV).FirstOrDefault();


                    if (valorVariableTmp == null)
                    {
                        #region auditoria
                        Auditoria auditoriaTmp;

                        auditoriaTmp = new Auditoria();
                        auditoriaTmp.ValorNuevo = nombreHotel + " : " + " Fecha : " + fecha.ToString("dd-MM") + " : " + itemVariable.Nombre + " = " + itemVariable.Valor.ToString("N");
                        auditoriaTmp.NombreTabla = "Variables";
                        auditoriaTmp.Accion = "Insertar";
                        auditoriaTmp.Campo = "Valor";
                        auditoriaTmp.Fechahora = DateTime.Now;
                        auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                        Contexto.AddToAuditoria(auditoriaTmp);
                        #endregion

                        Valor_Variable valorVariableNuevoTmp = new Valor_Variable();
                        valorVariableNuevoTmp.Fecha = fecha;
                        valorVariableNuevoTmp.Valor = itemVariable.Valor;
                        valorVariableNuevoTmp.VariableReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Variable", "IdVariable", itemVariable.IdVariable);

                        Contexto.AddToValor_Variable(valorVariableNuevoTmp);
                    }
                    else
                    {
                        #region auditoria
                        Auditoria auditoriaTmp;

                        auditoriaTmp = new Auditoria();
                        auditoriaTmp.ValorNuevo = nombreHotel + " : " + " Fecha : " + fecha.ToString("dd-MM") + " : " + itemVariable.Nombre + " = " + itemVariable.Valor.ToString("N"); ;
                        auditoriaTmp.ValorAnterior = nombreHotel + " : " + " Fecha : " + valorVariableTmp.Fecha.ToString("dd-MM") + " : " + itemVariable.Nombre + " = " + valorVariableTmp.Valor.ToString("N"); ;
                        auditoriaTmp.NombreTabla = "Variables";
                        auditoriaTmp.Accion = "Actualizar";
                        auditoriaTmp.Campo = "Valor";
                        auditoriaTmp.Fechahora = DateTime.Now;
                        auditoriaTmp.UsuarioReference.EntityKey = new System.Data.EntityKey("ContextoOwner.Usuario", "IdUsuario", idUsuario);
                        Contexto.AddToAuditoria(auditoriaTmp);
                        #endregion

                        valorVariableTmp.Valor = itemVariable.Valor;
                    }
                }

                Contexto.SaveChanges();
            }
        }

        public double Obtener(int idPropietario, int idVariable, int idSuite, int idHotel, DateTime fechaDesde, DateTime fechaHasta, bool esAcumulado)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                double valor = 0;

                object nombreVariable = (from SP in Contexto.Suit_Propietario
                                         join S in Contexto.Suit on SP.Suit.IdSuit equals S.IdSuit
                                         join H in Contexto.Hotel on S.Hotel.IdHotel equals H.IdHotel
                                         join VVS in Contexto.Valor_Variable_Suit on SP.IdSuitPropietario equals VVS.Suit_Propietario.IdSuitPropietario
                                         where VVS.Variable.IdVariable == idVariable &&
                                               H.IdHotel == idHotel &&
                                               S.IdSuit == idSuite &&
                                               SP.Propietario.IdPropietario == idPropietario
                                         select new { VVS.Variable.Nombre, VVS.Valor }).FirstOrDefault();

                string nameVar = nombreVariable.GetType().GetProperties()[0].GetValue(nombreVariable, null).ToString();

                if (!double.TryParse((nombreVariable.GetType().GetProperties()[1].GetValue(nombreVariable, null).ToString()), out valor))
                {
                    valor = (from L in Contexto.Liquidacion
                             join HL in Contexto.Historial_Liquidacion on L.IdLiquidacion equals HL.Liquidacion.IdLiquidacion
                             where HL.NombreVariable == nameVar &&
                                   L.FechaPeriodoLiquidado.Year == fechaDesde.Year &&
                                   L.FechaPeriodoLiquidado.Month == fechaDesde.Month &&
                                   L.Hotel.IdHotel == idHotel &&
                                   L.Suit.IdSuit == idSuite &&
                                   L.Propietario.IdPropietario == idPropietario
                             select HL.Valor).FirstOrDefault();
                }

                return valor;
            }
        }

        public Variable Obtener(string nombre, string codigoHotel)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return Contexto.Variable.Where(V => V.Nombre.ToUpper() == nombre && V.Hotel.Codigo == codigoHotel).FirstOrDefault();
            }
        }

        public Variable Obtener(int idVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                Variable oVariableDefault = Contexto.Variable.Where(V => V.IdVariable == idVariable).FirstOrDefault();
                if (oVariableDefault == null)
                {
                    oVariableDefault = new Variable();
                    oVariableDefault.ValorConstante = -1;
                    return oVariableDefault;
                }
                return oVariableDefault;
            }
        }

        public ObjetoGenerico ObtenerVariable(int idSuitPropietario, int idVariable)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                return (from VVS in Contexto.Valor_Variable_Suit
                        join V in Contexto.Variable on VVS.Variable.IdVariable equals V.IdVariable
                        where VVS.Suit_Propietario.IdSuitPropietario == idSuitPropietario &&
                              V.IdVariable == idVariable
                        select new ObjetoGenerico()
                        {
                            NombreVariable = V.Nombre,
                            Valor = VVS.Valor
                        }).FirstOrDefault();
            }
        }

    }
}
