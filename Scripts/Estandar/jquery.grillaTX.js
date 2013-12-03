(function ($) {
    var methods = {

        _renderGrilla: function () {

            //            <div class="itemFiltro" style="position:relative;">
            //				<span class="titulo">Caja de Textarea con limite de caracteres</span><br>
            //				<textarea class="txtStd contabilizarTexto" style="width:965px;" rows="3" id="descripcion2" maxText="50">Aca va un texto</textarea>
            //                <div id="divBarraFondo" class="divBarraProgreso" txtArea="descripcion2" style="position:absolute; top:0px; right:10px; width:200px; height:15px; display:none;">
            //                    <div id="divBarraProgreso" style="width: 60%; height:15px; padding-top:0px; line-height:14px;">
            //                        <span id="porcentaje">60</span>%
            //                    </div>
            //                </div>
            //			</div>

            var classConContenido = "";
            if (properties.textoDefault != "") {
                classConContenido = "inputContenido"
            }

            properties.divContainer.addClass('itemFiltro').css('position', 'relative').removeAttr('id');
            properties.divContainer.html('<span class="titulo">' + properties.titulo + '</span><br>');
            properties.divContainer.append('<textarea class="txtStd contabilizarTexto ' + classConContenido + '" style="width:' + properties.widthTextarea + '; height:' + properties.heightTextarea + ';" id="' + properties.idTextarea + '" maxText="' + properties.limiteCaracteres + '" contenidoactual="' + properties.textoDefault + '">' + properties.textoDefault + '</textarea>');

            var porcentaje = methods._contabilizarTexto();
            properties.divContainer.append('<div id="divBarraFondo" class="divBarraProgreso" txtArea="' + properties.idTextarea + '" style="position:absolute; top:0px; right:10px; width:200px; height:15px; display:none;">' +
                                                '<div id="divBarraProgreso" style="width:' + porcentaje + '%; height:15px; padding-top:0px; line-height:14px;">' +
                                                    '<span id="porcentaje">' + porcentaje + '</span>%' +
                                                '</div>' +
                                            '</div>');
        }


		, init: function (options, fn) {
		    var settings = {
		        'titulo': '',
		        //'idTextarea': 'descripcion21',
		        'widthTextarea': '800px',
		        'heightTextarea': '50px',
		        'textoDefault': '',
		        'limiteCaracteres': 50
		    };

		    return this.each(function () {

		        if (options) {
		            settings = $.extend(settings, options);
		        }
		        properties.titulo = settings.titulo;
		        properties.idTextarea = $(this).attr('id');
		        properties.widthTextarea = settings.widthTextarea;
		        properties.heightTextarea = settings.heightTextarea;
		        properties.textoDefault = settings.textoDefault;
		        properties.limiteCaracteres = settings.limiteCaracteres;
		        properties.divContainer = $(this);

		        methods._renderGrilla();

		    });
		} //Fin INIT


    };

    var properties = {
        titulo: '',
        idTextarea: '',
        widthTextarea: '',
        heightTextarea: '',
        textoDefault: '',
        limiteCaracteres: 100,
        divContainer: null
    }

    $.fn.limitTextarea = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('El metodo ' + method + ' no existe en el plugin jquery.limitTextarea');
        }
    };

})(jQuery); 