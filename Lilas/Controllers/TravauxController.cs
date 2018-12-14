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
            ListeDesTravauxViewModel vm;
            string trie = (string)TempData["ColonneTrie"];

            if(trie == null)
            {
                vm = this.majVMtravaux();
                TempData["ViewModel_ListeDesTravaux"] = vm;
            }
            else
            {
                vm = (ListeDesTravauxViewModel)TempData["ViewModel_ListeDesTravaux"];
                TempData["ViewModel_ListeDesTravaux"] = vm;
            }
                

            #region trie

            switch(trie)
            {
                case "type":
                    vm.listeTravaux = vm.listeTravaux.OrderBy(a => a.type).ToList();
                    break;
                case "taille":
                    vm.listeTravaux = vm.listeTravaux.OrderBy(a => a.TypeAppartement).ToList();
                    break;
                case "appt":
                    vm.listeTravaux = vm.listeTravaux.OrderBy(a => a._Appt_AppartementName).ToList();
                    break;
                case "prix":
                    vm.listeTravaux = vm.listeTravaux.OrderBy(a => a.Prix).ToList();
                    break;
                case "nom":
                    vm.listeTravaux = vm.listeTravaux.OrderBy(a => a.Entreprise).ToList();
                    break;
            }


            #endregion

            return View(vm);
        }


        private ListeDesTravauxViewModel majVMtravaux()
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
                switch (tr.type)
                {
                    case EnumTravail.IsolationTotale: tr.type = EnumImages.IsolationTotale; break;
                    case EnumTravail.IsolationPartielle: tr.type = EnumImages.IsolationPartielle; break;
                    case EnumTravail.DoubleVitrage: tr.type = EnumImages.DoubleVitrage; break;
                    case EnumTravail.Robinets: tr.type = EnumImages.Robinets; break;
                    case EnumTravail.Valves: tr.type = EnumImages.Valves; break;
                }
            }


            return vm;
        }


        [HttpPost]
        public ActionResult ListeDesTravaux(ListeDesTravauxViewModel vm)
        {
            TempData["ColonneTrie"] = vm.Trie;

            return RedirectToAction("ListeDesTravaux");
        }


    }
}