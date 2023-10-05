using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class UbicacionFiscal : UbicacionBase
    {
        public UbicacionFiscal()
            : base() {
            //this.PublicKey = Guid.NewGuid();
        }

        #region Private fields
        //private string calle;
        ////private string noExterior;
        ////private string noInterior;
        ////private string colonia;
        ////private string localidad;
        ////private string referencia;
        //private string municipio;
        //private string estado;
        ////private string pais;
        //private string codigoPostal;
        #endregion

        //[Key]
        //public int UbicacionId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        /// <summary>
        /// Este atributo requerido sirve para precisar la avenida, calle, camino o carretera donde se da la ubicación.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="calle" use="required">
        ///     <xs:annotation>
        ///         <xs:documentation>Este atributo requerido sirve para precisar la avenida, calle, camino o carretera donde se da la ubicación.</xs:documentation>
        ///     </xs:annotation>
        ///     <xs:simpleType>
        ///         <xs:restriction base="xs:string">
        ///             <xs:minLength value="1"/>
        ///             <xs:whiteSpace value="collapse"/>
        ///         </xs:restriction>
        ///     </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [Required]
        [MaxLength(256)]
        [XmlAttribute("calle")]
        public override string Calle {
            get { return this.calle; }
            set { this.calle = SATManager.NormalizeWhiteSpace(value); }
        }        

        /// <summary>
        /// Atributo requerido que sirve para precisar el municipio o delegación (en el caso del Distrito Federal) en donde se da la ubicación.
        /// </summary>
        [Required]
        [MaxLength(50)]
        [XmlAttribute("municipio")]
        public override string Municipio {
            get { return this.municipio; }
            set { this.municipio = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="municipio" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido que sirve para precisar el municipio o delegación (en el caso del Distrito Federal) en donde se da la ubicación.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido que sirve para precisar el estado o entidad federativa donde se da la ubicación.
        /// </summary>
        [Required]
        [MaxLength(50)]
        [XmlAttribute("estado")]
        public override string Estado {
            get { return this.estado; }
            set { this.estado = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="estado" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido que sirve para precisar el estado o entidad federativa donde se da la ubicación.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido que sirve para precisar el país donde se da la ubicación.
        /// </summary>
        [Required]
        [MaxLength(50)]
        [XmlAttribute("pais")]
        public override string Pais {
            get { return this.pais; }
            set { this.pais = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="pais" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido que sirve para precisar el país donde se da la ubicación.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido que sirve para asentar el código postal en donde se da la ubicación.
        /// </summary>
        [Required]
        [MaxLength(5)]
        [XmlAttribute("codigoPostal")]
        public override string CodigoPostal {
            get { return this.codigoPostal; }
            set { this.codigoPostal = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="codigoPostal" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido que sirve para asentar el código postal en donde se da la ubicación.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>
    }
}