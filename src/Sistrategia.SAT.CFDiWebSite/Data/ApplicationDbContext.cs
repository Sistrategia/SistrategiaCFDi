using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sistrategia.SAT.CFDiWebSite.Security;
using Sistrategia.SAT.CFDiWebSite.CFDI;


namespace Sistrategia.SAT.CFDiWebSite.Data
{
    public class ApplicationDbContext : IdentityDbContext<SecurityUser, SecurityRole
        , int, SecurityUserLogin, SecurityUserRole, SecurityUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultDatabase") {
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Emisor> Emisores { get; set; }
        public virtual DbSet<Ubicacion> Ubicaciones { get; set; }
        //public virtual DbSet<UbicacionFiscal> UbicacionesFiscales { get; set; }

        public virtual DbSet<Certificado> Certificados { get; set; }

        public virtual DbSet<Receptor> Receptores { get; set; }

        public virtual DbSet<Comprobante> Comprobantes { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder) {
            var user = modelBuilder.Entity<SecurityUser>()
                .ToTable("security_user");
            user.Property(u => u.Id).HasColumnName("user_id")
                .HasColumnOrder(1);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .HasColumnName("user_name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnOrder(2)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_user_name_index") { IsUnique = true }));
            user.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_user_public_key_index") { IsUnique = true }));
            user.Property(p => p.FullName)
                .HasColumnName("full_name")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_user_full_name_index") { IsUnique = false }));
            user.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(256);
            user.Property(u => u.EmailConfirmed)
                .HasColumnName("email_confirmed");
            user.Property(u => u.PasswordHash)
                .HasColumnName("password_hash");
            user.Property(u => u.SecurityStamp)
                .HasColumnName("security_stamp");
            user.Property(u => u.PhoneNumber)
                .HasColumnName("phone_number");
            user.Property(u => u.PhoneNumberConfirmed)
                .HasColumnName("phone_number_confirmed");
            user.Property(u => u.TwoFactorEnabled)
                .HasColumnName("two_factor_enabled");
            user.Property(u => u.LockoutEndDateUtc)
                .HasColumnName("lockout_end_date_utc");
            user.Property(u => u.LockoutEnabled)
                .HasColumnName("lockout_enabled");
            user.Property(u => u.AccessFailedCount)
                .HasColumnName("access_failed_count");

            var role = modelBuilder.Entity<SecurityRole>()
               .ToTable("security_roles");
            role.Property(r => r.Id).HasColumnName("role_id");
            role.Property(r => r.Name)
                .HasColumnName("role_name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_role_name_index") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);

            var userRole = modelBuilder.Entity<SecurityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("security_user_roles");
            userRole.Property(pr1 => pr1.RoleId).HasColumnName("role_id");
            userRole.Property(pr2 => pr2.UserId).HasColumnName("user_id");

            var userLogin = modelBuilder.Entity<SecurityUserLogin>()
                 .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                 .ToTable("security_user_logins");
            userLogin.Property(pr1 => pr1.LoginProvider).HasColumnName("login_provider");
            userLogin.Property(pr2 => pr2.ProviderKey).HasColumnName("provider_key");
            userLogin.Property(pr3 => pr3.UserId).HasColumnName("user_id");

            var userClaim = modelBuilder.Entity<SecurityUserClaim>()
                .ToTable("security_user_claims");
            userClaim.Property(pr1 => pr1.Id).HasColumnName("claim_id");
            userClaim.Property(pr2 => pr2.UserId).HasColumnName("user_id");
            userClaim.Property(pr3 => pr3.ClaimType).HasColumnName("claim_type");
            userClaim.Property(pr4 => pr4.ClaimValue).HasColumnName("claim_value");

            var emisor = modelBuilder.Entity<Emisor>()
                .ToTable("sat_emisor");
            emisor.Property(p => p.EmisorId)
                .HasColumnName("emisor_id");
            emisor.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            emisor.Property(p => p.RFC)
                .HasColumnName("rfc");
            emisor.Property(p => p.Nombre)
                .HasColumnName("nombre");

            emisor.Property(p => p.DomicilioFiscalId)
                .HasColumnName("domicilio_fiscal_id");
            emisor.Property(p => p.ExpedidoEnId)
                .HasColumnName("expedido_en_id");

            var ubicacion = modelBuilder.Entity<Ubicacion>()
                // .ToTable("sat_ubicacion");
                ;
            ubicacion.Property(p => p.UbicacionId)
                .HasColumnName("ubicacion_id");
            ubicacion.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            ubicacion.Property(p => p.Calle)
                .HasColumnName("calle")
                .IsOptional();
            ubicacion.Property(p => p.NoExterior)
                .HasColumnName("no_exterior")
                .IsOptional();
            ubicacion.Property(p => p.NoInterior)
                .HasColumnName("no_interior")
                .IsOptional();
            ubicacion.Property(p => p.Colonia)
                .HasColumnName("colonia")
                .IsOptional();
            ubicacion.Property(p => p.Localidad)
                .HasColumnName("localidad")
                .IsOptional();
            ubicacion.Property(p => p.Referencia)
                .HasColumnName("referencia")
                .IsOptional();
            ubicacion.Property(p => p.Municipio)
                .HasColumnName("municipio")
                .IsOptional();
            ubicacion.Property(p => p.Estado)
                .HasColumnName("estado")
                .IsOptional();
            ubicacion.Property(p => p.Pais)
                .HasColumnName("pais")
                .IsRequired();
            ubicacion.Property(p => p.CodigoPostal)
                .HasColumnName("codigo_postal")
                .IsOptional();


            modelBuilder.Entity<Ubicacion>()
                .Map<Ubicacion>(m => m.Requires("ubicacion_type").HasValue("Ubicacion"))
                .Map<UbicacionFiscal>(m => m.Requires("ubicacion_type").HasValue("UbicacionFiscal"))
                .ToTable("sat_ubicacion");

            //modelBuilder.Entity<UbicacionFiscal>().Map(m =>
            //{
            //    m.MapInheritedProperties();
            //    m.ToTable("UbicacionFiscal");
            //});

            //var ubicacionFiscal = modelBuilder.Entity<UbicacionFiscal>()
            //    .ToTable("sat_ubicacion_fiscal");
            //ubicacionFiscal.Property(p => p.UbicacionId)
            //    .HasColumnName("ubicacion_fiscal_id");
            //ubicacionFiscal.Property(p => p.PublicKey)
            //    .HasColumnName("public_key")
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            //ubicacionFiscal.Property(p => p.Calle)
            //    .HasColumnName("calle");
            //ubicacionFiscal.Property(p => p.NoExterior)
            //    .HasColumnName("no_exterior");
            //ubicacionFiscal.Property(p => p.NoInterior)
            //    .HasColumnName("no_interior");
            //ubicacionFiscal.Property(p => p.Colonia)
            //    .HasColumnName("colonia");
            //ubicacionFiscal.Property(p => p.Localidad)
            //    .HasColumnName("localidad");
            //ubicacionFiscal.Property(p => p.Referencia)
            //    .HasColumnName("referencia");
            //ubicacionFiscal.Property(p => p.Municipio)
            //    .HasColumnName("municipio");
            //ubicacionFiscal.Property(p => p.Estado)
            //    .HasColumnName("estado");
            //ubicacionFiscal.Property(p => p.Pais)
            //    .HasColumnName("pais");
            //ubicacionFiscal.Property(p => p.CodigoPostal)
            //    .HasColumnName("codigo_postal");

            var certificado = modelBuilder.Entity<Certificado>()
                .ToTable("sat_certificado");
            certificado.Property(p => p.CertificadoId)
                .HasColumnName("certificado_id");
            certificado.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            certificado.Property(p => p.NumSerie)
                .HasColumnName("num_serie")
                .IsRequired()
                .HasMaxLength(20);
            certificado.Property(p => p.RFC)
                .HasColumnName("rfc")
                .IsRequired()
                .HasMaxLength(13);
            certificado.Property(p => p.Inicia)
                .HasColumnName("inicia")
                .IsRequired();
            certificado.Property(p => p.Finaliza)
                .HasColumnName("finaliza")
                .IsRequired();
            certificado.Property(p => p.CertificadoBase64)
                .HasColumnName("certificado")
                //.HasMaxLength(20)
                .IsRequired();
            certificado.Property(p => p.PFXArchivo)
                .HasColumnName("pfx_archivo");
            certificado.Property(p => p.PFXContrasena)
                .HasColumnName("pfx_contrasena")
                .HasMaxLength(2048);
            certificado.Property(p => p.Estado)
                .HasColumnName("estado")
                .IsRequired();


            var regimenFiscal = modelBuilder.Entity<RegimenFiscal>()
                .ToTable("sat_regimen_fiscal");
            regimenFiscal.Property(p => p.RegimenFiscalId)
                .HasColumnName("regimen_fiscal_id");
            regimenFiscal.Property(p => p.Regimen)
                .HasColumnName("regimen");

            emisor.HasMany<Certificado>(p => p.Certificados)
                .WithOptional()
                .Map(pe => pe.MapKey("emisor_id"));

            emisor.HasMany<RegimenFiscal>(p => p.RegimenFiscal)
                .WithOptional()
                .Map(pe => pe.MapKey("emisor_id"));

            var receptor = modelBuilder.Entity<Receptor>()
                .ToTable("sat_receptor");
            receptor.Property(p => p.ReceptorId)
                .HasColumnName("receptor_id");
            receptor.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            receptor.Property(p => p.RFC)
                .HasColumnName("rfc");
            receptor.Property(p => p.Nombre)
                .HasColumnName("nombre");

            receptor.Property(p => p.DomicilioId)
                .HasColumnName("domicilio_id");

            var comprobante = modelBuilder.Entity<Comprobante>()
                .ToTable("sat_comprobante");
            comprobante.Property(p => p.ComprobanteId)
                .HasColumnName("comprobante_id");
            comprobante.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            comprobante.Property(p => p.Version)
                .HasColumnName("version")
                .IsRequired()
                .HasMaxLength(20);
            comprobante.Property(p => p.Serie)
                .HasColumnName("serie")
                .IsOptional()
                .HasMaxLength(25);
            comprobante.Property(p => p.Folio)
                .HasColumnName("folio")
                .IsOptional()
                .HasMaxLength(20);
            comprobante.Property(p => p.Fecha)
                .HasColumnName("fecha")
                .IsRequired();
            comprobante.Property(p => p.Sello)
                .HasColumnName("sello")
                .IsOptional() // porque sino no se puede guardar el comprobante antes de sellarse.
                .HasMaxLength(2048);
            comprobante.Property(p => p.FormaDePago)
                .HasColumnName("forma_de_pago")
                .IsRequired() // DEFAULT 'PAGO EN UNA SOLA EXHIBICION'
                .HasMaxLength(256);
            comprobante.Property(p => p.NoCertificado)
                .HasColumnName("no_certificado")
                .IsOptional()
                .HasMaxLength(20);
            comprobante.Property(p => p.Certificado)
                .HasColumnName("certificado")
                .IsOptional()
                .HasMaxLength(2048);
            comprobante.Property(p => p.CondicionesDePago)
                .HasColumnName("condiciones_de_pago")
                .IsOptional()
                .HasMaxLength(2048);
            comprobante.Property(p => p.SubTotal)
                .HasColumnName("sub_total")
                .HasPrecision(18, 6)
                .IsRequired();
            comprobante.Property(p => p.Descuento)
                .HasColumnName("descuento")
                .HasPrecision(18, 6)
                .IsOptional();
            //comprobante.Ignore(p => p.DescuentoSpecified);
            comprobante.Property(p => p.MotivoDescuento)
                .HasColumnName("motivo_descuento")
                .IsOptional()
                .HasMaxLength(2048);
            comprobante.Property(p => p.TipoCambio)
                .HasColumnName("tipo_cambio")
                .IsOptional()
                .HasMaxLength(50);
            comprobante.Property(p => p.Moneda)
                .HasColumnName("moneda")
                .IsOptional()
                .HasMaxLength(50);
            comprobante.Property(p => p.Total)
                .HasColumnName("total")
                .HasPrecision(18, 6)
                .IsRequired();
            comprobante.Property(p => p.TipoDeComprobante)
                .HasColumnName("tipo_de_comprobante")
                .HasMaxLength(50)
                .IsRequired();
            comprobante.Property(p => p.MetodoDePago)
                .HasColumnName("metodo_de_pago")
                .IsRequired() // Requerido
                .HasMaxLength(256);
            comprobante.Property(p => p.LugarExpedicion)
                .HasColumnName("lugar_expedicion")
                .IsRequired() // Requerido
                .HasMaxLength(2048);
            comprobante.Property(p => p.NumCtaPago)
                .HasColumnName("num_cta_pago")
                .IsOptional()
                .HasMaxLength(256);
            comprobante.Property(p => p.FolioFiscalOrig)
                .HasColumnName("folio_fiscal_orig")
                .IsOptional()
                .HasMaxLength(256);
            comprobante.Property(p => p.SerieFolioFiscalOrig)
                .HasColumnName("serie_folio_fiscal_orig")
                .IsOptional()
                .HasMaxLength(256);
            comprobante.Property(p => p.FechaFolioFiscalOrig)
                .HasColumnName("fecha_folio_fiscal_orig")
                .IsOptional();
            //comprobante.Ignore(p => p.FechaFolioFiscalOrigSpecified);
            comprobante.Property(p => p.MontoFolioFiscalOrig)
                .HasColumnName("monto_folio_fiscal_orig")
                .HasPrecision(18, 6)
                .IsOptional();
            //comprobante.Ignore(p => p.MontoFolioFiscalOrigSpecified);
            comprobante.Property(p => p.EmisorId)
                .HasColumnName("emisor_id");
            comprobante.Property(p => p.ReceptorId)
                .HasColumnName("receptor_id");

            comprobante.Ignore(p => p.DecimalFormat);

            comprobante.Property(p => p.ExtendedIntValue1)
                .HasColumnName("extended_int_value_1");
            comprobante.Property(p => p.ExtendedIntValue2)
                .HasColumnName("extended_int_value_2");
            comprobante.Property(p => p.ExtendedIntValue3)
                .HasColumnName("extended_int_value_3");

            comprobante.Property(p => p.ExtendedStringValue1)
                .HasColumnName("extended_string_value_1");
            comprobante.Property(p => p.ExtendedStringValue2)
                .HasColumnName("extended_string_value_2");
            comprobante.Property(p => p.ExtendedStringValue3)
                .HasColumnName("extended_string_value_3");                    


            var concepto = modelBuilder.Entity<Concepto>()
                .ToTable("sat_concepto");
            concepto.Property(p => p.ConceptoId)
                .HasColumnName("concepto_id");
            concepto.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            concepto.Property(p => p.Cantidad)
                .HasColumnName("cantidad")
                .HasPrecision(18, 6)
                .IsRequired();
            concepto.Property(p => p.Unidad)
                .HasColumnName("unidad")
                .HasMaxLength(50)
                .IsOptional();
            concepto.Property(p => p.NoIdentificacion)
                .HasColumnName("no_identificacion")
                .HasMaxLength(256)
                .IsOptional();
            concepto.Property(p => p.Descripcion)
                .HasColumnName("descripcion")
                //.HasMaxLength(2048)
                .IsOptional();
            concepto.Property(p => p.ValorUnitario)
                .HasColumnName("valor_unitario")
                .HasPrecision(18, 6)
                .IsRequired();
            concepto.Property(p => p.Importe)
                .HasColumnName("importe")
                .HasPrecision(18, 6)
                .IsRequired();

            comprobante.HasMany<Concepto>(p => p.Conceptos)
                .WithOptional()
                .Map(pe => pe.MapKey("comprobante_id"));

            //comprobante.HasMany<Concepto>(p => p.Conceptos)
            //    .WithOptional()
            //    .Map(pe => pe.MapKey("comprobante_id"));

            //var impuestos = modelBuilder.Entity<Impuestos>()
            //    .HasRequired(i => i.Comprobante)
            //    .WithOptional(c => c.Impuestos)

            var impuestos = modelBuilder.Entity<Impuestos>()
                .ToTable("sat_impuestos");

            impuestos.Property(p => p.ImpuestosId)
                .HasColumnName("impuestos_id");

            comprobante.Property(p => p.ImpuestosId)
                .HasColumnName("impuestos_id");

            comprobante.HasOptional(c => c.Impuestos);
            //<Impuestos>(p => p.Impuestos)
            //.WithRequired()
            //.Map(pe => pe.MapKey("impuestos_id"));

            var retencion = modelBuilder.Entity<Retencion>()
                .ToTable("sat_retencion");

            retencion.Property(p => p.RetencionId)
                .HasColumnName("retencion_id")
                ;
            retencion.Property(p => p.Impuesto)
                .HasColumnName("impuesto");
            retencion.Property(p => p.Importe)
                .HasColumnName("importe");

            impuestos.HasMany<Retencion>(p => p.Retenciones)
                .WithOptional()
                .Map(pe => pe.MapKey("impuesto_id"));

            var traslado = modelBuilder.Entity<Traslado>()
                .ToTable("sat_traslado");
            traslado.Property(p => p.TrasladoId)
                .HasColumnName("traslado_id")
                ;
            traslado.Property(p => p.Importe)
                .HasColumnName("importe");
            traslado.Property(p => p.Impuesto)
                .HasColumnName("impuesto");
            traslado.Property(p => p.Tasa)
                .HasColumnName("tasa");

            impuestos.HasMany<Traslado>(p => p.Traslados)
                .WithOptional()
                .Map(pe => pe.MapKey("impuesto_id"));

            var complemento = modelBuilder.Entity<Complemento>()
                .ToTable("sat_complemento");
            complemento.Property(p => p.ComplementoId)
                .HasColumnName("complemento_id");

            comprobante.HasMany<Complemento>(p => p.Complementos)
                .WithOptional()
                .Map(pe => pe.MapKey("comprobante_id"));


            var timbre = modelBuilder.Entity<TimbreFiscalDigital>()
                .ToTable("sat_timbre_fiscal_digital");
            //timbre.Property(p => p.TimbreFiscalDigitalId)
            //    .HasColumnName("timbre_fiscal_digital_id");
            //PublicKey
            timbre.Property(p => p.Version)
                .HasColumnName("version");
            timbre.Property(p => p.UUID)
                .HasColumnName("uuid");
            timbre.Property(p => p.FechaTimbrado)
                .HasColumnName("fecha_timbrado");
            timbre.Property(p => p.SelloCFD)
                .HasColumnName("sello_cfd");
            timbre.Property(p => p.NoCertificadoSAT)
                .HasColumnName("no_certificado_sat");
            timbre.Property(p => p.SelloSAT)
                .HasColumnName("sello_sat");
        }
                
    }
}