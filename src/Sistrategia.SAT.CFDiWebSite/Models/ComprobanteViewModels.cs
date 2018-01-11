using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
//using Microsoft.Owin.Security;
using Sistrategia.SAT.Resources;
using Sistrategia.SAT.CFDiWebSite.CFDI;
using System.Web.Mvc;
using System.Configuration;
using System.Web;
using System.Linq;

namespace Sistrategia.SAT.CFDiWebSite.Models
{

    #region ComprobanteIndexViewModel
    public class ComprobanteIndexViewModel
    {
        public ComprobanteIndexViewModel() {
            this.Comprobantes = new List<Comprobante>();
        }
        public List<Comprobante> Comprobantes { get; set; }
    }
    #endregion

    #region ComprobanteCreateViewModel
    public class ComprobanteCreateViewModel
    {
        public ComprobanteCreateViewModel() {
            this.Emisor = new EmisorDetailViewModel();
            this.ExpedidoEn = new UbicacionViewModel();
            this.Receptor = new ReceptorDetailsViewModel();
            this.Conceptos = new List<ConceptoViewModel>();
            this.Retenciones = new List<RetencionViewModel>();
            this.Traslados = new List<TrasladoViewModel>();
        }

        public string UsoCFDI { get; set; }
        public string Serie { get; set; }
        public string Folio { get; set; }

        public decimal SubTotal { get; set; }

        //public decimal IVA { get; set; }
        //public decimal TasaIVA { get; set; }
        //public decimal ISR { get; set; }

        public decimal? TotalImpuestosRetenidos { get; set; }
        public decimal? TotalImpuestosTrasladados { get; set; }

        public decimal Total { get; set; }

        public string FormaDePago { get; set; }
        public string FormaPago { get; set; }
        public string MetodoPago { get; set; }
        //public string MetodoDePago { get; set; }
        public string NumCtaPago { get; set; }
        public string LugarExpedicion { get; set; }
        public string TipoCambio { get; set; }
        public string Moneda { get; set; }
        public string Banco { get; set; }

        public EmisorDetailViewModel Emisor { get; set; }
        public UbicacionViewModel ExpedidoEn { get; set; }
        public ReceptorDetailsViewModel Receptor { get; set; }


        //public List<Receptor> Receptores { get; set; }
        public IEnumerable<SelectListItem> UsoCFDIList { get; set; }
        public IEnumerable<SelectListItem> Emisores { get; set; }
        public IEnumerable<SelectListItem> Receptores { get; set; }
        public IEnumerable<SelectListItem> Certificados { get; set; }
        public IEnumerable<SelectListItem> TipoMetodoDePago { get; set; }
        public IEnumerable<SelectListItem> TiposImpuestoRetencion { get; set; }
        public IEnumerable<SelectListItem> TiposImpuestoTraslado { get; set; }
        public IEnumerable<SelectListItem> TiposFormaDePago { get; set; }
        public IEnumerable<SelectListItem> TiposMoneda { get; set; }
        public IEnumerable<SelectListItem> TiposDeComprobante { get; set; }
        public IEnumerable<SelectListItem> Bancos { get; set; }

        public IEnumerable<SelectListItem> ViewTemplates { get; set; }

        public List<ConceptoViewModel> Conceptos { get; set; }
        public List<TrasladoViewModel> Traslados { get; set; }
        public List<RetencionViewModel> Retenciones { get; set; }

        public int? EmisorId { get; set; }
        public int? ReceptorId { get; set; }
        public int? CertificadoId { get; set; }
        public int? MetodoDePagoId { get; set; }

        public string TipoDeComprobante { get; set; }

        public int? CteNumero { get; set; }
        public int? OrdenNumero { get; set; }
        public int? TemplateId { get; set; }

        public string Notas { get; set; }
    }
    #endregion

    #region ComprobanteEditViewModel
    public class ComprobanteEditViewModel
    {
        public ComprobanteEditViewModel() {
            this.Emisor = new EmisorDetailViewModel();
            this.ExpedidoEn = new UbicacionViewModel();
            this.Receptor = new ReceptorDetailsViewModel();
            this.Conceptos = new List<ConceptoViewModel>();
            this.Retenciones = new List<RetencionViewModel>();
            this.Traslados = new List<TrasladoViewModel>();
        }

