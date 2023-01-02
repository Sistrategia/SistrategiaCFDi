/*************************************************************************************************************
* IComprobante.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
* Copyright (C) 2010-2023 Sistrategia.
* 
* Contributor(s):	J. Ernesto Ocampo Cicero, ernesto@sistrategia.com
*					Antonio Mendez Granda, antonio.mendez@sistrategia.com
*					Jocelyn Xanat Ledesma Herrera, jocelyn@sistrategia.com
* Last Update:		2023-Jan-02
* Created:			2010-Sep-08
* Version:			1.6.1707.1
* 
* Notas: Los atributos tienen en comentarios naturales al final la descripción del xsd.
*        No fue posible agregar la descripción en el nodo <code> de la documentación xml
*        porque marca error y no genera la documentación del intellisense
*************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sistrategia.SAT.CFDiWebSite.CFDI.v40
{
    /// <summary>
    /// Estándar de Comprobante Fiscal Digital por Internet.
    /// </summary>
    public interface IComprobante
    {
        /// <summary>
        /// Atributo requerido con valor prefijado a 4.0 que indica la versión del estándar bajo el que se 
        /// encuentra expresado el comprobante. 
        /// </summary>
        /// <remarks>
        /// Requerido con valor prefijado a 4.0
        /// No debe contener espacios en blanco        
        /// </remarks>
        [XmlAttribute("Version")]
        string Version { get; set; }
        // <xs:attribute name="Version" use="required" fixed="4.0">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido con valor prefijado a 4.0 que indica la versión del estándar bajo el que 
        //       se encuentra expresado el comprobante.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>        
        /// Atributo opcional para precisar la serie para control interno del contribuyente. 
        /// Este atributo acepta una cadena de caracteres.        
        /// </summary>
        /// <remarks>
        /// Opcional
        /// El largo debe estar entre 1 y 25 caracteres
        /// No debe contener espacios en blanco                
        /// </remarks>
        [XmlAttribute("Serie")]
        string Serie { get; set; }
        // <xs:attribute name="Serie" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para precisar la serie para control interno del 
        //     contribuyente. Este atributo acepta una cadena de caracteres.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="25"/>
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:pattern value="[^|]{1,25}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional para control interno del contribuyente que expresa el folio del comprobante, 
        /// acepta una cadena de caracteres.        
        /// </summary>
        /// <remarks>
        /// opcional
        /// El largo debe estar entre 1 y 40 caracteres
        /// No debe contener espacios en blanco                
        /// </remarks>
        [XmlAttribute("Folio")]
        string Folio { get; set; }
        // <xs:attribute name="Folio" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para control interno del contribuyente que expresa el 
        //     folio del comprobante, acepta una cadena de caracteres.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="40"/>
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:pattern value="[^|]{1,40}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para la expresión de la fecha y hora de expedición del Comprobante Fiscal 
        /// Digital por Internet. Se expresa en la forma AAAA-MM-DDThh:mm:ss y debe corresponder con la hora 
        /// local donde se expide el comprobante.        
        /// </summary>
        /// <remarks>
        /// Requerido
        /// Fecha y hora de expedición del comprobante fiscal
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlAttribute("Fecha")]
        DateTime Fecha { get; set; }
        // <xs:attribute name="Fecha" use="required" type="tdCFDI:t_FechaH">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para la expresión de la fecha y hora de expedición del 
        //     Comprobante Fiscal Digital por Internet. Se expresa en la forma AAAA-MM-DDThh:mm:ss y debe 
        //     corresponder con la hora local donde se expide el comprobante.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
        // ...
        // <xs:simpleType name="t_FechaH">
        //   <xs:annotation>
        //     <xs:documentation>Tipo definido para la expresión de la fecha y hora. Se expresa en la forma 
        //     AAAA-MM-DDThh:mm:ss</xs:documentation>
        //   </xs:annotation>
        //   <xs:restriction base="xs:dateTime">
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:pattern value="(20[1-9][0-9])-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])T(([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9])"/>
        //   </xs:restriction>
        // </xs:simpleType>
    }

    /// <summary>
    /// Atributo requerido para contener el sello digital del comprobante fiscal, al que hacen referencia
    /// las reglas de resolución miscelánea vigente. El sello debe ser expresado como una cadena de texto
    /// en formato Base 64.
    /// </summary>
    /// <remarks>
    /// Requerido
    /// El sello deberá ser expresado cómo una cadena de texto en formato Base 64.
    /// No debe contener espacios en blanco        
    /// </remarks>
    [XmlAttribute("Sello")]
    string Sello { get; set; }
    // <xs:attribute name="Sello" use="required">
    //   <xs:annotation>
    //     <xs:documentation>Atributo requerido para contener el sello digital del comprobante fiscal, 
    //     al que hacen referencia las reglas de resolución miscelánea vigente. El sello debe ser 
    //     expresado como una cadena de texto en formato Base 64.</xs:documentation>
    //   </xs:annotation>
    //   <xs:simpleType>
    //     <xs:restriction base="xs:string">
    //       <xs:whiteSpace value="collapse"/>
    //     </xs:restriction>
    //   </xs:simpleType>
    // </xs:attribute>

    /// <summary>
    /// Atributo condicional para expresar la clave de la forma de pago de los bienes o servicios 
    /// amparados por el comprobante.
    /// </summary>
    /// <remarks>
    /// Requerido
    /// No debe contener espacios en blanco        
    /// </remarks>
    [XmlAttribute("FormaPago")]
    string FormaPago { get; set; }
    // <xs:attribute name="FormaPago" use="optional" type="catCFDI:c_FormaPago">
    //   <xs:annotation>
    //     <xs:documentation>
    //       Atributo condicional para expresar la clave de la forma de pago de los bienes o servicios 
    //       amparados por el comprobante.
    //     </xs:documentation>
    //   </xs:annotation>
    // </xs:attribute>
    // ...
    // <xs:simpleType name="c_FormaPago">
    //   <xs:restriction base="xs:string">
    //     <xs:whiteSpace value="collapse"/>
    //     <xs:enumeration value="01"/>
    //     <xs:enumeration value="02"/>
    //     ...
    //     <xs:enumeration value="31"/>
    //     <xs:enumeration value="99"/>
    //   </xs:restriction>
    // </xs:simpleType>

    /// <summary>
    /// Atributo requerido para expresar el número de serie del certificado de sello digital que 
    /// ampara al comprobante, de acuerdo con el acuse correspondiente a 20 posiciones otorgado 
    /// por el sistema del SAT.
    /// </summary>
    /// <remarks>
    /// Requerido
    /// No debe contener espacios en blanco
    /// El largo debe estar a 20 caracteres        
    /// </remarks>
    [XmlAttribute("NoCertificado")]
    string NoCertificado { get; set; }
    // <xs:attribute name="NoCertificado" use="required">
    //   <xs:annotation>
    //     <xs:documentation>
    //       Atributo requerido para expresar el número de serie del certificado de sello digital
    //       que ampara al comprobante, de acuerdo con el acuse correspondiente a 20 posiciones 
    //       otorgado por el sistema del SAT.
    //     </xs:documentation>
    //   </xs:annotation>
    //   <xs:simpleType>
    //     <xs:restriction base="xs:string">
    //       <xs:length value="20"/>
    //       <xs:whiteSpace value="collapse"/>
    //       <xs:pattern value="[0-9]{20}"/>
    //     </xs:restriction>
    //   </xs:simpleType>
    // </xs:attribute>
}