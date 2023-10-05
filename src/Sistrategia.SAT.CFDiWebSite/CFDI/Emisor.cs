// Copyright (c) JEOCSI SA DE CV. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class Emisor
    {
        public Emisor()
            : base() {
            this.PublicKey = Guid.NewGuid();
            this.Status = "A";
        }

        //private string rfc;
        private string nombre;
        // private UbicacionFiscal domicilioFiscal;
        // private Ubicacion expedidoEn;

        [Key]
        public int EmisorId { get; set; }

        [Required]
        public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido para registrar la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente emisor del comprobante.
        /// </summary>
        [Required]
        [MaxLength(13)]
        public string RFC { get; set; }
        // <xs:attribute name="rfc" type="cfdi:t_RFC" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para registrar la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente emisor del comprobante.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
        // <xs:simpleType name="t_RFC">
        //   <xs:annotation>
        //     <xs:documentation>Tipo definido para expresar claves del Registro Federal de Contribuyentes</xs:documentation>
        //   </xs:annotation>
        //   <xs:restriction base="xs:string">
        //     <xs:minLength value="12"/>
        //     <xs:maxLength value="13"/>
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:pattern value="[A-Z&amp;Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A]"/>    
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>
        /// Atributo requerido para registrar el nombre, denominación o razón social del contribuyente inscrito en el RFC, del emisor del comprobante.
        /// </summary>
        [MaxLength(300)]
        public string Nombre {
            get { return this.nombre; }
            set { this.nombre = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="nombre">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para registrar el nombre, denominación o razón social del contribuyente inscrito en el RFC, del emisor del comprobante.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="300"/>
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:pattern value="[^|]{1,300}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>


        [ForeignKey("DomicilioFiscal")]
        public int? DomicilioFiscalId { get; set; }

        /// <summary>
        /// Nodo opcional para precisar la información de ubicación del domicilio fiscal del contribuyente emisor.
        /// </summary>
        /// <remarks>
        /// Antes era requerido
        /// <code>
        /// <xs:element name="DomicilioFiscal" type="cfdi:t_UbicacionFiscal" minOccurs="0">
        ///     <xs:annotation>
        ///         <xs:documentation>Nodo opcional para precisar la información de ubicación del domicilio fiscal del contribuyente emisor</xs:documentation>
        ///     </xs:annotation>
        /// </xs:element>
        /// </code>
        /// </remarks>
        //[XmlElement("DomicilioFiscal")]
        public virtual UbicacionFiscal DomicilioFiscal { get; set; }

        //[ForeignKey("ExpedidoEn")]
        //public int? ExpedidoEnId { get; set; }

        ///// <summary>
        ///// Nodo opcional para precisar la información de ubicación del domicilio en donde es emitido 
        ///// el comprobante fiscal en caso de que sea distinto del domicilio fiscal del contribuyente emisor.
        ///// </summary>
        ///// <remarks>
        ///// <code>
        ///// <xs:element name="ExpedidoEn" type="cfdi:t_Ubicacion" minOccurs="0">
        /////     <xs:annotation>
        /////         <xs:documentation>Nodo opcional para precisar la información de ubicación del domicilio en donde es emitido el comprobante fiscal en caso de que sea distinto del domicilio fiscal del contribuyente emisor.</xs:documentation>
        /////     </xs:annotation>
        ///// </xs:element>
        ///// </code>
        ///// </remarks>
        ////[XmlElement("ExpedidoEn")]
        ////public virtual Ubicacion ExpedidoEn { get; set; }
        public virtual List<Ubicacion> ExpedidoEn { get; set; }

        /// <summary>
        /// Nodo requerido para incorporar los regímenes en los que tributa el contribuyente emisor. Puede contener más de un régimen.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:sequence>
        ///   <xs:element name="RegimenFiscal" maxOccurs="unbounded">
        ///     <xs:annotation>
        ///       <xs:documentation>Nodo requerido para incorporar los regímenes en los que tributa el contribuyente emisor. Puede contener más de un régimen.</xs:documentation>
        ///     </xs:annotation>
        ///     <xs:complexType>
        ///       <xs:attribute name="Regimen" use="required">
        ///         <xs:annotation>
        ///           <xs:documentation>Atributo requerido para incorporar el nombre del régimen en el que tributa el contribuyente emisor.</xs:documentation>
        ///         </xs:annotation>
        ///         <xs:simpleType>
        ///           <xs:restriction base="xs:string">
        ///             <xs:minLength value="1"/>
        ///             <xs:whiteSpace value="collapse"/>
        ///           </xs:restriction>
        ///         </xs:simpleType>
        ///       </xs:attribute>
        ///     </xs:complexType>
        ///   </xs:element>
        /// </xs:sequence>        
        /// </code>
        /// </remarks>
        //[XmlElement("RegimenFiscal", IsNullable = false)]
        public virtual List<RegimenFiscal> RegimenFiscal { get; set; }

        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string CifUrl { get; set; }
        public string LogoUrl { get; set; }

        public virtual List<Certificado> Certificados { get; set; }

        [ForeignKey("ViewTemplate")]
        public int? ViewTemplateId { get; set; }
        public virtual ViewTemplate ViewTemplate { get; set; }

        //[XmlIgnore]
        public string Status { get; set; }
    }
}