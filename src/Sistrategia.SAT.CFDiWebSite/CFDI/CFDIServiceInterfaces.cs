using System;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public interface ICFDIService // ICFDITimbradoService?
    {
        byte[] GetCFDI(string user, string password, byte[] file);
        byte[] GetTimbreCFDI(string user, string password, byte[] file);
        ICancelaResponse CancelaCFDI(string user, string password, string rfc, string[] uuid, byte[] pfx, string pfxPassword);
    }

    public interface ICancelaResponse
    {
        //Guid ComprobanteId { get; set; }
        string Ack { get; set; }
        string Text { get; set; }
        string[] UUIDs { get; set; }
        string XmlResponse { get; set; }
    }

    public class CancelaResponseBase : ICancelaResponse
    {
        //private Guid comprobanteCanceladoId;
        private string ack;
        private string text;
        private string[] uuids;
        //private Guid comprobanteId;
        private string xmlResponse;

        //public Guid ComprobanteCanceladoId {
        //    get { return this.comprobanteCanceladoId; }
        //    set { this.comprobanteCanceladoId = value; }
        //}

        public string Ack {
            get { return this.ack; }
            set { this.ack = value; }
        }

        public string Text {
            get { return this.text; }
            set { this.text = value; }
        }

        public string[] UUIDs {
            get { return this.uuids; }
            set { this.uuids = value; }
        }

        //public Guid ComprobanteId {
        //    get { return this.comprobanteId; }
        //    set { this.comprobanteId = value; }
        //}

        public string XmlResponse {
            get { return this.xmlResponse; }
            set { this.xmlResponse = value; }
        }
    }

}