        public ComprobanteEditViewModel(Comprobante comprobante) {
            if (comprobante == null)
                throw new ArgumentNullException("comprobante");

            if (comprobante.Emisor != null) {
                this.Emisor = new EmisorDetailViewModel(comprobante.Emisor.Emisor);
            }

            if (comprobante.Receptor != null) {
                this.Receptor = new ReceptorDetailsViewModel(comprobante.Receptor.Receptor);
            }

            if (comprobante.Conceptos != null && comprobante.Conceptos.Count > 0) {
                this.Conceptos = new List<ConceptoViewModel>();
                foreach (Concepto concepto in comprobante.Conceptos) {
                    this.Conceptos.Add(new ConceptoViewModel(concepto));
                }
            }

            if (comprobante.Impuestos.Traslados != null && comprobante.Impuestos.Traslados.Count > 0) {
                this.Traslados = new List<TrasladoViewModel>();
                foreach (Traslado traslado in comprobante.Impuestos.Traslados) {
                    this.Traslados.Add(new TrasladoViewModel(traslado));
                }
            }

            if (comprobante.Impuestos.Retenciones != null && comprobante.Impuestos.Retenciones.Count > 0) {
                this.Retenciones = new List<RetencionViewModel>();
                foreach (Retencion retencion in comprobante.Impuestos.Retenciones) {
                    this.Retenciones.Add(new RetencionViewModel(retencion));
                }
            }

            this.ComprobanteId = comprobante.ComprobanteId;
            this.EmisorId = comprobante.ComprobanteEmisorId;
            //this.DomicilioFiscalId = comprobante.Emisor.DomicilioFiscalId;
            //this.ExpedidoEnId = comprobante.Emisor.DomicilioFiscalId;
            this.CertificadoId = comprobante.CertificadoId;
            this.ReceptorId = comprobante.ComprobanteReceptorId;

            this.MetodoDePago = comprobante.MetodoDePago;
            this.LugarExpedicion = comprobante.LugarExpedicion;
            this.FormaDePago = comprobante.FormaDePago;

            this.UsoCFDI = comprobante.Receptor.UsoCFDI;
            this.Serie = comprobante.Serie;
            this.Folio = comprobante.Folio;
            this.Fecha = comprobante.Fecha.ToString("yyyy-MM-ddTHH:mm:ss");

            this.TipoCambio = comprobante.TipoCambio;
            this.Moneda = comprobante.Moneda;

            this.Banco = comprobante.ExtendedStringValue1;

            this.SubTotal = comprobante.SubTotal;
            this.Total = comprobante.Total;

            this.TotalImpuestosRetenidos = comprobante.Impuestos.TotalImpuestosRetenidos;
            this.TotalImpuestosTrasladados = comprobante.Impuestos.TotalImpuestosTrasladados;

            this.Notas = comprobante.ExtendedStringValue2;

            this.CadenaOriginal = comprobante.GetCadenaOriginal();
            this.Sello = comprobante.Sello;
            Sistrategia.SAT.CFDiWebSite.Data.ApplicationDbContext DBContext = new Sistrategia.SAT.CFDiWebSite.Data.ApplicationDbContext();
            var certificado = DBContext.Certificados.Where(e => e.NumSerie == comprobante.NoCertificado).SingleOrDefault();
            this.GetSello = certificado.GetSello(this.CadenaOriginal);
        }

        public int? ComprobanteId { get; set; }
        public string UsoCFDI { get; set; }
        public string Serie { get; set; }
        public string Folio { get; set; }
        //public DateTime Fecha { get; set; }
        public string Fecha { get; set; }

        public decimal SubTotal { get; set; }

        //public decimal IVA { get; set; }
        //public decimal TasaIVA { get; set; }
        //public decimal ISR { get; set; }

        public decimal? TotalImpuestosRetenidos { get; set; }
        public decimal? TotalImpuestosTrasladados { get; set; }

        public decimal Total { get; set; }

        public string CadenaOriginal { get; set; }
        public string Sello { get; set; }
        public string GetSello { get; set; }

        public string FormaDePago { get; set; }
        public string MetodoDePago { get; set; }
        public string NumCtaPago { get; set; }
        public string LugarExpedicion { get; set; }
        public string TipoCambio { get; set; }
        public string Moneda { get; set; }
        public string Banco { get; set; }

