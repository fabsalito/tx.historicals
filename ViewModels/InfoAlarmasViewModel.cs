using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sga.ViewModels
{
    public class InfoAlarmasViewModel
    {
        public Models.obtener_info_alarma_Result alarma;
        public IEnumerable<Models.get_alarm_status_Result> estados;
    }
}