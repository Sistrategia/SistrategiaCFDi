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
    public class Traslado
    {
        public Traslado() {
            //this.PublicKey = Guid.NewGuid();
        }

        //private TrasladoImpuesto impuesto;
        private string impuesto;
        private string tipoFactor;
        private decimal? tasaOCuota;
        private decimal? tasa; // private decimal tasa;
        private decimal importe;

        [Key]
        public int TrasladoId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido para señalar el tipo de impuesto trasladado
        /// </summary>
        [XmlAttribute("Impuesto")] // Version 3.2: [XmlAttribute("impuesto")]
        //public ComprobanteImpuestosTrasladoImpuesto Impuesto {
        public string Impuesto {
            get { return this.impuesto; }
            set { this.impuesto = value; }
        }
        // <xs:attribute name="Impuesto" use="required" type="catCFDI:c_Impuesto">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para señalar la clave del tipo de impuesto trasladado.
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
        /// Atributo requerido para señalar la clave del tipo de factor que se aplica a la base del impuesto.
        /// </summary>
        [XmlAttribute("TipoFactor")]
        public string TipoFactor {
            get { return this.tipoFactor; }
            set { this.tipoFactor = value; }
        }
        // <xs:attribute name="TipoFactor" type="catCFDI:c_TipoFactor" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para señalar la clave del tipo de factor que se aplica 
        //       a la base del impuesto.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
        // ... 
        // <xs:simpleType name="c_TipoFactor">
        //   <xs:restriction base="xs:string">
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:enumeration value="Tasa"/>
        //     <xs:enumeration value="Cuota"/>
        //     <xs:enumeration value="Exento"/>
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>
        /// Atributo requerido para señalar la tasa del impuesto que se traslada por cada concepto 
        /// amparado en el comprobante
        /// </summary>
        [XmlIgnore] // Version 3.2: [XmlAttribute("tasa")]
        [Obsolete("Descontinuado a partir de la versión 3.3.", false)]
        public decimal? Tasa {
            get { return this.tasa; }
            set { this.tasa = value; }
        }
        // <xs:attribute name="tasa" type="cfdi:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para señalar la tasa del impuesto que se traslada por cada concepto 
        //       amparado en el comprobante
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para señalar el valor de la tasa o cuota del impuesto que se traslada 
        /// por los conceptos amparados en el comprobante.
        /// </summary>
        [XmlAttribute("TasaOCuota")]
        public decimal? TasaOCuota {
            get { return this.tasaOCuota; }
            set { this.tasaOCuota = value; }
        }
        // <xs:attribute name="TasaOCuota" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para señalar el valor de la tasa o cuota del impuesto 
        //       que se traslada por los conceptos amparados en el comprobante.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:decimal">
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:minInclusive value="0.000000"/>
        //     <xs:fractionDigits value="6"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para señalar la suma del importe del impuesto trasladado, agrupado por 
        /// impuesto, TipoFactor y TasaOCuota. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("Importe")]
        public decimal Importe {
            get { return this.importe; }
            set { this.importe = value; }
        }
        // <xs:attribute name="Importe" type="tdCFDI:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para señalar la suma del importe del impuesto trasladado, agrupado 
        //       por impuesto, TipoFactor y TasaOCuota. No se permiten valores negativos.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        [XmlIgnore]
        public int? Ordinal { get; set; }
    }
}


///// <summary>
///// Atributo requerido para señalar el tipo de impuesto trasladado
///// </summary>
//[Serializable]
//[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
//public enum TrasladoImpuesto
//{
//    /// <summary>
//    /// Impuesto al Valor Agregado
//    /// </summary>
//    IVA,
//    /// <summary>
//    /// Impuesto especial sobre productos y servicios
//    /// </summary>
//    IEPS,
//}
////<xs:attribute name="impuesto" use="required">
////    <xs:annotation>
////    <xs:documentation>Atributo requerido para señalar el tipo de impuesto trasladado</xs:documentation>
////    </xs:annotation>
////    <xs:simpleType>
////    <xs:restriction base="xs:string">
////        <xs:whiteSpace value="collapse"/>
////        <xs:enumeration value="IVA">
////        <xs:annotation>
////            <xs:documentation>Impuesto al Valor Agregado</xs:documentation>
////        </xs:annotation>
////        </xs:enumeration>
////        <xs:enumeration value="IEPS">
////        <xs:annotation>
////            <xs:documentation>Impuesto especial sobre productos y servicios</xs:documentation>
////        </xs:annotation>
////        </xs:enumeration>
////    </xs:restriction>
////    </xs:simpleType>
////</xs:attribute>
//}