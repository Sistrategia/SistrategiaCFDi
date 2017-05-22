using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class CFDIXmlTextWriter
    {
        private Comprobante comprobante = null;
        private string fileName = null;
        private Encoding encoding = System.Text.Encoding.UTF8;
        private System.Xml.XmlTextWriter writer = null;

        public CFDIXmlTextWriter(Comprobante comprobante, string fileName, Encoding encoding) {
            this.comprobante = comprobante;
            this.fileName = fileName;
            this.encoding = encoding;

            this.writer = new System.Xml.XmlTextWriter(fileName, System.Text.Encoding.UTF8);
        }

        public CFDIXmlTextWriter(Comprobante comprobante, System.IO.Stream stream, Encoding encoding) {
            this.comprobante = comprobante;
            this.fileName = null;
            this.encoding = encoding;

            this.writer = new System.Xml.XmlTextWriter(stream, System.Text.Encoding.UTF8);
        }

        public void WriteXml() {
            if (1 != int.Parse(2.ToString())) {
                this.WriteXmlDirect();
            }
            else {
                this.WriteXmlWithSerializer();
            }
        }


        private void WriteXmlDirect() {
            // writer.Settings.
            writer.Formatting = System.Xml.Formatting.Indented;
            writer.WriteStartDocument();

            switch (comprobante.Version) {
                case "1.0":
                    writer.WriteStartElement("Comprobante");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    break;
                case "2.0":
                    writer.WriteStartElement("Comprobante", "http://www.sat.gob.mx/cfd/2");
                    writer.WriteAttributeString("xmlns", "http://www.sat.gob.mx/cfd/2");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.sat.gob.mx/cfd/2 http://www.sat.gob.mx/sitio_internet/cfd/2/cfdv2.xsd");
                    break;
                case "2.2":
                    writer.WriteStartElement("Comprobante", "http://www.sat.gob.mx/cfd/2");
                    writer.WriteAttributeString("xmlns", "http://www.sat.gob.mx/cfd/2");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.sat.gob.mx/cfd/2 cfdv22.xsd");
                    break;
                case "3.0":
                    writer.WriteStartElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3");
                    writer.WriteAttributeString("xmlns", "cfdi", null, "http://www.sat.gob.mx/cfd/3");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.sat.gob.mx/cfd/3 cfdv3.xsd");
                    break;
                case "3.3":
                    writer.WriteStartElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3");
                    writer.WriteAttributeString("xmlns", "cfdi", null, "http://www.sat.gob.mx/cfd/3");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd");
                    break;
                case "3.2":
                default:
                    writer.WriteStartElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3");
                    writer.WriteAttributeString("xmlns", "cfdi", null, "http://www.sat.gob.mx/cfd/3");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd");
                    break;
            }


            // Atributo requerido (con valor prefijado a 3.3 en la ultima versión) que indica la versión del estándar bajo el que se encuentra expresado el comprobante.            
            // Atributo requerido con valor prefijado a 3.3 que indica la versión del estándar bajo el que se encuentra expresado el comprobante.
            // No debe contener espacios en blanco            
            writer.WriteAttributeString("version", comprobante.Version);


            if (!string.IsNullOrEmpty(comprobante.Serie))
                writer.WriteAttributeString("serie", comprobante.Serie);
            if (!string.IsNullOrEmpty(comprobante.Folio))
                writer.WriteAttributeString("folio", comprobante.Folio);

            switch (comprobante.Version) {
                case "1.0":
                    // Atributo requerido para la expresión de la fecha de expedición del comprobante fiscal. Se expresa en la forma aaaa-mm-dd
                    writer.WriteAttributeString("fecha", comprobante.Fecha.ToString("yyyy-mm-dd"));
                    break;
                case "2.0":
                // Atributo requerido para la expresión de la fecha y hora de expedición  del comprobante fiscal. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
                // http://en.wikipedia.org/wiki/ISO_8601
                case "2.2":
                // Atributo requerido para la expresión de la fecha y hora de expedición  del comprobante fiscal. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.                                        
                case "3.0":
                // Atributo requerido para la expresión de la fecha y hora de expedición  del comprobante fiscal. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
                case "3.2":
                // Atributo requerido para la expresión de la fecha y hora de expedición  del comprobante fiscal. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
                case "3.3":
                // Atributo requerido para la expresión de la fecha y hora de expedición  del comprobante fiscal. Se expresa en la forma aaaa-mm-ddThh:mm:ss, de acuerdo con la especificación ISO 8601.
                default:
                    writer.WriteAttributeString("fecha", comprobante.Fecha.ToString("yyyy-MM-ddTHH:mm:ss"));
                    break;
            }


            if (!string.IsNullOrEmpty(comprobante.Sello))
                writer.WriteAttributeString("sello", comprobante.Sello);


            switch (comprobante.Version) {
                case "1.0":
                    // if (!string.IsNullOrEmpty(comprobante.NoAprobacion))
                    writer.WriteAttributeString("noAprobacion", comprobante.NoAprobacion);
                    break;
                case "2.0":
                case "2.2":
                    // if (!string.IsNullOrEmpty(comprobante.NoAprobacion))
                    writer.WriteAttributeString("noAprobacion", comprobante.NoAprobacion);
                    // if (!string.IsNullOrEmpty(comprobante.AnoAprobacion))
                    writer.WriteAttributeString("anoAprobacion", comprobante.AnoAprobacion);
                    break;
                case "3.0":
                case "3.2":
                default:
                    break;
            }

            switch (comprobante.Version) {
                case "1.0":
                    break;
                case "2.0":                    
                    writer.WriteAttributeString("tipoDeComprobante", comprobante.TipoDeComprobante.ToString());
                    break;
                case "2.2": 
                    writer.WriteAttributeString("tipoDeComprobante", comprobante.TipoDeComprobante.ToString());                    
                    break;
                case "3.0":                    
                    writer.WriteAttributeString("tipoDeComprobante", comprobante.TipoDeComprobante.ToString());
                    break;
                case "3.2":
                case "3.3":
                default:                    
                    writer.WriteAttributeString("tipoDeComprobante", comprobante.TipoDeComprobante.ToString());                    
                    break;
            }

            switch (comprobante.Version) {
                case "1.0":
                    if (!string.IsNullOrEmpty(comprobante.FormaDePago))
                        writer.WriteAttributeString("formaDePago", comprobante.FormaDePago);
                    break;
                case "2.0":
                case "2.2":
                case "3.0":
                case "3.2":
                default:
                    // ES OBLIGATORIO
                    // if (!string.IsNullOrEmpty(comprobante.FormaDePago))
                    writer.WriteAttributeString("formaDePago", comprobante.FormaDePago);
                    break;
            }
            
            // ESTE VA AQUI Temporalmente lo pongo más abajo por Figueroa
            //switch (comprobante.Version) {
            //    case "1.0":
            //        if (!string.IsNullOrEmpty(comprobante.NoCertificado))
            //            writer.WriteAttributeString("noCertificado", comprobante.NoCertificado);
            //        if (!string.IsNullOrEmpty(comprobante.CertificadoBase64))
            //            writer.WriteAttributeString("certificado", comprobante.CertificadoBase64);
            //        break;
            //    case "2.0":
            //    case "2.2":
            //        writer.WriteAttributeString("noCertificado", comprobante.NoCertificado); // requerido en el esquema
            //        if (!string.IsNullOrEmpty(comprobante.CertificadoBase64))
            //            writer.WriteAttributeString("certificado", comprobante.CertificadoBase64);
            //        break;
            //    case "3.0":
            //    case "3.2":
            //    default:
            //        // REQUERIDOS
            //        writer.WriteAttributeString("noCertificado", comprobante.NoCertificado); // requerido en el esquema
            //        writer.WriteAttributeString("certificado", comprobante.CertificadoBase64); // requerido en el esquema
            //        break;
            //}

            switch (comprobante.Version) {
                case "1.0":
                    break;
                case "2.0":
                case "2.2":
                case "3.0":
                case "3.2":
                default:
                    if (!string.IsNullOrEmpty(comprobante.CondicionesDePago))
                        writer.WriteAttributeString("condicionesDePago", comprobante.CondicionesDePago);
                    break;
            }



            switch (comprobante.Version) {
                case "1.0":
                    break;
                case "2.0":
                case "2.2":
                case "3.0":
                case "3.2":
                default:
                    // if (!string.IsNullOrEmpty(comprobante.CondicionesDePago))
                    writer.WriteAttributeString("subTotal", comprobante.SubTotal.ToString(comprobante.DecimalFormat));
                    //if (comprobante.DescuentoSpecified) {
                    //if (comprobante.Descuento != null) {
                    if (comprobante.Descuento.HasValue) {
                        writer.WriteAttributeString("descuento", comprobante.Descuento.Value.ToString(comprobante.DecimalFormat));
                    }
                    if (!string.IsNullOrEmpty(comprobante.MotivoDescuento))
                        writer.WriteAttributeString("motivoDescuento", comprobante.MotivoDescuento);
                    break;
            }



            switch (comprobante.Version) {
                case "1.0":
                    break;
                case "2.0":
                    writer.WriteAttributeString("total", comprobante.Total.ToString(comprobante.DecimalFormat));
                    break;
                case "2.2":
                    if (!string.IsNullOrEmpty(comprobante.TipoCambio))
                        writer.WriteAttributeString("TipoCambio", comprobante.TipoCambio);
                    if (!string.IsNullOrEmpty(comprobante.Moneda))
                        writer.WriteAttributeString("Moneda", comprobante.Moneda);
                    writer.WriteAttributeString("total", comprobante.Total.ToString(comprobante.DecimalFormat));
                    break;
                case "3.0":
                    if (!string.IsNullOrEmpty(comprobante.TipoCambio))
                        writer.WriteAttributeString("TipoCambio", comprobante.TipoCambio);
                    if (!string.IsNullOrEmpty(comprobante.Moneda))
                        writer.WriteAttributeString("Moneda", comprobante.Moneda);
                    writer.WriteAttributeString("total", comprobante.Total.ToString(comprobante.DecimalFormat));
                    break;
                case "3.2":
                default:
                    if (!string.IsNullOrEmpty(comprobante.TipoCambio))
                        writer.WriteAttributeString("TipoCambio", comprobante.TipoCambio);
                    if (!string.IsNullOrEmpty(comprobante.Moneda))
                        writer.WriteAttributeString("Moneda", comprobante.Moneda);
                    writer.WriteAttributeString("total", comprobante.Total.ToString(comprobante.DecimalFormat));
                    break;
            }



            // ARREGLAR PORQUE ESTA ERA LA CORRECTA!!!!!!!
            //switch (comprobante.Version) {
            //    case "1.0":
            //        break;
            //    case "2.0":
            //        if (!string.IsNullOrEmpty(comprobante.MetodoDePago))
            //            writer.WriteAttributeString("metodoDePago", comprobante.MetodoDePago);
            //        //if (!string.IsNullOrEmpty(comprobante.TipoDeComprobante))
            //        writer.WriteAttributeString("tipoDeComprobante", comprobante.TipoDeComprobante.ToString());
            //        break;
            //    case "2.2": // SE INVIERTEN ORDEN
            //        //if (!string.IsNullOrEmpty(comprobante.TipoDeComprobante))
            //        writer.WriteAttributeString("tipoDeComprobante", comprobante.TipoDeComprobante.ToString());
            //        if (!string.IsNullOrEmpty(comprobante.MetodoDePago))
            //            writer.WriteAttributeString("metodoDePago", comprobante.MetodoDePago);
            //        break;
            //    case "3.0":
            //        if (!string.IsNullOrEmpty(comprobante.MetodoDePago))
            //            writer.WriteAttributeString("metodoDePago", comprobante.MetodoDePago);
            //        //if (!string.IsNullOrEmpty(comprobante.TipoDeComprobante))
            //        writer.WriteAttributeString("tipoDeComprobante", comprobante.TipoDeComprobante.ToString());
            //        break;
            //    case "3.2":
            //    default:
            //        //if (!string.IsNullOrEmpty(comprobante.TipoDeComprobante))
            //        writer.WriteAttributeString("tipoDeComprobante", comprobante.TipoDeComprobante.ToString());
            //        //if (!string.IsNullOrEmpty(comprobante.MetodoDePago))
            //        writer.WriteAttributeString("metodoDePago", comprobante.MetodoDePago);
            //        break;
            //}


            switch (comprobante.Version) {
                case "1.0":
                    break;
                case "2.0":
                    if (!string.IsNullOrEmpty(comprobante.MetodoDePago))
                        writer.WriteAttributeString("metodoDePago", comprobante.MetodoDePago);                    
                    break;
                case "2.2": // SE INVIERTEN ORDEN                   
                    if (!string.IsNullOrEmpty(comprobante.MetodoDePago))
                        writer.WriteAttributeString("metodoDePago", comprobante.MetodoDePago);
                    break;
                case "3.0":
                    if (!string.IsNullOrEmpty(comprobante.MetodoDePago))
                        writer.WriteAttributeString("metodoDePago", comprobante.MetodoDePago);                   
                    break;
                case "3.2":
                default:                   
                    writer.WriteAttributeString("metodoDePago", comprobante.MetodoDePago);
                    break;
            }

            switch (comprobante.Version) {
                case "1.0":
                case "2.0":
                    break;
                case "2.2":

                    if (!string.IsNullOrEmpty(comprobante.LugarExpedicion))
                        writer.WriteAttributeString("LugarExpedicion", comprobante.LugarExpedicion);
                    if (!string.IsNullOrEmpty(comprobante.NumCtaPago))
                        writer.WriteAttributeString("NumCtaPago", comprobante.NumCtaPago);
                    if (!string.IsNullOrEmpty(comprobante.FolioFiscalOrig))
                        writer.WriteAttributeString("FolioFiscalOrig", comprobante.FolioFiscalOrig);
                    if (!string.IsNullOrEmpty(comprobante.SerieFolioFiscalOrig))
                        writer.WriteAttributeString("SerieFolioFiscalOrig", comprobante.SerieFolioFiscalOrig);

                    //if (comprobante.FechaFolioFiscalOrigSpecified && comprobante.FechaFolioFiscalOrig.HasValue) //if (!string.IsNullOrEmpty(comprobante.FechaFolioFiscalOrig))
                    if (comprobante.FechaFolioFiscalOrig.HasValue)
                        writer.WriteAttributeString("FechaFolioFiscalOrig", comprobante.FechaFolioFiscalOrig.Value.ToString("yyyy-MM-ddTHH:mm:ss"));

                    //if (comprobante.MontoFolioFiscalOrigSpecified && comprobante.MontoFolioFiscalOrig.HasValue) // if (!string.IsNullOrEmpty(comprobante.MontoFolioFiscalOrig))
                    if (comprobante.MontoFolioFiscalOrig.HasValue)
                        writer.WriteAttributeString("MontoFolioFiscalOrig", comprobante.MontoFolioFiscalOrig.Value.ToString(comprobante.DecimalFormat));

                    break;
                case "3.0":
                    break;
                case "3.2":
                default:
                    if (!string.IsNullOrEmpty(comprobante.LugarExpedicion))
                        writer.WriteAttributeString("LugarExpedicion", comprobante.LugarExpedicion);
                    if (!string.IsNullOrEmpty(comprobante.NumCtaPago))
                        writer.WriteAttributeString("NumCtaPago", comprobante.NumCtaPago);
                    if (!string.IsNullOrEmpty(comprobante.FolioFiscalOrig))
                        writer.WriteAttributeString("FolioFiscalOrig", comprobante.FolioFiscalOrig);
                    if (!string.IsNullOrEmpty(comprobante.SerieFolioFiscalOrig))
                        writer.WriteAttributeString("SerieFolioFiscalOrig", comprobante.SerieFolioFiscalOrig);

                    //if (comprobante.FechaFolioFiscalOrigSpecified && comprobante.FechaFolioFiscalOrig.HasValue) //if (!string.IsNullOrEmpty(comprobante.FechaFolioFiscalOrig))
                    if (comprobante.FechaFolioFiscalOrig.HasValue)
                        writer.WriteAttributeString("FechaFolioFiscalOrig", comprobante.FechaFolioFiscalOrig.Value.ToString("yyyy-MM-ddTHH:mm:ss"));

                    //if (comprobante.MontoFolioFiscalOrigSpecified && comprobante.MontoFolioFiscalOrig.HasValue) // if (!string.IsNullOrEmpty(comprobante.MontoFolioFiscalOrig))
                    if (comprobante.MontoFolioFiscalOrig.HasValue) 
                        writer.WriteAttributeString("MontoFolioFiscalOrig", comprobante.MontoFolioFiscalOrig.Value.ToString(comprobante.DecimalFormat));
                    break;
            }

            switch (comprobante.Version) {
                case "1.0":
                    if (!string.IsNullOrEmpty(comprobante.NoCertificado))
                        writer.WriteAttributeString("noCertificado", comprobante.NoCertificado);
                    if (!string.IsNullOrEmpty(comprobante.CertificadoBase64))
                        writer.WriteAttributeString("certificado", comprobante.CertificadoBase64);
                    break;
                case "2.0":
                case "2.2":
                    writer.WriteAttributeString("noCertificado", comprobante.NoCertificado); // requerido en el esquema
                    if (!string.IsNullOrEmpty(comprobante.CertificadoBase64))
                        writer.WriteAttributeString("certificado", comprobante.CertificadoBase64);
                    break;
                case "3.0":
                case "3.2":
                default:
                    // REQUERIDOS
                    writer.WriteAttributeString("noCertificado", comprobante.NoCertificado); // requerido en el esquema
                    writer.WriteAttributeString("certificado", comprobante.CertificadoBase64); // requerido en el esquema
                    break;
            }

            // Emisor Obligatorio
            //if (comprobante.Emisor != null && !string.IsNullOrEmpty(comprobante.Emisor.RFC)) {
            switch (comprobante.Version) {
                case "1.0":
                    writer.WriteStartElement("Emisor");
                    break;
                case "2.0":
                case "2.2":
                    writer.WriteStartElement("Emisor", "http://www.sat.gob.mx/cfd/2");
                    break;
                case "3.0":
                case "3.2":
                default:
                    writer.WriteStartElement("cfdi", "Emisor", "http://www.sat.gob.mx/cfd/3");
                    break;
            }

            // Requeridos
            //if (!string.IsNullOrEmpty(comprobante.Emisor.RFC))
            writer.WriteAttributeString("rfc", comprobante.Emisor.RFC);
            //if (!string.IsNullOrEmpty(comprobante.Emisor.Nombre))
            writer.WriteAttributeString("nombre", comprobante.Emisor.Nombre);

            // Ubicación Fiscal 1.0 2.0 y 3.0 era requerido
            if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Pais)) {
                switch (comprobante.Version) {
                    case "1.0":
                        writer.WriteStartElement("DomicilioFiscal");
                        break;
                    case "2.0":
                    case "2.2":
                        writer.WriteStartElement("DomicilioFiscal", "http://www.sat.gob.mx/cfd/2");
                        break;
                    case "3.0":
                    case "3.2":
                    default:
                        writer.WriteStartElement("cfdi", "DomicilioFiscal", "http://www.sat.gob.mx/cfd/3");
                        break;
                }

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Calle))
                    writer.WriteAttributeString("calle", comprobante.Emisor.DomicilioFiscal.Calle);

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.NoExterior))
                    writer.WriteAttributeString("noExterior", comprobante.Emisor.DomicilioFiscal.NoExterior);

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.NoInterior))
                    writer.WriteAttributeString("noInterior", comprobante.Emisor.DomicilioFiscal.NoInterior);

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Colonia))
                    writer.WriteAttributeString("colonia", comprobante.Emisor.DomicilioFiscal.Colonia);

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Localidad))
                    writer.WriteAttributeString("localidad", comprobante.Emisor.DomicilioFiscal.Localidad);

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Referencia))
                    writer.WriteAttributeString("referencia", comprobante.Emisor.DomicilioFiscal.Referencia);

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Municipio))
                    writer.WriteAttributeString("municipio", comprobante.Emisor.DomicilioFiscal.Municipio);

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Estado))
                    writer.WriteAttributeString("estado", comprobante.Emisor.DomicilioFiscal.Estado);

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Pais))
                    writer.WriteAttributeString("pais", comprobante.Emisor.DomicilioFiscal.Pais);

                if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.CodigoPostal))
                    writer.WriteAttributeString("codigoPostal", comprobante.Emisor.DomicilioFiscal.CodigoPostal);

                writer.WriteEndElement();
            }
            else {
                // Como es requerido en 1.0, 2.0 y en 3.0 si no está poner el esquema con valores vacios:
                switch (comprobante.Version) {
                    case "1.0":
                        writer.WriteStartElement("DomicilioFiscal");
                        break;
                    case "2.0":
                        writer.WriteStartElement("DomicilioFiscal", "http://www.sat.gob.mx/cfd/2");
                        break;
                    case "3.0":
                        writer.WriteStartElement("cfdi", "DomicilioFiscal", "http://www.sat.gob.mx/cfd/3");
                        break;
                }

                switch (comprobante.Version) {
                    case "1.0":
                    case "2.0":
                    case "3.0":
                        //if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Calle))
                        writer.WriteAttributeString("calle", comprobante.Emisor.DomicilioFiscal.Calle);

                        if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.NoExterior))
                            writer.WriteAttributeString("noExterior", comprobante.Emisor.DomicilioFiscal.NoExterior);

                        if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.NoInterior))
                            writer.WriteAttributeString("noInterior", comprobante.Emisor.DomicilioFiscal.NoInterior);

                        if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Colonia))
                            writer.WriteAttributeString("colonia", comprobante.Emisor.DomicilioFiscal.Colonia);

                        if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Localidad))
                            writer.WriteAttributeString("localidad", comprobante.Emisor.DomicilioFiscal.Localidad);

                        if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Referencia))
                            writer.WriteAttributeString("referencia", comprobante.Emisor.DomicilioFiscal.Referencia);

                        //if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Municipio))
                        writer.WriteAttributeString("municipio", comprobante.Emisor.DomicilioFiscal.Municipio);

                        //if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Estado))
                        writer.WriteAttributeString("estado", comprobante.Emisor.DomicilioFiscal.Estado);

                        //if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.Pais))
                        writer.WriteAttributeString("pais", comprobante.Emisor.DomicilioFiscal.Pais);

                        //if (!string.IsNullOrEmpty(comprobante.Emisor.DomicilioFiscal.CodigoPostal))
                        writer.WriteAttributeString("codigoPostal", comprobante.Emisor.DomicilioFiscal.CodigoPostal);
                        writer.WriteEndElement();
                        break;
                }

            }

            //// Expedido En
            //if ( !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Calle)
            //    || !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.NoExterior)
            //    || !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.NoInterior)
            //    || !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Colonia)
            //    || !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Localidad)
            //    || !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Referencia)
            //    || !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Municipio)
            //    || !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Estado)
            //    || !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Pais) // SI PAIS ES EL UNICO NO ES NECESARIO TODO EL NODO ?
            //    || !string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.CodigoPostal)
            //    ) {
            //    switch (comprobante.Version) {
            //        case "1.0":
            //            writer.WriteStartElement("ExpedidoEn");
            //            break;
            //        case "2.0":
            //        case "2.2":
            //            writer.WriteStartElement("ExpedidoEn", "http://www.sat.gob.mx/cfd/2");
            //            break;
            //        case "3.0":
            //        case "3.2":
            //        default:
            //            writer.WriteStartElement("cfdi", "ExpedidoEn", "http://www.sat.gob.mx/cfd/3");
            //            break;
            //    }

            //    if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Calle))
            //        writer.WriteAttributeString("calle", comprobante.Emisor.ExpedidoEn.Calle);

            //    if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.NoExterior))
            //        writer.WriteAttributeString("noExterior", comprobante.Emisor.ExpedidoEn.NoExterior);

            //    if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.NoInterior))
            //        writer.WriteAttributeString("noInterior", comprobante.Emisor.ExpedidoEn.NoInterior);

            //    if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Colonia))
            //        writer.WriteAttributeString("colonia", comprobante.Emisor.ExpedidoEn.Colonia);

            //    if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Localidad))
            //        writer.WriteAttributeString("localidad", comprobante.Emisor.ExpedidoEn.Localidad);

            //    if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Referencia))
            //        writer.WriteAttributeString("referencia", comprobante.Emisor.ExpedidoEn.Referencia);

            //    if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Municipio))
            //    writer.WriteAttributeString("municipio", comprobante.Emisor.ExpedidoEn.Municipio);

            //    if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Estado))
            //        writer.WriteAttributeString("estado", comprobante.Emisor.ExpedidoEn.Estado);

            //    // UNICO REQUERIDO
            //    //if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.Pais))
            //        writer.WriteAttributeString("pais", comprobante.Emisor.ExpedidoEn.Pais);

            //    if (!string.IsNullOrEmpty(comprobante.Emisor.ExpedidoEn.CodigoPostal))
            //        writer.WriteAttributeString("codigoPostal", comprobante.Emisor.ExpedidoEn.CodigoPostal);

            //    writer.WriteEndElement();
            //}



            // Regimen
            if (comprobante.Emisor.RegimenFiscal != null && comprobante.Emisor.RegimenFiscal.Count > 0) {

                switch (comprobante.Version) {
                    case "1.0":
                        //writer.WriteStartElement("RegimenFiscal");
                        break;
                    case "2.0":
                    case "2.2":
                        writer.WriteStartElement("RegimenFiscal", "http://www.sat.gob.mx/cfd/2");
                        break;
                    case "3.0":
                    case "3.2":
                    default:
                        writer.WriteStartElement("cfdi", "RegimenFiscal", "http://www.sat.gob.mx/cfd/3");
                        break;
                }

                if (!string.IsNullOrEmpty(comprobante.Emisor.RegimenFiscal[0].Regimen))
                    writer.WriteAttributeString("Regimen", comprobante.Emisor.RegimenFiscal[0].Regimen);

                //End RegimenFiscal
                writer.WriteEndElement();
            }


            // EndEmisor
            writer.WriteEndElement();
            // }







            // Nodo Receptor siempre obligatorio
            //if (comprobante.Receptor != null && !string.IsNullOrEmpty(comprobante.Receptor.RFC)) {
            switch (comprobante.Version) {
                case "1.0":
                    writer.WriteStartElement("Receptor");
                    if (!string.IsNullOrEmpty(comprobante.Receptor.RFC)) // Al revés de los demás, en 1.0 solo el nombre es requerido
                        writer.WriteAttributeString("rfc", comprobante.Receptor.RFC);
                    // if (!string.IsNullOrEmpty(comprobante.Receptor.Nombre))
                    writer.WriteAttributeString("nombre", comprobante.Receptor.Nombre);
                    // Solo en este el domicilio es requerido ver eso
                    break;
                case "2.0":
                case "2.2":
                    writer.WriteStartElement("Receptor", "http://www.sat.gob.mx/cfd/2");
                    //if (!string.IsNullOrEmpty(comprobante.Receptor.RFC))
                    writer.WriteAttributeString("rfc", comprobante.Receptor.RFC);
                    if (!string.IsNullOrEmpty(comprobante.Receptor.Nombre))
                        writer.WriteAttributeString("nombre", comprobante.Receptor.Nombre);
                    break;
                case "3.0":
                case "3.2":
                default:
                    writer.WriteStartElement("cfdi", "Receptor", "http://www.sat.gob.mx/cfd/3");
                    //if (!string.IsNullOrEmpty(comprobante.Receptor.RFC))
                    writer.WriteAttributeString("rfc", comprobante.Receptor.RFC);
                    if (!string.IsNullOrEmpty(comprobante.Receptor.Nombre))
                        writer.WriteAttributeString("nombre", comprobante.Receptor.Nombre);
                    break;
            }

            // Domicilio Receptor opcional
            //if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Pais)) {
            if (comprobante.Receptor.Domicilio != null && (
                //comprobante.Receptor.DomicilioSpecified  ||
                !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Calle)
                    || !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.NoExterior)
                    || !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.NoInterior)
                    || !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Colonia)
                    || !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Localidad)
                    || !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Referencia)
                    || !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Municipio)
                    || !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Estado)
                    || !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Pais) // SI PAIS ES EL UNICO NO ES NECESARIO TODO EL NODO ?
                    || !string.IsNullOrEmpty(comprobante.Receptor.Domicilio.CodigoPostal)
                )) {
                switch (comprobante.Version) {
                    case "1.0":
                        writer.WriteStartElement("Domicilio");
                        break;
                    case "2.0":
                    case "2.2":
                        writer.WriteStartElement("Domicilio", "http://www.sat.gob.mx/cfd/2");
                        break;
                    case "3.0":
                    case "3.2":
                    default:
                        writer.WriteStartElement("cfdi", "Domicilio", "http://www.sat.gob.mx/cfd/3");
                        break;
                }

                if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Calle))
                    writer.WriteAttributeString("calle", comprobante.Receptor.Domicilio.Calle);

                if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.NoExterior))
                    writer.WriteAttributeString("noExterior", comprobante.Receptor.Domicilio.NoExterior);

                if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.NoInterior))
                    writer.WriteAttributeString("noInterior", comprobante.Receptor.Domicilio.NoInterior);

                if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Colonia))
                    writer.WriteAttributeString("colonia", comprobante.Receptor.Domicilio.Colonia);

                if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Localidad))
                    writer.WriteAttributeString("localidad", comprobante.Receptor.Domicilio.Localidad);

                if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Referencia))
                    writer.WriteAttributeString("referencia", comprobante.Receptor.Domicilio.Referencia);

                if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Municipio))
                    writer.WriteAttributeString("municipio", comprobante.Receptor.Domicilio.Municipio);

                if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Estado))
                    writer.WriteAttributeString("estado", comprobante.Receptor.Domicilio.Estado);

                //if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.Pais))
                writer.WriteAttributeString("pais", comprobante.Receptor.Domicilio.Pais);

                if (!string.IsNullOrEmpty(comprobante.Receptor.Domicilio.CodigoPostal))
                    writer.WriteAttributeString("codigoPostal", comprobante.Receptor.Domicilio.CodigoPostal);

                writer.WriteEndElement();

            }

            //END Receptor
            writer.WriteEndElement();
            //}



            // Conceptos

            switch (comprobante.Version) {
                case "1.0":
                    writer.WriteStartElement("Conceptos");
                    break;
                case "2.0":
                case "2.2":
                    writer.WriteStartElement("Conceptos", "http://www.sat.gob.mx/cfd/2");
                    break;
                case "3.0":
                case "3.2":
                default:
                    writer.WriteStartElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3");
                    break;
            }

            if (comprobante.Conceptos != null && comprobante.Conceptos.Count > 0) {
                //foreach (ComprobanteConcepto concepto in comprobante.Conceptos) {
                foreach (Concepto concepto in comprobante.Conceptos.OrderBy(p => p.Ordinal)) {

                    switch (comprobante.Version) {
                        case "1.0":
                            writer.WriteStartElement("Concepto");
                            break;
                        case "2.0":
                        case "2.2":
                            writer.WriteStartElement("Concepto", "http://www.sat.gob.mx/cfd/2");
                            break;
                        case "3.0":
                        case "3.2":
                        default:
                            writer.WriteStartElement("cfdi", "Concepto", "http://www.sat.gob.mx/cfd/3");
                            break;
                    }

                    //if (!string.IsNullOrEmpty(concepto.Cantidad))
                    writer.WriteAttributeString("cantidad", concepto.Cantidad.ToString(comprobante.DecimalFormat));

                    if (!string.IsNullOrEmpty(concepto.Unidad))
                        writer.WriteAttributeString("unidad", concepto.Unidad);

                    if (!string.IsNullOrEmpty(concepto.NoIdentificacion))
                        writer.WriteAttributeString("noIdentificacion", concepto.NoIdentificacion);

                    // checar
                    // if (!string.IsNullOrEmpty(concepto.Descripcion))
                    writer.WriteAttributeString("descripcion", concepto.Descripcion);

                    //if (!string.IsNullOrEmpty(concepto.ValorUnitario))
                    writer.WriteAttributeString("valorUnitario", concepto.ValorUnitario.ToString(comprobante.DecimalFormat));

                    //if (!string.IsNullOrEmpty(concepto.Importe))
                    writer.WriteAttributeString("importe", concepto.Importe.ToString(comprobante.DecimalFormat));

                    // EndConcepto
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
            // EndCponcetos


            // END conceptos





            // Impuestos


            //if (comprobante.Impuestos != null && comprobante.Conceptos.Count > 0) {

            switch (comprobante.Version) {
                case "1.0":
                    writer.WriteStartElement("Impuestos");
                    break;
                case "2.0":
                case "2.2":
                    writer.WriteStartElement("Impuestos", "http://www.sat.gob.mx/cfd/2");
                    break;
                case "3.0":
                case "3.2":
                default:
                    writer.WriteStartElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3");
                    break;
            }

            //foreach (ComprobanteConcepto concepto in comprobante.Conceptos) {

            //    switch (comprobante.Version) {
            //        case "1.0":
            //            writer.WriteStartElement("Concepto ");
            //            break;
            //        case "2.0":
            //        case "2.2":
            //            writer.WriteStartElement("Concepto ", "http://www.sat.gob.mx/cfd/2");
            //            break;
            //        case "3.0":
            //        case "3.2":
            //        default:
            //            writer.WriteStartElement("cfdi", "Concepto ", "http://www.sat.gob.mx/cfd/3");
            //            break;
            //    }

            //    //if (!string.IsNullOrEmpty(concepto.Cantidad))
            //    writer.WriteAttributeString("cantidad", concepto.Cantidad.ToString("0.000000"));
            //}


            if (comprobante.Impuestos != null) {
                //if (!string.IsNullOrEmpty(comprobante.Impuestos.TotalImpuestosRetenidosSpecified))
                //if (comprobante.Impuestos.TotalImpuestosRetenidosSpecified && comprobante.Impuestos.Retenciones.Count > 0)
                if (comprobante.Impuestos.TotalImpuestosRetenidos.HasValue && comprobante.Impuestos.Retenciones.Count > 0)
                    writer.WriteAttributeString("totalImpuestosRetenidos", comprobante.Impuestos.TotalImpuestosRetenidos.Value.ToString(comprobante.DecimalFormat));

                //if (comprobante.Impuestos.TotalImpuestosTrasladadosSpecified && comprobante.Impuestos.Traslados.Count > 0)
                if (comprobante.Impuestos.TotalImpuestosTrasladados.HasValue && comprobante.Impuestos.Traslados.Count > 0)
                    writer.WriteAttributeString("totalImpuestosTrasladados", comprobante.Impuestos.TotalImpuestosTrasladados.Value.ToString(comprobante.DecimalFormat));


                //if (comprobante.Impuestos.Retenciones.Count > 0 || (comprobante.Impuestos.Retenciones.Count > 0 && comprobante.Impuestos.TotalImpuestosRetenidosSpecified)) {
                if (comprobante.Impuestos.Retenciones!=null && ( comprobante.Impuestos.Retenciones.Count > 0 || (comprobante.Impuestos.Retenciones.Count > 0 && comprobante.Impuestos.TotalImpuestosRetenidos.HasValue))) {

                    // retenciones
                    switch (comprobante.Version) {
                        case "1.0":
                            writer.WriteStartElement("Retenciones");
                            break;
                        case "2.0":
                        case "2.2":
                            writer.WriteStartElement("Retenciones", "http://www.sat.gob.mx/cfd/2");
                            break;
                        case "3.0":
                        case "3.2":
                        default:
                            writer.WriteStartElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3");
                            break;
                    }
                    //foreach (ComprobanteImpuestosRetencion retencion in comprobante.Impuestos.Retenciones) {
                    foreach (Retencion retencion in comprobante.Impuestos.Retenciones.OrderBy(r => r.Ordinal)) {

                        switch (comprobante.Version) {
                            case "1.0":
                                writer.WriteStartElement("Retencion");
                                break;
                            case "2.0":
                            case "2.2":
                                writer.WriteStartElement("Retencion", "http://www.sat.gob.mx/cfd/2");
                                break;
                            case "3.0":
                            case "3.2":
                            default:
                                writer.WriteStartElement("cfdi", "Retencion", "http://www.sat.gob.mx/cfd/3");
                                break;
                        }

                        if (!string.IsNullOrEmpty(retencion.Impuesto))
                            writer.WriteAttributeString("impuesto", retencion.Impuesto);

                        writer.WriteAttributeString("importe", retencion.Importe.ToString(comprobante.DecimalFormat));

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }




                // traslados                  
                //if (comprobante.Impuestos.Traslados.Count > 0 || (comprobante.Impuestos.Traslados.Count > 0 && comprobante.Impuestos.TotalImpuestosTrasladadosSpecified)) {
                if (comprobante.Impuestos.Traslados.Count > 0 || (comprobante.Impuestos.Traslados.Count > 0 && comprobante.Impuestos.TotalImpuestosTrasladados.HasValue)) {
                    //if (comprobante.Impuestos.TotalImpuestosTrasladadosSpecified || comprobante.Impuestos.Traslados.Count > 0) {
                    switch (comprobante.Version) {
                        case "1.0":
                            writer.WriteStartElement("Traslados");
                            break;
                        case "2.0":
                        case "2.2":
                            writer.WriteStartElement("Traslados", "http://www.sat.gob.mx/cfd/2");
                            break;
                        case "3.0":
                        case "3.2":
                        default:
                            writer.WriteStartElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3");
                            break;
                    }
                    //foreach (ComprobanteImpuestosTraslado traslado in comprobante.Impuestos.Traslados) {
                    foreach (Traslado traslado in comprobante.Impuestos.Traslados.OrderBy(t => t.Ordinal)) {

                        switch (comprobante.Version) {
                            case "1.0":
                                writer.WriteStartElement("Traslado");
                                break;
                            case "2.0":
                            case "2.2":
                                writer.WriteStartElement("Traslado", "http://www.sat.gob.mx/cfd/2");
                                break;
                            case "3.0":
                            case "3.2":
                            default:
                                writer.WriteStartElement("cfdi", "Traslado", "http://www.sat.gob.mx/cfd/3");
                                break;
                        }


                        switch (comprobante.Version) {
                            case "1.0":
                                //if (!string.IsNullOrEmpty(traslado.Impuesto))
                                writer.WriteAttributeString("impuesto", traslado.Impuesto);
                                // no lleva tasa la versión 1.0
                                writer.WriteAttributeString("importe", traslado.Importe.ToString(comprobante.DecimalFormat));
                                break;
                            case "2.0":
                            case "2.2":
                            case "3.0":
                            case "3.2":
                            default:
                                if (!string.IsNullOrEmpty(traslado.Impuesto))
                                    writer.WriteAttributeString("impuesto", traslado.Impuesto);

                                writer.WriteAttributeString("tasa", traslado.Tasa.ToString(comprobante.DecimalFormat));
                                writer.WriteAttributeString("importe", traslado.Importe.ToString(comprobante.DecimalFormat));
                                break;
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

            }// EndImpuestos
            writer.WriteEndElement();


            if (comprobante.Complementos != null && comprobante.Complementos.Count > 0) {

                 switch (comprobante.Version) {
                    case "1.0":
                        writer.WriteStartElement("Complemento");
                        break;
                    case "2.0":
                    case "2.2":
                        writer.WriteStartElement("Complemento", "http://www.sat.gob.mx/cfd/2");
                        break;
                    case "3.0":
                    case "3.2":
                    default:
                        writer.WriteStartElement("cfdi", "Complemento", "http://www.sat.gob.mx/cfd/3");
                        break;
                }

                 foreach (Complemento complemento in comprobante.Complementos.OrderBy(c => c.Ordinal)) {

                     if (complemento is TimbreFiscalDigital) {
                         TimbreFiscalDigital timbre = (TimbreFiscalDigital)complemento;

                         writer.WriteStartElement("tfd", "TimbreFiscalDigital", "http://www.sat.gob.mx/TimbreFiscalDigital");                           
                         writer.WriteAttributeString("xmlns", "tfd", null, "http://www.sat.gob.mx/TimbreFiscalDigital");
                         //writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                         writer.WriteAttributeString("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/TimbreFiscalDigital/TimbreFiscalDigital.xsd");
                    
                         writer.WriteAttributeString("version", timbre.Version);
                         writer.WriteAttributeString("UUID", timbre.UUID);
                         writer.WriteAttributeString("FechaTimbrado", timbre.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss"));
                         writer.WriteAttributeString("selloCFD", timbre.SelloCFD);
                         writer.WriteAttributeString("noCertificadoSAT", timbre.NoCertificadoSAT);
                         writer.WriteAttributeString("selloSAT", timbre.SelloSAT);

                         writer.WriteEndElement();
                     }

                     //if (comprobante.Complemento.Nomina != null && comprobante.Complemento.NominaSpecified) {
                     //    writer.WriteStartElement("nomina", "Nomina", "http://www.sat.gob.mx/cfd/3");
                     //    writer.WriteAttributeString("version", comprobante.Complemento.Nomina.Version);
                     //    writer.WriteEndElement();
                     //    // end Nomina
                     //}                     

                 }


                writer.WriteEndElement();
                // end Complemento
            }





            writer.WriteEndElement();
            writer.Flush();
            // writer.Close();
        }

        private void WriteXmlWithSerializer() {

            //// ftp://ftp2.sat.gob.mx/asistencia_servicio_ftp/publicaciones/solcedi/ejemplo1%20cfdv3.xml

            ////http://stackoverflow.com/questions/258960/how-to-serialize-an-object-to-xml-without-getting-xmlns
            //System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
            ////ns.Add("", "");
            //ns.Add("cdfi", "http://www.sat.gob.mx/cfd/3");
            //ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            //System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Comprobante));
            //serializer.Serialize(writer, comprobante, ns);
            //writer.Formatting = System.Xml.Formatting.Indented;
            //writer.Flush();
            //// writer.Close();
        }

        public void Close() {
            writer.Close();
        }

        public static string NormalizeWhiteSpace(string S) {
            if (string.IsNullOrEmpty(S))
                return S;

            string s = S.Trim();
            bool iswhite = false;
            int iwhite;
            int sLength = s.Length;
            StringBuilder sb = new StringBuilder(sLength);
            foreach (char c in s.ToCharArray()) {
                if (Char.IsWhiteSpace(c)) {
                    if (iswhite) {
                        //Continuing whitespace ignore it.
                        continue;
                    }
                    else {
                        //New WhiteSpace

                        //Replace whitespace with a single space.
                        sb.Append(" ");
                        //Set iswhite to True and any following whitespace will be ignored
                        iswhite = true;
                    }
                }
                else {
                    sb.Append(c.ToString());
                    //reset iswhitespace to false
                    iswhite = false;
                }
            }
            return sb.ToString();
        }

    }
}