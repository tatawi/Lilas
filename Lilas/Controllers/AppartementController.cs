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
        Bdd_Travaux bdd_travaux;


        public AppartementController()
        {
            this.bdd_appartement = new Bdd_Appartement();
            this.bdd_conso = new Bdd_Consommation();
            this.bdd_travaux = new Bdd_Travaux();
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
            //vm.nbApptsTotal = 212;
            //vm.nbPartage = vm.list_appts.Where(a => a.Partager == true).ToList().Count();

            return View(vm);
        }


        // GET (AJAX) - Récupére les informations de l'appartement
        public JsonResult GetInformationsAppartement(int paramApptId)
        {
            Appartement appt = bdd_appartement.Get_AppartementFromId(paramApptId);
            String Orientation = "";
            #region orientation
            if(appt.Orientation.Contains("Nord"))
            {
                Orientation += "or_N";
                if (appt.Orientation.Contains("Est"))
                    Orientation += "E";
                if (appt.Orientation.Contains("Ouest"))
                    Orientation += "W";
            }

            if (appt.Orientation.Contains("Sud"))
            {
                Orientation += "or_S";
                if (appt.Orientation.Contains("Est"))
                    Orientation += "E";
                if (appt.Orientation.Contains("Ouest"))
                    Orientation += "W";
            }

            if(Orientation=="" && appt.Orientation.Contains("Est"))
                Orientation += "or_E";

            if (Orientation == "" && appt.Orientation.Contains("Ouest"))
                Orientation += "or_W";
            #endregion
            var result = new List<object>();

                result.Add(
                    new
                    {
                        appt.AppartementName,
                        appt.Type,
                        Orientation
                    });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // GET (AJAX) - Récupére les informations de l'appartement
        public JsonResult GetInformationsTravauxForOthers(int paramApptId)
        {
            Appartement user = bdd_appartement.Get_AppartementFromName(User.Identity.Name);
            Appartement appt = bdd_appartement.Get_AppartementFromId(paramApptId);
            appt.listeTravaux = bdd_travaux.Get_TravauxAppt(paramApptId);
            var result = new List<object>();
            String imgName = "";
            String nom = "";
            String prix = "-";
            Travail tr;

            if (appt.IsDoubleVitrage)
            {
                tr= appt.listeTravaux.FirstOrDefault(t => t.type == EnumTravail.DoubleVitrage);
                imgName = EnumImages.DoubleVitrage;
                nom = "Double vitrage";

                if (appt.PartagerTravaux && user.PartagerTravaux)
                    prix = tr.Prix.ToString();

                result.Add(new{imgName,nom,prix,tr.Entreprise,tr.Contact });
            }

            if (appt.IsIsolationTotale && user.PartagerTravaux)
            {
                tr = appt.listeTravaux.FirstOrDefault(t => t.type == EnumTravail.IsolationTotale);
                imgName = EnumImages.IsolationTotale;
                nom = "Isolation intérieure totale";

                if (appt.PartagerTravaux && user.PartagerTravaux)
                    prix = tr.Prix.ToString();

                result.Add(new { imgName, nom, prix, tr.Entreprise, tr.Contact });
            }

            if (appt.IsIsolationPartielle)
            {
                tr = appt.listeTravaux.FirstOrDefault(t => t.type == EnumTravail.IsolationPartielle);
                imgName = EnumImages.IsolationPartielle;
                nom = "Isolation intérieure partielle";

                if (appt.PartagerTravaux && user.PartagerTravaux)
                    prix = tr.Prix.ToString();

                result.Add(new { imgName, nom, prix, tr.Entreprise, tr.Contact });
            }

            if (appt.IsRobinetsThermo)
            {
                tr = appt.listeTravaux.FirstOrDefault(t => t.type == EnumTravail.Robinets);
                imgName = EnumImages.Robinets;
                nom = "Robinets Thermostatiques";

                if (appt.PartagerTravaux && user.PartagerTravaux)
                    prix = tr.Prix.ToString();

                result.Add(new { imgName, nom, prix, tr.Entreprise, tr.Contact });
            }

            if (appt.IsValvesAuto)
            {
                tr = appt.listeTravaux.FirstOrDefault(t => t.type == EnumTravail.Valves);
                imgName = EnumImages.Valves;
                nom = "Valves automatiques";

                if (appt.PartagerTravaux && user.PartagerTravaux)
                    prix = tr.Prix.ToString();

                result.Add(new { imgName, nom, prix, tr.Entreprise, tr.Contact });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion



        #region Mon Appartement

        // GET: Appartement
        public ActionResult MonAppartement()
        {
            Appartement user = bdd_appartement.Get_AppartementFromName(User.Identity.Name);
            user.listeTravaux = bdd_travaux.Get_TravauxAppt(user.AppartementId);
            MonAppartementViewModel vm = new MonAppartementViewModel(user);

            return View(vm);
        }


        [HttpPost]
        public ActionResult MonAppartement(MonAppartementViewModel vm)
        {
            Appartement user = bdd_appartement.Get_AppartementFromName(User.Identity.Name);
            user.listeTravaux = bdd_travaux.Get_TravauxAppt(user.AppartementId);
            bool isModify = false;

            #region Infos
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

            String orientation = "";

            if (vm.IsNord) orientation += "Nord";
            if (vm.IsSud) orientation += "Sud";
            if((vm.IsNord || vm.IsSud) && (vm.IsEst || vm.IsOuest)) orientation += " ";
            if (vm.IsEst) orientation += "Est";
            if (vm.IsOuest) orientation += "Ouest";

            if(user.Orientation != orientation)
            {
                isModify = true;
                user.Orientation = orientation;
            }
            #endregion

            #region Travaux

            #region DoubleVitrage
            if (user.IsDoubleVitrage != vm.IsDoubleVitrage) {user.IsDoubleVitrage = vm.IsDoubleVitrage;isModify = true;}

            Travail vitrage = user.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.DoubleVitrage);
            bool isModifyDoubleVitrage = false;
            if (vitrage.Prix != vm.InfosVitrage.Prix) { vitrage.Prix = vm.InfosVitrage.Prix; isModifyDoubleVitrage = true; }
            if (vitrage.Entreprise != vm.InfosVitrage.Entreprise) { vitrage.Entreprise = vm.InfosVitrage.Entreprise; isModifyDoubleVitrage = true; }
            if (vitrage.Contact != vm.InfosVitrage.Contact) { vitrage.Contact = vm.InfosVitrage.Contact; isModifyDoubleVitrage = true; }

            #endregion

            #region IsolationPartielle
            if (user.IsIsolationPartielle != vm.IsIsolationPartielle){user.IsIsolationPartielle = vm.IsIsolationPartielle;isModify = true;}

            Travail IsolationPartielle = user.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.IsolationPartielle);
            bool isModifyIsolationPartielle = false;
            if (IsolationPartielle.Prix != vm.InfosIsolationPartielle.Prix) { IsolationPartielle.Prix = vm.InfosIsolationPartielle.Prix; isModifyIsolationPartielle = true; }
            if (IsolationPartielle.Entreprise != vm.InfosIsolationPartielle.Entreprise) { IsolationPartielle.Entreprise = vm.InfosIsolationPartielle.Entreprise; isModifyIsolationPartielle = true; }
            if (IsolationPartielle.Contact != vm.InfosIsolationPartielle.Contact) { IsolationPartielle.Contact = vm.InfosIsolationPartielle.Contact; isModifyIsolationPartielle = true; }

            #endregion

            #region IsolationTotale
            if (user.IsIsolationTotale != vm.IsIsolationTotale) {user.IsIsolationTotale = vm.IsIsolationTotale; isModify = true; }
            Travail IsolationTotale = user.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.IsolationTotale);
            bool isModifyIsolationTotale = false;
            if (IsolationTotale.Prix != vm.InfosIsolationTotale.Prix) { IsolationTotale.Prix = vm.InfosIsolationTotale.Prix; isModifyIsolationTotale = true; }
            if (IsolationTotale.Entreprise != vm.InfosIsolationTotale.Entreprise) { IsolationTotale.Entreprise = vm.InfosIsolationTotale.Entreprise; isModifyIsolationTotale = true; }
            if (IsolationTotale.Contact != vm.InfosIsolationTotale.Contact) { IsolationTotale.Contact = vm.InfosIsolationTotale.Contact; isModifyIsolationTotale = true; }

            #endregion

            #region RobinetsThermo
            if (user.IsRobinetsThermo != vm.IsRobinetsThermo){user.IsRobinetsThermo = vm.IsRobinetsThermo;isModify = true;}
            Travail RobinetsThermo = user.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.Robinets);
            bool isModifyRobinetsThermo = false;
            if (RobinetsThermo.Prix != vm.InfosRobinets.Prix) { RobinetsThermo.Prix = vm.InfosRobinets.Prix; isModifyRobinetsThermo = true; }
            if (RobinetsThermo.Entreprise != vm.InfosRobinets.Entreprise) { RobinetsThermo.Entreprise = vm.InfosRobinets.Entreprise; isModifyRobinetsThermo = true; }
            if (RobinetsThermo.Contact != vm.InfosRobinets.Contact) { RobinetsThermo.Contact = vm.InfosRobinets.Contact; isModifyRobinetsThermo = true; }

            #endregion

            #region ValvesAuto
            if (user.IsValvesAuto != vm.IsValvesAuto){user.IsValvesAuto = vm.IsValvesAuto;isModify = true;}
            Travail ValvesAuto = user.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.Valves);
            bool isModifyValvesAuto = false;
            if (ValvesAuto.Prix != vm.InfosValves.Prix) { ValvesAuto.Prix = vm.InfosValves.Prix; isModifyValvesAuto = true; }
            if (ValvesAuto.Entreprise != vm.InfosValves.Entreprise) { ValvesAuto.Entreprise = vm.InfosValves.Entreprise; isModifyValvesAuto = true; }
            if (ValvesAuto.Contact != vm.InfosValves.Contact) { ValvesAuto.Contact = vm.InfosValves.Contact; isModifyValvesAuto = true; }

            #endregion

            #endregion

            #region Partage
            if (user.Partager != vm.Partager)
            {
                isModify = true;
                user.Partager = vm.Partager;
            }

            if (user.PartagerTravaux != vm.PartagerTravaux)
            {
                isModify = true;
                user.PartagerTravaux = vm.PartagerTravaux;
            }
            #endregion

            #region MAJ BDD
            if (isModify)
                bdd_appartement.Update_Appartement(user);
            if (isModifyDoubleVitrage)
                bdd_travaux.Update_Travail(vitrage);
            if (isModifyIsolationPartielle)
                bdd_travaux.Update_Travail(IsolationPartielle);
            if (isModifyIsolationTotale)
                bdd_travaux.Update_Travail(IsolationTotale);
            if (isModifyRobinetsThermo)
                bdd_travaux.Update_Travail(RobinetsThermo);
            if (isModifyValvesAuto)
                bdd_travaux.Update_Travail(ValvesAuto);


            #endregion

            return RedirectToAction("MonAppartement");
        }


        #endregion
    }
}