/*************************************************************************************************************
* Comprobante.cs is part of the Sistrategia.SAT Framework developed by Sistrategia
* Copyright (C) 2017 Sistrategia.
* 
* Contributor(s):	J. Ernesto Ocampo Cicero, ernesto@sistrategia.com
* Last Update:		2016-May-27
* Created:			2010-Sep-08
* Version:			1.6.1707.1
*
* Notas: Los atributos tienen en comentarios naturales al final la descripción del xsd.
*        No fue posible agregar la descripción en el nodo <code> de la documentación xml
*        porque marca error y no genera la documentación del intellisense
*        Se podría utilizar <![CDATA[ ... ]]>, pero se decidió no interferir con la documentación.
*************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using System.Text;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    /// <summary>
    /// Estándar para la expresión de comprobantes fiscales digitales.
    /// </summary>
    public class Comprobante
    {
        #region Private Fields
        public int comprobanteId;
        public Guid publicKey;
        private string version;
        private string serie;   // opcional en CFDi
        private string folio;   // opcional en CFDi
        private DateTime fecha;
        private string sello;
        private string noAprobacion; // Este atributo no está presente en la versión 3.0 y 3.2 (Exclusivo de CFD)
        private string anoAprobacion;  // Este atributo no está presente en la versión 3.0 y 3.2 (Exclusivo de CFD)
        private string formaDePago;
        private string formaPago;
        private string noCertificado;
        private bool hasNoCertificado;
        private string certificado;
        private bool hasCertificado;
        private string condicionesDePago;
        private decimal subTotal;
        private decimal? descuento;
        private string motivoDescuento;
        private string tipoCambio;
        private string moneda;
        private decimal total;
        //private ComprobanteTipoDeComprobante tipoDeComprobante;
        private string tipoDeComprobante;
        private string metodoDePago;
        private string metodoPago;
        private string lugarExpedicion;
        private string confirmacion;
        private string numCtaPago;
        private string folioFiscalOrig;
        private string serieFolioFiscalOrig;
        private DateTime? fechaFolioFiscalOrig;
        private decimal? montoFolioFiscalOrig;
        //extended fields:
        private string status;
       
        private string decimalFormat;
        private int? decimalPlaces = null;
                
        //private Emisor emisor;
        //private Receptor receptor;

        ////private Concepto[] conceptos;
        //private virtual List<Concepto> conceptos;
        //private virtual Impuestos impuestos;
        //private Impuestos impuestos;

        //private virtual Complemento complemento;
        //private Addenda addenda;
        #endregion

        #region Constructors
        public Comprobante() {
            this.InitializeDefaults();            
        }

        public Comprobante(string version) {
            this.Version = version; // Importante pasar por la validación.
            this.InitializeDefaults();
        }

        private void InitializeDefaults() {
            if (string.IsNullOrEmpty(this.version))
                this.version = "3.3";
            this.PublicKey = Guid.NewGuid();
            this.status = "P"; // A
            //this.Fecha = DateTime.Now; // Cambiar esta implementación, ya que en producción asignaría la hora del servidor (EU)
            if (this.version == "3.3")
                this.TipoDeComprobante = "I";
            else
                this.TipoDeComprobante = "ingreso";
            if (this.version == "3.3")
                this.FormaPago = "99";
            else
                this.FormaDePago = SATManager.GetFormaDePagoDefault(); // "PAGO EN UNA SOLA EXHIBICION";
            if (this.version == "3.3")
                this.MetodoPago = SATManager.GetMetodoPagoDefault(); // PUE - Pago en una sola exhibición
            else
                this.MetodoDePago = SATManager.GetMetodoDePagoDefault(); // "99" - Otros
            //this.emisor = new Emisor();
            //this.receptor = new Receptor();
            //this.Conceptos = new List<ComprobanteConcepto>();
        }
        #endregion

        #region Configuration Fields

        public string DecimalFormat {
            get {
                if (string.IsNullOrEmpty(this.decimalFormat))
                    return SATManager.GetDecimalFormatDefault();
                else
                    return
                        this.decimalFormat;
            }
            set { this.decimalFormat = value; }
        }

        public int DecimalPlaces {
            get {
                if (this.decimalPlaces.HasValue)
                    return this.decimalPlaces.Value;
                else
                    return SATManager.GetDecimalPlacesDefault();                
            }
        }

        #endregion

        public string GetXml() {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CFDIXmlTextWriter writer =
                new CFDIXmlTextWriter(this, ms, System.Text.Encoding.UTF8);
            writer.WriteXml();
            ms.Position = 0;
            System.IO.StreamReader reader = new System.IO.StreamReader(ms);
            string xml = reader.ReadToEnd();
            reader.Close();
            writer.Close();
            return xml;
        }

        public byte[] GetXmlAsByteArray() {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CFDIXmlTextWriter writer =
                //new CFDIXmlTextWriter(this, ms, System.Text.Encoding.UTF8); // new System.Text.UTF8Encoding(true));            
                new CFDIXmlTextWriter(this, ms, new System.Text.UTF8Encoding(false)); // new System.Text.UTF8Encoding(true));            
            writer.WriteXml();
            ms.Position = 0;
            //byte[] file = ms.ToArray();
            string xmlString = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            writer.Close();
            var encoding = new System.Text.UTF8Encoding(false);
            byte[] file = encoding.GetBytes(xmlString); // System.Text.Encoding.UTF8.GetBytes(xmlString);
            if (file[0] == 239 && file[1] == 187 && file[2] == 191) { // Skip the UTF-8 BOM
                byte[] file2 = new byte[file.Length - 3];
                Array.Copy(file, 3, file2, 0, file.Length - 3);
                file = file2;
            }
            //byte[] file2 = System.Text.Encoding.UTF8.GetBytes(xmlString);
            return file;
        }

        public string GetCadenaOriginal() {
            string xml = this.GetXml();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);

            System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();

            ////using (System.IO.Stream stream = typeof(SATManager).Assembly.GetManifestResourceStream("Sistrategia.Server.SAT.XSLT.cadenaoriginal_3_2.xslt")) {
            ////using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stream)) {
            //// xslt.Load(xmlReader);
            //xslt.Load("http://www.sat.gob.mx/sitio_internet/cfd/3/cadenaoriginal_3_2/cadenaoriginal_3_2.xslt");


            try {
                if (doc.ChildNodes[1].Attributes["Version"] != null && "3.3".Equals(doc.ChildNodes[1].Attributes["Version"].Value)) {
                    xslt.Load("http://www.sat.gob.mx/sitio_internet/cfd/3/cadenaoriginal_3_3/cadenaoriginal_3_3.xslt");
                }
                else if ("3.2".Equals(doc.ChildNodes[1].Attributes["version"].Value)) {
                    xslt.Load("http://www.sat.gob.mx/sitio_internet/cfd/3/cadenaoriginal_3_2/cadenaoriginal_3_2.xslt");
                }
                else {
                    xslt.Load("http://www.sat.gob.mx/sitio_internet/cfd/3/cadenaoriginal_3_0/cadenaoriginal_3_0.xslt");
                }
            }
            catch {

                try {
                    xslt.Load("https://sistrategial1.blob.core.windows.net/wwwimages/satcadenaoriginal/cadenaoriginal_3_2.xslt");
                }
                catch (Exception innerException) {
                    throw; // new Sistrategia.Server.SAT.SATException("No se completó la creación del comprobante. No se puede establecer comunicación con el SAT intente mas tarde.", innerException);
                }
            }

            System.IO.MemoryStream ms2 = new System.IO.MemoryStream();
            xslt.Transform(doc, null, ms2);
            ms2.Position = 3;

            System.IO.StreamReader sr = new System.IO.StreamReader(ms2);
            string cadenaOriginal = sr.ReadToEnd();
            sr.Close();

            return cadenaOriginal;
            //}
            //}
        }

        public string GetCadenaSAT() {
            //string xml = comprobante.GetXml();
            //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            //doc.LoadXml(xml);
            // System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
            //xslt.Load("http://www.sat.gob.mx/sitio_internet/cfd/3/cadenaoriginal_3_2/cadenaoriginal_3_2.xslt");
            //System.IO.MemoryStream ms2 = new System.IO.MemoryStream();
            //xslt.Transform(doc, null, ms2);
            //ms2.Position = 3;
            //System.IO.StreamReader sr = new System.IO.StreamReader(ms2);
            //string cadenaOriginal = sr.ReadToEnd();           
            //sr.Close();
            //return cadenaOriginal;   

            var comprobante = this;

            if ((comprobante != null) && (comprobante.Complementos != null) && (comprobante.Complementos.Count > 0 )) {
                foreach (Complemento complemento in comprobante.Complementos) {
                    if (complemento is TimbreFiscalDigital) {
                        TimbreFiscalDigital timbre = complemento as TimbreFiscalDigital;

                        StringBuilder sb = new StringBuilder();
                        sb.Append("||1.0|");
                        sb.Append(timbre.UUID);
                        sb.Append("|");
                        sb.Append(timbre.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss"));
                        sb.Append("|");
                        sb.Append(timbre.SelloCFD);
                        sb.Append("|");
                        sb.Append(timbre.NoCertificadoSAT);
                        sb.Append("||");
                        return sb.ToString();

                    }
                }
                
            }
           
            return GetCadenaOriginal();
        }

        public string GetQrCode() {
            if ((this.Complementos != null) && (this.Complementos.Count > 0)) {
                foreach (Complemento complemento in this.Complementos) {
                    if (complemento is TimbreFiscalDigital) {
                        TimbreFiscalDigital timbre = complemento as TimbreFiscalDigital;
                        if ("3.3".Equals(this.Version)) {
                            string info = @"https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?id=" + timbre.UUID
                                + "&re=" + this.Emisor.RFC + "&rr=" + this.Receptor.RFC + "&tt=" + this.Total.ToString(this.DecimalFormat)
                                + "&fe=" + this.Sello.Substring(this.Sello.Length - 8, 8);
                            string cbb = SATManager.GetQrCode(info);//System.Convert.ToBase64String(toEncodeAsBytes);
                            return cbb;
                        }
                        else {
                            string info = string.Format("?re={0}&rr={1}&tt={2}&id={3}",
                            this.Emisor.RFC, this.Receptor.RFC, this.Total.ToString(this.DecimalFormat), timbre.UUID);
                            string cbb = SATManager.GetQrCode(info, 8);
                            return cbb;
                        }
                    }
                }
            }
            return string.Empty;
        }

        [Key]
        public int ComprobanteId {
            get { return this.comprobanteId; }
            set { this.comprobanteId = value; }
        }

        [Required]
        public Guid PublicKey {
            get { return this.publicKey; }
            set { this.publicKey = value; }
        }

        #region Public Properties

        /// <summary>
        /// Atributo requerido con valor prefijado a 3.3 que indica la versión del estándar bajo el que se 
        /// encuentra expresado el comprobante.
        /// </summary>
        /// <remarks>
        /// Requerido con valor prefijado a 3.3
        /// No debe contener espacios en blanco        
        /// </remarks>
        [XmlAttribute("Version")] // 3.2 y anterior: [XmlAttribute("version")]
        public string Version {
            get { return this.version; }
            set {
                //if (value != "3.3") {
                //    throw new ArgumentException("Atributo requerido con valor prefijado a 3.3");
                //}
                if (
                    value == "3.3" // Atributo requerido con valor prefijado a 3.3
                    || value == "3.2" // Atributo requerido con valor prefijado a 3.2
                    || value == "3.0" // Atributo requerido con valor prefijado a 3.0
                    || value == "2.2" // Atributo requerido con valor prefijado a 2.2
                    || value == "2.0" // Atributo requerido con valor prefijado a 2.0
                    || value == "1.0" // Atributo requerido con valor prefijado a 1.0
                    )
                    this.version = value; // validar las posibles versiones
                else {
                    this.version = "3.3";
                    throw new ArgumentException("Atributo requerido con valor prefijado a 3.3, los únicos valores "
                        + "válidos son: 1.0, 2.0, 2.2. 3.0, 3.2 y 3.3");
                }
            }
        }
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
        /// Version 3.2: Atributo opcional para precisar la serie para control interno del contribuyente. 
        /// Este atributo acepta una cadena de caracteres alfabéticos de 1 a 25 caracteres sin incluir 
        /// caracteres acentuados.
        /// </remarks>
        [XmlAttribute("Serie")] // 3.2 y anterior: [XmlAttribute("serie")]
        public string Serie {
            get { return this.serie; }
            set {
                if (!string.IsNullOrEmpty(value) && value.Length > 25) {
                    throw new ArgumentException("El largo del atributo Serie debe "
                        + "estar entre 1 y 25 caracteres");
                }
                this.serie = value.Trim();
            }
        }
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
        /// Version 3.2: Atributo opcional para control interno del contribuyente que acepta un valor 
        /// numérico entero superior a 0 que expresa el folio del comprobante.        
        /// </remarks>
        [XmlAttribute("Folio")] // Version 3.2: [XmlAttribute("folio")]
        public string Folio {
            get { return this.folio; }
            set {
                if (this.version == "3.3" && (!string.IsNullOrEmpty(value) && value.Length > 40)) {
                    throw new ArgumentException("El largo del atributo Folio debe estar entre 1 y 40 caracteres");
                }
                else if (!string.IsNullOrEmpty(value) && value.Length > 20) { // 3.2 y anteriores
                    throw new ArgumentException("El largo del atributo folio debe estar entre 1 y 20 caracteres");
                }
                this.folio = value;
            }
        }
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
        /// Version 3.2: Atributo requerido para la expresión de la fecha y hora de expedición del 
        /// comprobante fiscal. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la 
        /// especificación ISO 8601.
        /// </remarks>
        [XmlAttribute("Fecha")] // Version 3.2: [XmlAttribute("fecha")]
        public DateTime Fecha {
            get { return this.fecha; }
            set {
                string fechaString = Convert.ToDateTime(value).ToString("dd/MM/yyyy HH:mm:ss");
                IFormatProvider culture = new System.Globalization.CultureInfo("es-MX", true);
                value = DateTime.ParseExact(fechaString, "dd/MM/yyyy HH:mm:ss", culture);
                this.fecha = value;
            }
        }
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
        [XmlAttribute("Sello")] // Version 3.2: [XmlAttribute("sello")]
        public string Sello {
            get { return this.sello; }
            set { this.sello = value.Trim(); }
        }
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
        /// Atributo requerido para precisar el número de aprobación emitido por el SAT, para el rango de 
        /// folios al que pertenece el folio particular que ampara el comprobante fiscal digital.
        /// </summary>
        /// <remarks>
        /// Este atributo no está presente en la versión 3.0, 3.2 y 3.0 (Exclusivo de CFD)
        /// </remarks>
        [XmlIgnore]
        //[XmlAttribute("noAprobacion", DataType = "integer")]
        ////[XmlAttribute("noAprobacion", typeof(System.Decimal))]
        [Obsolete("Attributo depreciado en la versión 3.0", false)]
        public string NoAprobacion {
            get { return this.noAprobacion; }
            set { this.noAprobacion = value; }
        }

        /// <summary>
        /// Atributo requerido para precisar el año en que se solicito el folio que se están utilizando 
        /// para emitir el comprobante fiscal digital.
        /// </summary>
        /// <remarks>
        /// 4 Dígitos
        /// Este atributo empezó en la versión 2.0 hasta la versión 2.2 (no se encuentra en la versión 1.0)
        /// Este atributo no está presente en la versión 3.0, 3.2 y 3.3 (Exclusivo de CFD)
        /// </remarks>
        [XmlIgnore] // [XmlAttribute("anoAprobacion", DataType = "integer")]
        [Obsolete("Attributo depreciado en la versión 3.0", false)]
        public string AnoAprobacion {
            get { return this.anoAprobacion; }
            set { this.anoAprobacion = value; }
        }

        /// <summary>
        /// Atributo requerido para precisar la forma de pago que aplica para este comprobante fiscal digital 
        /// a través de Internet. Se utiliza para expresar Pago en una sola exhibición o número de 
        /// parcialidad pagada contra el total de  parcialidades, Parcialidad 1 de X.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlIgnore] //[XmlAttribute("formaDePago")]
        [Obsolete("Attributo depreciado en la versión 3.3", false)]
        public string FormaDePago {
            get { return this.formaDePago; }
            set { this.formaDePago = value; }
        }
        // <xs:attribute name="formaDePago" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para precisar la forma de pago que aplica para este comprobnante fiscal 
        //       digital a través de Internet. Se utiliza para expresar Pago en una sola exhibición o número 
        //       de parcialidad pagada contra el total de parcialidades, Parcialidad 1 de X.
        //     </xs:documentation>
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
        /// Se debe registrar la clave de la forma de pago de la adquisición de los bienes o de la 
        /// prestación de los servicios contenidos en el comprobante.
        /// </para>
        /// <para>
        /// En el caso, de que se haya recibido el pago de la contraprestación al momento de la emisión
        /// del comprobante fiscal, los contribuyentes deberán consignar en éste, la clave correspondiente
        /// a la forma de pago de conformidad con el catálogo c_FormaPago publicado en el portal del SAT;
        /// no debiendo incorporar el "Complemento para recepción de pagos".
        /// </para>
        /// <para>
        /// En el caso de aplicar más de una forma de pago en una transacción, los contribuyentes deben
        /// incluir en este campo, la clave de forma de pago con la que se liquida la mayor cantidad del 
        /// pago con el mismo importe, el contribuyente debe registrar a su consideración, una de las 
        /// formas de pago con las que se recibió el pago de la contraprestación.
        /// </para>
        /// <para>
        /// En el caso de que no se reciba el pago de la contraprestación al momento de la emisión del 
        /// comprobante fiscal (pago en parcialidades o diferido), los constribuyentes deberán seleccionar
        /// la clave "99" (Por definir) del catálogo c_FormaPago publicdo en el Portal del SAT.
        /// </para>
        /// <para>
        /// Cuando el tipo de comprobante sea "E" (Egreso), se deberá registrar como forma de pago, la misma
        /// que se registró en el CFDI "I" (Ingreso" que dió origen a este comprobante, derivado ya sea de
        /// una devolución, descuento o bonificación, conforme al catálogo de formas de pago del Anexo 20,
        /// opcionalmente se podrá registrar la forma de pago conl a que se está efectuando el descuento,
        /// devolución o bonificación en su caso.
        /// </para>
        /// </remarks>
        [XmlAttribute("FormaPago")]
        public string FormaPago {
            get { return this.formaPago; }
            set { this.formaPago = value; }
        }
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
        [XmlAttribute("NoCertificado")] // Version 3.2: [XmlAttribute("noCertificado")]
        public string NoCertificado {
            get {
                if (this.HasNoCertificado && this.Certificado != null)
                    return this.Certificado.NumSerie;
                else
                    return null;
            }
            set {
                if (this.version == "3.3" && (!string.IsNullOrEmpty(value) && value.Length > 20)) {
                    throw new ArgumentException("El largo del atributo NoCertificado debe estar entre 1 y 20 caracteres");
                }
                this.noCertificado = value;
            }
        }
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

        [ForeignKey("Certificado")]
        public int? CertificadoId { get; set; }

        [XmlIgnore()]
        public virtual Certificado Certificado { get; set; }
        //    get { return this.certificado; }
        //    set { this.certificado = value; }
        //}

        [XmlIgnore]
        public bool HasNoCertificado {
            get { return this.hasNoCertificado; }
            set { this.hasNoCertificado = value; }
        }

        [XmlIgnore]
        public bool HasCertificado {
            get { return this.hasCertificado; }
            set { this.hasCertificado = value; }
        }

        /// <summary>
        /// Atributo requerido que sirve para incorporar el certificado de sello digital que ampara 
        /// al comprobante, como texto en formato base 64.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlAttribute("certificado")] // Version 3.2: [XmlAttribute("certificado")]
        public string CertificadoBase64 {
            get {
                if (this.HasCertificado && this.Certificado != null)
                    return this.Certificado.CertificadoBase64;
                else
                    return null;
            }
            //get { return this.certificado; }
            set { this.certificado = value; } // .Trim() ??
        }
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
        [XmlAttribute("CondicionesDePago")] // version 3.2: [XmlAttribute("condicionesDePago")]
        public string CondicionesDePago {
            get { return this.condicionesDePago; }
            set {
                if (this.version == "3.3" && (!string.IsNullOrEmpty(value) && value.Trim().Length > 1000)) {
                    throw new ArgumentException("El largo del atributo CondicionesDePago debe estar entre 1 y 1000 caracteres");
                }
                this.condicionesDePago = value != null ? value.Trim() : null;
            }
        }
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
        /// <para>
        /// Este campo debe tener hasta la cantidad de decimales que soporte la moneda, ver ejemplo
        /// del campo Moneda.
        /// </para>
        /// <para>
        /// Cuando en el campo TipoDeComprobante sea "I" (Ingreso), "E" (Egreso) o "N" (Nómina), el 
        /// importe registrado en este campo deber ser igual al redondeo de la suma de los importes
        /// de los conceptos registrados.
        /// </para>
        /// <para>
        /// Cuando en el campo TipoDeComprobante sea "T" (Traslado) o "P" (Pago) el importe registrado
        /// en este campo debe ser igual a cero.
        /// </para>
        /// </remarks>
        [XmlAttribute("SubTotal")] // version 3.2: [XmlAttribute("subTotal")]
        public decimal SubTotal {
            get { return this.subTotal; }
            set { this.subTotal = value; }
        }
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
        [XmlAttribute("Descuento")] // [XmlAttribute("descuento")]
        public decimal? Descuento {
            get { return this.descuento; }
            set { this.descuento = value; }
        }
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
        /// Attributo depreciado en la versión 3.3
        /// Versión 3.2: Atributo opcional para expresar el motivo del descuento aplicable.
        /// </summary>
        /// <remarks>
        /// Obsoleto
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlIgnore] // Versión 3.2: [XmlAttribute("motivoDescuento")]
        [Obsolete("Attributo depreciado en la versión 3.3", false)]
        public string MotivoDescuento {
            get { return this.motivoDescuento; }
            set { this.motivoDescuento = value; } // private or exception when this attribute is used 3.3 ?
        }
        // <xs:attribute name="motivoDescuento" use="optional">
        //   <xs:annotation>
        //       <xs:documentation>
        //           Atributo opcional para expresar el motivo del descuento aplicable.
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
        public string Moneda {
            get { return this.moneda; }
            set { this.moneda = value; }
        }
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
        /// Atributo condicional para representar el tipo de cambio conforme con la moneda usada. 
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
        public string TipoCambio {
            get { return this.tipoCambio; }
            set { this.tipoCambio = value; }
        }
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
        public decimal Total {
            get { return this.total; }
            set { this.total = value; }
        }
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
        [XmlAttribute("TipoDeComprobante")] // Versión 3.2: [XmlAttribute("tipoDeComprobante")]
        public string TipoDeComprobante {
            get { return this.tipoDeComprobante; }
            set {
                if (this.version == "3.3") {
                    if ("I".Equals(value, StringComparison.InvariantCulture)
                    || "E".Equals(value, StringComparison.InvariantCulture)
                    || "T".Equals(value, StringComparison.InvariantCulture)
                    || "N".Equals(value, StringComparison.InvariantCulture)
                    || "P".Equals(value, StringComparison.InvariantCulture)
                    ) {
                        this.tipoDeComprobante = value;
                        //I	Ingreso
                        //E	Egreso
                        //T	Traslado
                        //N	Nómina
                        //P	Pago
                    }
                    else
                        throw new ArgumentException("Los valores válidos para el atributo TipoDeComprobante son: I, E, T, N o P.");
                }
                else if ("ingreso".Equals(value, StringComparison.InvariantCulture)
                    || "egreso".Equals(value, StringComparison.InvariantCulture)
                    || "traslado".Equals(value, StringComparison.InvariantCulture))
                    this.tipoDeComprobante = value;
                else
                    throw new ArgumentException("Los valores válidos para el atributo TipoDeComprobante son: ingreso, egreso o traslado.");
            }
        }
        // <xs:attribute name="TipoDeComprobante" use="required" type="catCFDI:c_TipoDeComprobante">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido para expresar la clave del efecto del comprobante fiscal para 
        //       el contribuyente emisor.
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
        /// Attributo depreciado en la versión 3.3. 
        /// Versión 3.2: Atributo requerido de texto libre para expresar el método de pago de los bienes o 
        /// servicios amparados por el comprobante. Se entiende como método de pago leyendas tales como: 
        /// cheque, tarjeta de crédito o debito, depósito en cuenta, etc.
        /// </summary>
        /// <remarks>
        /// ACTUALIZACION: Este campo deberá tener ahora un valor de acuerdo al catálogo de métodos de pago 
        /// admitidos por el SAT        
        /// </remarks>
        [XmlIgnore] // Versión 3.2: [XmlAttribute("metodoDePago")]
        [Obsolete("Attributo depreciado en la versión 3.3", false)]
        public string MetodoDePago {
            get { return this.metodoDePago; }
            set { this.metodoDePago = value; }
        }
        // <xs:attribute name="metodoDePago" use="required">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo requerido de texto libre para expresar el método de pago de los bienes o servicios 
        //       amparados por el comprobante. Se entiende como método de pago leyendas tales como: 
        //       cheque, tarjeta de crédito o debito, depósito en cuenta, etc.
        //     </xs:documentation>
        //   </xs:annotation>
        //   <xs:simpleType>
        //     <xs:restriction base="xs:string">
        //       <xs:minLength value="1"/>
        //       <xs:whiteSpace value="collapse"/>
        //     </xs:restriction>
        //   </xs:simpleType>
        // </xs:attribute>

        [ForeignKey("TipoMetodoDePago")]
        public int? TipoMetodoDePagoId { get; set; }
        [XmlIgnore]
        public virtual TipoMetodoDePago TipoMetodoDePago { get; set; }

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
        public string MetodoPago {
            get { return this.metodoPago; }
            set { this.metodoPago = value; }
        }
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
        /// Versión 3.2:
        /// Atributo requerido para incorporar el lugar de expedición del comprobante.
        /// </remarks>
        [XmlAttribute("LugarExpedicion")]
        public string LugarExpedicion {
            get { return this.lugarExpedicion; }
            set {
                if (this.version == "3.3") {
                    if ((!string.IsNullOrEmpty(value) && value.Trim().Length != 5)) {
                        throw new ArgumentException("El largo del atributo LugarExpedicion debe ser de 5 caracteres");
                    }
                    this.lugarExpedicion = value != null ? value.Trim() : null;
                }
                else
                    this.lugarExpedicion = value;
            }
        }
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
        // Versión 3.2:
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
        /// Atributo condicional para registrar la clave de confirmación que entregue el PAC para expedir 
        /// el comprobante con importes grandes, con un tipo de cambio fuera del rango establecido o 
        /// con ambos casos. Es requerido cuando se registra un tipo de cambio o un total fuera del 
        /// rango establecido.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [XmlAttribute("Confirmacion")]
        public string Confirmacion {
            get { return this.confirmacion; }
            set {
                if ((!string.IsNullOrEmpty(value) && value.Trim().Length != 5)) {
                    throw new ArgumentException("El largo del atributo Confirmacion debe ser de 5 caracteres");
                }
                this.confirmacion = value != null ? value.Trim() : null;
            }
        }
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

        /// <summary>
        /// Atributo depreciado en la versión 3.3 
        /// Versión 3.2: Atributo Opcional para incorporar al menos los cuatro últimos digitos del 
        /// número de cuenta con la que se realizó el pago.
        /// </summary>
        /// <remarks>
        /// Obsoleto: Attributo depreciado en la versión 3.3
        /// </remarks>
        [XmlAttribute("NumCtaPago")]
        [Obsolete("Attributo depreciado en la versión 3.3", false)]
        public string NumCtaPago {
            get { return this.numCtaPago; }
            set { this.numCtaPago = value; }
        }
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
        /// Atributo depreciado en la versión 3.3 
        /// Versión 3.2: Atributo opcional para señalar el número de folio fiscal del comprobante que se 
        /// hubiese expedido por el valor total del comprobante, tratándose del pago en parcialidades.
        /// </summary>
        /// <remarks>       
        /// </remarks>
        [XmlAttribute("FolioFiscalOrig")]
        [Obsolete("Attributo depreciado en la versión 3.3", false)]
        public string FolioFiscalOrig {
            get { return this.folioFiscalOrig; }
            set { this.folioFiscalOrig = value; }
        }
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
        /// Atributo depreciado en la versión 3.3 
        /// Versión 3.2: Atributo opcional para señalar la serie del folio del comprobante que se hubiese 
        /// expedido por el valor total del comprobante, tratándose del pago en parcialidades.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [XmlAttribute("SerieFolioFiscalOrig")]
        [Obsolete("Attributo depreciado en la versión 3.3", false)]
        public string SerieFolioFiscalOrig {
            get { return this.serieFolioFiscalOrig; }
            set { this.serieFolioFiscalOrig = value; }
        }
        // <xs:attribute name="SerieFolioFiscalOrig">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo opcional para señalar la serie del folio del comprobante que se hubiese 
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
        /// Atributo depreciado en la versión 3.3 
        /// Versión 3.2: Atributo opcional para señalar la fecha de expedición del comprobante que se hubiese
        /// emitido por el valor total del comprobante, tratándose del pago en parcialidades. Se expresa en 
        /// la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("FechaFolioFiscalOrig")]
        [Obsolete("Attributo depreciado en la versión 3.3", false)]
        public System.DateTime? FechaFolioFiscalOrig {
            get { return this.fechaFolioFiscalOrig; }
            set { this.fechaFolioFiscalOrig = value; }
        }
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
        /// Atributo depreciado en la versión 3.3 
        /// Versión 3.2: Atributo opcional para señalar el total del comprobante que se hubiese expedido 
        /// por el valor total de la operación, tratándose del pago en parcialidades
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [XmlAttribute("MontoFolioFiscalOrig")]
        [Obsolete("Attributo depreciado en la versión 3.3", false)]
        public decimal? MontoFolioFiscalOrig {        
            get { return this.montoFolioFiscalOrig; }
            set { this.montoFolioFiscalOrig = value; }
        }
        // <xs:attribute name="MontoFolioFiscalOrig" type="cfdi:t_Importe">
        //   <xs:annotation>
        //     <xs:documentation>
        //       Atributo opcional para señalar el total del comprobante que se hubiese expedido por el 
        //       valor total de la operación, tratándose del pago en parcialidades
        //     </xs:documentation>
        //   </xs:annotation>
        // </xs:attribute>
        
        //[ForeignKey("Emisor")]
        //public int? EmisorId { get; set; }

        ///// <summary>
        ///// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
        ///// </summary>
        //[XmlElement("Emisor", typeof(Emisor))]
        //public virtual Emisor Emisor { get; set; }
        ////public virtual Emisor Emisor {
        ////    get { return this.emisor; }
        ////    set { this.emisor = value; }
        ////}

        #endregion

        #region Public Relations

        [ForeignKey("Emisor")]
        public int? ComprobanteEmisorId { get; set; }

        /// <summary>
        /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
        /// </summary>
        [XmlElement("Emisor", typeof(ComprobanteEmisor))]
        public virtual ComprobanteEmisor Emisor { get; set; }
        //public virtual Emisor Emisor {
        //    get { return this.emisor; }
        //    set { this.emisor = value; }
        //}
        

        //[ForeignKey("Receptor")]
        //public int? ReceptorId { get; set; }

        ///// <summary>
        ///// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
        ///// </summary>
        //[XmlElement("Receptor", typeof(Receptor))]
        //public virtual Receptor Receptor { get; set; }
        ////public virtual Receptor Receptor {
        ////    get { return this.receptor; }
        ////    set { this.receptor = value; }
        ////}

        [ForeignKey("Receptor")]
        public int? ComprobanteReceptorId { get; set; }

        /// <summary>
        /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
        /// </summary>
        [XmlElement("Receptor", typeof(ComprobanteReceptor))]
        public virtual ComprobanteReceptor Receptor { get; set; }
        //public virtual Receptor Receptor {
        //    get { return this.receptor; }
        //    set { this.receptor = value; }
        //}

        /// <summary>
        /// Nodo para introducir la información detallada de un bien o servicio amparado en el comprobante.
        /// </summary>
        [XmlArrayItemAttribute("Concepto", IsNullable = false)]
        public virtual List<Concepto> Conceptos { get; set; }
        //public IList<ComprobanteConcepto> Conceptos {
        //    get { return this.conceptos; }
        //    //set { this.conceptos = value; }
        //}

        ///// <summary>
        ///// Nodo para introducir la información detallada de un bien o servicio amparado en el comprobante.
        ///// </summary>
        //// [XmlArrayItemAttribute("Concepto", IsNullable = false)]
        //public IComprobanteConceptoCollection Conceptos {
        //    get { return this.currentData.Conceptos; }
        //    //set { this.currentData.Conceptos = value; }
        //}

        [ForeignKey("Impuestos")]
        public int? ImpuestosId { get; set; }

        /// <summary>
        /// Nodo requerido para capturar los impuestos aplicables.
        /// </summary>
        public virtual Impuestos Impuestos { get; set; }
        //public Impuestos Impuestos {
        //    get { return this.impuestos; }
        //    set { this.impuestos = value; }
        //}

        //private Complemento complemento;

        ///// <summary>
        ///// Nodo opcional donde se incluirá el complemento Timbre Fiscal Digital de manera obligatoria 
        ///// y los nodos complementarios determinados por el SAT, de acuerdo a las disposiciones particulares 
        ///// a un sector o actividad específica.
        ///// </summary>
        //public Complemento Complemento {
        //    get { return this.complemento; }
        //    set { this.complemento = value; }
        //}


        //private List<Complemento> complementos;

        /// <summary>
        /// Nodo opcional donde se incluirá el complemento Timbre Fiscal Digital de manera obligatoria 
        /// y los nodos complementarios determinados por el SAT, de acuerdo a las disposiciones particulares 
        /// a un sector o actividad específica.
        /// </summary>
        public virtual List<Complemento> Complementos { get; set; }

        public virtual List<ReceptorCorreoEntrega> CorreosEntrega { get; set; }

        #endregion

        [XmlIgnore]
        public string GeneratedCadenaOriginal { get; set; }

        [XmlIgnore]
        public string GeneratedXmlUrl { get; set; }

        [XmlIgnore]
        public string GeneratedPDFUrl { get; set; }

        [ForeignKey("ViewTemplate")]
        public int? ViewTemplateId { get; set; }
        public virtual ViewTemplate ViewTemplate { get; set; }

        public int? ExtendedIntValue1 { get; set; }
        public int? ExtendedIntValue2 { get; set; }
        public int? ExtendedIntValue3 { get; set; }

        public string ExtendedStringValue1 { get; set; }
        public string ExtendedStringValue2 { get; set; }
        public string ExtendedStringValue3 { get; set; }

        [XmlIgnore]
        public string Status { 
            get { return this.status; }
            set { this.status = value; }
        }

    }

    //public enum ComprobanteTipoDeComprobante
    //{
    //    ingreso,
    //    egreso,
    //    traslado
    //}

    public class ComprobanteEmisor
    {
        private string rfc;
        private string nombre;
        private Emisor emisor;

        [Key]
        public int ComprobanteEmisorId { get; set; }

        [ForeignKey("Emisor")]
        public int EmisorId { get; set; }

        [Required]
        [NotMapped]
        public Guid PublicKey { get { return this.Emisor.PublicKey; } }        

        /// <summary>
        /// Atributo requerido para la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente emisor del comprobante sin guiones o espacios.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="rfc" type="cfdi:t_RFC" use="required">
        ///     <xs:annotation>
        ///         <xs:documentation>Atributo requerido para la Clave del Registro Federal de Contribuyentes correspondiente al contribuyente emisor del comprobante sin guiones o espacios.</xs:documentation>
        ///     </xs:annotation>
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
        [Required]
        [MaxLength(13)]
        public string RFC
        {
            get { return this.rfc; }
            set { this.rfc = SATManager.NormalizeWhiteSpace(value); }
        }
        //[NotMapped]
        //public string RFC { get { return this.Emisor.RFC; } }
        ////public string RFC { get; set; }

        /// <summary>
        /// Atributo opcional para el nombre, denominación o razón social del contribuyente emisor del comprobante.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="nombre">
        ///     <xs:annotation>
        ///         <xs:documentation>Atributo opcional para el nombre, denominación o razón social del contribuyente emisor del comprobante.</xs:documentation>
        ///     </xs:annotation>
        ///     <xs:simpleType>
        ///         <xs:restriction base="xs:string">
        ///             <xs:minLength value="1"/>
        ///             <xs:whiteSpace value="collapse"/>
        ///         </xs:restriction>
        ///     </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// <xs:minLength value="1"/>
        /// <xs:whiteSpace value="collapse"/>
        /// </remarks>
        [MaxLength(256)]
        public string Nombre {
            get { return this.nombre; }
            set { this.nombre = SATManager.NormalizeWhiteSpace(value); }
        }
        //[NotMapped]
        //public string Nombre { get { return this.Emisor.Nombre; } }

        /// <summary>
        /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
        /// </summary>
        [XmlElement("Emisor", typeof(Emisor))]
        public virtual Emisor Emisor { 
            get { return this.emisor; }
            set
            {
                this.emisor = value;
                this.rfc = value.RFC;
                this.nombre = value.Nombre; // Validate?
            } 
        }
        //public virtual Emisor Emisor {
        //    get { return this.emisor; }
        //    set { this.emisor = value; }
        //}

        [ForeignKey("DomicilioFiscal")]
        public int? DomicilioFiscalId { get; set; }

        /// <summary>
        /// Nodo opcional para precisar la información de ubicación del domicilio fiscal del contribuyente emisor.
        /// </summary>
        /// <remarks>
        /// Antes era requerido
        /// <code>
        /// <xs:element name="DomicilioFiscal" type="cfdi:t_UbicacionFiscal" minOccurs="0">
        ///     <xs:annotation>
        ///         <xs:documentation>Nodo opcional para precisar la información de ubicación del domicilio fiscal del contribuyente emisor</xs:documentation>
        ///     </xs:annotation>
        /// </xs:element>
        /// </code>
        /// </remarks>
        //[XmlElement("DomicilioFiscal")]
        public virtual UbicacionFiscal DomicilioFiscal { get; set; }


        [ForeignKey("ExpedidoEn")]
        public int? ExpedidoEnId { get; set; }

        /// <summary>
        /// Nodo opcional para precisar la información de ubicación del domicilio en donde es emitido 
        /// el comprobante fiscal en caso de que sea distinto del domicilio fiscal del contribuyente emisor.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:element name="ExpedidoEn" type="cfdi:t_Ubicacion" minOccurs="0">
        ///     <xs:annotation>
        ///         <xs:documentation>Nodo opcional para precisar la información de ubicación del domicilio en donde es emitido el comprobante fiscal en caso de que sea distinto del domicilio fiscal del contribuyente emisor.</xs:documentation>
        ///     </xs:annotation>
        /// </xs:element>
        /// </code>
        /// </remarks>
        //[XmlElement("ExpedidoEn")]
        public virtual Ubicacion ExpedidoEn { get; set; }

        [NotMapped]
        public virtual string RegimenFiscalClave { 
            get {
                if (this.Emisor.RegimenFiscal != null && this.Emisor.RegimenFiscal.Count > 0) {
                    foreach (var regimen in this.Emisor.RegimenFiscal) {
                        if (regimen.Status == "A")
                            return regimen.RegimenFiscalClave;
                    }                    
                }
                return null;
            } 
        }

        public virtual List<ComprobanteEmisorRegimenFiscal> RegimenFiscal { get; set; }

        [NotMapped]
        public string Telefono { get { return this.Emisor.Telefono; } }
        [NotMapped]
        public string Correo { get { return this.Emisor.Correo; } }
        [NotMapped]
        public string CifUrl { get { return this.Emisor.CifUrl; } }
        [NotMapped]
        public string LogoUrl { get { return this.Emisor.LogoUrl; } }
        [NotMapped]
        public int? ViewTemplateId { get { return this.Emisor.ViewTemplateId; } }
        [NotMapped]
        public ViewTemplate ViewTemplate { get { return this.Emisor.ViewTemplate; } }
    }

    //public class ComprobanteEmisorRegimenFiscal
    //{
    //    [Key]
    //    public int ComprobanteEmisorId { get; set; }

    //    public virtual List<ComprobanteEmisorRegimenFiscalItem> RegimenFiscal { get; set; }
    //}

    public class ComprobanteEmisorRegimenFiscal // Item
    {
        [Key]
        public int ComprobanteEmisorRegimenFiscalId { get; set; }
        //public int ComprobanteEmisorRegimenFiscalItemId { get; set; }

        [ForeignKey("ComprobanteEmisor")]
        public int ComprobanteEmisorId { get; set; }

        public virtual ComprobanteEmisor ComprobanteEmisor { get; set; }

        [NotMapped]
        public string Regimen { get { return this.RegimenFiscal.Regimen; } }

        [NotMapped]
        public string RegimenFiscalClave { get { return this.RegimenFiscal.RegimenFiscalClave; } }

        [ForeignKey("RegimenFiscal")]
        public int RegimenFiscalId { get; set; }

        public virtual RegimenFiscal RegimenFiscal { get; set; }

        public int Ordinal { get; set; }
    }

    /// <summary>
    /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
    /// </summary>
    public class ComprobanteReceptor
    {
        [Key]
        public int ComprobanteReceptorId { get; set; }

        [ForeignKey("Receptor")]
        public int ReceptorId { get; set; }

        [Required]
        [NotMapped]
        public Guid PublicKey { get { return this.Receptor.PublicKey; } }

        [Required]
        [MaxLength(13)]
        [NotMapped]
        public string RFC { get { return this.Receptor.RFC; } }
        //public string RFC { get; set; }

        [MaxLength(256)]
        [NotMapped]
        public string Nombre { get { return this.Receptor.Nombre; } }

        /// <summary>
        /// Atributo condicional para registrar la clave del país de residencia para efectos fiscales 
        /// del receptor del comprobante, cuando se trate de un extranjero, y que es conforme con 
        /// la especificación ISO 3166-1 alpha-3. Es requerido cuando se incluya el complemento de 
        /// comercio exterior o se registre el atributo NumRegIdTrib.
        /// receptor del comprobante.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        [MaxLength(3)]
        //[XmlAttribute("ResidenciaFiscal")]
        [NotMapped]
        public string ResidenciaFiscal { get { return this.Receptor.ResidenciaFiscal; } }
        //    get { return this.residenciaFiscal; }
        //    set { this.residenciaFiscal = value; }
        //}
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
        [MaxLength(40)]
        //[XmlAttribute("NumRegIdTrib")]
        [NotMapped]        
        public string NumRegIdTrib { get { return this.Receptor.NumRegIdTrib; } }
        //    get { return this.numRegIdTrib; }
        //    set { this.numRegIdTrib = value; }
        //}
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
        [NotMapped]
        //[XmlAttribute("UsoCFDI")]
        public string UsoCFDI { get { return this.Receptor.UsoCFDI; } }
        //    get { return this.usoCFDI; }
        //    set { this.usoCFDI = value; }
        //}
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
        

        /// <summary>
        /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
        /// </summary>
        [XmlElement("Receptor", typeof(Receptor))]
        public virtual Receptor Receptor { get; set; }
        //public virtual Receptor Receptor {
        //    get { return this.receptor; }
        //    set { this.receptor = value; }
        //}

        [ForeignKey("Domicilio")]
        public int? DomicilioId { get; set; }

        /// <summary>
        /// Nodo opcional para la definición de la ubicación donde se da el domicilio del receptor del comprobante fiscal.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:sequence>
        ///   <xs:element name="Domicilio" type="cfdi:t_Ubicacion" minOccurs="0">
        ///     <xs:annotation>
        ///       <xs:documentation>
        ///         Nodo opcional para la definición de la ubicación donde se da el domicilio del receptor del comprobante fiscal.
        ///       </xs:documentation>
        ///     </xs:annotation>
        ///   </xs:element>
        /// </xs:sequence>
        /// </code>
        /// </remarks>
        [XmlElement("Domicilio")]
        public virtual Ubicacion Domicilio { get; set; }
        //    get { return this.domicilio; }
        //    set { this.domicilio = value; }
        //}
    }

    
}