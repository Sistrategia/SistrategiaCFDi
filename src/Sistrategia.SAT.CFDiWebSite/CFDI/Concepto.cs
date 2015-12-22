using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class Concepto
    {
        #region Private Fields
        private decimal cantidad;
        private string unidad;
        private string noIdentificacion;
        private string descripcion;
        private decimal valorUnitario;
        private decimal importe;
        private int ordinal;
        #endregion

        [Key]
        public int ConceptoId { get; set; }

        [Required]
        public Guid PublicKey { get; set; }

        //private object[] items;

        ///// <summary>
        ///// ComplementoConcepto: Nodo opcional donde se incluirán los nodos complementarios de extensión al concepto, definidos por el SAT, de acuerdo a disposiciones particulares a un sector o actividad especifica.
        ///// CuentaPredial: Nodo opcional para asentar el número de cuenta predial con el que fue registrado el inmueble, en el sistema catastral de la entidad federativa de que trate, o bien para incorporar los datos de identificación del certificado de participación inmobiliaria no amortizable.
        ///// InformacionAduanera: Nodo opcional para introducir la información aduanera aplicable cuando se trate de ventas de primera mano de mercancías importadas.
        ///// Parte: Nodo opcional para expresar las partes o componentes que integran la totalidad del concepto expresado en el comprobante fiscal digital a través de Internet.
        ///// </summary>        
        //[XmlElement("ComplementoConcepto", typeof(ComplementoConcepto))]
        //[XmlElement("CuentaPredial", typeof(CuentaPredial))]
        //[XmlElement("InformacionAduanera", typeof(InformacionAduanera))]
        //[XmlElement("Parte", typeof(ConceptoParte))]
        //public object[] Items {
        //    get { return this.items; }
        //    set { this.items = value; }
        //}
        ////<xs:choice minOccurs="0">
        ////    <xs:element name="InformacionAduanera" type="cfdi:t_InformacionAduanera" minOccurs="0" maxOccurs="unbounded">
        ////    <xs:annotation>
        ////        <xs:documentation>
        ////        Nodo opcional para introducir la información aduanera aplicable cuando se trate de ventas de primera mano de mercancías importadas.
        ////        </xs:documentation>
        ////    </xs:annotation>
        ////    </xs:element>
        ////    <xs:element name="CuentaPredial" minOccurs="0">
        ////    <xs:annotation>
        ////        <xs:documentation>
        ////        Nodo opcional para asentar el número de cuenta predial con el que fue registrado el inmueble, en el sistema catastral de la entidad federativa de que trate, o bien para incorporar los datos de identificación del certificado de participación inmobiliaria no amortizable.
        ////        </xs:documentation>
        ////    </xs:annotation>
        ////    <xs:complexType>
        ////        <xs:attribute name="numero" use="required">
        ////        <xs:annotation>
        ////            <xs:documentation>
        ////            Atributo requerido para precisar el número de la cuenta predial del inmueble cubierto por el presente concepto, o bien para incorporar los datos de identificación del certificado de participación inmobiliaria no amortizable, tratándose de arrendamiento.
        ////            </xs:documentation>
        ////        </xs:annotation>
        ////        <xs:simpleType>
        ////            <xs:restriction base="xs:string">
        ////            <xs:whiteSpace value="collapse"/>
        ////            <xs:minLength value="1"/>
        ////            </xs:restriction>
        ////        </xs:simpleType>
        ////        </xs:attribute>
        ////    </xs:complexType>
        ////    </xs:element>
        ////    <xs:element name="ComplementoConcepto" minOccurs="0">
        ////    <xs:annotation>
        ////        <xs:documentation>
        ////        Nodo opcional donde se incluirán los nodos complementarios de extensión al concepto, definidos por el SAT, de acuerdo a disposiciones particulares a un sector o actividad especifica.
        ////        </xs:documentation>
        ////    </xs:annotation>
        ////    <xs:complexType>
        ////        <xs:sequence>
        ////        <xs:any minOccurs="0" maxOccurs="unbounded"/>
        ////        </xs:sequence>
        ////    </xs:complexType>
        ////    </xs:element>
        ////    <xs:element name="Parte" minOccurs="0" maxOccurs="unbounded">
        ////    <xs:annotation>
        ////        <xs:documentation>
        ////        Nodo opcional para expresar las partes o componentes que integran la totalidad del concepto expresado en el comprobante fiscal digital a través de Internet
        ////        </xs:documentation>
        ////    </xs:annotation>
        ////    <xs:complexType>
        ////        <xs:sequence>
        ////        <xs:element name="InformacionAduanera" type="cfdi:t_InformacionAduanera" minOccurs="0" maxOccurs="unbounded">
        ////            <xs:annotation>
        ////            <xs:documentation>
        ////                Nodo opcional para introducir la información aduanera aplicable cuando se trate de partes o componentes importados vendidos de primera mano.
        ////            </xs:documentation>
        ////            </xs:annotation>
        ////        </xs:element>
        ////        </xs:sequence>


        /// <summary>
        /// Atributo requerido para precisar la cantidad de bienes o servicios del tipo particular definido por el presente concepto.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="cantidad" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para precisar la cantidad de bienes o servicios del tipo particular definido por el presente concepto.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:decimal">
        ///       <xs:whiteSpace value="collapse"/>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("cantidad")]
        public decimal Cantidad {
            get { return this.cantidad; }
            set { this.cantidad = value; }
        }

        /// <summary>
        /// Atributo requerido para precisar la unidad de medida aplicable para la cantidad expresada en el concepto.
        /// </summary>
        /// <remarks>
        /// Antes era opcional.
        /// <code>
        /// <xs:attribute name="unidad" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para precisar la unidad de medida aplicable para la cantidad expresada en el concepto.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:whiteSpace value="collapse"/>
        ///       <xs:minLength value="1"/>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("unidad")]
        public string Unidad {
            get { return this.unidad; }
            set { this.unidad = value; }
        }

        /// <summary>
        /// Atributo opcional para expresar el número de serie del bien o identificador del servicio amparado por el presente concepto.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="noIdentificacion" use="optional">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo opcional para expresar el número de serie del bien o identificador del servicio amparado por el presente concepto.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:minLength value="1"/>
        ///       <xs:whiteSpace value="collapse"/>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("noIdentificacion")]
        public string NoIdentificacion {
            get { return this.noIdentificacion; }
            set { this.noIdentificacion = value; }
        }

        /// <summary>
        /// Atributo requerido para precisar la descripción del bien o servicio cubierto por el presente concepto.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="descripcion" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para precisar la descripción del bien o servicio cubierto por el presente concepto.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:minLength value="1"/>
        ///       <xs:whiteSpace value="collapse"/>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("descripcion")]
        public string Descripcion {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }

        /// <summary>
        /// Atributo requerido para precisar el valor o precio unitario del bien o servicio cubierto por el presente concepto.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="valorUnitario" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para precisar el valor o precio unitario del bien o servicio cubierto por el presente concepto.
        ///     </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("valorUnitario")]
        public decimal ValorUnitario {
            get { return this.valorUnitario; }
            set { this.valorUnitario = value; }
        }


        /// <summary>
        /// Atributo requerido para precisar el importe total de los bienes o servicios del presente concepto. Debe ser equivalente al resultado de multiplicar la cantidad por el valor unitario expresado en el concepto.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="importe" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para precisar el importe total de los bienes o servicios del presente concepto. Debe ser equivalente al resultado de multiplicar la cantidad por el valor unitario expresado en el concepto.
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
        public int Ordinal {
            get { return this.ordinal; }
            set { this.ordinal = value; }
        }
    }
}