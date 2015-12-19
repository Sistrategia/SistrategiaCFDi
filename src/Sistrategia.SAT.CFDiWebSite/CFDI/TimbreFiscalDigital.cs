using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    /// <summary>
    /// Complemento requerido para el Timbrado Fiscal Digital que da valides a un Comprobante Fiscal Digital.
    /// </summary>
    public class TimbreFiscalDigital
    {
        private int timbreFiscalDigitalId;
        //private Guid satTimbreId;
        private Guid publicKey;
        private string version;
        private string uuid;
        private DateTime fechaTimbrado;
        private string selloCFD;
        private string noCertificadoSAT;
        private string selloSAT;

        //public Guid SatTimbreId {
        //    get { return this.satTimbreId; }
        //    set { this.satTimbreId = value; }
        //}

        public int TimbreFiscalDigitalId {
            get { return this.timbreFiscalDigitalId; }
            set { this.timbreFiscalDigitalId = value; }
        }

        public Guid PublicKey {
            get { return this.publicKey; }
            set { this.publicKey = value; }
        }

        //<xs:attribute name="version" use="required" fixed="1.0">
        //<xs:annotation>
        //<xs:documentation>
        //Atributo requerido para la expresión de la versión del estándar del Timbre Fiscal Digital
        //</xs:documentation>
        //</xs:annotation>
        //</xs:attribute>
        public string Version {
            get { return this.version; }
            set { this.version = value; }
        }

        //<xs:attribute name="UUID" use="required" id="UUID">
        //<xs:annotation>
        //<xs:documentation>
        //Atributo requerido para expresar los 36 caracteres del UUID de la transacción de timbrado
        //</xs:documentation>
        //</xs:annotation>
        //<xs:simpleType>
        //<xs:restriction base="xs:string">
        //<xs:whiteSpace value="collapse"/>
        //<xs:length value="36"/>
        //<xs:pattern value="[a-f0-9A-F]{8}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{4}-[a-f0-9A-F]{12}"/>
        //</xs:restriction>
        //</xs:simpleType>
        //</xs:attribute>
        public string UUID {
            get { return this.uuid; }
            set { this.uuid = value; }
        }

        //<xs:attribute name="FechaTimbrado" use="required">
        //<xs:annotation>
        //<xs:documentation>
        //Atributo requerido para expresar la fecha y hora de la generación del timbre
        //</xs:documentation>
        //</xs:annotation>
        //<xs:simpleType>
        //<xs:restriction base="xs:dateTime">
        //<xs:whiteSpace value="collapse"/>
        //</xs:restriction>
        //</xs:simpleType>
        //</xs:attribute>
        public DateTime FechaTimbrado {
            get { return this.fechaTimbrado; }
            set { this.fechaTimbrado = value; }
        }

        //<xs:attribute name="selloCFD" use="required">
        //<xs:annotation>
        //<xs:documentation>
        //Atributo requerido para contener el sello digital del comprobante fiscal, que será timbrado. El sello deberá ser expresado cómo una cadena de texto en formato Base 64.
        //</xs:documentation>
        //</xs:annotation>
        //<xs:simpleType>
        //<xs:restriction base="xs:string">
        //<xs:whiteSpace value="collapse"/>
        //</xs:restriction>
        //</xs:simpleType>
        //</xs:attribute>
        public string SelloCFD {
            get { return this.selloCFD; }
            set { this.selloCFD = value; }
        }

        //<xs:attribute name="noCertificadoSAT" use="required">
        //<xs:annotation>
        //<xs:documentation>
        //Atributo requerido para expresar el número de serie del certificado del SAT usado para el Timbre
        //</xs:documentation>
        //</xs:annotation>
        //<xs:simpleType>
        //<xs:restriction base="xs:string">
        //<xs:whiteSpace value="collapse"/>
        //<xs:length value="20"/>
        //</xs:restriction>
        //</xs:simpleType>
        //</xs:attribute>
        public string NoCertificadoSAT {
            get { return this.noCertificadoSAT; }
            set { this.noCertificadoSAT = value; }
        }

        //<xs:attribute name="selloSAT" use="required">
        //<xs:annotation>
        //<xs:documentation>
        //Atributo requerido para contener el sello digital del Timbre Fiscal Digital, al que hacen referencia las reglas de resolución miscelánea aplicable. El sello deberá ser expresado cómo una cadena de texto en formato Base 64.
        //</xs:documentation>
        //</xs:annotation>
        //<xs:simpleType>
        //<xs:restriction base="xs:string">
        //<xs:whiteSpace value="collapse"/>
        //</xs:restriction>
        //</xs:simpleType>
        //</xs:attribute>
        public string SelloSAT {
            get { return this.selloSAT; }
            set { this.selloSAT = value; }
        }
    }
}
