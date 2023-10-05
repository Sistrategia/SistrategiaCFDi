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
    /// amparados por el comprobante. Si no se conoce la forma de pago este atributo se debe omitir.
    /// </summary>
    /// <remarks>
    /// Requerido
    /// No debe contener espacios en blanco
    /// <para>
    /// Se debe registrar la clave de la forma de pago de la adquisición de los bienes, la 
    /// prestación de los servicios, el otorgamiento del uso o goce, o la forma en que se 
    /// recibe el donativo, contenidos en el comprobante.
    /// </para>
    /// <para>
    /// En el caso, de que se haya recibido el pago de la contraprestación al momento de la emisión
    /// del comprobante fiscal, los contribuyentes deberán consignar en éste, la clave vigente
    /// correspondiente a la forma en que se recibió el pago de conformidad con el catálogo 
    /// c_FormaPago publicado en el portal del SAT.
    /// </para>
    /// <para>
    /// En este supuesto no se debe emitor adicionalmente un CFDI al que se le incorpore el 
    /// "Complemento para recepción de pagos", porque el comprobante ya está pagado.
    /// </para>
    /// <para>
    /// En el caso de aplicar más de una forma de pago en una transacción, los contribuyentes deben
    /// incluir en este campo, la clave vigente del catálogo c_FormaPago de la forma de pago 
    /// con la que se liquida la mayor cantidad del pago. En caso de que se reciban distintas formas
    /// de pago con el mismo importe, el contribuyente debe registrar a su consideración, una de las 
    /// formas de pago con las que se recibió el pago de la contraprestación.
    /// </para>
    /// <para>
    /// En el caso de que no se reciba el pago de la contraprestación al momento de la emisión del 
    /// comprobante fiscal (pago en parcialidades o diferido), los constribuyentes deberán seleccionar
    /// la clave "99" (Por definir) del catálogo c_FormaPago publicado en el Portal del SAT.
    /// </para>
    /// <para>
    /// En este supuesto la clave del método de pago debe ser "PPD" (Pago en parcialidades o diferido)
    /// y cuando se reciba el pago total o parcial se debe emitir adicionalmente un CFDI al que se le 
    /// incorpore el "Complemento para recepción de pagos" por cada pago que se reciba.
    /// </para>
    /// <para>
    /// En el caso de donativos entregados en especie, este campo se debe registrar la calve "12" 
    /// (Dación en pago).
    /// </para>
    /// <para>
    /// Las diferentes claves de forma de pago se encuentran incluídas en el catálogo c_FormaPago. 
    /// Elemplo:
    /// </para>
    /// <para>
    /// Cuando el tipo de comprobante sea "E" (Egreso), se deberá registrar como forma de pago, la misma
    /// clave vigente que se registró en el CFDI "I" (Ingreso" que dió origen a este comprobante, 
    /// derivado ya sea de una devolución, descuento o bonificación, conforme al catálogo de formas de 
    /// pago del Anexo 20, opcionalmente se podrá registrar la clave vigente de forma de pago con la que 
    /// se está efectuando el descuento, devolución o bonificación en su caso.
    /// </para>
    /// <para>
    /// Ejemplo: Un contribuyente realiza la compra de un producto
    /// por un valor de $1000.00, y se le emite un CFDI de tipo "I"
    /// (Ingreso). La compra se pagó con forma de pago "01" (Efectivo),
    /// posteriormente, éste realiza la devolución de dicho producto,
    /// por lo que el contribuyente emisor del comprobante debe
    /// emitir un CFDI de tipo "E" (Egreso) por dicha devolución,
    /// registrando la forma de pago "01" (Efectivo), puesto que ésta es
    /// la forma de pago registrada en el CFDI tipo "I" (Ingreso) que se
    /// generó en la operación de origen.
    /// FormaPago= 01
    /// </para>
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

    /// <summary>
    /// Atributo requerido que sirve para incorporar el certificado de sello digital que ampara 
    /// al comprobante, como texto en formato base 64.
    /// </summary>
    /// <remarks>
    /// Requerido
    /// No debe contener espacios en blanco
    /// </remarks>
    [XmlAttribute("Certificado")] // 3.3: certificado
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
    /// <para>
    /// Este campo debe tener hasta la cantidad de decimales que soporte la moneda, ver ejemplo del 
    /// campo Moneda.
    /// </para>
    /// <para>
    /// El valor registrado en este campo debe ser menor o igual que el campo SubTotal.
    /// </para>
    /// <para>
    /// Cuando en el campo TipoDeComprobante sea "I" (Ingreso), "E" (Egreso) o "N" (Nómina), y algún
    /// concepto incluya un descuento, este campo debe existir y debe ser igual al redondeo de la suma
    /// de los campos Descuento registrados en los conceptos; en otro caso se debe omitir este campo.
    /// </para>
    /// </remarks>
    [XmlAttribute("Descuento", typeof(System.Decimal?))]
    public decimal? Descuento { get; set; }
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


    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada para expresar los montos, 
    /// cuando se usa moneda nacional se registra MXN. Conforme con la especificación ISO 4217.        
    /// </summary>
    /// <remarks>
    /// Version 3.2: Atributo opcional para expresar la moneda utilizada para expresar los montos.        
    /// <para>
    /// Las distintas claves de moneda se encuentran incluidas en el catálogo c_Moneda y ahí se indica
    /// el número de decimales que deberán utilizarse.
    /// MXN  Peso Mexicano  2  35%
    /// USD  Dolar americano  2  35%
    /// </para>
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
    //   <xs:simpleType>
    //     <xs:restriction base="xs:string">
    //       <xs:whiteSpace value="collapse"/>
    //     </xs:restriction>
    //   </xs:simpleType>
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
    /// Atributo condicional para representar el tipo de cambio FIX conforme con la moneda usada. 
    /// Es requerido cuando la clave de moneda es distinta de MXN y de XXX. El valor debe reflejar 
    /// el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el 
    /// atributo moneda. Si el valor está fuera del porcentaje aplicable a la moneda tomado del 
    /// catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera 
    /// no automática, una clave de confirmación para ratificar que el valor es correcto e integrar 
    /// dicha clave en el atributo Confirmacion.
    /// </summary>
    /// <remarks>        
    /// Versión 3.2: Atributo opcional para representar el tipo de cambio conforme a la moneda usada.
    /// </remarks>
    [XmlAttribute("TipoCambio")]
    string? TipoCambio { get; set; }
    // <xs:attribute name="TipoCambio" use="optional">
    //   <xs:annotation>
    //     <xs:documentation>
    //       Atributo condicional para representar el tipo de cambio FIX conforme con la moneda usada. 
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
    /// <para>
    /// Es la suma del SubTotal, menos los descuentos aplicables, más las contribuciones recibidas
    /// (impuestos trasladados - federales o locales, derechos, productos, aprovechamientos, aportaciones 
    /// de seguridad social, contribuciones de mejoras) menos los impuestos retenidos. No se permiten 
    /// valores negativos.
    /// </para>
    /// <para>
    /// Este campo debe tener hasta la cantidad de decimales que soporte la moneda, ver ejemplo del
    /// campo Moneda.
    /// </para>
    /// <para>
    /// Cuando el campo TipoDeComprobante sea "T" (Traslado) o "P" (Pago), el importe registrado en este
    /// campo debe ser igual a cero.
    /// </para>
    /// <para>
    /// El SAT publica el límite para el valor máximo de este campo en:
    /// El catálogo c_TipoDeComprobante
    /// En la lista de RFC (l_RFC), cuando el contribuyente registr en el portal del SAT los límites
    /// personalizados.
    /// </para>
    /// <para>
    /// Cuando el valor equivalente en "MXN" (Peso Mexicano) de estge campo exceda el límite establecido, 
    /// debe existir el campo Confirmacion.
    /// </para>
    /// </remarks>
    [XmlAttribute("Total")] // [XmlAttribute("total")]
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
    /// Atributo requerido para expresar si el comprobante ampara una operación de exportación.    
    /// </summary>
    /// <remarks>
    /// Se debe registrar la clave con la que se identifica si el comprobante ampara una 
    /// operación de exportación, las distintas claves vigentes se encuentran incluidas 
    /// en el catálogo c_Exportacion.
    /// 
    /// Cuando se registre el valor “02”, se debe incluir el “Complemento para Comercio Exterior”.
    ///
    /// 01 = No aplica
    /// 
    /// Versión 3.3: Campo no existente en la versión 3.3 y anterioes        
    /// </remarks>
    [XmlAttribute("Exportacion")]
    string Exportacion { get; set; }
    // <xs:simpleType name="c_Exportacion">
    // 	<xs:restriction base="xs:string">
    // 		<xs:whiteSpace value="collapse"/>
    // 		<xs:enumeration value="01"/>
    // 		<xs:enumeration value="02"/>
    // 		<xs:enumeration value="03"/>
    // 		<xs:enumeration value="04"/>
    // 	</xs:restriction>
    // </xs:simpleType>
}