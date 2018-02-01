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
    public class ConceptoImpuestosRetencion
    {
        public ConceptoImpuestosRetencion() {
            //this.PublicKey = Guid.NewGuid();
        }

        private decimal _base;        
        private string impuesto;        
        private string tipoFactor;
        private decimal? tasaOCuota;
        private decimal? importe;

        [Key]
        public int ConceptoImpuestosRetencionId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido para señalar la base para el cálculo de la retención, la determinación de la 
        /// base se realiza de acuerdo con las disposiciones fiscales vigentes. No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("Base")]
        public decimal Base {
            get { return this._base; }
            set { this._base = value; }
        }
        // <xs:attribute name="Base" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para señalar la base para el cálculo de la retención, 
        //      la determinación de la base se realiza de acuerdo con las disposiciones fiscales vigentes. 
        //      No se permiten valores negativos.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:decimal">
        //       <xs:fractionDigits value="6"/>
        //       <xs:minInclusive value="0.000001"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para señalar la clave del tipo de impuesto retenido aplicable al concepto.
        /// </summary>
        [XmlAttribute("Impuesto")]
        public string Impuesto {
            get { return this.impuesto; }
            set { this.impuesto = value; }
        }
        // <xs:attribute name="Impuesto" use="required" type="catCFDI:c_Impuesto">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para señalar la clave del tipo de impuesto retenido aplicable al concepto.
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
        /// Atributo requerido para señalar la tasa o cuota del impuesto que se retiene para el presente concepto.
        /// </summary>
        [XmlAttribute("TasaOCuota")]
        public decimal? TasaOCuota {
            get { return this.tasaOCuota; }
            set { this.tasaOCuota = value; }
        }
        // <xs:attribute name="TasaOCuota" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo condicional para señalar el valor de la tasa o cuota del impuesto 
        //       que se traslada para el presente concepto. Es requerido cuando el atributo TipoFactor tenga 
        //       una clave que corresponda a Tasa o Cuota.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:decimal">
        //     <xs:fractionDigits value="6"/>        
        //     <xs:minInclusive value="0.000000"/>
        //     <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para señalar el importe del impuesto retenido que aplica al concepto. 
        /// No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("importe")]
        public decimal? Importe {
            get { return this.importe; }
            set { this.importe = value; }
        }
        // <xs:attribute name="Importe" type="tdCFDI:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para señalar el importe del impuesto retenido que aplica al concepto. 
        //       No se permiten valores negativos.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        [XmlIgnore]
        public int? Ordinal { get; set; }
    }
}