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
    self.UsoCFDI = ko.observable("");
    self.EmisorId = ko.observable("");
    self.ReceptorId = ko.observable("");
    self.Serie = ko.observable("");
    self.Folio = ko.observable("");
    self.FormaDePago = ko.observable("");
    self.MetodoDePagoId = ko.observable("");
    self.LugarExpedicion = ko.observable("");
    self.NumCtaPago = ko.observable("");
    self.CertificadoId = ko.observable("");
    self.TipoDeComprobante = ko.observable("");

    self.Fecha = ko.observable("");
    self.TipoCambio = ko.observable("");
    self.Moneda = ko.observable("");
    self.Banco = ko.observable("");

    self.Nota = ko.observable("");
    self.CteNumero = ko.observable("");
    self.OrdenNumero = ko.observable("");
    self.TemplateId = ko.observable("");

    self.IsRequiredNumCtaPago = ko.observable(false);
}

var Concepto = function () {
    this.ConceptoCantidad = null;
    this.ConceptoClaveProdServ = null;
    this.ConceptoUnidad = null;
    this.ConceptoClaveUnidad = null;
    this.ConceptoNoIdentificacion = null;
    this.ConceptoDescripcion = null;
    this.ConceptoValorUnitario = null;
    this.ConceptoImporte = null;
    this.ConceptoDescuento = null;
    this.ConceptoOrdinal = null;

    this.ConceptoImpuestoTipo = null;
    this.ConceptoImpuestoBase = null;
    this.ConceptoImpuestoImpuesto = null;
    this.ConceptoImpuestoTipoFactor = null;
    this.ConceptoImpuestoTasaOCuota = null;
    this.ConceptoImpuestoImporte = null;
    this.ConceptoImpuestoOrdinal = null;
}

