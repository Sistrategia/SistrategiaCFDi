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
        private string claveProdServ;
        private string noIdentificacion;
        private decimal cantidad;
        private string claveUnidad;
        private string unidad;        
        private string descripcion;
        private decimal valorUnitario;
        private decimal importe;
        private decimal? descuento;
        private string motivoDescuento { get; set; }
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
        /// Atributo requerido para expresar la clave del producto o del servicio amparado por el 
        /// presente concepto. Es requerido y deben utilizar las claves del catálogo de productos 
        /// y servicios, cuando los conceptos que registren por sus actividades correspondan con 
        /// dichos conceptos.
        /// </summary>
        [XmlAttribute("ClaveProdServ")]
        public string ClaveProdServ {
            get { return this.claveProdServ; }
            set { this.claveProdServ = value; }
        }
        //<xs:attribute name="ClaveProdServ" use="required" type="catCFDI:c_ClaveProdServ">
        //    <xs:annotation>
        //    <xs:documentation>Atributo requerido para expresar la clave del producto o del servicio amparado por el presente concepto. Es requerido y deben utilizar las claves del catálogo de productos y servicios, cuando los conceptos que registren por sus actividades correspondan con dichos conceptos.</xs:documentation>
        //    </xs:annotation>
        //</xs:attribute>

        /// <summary>
        /// Atributo opcional para expresar el número de parte, identificador del producto o del servicio, 
        /// la clave de producto o servicio, SKU o equivalente, propia de la operación del emisor, amparado 
        /// por el presente concepto. Opcionalmente se puede utilizar claves del estándar GTIN.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("NoIdentificacion")] // Version 3.2: [XmlAttribute("noIdentificacion")]
        public string NoIdentificacion {
            get { return this.noIdentificacion; }
            set { this.noIdentificacion = value; }
        }
        // <xs:attribute name="NoIdentificacion" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para expresar el número de parte, identificador del producto o del servicio, la clave de producto o servicio, SKU o equivalente, propia de la operación del emisor, amparado por el presente concepto. Opcionalmente se puede utilizar claves del estándar GTIN.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="100"/>
        //       <xs:pattern value="[^|]{1,100}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para precisar la cantidad de bienes o servicios del tipo particular definido 
        /// por el presente concepto.
        /// </summary>
        /// <remarks>
        /// En este campo se debe registrar la cantidad de bienes o servicios que correspondan a 
        /// cada concepto, puede contener de cero hasta seis decimales.
        /// </remarks>
        [XmlAttribute("Cantidad")] // Version 3.2: [XmlAttribute("cantidad")]
        public decimal Cantidad {
            get { return this.cantidad; }
            set { this.cantidad = value; }
        }
        // <xs:attribute name="Cantidad" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para precisar la cantidad de bienes o servicios del tipo 
        //       particular definido por el presente concepto.</xs:documentation>
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
        /// Atributo requerido para precisar la clave de unidad de medida estandarizada aplicable para la 
        /// cantidad expresada en el concepto. La unidad debe corresponder con la descripción del concepto.
        /// </summary>
        [XmlAttribute("ClaveUnidad")]
        public string ClaveUnidad {
            get { return this.claveUnidad; }
            set { this.claveUnidad = value; }
        }
        // <xs:attribute name="ClaveUnidad" use="required" type="catCFDI:c_ClaveUnidad">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para precisar la clave de unidad de medida estandarizada 
        //       aplicable para la cantidad expresada en el concepto. La unidad debe corresponder con la 
        //       descripción del concepto.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional para precisar la unidad de medida propia de la operación del emisor, aplicable 
        /// para la cantidad expresada en el concepto. La unidad debe corresponder con la descripción del concepto.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("Unidad")] // Version 3.2: [XmlAttribute("unidad")]
        public string Unidad {
            get { return this.unidad; }
            set { this.unidad = value; }
        }
        // <xs:attribute name="Unidad" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para precisar la unidad de medida propia de la operación 
        //       del emisor, aplicable para la cantidad expresada en el concepto. La unidad debe corresponder 
        //       con la descripción del concepto.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="20"/>
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:pattern value="[^|]{1,20}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para precisar la descripción del bien o servicio cubierto por el presente concepto.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("Descripcion")] // Version 3.2: [XmlAttribute("descripcion")]
        public string Descripcion {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }
        // <xs:attribute name="Descripcion" use="required">
        //    <xs:annotation>
        //    <xs:documentation>Atributo requerido para precisar la descripción del bien o servicio cubierto por el presente concepto.</xs:documentation>
        //    </xs:annotation>
        //    <xs:simpleType>
        //    <xs:restriction base="xs:string">
        //        <xs:minLength value="1"/>
        //        <xs:maxLength value="1000"/>
        //        <xs:whiteSpace value="collapse"/>
        //        <xs:pattern value="[^|]{1,1000}"/>
        //    </xs:restriction>
        //    </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para precisar el valor o precio unitario del bien o servicio cubierto 
        /// por el presente concepto.
        /// </summary>
        /// <remarks>
        /// <para>
        /// En este campo se debe registrar el valor o precio unitario del bien o servicio por cada
        /// concepto, el cual puede contener de cero hasta seis decimales.
        /// </para>
        /// <para>
        /// Si el tipo de comprobante es de "I" (Ingreso), "E" (Egreso) o "N" (Nómina) este valor 
        /// debe ser mayor a cero y si es de "T" (Traslado) puede ser mayor o igual a cero y si es
        /// de "P" (Pago) debe ser igual a cero.
        /// </para>
        /// </remarks>
        [XmlAttribute("ValorUnitario")] // Version 3.2: [XmlAttribute("valorUnitario")]
        public decimal ValorUnitario {
            get { return this.valorUnitario; }
            set { this.valorUnitario = value; }
        }
        // <xs:attribute name="ValorUnitario" type="tdCFDI:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para precisar el valor o precio unitario del bien o 
        //       servicio cubierto por el presente concepto.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>


        /// <summary>
        /// Atributo requerido para precisar el importe total de los bienes o servicios del presente 
        /// concepto. Debe ser equivalente al resultado de multiplicar la cantidad por el valor unitario 
        /// expresado en el concepto. No se permiten valores negativos.
        /// </summary>
        /// <remarks>       
        /// <para>
        /// Se debe registrar el importe total de los bienes o servicios de cada concepto.
        /// Debe ser equivalente al resultado de multiplicar la cantidad por el valor unitario expresado
        /// en el concepto, el cual debe ser calculado por el sistema que genera el comprobante y 
        /// considerará los redondeos que tenga registrado este campo en el estándar técnico 
        /// del Anexo 20. No se permiten negativos.
        /// </para>
        /// </remarks>
        [XmlAttribute("Importe")] // Version 3.2: [XmlAttribute("importe")]
        public decimal Importe {
            get { return this.importe; }
            set { this.importe = value; }
        }
        // <xs:attribute name="Importe" type="tdCFDI:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para precisar el importe total de los bienes o 
        //       servicios del presente concepto. Debe ser equivalente al resultado de multiplicar la 
        //       cantidad por el valor unitario expresado en el concepto. No se permiten valores negativos. 
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional para representar el importe de los descuentos aplicables al concepto. 
        /// No se permiten valores negativos.
        /// </summary>
        [XmlAttribute("Descuento")]
        public decimal? Descuento {
            get { return this.descuento; }
            set { this.descuento = value; }
        }
        // <xs:attribute name="Descuento" type="tdCFDI:t_Importe" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para representar el importe de los descuentos aplicables 
        //       al concepto. No se permiten valores negativos.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional para expresar el motivo del descuento aplicable.
        /// </summary>
        public string MotivoDescuento {
            get { return this.motivoDescuento; }
            set { this.motivoDescuento = value; }
        }
        // <xs:attribute name="motivoDescuento" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para expresar el motivo del descuento aplicable.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        [XmlIgnore]
        public int Ordinal {
            get { return this.ordinal; }
            set { this.ordinal = value; }
        }

        [ForeignKey("Impuestos")]
        public int? ImpuestosId { get; set; }

        /// <summary>
        /// Nodo condicional para expresar el resumen de los impuestos aplicables.
        /// </summary>
        public virtual ConceptoImpuestos Impuestos { get; set; }
        //public Impuestos Impuestos {
        //    get { return this.impuestos; }
        //    set { this.impuestos = value; }
        //}

        ///// <summary>
        ///// Nodo condicional para expresar el resumen de los impuestos aplicables.
        ///// </summary>
        //[XmlArrayItemAttribute("Impuesto", IsNullable = false)]
        //public virtual List<ConceptoImpuestos> Impuestos { get; set; }
        ////public IList<ComprobanteConcepto> Conceptos {
        ////    get { return this.conceptos; }
        ////    //set { this.conceptos = value; }
        ////}
    }
}