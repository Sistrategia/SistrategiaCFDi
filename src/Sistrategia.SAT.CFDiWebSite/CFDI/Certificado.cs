using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sistrategia.SAT.CFDiWebSite.CFDI
{
    public class Certificado
    {
        public Certificado()
            : base() {
            this.PublicKey = Guid.NewGuid();
        }

        [Key]
        public int CertificadoId { get; set; }

        [Required]
        public Guid PublicKey { get; set; }        

        public string NumSerie { get; set; }
        public string RFC { get; set; }
        public DateTime Inicia { get; set; }
        public DateTime Finaliza { get; set; }

        public string CertificadoBase64 { get; set; }
        public byte[] PFXArchivo { get; set; }
        public string PFXContrasena { get; set; }

        public byte[] CertificadoDER { get; set; }
        public byte[] PrivateKeyDER { get; set; }
        public string PrivateKeyContrasena { get; set; }

        public string Estado { get; set; }

        // [XmlIgnore]
        public int Ordinal { get; set; }

        private System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate2 = null;
        private System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = null;

        protected System.Security.Cryptography.SHA1CryptoServiceProvider GetSHA1CryptoServiceProvider() {
            if (this.sha1 == null) {
                this.sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            }
            return this.sha1;
        }

        protected System.Security.Cryptography.X509Certificates.X509Certificate2 GetX509Certificate2() {
            if (this.x509Certificate2 == null) {
                if (this.PFXArchivo != null && this.PFXArchivo.Length > 0 && !string.IsNullOrEmpty(this.PFXContrasena)) {
                    this.x509Certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(this.PFXArchivo,
                        this.PFXContrasena, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
                }
                else if (this.CertificadoDER != null && this.CertificadoDER.Length > 0 && !string.IsNullOrEmpty(this.PrivateKeyContrasena)) {
                    this.x509Certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(this.CertificadoDER,
                        this.PrivateKeyContrasena, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
                }
            }

            return this.x509Certificate2;
        }


        public string GetNumeroSerie() {
            return Certificado.GetSerialNumberString(this.GetX509Certificate2());
        }

        public string GetSubject() {
            return Certificado.GetSubjectString(this.GetX509Certificate2());
        }

        public string GetSubjectSimpleName() {
            return Certificado.GetSubjectSimpleName(this.GetX509Certificate2());
        }

        public string GetIssuerName() {
            return Certificado.GetIssuerName(this.GetX509Certificate2());
        }

        private static string GetIssuerName(System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate2) {
             return x509Certificate2.IssuerName.Name;
        }

        public string GetIssuerSimpleName() {
            return Certificado.GetIssuerSimpleName(this.GetX509Certificate2());
        }

        public static string GetIssuerSimpleName(System.Security.Cryptography.X509Certificates.X509Certificate2 x509Certificate2) {
            return x509Certificate2.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, true);
        }

        public string GetRFC() {
            return Certificado.GetRFC(this.GetX509Certificate2());
        }

        public string GetEffectiveDateString() {
            return Certificado.GetEffectiveDateString(this.GetX509Certificate2());
        }

        public string GetExpirationDateString() {
            return Certificado.GetExpirationDateString(this.GetX509Certificate2());
        }

        private static string GetEffectiveDateString(System.Security.Cryptography.X509Certificates.X509Certificate2 cert) {
            return cert.GetEffectiveDateString();
        }

        private static string GetExpirationDateString(System.Security.Cryptography.X509Certificates.X509Certificate2 cert) {
            return cert.GetExpirationDateString();
        }

        private static string GetSubjectString(System.Security.Cryptography.X509Certificates.X509Certificate2 cert) {
            return cert.Subject; // .GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, false);
        }

        private static string GetSubjectSimpleName(System.Security.Cryptography.X509Certificates.X509Certificate2 cert) {
            return cert.GetNameInfo(System.Security.Cryptography.X509Certificates.X509NameType.SimpleName, false);
        }

        public static string GetSerialNumberString(System.Security.Cryptography.X509Certificates.X509Certificate2 cert) {
            var hexString = cert.GetSerialNumberString();
            var sb = new StringBuilder();
            for (int i = 0; i < hexString.Length; i += 2) {
                string hs = hexString.Substring(i, 2);
                sb.Append(Convert.ToChar(Convert.ToUInt32(hs, 16)));
            }
            return sb.ToString();
        }

        private static string GetRFC(System.Security.Cryptography.X509Certificates.X509Certificate2 cert) {
            string[] subject = cert.Subject.Split(',');
            foreach (string strVal in subject) {
                string value = strVal.Trim();
                if (value.StartsWith("OID.2.5.4.45=")) {
                    string value2 = value.Replace("OID.2.5.4.45=", "");
                    return value2.Substring(0, value2.IndexOf('/') >= 0 ? value2.IndexOf('/') : value2.Length).Trim();
                }
            }
            return null;
        }

        public string GetSello(string cadenaOriginal) {
            if (this.PFXArchivo != null && this.PFXArchivo.Length > 0 && !string.IsNullOrEmpty(this.PFXContrasena))
                return GetSelloFromPFX(cadenaOriginal);
            else {
                return GetSelloFromDerKey(cadenaOriginal);
            }
        }

        private string GetSelloFromDerKey(string cadenaOriginal) {
            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = this.GetSHA1CryptoServiceProvider();
            System.Security.SecureString passwordSeguro = new System.Security.SecureString();
            passwordSeguro.Clear();
            foreach (char c in this.PrivateKeyContrasena.ToCharArray())
                passwordSeguro.AppendChar(c);
            var rsaCryptoIPT = JavaScience.opensslkey.DecodeEncryptedPrivateKeyInfo(this.PrivateKeyDER, passwordSeguro);

            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            byte[] binData = encoder.GetBytes(cadenaOriginal);
            byte[] binSignature = rsaCryptoIPT.SignData(binData, sha1);
            string sello = Convert.ToBase64String(binSignature);
            return sello;
        }


        private string GetSelloFromPFX(string cadenaOriginal) {
            System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = this.GetSHA1CryptoServiceProvider();
            //System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(this.PFXArchivo,
            //     this.PFXContrasena, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
            System.Security.Cryptography.X509Certificates.X509Certificate2 cert = this.GetX509Certificate2();
            System.Security.Cryptography.RSACryptoServiceProvider rsaCryptoIPT = (System.Security.Cryptography.RSACryptoServiceProvider)cert.PrivateKey;

            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            byte[] binData = encoder.GetBytes(cadenaOriginal);
            byte[] binSignature = rsaCryptoIPT.SignData(binData, sha1);
            string sello = Convert.ToBase64String(binSignature);
            return sello;
        }

      
    }
}