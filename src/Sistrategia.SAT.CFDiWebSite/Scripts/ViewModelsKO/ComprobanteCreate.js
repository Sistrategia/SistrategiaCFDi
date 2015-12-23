$(document).ready(function () {

   // #region Select2 JS
    $.fn.select2.defaults.set('language', 'es');

    $("#EmisorId").select2({
        placeholder: "Seleccione",
        minimumInputLength: 2,
        allowClear: false,
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
        allowClear: false,
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
    }    var ConceptosModel = function (modelId) {
        var self = this;
    }
    var ImpuestosModel = function (modelId) {
        var self = this;
    }
});