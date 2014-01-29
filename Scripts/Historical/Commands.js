﻿/*
Nombre:         Commands.js
Fecha:          27/01/2014
Autor:          Fabricio salinas
Proposito:      Utilerías comunes para histórico de comandos
*/

function ActualizaFechasDatapicker() {
    var diaIniConFormato = '';
    var mesIniConFormato = '';
    var diafinConFormato = '';
    var mesFinConFormato = '';
    var maxMonth = 1;
    var maxYear = 1;
    // obtiene datos fecha actual
    var mesSelected = $("#ddlFiltroMes").val();
    var anioSelected = $("#ddlFiltroAnio").val();

    // obtiene minima fecha
    var minDate = $("#txtFiltroDiaInicio").datepicker("getDate");

    // obtiene máxima fecha
    if (11 == minDate.getMonth()) {
        maxMonth = 1;
        maxYear = minDate.getFullYear() + 1;
    }
    else {
        maxMonth = minDate.getMonth() + 1;
        maxYear = minDate.getFullYear();
    }

    //var maxDate = new Date(minDate.getTime() + (30 * 24 * 3600 * 1000));  // cantidad de días después
    var maxDate = new Date(maxYear, maxMonth, minDate.getDate() - 1);

    // define limites para fecha fin
    $("#txtFiltroDiaFin").datepicker("option", "minDate", minDate);
    $("#txtFiltroDiaFin").datepicker("option", "maxDate", maxDate);

    // define dia actual para el inicio
    $("#txtFiltroDiaInicio").datepicker('setDate', minDate);
    $("#txtFiltroDiaFin").datepicker('setDate', minDate);

}

function FiltrarHistoricoComandos() {
    // actualiza filtros
    ActualizaFiltroContainers();

    // actualiza grilla
    $("#historical").dataTable().fnReloadAjax();
}

function ActualizaFiltroContainers() {
    // almacena filtros
    $("#hidFiltroDescripcion").val($("#txtFiltroDescripcion").val());
    $("#hidFiltroDireccion").val($("#txtFiltroDireccion").val());
    $("#hidFiltroSistema").val($("#ddlFiltroSistema").val());
    $("#hidFiltroNode").val($("#ddlFiltroNodo").val());
    $("#hidFiltroDiaInicio").val($("#txtFiltroDiaInicio").val());
    $("#hidFiltroDiaFin").val($("#txtFiltroDiaFin").val());
    $("#hidIdLinea").val($("#ddlFiltroLinea").val());

    if ('' == $("#txtFiltroCodigo").val()) {
        $("#hidFiltroCodigo").val('-1');5
    }
    else {
        $("#hidFiltroCodigo").val($("#txtFiltroCodigo").val());
    }
}

function dataTableRezize() {
    $('.dataTables_scrollBody').css('height', $('.content-main').height() - ($('.dataTables_filter').height() * 2) - $('.dataTables_info').height() - $('.dataTables_scrollHead').height());
}

function VerCommandInfo(idCommand, timeOn) {
    var params;
    var url = $("#hidUrlAlarmInfo").val();
    var idLinea = $("#hidIdLinea").val();

    // define parámetros (json)
    params = {
        "idLinea": idLinea,
        "idCommand": idCommand,
        "timeOn": timeOn
    };

    // carga partial view en el diálogo
    CallBacks.loadPartialView(url, "POST", VerCommandInfoDialog, params);
}

function VerCommandInfoDialog(data) {
    // llena el div con el contenido devuelto por el partial view
    $("#divDialogCommandInfo").html(data);

    // muestra el diálogo
    $("#divDialogCommandInfo").dialog('open');
}

function ActualizaComboSistema() {
    var params;
    var url = $("#hidUrlActualizaComboSistema").val();
    var idLinea = $("#ddlFiltroLinea").val();

    params = {
        "idLinea": idLinea
    };

    // llama al aacción que devolverá resultados para el combo
    CallBacks.loadJsonView(url, "POST", ActualizaComboSistemaFill, params);
}

function ActualizaComboSistemaFill(jsonvalue) {
    var combo = $("#ddlFiltroSistema").msDropDown().data("dd");

    // destruye combo
    combo.destroy();

    // vacía drop down
    $("#ddlFiltroSistema").empty();

    // agrega option "Seleccionar"
    combo.add(new Option("Todos", "-1"));

    // agregar los option obtenidos
    $.each(jsonvalue, function (i, data) {
        var desc = data.description;
        var id = data.idPlc;

        combo.add(new Option(desc.toString(), id.toString()));
    });

    // actualiza el nombre de la línea
    $("#lblLinea").text($("#ddlFiltroLinea option:selected").text());
}