var ConceptosModel = function (modelId) {
    var self = this;
    self.Cantidad = ko.observable("");
    self.ClaveProdServ = ko.observable("");
    self.Unidad = ko.observable("");
    self.ClaveUnidad = ko.observable("");
    self.NoIdentificacion = ko.observable("");
    self.Descripcion = ko.observable("");
    self.ValorUnitario = ko.observable("");
    self.Descuento = ko.observable(""); 
    self.Importe = ko.observable("");

    self.ImpuestoTipo = ko.observable("");
    self.ImpuestoBase = ko.observable("");
    self.ImpuestoImpuesto = ko.observable("");
    self.ImpuestoTipoFactor = ko.observable("");
    self.ImpuestoTasaOCuota = ko.observable("");
    self.ImpuestoImporte = ko.observable("");
    self.ImpuestoOrdinal = ko.observable("");

    self.Ordinal = ko.observable(0);
    self.Items = ko.observableArray([]);
    self.HasItems = ko.observable(false);

    self.addItem = function () {
        if (self.Cantidad() != ""
            && self.ClaveProdServ() != ""
            && self.ClaveUnidad() != ""
            && self.NoIdentificacion() != ""
            && self.Descripcion() != ""
            && self.ValorUnitario() != "") {

            self.Ordinal(self.Ordinal() + 1);
            self.ImpuestoOrdinal(self.Ordinal() + 1);
            
            var sumaImporteConIVA = ((Math.round(parseFloat(self.Cantidad()) * 100) / 100) * (Math.round(parseFloat(self.ValorUnitario()) * 100) / 100));
            var sumaImporteSinIVA = ((parseFloat(sumaImporteConIVA.toFixed(2) / 1.160000).toFixed(2) * 100 ) / 100);
            if (self.Importe() == "") {
                if (self.ImpuestoTasaOCuota() == 0.160000)
                    self.Importe(sumaImporteSinIVA);
                else
                    self.Importe(sumaImporteConIVA);
            }

            var concepto = new Concepto();            
            concepto.ConceptoCantidad = ko.observable(self.Cantidad());
            concepto.ConceptoClaveProdServ = ko.observable(self.ClaveProdServ());
            concepto.ConceptoUnidad = ko.observable(self.Unidad());
            concepto.ConceptoClaveUnidad = ko.observable(self.ClaveUnidad());
            concepto.ConceptoNoIdentificacion = ko.observable(self.NoIdentificacion());
            concepto.ConceptoDescripcion = ko.observable(self.Descripcion());

            if (self.ImpuestoTasaOCuota() == 0.160000) {
                concepto.ConceptoValorUnitario = ko.observable(((parseFloat(self.ValorUnitario()).toFixed(2) / 1.160000).toFixed(2) * 100) / 100);
            }
            else {
                concepto.ConceptoValorUnitario = ko.observable((parseFloat(self.ValorUnitario()).toFixed(2) * 100) / 100);
            }
            concepto.ConceptoImporte = ko.observable(self.Importe());
            concepto.ConceptoOrdinal = ko.observable(self.Ordinal());

            concepto.ConceptoImpuestoTipo = ko.observable("traslado");
            concepto.ConceptoImpuestoBase = ko.observable(self.Importe());
            concepto.ConceptoImpuestoImpuesto = ko.observable(self.ImpuestoImpuesto());
            concepto.ConceptoImpuestoTipoFactor = ko.observable(self.ImpuestoTipoFactor());
            concepto.ConceptoImpuestoTasaOCuota = ko.observable(self.ImpuestoTasaOCuota());

            if (self.ImpuestoTasaOCuota() == 0.160000) {
                if (self.ImpuestoImporte() == "") {
                    concepto.ConceptoImpuestoImporte = ko.observable((parseFloat(sumaImporteConIVA - sumaImporteSinIVA).toFixed(2) * 100) / 100);
                }
                else {
                    concepto.ConceptoImpuestoImporte = ko.observable(self.ImpuestoImporte().toFixed(2));
                }
            }
            else {
                concepto.ConceptoImpuestoImporte = ko.observable(0);
            }
            concepto.ConceptoImpuestoOrdinal = ko.observable(self.Ordinal());


            self.Items.push(concepto);
            
            //self.Cantidad("");
            self.ClaveProdServ("");
            self.NoIdentificacion("");
            self.Descripcion("");
            self.ValorUnitario("");
            self.Importe("");

            self.ImpuestoBase("");
            self.ImpuestoTipo("");
            self.ImpuestoBase("");
            self.ImpuestoImpuesto("");
            self.ImpuestoTipoFactor("");
            self.ImpuestoImporte("");

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
    this.TrasladadoTipoFactor = null;
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
    self.TrasladadoTipoFactor = ko.observable("");
    self.TrasladosImporte = ko.observable("");
    self.TrasladosTasa = ko.observable("");

    self.RetencionesImpuesto = ko.observable("");
    self.RetencionesImporte = ko.observable("");

    self.SubTotal = ko.observable(0);
    self.TotalTraslados = ko.observable(0);
    self.TotalRetenciones = ko.observable(0);
    self.Total = ko.observable(0);

    self.TrasladosOrdinal = ko.observable(0);
    self.TrasladosItems = ko.observableArray([]);
    self.TrasladosHasItems = ko.observable(false);
   
    self.RetencionesOrdinal = ko.observable(0);
    self.RetencionesItems = ko.observableArray([]);
    self.RetencionesHasItems = ko.observable(false);

    self.addTraslado = function () {
        if (self.TrasladosImpuesto() != "" && self.TrasladosImporte() != "" && self.TrasladosTasa() != "") {
            self.TrasladosOrdinal(self.TrasladosOrdinal() + 1);
            var trasladado = new Trasladado();
            trasladado.TrasladadoImpuesto = ko.observable(self.TrasladosImpuesto());
            trasladado.TrasladadoTipoFactor = ko.observable(self.TrasladadoTipoFactor());
            trasladado.TrasladadoTasa = ko.observable(self.TrasladosTasa());
            trasladado.TrasladadoImporte = ko.observable(self.TrasladosImporte());
            trasladado.TrasladadoOrdinal = ko.observable(self.TrasladosOrdinal());
            self.TrasladosItems.push(trasladado);
            
            (self.TrasladosTasa() == 0) ? self.TrasladosImporte("0.00") : self.TrasladosImporte("");
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

    self.TrasladosTasa.subscribe(function () {
        if (self.TrasladosTasa() == 0)
            self.TrasladosImporte("0.00");
        else
            self.TrasladosImporte("");
    });
}