        public EmisorDetailViewModel Emisor { get; set; }
        public UbicacionViewModel ExpedidoEn { get; set; }
        public ReceptorDetailsViewModel Receptor { get; set; }


        //public List<Receptor> Receptores { get; set; }
        public IEnumerable<SelectListItem> Emisores { get; set; }
        public IEnumerable<SelectListItem> Receptores { get; set; }
        public IEnumerable<SelectListItem> Certificados { get; set; }
        public IEnumerable<SelectListItem> TipoMetodoDePago { get; set; }
        public IEnumerable<SelectListItem> TiposImpuestoRetencion { get; set; }
        public IEnumerable<SelectListItem> TiposImpuestoTraslado { get; set; }
        public IEnumerable<SelectListItem> TiposFormaDePago { get; set; }
        public IEnumerable<SelectListItem> TiposMoneda { get; set; }
        public IEnumerable<SelectListItem> Bancos { get; set; }

        public List<ConceptoViewModel> Conceptos { get; set; }
        public List<TrasladoViewModel> Traslados { get; set; }
        public List<RetencionViewModel> Retenciones { get; set; }

        public int? EmisorId { get; set; }
        public int? ReceptorId { get; set; }
        public int? CertificadoId { get; set; }



        public string Notas { get; set; }
    }
    #endregion

    #region ConceptoViewModel
    public class ConceptoViewModel
    {
        public ConceptoViewModel() {
            this.Unidad = "pza";
        }

        public ConceptoViewModel(Concepto concepto) {
            this.Cantidad = concepto.Cantidad;
            this.ClaveProdServ = concepto.ClaveProdServ;
            this.Unidad = concepto.Unidad;
            this.ClaveUnidad = concepto.ClaveUnidad;
            this.NoIdentificacion = concepto.NoIdentificacion;
            this.Descripcion = concepto.Descripcion;
            this.ValorUnitario = concepto.ValorUnitario;
            this.Importe = concepto.Importe;
            this.Descuento = concepto.Descuento;
            this.Ordinal = concepto.Ordinal;

            if (concepto.Impuestos != null && concepto.Impuestos.Traslados.Count > 0)
            {
                this.ImpuestoTipo = "traslado";
                this.ImpuestoBase = concepto.Impuestos.Traslados[concepto.Ordinal-1].Base;
                this.ImpuestoImpuesto = concepto.Impuestos.Traslados[concepto.Ordinal - 1].Impuesto;
                this.ImpuestoTipoFactor = concepto.Impuestos.Traslados[concepto.Ordinal - 1].TipoFactor;
                this.ImpuestoTasaOCuota = concepto.Impuestos.Traslados[concepto.Ordinal - 1].TasaOCuota;
                this.ImpuestoImporte = concepto.Impuestos.Traslados[concepto.Ordinal - 1].Importe;
                this.ImpuestoOrdinal = concepto.Impuestos.Traslados[concepto.Ordinal - 1].Ordinal;
            }

            if (concepto.Impuestos != null && concepto.Impuestos.Retenciones.Count > 0)
            {
                this.ImpuestoTipo = "retencion";
                this.ImpuestoBase = concepto.Impuestos.Traslados[concepto.Ordinal - 1].Base;
                this.ImpuestoImpuesto = concepto.Impuestos.Traslados[concepto.Ordinal - 1].Impuesto;
                this.ImpuestoTipoFactor = concepto.Impuestos.Traslados[concepto.Ordinal - 1].TipoFactor;
                this.ImpuestoTasaOCuota = concepto.Impuestos.Traslados[concepto.Ordinal - 1].TasaOCuota;
                this.ImpuestoImporte = concepto.Impuestos.Traslados[concepto.Ordinal - 1].Importe;
                this.ImpuestoOrdinal = concepto.Impuestos.Traslados[concepto.Ordinal - 1].Ordinal;
            }
        }

