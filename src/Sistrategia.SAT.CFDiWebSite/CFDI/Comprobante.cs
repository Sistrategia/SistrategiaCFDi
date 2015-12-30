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
    public class Comprobante
    {
        #region Private Fields
        private string version;
        private string serie;   // opcional en CFDi
        private string folio;   // opcional en CFDi
        private DateTime fecha;
        private string sello;

        private string noAprobacion;
        private string anoAprobacion;

        private string formaDePago;
        private string noCertificado;
        private bool hasNoCertificado;
        private string certificado;
        private bool hasCertificado;
        private string condicionesDePago;
        private decimal subTotal;
        //private decimal descuento;
        //private bool descuentoSpecified;
        private decimal? descuento;
        private string motivoDescuento;

        private string tipoCambio;
        private string moneda;

        private decimal total;
        //private ComprobanteTipoDeComprobante tipoDeComprobante;
        private string tipoDeComprobante;
        private string metodoDePago;
        private string lugarExpedicionField;
        private string numCtaPagoField;
        private string folioFiscalOrigField;
        //private bool fechaFolioFiscalOrigFieldSpecified = false;
        private string serieFolioFiscalOrigField;
        private DateTime? fechaFolioFiscalOrigField;
        private decimal? montoFolioFiscalOrigField;
        //private bool montoFolioFiscalOrigFieldSpecified = false;

       

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
            this.version = "3.2";
            //this.emisor = new Emisor();
            //this.receptor = new Receptor();
        }
        #endregion

        public string DecimalFormat {
            get {
                //return "0.00";
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
                xslt.Load("http://www.sat.gob.mx/sitio_internet/cfd/3/cadenaoriginal_3_2/cadenaoriginal_3_2.xslts");
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
                        string info = string.Format("?re={0}&rr={1}&tt={2}&id={3}",
                        this.Emisor.RFC, this.Receptor.RFC, this.Total.ToString(this.DecimalFormat), timbre.UUID);
                        string cbb = SATManager.GetQrCode(info);
                        return cbb;
                    }
                }
            }
            return string.Empty;
        }

        [Key]
        public int ComprobanteId { get; set; }

        [Required]
        public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido con valor prefijado a 3.2 que indica la versión del estándar bajo el que se encuentra expresado el comprobante.
        /// </summary>
        /// <remarks>
        /// Requerido con valor prefijado a 3.2
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlAttribute("version")]
        public string Version {
            get { return version; }
            set {
                //if (value != "3.2") {
                //    throw new ArgumentException("Atributo requerido con valor prefijado a 3.2");
                //}
                this.version = value; // validar las posibles versiones
            }
        }

        /// <summary>
        /// Atributo opcional para precisar la serie para control interno del contribuyente. Este atributo acepta una cadena de caracteres alfabéticos de 1 a 25 caracteres sin incluir caracteres acentuados.
        /// </summary>
        /// <remarks>
        /// Opcional
        /// El largo debe estar entre 1 y 25 caracteres
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlAttribute("serie")]
        public string Serie {
            get { return this.serie; }
            set { this.serie = value; }
        }

        /// <summary>
        ///   Atributo opcional para control interno del contribuyente que acepta un valor numérico entero superior a 0 que expresa el folio del comprobante.
        /// </summary>
        /// <remarks>
        /// opcional
        /// El largo debe estar entre 1 y 20 caracteres
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlAttribute("folio")]
        public string Folio {
            get { return this.folio; }
            set { this.folio = value; }
        }

        /// <summary>
        /// Atributo requerido para la expresión de la fecha y hora de expedición del comprobante fiscal. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// Fecha y hora de expedición del comprobante fiscal
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlAttribute("fecha")]
        public DateTime Fecha {
            get { return this.fecha; }
            set {
                // this.fecha = value; 
                string fechaString = Convert.ToDateTime(value).ToString("dd/MM/yyyy HH:mm:ss");
                IFormatProvider culture = new System.Globalization.CultureInfo("es-MX", true);
                value = DateTime.ParseExact(fechaString, "dd/MM/yyyy HH:mm:ss", culture);
                this.fecha = value;
            }
        }

        /// <summary>
        /// Atributo requerido para contener el sello digital del comprobante fiscal, al que hacen referencia las reglas de resolución miscelánea aplicable. El sello deberá ser expresado cómo una cadena de texto en formato Base 64.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// El sello deberá ser expresado cómo una cadena de texto en formato Base 64.
        /// No debe contener espacios en blanco
        /// </remarks>
        [XmlAttribute("sello")]
        public string Sello {
            get { return this.sello; }
            set { this.sello = value; }
        }

        /// <summary>
        /// Atributo requerido para precisar el número de aprobación emitido por el SAT, para el rango de folios al que pertenece el folio particular que ampara el comprobante fiscal digital.
        /// </summary>
        /// <remarks>
        /// Este atributo no está presente en la versión 3.0 y 3.2 (Exclusivo de CFD)
        /// </remarks>
        [XmlAttribute("noAprobacion", DataType = "integer")]
        //[XmlAttributeAttribute("noAprobacion", typeof(System.Decimal))]
        public string NoAprobacion {
            get { return this.noAprobacion; }
            set { this.noAprobacion = value; }
            //get { return this.currentData.NoAprobacion; }
            //set { this.currentData.NoAprobacion = value; }
        }

        /// <summary>
        /// Atributo requerido para precisar el año en que se solicito el folio que se están utilizando para emitir el comprobante fiscal digital.
        /// </summary>
        /// <remarks>
        /// 4 Dígitos
        /// Este atributo empezó en la versión 2.0 hasta la versión 2.2 (no se encuentra en la versión 1.0)
        /// Este atributo no está presente en la versión 3.0 y 3.2 (Exclusivo de CFD)
        /// </remarks>
        [XmlAttribute("anoAprobacion", DataType = "integer")]
        public string AnoAprobacion {
            get { return this.anoAprobacion; }
            set { this.anoAprobacion = value; }
            //get { return this.currentData.AnoAprobacion; }
            //set { this.currentData.AnoAprobacion = value; }
        }

        /// <summary>
        /// Atributo requerido para precisar la forma de pago que aplica para este comprobante fiscal digital a través de Internet. Se utiliza para expresar Pago en una sola exhibición o número de parcialidad pagada contra el total de  parcialidades, Parcialidad 1 de X.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// No debe contener espacios en blanco
        /// <code>
        /// <xs:attribute name="formaDePago" use="required">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo requerido para precisar la forma de pago que aplica para este comprobnante fiscal digital a través de Internet. Se utiliza para expresar Pago en una sola exhibición o número de parcialidad pagada contra el total de parcialidades, Parcialidad 1 de X.
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
        [XmlAttribute("formaDePago")]
        public string FormaDePago {
            get { return this.formaDePago; }
            set { this.formaDePago = value; }
        }


        /// <summary>
        /// Atributo requerido para expresar el número de serie del certificado de sello digital que ampara al comprobante, de acuerdo al acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.
        /// </summary>
        /// <remarks>
        /// Requerido
        /// No debe contener espacios en blanco
        /// El largo debe estar a 20 caracteres
        /// <code>
        /// <xs:attribute name="noCertificado" use="required">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo requerido para expresar el número de serie del certificado de sello digital que ampara al comprobante, de acuerdo al acuse correspondiente a 20 posiciones otorgado por el sistema del SAT.
        ///       </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///       <xs:restriction base="xs:string">
        ///           <xs:length value="20"/>
        ///           <xs:whiteSpace value="collapse"/>
        ///       </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("noCertificado")]
        public string NoCertificado {
            get {
                if (this.HasNoCertificado && this.Certificado != null)
                    return this.Certificado.NumSerie;
                else
                    return null;
            }
            set { this.noCertificado = value; }
        }

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
        /// Atributo requerido que sirve para expresar el certificado de sello digital que ampara al comprobante como texto, en formato base 64.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="certificado" use="required">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo requerido que sirve para expresar el certificado de sello digital que ampara al comprobante como texto, en formato base 64.
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
        public string CertificadoBase64 {
            get {
                if (this.HasCertificado && this.Certificado != null)
                    return this.Certificado.CertificadoBase64;
                else
                    return null;
            }
            //get { return this.certificado; }
            set { this.certificado = value; }
        }

        

        /// <summary>
        /// Atributo opcional para expresar las condiciones comerciales aplicables para el pago del comprobante fiscal digital a través de Internet.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="condicionesDePago" use="optional">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo opcional para expresar las condiciones comerciales aplicables para el pago del comprobante fiscal digital a través de Internet.
        ///       </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///       <xs:restriction base="xs:string">
        ///           <xs:whiteSpace value="collapse"/>
        ///           <xs:minLength value="1"/>
        ///       </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("condicionesDePago")]
        public string CondicionesDePago {
            get { return this.condicionesDePago; }
            set { this.condicionesDePago = value; }
        }

        /// <summary>
        /// Atributo requerido para representar la suma de los importes antes de descuentos e impuestos.
        /// </summary>
        /// <remarks>
        /// Tipo t_Importe a 6 decimales
        /// <code>
        /// <xs:attribute name="subTotal" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo requerido para representar la suma de los importes antes de descuentos e impuestos.
        ///       </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("subTotal")]        
        public decimal SubTotal {
            get { return this.subTotal; }
            set { this.subTotal = value; }
        }

        /// <summary>
        /// Atributo opcional para representar el importe total de los descuentos aplicables antes de impuestos
        /// </summary>
        /// <remarks>
        /// Tipo t_Importe a 6 decimales
        /// <code>
        /// <xs:attribute name="descuento" type="cfdi:t_Importe" use="optional">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo opcional para representar el importe total de los descuentos aplicables antes de impuestos.
        ///       </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("descuento")]
        //public decimal Descuento {
        public decimal? Descuento {
            get { return this.descuento; }
            set { this.descuento = value; }
        }

        //[XmlIgnore]
        //public bool DescuentoSpecified {
        //    get { return this.descuentoSpecified; }
        //    set { this.descuentoSpecified = value; }
        //}

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
        public string MotivoDescuento {
            get { return this.motivoDescuento; }
            set { this.motivoDescuento = value; }
        }

        /// <summary>
        /// Atributo opcional para representar el tipo de cambio conforme a la moneda usada
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="TipoCambio">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo opcional para representar el tipo de cambio conforme a la moneda usada
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
        [XmlAttribute("TipoCambio")]
        public string TipoCambio {
            get { return this.tipoCambio; }
            set { this.tipoCambio = value; }
        }

        /// <summary>
        /// Atributo opcional para expresar la moneda utilizada para expresar los montos.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="Moneda">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo opcional para expresar la moneda utilizada para expresar los montos
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
        [XmlAttribute("Moneda")]
        public string Moneda {
            get { return this.moneda; }
            set { this.moneda = value; }
        }

        /// <summary>
        /// Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, más los impuestos trasladados, menos los impuestos retenidos.
        /// </summary>
        /// <remarks>
        /// Tipo t_Importe a 6 decimales
        /// <code>
        /// <xs:attribute name="total" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, más los impuestos trasladados, menos los impuestos retenidos.
        ///       </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("total")]
        public decimal Total {
            get { return this.total; }
            set { this.total = value; }
        }

        ///// <summary>
        ///// Atributo requerido para expresar el efecto del comprobante fiscal para el contribuyente emisor.
        ///// </summary>
        ///// <remarks>
        ///// <code>
        ///// <xs:attribute name="tipoDeComprobante" use="required">
        /////   <xs:annotation>
        /////       <xs:documentation>
        /////           Atributo requerido para expresar el efecto del comprobante fiscal para el contribuyente emisor.
        /////       </xs:documentation>
        /////   </xs:annotation>
        /////   <xs:simpleType>
        /////       <xs:restriction base="xs:string">
        /////           <xs:enumeration value="ingreso"/>
        /////           <xs:enumeration value="egreso"/>
        /////           <xs:enumeration value="traslado"/>
        /////       </xs:restriction>
        /////   </xs:simpleType>
        ///// </xs:attribute>
        ///// </code>
        ///// </remarks>
        //[XmlAttribute("tipoDeComprobante")]
        //public ComprobanteTipoDeComprobante TipoDeComprobante {
        //    get { return this.tipoDeComprobante; }
        //    set { this.tipoDeComprobante = value; }
        //}

        /// <summary>
        /// Atributo requerido para expresar el efecto del comprobante fiscal para el contribuyente emisor.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="tipoDeComprobante" use="required">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo requerido para expresar el efecto del comprobante fiscal para el contribuyente emisor.
        ///       </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///       <xs:restriction base="xs:string">
        ///           <xs:enumeration value="ingreso"/>
        ///           <xs:enumeration value="egreso"/>
        ///           <xs:enumeration value="traslado"/>
        ///       </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("tipoDeComprobante")]
        public string TipoDeComprobante {
        //public ComprobanteTipoDeComprobante TipoDeComprobante {
            get { return this.tipoDeComprobante; }
            set { this.tipoDeComprobante = value; }
        }

        /// <summary>		
        /// Atributo requerido de texto libre para expresar el método de pago de los bienes o servicios amparados por el comprobante. Se entiende como método de pago leyendas tales como: cheque, tarjeta de crédito o debito, depósito en cuenta, etc.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="metodoDePago" use="required">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo requerido de texto libre para expresar el método de pago de los bienes o servicios amparados por el comprobante. Se entiende como método de pago leyendas tales como: cheque, tarjeta de crédito o debito, depósito en cuenta, etc.
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
        [XmlAttribute("metodoDePago")]
        public string MetodoDePago {
            get { return this.metodoDePago; }
            set { this.metodoDePago = value; }
        }

        /// <summary>
        /// Atributo requerido para incorporar el lugar de expedición del comprobante.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="LugarExpedicion" use="required">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo requerido para incorporar el lugar de expedición del comprobante.
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
        [XmlAttribute("LugarExpedicion")]
        public string LugarExpedicion {
            get { return this.lugarExpedicionField; }
            set { this.lugarExpedicionField = value; }
        }

        /// <summary>
        /// Atributo Opcional para incorporar al menos los cuatro últimos digitos del número de cuenta con la que se realizó el pago.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="NumCtaPago">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo Opcional para incorporar al menos los cuatro últimos digitos del número de cuenta con la que se realizó el pago.
        ///       </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///       <xs:restriction base="xs:string">
        ///           <xs:minLength value="4"/>
        ///           <xs:whiteSpace value="collapse"/>
        ///       </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("NumCtaPago")]
        public string NumCtaPago {
            get { return this.numCtaPagoField; }
            set { this.numCtaPagoField = value; }
        }

        /// <summary>
        /// Atributo opcional para señalar el número de folio fiscal del comprobante que se hubiese expedido por el valor total del comprobante, tratándose del pago en parcialidades.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="FolioFiscalOrig">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo opcional para señalar el número de folio fiscal del comprobante que se hubiese expedido por el valor total del comprobante, tratándose del pago en parcialidades.
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
        [XmlAttribute("FolioFiscalOrig")]
        public string FolioFiscalOrig {
            get { return this.folioFiscalOrigField; }
            set { this.folioFiscalOrigField = value; }
        }

        /// <summary>
        /// Atributo opcional para señalar la serie del folio del comprobante que se hubiese expedido por el valor total del comprobante, tratándose del pago en parcialidades.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="SerieFolioFiscalOrig">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo opcional para señalar la serie del folio del comprobante que se hubiese expedido por el valor total del comprobante, tratándose del pago en parcialidades.
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
        [XmlAttribute("SerieFolioFiscalOrig")]
        public string SerieFolioFiscalOrig {
            get { return this.serieFolioFiscalOrigField; }
            set { this.serieFolioFiscalOrigField = value; }
        }

        /// <summary>
        /// Atributo opcional para señalar la fecha de expedición del comprobante que se hubiese emitido por el valor total del comprobante, tratándose del pago en parcialidades. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
        /// </summary>
        /// <remarks>
        /// <code>
        //<xs:attribute name="FechaFolioFiscalOrig">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo opcional para señalar la fecha de expedición del comprobante que se hubiese emitido por el valor total del comprobante, tratándose del pago en parcialidades. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
        ///       </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///       <xs:restriction base="xs:dateTime">
        ///           <xs:whiteSpace value="collapse"/>
        ///       </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("FechaFolioFiscalOrig")]
        public System.DateTime? FechaFolioFiscalOrig {
        //public System.DateTime FechaFolioFiscalOrig {
            get { return this.fechaFolioFiscalOrigField; }
            set { this.fechaFolioFiscalOrigField = value; }
        }


        //[XmlIgnoreAttribute()]
        //public bool FechaFolioFiscalOrigSpecified {
        //    get { return this.fechaFolioFiscalOrigFieldSpecified; }
        //    set { this.fechaFolioFiscalOrigFieldSpecified = value; }
        //}

        /// <summary>
        /// Atributo opcional para señalar el total del comprobante que se hubiese expedido por el valor total de la operación, tratándose del pago en parcialidades
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="MontoFolioFiscalOrig" type="cfdi:t_Importe">
        ///   <xs:annotation>
        ///       <xs:documentation>
        ///           Atributo opcional para señalar el total del comprobante que se hubiese expedido por el valor total de la operación, tratándose del pago en parcialidades
        ///       </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("MontoFolioFiscalOrig")]
        public decimal? MontoFolioFiscalOrig {
        //public decimal MontoFolioFiscalOrig {
            get { return this.montoFolioFiscalOrigField; }
            set { this.montoFolioFiscalOrigField = value; }
        }


        //[XmlIgnoreAttribute()]
        //public bool MontoFolioFiscalOrigSpecified {
        //    get { return this.montoFolioFiscalOrigFieldSpecified; }
        //    set { this.montoFolioFiscalOrigFieldSpecified = value; }
        //}

        [ForeignKey("Emisor")]
        public int? EmisorId { get; set; }

        /// <summary>
        /// Nodo requerido para expresar la información del contribuyente emisor del comprobante.
        /// </summary>
        [XmlElement("Emisor", typeof(Emisor))]
        public virtual Emisor Emisor { get; set; }
        //public virtual Emisor Emisor {
        //    get { return this.emisor; }
        //    set { this.emisor = value; }
        //}

        [ForeignKey("Receptor")]
        public int? ReceptorId { get; set; }

        /// <summary>
        /// Nodo requerido para precisar la información del contribuyente receptor del comprobante.
        /// </summary>
        [XmlElement("Receptor", typeof(Receptor))]
        public virtual Receptor Receptor { get; set; }
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
        public string GeneratedXmlUrl { get; set; }

        [XmlIgnore]
        public string GeneratedPDFUrl { get; set; }

        [XmlIgnore]
        public string Status { get; set; }

    }

    //public enum ComprobanteTipoDeComprobante
    //{
    //    ingreso,
    //    egreso,
    //    traslado
    //}

    public class Complemento
    {
        [Key]
        public int ComplementoId { get; set; }

        [XmlIgnore]
        public int? Ordinal { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        //private System.Xml.XmlElement[] any;

        //[XmlAnyElement]
        //public virtual List<System.Xml.XmlElement> Any { get; set; }
        ////public System.Xml.XmlElement[] Any {
        ////    get { return this.any; }
        ////    set { this.any = value; }
        ////}

        //[ForeignKey("TimbreFiscalDigital")]
        //public int? TimbreFiscalDigitalId { get; set; }

        //public virtual TimbreFiscalDigital TimbreFiscalDigital { get; set; }

        ///// <summary>
        ///// Nodo opcional para capturar los impuestos retenidos aplicables
        ///// </summary>
        //[XmlArrayItem("Retencion", IsNullable = false)]
        //public virtual List<Retencion> Retenciones { get; set; }
        ////public Retencion[] Retenciones {
        ////    get { return this.retenciones; }
        ////    set { this.retenciones = value; }
        ////}
    }

    //public class Complemento // : System.Xml.XmlDocument
    //{
    //    public int ComplementoId { get; set; }

    //    public virtual string Complemento { get; set; }

    //    //private System.Xml.XmlElement[] any;

    //    //[XmlAnyElement]
    //    //public virtual List<System.Xml.XmlElement> Any { get; set; }
    //    //public System.Xml.XmlElement[] Any {
    //    //    get { return this.any; }
    //    //    set { this.any = value; }
    //    //}
    //}

    public class Impuestos
    {
        #region
        private decimal? totalImpuestosRetenidos;
        //private bool totalImpuestosRetenidosSpecified = false;
        private decimal? totalImpuestosTrasladados;
        //private bool totalImpuestosTrasladadosSpecified = false;
        #endregion
        [Key]
        public int ImpuestosId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        /// <summary>
        /// Nodo opcional para capturar los impuestos retenidos aplicables
        /// </summary>
        [XmlArrayItem("Retencion", IsNullable = false)]
        public virtual List<Retencion> Retenciones { get; set; }
        //public Retencion[] Retenciones {
        //    get { return this.retenciones; }
        //    set { this.retenciones = value; }
        //}

        /// <summary>
        /// Nodo opcional para asentar o referir los impuestos trasladados aplicables
        /// </summary>
        [XmlArrayItem("Traslado", IsNullable = false)]
        public virtual List<Traslado> Traslados { get; set; }
        //public Traslado[] Traslados {
        //    get { return this.traslados; }
        //    set { this.traslados = value; }
        //}

        /// <summary>
        /// Atributo opcional para expresar el total de los impuestos retenidos que se desprenden 
        /// de los conceptos expresados en el comprobante fiscal digital a través de Internet.
        /// </summary>
        [XmlAttribute("totalImpuestosRetenidos")]
        public decimal? TotalImpuestosRetenidos {
            get { return this.totalImpuestosRetenidos; }
            set { this.totalImpuestosRetenidos = value; }
        }
        //<xs:attribute name="totalImpuestosRetenidos" type="cfdi:t_Importe" use="optional">
        //    <xs:annotation>
        //    <xs:documentation>Atributo opcional para expresar el total de los impuestos retenidos que se desprenden de los conceptos expresados en el comprobante fiscal digital a través de Internet.</xs:documentation>
        //    </xs:annotation>
        //</xs:attribute>


        //[XmlIgnore]
        //public bool TotalImpuestosRetenidosSpecified {
        //    get { return this.totalImpuestosRetenidosSpecified; }
        //    set { this.totalImpuestosRetenidosSpecified = value; }
        //}

        /// <summary>
        /// Atributo opcional para expresar el total de los impuestos trasladados que se desprenden 
        /// de los conceptos expresados en el comprobante fiscal digital a través de Internet.
        /// </summary>
        [XmlAttribute("totalImpuestosTrasladados")]
        public decimal? TotalImpuestosTrasladados {
            get { return this.totalImpuestosTrasladados; }
            set { this.totalImpuestosTrasladados = value; }
        }
        //<xs:attribute name="totalImpuestosTrasladados" type="cfdi:t_Importe" use="optional">
        //    <xs:annotation>
        //    <xs:documentation>Atributo opcional para expresar el total de los impuestos trasladados que se desprenden de los conceptos expresados en el comprobante fiscal digital a través de Internet.</xs:documentation>
        //    </xs:annotation>
        //</xs:attribute>

        //[XmlIgnore]
        //public bool TotalImpuestosTrasladadosSpecified {
        //    get { return this.totalImpuestosTrasladadosSpecified; }
        //    set { this.totalImpuestosTrasladadosSpecified = value; }
        //}
    }


    public class Retencion
    {
        public Retencion() {
            //this.PublicKey = Guid.NewGuid();
        }

        //private RetencionImpuesto impuesto;
        private string impuesto;
        private decimal importe;

        [Key]
        public int RetencionId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido para señalar el tipo de impuesto retenido
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="impuesto" use="required">
        ///     <xs:annotation>
        ///         <xs:documentation>Atributo requerido para señalar el tipo de impuesto retenido</xs:documentation>
        ///     </xs:annotation>
        ///     <xs:simpleType>
        ///         <xs:restriction base="xs:string">
        ///             <xs:whiteSpace value="collapse"/>
        ///             <xs:enumeration value="ISR">
        ///                 <xs:annotation>
        ///                     <xs:documentation>Impuesto sobre la renta</xs:documentation>
        ///                 </xs:annotation>
        ///             </xs:enumeration>
        ///             <xs:enumeration value="IVA">
        ///                 <xs:annotation>
        ///                     <xs:documentation>Impuesto al Valor Agregado</xs:documentation>
        ///                 </xs:annotation>
        ///             </xs:enumeration>
        ///         </xs:restriction>
        ///     </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("impuesto")]
        public string Impuesto {
        //public RetencionImpuesto Impuesto {
            get { return this.impuesto; }
            set { this.impuesto = value; }
        }

        /// <summary>
        /// Atributo requerido para señalar el importe o monto del impuesto retenido.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="importe" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para señalar el importe o monto del impuesto retenido.
        ///     </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("importe")]
        public decimal Importe {
            get { return this.importe; }
            set { this.importe = value; }
        }

        ///// <summary>
        ///// Atributo requerido para señalar el tipo de impuesto retenido.
        ///// </summary>
        //[Serializable]
        //[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
        //public enum RetencionImpuesto
        //{
        //    /// <summary>
        //    /// Impuesto sobre la renta
        //    /// </summary>
        //    ISR,
        //    /// <summary>
        //    /// Impuesto al Valor Agregado
        //    /// </summary>
        //    IVA,
        //}
        ////<xs:simpleType>
        ////    <xs:restriction base="xs:string">
        ////        <xs:whiteSpace value="collapse"/>
        ////        <xs:enumeration value="ISR">
        ////            <xs:annotation>
        ////                <xs:documentation>Impuesto sobre la renta</xs:documentation>
        ////            </xs:annotation>
        ////        </xs:enumeration>
        ////        <xs:enumeration value="IVA">
        ////            <xs:annotation>
        ////                <xs:documentation>Impuesto al Valor Agregado</xs:documentation>
        ////            </xs:annotation>
        ////        </xs:enumeration>
        ////    </xs:restriction>
        ////</xs:simpleType>

        [XmlIgnore]
        public int? Ordinal { get; set; }

    }

    public class Traslado
    {
        public Traslado() {
            //this.PublicKey = Guid.NewGuid();
        }

        //private TrasladoImpuesto impuesto;
        private string impuesto;
        private decimal tasa;
        private decimal importe;

        [Key]
        public int TrasladoId { get; set; }

        //[Required]
        //public Guid PublicKey { get; set; }

        /// <summary>
        /// Atributo requerido para señalar el tipo de impuesto trasladado.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="impuesto" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para señalar el tipo de impuesto trasladado
        ///     </xs:documentation>
        ///   </xs:annotation>
        ///   <xs:simpleType>
        ///     <xs:restriction base="xs:string">
        ///       <xs:whiteSpace value="collapse"/>
        ///       <xs:enumeration value="IVA">
        ///       <xs:annotation>
        ///           <xs:documentation>Impuesto al Valor Agregado</xs:documentation>
        ///       </xs:annotation>
        ///       </xs:enumeration>
        ///       <xs:enumeration value="IEPS">
        ///          <xs:annotation>
        ///               <xs:documentation>Impuesto especial sobre productos y servicios</xs:documentation>
        ///           </xs:annotation>
        ///       </xs:enumeration>
        ///     </xs:restriction>
        ///   </xs:simpleType>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("impuesto")]
        public string Impuesto {
        //public TrasladoImpuesto Impuesto {
            get { return this.impuesto; }
            set { this.impuesto = value; }
        }        

        /// <summary>
        /// Atributo requerido para señalar la tasa del impuesto que se traslada por cada concepto amparado en el comprobante.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="tasa" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para señalar la tasa del impuesto que se traslada por cada concepto amparado en el comprobante.
        ///     </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("tasa")]
        public decimal Tasa {
            get { return this.tasa; }
            set { this.tasa = value; }
        }        

        /// <summary>
        /// Atributo requerido para señalar el importe del impuesto trasladado.
        /// </summary>
        /// <remarks>
        /// <code>
        /// <xs:attribute name="importe" type="cfdi:t_Importe" use="required">
        ///   <xs:annotation>
        ///     <xs:documentation>
        ///       Atributo requerido para señalar el importe del impuesto trasladado.
        ///     </xs:documentation>
        ///   </xs:annotation>
        /// </xs:attribute>
        /// </code>
        /// </remarks>
        [XmlAttribute("importe")]
        public decimal Importe {
            get { return this.importe; }
            set { this.importe = value; }
        }

        [XmlIgnore]
        public int? Ordinal { get; set; }
    }

    //public class Complemento
    //{

    //}

    ///// <summary>
    ///// Atributo requerido para señalar el tipo de impuesto trasladado
    ///// </summary>
    //[Serializable]
    //[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/3")]
    //public enum TrasladoImpuesto
    //{
    //    /// <summary>
    //    /// Impuesto al Valor Agregado
    //    /// </summary>
    //    IVA,
    //    /// <summary>
    //    /// Impuesto especial sobre productos y servicios
    //    /// </summary>
    //    IEPS,
    //}
    ////<xs:attribute name="impuesto" use="required">
    ////    <xs:annotation>
    ////    <xs:documentation>Atributo requerido para señalar el tipo de impuesto trasladado</xs:documentation>
    ////    </xs:annotation>
    ////    <xs:simpleType>
    ////    <xs:restriction base="xs:string">
    ////        <xs:whiteSpace value="collapse"/>
    ////        <xs:enumeration value="IVA">
    ////        <xs:annotation>
    ////            <xs:documentation>Impuesto al Valor Agregado</xs:documentation>
    ////        </xs:annotation>
    ////        </xs:enumeration>
    ////        <xs:enumeration value="IEPS">
    ////        <xs:annotation>
    ////            <xs:documentation>Impuesto especial sobre productos y servicios</xs:documentation>
    ////        </xs:annotation>
    ////        </xs:enumeration>
    ////    </xs:restriction>
    ////    </xs:simpleType>
    ////</xs:attribute>
    //}

    public class ViewTemplate
    {
        [Key]
        public int ViewTemplateId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public string CodeName { get; set; }
    }

    public class ReceptorCorreoEntrega
    {
        [Key]
        public int ReceptorCorreoEntregaId { get; set; }
        public string Correo { get; set; }
        //public string Nombre { get; set; }
    }
}