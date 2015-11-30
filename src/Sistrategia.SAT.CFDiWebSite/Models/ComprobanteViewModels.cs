using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
//using Microsoft.Owin.Security;
using Sistrategia.SAT.Resources;
using Sistrategia.SAT.CFDiWebSite.CFDI;

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
            this.Receptor = new ReceptorDetailViewModel();
            this.Conceptos = new List<ConceptoViewModel>();
        }

        public ComprobanteCreateViewModel(Comprobante comprobante) {
            if (comprobante == null)
                throw new ArgumentNullException("comprobante");

            if (comprobante.Emisor != null) {
                this.Emisor = new EmisorDetailViewModel(comprobante.Emisor);
            }

            if (comprobante.Receptor != null) {
                this.Receptor = new ReceptorDetailViewModel(comprobante.Receptor);
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
        public decimal Total { get; set; }

        public EmisorDetailViewModel Emisor { get; set; }
        public ReceptorDetailViewModel Receptor { get; set; }

        public List<ConceptoViewModel> Conceptos { get; set; }
    }

    public class ComprbanteDetailViewModel
    {
        public ComprbanteDetailViewModel(Comprobante comprobante) {
            if (comprobante == null)
                throw new ArgumentNullException("comprobante");

            if (comprobante.Emisor != null) {
                this.Emisor = new EmisorDetailViewModel(comprobante.Emisor);
            }

            if (comprobante.Receptor != null) {
                this.Receptor = new ReceptorDetailViewModel(comprobante.Receptor);
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

        public string Title {
            get {
                return string.Format("{0}{1} - {2}", this.Serie, this.Folio, this.Receptor.Nombre);
            }
        }

        public string Serie { get; set; }
        public string Folio { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public EmisorDetailViewModel Emisor { get; set; }
        public ReceptorDetailViewModel Receptor { get; set; }

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