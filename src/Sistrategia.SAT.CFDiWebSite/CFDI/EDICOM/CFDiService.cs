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

namespace Sistrategia.SAT.CFDiWebSite.CFDI.EDICOM
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

        #region ICFDIService Implementation

        public byte[] GetCFDI(string user, string password, byte[] file) {
            return this.getCfdi(user, password, file);
        }

        public byte[] GetTimbreCFDI(string user, string password, byte[] file) {
            return this.getTimbreCfdi(user, password, file);
        }

        public ICancelaResponse CancelaCFDI(string user, string password, string rfc, string[] uuid, byte[] pfx, string pfxPassword) {
            //return this.cancelaCFDi(user, password, rfc, uuid, pfx, pfxPassword);

            CancelaResponse response = this.cancelaCFDi(user, password, rfc, uuid, pfx, pfxPassword);

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

        public byte[] GetCFDIFromUUID(string user, string password, string rfc, string[] uuid) {
            return this.getCfdiFromUUID(user, password, rfc, uuid);
        }

        #endregion

        #region Web Service Calls

        [SoapDocumentMethod("", RequestNamespace = "http://cfdi.service.ediwinws.edicom.com", 
                                ResponseNamespace = "http://cfdi.service.ediwinws.edicom.com", 
                                Use = SoapBindingUse.Literal, 
                                ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlElement("getCfdiReturn", DataType = "base64Binary")]
        public byte[] getCfdi(string user, string password, [XmlElement(DataType = "base64Binary")] byte[] file) {
            object[] results = this.Invoke("getCfdi", new object[] {
                        user,
                        password,
                        file});
            return ((byte[])(results[0]));
        }

        [SoapDocumentMethod("", RequestNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                ResponseNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                Use = SoapBindingUse.Literal,
                                ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlElement("getTimbreCfdiReturn", DataType = "base64Binary")]
        public byte[] getTimbreCfdi(string user, string password, [XmlElement(DataType = "base64Binary")] byte[] file) {
            object[] results = this.Invoke("getTimbreCfdi", new object[] {
                        user,
                        password,
                        file});
            return ((byte[])(results[0]));
        }

        [SoapDocumentMethod("", RequestNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                ResponseNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                Use = SoapBindingUse.Literal,
                                ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlElement("cancelaCFDiReturn")]
        public CancelaResponse cancelaCFDi(string user, string password, string rfc, [XmlElement("uuid")] string[] uuid, [XmlElement(DataType = "base64Binary")] byte[] pfx, string pfxPassword) {
            object[] results = this.Invoke("cancelaCFDi", new object[] {
                        user,
                        password,
                        rfc,
                        uuid,
                        pfx,
                        pfxPassword});
            return ((CancelaResponse)(results[0]));
        }

        [SoapDocumentMethod("", RequestNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                ResponseNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                Use = SoapBindingUse.Literal,
                                ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlElement("cancelaCFDiSignedReturn")]
        public CancelaResponse cancelaCFDiSigned(string user, string password, [XmlElement(DataType = "base64Binary")] byte[] sign) {
            object[] results = this.Invoke("cancelaCFDiSigned", new object[] {
                        user,
                        password,
                        sign});
            return ((CancelaResponse)(results[0]));
        }

        [SoapDocumentMethod("", RequestNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                ResponseNamespace = "http://cfdi.service.ediwinws.edicom.com",
                                Use = SoapBindingUse.Literal,
                                ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlElement("getCfdiFromUUIDReturn", DataType = "base64Binary")]
        public byte[] getCfdiFromUUID(string user, string password, string rfc, [XmlElement("uuid")] string[] uuid) {
            object[] results = this.Invoke("getCfdiFromUUID", new object[] {
                        user,
                        password,
                        rfc,
                        uuid});
            return ((byte[])(results[0]));
        }

        #endregion

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

    [Serializable]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://cfdi.service.ediwinws.edicom.com")]
    public partial class CancelaResponse
    {
        private string ackField;
        private string textField;
        //private string[] uuidsField;
        private List<string> uuidsField;

        [XmlElement(IsNullable = true)]
        public string ack {
            get { return this.ackField; }
            set { this.ackField = value; }
        }

        [XmlElement(IsNullable = true)]
        public string text {
            get { return this.textField; }
            set { this.textField = value; }
        }

        [XmlArray(IsNullable = true)]
        [XmlArrayItem("item", IsNullable = false)]
        public List<string> uuids {
        //public string[] uuids {
            get { return this.uuidsField; }
            set { this.uuidsField = value; }
        }
    }
}