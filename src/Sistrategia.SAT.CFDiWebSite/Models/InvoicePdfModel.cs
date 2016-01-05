using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Sistrategia.SAT.CFDiWebSite.CFDI;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sistrategia.SAT.CFDiWebSite.Models
{
    public class InvoicePdfModel
    {
        public MemoryStream CreatePDF(Comprobante comprobante) {
            try {
                MemoryStream outputStream = new MemoryStream();
                Document document = new Document(PageSize.LETTER, 53.858267717f, 51.023622047f, 45.354330709f, 45.354330709f);
                PdfWriter writer = PdfWriter.GetInstance(document, outputStream);
                document.Open();

                //Fonts

                string fontpath = HttpContext.Current.Server.MapPath(@"~/Content/fonts/Verdana/");
                BaseFont Verdana = BaseFont.CreateFont(fontpath + "verdana.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont VerdanaBold = BaseFont.CreateFont(fontpath + "verdanab.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font VerdanaBold7Color = new Font(VerdanaBold, 7f, Font.NORMAL, new Color(62, 84, 84));
                Font VerdanaBold9Color = new Font(VerdanaBold, 8f, Font.NORMAL, new Color(62, 84, 84));
                Font Verdana9 = new Font(Verdana, 7f, Font.NORMAL, new Color(0, 0, 0));
                Font VerdanaBold10Color = new Font(VerdanaBold, 8f, Font.NORMAL, new Color(62, 84, 84));
                Font Verdana5 = new Font(Verdana, 5f, Font.NORMAL, new Color(0, 0, 0));

                PdfPTable MainTable = new PdfPTable(1);
                MainTable.TotalWidth = 507.401574803f;
                MainTable.LockedWidth = true;
                MainTable.DefaultCell.Border = 0;

                #region Header

                PdfPTable HeaderTable = new PdfPTable(3);
                HeaderTable.TotalWidth = 507.401574803f;
                HeaderTable.LockedWidth = true;
                float[] widhtsHeader = new float[] { 116.220472441f, 198.42519685f, 192.755905512f };
                HeaderTable.SetWidths(widhtsHeader);
                HeaderTable.DefaultCell.Border = 0;

                //Logo de la empresa en Base64
                Byte[] bytes = this.LoadLogoBase64(comprobante.Emisor.LogoUrl);
                Image logo = iTextSharp.text.Image.GetInstance(bytes);
                logo.ScalePercent(30f);
                PdfPCell HeaderLogo = new PdfPCell(logo);
                HeaderLogo.FixedHeight = 93.543307087f;
                HeaderLogo.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                HeaderLogo.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                HeaderLogo.Border = 0;
                HeaderTable.AddCell(HeaderLogo);

                //Tabla Anidada con Datos de emisor
                PdfPTable DatosEmisor = new PdfPTable(1);
                DatosEmisor.DefaultCell.Border = 0;

                PdfPCell CorporateName = new PdfPCell(new Phrase(comprobante.Emisor.Nombre, VerdanaBold9Color));
                //CorporateName.PaddingTop = 20;
                CorporateName.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                CorporateName.Border = Rectangle.NO_BORDER;
                DatosEmisor.AddCell(CorporateName);

                PdfContentByte LineaDatosEmisor = writer.DirectContent;
                LineaDatosEmisor.MoveTo(175.748031496f, document.Top - 17.007874016f);
                LineaDatosEmisor.LineTo(360, document.Top - 17.007874016f);
                LineaDatosEmisor.SetRGBColorStroke(62, 84, 84);
                LineaDatosEmisor.Stroke();

                PdfPTable TableCompanyData = new PdfPTable(1);

                PdfPCell CompanyRFC = new PdfPCell(new Phrase("R.F.C. " + comprobante.Emisor.RFC, Verdana9));
                CompanyRFC.Border = 0;
                TableCompanyData.AddCell(CompanyRFC);

                PdfPCell CompanyCalle = new PdfPCell(new Phrase(comprobante.Emisor.DomicilioFiscal.Calle + " # " + comprobante.Emisor.DomicilioFiscal.NoExterior, Verdana9));
                CompanyCalle.Border = 0;
                TableCompanyData.AddCell(CompanyCalle);

                PdfPCell CompanyColonia = new PdfPCell(new Phrase(comprobante.Emisor.DomicilioFiscal.Colonia + ", C.P. " + comprobante.Emisor.DomicilioFiscal.CodigoPostal, Verdana9));
                CompanyColonia.Border = 0;
                TableCompanyData.AddCell(CompanyColonia);

                PdfPCell CompanyCiudad = new PdfPCell(new Phrase(comprobante.Emisor.DomicilioFiscal.Municipio + ", " + comprobante.Emisor.DomicilioFiscal.Estado + ". " + comprobante.Emisor.DomicilioFiscal.Pais , Verdana9));
                CompanyCiudad.Border = 0;
                TableCompanyData.AddCell(CompanyCiudad);

                PdfPCell CompanyTelefono = new PdfPCell(new Phrase("TEL. " + comprobante.Emisor.Telefono, Verdana9));
                CompanyTelefono.Border = 0;
                TableCompanyData.AddCell(CompanyTelefono);

                PdfPCell CompanyCorreo = new PdfPCell(new Phrase(comprobante.Emisor.Correo, Verdana9));
                CompanyCorreo.Border = 0;
                TableCompanyData.AddCell(CompanyCorreo);

                PdfPCell CompanyRegimen = null;
                if (comprobante.Emisor.RegimenFiscal.Count > 0)
                    CompanyRegimen = new PdfPCell(new Phrase("Régimen Fiscal: " + comprobante.Emisor.RegimenFiscal[0].Regimen, Verdana9));
                else
                    CompanyRegimen = new PdfPCell(new Phrase("Régimen Fiscal: ", Verdana9));

                CompanyRegimen.Border = 0;
                TableCompanyData.AddCell(CompanyRegimen);


                PdfPCell CompanyData = new PdfPCell(TableCompanyData);
                CompanyData.HorizontalAlignment = Rectangle.ALIGN_LEFT;
                CompanyData.PaddingLeft = 20;
                CompanyData.Border = 0;
                DatosEmisor.AddCell(CompanyData);

                HeaderTable.AddCell(DatosEmisor);

                //Tabla Anidada con datos de control de la factura
                PdfPTable ControlDataInvoice = new PdfPTable(2);
                float[] WidthsControlDataInvoice = new float[] { 76.535433071f, 116.220472441f };
                ControlDataInvoice.SetWidths(WidthsControlDataInvoice);

                PdfPCell TextFactura = new PdfPCell(new Phrase("Factura: " + comprobante.Serie + comprobante.Folio, VerdanaBold10Color));
                TextFactura.Colspan = 2;
                TextFactura.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextFactura.Border = 0;
                ControlDataInvoice.AddCell(TextFactura);



                PdfContentByte LineaDatosFactura = writer.DirectContent;
                LineaDatosFactura.MoveTo(374.842519685f, document.Top - 15.007874016f);
                LineaDatosFactura.LineTo(554.763779528f, document.Top - 15.007874016f);
                LineaDatosFactura.SetRGBColorStroke(62, 84, 84);
                LineaDatosFactura.SetLineWidth(1);
                LineaDatosFactura.Stroke();

                PdfPCell TextEmision = new PdfPCell(new Phrase("Emisión", Verdana9));
                TextEmision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextEmision.Border = 0;
                ControlDataInvoice.AddCell(TextEmision);


                PdfPCell FechaEmision = new PdfPCell(new Phrase(comprobante.Fecha.ToString("dd/MM/yyyy HH:mm:ss"), Verdana9));
                FechaEmision.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                FechaEmision.Border = 0;
                ControlDataInvoice.AddCell(FechaEmision);

                PdfPCell TextFolio = new PdfPCell(new Phrase("Folio Fiscal", Verdana9));
                TextFolio.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextFolio.Border = 0;
                ControlDataInvoice.AddCell(TextFolio);




                string PhraseUUID = "";

                if (comprobante.Status.Equals("A")) {
                    foreach (var complemento in comprobante.Complementos) {
                        if (complemento is TimbreFiscalDigital) {
                            TimbreFiscalDigital timbre = (TimbreFiscalDigital)complemento;
                            if (!String.IsNullOrEmpty(timbre.UUID)) {
                                PhraseUUID = timbre.UUID.ToString();
                            }
                        }
                    }
                    //if (!String.IsNullOrEmpty(comprobante .Complemento.TimbreFiscalDigital.UUID)) {
                    //    PhraseUUID = comprobante.Complemento.TimbreFiscalDigital.UUID.ToString();
                    //}
                }

                PdfPCell Folio = new PdfPCell(new Phrase(PhraseUUID, Verdana9));
                Folio.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Folio.Border = 0;
                ControlDataInvoice.AddCell(Folio);

                PdfPCell TextNoOrden = new PdfPCell(new Phrase("No. de Orden", Verdana9));
                TextNoOrden.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextNoOrden.Border = 0;
                ControlDataInvoice.AddCell(TextNoOrden);

                PdfPCell NoOrden = new PdfPCell(new Phrase(comprobante.ExtendedIntValue1.ToString(), Verdana9));
                NoOrden.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                NoOrden.Border = 0;
                ControlDataInvoice.AddCell(NoOrden);

                PdfPCell TextCliente = new PdfPCell(new Phrase("Cliente", Verdana9));
                TextCliente.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextCliente.Border = 0;
                ControlDataInvoice.AddCell(TextCliente);

                PdfPCell Cliente = new PdfPCell(new Phrase(comprobante.ExtendedIntValue2.ToString(), Verdana9));
                Cliente.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Cliente.Border = 0;
                ControlDataInvoice.AddCell(Cliente);

                PdfPCell TextTransporte = new PdfPCell(new Phrase("Transporte", Verdana9));
                TextTransporte.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextTransporte.Border = 0;
                ControlDataInvoice.AddCell(TextTransporte);

                PdfPCell Transporte = new PdfPCell(new Phrase("CLIENTE", Verdana9));
                Transporte.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Transporte.Border = 0;
                ControlDataInvoice.AddCell(Transporte);

                PdfPCell TextMetodoPago = new PdfPCell(new Phrase("Método de Pago", Verdana9));
                TextMetodoPago.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextMetodoPago.Border = 0;
                ControlDataInvoice.AddCell(TextMetodoPago);

                PdfPCell MetodoPago = new PdfPCell(new Phrase(comprobante.MetodoDePago, Verdana9));
                MetodoPago.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                MetodoPago.Border = 0;
                ControlDataInvoice.AddCell(MetodoPago);

                if (comprobante.MetodoDePago != null && (  comprobante.MetodoDePago.ToString().Equals("Cheque") || comprobante.MetodoDePago.ToString().Equals("Tarjeta de Crédito") || comprobante.MetodoDePago.ToString().Equals("Tarjeta de Débito"))) {

                    PdfPCell TextNoCuenta = new PdfPCell(new Phrase("Número de Cuenta", Verdana9));
                    TextNoCuenta.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    TextNoCuenta.Border = 0;
                    ControlDataInvoice.AddCell(TextNoCuenta);

                    PdfPCell Cuenta = new PdfPCell(new Phrase(comprobante.NumCtaPago, Verdana9));
                    Cuenta.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    Cuenta.Border = 0;
                    ControlDataInvoice.AddCell(Cuenta);
                }

                PdfPCell ContentDatosFactura = new PdfPCell(ControlDataInvoice);
                ContentDatosFactura.BorderColor = new Color(62, 84, 84);
                ContentDatosFactura.BorderWidth = 2;

                PdfPCell EspacioTablas = new PdfPCell();
                EspacioTablas.Colspan = 2;
                EspacioTablas.Border = 0;
                ControlDataInvoice.AddCell(EspacioTablas);

                HeaderTable.AddCell(ContentDatosFactura);

                MainTable.AddCell(HeaderTable);

                PdfPCell EspacioTablas1 = new PdfPCell();
                EspacioTablas1.Colspan = 2;
                EspacioTablas1.Border = 0;
                EspacioTablas1.FixedHeight = 7.086614173f;

                MainTable.AddCell(EspacioTablas1);

                #endregion


                #region DatosFacturacion

                PdfPTable DatosReceptor = new PdfPTable(3);
                DatosReceptor.TotalWidth = 507.401574803f;
                DatosReceptor.LockedWidth = true;
                float[] WidthsDatosReceptor = new float[] { 250.866141732f, 5.669291339f, 250.866141732f };
                DatosReceptor.SetWidths(WidthsDatosReceptor);

                PdfPTable DatosFacturacion = new PdfPTable(2);
                DatosFacturacion.TotalWidth = 250.866141732f;
                float[] WidthsDatosFacturacion = new float[] { 70.866141732f, 180f };
                DatosFacturacion.SetWidths(WidthsDatosFacturacion);

                PdfPCell TextDatosFacturacion = new PdfPCell(new Phrase("Datos de Facturación", VerdanaBold9Color));
                TextDatosFacturacion.Colspan = 2;
                TextDatosFacturacion.Border = Rectangle.BOTTOM_BORDER;
                TextDatosFacturacion.BorderColor = new Color(62, 84, 84);
                TextDatosFacturacion.BorderWidth = 1.3f;
                TextDatosFacturacion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                DatosFacturacion.AddCell(TextDatosFacturacion);

                PdfPCell TextoRFC = new PdfPCell(new Phrase("RFC", Verdana9));
                TextoRFC.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoRFC.Border = 0;
                DatosFacturacion.AddCell(TextoRFC);

                PdfPCell RFC = new PdfPCell(new Phrase(comprobante.Receptor.RFC, Verdana9));
                RFC.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                RFC.Border = 0;
                DatosFacturacion.AddCell(RFC);

                PdfPCell TextoNombre = new PdfPCell(new Phrase("Nombre", Verdana9));
                TextoNombre.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoNombre.Border = 0;
                DatosFacturacion.AddCell(TextoNombre);

                PdfPCell Nombre = new PdfPCell(new Phrase(comprobante.Receptor.Nombre, Verdana9));
                Nombre.Border = 0;
                Nombre.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                DatosFacturacion.AddCell(Nombre);

                PdfPCell TextDireccionFiscal = new PdfPCell(new Phrase("Dirección Fiscal", VerdanaBold9Color));
                TextDireccionFiscal.Colspan = 2;
                TextDireccionFiscal.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                TextDireccionFiscal.BorderColor = new Color(62, 84, 84);
                TextDireccionFiscal.BorderWidth = 1.3f;
                TextDireccionFiscal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                DatosFacturacion.AddCell(TextDireccionFiscal);

                PdfPCell TextoCalle = new PdfPCell(new Phrase("Calle", Verdana9));
                TextoCalle.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoCalle.Border = 0;
                DatosFacturacion.AddCell(TextoCalle);

                PdfPCell Calle = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Calle + " # " + comprobante.Receptor.Domicilio.NoExterior, Verdana9));
                Calle.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Calle.Border = 0;
                DatosFacturacion.AddCell(Calle);

                PdfPCell TextoColonia = new PdfPCell(new Phrase("Colonia", Verdana9));
                TextoColonia.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoColonia.Border = 0;
                DatosFacturacion.AddCell(TextoColonia);

                PdfPCell Colonia = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Colonia, Verdana9));
                Colonia.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Colonia.Border = 0;
                DatosFacturacion.AddCell(Colonia);

                PdfPCell TextoMunicipio = new PdfPCell(new Phrase("Municipio", Verdana9));
                TextoMunicipio.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoMunicipio.Border = 0;
                DatosFacturacion.AddCell(TextoMunicipio);

                PdfPCell Municipio = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Municipio, Verdana9));
                Municipio.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Municipio.Border = 0;
                DatosFacturacion.AddCell(Municipio);

                PdfPCell TextoEstado = new PdfPCell(new Phrase("Estado", Verdana9));
                TextoEstado.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoEstado.Border = 0;
                DatosFacturacion.AddCell(TextoEstado);

                PdfPCell Estado = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Estado, Verdana9));
                Estado.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Estado.Border = 0;
                DatosFacturacion.AddCell(Estado);

                PdfPCell TextoPais = new PdfPCell(new Phrase("País", Verdana9));
                TextoPais.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoPais.Border = 0;
                DatosFacturacion.AddCell(TextoPais);

                PdfPCell Pais = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Pais, Verdana9));
                Pais.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Pais.Border = 0;
                DatosFacturacion.AddCell(Pais);

                PdfPCell TextoCP = new PdfPCell(new Phrase("Código Postal", Verdana9));
                TextoCP.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoCP.Border = 0;
                DatosFacturacion.AddCell(TextoCP);

                PdfPCell CP = new PdfPCell(new Phrase("C.P. " + comprobante.Receptor.Domicilio.CodigoPostal, Verdana9));
                CP.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                CP.Border = 0;
                DatosFacturacion.AddCell(CP);


                PdfPCell CellDatosFacturacion = new PdfPCell(DatosFacturacion);
                CellDatosFacturacion.BorderColor = new Color(62, 84, 84);
                CellDatosFacturacion.BorderWidth = 2;
                CellDatosFacturacion.PaddingRight = 4;
                CellDatosFacturacion.PaddingLeft = 4;
                CellDatosFacturacion.PaddingBottom = 4;

                DatosReceptor.AddCell(CellDatosFacturacion);

                PdfPCell Espacio = new PdfPCell();
                Espacio.Border = 0;

                DatosReceptor.AddCell(Espacio);

                PdfPTable DatosDestinatario = new PdfPTable(2);
                DatosDestinatario.TotalWidth = 250.866141732f;
                DatosDestinatario.SetWidths(WidthsDatosFacturacion);

                PdfPCell TextDestinatarioHeader = new PdfPCell(new Phrase("Destinatario", VerdanaBold9Color));
                TextDestinatarioHeader.Colspan = 2;
                TextDestinatarioHeader.Border = Rectangle.BOTTOM_BORDER;
                TextDestinatarioHeader.BorderColor = new Color(62, 84, 84);
                TextDestinatarioHeader.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextDestinatarioHeader.BorderWidth = 1.3f;
                DatosDestinatario.AddCell(TextDestinatarioHeader);

                PdfPCell TextoDestinatario = new PdfPCell(new Phrase("Destinatario", Verdana9));
                TextoDestinatario.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoDestinatario.Border = 0;
                DatosDestinatario.AddCell(TextoDestinatario);

                PdfPCell Destinatario = new PdfPCell(new Phrase(comprobante.Receptor.Nombre, Verdana9));
                Destinatario.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Destinatario.Border = 0;
                DatosDestinatario.AddCell(Destinatario);

                PdfPCell TextDireccion = new PdfPCell(new Phrase("Dirección", VerdanaBold9Color));
                TextDireccion.Colspan = 2;
                TextDireccion.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                TextDireccion.BorderColor = new Color(62, 84, 84);
                TextDireccion.BorderWidth = 1.3f;
                TextDireccion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                DatosDestinatario.AddCell(TextDireccion);

                DatosDestinatario.AddCell(TextoCalle);

                PdfPCell CalleDireccion = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Calle + " # " + comprobante.Receptor.Domicilio.NoExterior, Verdana9));
                CalleDireccion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                CalleDireccion.Border = 0;
                DatosDestinatario.AddCell(CalleDireccion);

                DatosDestinatario.AddCell(TextoColonia);

                PdfPCell ColoniaDireccion = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Colonia, Verdana9));
                ColoniaDireccion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                ColoniaDireccion.Border = 0;
                DatosDestinatario.AddCell(ColoniaDireccion);

                DatosDestinatario.AddCell(TextoMunicipio);

                PdfPCell MunicipioDireccion = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Municipio, Verdana9));
                MunicipioDireccion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                MunicipioDireccion.Border = 0;
                DatosDestinatario.AddCell(MunicipioDireccion);

                DatosDestinatario.AddCell(TextoEstado);

                PdfPCell EstadoDireccion = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Estado, Verdana9));
                EstadoDireccion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                EstadoDireccion.Border = 0;
                DatosDestinatario.AddCell(EstadoDireccion);

                DatosDestinatario.AddCell(TextoPais);

                PdfPCell PaisDireccion = new PdfPCell(new Phrase(comprobante.Receptor.Domicilio.Pais, Verdana9));
                PaisDireccion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                PaisDireccion.Border = 0;
                DatosDestinatario.AddCell(PaisDireccion);

                DatosDestinatario.AddCell(TextoCP);

                PdfPCell CPDireccion = new PdfPCell(new Phrase("C.P. " + comprobante.Receptor.Domicilio.CodigoPostal, Verdana9));
                CPDireccion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                CPDireccion.Border = 0;
                DatosDestinatario.AddCell(CPDireccion);


                PdfPCell CellDatosDestinatario = new PdfPCell(DatosDestinatario);
                CellDatosDestinatario.BorderColor = new Color(62, 84, 84);
                CellDatosDestinatario.BorderWidth = 2;
                CellDatosDestinatario.PaddingRight = 4;
                CellDatosDestinatario.PaddingLeft = 4;
                CellDatosDestinatario.PaddingBottom = 4;

                DatosReceptor.AddCell(CellDatosDestinatario);

                MainTable.AddCell(DatosReceptor);
                #endregion

                PdfPCell TableDivision = new PdfPCell();
                TableDivision.FixedHeight = 7.086614173f;
                TableDivision.Border = 0;
                MainTable.AddCell(TableDivision);



                #region Conceptos
                PdfPTable Conceptos = new PdfPTable(1);
                Conceptos.TotalWidth = 507.401574803f;
                Conceptos.LockedWidth = true;

                PdfPTable TableConceptos = new PdfPTable(9);
                float[] WidhtsTableConceptos = new float[] { 51.023622047f, 2.834645669f, 63.779527559f, 2.834645669f, 246.614173228f, 2.834645669f, 62.362204724f, 2.834645669f, 65.196850394f };
                TableConceptos.SetWidths(WidhtsTableConceptos);
                TableConceptos.SplitRows = true;//.SpacingAfter = 50f;

                PdfPCell TextoCantidad = new PdfPCell(new Phrase("Cantidad", VerdanaBold9Color));
                TextoCantidad.Border = Rectangle.BOTTOM_BORDER;
                TextoCantidad.BorderWidth = 1.3f;
                TextoCantidad.BorderColor = new Color(62, 84, 84);
                TextoCantidad.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoCantidad.PaddingBottom = 4;
                TableConceptos.AddCell(TextoCantidad);

                PdfPCell Espacio1 = new PdfPCell();
                Espacio1.Border = 0;
                TableConceptos.AddCell(Espacio1);

                PdfPCell TextoClave = new PdfPCell(new Phrase("Clave", VerdanaBold9Color));
                TextoClave.Border = Rectangle.BOTTOM_BORDER;
                TextoClave.BorderWidth = 1.3f;
                TextoClave.BorderColor = new Color(62, 84, 84);
                TextoClave.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoClave.PaddingBottom = 4;
                TableConceptos.AddCell(TextoClave);

                TableConceptos.AddCell(Espacio1);

                PdfPCell TextoDescripción = new PdfPCell(new Phrase("Descripción", VerdanaBold9Color));
                TextoDescripción.Border = Rectangle.BOTTOM_BORDER;
                TextoDescripción.BorderWidth = 1.3f;
                TextoDescripción.BorderColor = new Color(62, 84, 84);
                TextoDescripción.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoDescripción.PaddingBottom = 4;
                TableConceptos.AddCell(TextoDescripción);

                TableConceptos.AddCell(Espacio1);

                PdfPCell TextoPUnitario = new PdfPCell(new Phrase("P. Unitario", VerdanaBold9Color));
                TextoPUnitario.Border = Rectangle.BOTTOM_BORDER;
                TextoPUnitario.BorderWidth = 1.3f;
                TextoPUnitario.BorderColor = new Color(62, 84, 84);
                TextoPUnitario.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoPUnitario.PaddingBottom = 4;
                TableConceptos.AddCell(TextoPUnitario);

                TableConceptos.AddCell(Espacio1);

                PdfPCell TextoImporte = new PdfPCell(new Phrase("Importe", VerdanaBold9Color));
                TextoImporte.Border = Rectangle.BOTTOM_BORDER;
                TextoImporte.BorderWidth = 1.3f;
                TextoImporte.BorderColor = new Color(62, 84, 84);
                TextoImporte.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoImporte.PaddingBottom = 4;
                TableConceptos.AddCell(TextoImporte);


                foreach (Concepto concepto in comprobante.Conceptos) {
                    PdfPCell conceptoCantidad = new PdfPCell(new Phrase(concepto.Cantidad.ToString("0.00"), Verdana9));
                    conceptoCantidad.HorizontalAlignment = Element.ALIGN_CENTER;
                    conceptoCantidad.Border = 0;
                    TableConceptos.AddCell(conceptoCantidad);

                    TableConceptos.AddCell(Espacio1);

                    PdfPCell conceptoClave = new PdfPCell(new Phrase(concepto.NoIdentificacion, Verdana9));
                    conceptoClave.HorizontalAlignment = Element.ALIGN_CENTER;
                    conceptoClave.Border = 0;
                    TableConceptos.AddCell(conceptoClave);

                    TableConceptos.AddCell(Espacio1);

                    PdfPCell conceptoDescripcion = new PdfPCell(new Phrase(concepto.Descripcion, Verdana9));
                    conceptoDescripcion.HorizontalAlignment = Element.ALIGN_CENTER;
                    conceptoDescripcion.Border = 0;
                    TableConceptos.AddCell(conceptoDescripcion);

                    TableConceptos.AddCell(Espacio1);

                    PdfPCell conceptoPUnitario = new PdfPCell(new Phrase("$" + concepto.ValorUnitario.ToString("#,##0.00"), Verdana9));
                    conceptoPUnitario.HorizontalAlignment = Element.ALIGN_CENTER;
                    conceptoPUnitario.Border = 0;
                    TableConceptos.AddCell(conceptoPUnitario);


                    TableConceptos.AddCell(Espacio1);

                    PdfPCell conceptoImporte = new PdfPCell(new Phrase("$" + concepto.Importe.ToString("#,##0.00"), Verdana9));
                    conceptoImporte.HorizontalAlignment = Element.ALIGN_CENTER;
                    conceptoImporte.Border = 0;
                    TableConceptos.AddCell(conceptoImporte);
                }

                PdfPCell CellConceptos = new PdfPCell(TableConceptos);
                CellConceptos.BorderWidth = 2;
                CellConceptos.BorderColor = new Color(62, 84, 84);
                CellConceptos.PaddingRight = 4;
                CellConceptos.PaddingLeft = 4;
                CellConceptos.MinimumHeight = 184.251968504f;

                Conceptos.AddCell(CellConceptos);

                MainTable.AddCell(Conceptos);

                #endregion

                MainTable.AddCell(TableDivision);

                PdfPTable FooterContent = new PdfPTable(1);
                FooterContent.TotalWidth = 507.401574803f;
                FooterContent.LockedWidth = true;

                FooterContent.DefaultCell.Border = 0;

                PdfPTable Footer = new PdfPTable(2);
                Footer.DefaultCell.Border = 0;
                float[] WidthsFooter = new float[] { 86.456692913f, 420.94488189f };
                Footer.TotalWidth = 507.401574803f;
                Footer.LockedWidth = true;
                Footer.SetWidths(WidthsFooter);



                 //CIF empresa en Base64
                Byte[] bytes2 = LoadCIFBase64(comprobante.Emisor.CifUrl);

                Image CIFImage = iTextSharp.text.Image.GetInstance(bytes2);
                CIFImage.ScaleAbsoluteWidth(76.535433071f);
                CIFImage.ScaleAbsoluteHeight(155.905511811f);

                PdfPCell CIF = new PdfPCell(CIFImage);
                CIF.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                CIF.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                CIF.Border = Rectangle.RIGHT_BORDER;
                CIF.BorderColor = new Color(62, 84, 84);
                CIF.BorderWidth = 1.3f;

                Footer.AddCell(CIF);

                PdfPTable FooterRight = new PdfPTable(1);
                FooterRight.DefaultCell.Border = 0;

                PdfPTable TableFooterTop = new PdfPTable(2);
                TableFooterTop.DefaultCell.Border = 0;
                float[] WidhtsFooterTop = new float[] { 198.42519685f, 222.519685039f };
                TableFooterTop.SetWidths(WidhtsFooterTop);

                PdfPTable FooterTopLeft = new PdfPTable(1);
                FooterTopLeft.DefaultCell.Border = 0;

                Font CamposSAT = new Font();
                CamposSAT = FontFactory.GetFont("Arial", 8.0f, Font.NORMAL, new Color(0, 0, 0));

                PdfPTable TableCertificacion = new PdfPTable(1);


                string PhraseFechaCertificacion = "";
                if (comprobante.Status.Equals("A")) {
                    foreach (var complemento in comprobante.Complementos) {
                        if (complemento is TimbreFiscalDigital) {
                            TimbreFiscalDigital timbre = (TimbreFiscalDigital)complemento;
                            //if (!String.IsNullOrEmpty(timbre.FechaTimbrado)) {
                            PhraseUUID = timbre.FechaTimbrado.ToString("dd/MM/yyyy HH:mm:ss");
                            //}
                        }
                    }
                    //if (!String.IsNullOrEmpty(comprobante.Complemento.TimbreFiscalDigital.FechaTimbrado.ToString())) {
                    //    PhraseFechaCertificacion = comprobante.Complemento.TimbreFiscalDigital.FechaTimbrado.ToString("dd/MM/yyyy HH:mm:ss");
                    //}
                }



                PdfPCell Certificacion = new PdfPCell(new Phrase("Certificación: " + PhraseFechaCertificacion + "\n", VerdanaBold9Color));
                Certificacion.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Certificacion.VerticalAlignment = Rectangle.ALIGN_CENTER;
                Certificacion.Border = 0;
                TableCertificacion.AddCell(Certificacion);

                TableCertificacion.AddCell(Espacio1);

                PdfPCell CadenaSAT = new PdfPCell(new Phrase("Cadena SAT", VerdanaBold9Color));
                CadenaSAT.FixedHeight = 14.173228346f;
                CadenaSAT.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                CadenaSAT.VerticalAlignment = Rectangle.ALIGN_CENTER;
                CadenaSAT.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                CadenaSAT.BorderColor = new Color(62, 84, 84);
                CadenaSAT.BorderWidth = 1.3f;
                TableCertificacion.AddCell(CadenaSAT);

                string cadenaSAT = comprobante.GetCadenaOriginal(); // .CadenaOriginal;
                //if (comprobante.Version.Equals("3.2"))
                    cadenaSAT = comprobante.GetCadenaSAT(); // .CadenaSAT;

                PdfPCell Cadena = new PdfPCell(new Phrase(cadenaSAT, Verdana5));
                Cadena.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Cadena.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                Cadena.Border = 0;
                TableCertificacion.AddCell(Cadena);

                PdfPCell CellCertificacion = new PdfPCell(TableCertificacion);
                CellCertificacion.Padding = 3;
                CellCertificacion.Border = Rectangle.BOTTOM_BORDER;
                CellCertificacion.BorderColor = new Color(62, 84, 84);
                CellCertificacion.BorderWidth = 1.3f;

                FooterTopLeft.AddCell(CellCertificacion);

                PdfPCell CellFooterTopLeft = new PdfPCell(FooterTopLeft);
                CellFooterTopLeft.Border = 0;
                CellFooterTopLeft.MinimumHeight = 68.031496063f;
                TableFooterTop.AddCell(CellFooterTopLeft);

                PdfPTable FooterTopRight = new PdfPTable(2);
                FooterTopRight.DefaultCell.Border = 0;
                float[] WidthsFooterTopRight = new float[] { 53.858267717f, 167.244094488f };
                FooterTopRight.SetWidths(WidthsFooterTopRight);

                PdfPCell TextoSubtotal = new PdfPCell(new Phrase("Subtotal", Verdana9));
                TextoSubtotal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoSubtotal.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                TextoSubtotal.Border = 0;
                FooterTopRight.AddCell(TextoSubtotal);

                PdfPCell Subtotal = new PdfPCell(new Phrase("$" + comprobante.SubTotal.ToString("#,##0.00"), Verdana9));
                Subtotal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Subtotal.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                Subtotal.Border = 0;
                FooterTopRight.AddCell(Subtotal);

                PdfPCell TextoIVA = new PdfPCell(new Phrase("16 % IVA", Verdana9));
                TextoIVA.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoIVA.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                TextoIVA.Border = 0;
                FooterTopRight.AddCell(TextoIVA);

                PdfPCell IVA = new PdfPCell(new Phrase("$" + comprobante.Impuestos.TotalImpuestosTrasladados.Value.ToString("#,##0.00"), Verdana9));
                IVA.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                IVA.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                IVA.Border = 0;
                FooterTopRight.AddCell(IVA);

                PdfPCell TextoTotal = new PdfPCell(new Phrase("Total", Verdana9));
                TextoTotal.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                TextoTotal.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                TextoTotal.Border = 0;
                FooterTopRight.AddCell(TextoTotal);

                PdfPCell Total = new PdfPCell(new Phrase("$" + comprobante.Total.ToString("#,##0.00"), Verdana9));
                Total.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Total.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                Total.Border = 0;
                FooterTopRight.AddCell(Total);

                PdfPTable TableTotalLetra = new PdfPTable(2);

                CantidadEnLetraConverter letraConverter = new CantidadEnLetraConverter();
                letraConverter.Numero = comprobante.Total;
                //this.TotalLetra = letraConverter.letra();

                //PdfPCell CellTableLetra = new PdfPCell(new Phrase(SATManager.GetImporteConLetra(comprobante.Total) + "  M.N", VerdanaBold7Color));
                PdfPCell CellTableLetra = new PdfPCell(new Phrase(letraConverter.letra() + "  M.N", VerdanaBold7Color));
                CellTableLetra.Colspan = 2;
                CellTableLetra.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                CellTableLetra.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                CellTableLetra.Border = Rectangle.TOP_BORDER;
                CellTableLetra.BorderColor = new Color(62, 84, 84);
                CellTableLetra.BorderWidth = 1.3f;
                TableTotalLetra.AddCell(CellTableLetra);

                PdfPCell TotalLetra = new PdfPCell(TableTotalLetra);
                TotalLetra.Border = Rectangle.BOTTOM_BORDER;
                TotalLetra.BorderColor = new Color(62, 84, 84);
                TotalLetra.BorderWidth = 1.3f;
                TotalLetra.Colspan = 2;
                TotalLetra.PaddingTop = 3;
                TotalLetra.PaddingLeft = 3;

                FooterTopRight.AddCell(TotalLetra);

                PdfPCell CellFooterTopRight = new PdfPCell(FooterTopRight);
                CellFooterTopRight.MinimumHeight = 68.031496063f;
                CellFooterTopRight.Border = Rectangle.LEFT_BORDER;
                CellFooterTopRight.BorderColor = new Color(62, 84, 84);
                CellFooterTopRight.BorderWidth = 1.3f;
                TableFooterTop.AddCell(CellFooterTopRight);

                FooterRight.AddCell(TableFooterTop);

                PdfPTable TableFooterBottom = new PdfPTable(2);
                float[] WidhtsFooterBottom = new float[] { 92.125984252f, 328.818897638f };
                TableFooterBottom.SetWidths(WidhtsFooterBottom);

                PdfPTable FooterBottomLeft = new PdfPTable(1);




                ///QRCODE
                ///
                if (!String.IsNullOrEmpty(comprobante.GetQrCode())) {
                    Byte[] QRCODEBase64 = Convert.FromBase64String(comprobante.GetQrCode());
                    Image QRCODEImage = iTextSharp.text.Image.GetInstance(QRCODEBase64);
                    QRCODEImage.ScaleAbsoluteWidth(76.535433071f);
                    QRCODEImage.ScaleAbsoluteHeight(76.535433071f);

                    PdfPCell CellFooterBottomLeft = new PdfPCell(QRCODEImage, false);
                    CellFooterBottomLeft.MinimumHeight = 85.039370079f;
                    CellFooterBottomLeft.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                    CellFooterBottomLeft.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                    CellFooterBottomLeft.Border = Rectangle.RIGHT_BORDER;
                    CellFooterBottomLeft.BorderColor = new Color(62, 84, 84);
                    CellFooterBottomLeft.BorderWidth = 1.3f;
                    TableFooterBottom.AddCell(CellFooterBottomLeft);
                }
                else {
                    TableFooterBottom.AddCell("");
                }

                PdfPTable TableRightFooter = new PdfPTable(1);


                string PhraseCertificadoSAT = "";

                if (comprobante.Status.Equals("A"))
                {
                    foreach (var complemento in comprobante.Complementos) {
                        if (complemento is TimbreFiscalDigital) {
                            TimbreFiscalDigital timbre = (TimbreFiscalDigital)complemento;
                            if (!String.IsNullOrEmpty(timbre.NoCertificadoSAT)) {
                                PhraseCertificadoSAT = timbre.NoCertificadoSAT.ToString();
                            }
                        }
                    }

                    //if (!String.IsNullOrEmpty(comprobante.Complemento.TimbreFiscalDigital.NoCertificadoSAT))
                    //{
                    //    PhraseCertificadoSAT = comprobante.Complemento.TimbreFiscalDigital.NoCertificadoSAT.ToString();
                    //}
                }

                PdfPCell Certificado = new PdfPCell(new Phrase("Certificado SAT: " + PhraseCertificadoSAT, VerdanaBold9Color));
                Certificado.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Certificado.VerticalAlignment = Rectangle.ALIGN_CENTER;
                Certificado.Border = 0;
                Certificado.PaddingTop = 0;
                TableRightFooter.AddCell(Certificado);

                TableRightFooter.AddCell(Espacio1);

                PdfPCell Sellos = new PdfPCell(new Phrase("Sellos Digitales", VerdanaBold9Color));
                Sellos.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Sellos.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                Sellos.Padding = 3;
                Sellos.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
                Sellos.BorderColor = new Color(62, 84, 84);
                Sellos.BorderWidth = 1.3f;
                TableRightFooter.AddCell(Sellos);

                PdfPCell SelloCFD = new PdfPCell(new Phrase("Sello CFDI: " + comprobante.Sello, Verdana5));
                SelloCFD.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                SelloCFD.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                SelloCFD.Padding = 3;
                SelloCFD.Border = 0;
                TableRightFooter.AddCell(SelloCFD);


                string PhraseSelloSat = "";

                if (comprobante.Status.Equals("A")) {
                    foreach (var complemento in comprobante.Complementos) {
                        if (complemento is TimbreFiscalDigital) {
                            TimbreFiscalDigital timbre = (TimbreFiscalDigital)complemento;
                            if (!String.IsNullOrEmpty(timbre.SelloSAT)) {
                                PhraseSelloSat = timbre.SelloSAT.ToString();
                            }
                        }
                    }
                }

                PdfPCell SelloSAT = new PdfPCell(new Phrase("Sello SAT: " + PhraseSelloSat, Verdana5));
                SelloSAT.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                SelloSAT.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                SelloSAT.Padding = 3;
                SelloSAT.Border = Rectangle.BOTTOM_BORDER;
                SelloSAT.BorderColor = new Color(62, 84, 84);
                SelloSAT.BorderWidth = 1.3f;
                TableRightFooter.AddCell(SelloSAT);

                TableRightFooter.AddCell(Espacio1);

                PdfPCell Leyenda = new PdfPCell(new Phrase("Este documento es una representación impresa de un CFDI.", VerdanaBold9Color));
                Leyenda.HorizontalAlignment = Rectangle.ALIGN_CENTER;
                Leyenda.VerticalAlignment = Rectangle.ALIGN_MIDDLE;
                Leyenda.Padding = 0;
                Leyenda.Border = 0;
                TableRightFooter.AddCell(Leyenda);


                PdfPCell CellFooterBottomRight = new PdfPCell(TableRightFooter);
                CellFooterBottomRight.Padding = 4;
                CellFooterBottomRight.Border = 0;
                CellFooterTopRight.MinimumHeight = 68.031496063f;
                TableFooterBottom.AddCell(CellFooterBottomRight);

                FooterRight.AddCell(TableFooterBottom);

                Footer.AddCell(FooterRight);


                PdfPCell CellFooter = new PdfPCell(Footer);
                CellFooter.BorderColor = new Color(62, 84, 84);
                CellFooter.BorderWidth = 2;

                MainTable.AddCell(CellFooter);

                document.Add(MainTable);


                if (comprobante.Status.ToString().Equals("C")) {
                    string urlImage = HttpContext.Current.Server.MapPath("~/Content/Invoice/cancelado.png");
                    Image watermark2 = Image.GetInstance(urlImage);
                    watermark2.SetAbsolutePosition(120, 300);

                    document.Add(watermark2);
                }


                writer.CloseStream = false;
                document.Close();
                outputStream.Flush();
                outputStream.Position = 0;


                return outputStream;

                //throw new NotImplementedException();
            }
            catch (Exception ex) {

                ex.Message.ToString();
                return null;
            }
        }



        private byte[] LoadLogoBase64(string url) {

            //CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureDefaultStorageConnectionString"]);
            //CloudBlobClient client = account.CreateCloudBlobClient();
            //// CloudBlobContainer container = client.GetContainerReference(ConfigurationManager.AppSettings["AzureDefaultStorage"]);

            //var blob = client.GetBlobReferenceFromServer( new Uri(url));
            //blob.FetchAttributes();
            //long fileByteLength = blob.Properties.Length;
            //byte[] fileContent = new byte[fileByteLength];
            //for (int i = 0; i < fileByteLength; i++) {
            //    fileContent[i] = 0x20;
            //}
            //blob.DownloadToByteArray(fileContent, 0);
            //return fileContent;

            return new System.Net.WebClient().DownloadData(url);
        }
       
        private byte[] LoadCIFBase64(string url) {
            return new System.Net.WebClient().DownloadData(url);
        }
    }
}