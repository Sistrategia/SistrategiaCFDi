using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Services.Description;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

namespace Sistrategia.SAT.CFDiWebSite.CFDI.EDICOM.TestCFDI
{
    [DesignerCategory("code")]
    [WebServiceBinding(Name = "CFDiSoapBinding", Namespace = "http://cfdi.service.ediwinws.edicom.com")]
    internal class CFDiService : SoapHttpClientProtocol, ICFDIService
    {
        private bool useDefaultCredentialsSetExplicitly;

        public CFDiService() {
            //this.Url = global::TestCFDiClient.Properties.Settings.Default.TestCFDiClient_edicom_CFDiService;
            //this.Url = @"https://cfdiws.sedeb2b.com/EdiwinWS/services/CFDi"; // System.Configuration.AppSettingsSection
            this.Url = System.Configuration.ConfigurationManager.AppSettings["cfdiRemoteService"].ToString();
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public new string Url {
            get { return base.Url; }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                            && (this.useDefaultCredentialsSetExplicitly == false))
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials {
            get { return base.UseDefaultCredentials; }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        [SoapDocumentMethod("", RequestNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                ResponseNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                Use = SoapBindingUse.Literal,
                                ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlElement("getCfdiTestReturn", DataType = "base64Binary")]
        public byte[] getCfdiTest(string user, string password, [XmlElement(DataType = "base64Binary")] byte[] file) {
            object[] results = this.Invoke("getCfdiTest", new object[] {
                        user,
                        password,
                        file});
            return ((byte[])(results[0]));
        }
        
        [SoapDocumentMethod("", RequestNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                ResponseNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                Use = SoapBindingUse.Literal,
                                ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlElement("getTimbreCfdiTestReturn", DataType = "base64Binary")]
        public byte[] getTimbreCfdiTest(string user, string password, [XmlElement(DataType = "base64Binary")] byte[] file) {
            object[] results = this.Invoke("getTimbreCfdiTest", new object[] {
                        user,
                        password,
                        file});
            return ((byte[])(results[0]));
        }

        #region ICFDIService Implementation

        byte[] ICFDIService.GetCFDI(string user, string password, byte[] file) {
            return this.getCfdiTest(user, password, file);
        }

        byte[] ICFDIService.GetTimbreCFDI(string user, string password, byte[] file) {
            return this.getTimbreCfdiTest(user, password, file);
        }

        #endregion

        ICancelaResponse ICFDIService.CancelaCFDI(string user, string password, string rfc, string[] uuid, byte[] pfx, string pfxPassword) {
            // throw new NotImplementedException();

            var response = new CancelaResponse {
                ack = "CANCELADO_PRUEBA_NO_VALIDO",
                text = "~CANCELADO_PRUEBA_NO_VALIDO",
                uuids = uuid.ToList()
            };

             //CancelaResponse response = this.cancelaCFDi(user, password, rfc, uuid, pfx, pfxPassword);

            CancelaResponseBase responseBase = new CancelaResponseBase();
            responseBase.Ack = response.ack;
            responseBase.Text = response.text;

            System.Collections.Generic.List<string> uuidsList = new System.Collections.Generic.List<string>();

            foreach (string uuidItem in response.uuids) {
                uuidsList.Add(uuidItem);
            }
            responseBase.UUIDs = uuidsList.ToArray();

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(response.GetType());
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding();
            XmlWriter xmlWriter = XmlWriter.Create(ms, settings);
            x.Serialize(xmlWriter, response);
            string xmlContent = Encoding.UTF8.GetString(ms.GetBuffer());

            responseBase.XmlResponse = xmlContent;

            return responseBase;

        }

        #region Utilities

        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
        #endregion
    }
}