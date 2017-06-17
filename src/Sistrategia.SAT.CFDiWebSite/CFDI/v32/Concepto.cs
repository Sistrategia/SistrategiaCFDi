/*************************************************************************************************************
* IComprobante.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
* Copyright (C) 2017 Sistrategia.
* 
* Contributor(s):	J. Ernesto Ocampo Cicero, ernesto@sistrategia.com
* Last Update:		2016-May-27
* Created:			2010-Sep-08
* Version:			1.6.1707.1
*************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sistrategia.SAT.CFDiWebSite.CFDI.v32
{
    public interface Concepto
    {
        ///// <summary>
        ///// ComplementoConcepto: Nodo opcional donde se incluirán los nodos complementarios de extensión al concepto, definidos por el SAT, de acuerdo a disposiciones particulares a un sector o actividad especifica.
        ///// CuentaPredial: Nodo opcional para asentar el número de cuenta predial con el que fue registrado el inmueble, en el sistema catastral de la entidad federativa de que trate, o bien para incorporar los datos de identificación del certificado de participación inmobiliaria no amortizable.
        ///// InformacionAduanera: Nodo opcional para introducir la información aduanera aplicable cuando se trate de ventas de primera mano de mercancías importadas.
        ///// Parte: Nodo opcional para expresar las partes o componentes que integran la totalidad del concepto expresado en el comprobante fiscal digital a través de Internet.
        ///// </summary>        
        //[XmlElementAttribute("ComplementoConcepto", typeof(ComprobanteConceptoComplementoConcepto))]
        //[XmlElementAttribute("CuentaPredial", typeof(ComprobanteConceptoCuentaPredial))]
        //[XmlElementAttribute("InformacionAduanera", typeof(t_InformacionAduanera))]
        //[XmlElementAttribute("Parte", typeof(ComprobanteConceptoParte))]
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
        /// Atributo requerido para precisar la cantidad de bienes o servicios del tipo particular definido 
        /// por el presente concepto.
        /// </summary>
        [XmlAttribute("cantidad")]
        decimal Cantidad { get; set; }
        // <xs:attribute name="cantidad" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para precisar la cantidad de bienes o servicios del tipo particular 
        //       definido por el presente concepto.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:decimal">
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para precisar la unidad de medida aplicable para la cantidad expresada en el concepto.
        /// </summary>
        /// <remarks>
        /// Antes era opcional.        
        /// </remarks>
        [XmlAttribute("unidad")]
        string Unidad { get; set; }
        // <xs:attribute name="unidad" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para precisar la unidad de medida aplicable para la cantidad expresada 
        //       en el concepto.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:minLength value="1"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional para expresar el número de serie del bien o identificador del servicio 
        /// amparado por el presente concepto.
        /// </summary>
        [XmlAttribute("noIdentificacion")]
        string NoIdentificacion { get; set; }
        // <xs:attribute name="noIdentificacion" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo opcional para expresar el número de serie del bien o identificador del 
        //       servicio amparado por el presente concepto.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para precisar la descripción del bien o servicio cubierto por el presente concepto.
        /// </summary>
        [XmlAttribute("descripcion")]
        string Descripcion { get; set; }
        // <xs:attribute name="descripcion" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para precisar la descripción del bien o servicio cubierto por el presente concepto.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para precisar el valor o precio unitario del bien o servicio cubierto por el 
        /// presente concepto.
        /// </summary>
        [XmlAttribute("valorUnitario")]
        decimal ValorUnitario { get; set; }
        // <xs:attribute name="valorUnitario" type="cfdi:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para precisar el valor o precio unitario del bien o servicio cubierto
        //       por el presente concepto.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para precisar el importe total de los bienes o servicios del presente concepto.
        /// Debe ser equivalente al resultado de multiplicar la cantidad por el valor unitario expresado 
        /// en el concepto.
        /// </summary>        
        [XmlAttribute("importe")]
        decimal Importe { get; set; }
        // <code>
        // <xs:attribute name="importe" type="cfdi:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para precisar el importe total de los bienes o servicios del presente concepto. 
        //       Debe ser equivalente al resultado de multiplicar la cantidad por el valor unitario expresado en el concepto.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
        // </code>

        //[XmlIgnore]
        //int Ordinal { get; set; }        
    }
}
