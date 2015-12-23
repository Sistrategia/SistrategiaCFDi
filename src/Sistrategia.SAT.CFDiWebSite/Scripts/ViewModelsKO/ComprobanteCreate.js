$(document).ready(function () {

   // #region Select2 JS
    $.fn.select2.defaults.set('language', 'es');

    $("#EmisorId").select2({
        placeholder: "Seleccione",
        minimumInputLength: 2,
        allowClear: true,
        ajax: {
            url: "/es-ES/Comprobante/GetIdByEmisores",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            delay: 250,
            data: function (params) {
                return {
                    value: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
    });

    $("#ReceptorId").select2({
        placeholder: "Seleccione",
        minimumInputLength: 2,
        allowClear: true,
        ajax: {
            url: "/es-ES/Comprobante/GetIdByReceptores",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            delay: 250,
            data: function (params) {
                return {
                    value: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data
                };
            },
            cache: true
        },
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
    });
   // #endregion Select2 JS

    //http://craigcav.wordpress.com/2012/05/16/simple-client-storage-for-view-models-with-amplifyjs-and-knockout/
    (function (ko) {
        ko.extenders.sessionStore = function (target, key) {
            var keyword = key;
            var value = amplify.store.sessionStorage(keyword) || target();
            var result = ko.computed({
                read: target,
                write: function (newValue) {
                    amplify.store.sessionStorage(keyword, newValue);
                    target(newValue);
                }
            });
            result(value);
            return result;
        };
    })(ko);
    var InitialModel = function (modelId) {
        var self = this;
        self.EmisorId = ko.observable("");
        self.ReceptorId = ko.observable("");
        self.Serie = ko.observable("");
        self.Folio = ko.observable("");
        self.FormaDePago = ko.observable("");
        self.MetodoDePago = ko.observable("");
        self.LugarExpedicion = ko.observable("");
        self.NumCtaPago = ko.observable("");
        self.CertificadoId = ko.observable("");
    }    var ConceptosModel = function (modelId) {
        var self = this;
        self.Cantidad = ko.observable("");
        self.Unidad = ko.observable("");
        self.NoIdentificacion = ko.observable("");
        self.Descripcion = ko.observable("");
        self.ValorUnitario = ko.observable("");
        self.Importe = ko.observable("");
    }
    var ImpuestosModel = function (modelId) {
        var self = this;
        self.TrasladosImpuesto = ko.observable("");
        self.TrasladosImporte = ko.observable("");
        self.TrasladosTasa = ko.observable("");

        self.RetencionesImpuesto = ko.observable("");
        self.RetencionesImporte = ko.observable("");

        self.SubTotal = ko.observable(0);
        self.TotalTraslados = ko.observable(0);
        self.TotalRetenciones = ko.observable(0);
        self.Total = ko.observable(0);
    }

    var initial_model = new InitialModel('InitialModel');
    var conceptos_model = new ConceptosModel('ConceptosModel');
    var impuestos_model = new ImpuestosModel('ImpuestosModel');

    ko.applyBindings(initial_model, document.getElementById("initial_model_id"));
    ko.applyBindings(conceptos_model, document.getElementById("conceptos_model_id"));
    ko.applyBindings(impuestos_model, document.getElementById("impuestos_model_id"));

});