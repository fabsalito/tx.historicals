/// <reference path="jquery-vsdoc.js" />

$(document).ready(function () {

    //Elijo si pongo paginacion o barra de progreso:
    //$('.contenedorBarraProgreso').hide("fast");
    //$('.contenedorBarraPaginador').hide("fast");

    // redefine el tamaño del contenido
    $('.content-main').css('height', $(window).height() - $('.content-header').height()); // el 50 es el padding-top del body; usar media queries

    dataTableRezize();

    //Muestra o no el encabezado
    $(document).on("click", ".contenedorFiltroColapsable", function () {
        $(this).parent().children('.contenidoFiltros').slideToggle("slow", function () {
            // redefine el tamaño del contenido
            $('.content-main').css('height', $(window).height() - $('.content-header').height()); // el 50 es el padding-top del body; usar media queries

            // modifica height de grilla
            dataTableRezize();
        });
    });

    // define acciones para el windows resize
    window.onresize = function () {
        $('.content-main').css('height', $(window).height() - $('.content-header').height()); // el 50 es el padding-top del body; usar media queries

        // modifica height de grilla
        dataTableRezize();
    };

    //Cambia la flechita al mostrar u ocultar de fondo
    $(document).on("toggle", ".contenedorFiltroColapsable", function () {
        $(this).addClass("barraFiltroColapsada");
    }, function () {
        $(this).removeClass("barraFiltroColapsada");
    });

    //Muestra el calendario donde se hizo click
    $(document).on("click", ".txtCalendario", function () {
        id = $(this).attr("id");
        mostrarCalendario(id);
    });



    //Efecto que pone gris los input de Texto
    $(document).on("focus", ".contenedorGeneralFiltro .txtStd", function () {
        //$(this).removeClass('inputContenido');
        $(this).addClass('inputFocus');
    }).on("blur", ".contenedorGeneralFiltro .txtStd", function () {
        $(this).removeClass('inputFocus inputContenido');
        if ($(this).val() != "") {
            //$(this).addClass('inputContenido');
            //pintarCampoConContenido($(this).attr("id"));
        }
    });

    //Efecto que pone gris los Selects
    $(document).on("focus", ".contenedorGeneralFiltro .cmbStd", function () {
        //$(this).removeClass('inputContenido');
        $(this).addClass('inputFocus');
    }).on("blur", ".contenedorGeneralFiltro .cmbStd", function () {
        $(this).removeClass('inputFocus inputContenido');
        if ($(this).val() != "-1" && $(this).val() != "") {
            //$(this).addClass('inputContenido');
            //pintarCampoConContenido($(this).attr("id"));
        }
    });


    var elementos = $(".contenedorGeneralFiltro .txtStd, .contenedorGeneralFiltro .cmbStd");
    for (var i = 0; i < elementos.length; i++) {
        id = elementos[i]['id'];
        if ($('#' + id).val() != "") {
            //pintarCampoConContenido(id);
        }
    }

    //Contabiliza el contenido inicial de cada TexArea con limite de caracteres.
    var elementos = $(".contabilizarTexto");
    for (var i = 0; i < elementos.length; i++) {
        id = elementos[i]['id'];

        contenido = $('#' + id).val();
        $('#' + id).attr("contenidoActual", contenido);

        var maxText = $('#' + id).attr("maxText");

        var porcentaje = calculoResto(id, maxText);

        if (porcentaje > 100) {
            $('.divBarraProgreso[txtArea=' + id + ']').children("div").css("width", "100%").children('span').html(">100");
        } else {
            $('.divBarraProgreso[txtArea=' + id + ']').children("div").css("width", porcentaje + "%").children('span').html(porcentaje);
            $('#' + id).attr("contenidoActual", $('#' + id).val());
        }

    }


    $(document).on("keyup", ".contabilizarTexto", function () {
        var maxText = $(this).attr("maxText");
        var id = $(this).attr("id");

        var porcentaje = calculoResto(id, maxText);

        if (porcentaje > 100) {
            alert("No puede ingresar mas caracteres");
            var contenidoActual = $(this).attr("contenidoActual");
            $(this).val(contenidoActual);
        } else {
            $('.divBarraProgreso[txtArea=' + id + ']').children("div").css("width", porcentaje + "%").children('span').html(porcentaje);
            $(this).attr("contenidoActual", $(this).val());
        }

    });

    $(document).on("focus", ".contabilizarTexto", function () {
        var id = $(this).attr("id");
        $('.divBarraProgreso[txtArea=' + id + ']').fadeIn(500);
    });

    $(document).on("blur", ".contabilizarTexto", function () {
        var id = $(this).attr("id");
        $('.divBarraProgreso[txtArea=' + id + ']').fadeOut(500);
    });



    //renderizaTablas();

    //muestraOpcionesColumna();

    //muestraTooltips();


    //Para la funcionalidad de busqueda por numero tisa/numero siderar
    $.expr[':'].containsIgnoreCase = function (n, i, m) {
        return jQuery(n).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
    };

    $(document).on("keyup", ".txtFiltroBusqueda", function () {

        var columna = $(this).attr("columna");

        $(".tablaGeneral").find("tbody tr").hide();
        var data = $(this).val();

        $(".tablaGeneral").find("thead tr th").removeClass("columnaFiltrada");

        if (data == "") {
            $(".tablaGeneral").find("tbody tr").show();
        } else {
            var jo = $(".tablaGeneral").find("tbody tr").each(function () {
                var celda = $(this).children("td:eq(" + columna + "):containsIgnoreCase('" + data + "')").html();
                if (celda != null) {
                    $(this).show();
                }
            });
            $(".tablaGeneral").find("thead tr th:eq(" + columna + ")").addClass("columnaFiltrada");
        }
        //Borro todos los valores de los filtros y pongo el que esta escribiendo.
        $(".tablaGeneral").find("thead tr th").attr("valorFiltro", "");
        $(".tablaGeneral").find("thead tr th:eq(" + columna + ")").attr("valorFiltro", data);

    });



});                     //FIN document ready


