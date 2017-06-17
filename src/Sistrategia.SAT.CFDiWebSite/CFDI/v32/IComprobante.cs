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
    public interface IComprobante
    {
        /// <summary>
        /// Atributo requerido con valor prefijado a 3.2 que indica la versión del estándar bajo el que se 
        /// encuentra expresado el comprobante.
        /// </summary>
        /// <remarks>
        /// Requerido con valor prefijado a 3.2
        /// No debe contener espacios en blanco
        /// <code>
        /// <xs:attribute name="version" use="required" fixed="3.2">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido con valor prefijado a 3.2 que indica la versión del estándar bajo el que 
        ///       se encuentra expresado el comprobante.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:whiteSpace value="collapse"/>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("version")]
        string Version { get; set; }

        /// <summary>        
        /// Atributo opcional para precisar la serie para control interno del contribuyente. 
        /// Este atributo acepta una cadena de caracteres alfabéticos de 1 a 25 caracteres sin incluir 
        /// caracteres acentuados.   
        /// </summary>
        /// <remarks>
        /// Opcional
        /// El largo debe estar entre 1 y 25 caracteres
        /// No debe contener espacios en blanco        
        /// <code>
        /// <xs:attribute name="serie" use="optional">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo opcional para precisar la serie para control interno del contribuyente. 
        ///       Este atributo acepta una cadena de caracteres alfabéticos de 1 a 25 caracteres sin incluir
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:minLength value="1"/>
        ///       <xs:maxLength value="25"/>
        ///       <xs:whiteSpace value="collapse"/>        
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("serie")]
        string Serie { get; set; }

        /// <summary>
        /// Atributo opcional para control interno del contribuyente que acepta un valor numérico 
        /// entero superior a 0 que expresa el folio del comprobante.        
        /// </summary>
        /// <remarks>
        /// opcional
        /// El largo debe estar entre 1 y 20 caracteres
        /// No debe contener espacios en blanco        
        /// <code>
        /// <xs:attribute name="Folio" use="optional">
        ///   <xs:annotation>
        ///     <xs:documentation>Atributo opcional para control interno del contribuyente que acepta un 
        ///     valor numérico entero superior a 0 que expresa el folio del comprobante.</xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:minLength value="1"/>
        ///       <xs:maxLength value="20"/>
        ///       <xs:whiteSpace value="collapse"/>        
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("folio")]
        string Folio { get; set; }

        /// <summary>
        /// Atributo requerido para la expresión de la fecha y hora de expedición del 
        /// comprobante fiscal. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la 
        /// especificación ISO 8601.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// Fecha y hora de expedición del comprobante fiscal
        /// No debe contener espacios en blanco        
        /// <code>
        /// <xs:attribute name="fecha" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>Atributo requerido para la expresión de la fecha y hora de expedición del 
        ///     comprobante fiscal. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la 
        ///     especificación ISO 8601.</xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>        
        /// </code>
        /// </remarks>
        [XmlAttribute("fecha")]
        DateTime Fecha { get; set; }

        /// <summary>
        /// Atributo requerido para contener el sello digital del comprobante fiscal, al que hacen referencia
        /// las reglas de resolución miscelánea aplicable. El sello deberá ser expresado cómo una cadena de 
        /// texto en formato Base 64.        
        /// </summary>
        /// <remarks>
        /// Requerido
        /// El sello deberá ser expresado cómo una cadena de texto en formato Base 64.
        /// No debe contener espacios en blanco
        /// <code>
        /// <xs:attribute name="sello" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>Atributo requerido para contener el sello digital del comprobante fiscal, 
        ///     al que hacen referencia las reglas de resolución miscelánea aplicable. El sello deberá ser 
        ///     expresado cómo una cadena de texto en formato Base 64.        
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:whiteSpace value="collapse"/>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("sello")]
        string Sello { get; set; }

        /// <summary>
        /// Atributo requerido para precisar la forma de pago que aplica para este comprobante fiscal digital
        /// a través de Internet. Se utiliza para expresar Pago en una sola exhibición o número de 
        /// parcialidad pagada contra el total de  parcialidades, Parcialidad 1 de X.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// No debe contener espacios en blanco
        /// <code>
        /// <xs:attribute name="formaDePago" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para precisar la forma de pago que aplica para este comprobnante fiscal 
        ///       digital a través de Internet. Se utiliza para expresar Pago en una sola exhibición o número 
        ///       de parcialidad pagada contra el total de parcialidades, Parcialidad 1 de X.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:whiteSpace value="collapse"/>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>        
        [XmlAttribute("formaDePago")]
        string FormaDePago { get; set; }

        /// <summary>
        /// Atributo requerido para expresar el número de serie del certificado de sello digital que 
        /// ampara al comprobante, de acuerdo al acuse correspondiente a 20 posiciones otorgado 
        /// por el sistema del SAT.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// No debe contener espacios en blanco
        /// El largo debe estar a 20 caracteres
        /// <code>
        /// <xs:attribute name="NoCertificado" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para expresar el número de serie del certificado de sello digital
        ///       que ampara al comprobante, de acuerdo con el acuse correspondiente a 20 posiciones 
        ///       otorgado por el sistema del SAT.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:length value="20"/>
        ///       <xs:whiteSpace value="collapse"/>        
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("noCertificado")]
        string NoCertificado { get; set; }

        /// <summary>
        /// Atributo requerido que sirve para incorporar el certificado de sello digital que ampara al comprobante, como texto en formato base 64.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="certificado" use="required">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo requerido que sirve para incorporar el certificado de sello digital que ampara al comprobante, como texto en formato base 64.
        ///       </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///       <xs:restriction base="xs:string">
        ///           <xs:whiteSpace value="collapse"/>
        ///       </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("certificado")]
        string Certificado { get; set; }

        /// <summary>
        /// Atributo opcional para expresar las condiciones comerciales aplicables para el pago 
        /// del comprobante fiscal digital a través de Internet.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="CondicionesDePago" use="optional">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo opcional para expresar las condiciones comerciales aplicables para el pago 
        ///       del comprobante fiscal digital a través de Internet.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:whiteSpace value="collapse"/>
        ///       <xs:minLength value="1"/>        
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("condicionesDePago")]
        string CondicionesDePago { get; set; }

        /// <summary>
        /// Atributo requerido para representar la suma de los importes antes de descuentos e impuestos.
        /// </summary>
        /// <remarks>
        /// Tipo t_Importe a 6 decimales
        /// <code>
        /// <xs:attribute name="subTotal" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para representar la suma de los importes antes de descuentos e impuestos.
        ///     </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// ...
        /// <xs:simpleType name="t_Importe">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Tipo definido para expresar importes numéricos con fracción hasta seis decimales. 
        ///       El valor se redondea de acuerdo con el número de decimales que soporta la moneda. 
        ///       No se permiten valores negativos.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:restriction base="xs:decimal">
        ///     <xs:fractionDigits value="6"/>
        ///     <xs:minInclusive value="0.000000"/>
        ///     <xs:pattern value="[0-9]{1,18}(.[0-9]{1,6})?"/>
        ///     <xs:whiteSpace value="collapse"/>
        ///   </xs:restriction>
        /// </xs:simpleType>
        /// </code>
        /// </remarks>
        [XmlAttribute("subTotal")]
        decimal SubTotal { get; set; }

        /// <summary>
        /// Atributo opcional para representar el importe total de los descuentos aplicables 
        /// antes de impuestos.
        /// </summary>
        /// <remarks>
        /// Tipo t_Importe a 6 decimales
        /// <code>
        /// <xs:attribute name="descuento" type="cfdi:t_Importe" use="optional">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo opcional para representar el importe total de los descuentos aplicables 
        ///       antes de impuestos.
        ///     </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// ...
        /// <xs:simpleType name="t_Importe">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Tipo definido para expresar importes numéricos con fracción hasta seis decimales. 
        ///       El valor se redondea de acuerdo con el número de decimales que soporta la moneda. 
        ///       No se permiten valores negativos.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:restriction base="xs:decimal">
        ///     <xs:fractionDigits value="6"/>
        ///     <xs:minInclusive value="0.000000"/>
        ///     <xs:pattern value="[0-9]{1,18}(.[0-9]{1,6})?"/>
        ///     <xs:whiteSpace value="collapse"/>
        ///   </xs:restriction>
        /// </xs:simpleType>
        /// </code>
        /// </remarks>
        [XmlAttribute("descuento")]        
        decimal? Descuento { get; set; }

        /// <summary>
        /// Atributo opcional para expresar el motivo del descuento aplicable.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="motivoDescuento" use="optional">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo opcional para expresar el motivo del descuento aplicable.
        ///       </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///       <xs:restriction base="xs:string">
        ///           <xs:minLength value="1"/>
        ///           <xs:whiteSpace value="collapse"/>
        ///       </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("motivoDescuento")]
        string MotivoDescuento { get; set; }

        /// <summary>
        /// Atributo opcional para representar el tipo de cambio conforme a la moneda usada
        /// </summary>
        /// <remarks>
        /// <code>        
        /// <xs:attribute name="TipoCambio" use="optional">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo opcional para representar el tipo de cambio conforme a la moneda usada
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">        
        ///       <xs:whiteSpace value="collapse"/>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("TipoCambio")]
        string TipoCambio { get;set; }

        /// <summary>
        /// Atributo opcional para expresar la moneda utilizada para expresar los montos.
        /// </summary>
        /// <remarks>        
        /// <code>
        /// <xs:attribute name="Moneda">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo opcional para expresar la moneda utilizada para expresar los montos.
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:whiteSpace value="collapse"/>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>        
        /// </code>
        /// </remarks>
        [XmlAttribute("Moneda")]
        string Moneda { get; set; }

        /// <summary>
        /// Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, 
        /// más los impuestos trasladados, menos los impuestos retenidos.
        /// </summary>
        /// <remarks>
        /// Tipo t_Importe a 6 decimales        
        /// </remarks>
        [XmlAttribute("total")]
        decimal Total { get; set; }
        // <xs:attribute name="total" type="cfdi:t_Importe" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables,
        //       más los impuestos trasladados, menos los impuestos retenidos.        
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para expresar el efecto del comprobante fiscal para el contribuyente emisor.
        /// el contribuyente emisor.
        /// </summary>
        /// <remarks>
        /// Versión 3.2: Atributo requerido para expresar el efecto del comprobante fiscal para 
        /// el contribuyente emisor.        
        /// </remarks>
        [XmlAttribute("tipoDeComprobante")]
        string TipoDeComprobante { get; set; }
        // <xs:attribute name="tipoDeComprobante" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para expresar el efecto del comprobante fiscal para el 
        //       contribuyente emisor.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:enumeration value="ingreso"/>
        //       <xs:enumeration value="egreso"/>
        //       <xs:enumeration value="traslado"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido de texto libre para expresar el método de pago de los bienes o servicios 
        /// amparados por el comprobante. Se entiende como método de pago leyendas tales como: cheque, 
        /// tarjeta de crédito o debito, depósito en cuenta, etc.
        /// </summary>
        /// <remarks>
        /// ACTUALIZACION: Este campo deberá tener ahora un valor de acuerdo al catálogo de métodos de 
        /// pago admitidos por el SAT        
        /// </remarks>
        [XmlAttribute("metodoDePago")]
        string MetodoDePago { get; set; }
        // <xs:attribute name="metodoDePago" use="required">
        //   <xs:annotation>
        //       <xs:documentation>
        //           Atributo requerido de texto libre para expresar el método de pago de los bienes o servicios amparados por el comprobante. Se entiende como método de pago leyendas tales como: cheque, tarjeta de crédito o debito, depósito en cuenta, etc.
        //       </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //       <xs:restriction base="xs:string">
        //           <xs:minLength value="1"/>
        //           <xs:whiteSpace value="collapse"/>
        //       </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>
        /// Atributo requerido para incorporar el lugar de expedición del comprobante.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("LugarExpedicion")]
        string LugarExpedicion { get; set; }
        // <xs:attribute name="LugarExpedicion" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para incorporar el lugar de expedición del comprobante.
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
        /// Atributo Opcional para incorporar al menos los cuatro últimos digitos del número
        /// de cuenta con la que se realizó el pago.
        /// </summary>        
        [XmlAttribute("NumCtaPago")]
        string NumCtaPago { get; set; }
        // <xs:attribute name="NumCtaPago">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo Opcional para incorporar al menos los cuatro últimos digitos del número de
        //       cuenta con la que se realizó el pago.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="4"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>        
        /// Atributo opcional para señalar el número de folio fiscal del comprobante que se hubiese 
        /// expedido por el valor total del comprobante, tratándose del pago en parcialidades.
        /// </summary>
        /// <remarks>       
        /// </remarks>
        [XmlAttribute("FolioFiscalOrig")]
        string FolioFiscalOrig { get; set; }
        // <xs:attribute name="FolioFiscalOrig">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo opcional para señalar el número de folio fiscal del comprobante que se hubiese 
        //       expedido por el valor total del comprobante, tratándose del pago en parcialidades.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>        
        /// Atributo opcional para señalar la serie del folio del comprobante que se hubiese expedido 
        /// por el valor total del comprobante, tratándose del pago en parcialidades.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [XmlAttribute("SerieFolioFiscalOrig")]
        string SerieFolioFiscalOrig { get; set; }
        // <xs:attribute name="SerieFolioFiscalOrig">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo opcional para señalar la serie del folio del comprobante que se hubiese 
        //       expedido por el valor total del comprobante, tratándose del pago en parcialidades.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //       <xs:restriction base="xs:string">
        //           <xs:whiteSpace value="collapse"/>
        //       </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>        
        /// Atributo opcional para señalar la fecha de expedición del comprobante que se hubiese emitido
        /// por el valor total del comprobante, tratándose del pago en parcialidades. Se expresa en la 
        /// forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("FechaFolioFiscalOrig")]
        System.DateTime? FechaFolioFiscalOrig { get; set; }
        // <xs:attribute name="FechaFolioFiscalOrig">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo opcional para señalar la fecha de expedición del comprobante que se hubiese 
        //       emitido por el valor total del comprobante, tratándose del pago en parcialidades. 
        //       Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:dateTime">
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        /// <summary>        
        /// Atributo opcional para señalar el total del comprobante que se hubiese expedido por el 
        /// valor total de la operación, tratándose del pago en parcialidades
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("MontoFolioFiscalOrig")]
        decimal? MontoFolioFiscalOrig { get; set; }
        // <xs:attribute name="MontoFolioFiscalOrig" type="cfdi:t_Importe">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo opcional para señalar el total del comprobante que se hubiese expedido por el 
        //       valor total de la operación, tratándose del pago en parcialidades
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
    }
}
