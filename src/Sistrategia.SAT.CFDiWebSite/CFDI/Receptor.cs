using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class Receptor
    {
        public Receptor()
            : base() {
            this.PublicKey = Guid.NewGuid();
            this.Status = "A";
        }

        #region Private Fields
        private string rfc;
        private string nombre;
        private Ubicacion domicilio;
        #endregion

        [Key]
        public int ReceptorId { get; set; }

        [Required]
        public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido para precisar la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente receptor del comprobante.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="rfc" type="cfdi:t_RFC" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para precisar la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente receptor del comprobante.
        ///     </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// <code>
        /// <xs:simpleType name="t_RFC">
        ///     <xs:annotation>
        ///         <xs:documentation>Tipo definido para expresar claves del Registro Federal de Contribuyentes</xs:documentation>
        ///     </xs:annotation>
        ///     <xs:restriction base="xs:string">
        ///         <xs:minLength value="12"/>
        ///         <xs:maxLength value="13"/>
        ///         <xs:whiteSpace value="collapse"/>
        ///         <xs:pattern value="[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?"/>
        ///     </xs:restriction>
        /// </xs:simpleType>
        /// </code>
        /// </remarks>
        [XmlAttribute("rfc")]
        public string RFC {
            get { return this.rfc; }
            set { this.rfc = value; }
        }

        /// <summary>
        /// Atributo opcional para precisar el nombre o razón social del contribuyente receptor.
        /// </summary>
        /// <remarks>
        /// <code>        
        /// <xs:attribute name="nombre" use="optional">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo opcional para el nombre, denominación o razón social del contribuyente receptor del comprobante.
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
        [XmlAttribute("nombre")]
        public string Nombre {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        [ForeignKey("Domicilio")]
        public int? DomicilioId { get; set; }

        /// <summary>
        /// Nodo opcional para la definición de la ubicación donde se da el domicilio del receptor del comprobante fiscal.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:sequence>
        ///   <xs:element name="Domicilio" type="cfdi:t_Ubicacion" minOccurs="0">
        ///     <xs:annotation>
        ///       <xs:documentation>
        ///         Nodo opcional para la definición de la ubicación donde se da el domicilio del receptor del comprobante fiscal.
        ///       </xs:documentation>
        ///     </xs:annotation>
        ///   </xs:element>
        /// </xs:sequence>
        /// </code>
        /// </remarks>
        [XmlElement("Domicilio")]
        public virtual Ubicacion Domicilio {
            get { return this.domicilio; }
            set { this.domicilio = value; }
        }

        [XmlIgnore]
        public string Status { get; set; }
    }
}