function calculoResto(textArea, cantidadMax) {
    var cantidadEscrita = $('#' + textArea).val().length;
    var porcentaje = (cantidadEscrita * 100) / cantidadMax;
    return porcentaje.toFixed(1);
}


function inicializarComboImagenes() {
    $(".cmbConImagen").msDropDown();
}

function renderizaTablas(tabla, alto) {
    $("." + tabla + " tr:odd[class!=encabezado] td").addClass("filaImpar"); // filas impares
    $("." + tabla + " tr:even[class!=encabezado] td").addClass("filaPar"); // filas pares

    $("." + tabla).fixedHeaderTable({
        footer: false,
        cloneHeadToFoot: false,
        fixedColumn: false,
        height: alto + 'px',
        borderCollapse: true
    });

    $(document).on("click", ".muestraOpcionesColumna", function () {
        muestraOpcionesColumna($(this));
    });
}


function muestraOpcionesColumna(element) {

    if (element.attr("valorFiltro") === undefined) {
        valorFiltro = "";
    } else {
        valorFiltro = element.attr("valorFiltro");
    }

    var contenidoTooltip = "Filtrar: <input type='text' class='txtStd txtMediano txtFiltroBusqueda' columna='" + $(element).index() + "' value='" + valorFiltro + "'>";
    $(element).qtip({
        content: {
            text: contenidoTooltip,
            title: {
                text: 'Mas opciones sobre la columna',
                button: 'Close'
            }
        },
        hide: {
            event: false
            //event: 'click'
        },
        show: {
            event: false,
            ready: true, // Needed to make it show on first mouseover event
            solo: true
        },
        style: {
            classes: 'ui-tooltip-rounded ui-tooltip-shadow'
        },
        position: {
            my: 'bottom center',
            at: 'top center'
            /*target: 'mouse',
            adjust: {
            x: -7, y: 10
            }*/
        }
    });

    element.qtip().show();
    $('.txtFiltroBusqueda').focus();
    return false;

}

