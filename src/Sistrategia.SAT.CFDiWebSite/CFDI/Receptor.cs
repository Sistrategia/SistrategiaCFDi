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
        private string residenciaFiscal;
        private string numRegIdTrib;
        private string usoCFDI;
        private Ubicacion domicilio;
        #endregion

        [Key]
        public int ReceptorId { get; set; }

        [Required]
        public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido para precisar la Clave del Registro Federal de Contribuyentes 
        /// correspondiente al contribuyente receptor del comprobante.
        /// </summary>
        /// <remarks>       
        /// </remarks>
        [XmlAttribute("Rfc")] // Version 3.2: [XmlAttribute("rfc")]
        public string RFC {
            get { return this.rfc; }
            set { this.rfc = value; }
        }
        // <xs:attribute name="Rfc" use="required"  type="tdCFDI:t_RFC">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para precisar la Clave del Registro Federal de 
        //       Contribuyentes correspondiente al contribuyente receptor del comprobante.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional para precisar el nombre, denominación o razón social del contribuyente 
        /// receptor del comprobante.
        /// </summary>
        /// <remarks> 
        /// El largo debe estar entre 1 y 254 caracteres
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlAttribute("Nombre")] // Version 3.2: [XmlAttribute("nombre")]
        public string Nombre {
            get { return this.nombre; }
            set { this.nombre = value; }
        }
        // <xs:attribute name="Nombre" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para precisar el nombre, denominación o razón social
        //      del contribuyente receptor del comprobante.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="254"/>
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:pattern value="[^|]{1,254}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo condicional para registrar la clave del país de residencia para efectos fiscales 
        /// del receptor del comprobante, cuando se trate de un extranjero, y que es conforme con 
        /// la especificación ISO 3166-1 alpha-3. Es requerido cuando se incluya el complemento de 
        /// comercio exterior o se registre el atributo NumRegIdTrib.
        /// receptor del comprobante.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("ResidenciaFiscal")]
        public string ResidenciaFiscal {
            get { return this.residenciaFiscal; }
            set { this.residenciaFiscal = value; }
        }
        // <xs:attribute name="ResidenciaFiscal" use="optional" type="catCFDI:c_Pais">
        //   <xs:annotation>
        //     <xs:documentation>Atributo condicional para registrar la clave del país de residencia para 
        //       efectos fiscales del receptor del comprobante, cuando se trate de un extranjero, y que 
        //       es conforme con la especificación ISO 3166-1 alpha-3. Es requerido cuando se incluya el 
        //       complemento de comercio exterior o se registre el atributo NumRegIdTrib.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo condicional para expresar el número de registro de identidad fiscal del receptor 
        /// cuando sea residente en el  extranjero. Es requerido cuando se incluya el complemento de 
        /// comercio exterior.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("NumRegIdTrib")]
        public string NumRegIdTrib {
            get { return this.numRegIdTrib; }
            set { this.numRegIdTrib = value; }
        }
        // <xs:attribute name="NumRegIdTrib" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo condicional para expresar el número de registro de identidad 
        //       fiscal del receptor cuando sea residente en el  extranjero. Es requerido cuando se incluya 
        //       el complemento de comercio exterior.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="40"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para expresar la clave del uso que dará a esta factura el receptor del CFDI.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("UsoCFDI")]
        public string UsoCFDI {
            get { return this.usoCFDI; }
            set { this.usoCFDI = value; }
        }
        // <xs:attribute name="UsoCFDI" use="required" type="catCFDI:c_UsoCFDI">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para expresar la clave del uso que dará a esta 
        //       factura el receptor del CFDI.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="40"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

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