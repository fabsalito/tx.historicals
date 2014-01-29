using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables;
using sga.ViewModels;

namespace sga.Controllers
{
    public class CommandController : Controller
    {

        Models.DATOS_SCADAEntities1 Object = new Models.DATOS_SCADAEntities1();

        public ActionResult Index(int idLinea = -1)
        {
            ViewModels.HistoricoComandosViewModel datos = new ViewModels.HistoricoComandosViewModel();
            IEnumerable<Models.llenar_combo_sistemas_Result> sistemas;
            IEnumerable<Models.llenar_combo_nodos_Result> nodos;
            IEnumerable<Models.get_lines_Result> lines;
            string linea = "";

            ViewBag.idLinea = idLinea;

            try
            {
                // obtiene sistemas
                sistemas = Object.llenar_combo_sistemas(idLinea).ToList();

                // obtiene nodos
                nodos = Object.llenar_combo_nodos(idLinea).ToList();

                // obtiene lineas
                lines = Object.get_lines(idLinea).ToList();

                // verifica si se obtuvo al menos una linea
                if (0 == lines.Count())
                {
                    // re-define idLinea
                    ViewBag.idLinea = -1;
                    idLinea = -1;

                    // obtiene todas lineas
                    lines = Object.get_lines(-1).ToList();
                }

                // obtiene nombre de línea
                if (-1 == idLinea)
                {
                    // texto para línea
                    linea = "[Seleccione línea]";
                }
                else
                {
                    // texto para linea
                    linea = lines.First().line;
                }

                // setea nombre de linea
                ViewBag.linea = linea;

                // define datos para la vista
                datos.sistemas = sistemas;
                datos.nodos = nodos;
                datos.lines = lines;

                return View(datos);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public JsonResult List()
        {
            IEnumerable<Models.get_commands_historical_Result> commandsEnum;
            bool sessionNull = false;
            bool sessionChanged = false;
            DateTime fechaInicio = Convert.ToDateTime(Request.QueryString["cusInicio"]);
            DateTime fechaFin = Convert.ToDateTime(Request.QueryString["cusFin"]);
            int idLinea = Convert.ToInt32(Request.QueryString["cusIdLinea"]);
            string descripcion = Request.QueryString["cusDescription"];
            string codigo = Request.QueryString["cusCodigo"];
            string direccion = Request.QueryString["cusDireccion"];
            int idSistema = Convert.ToInt32(Request.QueryString["cusSistema"]);
            string idNodo = Convert.ToString(Request.QueryString["cusNode"]);

            try
            {
                // verifica si existen las variables de sesión
                if (null == Session["sesInicio"] ||
                    null == Session["sesFin"] ||
                    null == Session["sesDescription"] ||
                    null == Session["sesCodigo"] ||
                    null == Session["sesDireccion"] ||
                    null == Session["sesSistema"] ||
                    null == Session["sesNode"] ||
                    null == Session["sesLine"])
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
                    Session["sesNode"] = idNodo;
                    Session["sesLine"] = idLinea;

                }

                // verifica si los parámetros han cambiado
                if (Session["sesInicio"].ToString() != fechaInicio.ToString() ||
                    Session["sesFin"].ToString() != fechaFin.ToString() ||
                    Session["sesDescription"].ToString() != descripcion ||
                    Session["sesCodigo"].ToString() != codigo ||
                    Session["sesDireccion"].ToString() != direccion ||
                    Session["sesSistema"].ToString() != idSistema.ToString() ||
                    Session["sesNode"].ToString() != idNodo.ToString() ||
                    Session["sesLine"].ToString() != idLinea.ToString())
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
                    Session["sesNode"] = idNodo;
                    Session["sesLine"] = idLinea;
                }

                // verifica si debe consultar nuevamente
                if (null == Session["comandos"] || sessionNull || sessionChanged)
                {
                    // obtiene los datos de alarmas
                    commandsEnum = Object.get_commands_historical(idLinea,
                        idSistema,
                        idNodo,
                        descripcion,
                        direccion,
                        codigo,
                        fechaInicio,
                        fechaFin).ToList();

                    // guarda en sesión
                    Session["comandos"] = commandsEnum;
                }
                else
                {
                    commandsEnum = (IEnumerable<Models.get_commands_historical_Result>)Session["comandos"];
                }

                IQueryable<Models.get_commands_historical_Result> comandos = commandsEnum.AsQueryable();

                if (Request["sEcho"] != null)
                {

                    var parser = new DataTableParser<Models.get_commands_historical_Result>(Request, comandos);

                    return Json(parser.Parse(), JsonRequestBehavior.AllowGet);

                }

                return Json(comandos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult ActualizaComboSistema(int idLinea = -1)
        {
            IEnumerable<Models.llenar_combo_sistemas_Result> sistemas;

            try
            {
                // obtiene sistemas
                sistemas = Object.llenar_combo_sistemas(idLinea).ToList();

                return Json(sistemas);
            }
            catch (Exception e)
            {
                throw (e);
            }

        }

        public ActionResult VerCommandInfo(int idLinea, int idCommand, DateTime timeOn)
        {
            IEnumerable<Models.obtener_info_alarma_Result> alarma;
            ViewModels.InfoAlarmasViewModel data = new InfoAlarmasViewModel();

            alarma = Object.obtener_info_alarma(idLinea, idCommand, timeOn).ToList();

            data.alarma = alarma.First();

            ViewBag.idLinea = idLinea;
            ViewBag.idAlarm = idCommand;
            ViewBag.timeOn = timeOn;

            return View(data);
        }

    }
}
