using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class ComprobantePagoDoctoRelacionado
    {
        private Guid comprobantePagoDoctoRelacionadoId { get; set; }
        private Guid comprobantePagoId { get; set; }
        private Guid doctoRelacionadoId { get; set; }
        private string idDocumento { get; set; }
        private string serie { get; set; }
        private string folio { get; set; }
        private string monedaDR { get; set; }
        private decimal tipoCambioDR { get; set; }
        private string metodoDePagoDR { get; set; }
        private int numParcialidades { get; set; }
        private decimal impSaldAnt { get; set; }
        private decimal impPagado { get; set; }
        private decimal impSaldoInsoluto { get; set; }
        private int ordinal { get; set; }

        public Guid ComprobantePagoDoctoRelacionadoId {
            get { return this.comprobantePagoDoctoRelacionadoId; }
            set { this.comprobantePagoDoctoRelacionadoId = value; }
        }

        public Guid ComprobantePagoId {
            get { return this.comprobantePagoId; }
            set { this.comprobantePagoId = value; }
        }

        public Guid DoctoRelacionadoId {
            get { return this.doctoRelacionadoId; }
            set { this.doctoRelacionadoId = value; }
        }

        public string IdDocumento {
            get { return this.idDocumento; }
            set { this.idDocumento = value; }
        }

        public string Serie {
            get { return this.serie; }
            set { this.serie = value; }
        }

        public string Folio {
            get { return this.folio; }
            set { this.folio = value; }
        }

        public string MonedaDR {
            get { return this.monedaDR; }
            set { this.monedaDR = value; }
        }

        public decimal TipoCambioDR {
            get { return this.tipoCambioDR; }
            set { this.tipoCambioDR = value; }
        }

        public string MetodoDePagoDR {
            get { return this.metodoDePagoDR; }
            set { this.metodoDePagoDR = value; }
        }

        public int NumParcialidades {
            get { return this.numParcialidades; }
            set { this.numParcialidades = value; }
        }

        public decimal ImpSaldAnt {
            get { return this.impSaldAnt; }
            set { this.impSaldAnt = value; }
        }

        public decimal ImpPagado {
            get { return this.impPagado; }
            set { this.impPagado = value; }
        }

        public decimal ImpSaldoInsoluto {
            get { return this.impSaldoInsoluto; }
            set { this.impSaldoInsoluto = value; }
        }

        public int Ordinal {
            get { return this.ordinal; }
            set { this.ordinal = value; }
        }
    }
}