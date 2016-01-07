using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.SAT.CFDiWebSite.Models;
using Sistrategia.SAT.CFDiWebSite.CFDI;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
    public class ReceptorController : BaseController
    {
        public ActionResult Index() {
            //var model = new ReceptorIndexViewModel {
            //    Receptores = this.DBContext.Receptores.Where(r => r.Status == "A").ToList()
            //};
            //return View(model);
            return View();
        }

        [HttpPost]
        public JsonResult LoadReceptores(int page, int pageSize, string search = null, string sort = null, string sortDir = null) {
            sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
            List<object> itemList = new List<object>();
            try {

                Func<Receptor, Object> orderByFunc = null;
                switch (sort) {
                    case "Nombre":
                        orderByFunc = sl => sl.Nombre;
                        break;
                    case "RFC":
                        orderByFunc = sl => sl.RFC;
                        break;                   
                    default:
                        orderByFunc = sl => sl.Nombre;
                        break;
                }

                List<Receptor> Receptores = new List<Receptor>();
                if (search != null)
                    Receptores = sortDir == "ASC" ? DBContext.Receptores.Where(x => x.Status.Equals("A") && (x.Nombre.Contains(search)
                        || x.RFC.Contains(search)) 
                        ).OrderBy(orderByFunc)
                        .Take(((page - 1) * pageSize) + pageSize)
                        .Skip(((page - 1) * pageSize)).ToList()
                        :
                        DBContext.Receptores.Where(x => x.Status.Equals("A") && (x.Nombre.Contains(search)
                        || x.RFC.Contains(search)) 
                        ).OrderByDescending(orderByFunc)
                        .Take(((page - 1) * pageSize) + pageSize)
                        .Skip(((page - 1) * pageSize)).ToList();
                else
                    Receptores = sortDir == "ASC" ? DBContext.Receptores.Where(x => x.Status.Equals("A")).OrderBy(orderByFunc).Take(((page - 1) * pageSize) + pageSize).Skip(((page - 1) * pageSize)).ToList()
                        : DBContext.Receptores.Where(x => x.Status.Equals("A")).OrderByDescending(orderByFunc).Take(((page - 1) * pageSize) + pageSize).Skip(((page - 1) * pageSize)).ToList();

                if (Receptores.Count > 0) {
                    int ReceptoresTotalRows = DBContext.Receptores.Where(x => x.Status.Equals("A") && (x.Nombre.Contains(search) || x.RFC.Contains(search))).Count();

                    foreach (Receptor receptor in Receptores) {
                        var dynamicItems = new {
                            error = false,
                            total_rows = ReceptoresTotalRows,
                            returned_rows = Receptores.Count,
                            comprobante_id = receptor.ReceptorId,
                            public_key = receptor.PublicKey,
                            receptor = receptor.Nombre,
                            rfc = receptor.RFC,
                            receptor_initial_letter = receptor.Nombre.Substring(0, 1),

                        };
                        itemList.Add(dynamicItems);
                    }
                }
            }
            catch (Exception ex) {
                var errorMessage = new {
                    error = true,
                    errorMsg = ex.ToString()
                };
                itemList.Add(errorMessage);
            }

            return Json(itemList);
        }

        public ActionResult Create() {
            var model = new ReceptorCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReceptorCreateViewModel model) {
            //var model = new EmisorCreateViewModel();
            var receptor = new Receptor();

            receptor.RFC = model.RFC;

            if (!string.IsNullOrEmpty(model.Nombre))
                receptor.Nombre = model.Nombre;

            if (model.Domicilio != null) {
                if (!string.IsNullOrEmpty(model.Domicilio.Pais)) {
                    receptor.Domicilio = new Ubicacion {
                        Pais = model.Domicilio.Pais,
                        Calle = string.IsNullOrEmpty(model.Domicilio.Calle) ? null : model.Domicilio.Calle,
                        NoExterior = string.IsNullOrEmpty(model.Domicilio.NoExterior) ? null : model.Domicilio.NoExterior,
                        NoInterior = string.IsNullOrEmpty(model.Domicilio.NoInterior) ? null : model.Domicilio.NoInterior,
                        Colonia = string.IsNullOrEmpty(model.Domicilio.Colonia) ? null : model.Domicilio.Colonia,
                        Localidad = string.IsNullOrEmpty(model.Domicilio.Localidad) ? null : model.Domicilio.Localidad,
                        Municipio = string.IsNullOrEmpty(model.Domicilio.Municipio) ? null : model.Domicilio.Municipio,
                        Estado = string.IsNullOrEmpty(model.Domicilio.Estado) ? null : model.Domicilio.Estado,
                        CodigoPostal = string.IsNullOrEmpty(model.Domicilio.CodigoPostal) ? null : model.Domicilio.CodigoPostal,
                        Referencia = string.IsNullOrEmpty(model.Domicilio.Referencia) ? null : model.Domicilio.Referencia
                    };
                }
            }           

            //if (!string.IsNullOrEmpty(model.RegimenFiscal))
            //    receptor.RegimenFiscal = new List<RegimenFiscal> {
            //        new RegimenFiscal {
            //            Regimen = model.RegimenFiscal
            //        }
            //    };

            this.DBContext.Receptores.Add(receptor);
            this.DBContext.SaveChanges();

            //return RedirectToAction("Details", new { Id = receptor.PublicKey.ToString("N") }); // "Index", "Home");
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var receptor = DBContext.Receptores.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (receptor == null)
                return HttpNotFound();

            var model = new ReceptorDetailsViewModel(receptor);
            return View(model);
        }

        public ActionResult Edit(string id) {
            Guid publicKey = Guid.Parse(id);
            var receptor = DBContext.Receptores.Where(r => r.PublicKey == publicKey).SingleOrDefault();
            var model = new ReceptorEditViewModel(receptor);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ReceptorEditViewModel model) {
            Guid publicKey = model.PublicKey; //  Guid.Parse(id);
            var originalReceptor = DBContext.Receptores.Where(r => r.PublicKey == publicKey).SingleOrDefault();

            if (originalReceptor.RFC.Equals(model.RFC) && originalReceptor.Nombre.Equals(model.Nombre)) {
                // if ubicacion is equals then don't create newReceptor

                if (model.Domicilio != null && originalReceptor.Domicilio != null) {
                    if (originalReceptor.Domicilio.Pais.Equals(model.Domicilio.Pais)
                        && originalReceptor.Domicilio.Calle.Equals(model.Domicilio.Calle)
                        && originalReceptor.Domicilio.NoExterior.Equals(model.Domicilio.NoExterior)
                        && originalReceptor.Domicilio.NoInterior.Equals(model.Domicilio.NoInterior)
                        && originalReceptor.Domicilio.Colonia.Equals(model.Domicilio.Colonia)
                        && originalReceptor.Domicilio.Localidad.Equals(model.Domicilio.Localidad)
                        && originalReceptor.Domicilio.Municipio.Equals(model.Domicilio.Municipio)
                        && originalReceptor.Domicilio.Estado.Equals(model.Domicilio.Estado)
                        && originalReceptor.Domicilio.CodigoPostal.Equals(model.Domicilio.CodigoPostal)
                        && originalReceptor.Domicilio.Referencia.Equals(model.Domicilio.Referencia)                        
                        )
                    {
                        return RedirectToAction("Index");
                    }
                }

            }

            var newReceptor = new Receptor();

            newReceptor.RFC = model.RFC;

            if (!string.IsNullOrEmpty(model.Nombre))
                newReceptor.Nombre = model.Nombre;

            if (model.Domicilio != null) {
                if (!string.IsNullOrEmpty(model.Domicilio.Pais)) {
                    newReceptor.Domicilio = new Ubicacion {
                        Pais = model.Domicilio.Pais,
                        Calle = string.IsNullOrEmpty(model.Domicilio.Calle) ? null : model.Domicilio.Calle,
                        NoExterior = string.IsNullOrEmpty(model.Domicilio.NoExterior) ? null : model.Domicilio.NoExterior,
                        NoInterior = string.IsNullOrEmpty(model.Domicilio.NoInterior) ? null : model.Domicilio.NoInterior,
                        Colonia = string.IsNullOrEmpty(model.Domicilio.Colonia) ? null : model.Domicilio.Colonia,
                        Localidad = string.IsNullOrEmpty(model.Domicilio.Localidad) ? null : model.Domicilio.Localidad,
                        Municipio = string.IsNullOrEmpty(model.Domicilio.Municipio) ? null : model.Domicilio.Municipio,
                        Estado = string.IsNullOrEmpty(model.Domicilio.Estado) ? null : model.Domicilio.Estado,
                        CodigoPostal = string.IsNullOrEmpty(model.Domicilio.CodigoPostal) ? null : model.Domicilio.CodigoPostal,
                        Referencia = string.IsNullOrEmpty(model.Domicilio.Referencia) ? null : model.Domicilio.Referencia
                    };
                }
            }

            //if (!string.IsNullOrEmpty(model.RegimenFiscal))
            //    receptor.RegimenFiscal = new List<RegimenFiscal> {
            //        new RegimenFiscal {
            //            Regimen = model.RegimenFiscal
            //        }
            //    };

            originalReceptor.Status = "I"; // Inactive
            //this.DBContext.Receptores.Add(originalReceptor);
            this.DBContext.Receptores.Add(newReceptor);
            this.DBContext.SaveChanges();

            //return RedirectToAction("Details", new { Id = receptor.PublicKey.ToString("N") }); // "Index", "Home");
            return RedirectToAction("Index");


        }
    }
}