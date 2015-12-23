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

namespace Sistrategia.SAT.CFDiWebSite.Models
{
    public class ComprobanteIndexViewModel
    {
        public ComprobanteIndexViewModel() {
            this.Comprobantes = new List<Comprobante>();
        }
        public List<Comprobante> Comprobantes { get; set; }
    }

    public class ComprobanteCreateViewModel
    {
        public ComprobanteCreateViewModel() {
            this.Emisor = new EmisorDetailViewModel();
            this.Receptor = new ReceptorDetailsViewModel();
            this.Conceptos = new List<ConceptoViewModel>();
        }

        public ComprobanteCreateViewModel(Comprobante comprobante) {
            if (comprobante == null)
                throw new ArgumentNullException("comprobante");

            if (comprobante.Emisor != null) {
                this.Emisor = new EmisorDetailViewModel(comprobante.Emisor);
            }

            if (comprobante.Receptor != null) {
                this.Receptor = new ReceptorDetailsViewModel(comprobante.Receptor);
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
        }

        public string Serie { get; set; }
        public string Folio { get; set; }

        public decimal SubTotal { get; set; }
        public decimal IVA { get; set; }
        public decimal TasaIVA { get; set; }
        public decimal ISR { get; set; }
        public decimal Total { get; set; }

        public string FormaDePago { get; set; }
        public string MetodoDePago { get; set; }
        public string NumCtaPago { get; set; }
        public string LugarExpedicion { get; set; }
        public string TipoCambio { get; set; }

        public EmisorDetailViewModel Emisor { get; set; }
        public ReceptorDetailsViewModel Receptor { get; set; }


        //public List<Receptor> Receptores { get; set; }
        public IEnumerable<SelectListItem> Emisores { get; set; }
        public IEnumerable<SelectListItem> Receptores { get; set; }
        public IEnumerable<SelectListItem> Certificados { get; set; }

        public List<ConceptoViewModel> Conceptos { get; set; }

        public int EmisorId { get; set; }
        public int ReceptorId { get; set; }
        public int CertificadoId { get; set; }
    }

    public class ComprobanteDetailViewModel
    {
        public ComprobanteDetailViewModel(Comprobante comprobante) {
            if (comprobante == null)
                throw new ArgumentNullException("comprobante");

            this.PublicKey = comprobante.PublicKey;

            if (comprobante.Emisor != null) {
                this.Emisor = new EmisorDetailViewModel(comprobante.Emisor);
            }

            if (comprobante.Receptor != null) {
                this.Receptor = new ReceptorDetailsViewModel(comprobante.Receptor);
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

            this.CadenaOriginal = comprobante.GetCadenaOriginal();
            this.Sello = comprobante.Sello;
            //this.Sello = comprobante.Sello;
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

        public EmisorDetailViewModel Emisor { get; set; }
        public ReceptorDetailsViewModel Receptor { get; set; }

        public List<ConceptoViewModel> Conceptos { get; set; }
    }

    public class ComprobanteHtmlViewModel
    {
        public ComprobanteHtmlViewModel(Comprobante comprobante) {
            if (comprobante == null)
                throw new ArgumentNullException("comprobante");

            if (comprobante.Emisor != null) {
                this.Emisor = new EmisorDetailViewModel(comprobante.Emisor);
            }

            if (comprobante.Receptor != null) {
                this.Receptor = new ReceptorDetailsViewModel(comprobante.Receptor);
            }

            if (comprobante.Conceptos != null && comprobante.Conceptos.Count > 0) {
                this.Conceptos = new List<ConceptoViewModel>();
                foreach (Concepto concepto in comprobante.Conceptos) {
                    this.Conceptos.Add(new ConceptoViewModel(concepto));
                }
            }

            this.PublicKey = comprobante.PublicKey;

            this.Fecha = comprobante.Fecha.ToString("dd/MM/yyyy hh:mm:ss");
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

            this.MainCss = ConfigurationManager.AppSettings["InvoiceMainCss"];
            this.PrintCss = ConfigurationManager.AppSettings["InvoicePrintCss"];

            this.EmisorLogoUrl = comprobante.Emisor.LogoUrl;
            this.EmisorTelefono = comprobante.Emisor.Telefono;
            this.EmisorCorreo = comprobante.Emisor.Correo;
            this.EmisorCifUrl = comprobante.Emisor.CifUrl;

            this.NoOrden = comprobante.ExtendedIntValue1.ToString();
            this.NoCliente = comprobante.ExtendedIntValue2.ToString();

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
                    this.FechaTimbre = timbre.FechaTimbrado.ToString("dd/MM/yyyy hh:mm:ss");
                    this.FolioFiscal = timbre.UUID;                    
                    this.NumSerieSAT = timbre.NoCertificadoSAT;
                    this.CadenaSAT = comprobante.GetCadenaSAT();
                    this.CBB = comprobante.GetQrCode();
                }
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

        public string MetodoDePago { get; set; }
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

        public EmisorDetailViewModel Emisor { get; set; }
        public ReceptorDetailsViewModel Receptor { get; set; }

        public List<ConceptoViewModel> Conceptos { get; set; }
    }

    public class ConceptoViewModel
    {
        public ConceptoViewModel() {
            this.Unidad = "pza";
        }

        public ConceptoViewModel(Concepto concepto) {
            this.Cantidad = concepto.Cantidad;
            this.Unidad = concepto.Unidad;
            this.NoIdentificacion = concepto.NoIdentificacion;
            this.Descripcion = concepto.Descripcion;
            this.ValorUnitario = concepto.ValorUnitario;
            this.Importe = concepto.Importe;
        }

        public decimal Cantidad { get; set; }
        public string Unidad { get; set; }
        public string NoIdentificacion { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Importe { get; set; }

    }
}