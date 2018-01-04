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
    public class Retencion
    {
        public Retencion() {
            //this.PublicKey = Guid.NewGuid();
        }

        //private RetencionImpuesto impuesto;
        private string impuesto;
        private decimal importe;

        [Key]
        public int RetencionId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido para señalar la clave del tipo de impuesto retenido
        /// </summary>
        [XmlAttribute("Impuesto")] // Version 3.2: [XmlAttribute("impuesto")]
        //public ComprobanteImpuestosRetencionImpuesto Impuesto {
        public string Impuesto {
            get { return this.impuesto; }
            set { this.impuesto = value; }
        }
        // <xs:attribute name="Impuesto" use="required" type="catCFDI:c_Impuesto">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para señalar la clave del tipo de impuesto retenido
        //     </xs:documentation>
        //   </xs:annotation>        
        // </xs:attribute>
        // ...
        // <xs:simpleType name="c_Impuesto">
        //   <xs:restriction base="xs:string">
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:enumeration value="001"/> // ISR
        //     <xs:enumeration value="002"/> // IVA
        //     <xs:enumeration value="003"/> // IEPS
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>
        /// Atributo requerido para señalar el monto del impuesto retenido. 
        /// No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("Importe")] // Version 3.2: [XmlAttribute("importe")]
        public decimal Importe {
            get { return this.importe; }
            set { this.importe = value; }
        }
        // <xs:attribute name="Importe" type="tdCFDI:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para señalar el monto del impuesto retenido. 
        //       No se permiten valores negativos.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        ///// <summary>
        ///// Atributo requerido para señalar el tipo de impuesto retenido.
        ///// </summary>
        //[Serializable]
        //[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
        //public enum RetencionImpuesto
        //{
        //    /// <summary>
        //    /// Impuesto sobre la renta
        //    /// </summary>
        //    ISR,
        //    /// <summary>
        //    /// Impuesto al Valor Agregado
        //    /// </summary>
        //    IVA,
        //}
        ////<xs:simpleType>
        ////    <xs:restriction base="xs:string">
        ////        <xs:whiteSpace value="collapse"/>
        ////        <xs:enumeration value="ISR">
        ////            <xs:annotation>
        ////                <xs:documentation>Impuesto sobre la renta</xs:documentation>
        ////            </xs:annotation>
        ////        </xs:enumeration>
        ////        <xs:enumeration value="IVA">
        ////            <xs:annotation>
        ////                <xs:documentation>Impuesto al Valor Agregado</xs:documentation>
        ////            </xs:annotation>
        ////        </xs:enumeration>
        ////    </xs:restriction>
        ////</xs:simpleType>

        [XmlIgnore]
        public int? Ordinal { get; set; }

    }
}