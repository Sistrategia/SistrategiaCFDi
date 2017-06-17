/*************************************************************************************************************
* IComprobante.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
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
    public interface IComprobante
    {
        /// <summary>
        /// Atributo requerido con valor prefijado a 3.3 que indica la versión del estándar bajo el que se 
        /// encuentra expresado el comprobante.
        /// </summary>
        /// <remarks>
        /// Requerido con valor prefijado a 3.3
        /// No debe contener espacios en blanco        
        /// </remarks>
        [XmlAttribute("Version")]
        string Version { get; set; }
        // <xs:attribute name="Version" use="required" fixed="3.3">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido con valor prefijado a 3.3 que indica la versión del estándar bajo el que 
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
        /// amparados por el comprobante. Si no se conoce la forma de pago este atributo se debe omitir.
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
        //       amparados por el comprobante. Si no se conoce la forma de pago este atributo se debe omitir.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

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

        /// <summary>
        /// Atributo requerido que sirve para incorporar el certificado de sello digital que ampara 
        /// al comprobante, como texto en formato base 64.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlAttribute("certificado")]
        string Certificado { get; set; }
        // <xs:attribute name="certificado" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido que sirve para incorporar el certificado de sello digital 
        //       que ampara al comprobante, como texto en formato base 64.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo condicional para expresar las condiciones comerciales aplicables para el pago 
        /// del comprobante fiscal digital por Internet. Este atributo puede ser condicionado mediante 
        /// atributos o complementos.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// No debe contener espacios en blanco
        /// Longitud Mínima: 1
        /// Longitud Máxima: 1000
        /// Patrón: <code>[^|]{1,1000}</code>
        /// </remarks>
        [XmlAttribute("CondicionesDePago")]
        string CondicionesDePago { get; set; }
        // <xs:attribute name="CondicionesDePago" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo condicional para expresar las condiciones comerciales aplicables para el pago 
        //       del comprobante fiscal digital por Internet. Este atributo puede ser condicionado mediante 
        //       atributos o complementos.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:minLength value="1"/>
        //       <xs:maxLength value="1000"/>
        //       <xs:pattern value="[^|]{1,1000}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para representar la suma de los importes de los conceptos antes de 
        /// descuentos e impuesto. No se permiten valores negativos.
        /// </summary>
        /// <remarks>
        /// Tipo t_Importe a 6 decimales
        /// </remarks>
        [XmlAttribute("SubTotal")]
        decimal SubTotal { get; set; }
        // <xs:attribute name="SubTotal" type="tdCFDI:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para representar la suma de los importes de los conceptos antes de 
        //       descuentos e impuesto. No se permiten valores negativos.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
        // ...
        // <xs:simpleType name="t_Importe">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Tipo definido para expresar importes numéricos con fracción hasta seis decimales. 
        //       El valor se redondea de acuerdo con el número de decimales que soporta la moneda. 
        //       No se permiten valores negativos.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:restriction base="xs:decimal">
        //     <xs:fractionDigits value="6"/>
        //     <xs:minInclusive value="0.000000"/>
        //     <xs:pattern value="[0-9]{1,18}(.[0-9]{1,6})?"/>
        //     <xs:whiteSpace value="collapse"/>
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>
        /// Atributo condicional para representar el importe total de los descuentos aplicables antes de 
        /// impuestos. No se permiten valores negativos. Se debe registrar cuando existan conceptos 
        /// con descuento.
        /// </summary>
        /// <remarks>
        /// Tipo t_Importe a 6 decimales        
        /// </remarks>
        [XmlAttribute("Descuento")]
        decimal? Descuento { get; set; }
        // <code>
        // <xs:attribute name="Descuento" type="tdCFDI:t_Importe" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo condicional para representar el importe total de los descuentos aplicables antes 
        //       de impuestos. No se permiten valores negativos. Se debe registrar cuando existan conceptos
        //       con descuento.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
        // ...
        // <xs:simpleType name="t_Importe">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Tipo definido para expresar importes numéricos con fracción hasta seis decimales. 
        //       El valor se redondea de acuerdo con el número de decimales que soporta la moneda. 
        //       No se permiten valores negativos.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:restriction base="xs:decimal">
        //     <xs:fractionDigits value="6"/>
        //     <xs:minInclusive value="0.000000"/>
        //     <xs:pattern value="[0-9]{1,18}(.[0-9]{1,6})?"/>
        //     <xs:whiteSpace value="collapse"/>
        //   </xs:restriction>
        // </xs:simpleType>
        // </code>

        /// <summary>
        /// Atributo requerido para identificar la clave de la moneda utilizada para expresar los montos, 
        /// cuando se usa moneda nacional se registra MXN. Conforme con la especificación ISO 4217.        
        /// </summary>
        /// <remarks>
        /// </remarks>
        [XmlAttribute("Moneda")]
        string Moneda { get; set; }
        // <xs:attribute name="Moneda" type="catCFDI:c_Moneda" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para identificar la clave de la moneda utilizada para expresar 
        //       los montos, cuando se usa moneda nacional se registra MXN. Conforme con la 
        //       especificación ISO 4217.
        //     </xs:documentation>
        //   </xs:annotation>        
        // </xs:attribute>
        // ...
        // <xs:simpleType name="c_Moneda">
        //   <xs:restriction base="xs:string">
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:enumeration value="AED"/>
        //     <xs:enumeration value="AFN"/>
        //     ...
        //     <xs:enumeration value="MXN"/>
        //     ...
        //     <xs:enumeration value="USD"/>
        //     ...
        //     <xs:enumeration value="XXX"/>
        //     ...
        //     <xs:enumeration value="ZWL"/>
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>
        /// Atributo condicional para representar el tipo de cambio conforme con la moneda usada. 
        /// Es requerido cuando la clave de moneda es distinta de MXN y de XXX. El valor debe reflejar 
        /// el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el 
        /// atributo moneda. Si el valor está fuera del porcentaje aplicable a la moneda tomado del 
        /// catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera 
        /// no automática, una clave de confirmación para ratificar que el valor es correcto e integrar 
        /// dicha clave en el atributo Confirmacion.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [XmlAttribute("TipoCambio")]
        string TipoCambio { get; set; }
        // <xs:attribute name="TipoCambio" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo condicional para representar el tipo de cambio conforme con la moneda usada. 
        //       Es requerido cuando la clave de moneda es distinta de MXN y de XXX. El valor debe reflejar 
        //       el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el 
        //       atributo moneda. Si el valor está fuera del porcentaje aplicable a la moneda tomado del 
        //       catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera 
        //       no automática, una clave de confirmación para ratificar que el valor es correcto e integrar
        //       dicha clave en el atributo Confirmacion.
        //     </xs:documentation>
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
        /// Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, 
        /// más las contribuciones recibidas (impuestos trasladados - federales o locales, derechos, 
        /// productos, aprovechamientos, aportaciones de seguridad social, contribuciones de mejoras) menos 
        /// los impuestos retenidos. Si el valor es superior al límite que establezca el SAT en la Resolución 
        /// Miscelánea Fiscal vigente, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera 
        /// no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha 
        /// clave en el atributo Confirmacion. No se permiten valores negativos.
        /// </summary>
        /// <remarks>
        /// Tipo t_Importe a 6 decimales        
        /// </remarks>
        [XmlAttribute("Total")]
        decimal Total { get; set; }
        // <xs:attribute name="Total" type="tdCFDI:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, 
        //       más las contribuciones recibidas (impuestos trasladados - federales o locales, derechos, 
        //       productos, aprovechamientos, aportaciones de seguridad social, contribuciones de mejoras) 
        //       menos los impuestos retenidos. Si el valor es superior al límite que establezca el SAT en 
        //       la Resolución Miscelánea Fiscal vigente, el emisor debe obtener del PAC que vaya a timbrar 
        //       el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es 
        //       correcto e integrar dicha clave en el atributo Confirmacion. 
        //       No se permiten valores negativos.
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para expresar la clave del efecto del comprobante fiscal para 
        /// el contribuyente emisor.
        /// </summary>
        /// <remarks>
        /// Versión 3.2: Atributo requerido para expresar el efecto del comprobante fiscal para 
        /// el contribuyente emisor.        
        /// </remarks>
        [XmlAttribute("TipoDeComprobante")]
        string TipoDeComprobante { get; set; }
        // <xs:attribute name="TipoDeComprobante" use="required" type="catCFDI:c_TipoDeComprobante">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para expresar la clave del efecto del comprobante fiscal para 
        //       el contribuyente emisor.
        //     </xs:documentation>
        //   </xs:annotation>        
        // </xs:attribute>
        // ...
        // <xs:simpleType name="c_TipoDeComprobante">
        //   <xs:restriction base="xs:string">
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:enumeration value="I"/>
        //     <xs:enumeration value="E"/>
        //     <xs:enumeration value="T"/>
        //     <xs:enumeration value="N"/>
        //     <xs:enumeration value="P"/>
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>		
        /// Atributo condicional para precisar la clave del método de pago que aplica para este comprobante 
        /// fiscal digital por Internet, conforme al Artículo 29-A fracción VII incisos a y b del CFF.
        /// </summary>
        /// <remarks>
        /// PUE	Pago en una sola exhibición
        /// PIP	Pago inicial y parcialidades
        /// PPD	Pago en parcialidades o diferido        
        /// </remarks>
        [XmlAttribute("MetodoPago")] // Versión 3.2: [XmlAttribute("metodoDePago")]
        string MetodoPago { get; set; }
        // <xs:attribute name="MetodoPago" use="optional" type="catCFDI:c_MetodoPago">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo condicional para precisar la clave del método de pago que aplica para este 
        //       comprobante fiscal digital por Internet, conforme al Artículo 29-A fracción VII 
        //       incisos a y b del CFF.
        //     </xs:documentation>
        //   </xs:annotation>        
        // </xs:attribute>
        // ...
        // <xs:simpleType name="c_MetodoPago">
        //   <xs:restriction base="xs:string">
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:enumeration value="PUE"/>
        //     <xs:enumeration value="PIP"/>
        //     <xs:enumeration value="PPD"/>
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>
        /// Atributo requerido para incorporar el código postal del lugar de expedición del comprobante 
        /// (domicilio de la matriz o de la sucursal).        
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("LugarExpedicion")]
        string LugarExpedicion { get; set; }
        // <xs:attribute name="LugarExpedicion" use="required" type="catCFDI:c_CodigoPostal">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para incorporar el código postal del lugar de expedición del comprobante
        //       (domicilio de la matriz o de la sucursal).
        //     </xs:documentation>
        //   </xs:annotation>        
        // </xs:attribute>
        // ...
        // <xs:simpleType name="c_CodigoPostal">
        //   <xs:restriction base="xs:string">
        //     <xs:whiteSpace value="collapse"/>
        //     <xs:enumeration value="00000"/>
        //     <xs:enumeration value="20000"/>
        //     ..
        //     <xs:enumeration value="20000"/>
        //     <xs:enumeration value="99998"/>
        //     <xs:enumeration value="99999"/>
        //   </xs:restriction>
        // </xs:simpleType>

        /// <summary>
        /// Atributo condicional para registrar la clave de confirmación que entregue el PAC para expedir 
        /// el comprobante con importes grandes, con un tipo de cambio fuera del rango establecido o 
        /// con ambos casos. Es requerido cuando se registra un tipo de cambio o un total fuera del 
        /// rango establecido.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [XmlAttribute("Confirmacion")]
        string Confirmacion { get; set; }
        // <xs:attribute name="Confirmacion" use="optional">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo condicional para registrar la clave de confirmación que entregue el PAC para 
        //       expedir el comprobante con importes grandes, con un tipo de cambio fuera del rango 
        //       establecido o con ambos casos. Es requerido cuando se registra un tipo de cambio o 
        //       un total fuera del rango establecido.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:whiteSpace value="collapse"/>
        //       <xs:length value="5"/>
        //       <xs:pattern value="[0-9a-zA-Z]{5}"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>
        
    }
}
