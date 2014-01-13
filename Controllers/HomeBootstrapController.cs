using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables;
using sga.ViewModels;

namespace sga.Controllers
{
    public class HomeBootstrapController : Controller
    {

        Models.DATOS_SCADAEntities1 Object = new Models.DATOS_SCADAEntities1();

        public ActionResult Index()
        {
            ViewModels.HistoricoAlarmasViewModel datos = new ViewModels.HistoricoAlarmasViewModel();
            IEnumerable<Models.llenar_combo_sistemas_Result> sistemas;

            try
            {
                // obtiene sistemas
                sistemas = Object.llenar_combo_sistemas(-1);

                // define datos para la vista
                datos.sistemas = sistemas;

                return View(datos);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public JsonResult List()
        {
            IEnumerable<Models.obtener_historico_alarmas_Result> alarmasEnum;
            bool sessionNull = false;
            bool sessionChanged = false;
            DateTime fechaInicio = Convert.ToDateTime(Request.QueryString["cusInicio"]);
            DateTime fechaFin = Convert.ToDateTime(Request.QueryString["cusFin"]);
            string descripcion = Request.QueryString["cusDescription"];
            int codigo = Convert.ToInt32(Request.QueryString["cusCodigo"]);
            string direccion = Request.QueryString["cusDireccion"];
            int idSistema = Convert.ToInt32(Request.QueryString["cusSistema"]);
            string idNodo = Convert.ToString(Request.QueryString["cusNodo"]);
            int idLinea = Convert.ToInt32(Request.QueryString["cusIdLinea"]);
            string valShowEvents = Request.QueryString["cusShowEvents"];
            string valShowAusNoRec = Request.QueryString["cusShowAusNoRec"];
            string valShowPreNoRec = Request.QueryString["cusShowPreNoRec"];
            string valShowPreRec = Request.QueryString["cusShowPreRec"];
            bool showEvents = false;
            bool showAusNoRec = false;
            bool showPreNoRec = false;
            bool showPreRec = false;

            try
            {
                // verifica si debe mostrar eventos
                if ("" != valShowEvents)
                {
                    showEvents = true;
                }

                // verifica si debe mostrar ausentes no reconocidas
                if ("" != valShowAusNoRec)
                {
                    showAusNoRec = true;
                }

                // verifica si debe mostrar presentes no reconocidas
                if ("" != valShowPreNoRec)
                {
                    showPreNoRec = true;
                }

                // verifica si debe mostrar presentes reconocidas
                if ("" != valShowPreRec)
                {
                    showPreRec = true;
                }

                // verifica si existen las variables de sesión
                if (null == Session["sesInicio"] ||
                    null == Session["sesFin"] ||
                    null == Session["sesDescription"] ||
                    null == Session["sesCodigo"] ||
                    null == Session["sesDireccion"] ||
                    null == Session["sesSistema"] ||
                    null == Session["sesShowEvents"] ||
                    null == Session["sesShowAusNoRec"] ||
                    null == Session["sesShowPreNoRec"] ||
                    null == Session["sesShowPreRec"])
                {
                    // indica que las variables de sesión no existen
                    sessionNull = true;

                    // define las varaibles de sesión
                    Session["sesInicio"] = fechaInicio;
                    Session["sesFin"] = fechaFin;
                    Session["sesDescription"] = descripcion;
                    Session["sesCodigo"] = codigo;
                    Session["sesDireccion"] = direccion;
                    Session["sesSistema"] = idSistema;
                    Session["sesShowEvents"] = showEvents;
                    Session["sesShowAusNoRec"] = showAusNoRec;
                    Session["sesShowPreNoRec"] = showPreNoRec;
                    Session["sesShowPreRec"] = showPreRec;

                }

                // verifica si los parámetros han cambiado
                if (Session["sesInicio"].ToString() != fechaInicio.ToString() ||
                    Session["sesFin"].ToString() != fechaFin.ToString() ||
                    Session["sesDescription"].ToString() != descripcion ||
                    Convert.ToInt32(Session["sesCodigo"]) != codigo ||
                    Session["sesDireccion"].ToString() != direccion ||
                    Session["sesSistema"].ToString() != idSistema.ToString() ||
                    Session["sesShowEvents"].ToString() != showEvents.ToString() ||
                    Session["sesShowAusNoRec"].ToString() != showAusNoRec.ToString() ||
                    Session["sesShowPreNoRec"].ToString() != showPreNoRec.ToString() ||
                    Session["sesShowPreRec"].ToString() != showPreRec.ToString())
                {
                    // indica que las variables de sesión no existen
                    sessionChanged = true;

                    // define las varaibles de sesión
                    Session["sesInicio"] = fechaInicio;
                    Session["sesFin"] = fechaFin;
                    Session["sesDescription"] = descripcion;
                    Session["sesCodigo"] = codigo;
                    Session["sesDireccion"] = direccion;
                    Session["sesSistema"] = idSistema;
                    Session["sesShowEvents"] = showEvents;
                    Session["sesShowAusNoRec"] = showAusNoRec;
                    Session["sesShowPreNoRec"] = showPreNoRec;
                    Session["sesShowPreRec"] = showPreRec;
                }

                // verifica si debe consultar nuevamente
                if (null == Session["alarmas"] || sessionNull || sessionChanged)
                {
                    // obtiene los datos de alarmas
                    alarmasEnum = Object.obtener_historico_alarmas(idLinea,
                        idSistema,
                        idNodo,
                        descripcion,
                        direccion,
                        codigo,
                        fechaInicio,
                        fechaFin,
                        showEvents,
                        showPreNoRec,
                        showPreRec,
                        showAusNoRec).ToList();

                    // guarda en sesión 
                    Session["alarmas"] = alarmasEnum;
                }
                else
                {
                    alarmasEnum = (IEnumerable<Models.obtener_historico_alarmas_Result>)Session["alarmas"];
                }

                IQueryable<Models.obtener_historico_alarmas_Result> alarmas = alarmasEnum.AsQueryable();

                if (Request["sEcho"] != null)
                {

                    var parser = new DataTableParser<Models.obtener_historico_alarmas_Result>(Request, alarmas);

                    return Json(parser.Parse(), JsonRequestBehavior.AllowGet);

                }

                return Json(alarmas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult DireccionAutocomplete(string term)
        {
            IEnumerable<Models.obtener_direccion_alarmas_Result> descripcion;

            descripcion = Object.obtener_direccion_alarmas(-1, term);

            var autoCompleteData = descripcion.Select(x => new { descripcion = x.address, }).ToArray();

            return Json(autoCompleteData, JsonRequestBehavior.AllowGet);
        }
    }
}
