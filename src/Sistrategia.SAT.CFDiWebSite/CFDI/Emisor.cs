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
        /// Atributo requerido para la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente emisor del comprobante sin guiones o espacios.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="rfc" type="cfdi:t_RFC" use="required">
        ///     <xs:annotation>
        ///         <xs:documentation>Atributo requerido para la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente emisor del comprobante sin guiones o espacios.</xs:documentation>
        ///     </xs:annotation>
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
        [Required]
        [MaxLength(13)]
        public string RFC { get; set; }

        /// <summary>
        /// Atributo opcional para el nombre, denominación o razón social del contribuyente emisor del comprobante.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="nombre">
        ///     <xs:annotation>
        ///         <xs:documentation>Atributo opcional para el nombre, denominación o razón social del contribuyente emisor del comprobante.</xs:documentation>
        ///     </xs:annotation>
        ///     <xs:simpleType>
        ///         <xs:restriction base="xs:string">
        ///             <xs:minLength value="1"/>
        ///             <xs:whiteSpace value="collapse"/>
        ///         </xs:restriction>
        ///     </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// <xs:minLength value="1"/>
        /// <xs:whiteSpace value="collapse"/>
        /// </remarks>
        [MaxLength(256)]
        public string Nombre {
            get { return this.nombre; }
            set { this.nombre = SATManager.NormalizeWhiteSpace(value); }
        }


        [ForeignKey("DomicilioFiscal")]
        public int? DomicilioFiscalId { get; set; }
        public virtual UbicacionFiscal DomicilioFiscal { get; set; }

        [ForeignKey("ExpedidoEn")]
        public int? ExpedidoEnId { get; set; }
        public virtual UbicacionFiscal ExpedidoEn { get; set; }


    }
}