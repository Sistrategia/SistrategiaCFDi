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
    /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
    /// </summary>
    public interface IEmisor
    {
        /// <summary>
        /// Atributo requerido para registrar la Clave del Registro Federal de Contribuyentes 
        /// correspondiente al contribuyente emisor del comprobante.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("Rfc")]
        //[Required]
        //[MaxLength(13)]
        string RFC { get; set; }
        // <xs:attribute name="Rfc" type="tdCFDI:t_RFC" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para registrar la Clave del Registro Federal de 
        //       Contribuyentes correspondiente al contribuyente emisor del comprobante.</xs:documentation>
        //      </xs:annotation>
        //    </xs:attribute>
        // ...
        // <xs:simpleType name="t_RFC">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Tipo definido para expresar claves del Registro Federal de Contribuyentes
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:restriction base="xs:string">
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:minLength value="12"/>
        //     <xs:maxLength value="13"/>        
        //     <xs:pattern value="[A-Z&amp;Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A]"/>
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>
        /// Atributo opcional para el nombre, denominación o razón social del contribuyente emisor del comprobante.
        /// </summary>
        /// <remarks>        
        /// Longitud Mínima: 1
        /// Longitud Máxima: 254
        /// Patrón <code>[^|]{1,254}</code>
        /// Colapsar espacios
        /// </remarks>
        [XmlAttribute("Nombre")]
        //[MaxLength(254)]
        string Nombre { get; set; }
        // <xs:attribute name="Nombre" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>Atributo opcional para registrar el nombre, denominación o razón social del 
        //       contribuyente emisor del comprobante.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //       <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="254"/>
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:pattern value="[^|]{1,254}"/>
        //       </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para incorporar la clave del régimen del contribuyente emisor al 
        /// que aplicará el efecto fiscal de este comprobante.
        /// </summary>
        /// <remarks>        
        /// Longitud Mínima: 1
        /// Colapsar espacios
        /// </remarks>
        [XmlAttribute("RegimenFiscal")]        
        string RegimenFiscal { get; set; }
        // <xs:attribute name="RegimenFiscal" use="required" type="catCFDI:c_RegimenFiscal">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para incorporar la clave del régimen del 
        //       contribuyente emisor al que aplicará el efecto fiscal de este comprobante.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        
    }
}