        public decimal Cantidad { get; set; }
        public string ClaveProdServ { get; set; }
        public string Unidad { get; set; }
        public string ClaveUnidad { get; set; }
        public string NoIdentificacion { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Importe { get; set; }
        public decimal? Descuento { get; set; }
        public int Ordinal { get; set; }

        public string ImpuestoTipo { get; set; }
        public decimal ImpuestoBase { get; set; }
        public string ImpuestoImpuesto { get; set; }
        public string ImpuestoTipoFactor { get; set; }
        public decimal? ImpuestoTasaOCuota { get; set; }
        public decimal? ImpuestoImporte { get; set; }
        public int? ImpuestoOrdinal { get; set; }

    }
    #endregion

    #region TrasladoViewModel
    public class TrasladoViewModel
    {

        public TrasladoViewModel() {
        }

        public TrasladoViewModel(Traslado traslado) {
            this.Importe = traslado.Importe;
            this.TipoFactor = traslado.TipoFactor;
            this.Impuesto = traslado.Impuesto;
            if (traslado.Tasa.HasValue)
                this.Tasa = traslado.Tasa.Value;
        }

        public string TipoFactor { get; set; }
        public decimal Importe { get; set; }
        public string Impuesto { get; set; }
        public decimal Tasa { get; set; }
    }
    #endregion

    #region RetencionViewModel
    public class RetencionViewModel
    {
        public RetencionViewModel() {
        }

        public RetencionViewModel(Retencion retencion) {
            this.Importe = retencion.Importe;
            this.Impuesto = retencion.Impuesto;
        }

        public decimal Importe { get; set; }
        public string Impuesto { get; set; }
    }
    #endregion

    #region ComprobanteDetailViewModel
    public class ComprobanteDetailViewModel
    {
        public ComprobanteDetailViewModel(Comprobante comprobante) {
            if (comprobante == null)
                throw new ArgumentNullException("comprobante");

            this.PublicKey = comprobante.PublicKey;

            if (comprobante.Emisor != null) {
                this.Emisor = new ComprobanteEmisorDetailViewModel(comprobante.Emisor);
            }

            if (comprobante.Receptor != null) {
                this.Receptor = new ComprobanteReceptorDetailsViewModel(comprobante.Receptor);
            }

            if (comprobante.Conceptos != null && comprobante.Conceptos.Count > 0) {
                this.Conceptos = new List<ConceptoViewModel>();
                foreach (Concepto concepto in comprobante.Conceptos) {
                    this.Conceptos.Add(new ConceptoViewModel(concepto));
                }
            }

            this.Serie = comprobante.Serie;
            this.Folio = comprobante.Folio;

            this.SubTotal = comprobante.SubTotal;
            this.Total = comprobante.Total;

            this.CadenaOriginal = comprobante.GeneratedCadenaOriginal; // comprobante.GetCadenaOriginal();
            //this.Sello = comprobante.Sello;
            this.Sello = comprobante.Sello;
            //this.Sello = comprobante.Sello;

            this.GeneratedXmlUrl = comprobante.GeneratedXmlUrl;
            this.GeneratedPDFUrl = comprobante.GeneratedPDFUrl;

            this.TimbreFiscalDigital = null;

            this.Status = comprobante.Status;

            if (comprobante.Complementos != null && comprobante.Complementos.Count > 0) {
                foreach (var complemento in comprobante.Complementos) {
                    if (complemento is TimbreFiscalDigital) {
                        this.TimbreFiscalDigital = complemento as TimbreFiscalDigital;
                    }
                }
            }
        }

        public string Title {
            get {
                return string.Format("{0}{1} - {2}", this.Serie, this.Folio, this.Receptor.Nombre);
            }
        }

        public Guid PublicKey { get; set; }

        public string Serie { get; set; }
        public string Folio { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public string CadenaOriginal { get; set; }
        public string Sello { get; set; }

        public ComprobanteEmisorDetailViewModel Emisor { get; set; }
        public ComprobanteReceptorDetailsViewModel Receptor { get; set; }

        public List<ConceptoViewModel> Conceptos { get; set; }

        public string GeneratedXmlUrl { get; set; }
        public string GeneratedPDFUrl { get; set; }

        public TimbreFiscalDigital TimbreFiscalDigital { get; set; }

        public string Status { get; set; }
    }
    #endregion

    #region ComprobanteEmisorDetailViewModel
    public class ComprobanteEmisorDetailViewModel
    {
        public ComprobanteEmisorDetailViewModel() {

        }

