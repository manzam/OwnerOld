using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BO
{
    public class DefinicionMovimientoContable
    {
        public Dictionary<string, string> ObtenerEstructura()
        {
            Dictionary<string, string> listaEstructura = new Dictionary<string, string>();
            listaEstructura.Add("F_NUMERO_REG", "");
            listaEstructura.Add("F_TIPO_REG", "0351");
            listaEstructura.Add("F_SUBTIPO_REG", "00");
            listaEstructura.Add("F_VERSION_REG", "01");
            listaEstructura.Add("F_CIA", "");
            listaEstructura.Add("F350_ID_CO", "");
            listaEstructura.Add("F350_ID_TIPO_DOCTO", "");
            listaEstructura.Add("F350_CONSEC_DOCTO", "");
            listaEstructura.Add("F351_ID_AUXILIAR", "");
            listaEstructura.Add("F351_ID_TERCERO", "");
            listaEstructura.Add("F351_ID_CO_MOV", "");
            listaEstructura.Add("F351_ID_UN", "");
            listaEstructura.Add("F351_ID_CCOSTO", "");
            listaEstructura.Add("F351_ID_FE", "");
            listaEstructura.Add("F351_VALOR_DB", "");
            listaEstructura.Add("F351_VALOR_CR", "+000000000000000.0000");
            listaEstructura.Add("F351_VALOR_DB_ALT", "+000000000000000.0000");
            listaEstructura.Add("F351_VALOR_CR_ALT", "+000000000000000.0000");
            listaEstructura.Add("F351_BASE_GRAVABLE", "+000000000000000.0000");
            listaEstructura.Add("F351_DOCTO_BANCO", "");
            listaEstructura.Add("F351_NRO_DOCTO_BANCO", "");
            listaEstructura.Add("F351_NOTAS", "");

            return listaEstructura;
        }
    }
}
