/*************************************************************************************************************
* CFDI.v33.IReceptor.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
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
    /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
    /// </summary>
    public interface IReceptor
    {
        /// <summary>
        /// Atributo requerido para precisar la Clave del Registro Federal de Contribuyentes correspondiente 
        /// al contribuyente receptor del comprobante.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("Rfc")]
        string RFC { get; set; }
        // <xs:attribute name="Rfc" use="required"  type="tdCFDI:t_RFC">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para precisar la Clave del Registro Federal de 
        //       Contribuyentes correspondiente al contribuyente receptor del comprobante.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo opcional para precisar el nombre, denominación o razón social del contribuyente receptor 
        /// del comprobante.
        /// </summary>
        /// <remarks>       
        /// </remarks>
        [XmlAttribute("Nombre")]
        string Nombre { get; set; }
        // <xs:attribute name="Nombre" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para precisar el nombre, denominación o razón social del 
        //       contribuyente receptor del comprobante.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="254"/>
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:pattern value="[^|]{1,254}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo condicional para registrar la clave del país de residencia para efectos fiscales 
        /// del receptor del comprobante, cuando se trate de un extranjero, y que es conforme con 
        /// la especificación ISO 3166-1 alpha-3. Es requerido cuando se incluya el complemento de 
        /// comercio exterior o se registre el atributo NumRegIdTrib.
        /// receptor del comprobante.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("ResidenciaFiscal")]
        string ResidenciaFiscal { get; set; }
        // <xs:attribute name="ResidenciaFiscal" use="optional" type="catCFDI:c_Pais">
        //   <xs:annotation>
        //     <xs:documentation>Atributo condicional para registrar la clave del país de residencia para 
        //       efectos fiscales del receptor del comprobante, cuando se trate de un extranjero, y que 
        //       es conforme con la especificación ISO 3166-1 alpha-3. Es requerido cuando se incluya el 
        //       complemento de comercio exterior o se registre el atributo NumRegIdTrib.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo condicional para expresar el número de registro de identidad fiscal del receptor 
        /// cuando sea residente en el  extranjero. Es requerido cuando se incluya el complemento de 
        /// comercio exterior.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("NumRegIdTrib")]
        string NumRegIdTrib { get; set; }
        // <xs:attribute name="NumRegIdTrib" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo condicional para expresar el número de registro de identidad 
        //       fiscal del receptor cuando sea residente en el  extranjero. Es requerido cuando se incluya 
        //       el complemento de comercio exterior.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="40"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para expresar la clave del uso que dará a esta factura el receptor del CFDI.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("UsoCFDI")]
        string UsoCFDI { get; set; }
        // <xs:attribute name="UsoCFDI" use="required" type="catCFDI:c_UsoCFDI">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para expresar la clave del uso que dará a esta 
        //       factura el receptor del CFDI.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="40"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>
       
    }
}