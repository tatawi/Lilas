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
    public class TravauxController : Controller
    {

        Bdd_Appartement bdd_appartement;
        Bdd_Consommation bdd_conso;
        Bdd_Travaux bdd_travaux;


        public TravauxController()
        {
            this.bdd_appartement = new Bdd_Appartement();
            this.bdd_conso = new Bdd_Consommation();
            this.bdd_travaux = new Bdd_Travaux();
        }

        // GET: Travaux
        public ActionResult ListeDesTravaux()
        {
            ListeDesTravauxViewModel vm = new ListeDesTravauxViewModel();
            Appartement user = bdd_appartement.Get_AppartementFromName(User.Identity.Name);

            vm.listeTravaux = bdd_travaux.Get_AllTravauxIncludedAppartements();
            vm.AppartementId = user.AppartementId;
            vm.PartageTravaux = user.PartagerTravaux;
            vm.NbDoubleVitrage = vm.listeTravaux.Count(s => s.type == EnumTravail.DoubleVitrage);
            vm.NbIsolationPartielle = vm.listeTravaux.Count(s => s.type == EnumTravail.IsolationPartielle);
            vm.NbIsolationTotale = vm.listeTravaux.Count(s => s.type == EnumTravail.IsolationTotale);
            vm.NbRobinets = vm.listeTravaux.Count(s => s.type == EnumTravail.Robinets);
            vm.NbValesAuto = vm.listeTravaux.Count(s => s.type == EnumTravail.Valves);

            foreach (var tr in vm.listeTravaux)
            {
                switch(tr.type)
                {
                    case EnumTravail.IsolationTotale: tr.type = EnumImages.IsolationTotale; break;
                    case EnumTravail.IsolationPartielle: tr.type = EnumImages.IsolationPartielle; break;
                    case EnumTravail.DoubleVitrage: tr.type = EnumImages.DoubleVitrage; break;
                    case EnumTravail.Robinets: tr.type = EnumImages.Robinets; break;
                    case EnumTravail.Valves: tr.type = EnumImages.Valves; break;
                }

            }

            return View(vm);
        }
    }
}