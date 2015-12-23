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
}var Concepto = function () {
    this.ConceptoCantidad = null;
    this.ConceptoUnidad = null;
    this.ConceptoNoIdentificacion = null;
    this.ConceptoDescripcion = null;
    this.ConceptoValorUnitario = null;
    this.ConceptoImporte = null;
    this.ConceptoOrdinal = null;
}var ConceptosModel = function (modelId) {
    var self = this;
    self.Cantidad = ko.observable("");
    self.Unidad = ko.observable("");
    self.NoIdentificacion = ko.observable("");
    self.Descripcion = ko.observable("");
    self.ValorUnitario = ko.observable("");
    self.Importe = ko.observable("");

    self.Ordinal = ko.observable(0);
    self.Items = ko.observableArray([]);    self.HasItems = ko.observable(false);
    self.addItem = function () {
        if (self.Cantidad() != "" && self.Unidad() != "" && self.NoIdentificacion() != "" && self.Descripcion() != "" && self.ValorUnitario() != "") {
            self.Ordinal(self.Ordinal() + 1);
            self.Importe(self.Cantidad() * self.ValorUnitario());

            var concepto = new Concepto();            
            concepto.ConceptoCantidad = ko.observable(self.Cantidad());
            concepto.ConceptoUnidad = ko.observable(self.Unidad());
            concepto.ConceptoNoIdentificacion = ko.observable(self.NoIdentificacion());
            concepto.ConceptoDescripcion = ko.observable(self.Descripcion());
            concepto.ConceptoValorUnitario = ko.observable(self.ValorUnitario());
            concepto.ConceptoImporte = ko.observable(self.Importe());
            concepto.ConceptoOrdinal = ko.observable(self.Ordinal());

            self.Items.push(concepto);
            
            self.Cantidad("");
            self.Unidad("");
            self.NoIdentificacion("");
            self.Descripcion("");
            self.ValorUnitario("");
            self.Importe("");
        }
    };

    self.removeSelected = function (data) {
        self.Items.remove(data);
        if (self.Items().length <= 0)
            self.Ordinal(0);        
        else
            self.Ordinal(self.Items().length);

        for (var i = data.ConceptoOrdinal() - 1; i < self.Items().length; i++)
            self.Items()[i].ConceptoOrdinal(i + 1);
    };

    self.Items.subscribe(function (newValue) {
        self.HasItems(newValue && newValue.length ? true : false);
    });

}

var Trasladado = function () {
    this.TrasladadoImpuesto = null;
    this.TrasladadoTasa = null;
    this.TrasladadoImporte = null;
    this.TrasladadoOrdinal = null;
}

var Retencion = function () {
    this.RetencionImpuesto = null;
    this.RetencionImporte = null;
    this.RetencionOrdinal = null;
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

    self.TrasladosOrdinal = ko.observable(0);
    self.TrasladosItems = ko.observableArray([]);    self.TrasladosHasItems = ko.observable(false);
   
    self.RetencionesOrdinal = ko.observable(0);
    self.RetencionesItems = ko.observableArray([]);    self.RetencionesHasItems = ko.observable(false);

    self.addTraslado = function () {
        if (self.TrasladosImpuesto() != "" && self.TrasladosImporte() != "" && self.TrasladosTasa() != "") {
            self.TrasladosOrdinal(self.TrasladosOrdinal() + 1);
            var trasladado = new Trasladado();
            trasladado.TrasladadoImpuesto = ko.observable(self.TrasladosImpuesto());
            trasladado.TrasladadoTasa = ko.observable(self.TrasladosImporte());
            trasladado.TrasladadoImporte = ko.observable(self.TrasladosTasa());
            trasladado.TrasladadoOrdinal = ko.observable(self.TrasladosOrdinal());
            self.TrasladosItems.push(trasladado);

            self.TrasladosImpuesto("");
            self.TrasladosImporte("");
            self.TrasladosTasa("");
        }
    };

    self.removeTraslado = function (data) {
        self.TrasladosItems.remove(data);
        if (self.TrasladosItems().length <= 0)
            self.TrasladosOrdinal(0);
        else
            self.TrasladosOrdinal(self.TrasladosItems().length);

        for (var i = data.TrasladadoOrdinal() - 1; i < self.TrasladosItems().length; i++)
            self.TrasladosItems()[i].TrasladadoOrdinal(i + 1);
    };

    self.TrasladosItems.subscribe(function (newValue) {
        self.TrasladosHasItems(newValue && newValue.length ? true : false);
    });

    self.addRetencion = function () {
        if (self.RetencionesImpuesto() != "" && self.RetencionesImporte() != "") {
            self.RetencionesOrdinal(self.RetencionesOrdinal() + 1);
            var retencion = new Retencion();
            retencion.RetencionImpuesto = ko.observable(self.RetencionesImpuesto());
            retencion.RetencionImporte = ko.observable(self.RetencionesImporte());
            retencion.RetencionOrdinal = ko.observable(self.RetencionesOrdinal());
            self.RetencionesItems.push(retencion);

            self.RetencionesImpuesto("");
            self.RetencionesImporte("");
        }
    };

    self.removeRetencion = function (data) {
        self.RetencionesItems.remove(data);
        if (self.RetencionesItems().length <= 0)
            self.RetencionesOrdinal(0);
        else
            self.RetencionesOrdinal(self.RetencionesItems().length);

        for (var i = data.RetencionOrdinal() - 1; i < self.RetencionesItems().length; i++)
            self.RetencionesItems()[i].RetencionOrdinal(i + 1);
    };

    self.RetencionesItems.subscribe(function (newValue) {
        self.RetencionesHasItems(newValue && newValue.length ? true : false);
    });
}

