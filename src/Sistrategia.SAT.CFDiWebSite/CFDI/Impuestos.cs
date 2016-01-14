using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using System.Text;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class Impuestos
    {
        #region
        private decimal? totalImpuestosRetenidos;
        //private bool totalImpuestosRetenidosSpecified = false;
        private decimal? totalImpuestosTrasladados;
        //private bool totalImpuestosTrasladadosSpecified = false;
        #endregion
        [Key]
        public int ImpuestosId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        /// <summary>
        /// Nodo opcional para capturar los impuestos retenidos aplicables
        /// </summary>
        [XmlArrayItem("Retencion", IsNullable = false)]
        public virtual List<Retencion> Retenciones { get; set; }
        //public Retencion[] Retenciones {
        //    get { return this.retenciones; }
        //    set { this.retenciones = value; }
        //}

        /// <summary>
        /// Nodo opcional para asentar o referir los impuestos trasladados aplicables
        /// </summary>
        [XmlArrayItem("Traslado", IsNullable = false)]
        public virtual List<Traslado> Traslados { get; set; }
        //public Traslado[] Traslados {
        //    get { return this.traslados; }
        //    set { this.traslados = value; }
        //}

        /// <summary>
        /// Atributo opcional para expresar el total de los impuestos retenidos que se desprenden 
        /// de los conceptos expresados en el comprobante fiscal digital a través de Internet.
        /// </summary>
        [XmlAttribute("totalImpuestosRetenidos")]
        public decimal? TotalImpuestosRetenidos {
            get { return this.totalImpuestosRetenidos; }
            set { this.totalImpuestosRetenidos = value; }
        }
        //<xs:attribute name="totalImpuestosRetenidos" type="cfdi:t_Importe" use="optional">
        //    <xs:annotation>
        //    <xs:documentation>Atributo opcional para expresar el total de los impuestos retenidos que se desprenden de los conceptos expresados en el comprobante fiscal digital a través de Internet.</xs:documentation>
        //    </xs:annotation>
        //</xs:attribute>


        //[XmlIgnore]
        //public bool TotalImpuestosRetenidosSpecified {
        //    get { return this.totalImpuestosRetenidosSpecified; }
        //    set { this.totalImpuestosRetenidosSpecified = value; }
        //}

        /// <summary>
        /// Atributo opcional para expresar el total de los impuestos trasladados que se desprenden 
        /// de los conceptos expresados en el comprobante fiscal digital a través de Internet.
        /// </summary>
        [XmlAttribute("totalImpuestosTrasladados")]
        public decimal? TotalImpuestosTrasladados {
            get { return this.totalImpuestosTrasladados; }
            set { this.totalImpuestosTrasladados = value; }
        }
        //<xs:attribute name="totalImpuestosTrasladados" type="cfdi:t_Importe" use="optional">
        //    <xs:annotation>
        //    <xs:documentation>Atributo opcional para expresar el total de los impuestos trasladados que se desprenden de los conceptos expresados en el comprobante fiscal digital a través de Internet.</xs:documentation>
        //    </xs:annotation>
        //</xs:attribute>

        //[XmlIgnore]
        //public bool TotalImpuestosTrasladadosSpecified {
        //    get { return this.totalImpuestosTrasladadosSpecified; }
        //    set { this.totalImpuestosTrasladadosSpecified = value; }
        //}
    }
}