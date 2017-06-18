/*************************************************************************************************************
* CFDI.v33.IEmisor.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
* Copyright (C) 2017 Sistrategia.
* 
* Contributor(s):	Antonio Mendez Granda, antonio.mendez@sistrategia.com
*					Jocelyn Xanat Ledesma Herrera, jocelyn@sistrategia.com
*					J. Ernesto Ocampo Cicero, ernesto@sistrategia.com
* Last Update:		2017-Jun-14
* Created:			2010-Sep-08
* Version:			1.6.1707.1
*************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sistrategia.SAT.CFDiWebSite.CFDI.v33
{
    /// <summary>
    /// Nodo requerido para registrar la información detallada de un bien o servicio amparado 
    /// en el comprobante.
    /// </summary>
    public interface IConcepto
    {
        /// <summary>
        /// Atributo requerido para expresar la clave del producto o del servicio amparado por el 
        /// presente concepto. Es requerido y deben utilizar las claves del catálogo de productos 
        /// y servicios, cuando los conceptos que registren por sus actividades correspondan con 
        /// dichos conceptos.
        /// </summary>
        [XmlAttribute("ClaveProdServ")]
        string ClaveProdServ { get; set; }
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
        [XmlAttribute("NoIdentificacion")]
        string NoIdentificacion { get; set; }
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
        /// </remarks>
        [XmlAttribute("Cantidad")]
        decimal Cantidad { get; set; }
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
        string ClaveUnidad { get; set; }
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
        [XmlAttribute("Unidad")]
        string Unidad { get; set; }
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
        [XmlAttribute("Descripcion")]
        string Descripcion { get; set; }
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
        /// </remarks>
        [XmlAttribute("ValorUnitario")]
        decimal ValorUnitario { get; set; }
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
        /// </remarks>
        [XmlAttribute("Importe")]
        decimal Importe { get; set; }
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
        decimal Descuento { get; set; }
        // <xs:attribute name="Descuento" type="tdCFDI:t_Importe" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para representar el importe de los descuentos aplicables 
        //       al concepto. No se permiten valores negativos.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
    }
}
