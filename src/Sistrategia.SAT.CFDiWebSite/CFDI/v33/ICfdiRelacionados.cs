/*************************************************************************************************************
* ICfdiRelacionados.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
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
    /// Nodo opcional para precisar la información de los comprobantes relacionados.
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    public interface ICfdiRelacionados
    {
        /// <summary>
        /// Nodo condicional para capturar los impuestos retenidos aplicables. Es requerido cuando 
        /// en los conceptos se registre algún impuesto retenido.
        /// </summary>
        [XmlArray("CfdiRelacionados", IsNullable = false)]
        [XmlArrayItem("CfdiRelacionado", IsNullable = false)]
        IEnumerable<ICfdiRelacionado> Items { get; set; }

        /// <summary>
        /// Atributo requerido para indicar la clave de la relación que existe entre éste que se 
        /// esta generando y el o los CFDI previos.
        /// </summary>
        [XmlAttribute("TipoRelacion")]
        string UUID { get; set; }
        // <xs:attribute name="TipoRelacion" use="required" type="catCFDI:c_TipoRelacion">
        //   <xs:annotation>
        //     <xs:documentation>Atributo requerido para indicar la clave de la relación que existe 
        //       entre éste que se esta generando y el o los CFDI previos.</xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
    }
}
