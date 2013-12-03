using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sga.ViewModels
{
    public class HistoricoAlarmasViewModel
    {
        public IEnumerable<Models.obtener_historico_alarmas_Result> alarmas;
        public IEnumerable<Models.llenar_combo_sistemas_Result> sistemas;
        public IEnumerable<Models.llenar_combo_nodos_Result> nodos;
    }
}