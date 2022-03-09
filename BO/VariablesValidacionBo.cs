﻿using DM;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class VariablesValidacionBo
    {
        private double ValorSuite { get; set; }
        public Response Validar(List<ObjetoGenerico> listVariable)
        {
            Response result = new Response();

            using (ContextoOwner Contexto = new ContextoOwner())
            {
                foreach (ObjetoGenerico itemVariable in listVariable)
                {
                    Variable oVariable = Contexto.Variable.Where(V => V.IdVariable == itemVariable.IdVariable).FirstOrDefault();
                    itemVariable.EsValidacion = oVariable.EsConValidacion;
                    itemVariable.TipoValidacion = oVariable.TipoValidacion;
                }
                // Validacion participacion %
                ObjetoGenerico variableToValidate = listVariable.Where(V => V.EsValidacion == true && V.TipoValidacion == 1).FirstOrDefault();
                if (variableToValidate != null)
                {
                    result.TipoValidacion = 1;
                    result.Error = ValidationParticipacion100(variableToValidate);
                    result.Lista = GetParticipacion100(variableToValidate.IdVariable.Value, variableToValidate.IdSuit, variableToValidate.IdPropietario);
                    return result;
                }
                // Validacion peso por suite
                variableToValidate = listVariable.Where(V => V.EsValidacion == true && V.TipoValidacion == 3).FirstOrDefault();
                if (variableToValidate != null)
                {
                    ObjetoGenerico variableSuite = listVariable.Where(V => V.EsValidacion == true && V.TipoValidacion == 4).FirstOrDefault();

                    result.TipoValidacion = 3;
                    result.Error = ValidationAgrupadaPesoSuite(variableToValidate, variableSuite.IdVariable.Value);
                    result.Lista = GetParticipacionPesoSuite(variableToValidate.IdVariable.Value, variableToValidate.IdSuit);
                    return result;
                }

            }
            return result;
        }

        private string ValidationAgrupadaPesoSuite(ObjetoGenerico variableToValidate, int idVariableSuite)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                // Calculo del peso de todos los propietarios
                string sqlProp = $@"SELECT sum(Valor_Variable_Suit.Valor) valor
                                            FROM Suit_Propietario 
                                            INNER JOIN Valor_Variable_Suit ON Suit_Propietario.IdSuitPropietario = Valor_Variable_Suit.IdSuitPropietario
                                            INNER JOIN Variable ON Valor_Variable_Suit.IdVariable = Variable.IdVariable
                                            WHERE Valor_Variable_Suit.IdVariable = {variableToValidate.IdVariable} and Suit_Propietario.IdSuit = {variableToValidate.IdSuit} and 
                                            Suit_Propietario.IdPropietario <> {variableToValidate.IdPropietario} ";
                object valueProp = Utilities.ExecuteScalar(sqlProp);
                // El peso de la suite
                string sqlSuite = $@"SELECT top 1 Valor_Variable_Suit.Valor
                                                FROM Suit_Propietario 
                                                INNER JOIN Valor_Variable_Suit ON Suit_Propietario.IdSuitPropietario = Valor_Variable_Suit.IdSuitPropietario
                                                INNER JOIN Variable ON Valor_Variable_Suit.IdVariable = Variable.IdVariable
                                                WHERE 
                                                Valor_Variable_Suit.IdVariable = {idVariableSuite} and Suit_Propietario.IdSuit = {variableToValidate.IdSuit} ";

                object valueSuite = Utilities.ExecuteScalar(sqlSuite);

                double valorProp = 0;
                double valorSuite = 0;
                if (valueProp != null && valueSuite != null)
                {
                    valorProp = Convert.ToDouble(valueProp) + variableToValidate.Valor;
                    valorSuite = Convert.ToDouble(valueSuite);
                    this.ValorSuite = valorSuite;
                }

                if (valorProp > valorSuite)
                    return "La sumatoria de los coeficientes propietario supera el coeficiente de la suite";

                return "";
            }
        }

        private List<ObjetoGenerico> GetParticipacionPesoSuite(int idVariable, int idSuit)
        {
            string sql = $@"SELECT 
                            (Propietario.NombrePrimero + ' ' + Propietario.NombreSegundo + ' ' + Propietario.ApellidoPrimero + ' ' + Propietario.ApellidoSegundo) Nombre, 
                            Propietario.NumIdentificacion, 
                            Suit.NumSuit,
                            Valor_Variable_Suit.Valor
                            FROM Suit_Propietario 
                            INNER JOIN Valor_Variable_Suit ON Suit_Propietario.IdSuitPropietario = Valor_Variable_Suit.IdSuitPropietario 
                            INNER JOIN Propietario ON Suit_Propietario.IdPropietario = Propietario.IdPropietario 
                            INNER JOIN Suit ON Suit_Propietario.IdSuit = Suit.IdSuit
                            WHERE (Valor_Variable_Suit.IdVariable = {idVariable}) AND (Suit_Propietario.IdSuit = {idSuit})";
            DataTable dtProp = Utilities.Select(sql, "listPtop");
            if (dtProp != null)
            {
                return (from row in dtProp.AsEnumerable()
                        select new ObjetoGenerico()
                        {
                            Nombre = row["Nombre"].ToString(),
                            NumIdentificacion = row["NumIdentificacion"].ToString(),
                            NumSuit = row["NumSuit"].ToString(),
                            Valor = Convert.ToDouble(row["Valor"]),
                            ValorSuite = this.ValorSuite
                        }).ToList();
            }
            return new List<ObjetoGenerico>();
        }

        private string ValidationParticipacion100(ObjetoGenerico variableToValidate)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                string sql = $@"SELECT (isnull(sum(Valor_Variable_Suit.Valor),0) + {variableToValidate.Valor}) valor
                                        FROM Suit_Propietario 
                                        INNER JOIN Valor_Variable_Suit ON Suit_Propietario.IdSuitPropietario = Valor_Variable_Suit.IdSuitPropietario
                                        INNER JOIN Variable ON Valor_Variable_Suit.IdVariable = Variable.IdVariable
                                        WHERE Suit_Propietario.EsActivo = 1 and Valor_Variable_Suit.IdVariable = {variableToValidate.IdVariable} and 
                                        Suit_Propietario.IdSuit = {variableToValidate.IdSuit} and Suit_Propietario.IdPropietario <> {variableToValidate.IdPropietario} ";

                float valorTmp = 0;
                object value = Utilities.ExecuteScalar(sql);
                if (value == null)
                    value = 0;

                valorTmp = float.Parse(value.ToString());

                if (valorTmp > 1)
                    return "La participación supera el 100%";

                return "";
            }
        }

        private List<ObjetoGenerico> GetParticipacion100(int idVariable, int idSuit, int idPropietario)
        {
            string sql = $@"SELECT 
                            (Propietario.NombrePrimero + ' ' + Propietario.NombreSegundo + ' ' + Propietario.ApellidoPrimero + ' ' + Propietario.ApellidoSegundo) Nombre, 
                            Propietario.NumIdentificacion, 
                            Suit.NumSuit,
                            Valor_Variable_Suit.Valor
                            FROM Suit_Propietario 
                            INNER JOIN Valor_Variable_Suit ON Suit_Propietario.IdSuitPropietario = Valor_Variable_Suit.IdSuitPropietario 
                            INNER JOIN Propietario ON Suit_Propietario.IdPropietario = Propietario.IdPropietario 
                            INNER JOIN Suit ON Suit_Propietario.IdSuit = Suit.IdSuit
                            WHERE (Valor_Variable_Suit.IdVariable = {idVariable}) AND (Suit_Propietario.IdSuit = {idSuit}) AND (Suit_Propietario.IdPropietario <> {idPropietario}) ";
            DataTable dtProp = Utilities.Select(sql, "listPtop");
            if (dtProp != null)
            {
                return (from row in dtProp.AsEnumerable()
                        select new ObjetoGenerico()
                        {
                            Nombre = row["Nombre"].ToString(),
                            NumIdentificacion = row["NumIdentificacion"].ToString(),
                            NumSuit = row["NumSuit"].ToString(),
                            Valor = Convert.ToDouble(row["Valor"])
                        }).ToList();
            }
            return new List<ObjetoGenerico>();
        }
    }
}
