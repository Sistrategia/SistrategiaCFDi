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
        /// Atributo requerido para señalar el tipo de impuesto retenido
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="impuesto" use="required">
        ///     <xs:annotation>
        ///         <xs:documentation>Atributo requerido para señalar el tipo de impuesto retenido</xs:documentation>
        ///     </xs:annotation>
        ///     <xs:simpleType>
        ///         <xs:restriction base="xs:string">
        ///             <xs:whiteSpace value="collapse"/>
        ///             <xs:enumeration value="ISR">
        ///                 <xs:annotation>
        ///                     <xs:documentation>Impuesto sobre la renta</xs:documentation>
        ///                 </xs:annotation>
        ///             </xs:enumeration>
        ///             <xs:enumeration value="IVA">
        ///                 <xs:annotation>
        ///                     <xs:documentation>Impuesto al Valor Agregado</xs:documentation>
        ///                 </xs:annotation>
        ///             </xs:enumeration>
        ///         </xs:restriction>
        ///     </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("impuesto")]
        public string Impuesto {
            //public RetencionImpuesto Impuesto {
            get { return this.impuesto; }
            set { this.impuesto = value; }
        }

        /// <summary>
        /// Atributo requerido para señalar el importe o monto del impuesto retenido.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="importe" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para señalar el importe o monto del impuesto retenido.
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