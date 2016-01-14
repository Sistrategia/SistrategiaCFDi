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
        private decimal tasa;
        private decimal importe;

        [Key]
        public int TrasladoId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido para señalar el tipo de impuesto trasladado.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="impuesto" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para señalar el tipo de impuesto trasladado
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:whiteSpace value="collapse"/>
        ///       <xs:enumeration value="IVA">
        ///       <xs:annotation>
        ///           <xs:documentation>Impuesto al Valor Agregado</xs:documentation>
        ///       </xs:annotation>
        ///       </xs:enumeration>
        ///       <xs:enumeration value="IEPS">
        ///          <xs:annotation>
        ///               <xs:documentation>Impuesto especial sobre productos y servicios</xs:documentation>
        ///           </xs:annotation>
        ///       </xs:enumeration>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("impuesto")]
        public string Impuesto {
            //public TrasladoImpuesto Impuesto {
            get { return this.impuesto; }
            set { this.impuesto = value; }
        }

        /// <summary>
        /// Atributo requerido para señalar la tasa del impuesto que se traslada por cada concepto amparado en el comprobante.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="tasa" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para señalar la tasa del impuesto que se traslada por cada concepto amparado en el comprobante.
        ///     </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("tasa")]
        public decimal Tasa {
            get { return this.tasa; }
            set { this.tasa = value; }
        }

        /// <summary>
        /// Atributo requerido para señalar el importe del impuesto trasladado.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="importe" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para señalar el importe del impuesto trasladado.
        ///     </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("importe")]
        public decimal Importe {
            get { return this.importe; }
            set { this.importe = value; }
        }

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