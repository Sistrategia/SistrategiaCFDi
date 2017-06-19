/*************************************************************************************************************
* ICfdiRelacionado.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
* Copyright (C) 2017 Sistrategia.
* 
* Contributor(s):	Antonio Mendez Granda, antonio.mendez@sistrategia.com
*					Jocelyn Xanat Ledesma Herrera, jocelyn@sistrategia.com
*					J. Ernesto Ocampo Cicero, ernesto@sistrategia.com
* Last Update:		2017-Jun-14
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

namespace Sistrategia.SAT.CFDiWebSite.CFDI.v33
{
    /// <summary>
    /// Nodo requerido para precisar la información de los comprobantes relacionados.
    /// </summary>
    public interface ICfdiRelacionado
    {
        /// <summary>
        /// Atributo requerido para registrar el folio fiscal (UUID) de un CFDI relacionado 
        /// con el presente comprobante, por ejemplo: Si el CFDI relacionado es un comprobante 
        /// de traslado que sirve para registrar el movimiento de la mercancía. Si este comprobante 
        /// se usa como nota de crédito o nota de débito del comprobante relacionado. Si este 
        /// comprobante es una devolución sobre el comprobante relacionado. Si éste sustituye a una 
        /// factura cancelada.
        /// </summary>
        [XmlAttribute("UUID")]
        string UUID { get; set; }
        // <xs:attribute name="UUID" use="required">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para registrar el folio fiscal (UUID) de un CFDI
        //       relacionado con el presente comprobante, por ejemplo: Si el CFDI relacionado es un 
        //       comprobante de traslado que sirve para registrar el movimiento de la mercancía. Si 
        //       este comprobante se usa como nota de crédito o nota de débito del comprobante relacionado. 
        //       Si este comprobante es una devolución sobre el comprobante relacionado. Si éste sustituye 
        //       a una factura cancelada.</xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:length value="36"/>
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:pattern value="[a-f0-9A-F]{8}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{12}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

    }
    // <xs:element name="CfdiRelacionado" minOccurs="1" maxOccurs="unbounded">
    //   <xs:annotation>
    //     <xs:documentation>Nodo requerido para precisar la información de los comprobantes 
    //      relacionados.</xs:documentation>
    //   </xs:annotation>
    //   <xs:complexType>
    //     <xs:attribute name="UUID" use="required">
    //       <xs:annotation>
    //         <xs:documentation>Atributo requerido para registrar el folio fiscal (UUID) de un CFDI 
    //           relacionado con el presente comprobante, por ejemplo: Si el CFDI relacionado es un 
    //           comprobante de traslado que sirve para registrar el movimiento de la mercancía. Si 
    //           este comprobante se usa como nota de crédito o nota de débito del comprobante relacionado. 
    //           Si este comprobante es una devolución sobre el comprobante relacionado. Si éste sustituye 
    //           a una factura cancelada.</xs:documentation>
    //       </xs:annotation>
    //       <xs:simpleType>
    //         <xs:restriction base="xs:string">
    //           <xs:length value="36"/>
    //           <xs:whiteSpace value="collapse"/>
    //           <xs:pattern value="[a-f0-9A-F]{8}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{12}"/>
    //         </xs:restriction>
    //       </xs:simpleType>
    //     </xs:attribute>
    //   </xs:complexType>
    // </xs:element>
}
