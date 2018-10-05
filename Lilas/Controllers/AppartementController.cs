using Lilas.Bdd;
using Lilas.Models;
using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lilas.Controllers
{
    [Authorize]
    public class AppartementController : Controller
    {

        Bdd_Appartement bdd_appartement;
        Bdd_Consommation bdd_conso;


        public AppartementController()
        {
            this.bdd_appartement = new Bdd_Appartement();
            this.bdd_conso = new Bdd_Consommation();
        }


        #region Appartements

        // GET: Appartement
        public ActionResult Appartements()
        {
            Appartement user = bdd_appartement.Get_AppartementFromName(User.Identity.Name);
            AppartementsViewModel vm = new AppartementsViewModel();
            vm.JePartage = user.Partager;
            vm.list_appts = bdd_appartement.Get_AllAppartements().OrderBy(a=>a.AppartementName).ToList();
            vm.nbAppts = vm.list_appts.Count();
            vm.nbApptsTotal = 212;
            vm.nbPartage = vm.list_appts.Where(a => a.Partager == true).ToList().Count();

            return View(vm);
        }


        // GET (AJAX) - Récupére les informations de l'appartement
        public JsonResult GetInformationsAppartement(int paramApptId)
        {
            Appartement appt = bdd_appartement.Get_AppartementFromId(paramApptId);


            var result = new List<object>();

                result.Add(
                    new
                    {
                        appt.Type,
                        appt.IsDoubleVitrage,
                        appt.IsRobinetsThermo,
                        appt.Orientation
                    });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion



        #region Mon Appartement

        // GET: Appartement
        public ActionResult MonAppartement()
        {
            Appartement user = bdd_appartement.Get_AppartementFromName(User.Identity.Name);
            MonAppartementViewModel vm = new MonAppartementViewModel(user);

            return View(vm);
        }


        [HttpPost]
        public ActionResult MonAppartement(MonAppartementViewModel vm)
        {
            Appartement user = bdd_appartement.Get_AppartementFromName(User.Identity.Name);
            bool isModify = false;

            if (user.AppartementName != vm.Appartement)
            {
                isModify = true;
                user.AppartementName = vm.Appartement;
            }

            if (user.UserName != vm.UserName)
            {
                isModify = true;
                user.UserName = vm.UserName;
            }
                
            if (user.Type != vm.Type)
            {
                isModify = true;
                user.Type = vm.Type;
            }
            
            if (user.Orientation != (vm.OrientationNS +" " + vm.OrientationEO))
            {
                isModify = true;
                user.Orientation = "";
                if (vm.OrientationNS.Contains("Sud")) user.Orientation += "Sud ";
                if (vm.OrientationNS.Contains("Nord")) user.Orientation += "Nord ";
                if (vm.OrientationEO.Contains("Est")) user.Orientation += "Est";
                if (vm.OrientationEO.Contains("Ouest")) user.Orientation += "Ouest";
            }

            if (user.IsDoubleVitrage != vm.IsDoubleVitrage)
            {
                isModify = true;
                user.IsDoubleVitrage = vm.IsDoubleVitrage;
            }

            if (user.IsRobinetsThermo != vm.IsRobinetsThermo)
            {
                isModify = true;
                user.IsRobinetsThermo = vm.IsRobinetsThermo;
            }

            if (user.Partager != vm.Partager)
            {
                isModify = true;
                user.Partager = vm.Partager;
            }

            if(isModify)
            {
                bdd_appartement.Update_Appartement(user);
            }


            return RedirectToAction("MonAppartement");
        }


        #endregion
    }
}