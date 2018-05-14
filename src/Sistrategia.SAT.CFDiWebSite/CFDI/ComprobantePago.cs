using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class ComprobantePago : Complemento
    {
        //private Guid comprobantePagoId { get; set; }
        //private Guid comprobanteId { get; set; }
        private string version { get; set; }
        private DateTime fechaPago { get; set; }
        private string formaDePagoP { get; set; }
        private string monedaP { get; set; }
        private string tipoCambioP { get; set; }
        private decimal monto { get; set; }
        private string numOperacion { get; set; }
        private string rfcEmisorCtaOrd { get; set; }
        private string nomBancoOrdExt { get; set; }
        private string ctaOrdenante { get; set; }
        private string rfcEmisorCtaBen { get; set; }
        private string ctaBeneficiario { get; set; }
        private string tipoCadPago { get; set; }
        private string certPago { get; set; }
        private string cadPago { get; set; }
        private string selloPago { get; set; }
        //private int ordinal { get; set; }

        private List<ComprobantePagoDoctoRelacionado> doctosRelacionados { get; set; }

        public ComprobantePago()
        {
            this.doctosRelacionados = new List<ComprobantePagoDoctoRelacionado>();
        }

        //public Guid ComprobantePagoId {
        //    get { return this.comprobantePagoId; }
        //    set { this.comprobantePagoId = value; }
        //}

        //public Guid ComprobanteId {
        //    get { return this.comprobanteId; }
        //    set { this.comprobanteId = value; }
        //}

        public string Version {
            get { return this.version; }
            set { this.version = value; }
        }

        public DateTime FechaPago {
            get { return this.fechaPago; }
            set { this.fechaPago = value; }
        }

        public string FormaDePagoP {
            get { return this.formaDePagoP; }
            set { this.formaDePagoP = value; }
        }

        public string MonedaP {
            get { return this.monedaP; }
            set { this.monedaP = value; }
        }

        public string TipoCambioP {
            get { return this.tipoCambioP; }
            set { this.tipoCambioP = value; }
        }

        public decimal Monto {
            get { return this.monto; }
            set { this.monto = value; }
        }

        public string NumOperacion {
            get { return this.numOperacion; }
            set { this.numOperacion = value; }
        }

        public string RfcEmisorCtaOrd {
            get { return this.rfcEmisorCtaOrd; }
            set { this.rfcEmisorCtaOrd = value; }
        }

        public string NombBancoOrdExt {
            get { return this.nomBancoOrdExt; }
            set { this.nomBancoOrdExt = value; }
        }

        public string CtaOrdenante {
            get { return this.ctaOrdenante; }
            set { this.ctaOrdenante = value; }
        }

        public string RfcEmisorCtaBen {
            get { return this.rfcEmisorCtaBen; }
            set { this.rfcEmisorCtaBen = value; }
        }

        public string CtaBeneficiario {
            get { return this.ctaBeneficiario; }
            set { this.ctaBeneficiario = value; }
        }

        public string TipoCadPago {
            get { return this.tipoCadPago; }
            set { this.tipoCadPago = value; }
        }

        public string CertPago {
            get { return this.certPago; }
            set { this.certPago = value; }
        }

        public string CadPago {
            get { return this.cadPago; }
            set { this.cadPago = value; }
        }

        public string SelloPago {
            get { return this.selloPago; }
            set { this.selloPago = value; }
        }

        //public int Ordinal {
        //    get { return this.ordinal; }
        //    set { this.ordinal = value; }
        //}

        public List<ComprobantePagoDoctoRelacionado> DoctosRelacionados {
            get { return this.doctosRelacionados; }
            set { this.doctosRelacionados = value; }
        }
    }
}