using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DM;
using BO;
using System.Collections.Specialized;
using System.Data;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ContextoOwner ctx = new ContextoOwner();

            List<ObjetoGenerico> respuesta = (from V in ctx.Variable
                                              join VVS in ctx.Valor_Variable_Suit on V.IdVariable equals VVS.Variable.IdVariable
                                              join SP in ctx.Suit_Propietario on VVS.Suit_Propietario.IdSuitPropietario equals SP.IdSuitPropietario
                                              join P in ctx.Propietario on SP.Propietario.IdPropietario equals P.IdPropietario
                                              where V.Activo == true &&
                                                    V.IdVariable == 1721 &&
                                                    V.Hotel.IdHotel == 367
                                              select new ObjetoGenerico()
                                              {
                                                  IdPropietario = P.IdPropietario,
                                                  IdValorVariableSuit = VVS.IdValorVariableSuit,
                                                  IdSuit = SP.Suit.IdSuit,
                                                  Nombre = P.NombrePrimero,
                                                  Apellido = P.ApellidoPrimero
                                              }).OrderBy(P => P.IdPropietario).ToList();

            int idTmp = respuesta[0].IdSuit;
            string ids = "";

            for (int i = 0; i < respuesta.Count; i++)
            {
                if ((i + 1) > respuesta.Count)
                {
 
                }

                int id = respuesta[i + 1].IdSuit;
                if (idTmp == id)
                {
                    ids += ids + respuesta[i].IdValorVariableSuit + ",";
                    System.Console.WriteLine(respuesta[i].IdValorVariableSuit + ",");                    
                }
                idTmp = id;
            }



        }
    }
}
