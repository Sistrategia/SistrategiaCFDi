/*************************************************************************************************************
* IEmisor.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
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
    /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.    
    /// </summary> 
    public interface IEmisor
    {
        /// <summary>
        /// Atributo requerido para la Clave del Registro Federal de Contribuyentes correspondiente al 
        /// contribuyente emisor del comprobante sin guiones o espacios.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("rfc")]
        //[Required]
        //[MaxLength(13)]
        string RFC { get; set; }
        // <xs:attribute name="rfc" type="cfdi:t_RFC" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para la Clave del Registro Federal de Contribuyentes 
        //       correspondiente al contribuyente emisor del comprobante sin guiones o espacios.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
        // </code>
        // <code>
        // <xs:simpleType name="t_RFC">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Tipo definido para expresar claves del Registro Federal de Contribuyentes
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:restriction base="xs:string">
        //     <xs:minLength value="12"/>
        //     <xs:maxLength value="13"/>
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:pattern value="[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?"/>
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>
        /// Atributo opcional para el nombre, denominación o razón social del contribuyente emisor 
        /// del comprobante.
        /// </summary>
        /// <remarks>        
        /// Longitud Mínima: 1
        /// Colapsar espacios
        /// </remarks>
        [XmlAttribute("nombre")]
        //[MaxLength(256)]
        string Nombre { get; set; }
        // <xs:attribute name="nombre">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo opcional para el nombre, denominación o razón social del contribuyente emisor 
        //       del comprobante.
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
        /// Nodo opcional para precisar la información de ubicación del domicilio fiscal del 
        /// contribuyente emisor.
        /// </summary>
        /// <remarks>
        /// Antes era requerido        
        /// </remarks>
        [XmlElement("DomicilioFiscal")]
        UbicacionFiscal DomicilioFiscal { get; set; }
        // <xs:element name="DomicilioFiscal" type="cfdi:t_UbicacionFiscal" minOccurs="0">
        //   <xs:annotation>
        //     <xs:documentation>Nodo opcional para precisar la información de ubicación del domicilio 
        //       fiscal del contribuyente emisor</xs:documentation>
        //   </xs:annotation>
        // </xs:element>

        /// <summary>
        /// Nodo opcional para precisar la información de ubicación del domicilio en donde es emitido 
        /// el comprobante fiscal en caso de que sea distinto del domicilio fiscal del contribuyente emisor.
        /// </summary>
        [XmlElement("ExpedidoEn")]
        Ubicacion ExpedidoEn { get; set; }
        // <xs:element name="ExpedidoEn" type="cfdi:t_Ubicacion" minOccurs="0">
        //   <xs:annotation>
        //     <xs:documentation>Nodo opcional para precisar la información de ubicación del domicilio 
        //       en donde es emitido el comprobante fiscal en caso de que sea distinto del domicilio 
        //       fiscal del contribuyente emisor.</xs:documentation>
        //   </xs:annotation>
        // </xs:element>

        /// <summary>
        /// Nodo requerido para incorporar los regímenes en los que tributa el contribuyente emisor. 
        /// Puede contener más de un régimen.
        /// </summary>
        [XmlElementAttribute("RegimenFiscal", IsNullable = false)]        
        ComprobanteEmisorRegimenFiscal[] RegimenFiscal { get; set; }
        // <xs:sequence>
        //   <xs:element name="RegimenFiscal" maxOccurs="unbounded">
        //     <xs:annotation>
        //       <xs:documentation>Nodo requerido para incorporar los regímenes en los que tributa el 
        //         contribuyente emisor. Puede contener más de un régimen.</xs:documentation>
        //     </xs:annotation>
        //     <xs:complexType>
        //       <xs:attribute name="Regimen" use="required">
        //         <xs:annotation>
        //           <xs:documentation>Atributo requerido para incorporar el nombre del régimen en el 
        //             que tributa el contribuyente emisor.</xs:documentation>
        //         </xs:annotation>
        //         <xs:simpleType>
        //           <xs:restriction base="xs:string">
        //             <xs:minLength value="1"/>
        //             <xs:whiteSpace value="collapse"/>
        //           </xs:restriction>
        //         </xs:simpleType>
        //       </xs:attribute>
        //     </xs:complexType>
        //   </xs:element>
        // </xs:sequence>
    }
}
