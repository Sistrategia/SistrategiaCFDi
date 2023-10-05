﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    /// <summary>
    /// Para los comprobantes CFDI anteriores a la versión 3.3 se utilizaba el nodo ubicación para expresar la ubicación del emisor y del receptor.
    /// </summary>
    public abstract class UbicacionBase
    {
        #region Private fields
        protected string calle;
        protected string noExterior;
        protected string noInterior;
        protected string colonia;
        protected string localidad;
        protected string referencia;
        protected string municipio;
        protected string estado;
        protected string pais;
        protected string codigoPostal;
        protected string lugarExpedicion;
        #endregion

        protected UbicacionBase()
            : base() {
            this.PublicKey = Guid.NewGuid();
            //this.Status = "A";
        }

        [Key]
        public int UbicacionId { get; set; }

        [Required]
        public Guid PublicKey { get; set; }

        /// <summary>
        /// Este atributo opcional sirve para precisar la avenida, calle, camino o carretera donde se da la ubicación.
        /// </summary>
        [MaxLength(256)]
        [XmlAttribute("calle")]
        public virtual string Calle {
            get { return this.calle; }
            set { this.calle = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="calle" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Este atributo opcional sirve para precisar la avenida, calle, camino o carretera donde se da la ubicación.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Este atributo opcional sirve para expresar el número particular en donde se da la ubicación sobre una calle dada        
        /// </summary>
        [MaxLength(50)]
        [XmlAttribute("noExterior")]
        public string NoExterior {
            get { return this.noExterior; }
            set { this.noExterior = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="noExterior" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Este atributo opcional sirve para expresar el número particular en donde se da la ubicación sobre una calle dada.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Este atributo opcional sirve para expresar información adicional para especificar la ubicación 
        /// cuando calle y número exterior (noExterior) no resulten suficientes para determinar 
        /// la ubicación de forma precisa.
        /// </summary>
        [MaxLength(50)]
        [XmlAttribute("noInterior")]
        public string NoInterior {
            get { return this.noInterior; }
            set { this.noInterior = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="noInterior" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Este atributo opcional sirve para expresar información adicional para especificar la ubicación cuando calle y número exterior (noExterior) no resulten suficientes para determinar la ubicación de forma precisa.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Este atributo opcional sirve para precisar la colonia en donde se da la ubicación cuando se desea ser más específico en casos de ubicaciones urbanas.
        /// </summary>        
        [MaxLength(50)]
        [XmlAttribute("colonia")]
        public string Colonia {
            get { return this.colonia; }
            set { this.colonia = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="colonia" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Este atributo opcional sirve para precisar la colonia en donde se da la ubicación cuando se desea ser más específico en casos de ubicaciones urbanas.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional que sirve para precisar la ciudad o población donde se da la ubicación.
        /// </summary>
        [MaxLength(50)]
        [XmlAttribute("localidad")]
        public string Localidad {
            get { return this.localidad; }
            set { this.localidad = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="localidad" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional que sirve para precisar la ciudad o población donde se da la ubicación.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional para expresar una referencia de ubicación adicional.
        /// </summary>
        [MaxLength(256)]
        [XmlAttribute("referencia")]
        public string Referencia {
            get { return this.referencia; }
            set { this.referencia = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="referencia" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para expresar una referencia de ubicación adicional.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional que sirve para precisar el municipio o delegación (en el caso del Distrito Federal) 
        /// en donde se da la ubicación.
        /// </summary>
        [MaxLength(50)]
        [XmlAttribute("municipio")]
        public virtual string Municipio {
            get { return this.municipio; }
            set { this.municipio = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="municipio" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional que sirve para precisar el municipio o delegación (en el caso del Distrito Federal) en donde se da la ubicación.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional que sirve para precisar el estado o entidad federativa donde se da la ubicación.
        /// </summary>
        [MaxLength(50)]
        [XmlAttribute("estado")]
        public virtual string Estado {
            get { return this.estado; }
            set { this.estado = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="estado" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional que sirve para precisar el estado o entidad federativa donde se da la ubicación.</xs:documentation>
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
        public virtual string Pais {
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
        /// Atributo opcional que sirve para asentar el código postal en donde se da la ubicación.
        /// </summary>
        [MaxLength(5)]
        [XmlAttribute("codigoPostal")]
        public virtual string CodigoPostal {
            get { return this.codigoPostal; }
            set { this.codigoPostal = SATManager.NormalizeWhiteSpace(value); }
        }
        // <xs:attribute name="codigoPostal" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional que sirve para asentar el código postal en donde se da la ubicación.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        [MaxLength(2048)]
        [XmlIgnore]
        public virtual string LugarExpedicion {
            get { return this.lugarExpedicion; }
            set { this.lugarExpedicion = SATManager.NormalizeWhiteSpace(value); }
        }

        [XmlIgnore]
        public int Ordinal { get; set; }
        //    get { return this.ordinal; }
        //    set { this.ordinal = value; }
        //}
    }

    public class Ubicacion : UbicacionBase
    {
        public Ubicacion()
            : base() {
            //this.PublicKey = Guid.NewGuid();
            //this.Status = "A";
        }

        /// <summary>
        /// Atributo requerido que sirve para precisar el país donde se da la ubicación.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="pais" use="required">
        ///     <xs:annotation>
        ///         <xs:documentation>Atributo requerido que sirve para precisar el país donde se da la ubicación.</xs:documentation>
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
        [MaxLength(50)]
        [XmlAttribute("pais")]
        public virtual string Pais {
            get { return this.pais; }
            set { this.pais = SATManager.NormalizeWhiteSpace(value); }
        }        

        //[MaxLength(2048)]
        //[XmlIgnore]
        //public virtual string LugarExpedicion {
        //    get { return this.lugarExpedicion; }
        //    set { this.lugarExpedicion = SATManager.NormalizeWhiteSpace(value); }
        //}

        //[XmlIgnore]
        //public int Ordinal { get; set; }
        ////    get { return this.ordinal; }
        ////    set { this.ordinal = value; }
        ////}

        ////[XmlIgnore]
        ////public string Status { get; set; }
    }
}