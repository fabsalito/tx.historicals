﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables;
using sga.ViewModels;

namespace sga.Controllers
{
    public class AlarmController : Controller
    {

        Models.DATOS_SCADAEntities1 Object = new Models.DATOS_SCADAEntities1();

        public ActionResult Index(int idLinea = -1)
        {
            ViewModels.HistoricoAlarmasViewModel datos = new ViewModels.HistoricoAlarmasViewModel();
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
            IEnumerable<Models.obtener_historico_alarmas_Result> alarmasEnum;
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
                if ("" != valShowEvents){
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
                    null == Session["sesNode"] ||
                    null == Session["sesLine"] ||
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
                    Session["sesNode"] = idNodo;
                    Session["sesLine"] = idLinea;
                    Session["sesShowEvents"] = showEvents;
                    Session["sesShowAusNoRec"] = showAusNoRec;
                    Session["sesShowPreNoRec"] = showPreNoRec;
                    Session["sesShowPreRec"] = showPreRec;

                }

                // verifica si los parámetros han cambiado
                if (Session["sesInicio"].ToString() != fechaInicio.ToString() ||
                    Session["sesFin"].ToString() != fechaFin.ToString() ||
                    Session["sesDescription"].ToString() != descripcion ||
                    Session["sesCodigo"].ToString() != codigo ||
                    Session["sesDireccion"].ToString() != direccion ||
                    Session["sesSistema"].ToString() != idSistema.ToString() ||
                    Session["sesNode"].ToString() != idNodo.ToString() ||
                    Session["sesLine"].ToString() != idLinea.ToString() ||
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
                    Session["sesNode"] = idNodo;
                    Session["sesLine"] = idLinea;
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

        public JsonResult DireccionAutocomplete(string term)
        {
            IEnumerable<Models.obtener_direccion_alarmas_Result> descripcion;

            descripcion = Object.obtener_direccion_alarmas(-1, term);

            var autoCompleteData = descripcion.Select(x => new { descripcion = x.address, }).ToArray();

            return Json(autoCompleteData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerAlarmInfo(int idLinea, int idAlarm, DateTime timeOn)
        {
            IEnumerable<Models.obtener_info_alarma_Result> alarma;
            ViewModels.InfoAlarmasViewModel data = new InfoAlarmasViewModel();

            alarma = Object.obtener_info_alarma(idLinea, idAlarm, timeOn).ToList();

            data.alarma = alarma.First();

            ViewBag.idLinea = idLinea;
            ViewBag.idAlarm = idAlarm;
            ViewBag.timeOn = timeOn;

            return View(data);
        }

        public ActionResult TooltipEstados()
        {
            IEnumerable<Models.get_alarm_status_Result> estados;
            ViewModels.InfoAlarmasViewModel data;

            try
            {
                // obtiene estados
                estados = Object.get_alarm_status(-1).ToList();

                data = new ViewModels.InfoAlarmasViewModel();

                data.estados = estados;

                return View(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