        public ComprobanteEmisorDetailViewModel(ComprobanteEmisor emisor) {
            if (emisor == null)
                throw new ArgumentNullException("emisor");

            this.PublicKey = emisor.PublicKey;

            this.RFC = emisor.RFC;
            this.Nombre = emisor.Nombre;
            if (emisor.RegimenFiscal != null && emisor.RegimenFiscal.Count > 0)
                this.RegimenFiscal = emisor.RegimenFiscal[0].Regimen;

            if (emisor.DomicilioFiscal != null) {
                this.Calle = emisor.DomicilioFiscal.Calle;
                this.NoExterior = emisor.DomicilioFiscal.NoExterior;
                this.NoInterior = emisor.DomicilioFiscal.NoInterior;
                this.Colonia = emisor.DomicilioFiscal.Colonia;
                this.Localidad = emisor.DomicilioFiscal.Localidad;
                this.Municipio = emisor.DomicilioFiscal.Municipio;
                this.Estado = emisor.DomicilioFiscal.Estado;
                this.Pais = emisor.DomicilioFiscal.Pais;
                this.CodigoPostal = emisor.DomicilioFiscal.CodigoPostal;
                this.Referencia = emisor.DomicilioFiscal.Referencia;
            }
            //this.RegimenFiscal = emisor.RegimenFiscal;

            //  this.EmisorLogoUrl = comprobante.Emisor.LogoUrl;
            //this.EmisorTelefono = comprobante.Emisor.Telefono;
            //this.EmisorCorreo = comprobante.Emisor.Correo;
            //this.EmisorCifUrl = comprobante.Emisor.CifUrl;

            this.Correo = emisor.Correo;
            this.Telefono = emisor.Telefono;
            this.CifUrl = emisor.CifUrl;
            this.LogoUrl = emisor.LogoUrl;

            this.ViewTemplateId = emisor.ViewTemplateId;
        }

