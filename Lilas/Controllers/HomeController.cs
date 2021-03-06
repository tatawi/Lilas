﻿using Lilas.Bdd;
using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lilas.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        Bdd_Appartement bdd_appartement;
        Bdd_Consommation bdd_conso;


        public HomeController()
        {
            this.bdd_appartement = new Bdd_Appartement();
            this.bdd_conso = new Bdd_Consommation();
        }


        public JsonResult Get_message()
        {
            string message = "Pas d'informations";
            if (TempData["message"] != null)
            {
                message = TempData["message"].ToString();
            }


            var result = new List<object>();
            result.Add(new { message });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region Vue Générale
        public ActionResult Index()
        {
            Appartement user = bdd_appartement.Get_AppartementFromName(User.Identity.Name);
            ViewBag.ApptId = user.AppartementId;
            ViewBag.Type = user.Type;

            return View();
        }

        //GET - consommations par piéces moyennes pour l'année
        public JsonResult GetConsommationsMoyennes(int paramAnnee, int paramApptId)
        {
            Consommation maConso = bdd_conso.get_ConsommationForYearAndAppt(paramAnnee, paramApptId);
            Consommation consoMoyenne = this.CalculerListeConsoMoyenneAnnee(paramAnnee);
            Consommation consoMoyenneType = (Consommation)TempData["consoMoyenneType"];
            Consommation consoFaibleType = (Consommation)TempData["consoMoyenneType"];


            var result = new List<object>();
            String legend = "Cuisine";
            int perso = maConso.Cuisine;
            int moyenne = consoMoyenne.Cuisine;
            int moyType = consoMoyenneType.Cuisine;
            result.Add(new {legend, perso, moyenne, moyType });

            legend = "Salon"; perso = maConso.Salon; moyenne = consoMoyenne.Salon;  moyType = consoMoyenneType.Salon;
            result.Add(new { legend, perso, moyenne, moyType });

            legend = "Salon / Chambre"; perso = maConso.Chambre_Salon; moyenne = consoMoyenne.Chambre_Salon; moyType = consoMoyenneType.Chambre_Salon;
            result.Add(new { legend, perso, moyenne, moyType });

            legend = "Chambre 1"; perso = maConso.Chambre1; moyenne = consoMoyenne.Chambre1; moyType = consoMoyenneType.Chambre1;
            result.Add(new { legend, perso, moyenne, moyType });

            legend = "Chambre 2"; perso = maConso.Chambre2; moyenne = consoMoyenne.Chambre2; moyType = consoMoyenneType.Chambre2;
            result.Add(new { legend, perso, moyenne, moyType });

            legend = "Chambre 3"; perso = maConso.Chambre3; moyenne = consoMoyenne.Chambre3; moyType = consoMoyenneType.Chambre3;
            result.Add(new { legend, perso, moyenne, moyType });

            legend = "Sdb"; perso = maConso.Sdb; moyenne = consoMoyenne.Sdb; moyType = consoMoyenneType.Sdb;
            result.Add(new { legend, perso, moyenne, moyType });


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //GET - consommations moyennes pour toutes les années
        public JsonResult GetConsommationsMoyennesTotal(int paramApptId)
        {
            int currentYear = DateTime.Now.Year;
            int minYear = bdd_conso.get_MinimumYear(paramApptId); 
            var result = new List<object>();

            for (int annee = minYear; annee <= currentYear; annee++)
            {
                Consommation maConso = bdd_conso.get_ConsommationForYearAndAppt(annee, paramApptId);
                Consommation consoMoyenne = CalculerListeConsoMoyenneAnnee(annee);
                Consommation consoType = (Consommation)TempData["consoMoyenneType"];

                int maConsoTotale = maConso.calculerTotal();
                int consoMoyTotale = consoMoyenne.calculerTotal();
                int consoTypeTotale = consoType.calculerTotal();
                result.Add(new { annee, maConsoTotale, consoMoyTotale, consoTypeTotale });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }




        #endregion



        #region Ma consommation

        public ActionResult MaConsommation()
        {
            Appartement user= bdd_appartement.Get_AppartementFromName(User.Identity.Name);
            ViewBag.ApptId = user.AppartementId;
            ViewBag.Type = user.Type;

            return View();
        }


        // GET (AJAX) - Chargement du tableau de consommation de chauffage de l'année
        public JsonResult GetListConsoChauffageAnnee(int paramAnnee, int paramApptId)
        {
            List<Consommation> list_Conso = this.CalculerListeConsoPersoAnnee(paramAnnee, paramApptId);

            var result = new List<object>();
            foreach (Consommation info in list_Conso)
            {
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string Mois = mfi.GetMonthName(info.Date.Month).ToString();

                result.Add(
                    new
                    {
                        Mois,
                        info.Cuisine,
                        info.Salon,
                        info.Chambre_Salon,
                        info.Chambre1,
                        info.Chambre2,
                        info.Chambre3,
                        info.Sdb
                    });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // GET (AJAX) - Chargement du tableau de consommation de chauffage de l'année
        public JsonResult GetListConsoChauffagePieAnnee(int paramAnnee, int paramApptId)
        {

            Consommation conso  = bdd_conso.get_ConsommationForYearAndAppt(paramAnnee, paramApptId);


            var result = new List<object>();

            result.Add(
                new
                {
                    conso.Cuisine,
                    conso.Salon,
                    conso.Chambre_Salon,
                    conso.Chambre1,
                    conso.Chambre2,
                    conso.Chambre3,
                    conso.Sdb
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // GET (AJAX) - Chargement du tableau de consommation de chauffage de l'année
        public JsonResult GetListConsoChauffageTotal(int paramApptId)
        {
            List<Consommation> list_Recap = new List<Consommation>();
            

            int currentYear = DateTime.Now.Year;
            int minYear = bdd_conso.get_MinimumYear(paramApptId);

            for (int annee = minYear; annee <= currentYear; annee++)
            {
                Consommation conso = bdd_conso.get_ConsommationForYearAndAppt(annee, paramApptId);
                list_Recap.Add(conso);
            }

            var result = new List<object>();
            foreach (Consommation info in list_Recap)
            {
                int Annee = info.Date.Year;

                result.Add(
                    new
                    {
                        Annee,
                        info.Cuisine,
                        info.Salon,
                        info.Chambre_Salon,
                        info.Chambre1,
                        info.Chambre2,
                        info.Chambre3,
                        info.Sdb
                    });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        //GET (AJAX) - Liste Sous-catégories
        public JsonResult GetListRelevesAnnee(int paramAnnee, int paramApptId)
        {
            List<Consommation> listReleves = this.bdd_conso.get_ListConsommationForYearAndAppt(paramAnnee, paramApptId).OrderBy(d=>d.Date).ToList();

            var result = new List<object>();
            foreach (Consommation conso in listReleves)
            {
                String MaDate = conso.Date.ToShortDateString();
                result.Add(
                    new
                    {
                        MaDate,
                        conso.Cuisine,
                        conso.Salon,
                        conso.Chambre_Salon,
                        conso.Chambre1,
                        conso.Chambre2,
                        conso.Chambre3,
                        conso.Sdb,
                        conso.ConsommationId
                    });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        

        //Ajout d'une ligne de relevé
        public void AjouteReleve()
        {
            try
            {
                int appId = Convert.ToInt32(Request.Params["paramApptId"]);
                DateTime dateStr = Convert.ToDateTime(Request.Params["paramDate"]);
                int ConsoCuisine = Convert.ToInt16(Request.Params["paramConsoCuisine"]);
                int ConsoSalon = Convert.ToInt16(Request.Params["paramConsoSalon"]);
                int ConsoSalonCh = Convert.ToInt16(Request.Params["paramConsoSalonCh"]);
                int ConsoChambre1 = Convert.ToInt16(Request.Params["paramConsoChambre1"]);
                int ConsoChambre2 = Convert.ToInt16(Request.Params["paramConsoChambre2"]);
                int ConsoChambre3 = Convert.ToInt16(Request.Params["paramConsoChambre3"]);
                int ConsoSdb = Convert.ToInt16(Request.Params["paramConsoSdb"]);

                Consommation conso = new Consommation();
                conso.AppartementId = appId;
                conso.Date = dateStr;
                conso.Cuisine = ConsoCuisine;
                conso.Salon = ConsoSalon;
                conso.Chambre_Salon = ConsoSalonCh;
                conso.Chambre1 = ConsoChambre1;
                conso.Chambre2 = ConsoChambre2;
                conso.Chambre3 = ConsoChambre3;
                conso.Sdb = ConsoSdb;

                this.bdd_conso.Add_ConsoChauffage(conso);

                TempData["message"] = "Consommation ajoutée";
            }
            catch (Exception ex)
            {
                TempData["message"] = "Erreur lors de l'ajout : " + ex.Message;
            }
        }


        //POST (AJAX)Supprimer une dépense
        public void DeleteReleve()
        {
            int paramId = Convert.ToInt32(Request.Params["paramId"]);
            this.bdd_conso.Del_Conso(paramId);
        }



        #endregion


        #region Fonctions

        private List<Consommation> CalculerListeConsoPersoAnnee(int paramAnnee, int paramApptId)
        {
            List<Consommation> list_Conso = new List<Consommation>();
            List<Consommation> list_Date = bdd_conso.get_ListConsommationForYearAndAppt(paramAnnee, paramApptId);
            Consommation consoPrecedente = new Consommation(); ;
            list_Date = list_Date.OrderBy(v => v.Date).ToList();

            for (int mois = 1; mois <= 12; mois++)
            {
                Consommation consoDate = list_Date.Where(d => d.Date.Month == mois).OrderByDescending(d => d.Date).FirstOrDefault();

                if (consoDate != null)
                {
                    Consommation conso = consoDate.ShallowCopy();
                    if (mois != 1)
                    {
                        conso.Cuisine -= consoPrecedente.Cuisine;
                        conso.Salon -= consoPrecedente.Salon;
                        conso.Chambre_Salon -= consoPrecedente.Chambre_Salon;
                        conso.Chambre1 -= consoPrecedente.Chambre1;
                        conso.Chambre2 -= consoPrecedente.Chambre2;
                        conso.Chambre3 -= consoPrecedente.Chambre3;
                        conso.Sdb -= consoPrecedente.Sdb;
                    }

                    consoPrecedente = list_Date.Where(d => d.Date.Month == mois).OrderByDescending(d => d.Date).FirstOrDefault();
                    list_Conso.Add(conso);
                }
            }

            return list_Conso;
        }


        private Consommation CalculerListeConsoMoyenneAnnee(int paramAnnee)
        {
            List<Appartement> liste_Appt = bdd_appartement.Get_AllAppartements();
            List<Consommation> list_Conso = new List<Consommation>();
            List<Consommation> list_ConsoType = new List<Consommation>();
            Consommation consoMoyenne = new Consommation();
            Consommation consoMoyenneType = new Consommation();

            string type = bdd_appartement.Get_AppartementFromName(User.Identity.Name).Type;

            foreach (Appartement app in liste_Appt)
            {
                Consommation conso_appt = bdd_conso.get_ConsommationForYearAndAppt(paramAnnee, app.AppartementId);
                list_Conso.Add(conso_appt);

                if (type == app.Type)
                {
                    list_ConsoType.Add(conso_appt);
                }
            }

            //Consos moyenne
            int nbconsos = list_Conso.Count();
            if (nbconsos == 0) nbconsos = 1;
            consoMoyenne.Cuisine = list_Conso.Sum(m => m.Cuisine) / nbconsos;
            consoMoyenne.Salon = list_Conso.Sum(m => m.Salon) / nbconsos;
            consoMoyenne.Chambre_Salon = list_Conso.Sum(m => m.Chambre_Salon) / nbconsos;
            consoMoyenne.Chambre1 = list_Conso.Sum(m => m.Chambre1) / nbconsos;
            consoMoyenne.Chambre2 = list_Conso.Sum(m => m.Chambre2) / nbconsos;
            consoMoyenne.Chambre3 = list_Conso.Sum(m => m.Chambre3) / nbconsos;
            consoMoyenne.Sdb = list_Conso.Sum(m => m.Sdb) / nbconsos;

            //Conso moyenne type
            int nbconsosType = list_ConsoType.Count();
            if (nbconsosType == 0) nbconsosType = 1;
            consoMoyenneType.Cuisine = list_ConsoType.Sum(m => m.Cuisine) / nbconsosType;
            consoMoyenneType.Salon = list_ConsoType.Sum(m => m.Salon) / nbconsosType;
            consoMoyenneType.Chambre_Salon = list_ConsoType.Sum(m => m.Chambre_Salon) / nbconsosType;
            consoMoyenneType.Chambre1 = list_ConsoType.Sum(m => m.Chambre1) / nbconsosType;
            consoMoyenneType.Chambre2 = list_ConsoType.Sum(m => m.Chambre2) / nbconsosType;
            consoMoyenneType.Chambre3 = list_ConsoType.Sum(m => m.Chambre3) / nbconsosType;
            consoMoyenneType.Sdb = list_ConsoType.Sum(m => m.Sdb) / nbconsosType;


            TempData["consoMoyenneType"] = consoMoyenneType;
            return consoMoyenne;
        }


        //private Consommation majConso(Consommation toUpdate, Consommation forUpdate)
        //{
        //    if (toUpdate.AppartementId == 0)
        //    {
        //        toUpdate = forUpdate.ShallowCopy();
        //    }
        //    else
        //    {
        //        toUpdate.Cuisine = (toUpdate.Cuisine + forUpdate.Cuisine) / 2;
        //        toUpdate.Salon = (toUpdate.Salon + forUpdate.Salon) / 2;
        //        toUpdate.Chambre_Salon = (toUpdate.Chambre_Salon + forUpdate.Chambre_Salon) / 2;
        //        toUpdate.Chambre1 = (toUpdate.Chambre1 + forUpdate.Chambre1) / 2;
        //        toUpdate.Chambre2 = (toUpdate.Chambre2 + forUpdate.Chambre2) / 2;
        //        toUpdate.Chambre3 = (toUpdate.Chambre3 + forUpdate.Chambre3) / 2;
        //        toUpdate.Sdb = (toUpdate.Sdb + forUpdate.Sdb) / 2;
        //    }

        //    return toUpdate;
        //}
        #endregion


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}