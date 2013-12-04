using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Trendings;
//using RestApiLib;
//using System.Web.UI;

namespace sga.Controllers
{
    public class ServiceController : Controller
    {

        Models.DATOS_SCADAEntities1 Object = new Models.DATOS_SCADAEntities1();

        public JsonResult AlarmasCantidad(int idLinea, DateTime fechaInicio, DateTime fechaFin, int intervalos)
        {
            IEnumerable<Models.alarmas_cantidad_Result> datos;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            TrendingParser parser = new TrendingParser();

            datos = Object.alarmas_cantidad(idLinea, fechaInicio, fechaFin, intervalos);

            //return Json(serializer.Serialize (parser.Parser(datos)), JsonRequestBehavior.AllowGet);
            return Json(parser.Parser(datos), JsonRequestBehavior.AllowGet);
        }

    }
}