function muestraTooltips() {

    $('.muestraTooltip, .muestraTooltipArriba').qtip({
        style: {
            classes: 'ui-tooltip-rounded ui-tooltip-shadow'
        },
        position: {
            my: 'bottom center',
            at: 'top center'
            /*target: 'mouse',
            adjust: {
            x: 10, y: 0
            }*/
        }
    });

    $('.muestraTooltipDerecha').qtip({
        style: {
            classes: 'ui-tooltip-rounded ui-tooltip-shadow'
        },
        position: {
            my: 'left center',
            at: 'right center'
            /*target: 'mouse',
            adjust: {
            x: 10, y: 0
            }*/
        }
    });

    $('.muestraTooltipAbajo').qtip({
        style: {
            classes: 'ui-tooltip-rounded ui-tooltip-shadow'
        },
        position: {
            my: 'top center',
            at: 'bottom center'
            /*target: 'mouse',
            adjust: {
            x: 10, y: 0
            }*/
        }
    });

    $('.muestraTooltipIzquierda').qtip({
        style: {
            classes: 'ui-tooltip-rounded ui-tooltip-shadow'
        },
        position: {
            my: 'right center',
            at: 'left center'
            /*target: 'mouse',
            adjust: {
            x: 10, y: 0
            }*/
        }
    });

}


function limpiarBusqueda() {
    $('.contenidoFiltroCampos input[type=text]').removeClass('inputContenido').val('');
    $('.itemFiltro select').removeClass('inputContenido').prop('selectedIndex', 0);
}

function pintarCampoConContenido(id) {
    $('#' + id).addClass('inputContenido');
}

function renderizarTabs() {
    $('.tabs').tabs();
}



/***************************/
/******* CALENDARIO ********/
/***************************/
var calendar = null;
var flagcal = true;

function mostrarCalendario(idtext) {
    return showCalendar(idtext, 'dd/mm/y');
}

function closeHandler(cal) {
    cal.hide();
    Calendar.removeEvent(document, "mousedown", checkCalendar);
}

function showCalendar(id, format) {
    var el = document.getElementById(id);

    if (calendar != null) {
        calendar.hide();                 // so we hide it first.
    }
    else {
        var cal = new Calendar(false, null, selected, closeHandler);

        calendar = cal;                  // remember it in the global var
        cal.setRange(1900, 2070);        // min/max year allowed.
        cal.create();
    }

    calendar.setDateFormat(format);    // set the specified date format
    calendar.parseDate(el.value);      // try to parse the text in field
    calendar.sel = el;                 // inform it what input field we use

    calendar.showAtElement(el);        // show the calendar below it

    // catch "mousedown" on document
    Calendar.addEvent(document, "mousedown", checkCalendar);
    Calendar.addEvent(document, "mouseover", checkCalendar);

    return false;
}

function checkCalendar(ev) {
    var el = Calendar.is_ie ? Calendar.getElement(ev) : Calendar.getTargetElement(ev);
    for (; el != null; el = el.parentNode)
        if (el == calendar.element || el.tagName == "A") break;

    if (el == null) {
        calendar.callCloseHandler();
        Calendar.stopEvent(ev);
    }
}

function selected(cal, date) {
    if (cal.sel.value == date) {
        cal.sel.value = date;
        cal.callCloseHandler();
    }
    else {
        cal.sel.value = date;
        cal.callCloseHandler();
        //Esto lo pongo asi para que tome el onChange del Textbox (Fernando)
        $(cal.sel).blur();

        if (flagcal) {
            //__doPostBack(cal.sel.id)
        }
    }
}


/******************************************************************/
/******* PONE EN MAYUSCULA LA PRMIER LETRA DE CADA PALABRA ********/
/******************************************************************/
function ucWords(string) {
    var arrayWords;
    var returnString = "";
    var len;
    arrayWords = string.split(" ");
    len = arrayWords.length;
    for (i = 0; i < len; i++) {
        if (i != (len - 1)) {
            returnString = returnString + ucFirst(arrayWords[i]) + " ";
        }
        else {
            returnString = returnString + ucFirst(arrayWords[i]);
        }
    }
    return returnString;
}
/**************************************************/
/******* PONE EN MAYUSCULA LA PRIMER LETRA ********/
/**************************************************/
function ucFirst(string) {
    return string.substr(0, 1).toUpperCase() + string.substr(1, string.length).toLowerCase();
}