        [Required]
        [Display(Name = "RFC")]
        public string RFC { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalNameField", ShortName = "Name")]
        public string Nombre { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")] //[Display(Name = "Correo")]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "PhoneField")]
        public string Telefono { get; set; }

        [Required]
        [Display(Name = "CifUrl")]
        public string CifUrl { get; set; }

        [Required]
        [Display(Name = "LogoUrl")]
        public string LogoUrl { get; set; }

        //[Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        public string Calle { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        public string NoExterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        public string NoInterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        public string Colonia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        public string Localidad { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        public string Municipio { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        public string Estado { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        public string Pais { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        public string CodigoPostal { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        public string Referencia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStreetField")]
        public string ExpedidoEnCalle { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressExtNumberField")]
        public string ExpedidoEnNoExterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressIntNumberField")]
        public string ExpedidoEnNoInterior { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressColonyField")]
        public string ExpedidoEnColonia { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCityField")]
        public string ExpedidoEnLocalidad { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountyField")]
        public string ExpedidoEnMunicipio { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressStateField")]
        public string ExpedidoEnEstado { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressCountryField")]
        public string ExpedidoEnPais { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressZipField")]
        public string ExpedidoEnCodigoPostal { get; set; }

        [Display(ResourceType = typeof(LocalizedStrings), Name = "AddressReferenceField")]
        public string ExpedidoEnReferencia { get; set; }


        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalRegimeField")]
        public string RegimenFiscal { get; set; }

        public Guid PublicKey { get; set; }

        public int? ViewTemplateId { get; set; }
    }
    #endregion

    #region ComprobanteReceptorDetailsViewModel
    public class ComprobanteReceptorDetailsViewModel
    {
        public ComprobanteReceptorDetailsViewModel() {
            this.Domicilio = new UbicacionViewModel();
        }

        public ComprobanteReceptorDetailsViewModel(ComprobanteReceptor receptor) {
            if (receptor == null)
                throw new ArgumentNullException("receptor");

            this.PublicKey = receptor.PublicKey;

            this.RFC = receptor.RFC;
            this.Nombre = receptor.Nombre;
            //if (receptor.RegimenFiscal != null && receptor.RegimenFiscal.Count > 0)
            //    this.RegimenFiscal = receptor.RegimenFiscal[0].Regimen;

            if (receptor.Domicilio != null) {
                this.Domicilio = new UbicacionViewModel(receptor.Domicilio);
            }
        }

        [Required]
        [Display(Name = "RFC")]
        public string RFC { get; set; }

        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalNameField", ShortName = "Name")]
        public string Nombre { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "FiscalRegimeField")]
        //public string RegimenFiscal { get; set; }

        public UbicacionViewModel Domicilio { get; set; }

        public Guid PublicKey { get; set; }
    }
    #endregion

    #region ComprobanteHtmlViewModel
    public class ComprobanteHtmlViewModel
    {
        public ComprobanteHtmlViewModel(Comprobante comprobante) {

            this.Traslados = new List<ComprobanteImpuestoTrasladoTotalPorTipoViewModel>();

            if (comprobante == null)
                throw new ArgumentNullException("comprobante");

            if (comprobante.Emisor != null) {
                this.Emisor = new ComprobanteEmisorDetailViewModel(comprobante.Emisor);
            }

            if (comprobante.Receptor != null) {
                this.Receptor = new ComprobanteReceptorDetailsViewModel(comprobante.Receptor);
            }

            if (comprobante.Conceptos != null && comprobante.Conceptos.Count > 0) {
                this.Conceptos = new List<ConceptoViewModel>();
                foreach (Concepto concepto in comprobante.Conceptos) {
                    this.Conceptos.Add(new ConceptoViewModel(concepto));
                }
            }

            if (comprobante.Impuestos.Traslados != null && comprobante.Impuestos.Traslados.Count > 0) {
                this.Traslados = comprobante.Impuestos.Traslados
                    .GroupBy(traslado => new { traslado.Impuesto, traslado.Tasa })
                    .OrderByDescending(traslado => traslado.First().Impuesto)
                    .ThenBy(traslado => traslado.First().Tasa)
                    .Select(trasladoGrouped =>
                        new ComprobanteImpuestoTrasladoTotalPorTipoViewModel() {
                            Tasa = String.Format("{0}% {1}", (int)trasladoGrouped.First().Tasa, trasladoGrouped.First().Impuesto),
                            Importe = String.Format("{0:C2}", trasladoGrouped.Sum(t => t.Importe))
                        }
                    ).ToList();
            }

            this.PublicKey = comprobante.PublicKey;
            this.TipoDeComprobante = comprobante.TipoDeComprobante;
            this.Fecha = comprobante.Fecha.ToString("dd/MM/yyyy HH:mm:ss");
            this.Serie = comprobante.Serie;
            this.Folio = comprobante.Folio;



            //this.FolioFiscal = comprobante.

            this.SubTotal = comprobante.SubTotal;
            if (comprobante.Impuestos != null && comprobante.Impuestos.TotalImpuestosTrasladados.HasValue)
                this.IVA = comprobante.Impuestos.TotalImpuestosTrasladados.Value;
            this.Total = comprobante.Total;

            CantidadEnLetraConverter letraConverter = new CantidadEnLetraConverter();
            letraConverter.Numero = comprobante.Total;
            this.TotalLetra = letraConverter.letra();

            this.MetodoDePago = comprobante.MetodoDePago;
            this.NumCuenta = comprobante.NumCtaPago;

            this.MainCss = ConfigurationManager.AppSettings["InvoiceMainCss"];
            this.PrintCss = ConfigurationManager.AppSettings["InvoicePrintCss"];

            this.EmisorLogoUrl = comprobante.Emisor.LogoUrl;
            this.EmisorTelefono = comprobante.Emisor.Telefono;
            this.EmisorCorreo = comprobante.Emisor.Correo;
            this.EmisorCifUrl = comprobante.Emisor.CifUrl;

            this.NoOrden = comprobante.ExtendedIntValue1.ToString();
            this.NoCliente = comprobante.ExtendedIntValue2.ToString();

            this.Notas = comprobante.ExtendedStringValue2;

            //this.FechaTimbre
            //this.CadenaSAT = comprobante.GetCadenaSAT();
            //this.CBB
            //this.NumSerieSAT
            this.SelloCFD = comprobante.Sello;
            //this.SelloSAT = comprobante.Complementos.
            foreach (Complemento complemento in comprobante.Complementos) {
                if (complemento is TimbreFiscalDigital) {
                    TimbreFiscalDigital timbre = complemento as TimbreFiscalDigital;
                    this.SelloSAT = timbre.SelloSAT;
                    this.FechaTimbre = timbre.FechaTimbrado.ToString("dd/MM/yyyy HH:mm:ss");
                    this.FolioFiscal = timbre.UUID;
                    this.NumSerieSAT = timbre.NoCertificadoSAT;
                    this.CadenaSAT = comprobante.GetCadenaSAT();
                    this.CBB = comprobante.GetQrCode();
                }
            }
        }

        public string DocumentName {
            get {
                if ("ingreso".Equals(this.TipoDeComprobante, StringComparison.InvariantCultureIgnoreCase))
                    return "Factura";
                if ("egreso".Equals(this.TipoDeComprobante, StringComparison.InvariantCultureIgnoreCase)) // verificar en que caso sería nómina (complemento)
                    return "Nota de Crédito";

                return "Factura";
            }
        }

        public string EmisorTelefono { get; set; }
        public string EmisorCorreo { get; set; }

        public string MainCss { get; set; }

        public string PrintCss { get; set; }

        public string EmisorLogoUrl { get; set; }

        public string EmisorCifUrl { get; set; }

        public Guid PublicKey { get; set; }

        public string Status { get { return "A"; } }

        public string TipoDeComprobante { get; set; }

        //public string Title {
        //    get {
        //        return string.Format("{0}{1} - {2}", this.Serie, this.Folio, this.Receptor.Nombre);
        //    }
        //}

        public string Serie { get; set; }
        public string Folio { get; set; }

        public string Fecha { get; set; }
        public string FolioFiscal { get; set; }

        public string NoOrden { get; set; }
        public string NoCliente { get; set; }
        public string Notas { get; set; }

        public string MetodoDePago { get; set; }
        public string MetodoDePagoCode { get; set; }
        public string MetodoDePagoDescription { get; set; }
        public string MetodoDePagoDisplayName {
            get {
                if (this.MetodoDePagoCode != null && this.MetodoDePagoDescription != null)
                    return this.MetodoDePagoCode + "-" + this.MetodoDePagoDescription;
                else if (this.MetodoDePagoCode != null)
                    return this.MetodoDePagoCode;
                else if (this.MetodoDePagoDescription != null)
                    return this.MetodoDePagoDescription;
                else
                    return this.MetodoDePago;
            }
        }
        public string NumCuenta { get; set; }
        //public string NoOrden { get; set; }

        public decimal SubTotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }

        public string TotalLetra { get; set; }

        public string FechaTimbre { get; set; }
        public string CadenaSAT { get; set; }

        public string CBB { get; set; }
        public string NumSerieSAT { get; set; }

        public string SelloCFD { get; set; }
        public string SelloSAT { get; set; }

        public ComprobanteEmisorDetailViewModel Emisor { get; set; }
        public ComprobanteReceptorDetailsViewModel Receptor { get; set; }

        public List<ConceptoViewModel> Conceptos { get; set; }
        public List<ComprobanteImpuestoTrasladoTotalPorTipoViewModel> Traslados { get; set; }
    }
    #endregion

    #region ComprobanteUploadViewModel
    public class ComprobanteUploadViewModel
    {
        public ComprobanteUploadViewModel() {

        }

        //[Display(Name = "Certificado")]
        [Required, DataType(DataType.Upload), Display(Name = "XML")]
        public HttpPostedFileBase ComprobanteArchivo { get; set; }

        [DataType(DataType.Upload), Display(Name = "PDF")]
        public HttpPostedFileBase ComprobantePDFArchivo { get; set; }

        public string NoOrden { get; set; }
        public string NoCliente { get; set; }


    }
    #endregion

    #region ComprobanteUploadCancelacionViewModel
    public class ComprobanteUploadCancelacionViewModel
    {
        [Required, DataType(DataType.Upload), Display(Name = "Archivo")]
        public HttpPostedFileBase CancelacionArchivo { get; set; }

    }
    #endregion

    #region ComprobanteTrasladosPorTasaViewModel
    public class ComprobanteImpuestoTrasladoTotalPorTipoViewModel
    {
        public string Importe { get; set; }
        public string Tasa { get; set; }
    }
    #endregion

}