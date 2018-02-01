/*************************************************************************************************************
* CFDI.v32.IReceptor.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
* Copyright (C) 2017 Sistrategia.
* 
* Contributor(s):	J. Ernesto Ocampo Cicero, ernesto@sistrategia.com
* Last Update:		2016-Jul-15
* Created:			2010-Sep-08
* Version:			1.6.1607.15
*************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sistrategia.SAT.CFDiWebSite.CFDI.v32
{
    /// <summary>
    /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
    /// </summary>
    public interface IReceptor
    {
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
        string RFC { get; set; }

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
        string Nombre { get; set; }

        // ResidenciaFiscal="MEX" 

        // NumRegIdTrib="0000000000000" 

        // UsoCFDI="G01"

        /// <summary>
        /// Nodo opcional para la definición de la ubicación donde se da el domicilio del receptor 
        /// del comprobante fiscal.
        /// </summary>     
        [XmlElement("Domicilio")]
        Ubicacion Domicilio { get; set; }
        // <code>
        // <xs:sequence>
        //   <xs:element name="Domicilio" type="cfdi:t_Ubicacion" minOccurs="0">
        //     <xs:annotation>
        //       <xs:documentation>
        //         Nodo opcional para la definición de la ubicación donde se da el domicilio 
        //         del receptor del comprobante fiscal.
        //       </xs:documentation>
        //     </xs:annotation>
        //   </xs:element>
        // </xs:sequence>
        // </code>
    }
}
