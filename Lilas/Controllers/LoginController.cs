using Lilas.Bdd;
using Lilas.Models;
using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Lilas.Controllers
{
    public class LoginController : Controller
    {
        Bdd_Appartement bdd_appartement;


        // GET: Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            LoginViewModel viewModel = new LoginViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };

            if (viewModel.Authentifie)
            {
                viewModel.Utilisateur = HttpContext.User.Identity.Name;
            }
            if(TempData["messageConnexion"]!=null)
            {
                viewModel.isErreur = true;
            }


            return View(viewModel);
        }



        [HttpPost]
        public ActionResult Index(LoginViewModel vm)
        {
            bdd_appartement = new Bdd_Appartement();
            Appartement appt = new Appartement();
            appt.AppartementName = vm.Appart;
            appt.UserName = vm.Utilisateur;

            if(bdd_appartement.IsAppartementNamePresent(appt.AppartementName))
            {
                Appartement bddUser = bdd_appartement.Get_AppartementFromName(appt.AppartementName);
                TempData["messageConnexion"] = null;
                if (bddUser.UserName.Equals(appt.UserName))
                {
                    FormsAuthentication.SetAuthCookie(vm.Appart, false);
                }
            }
            else
            {
                TempData["messageConnexion"] = "Impossible de vous authentifier avec ce couplke Appartement / Nom";
            }

            return Redirect(@Url.Content("~/"));

        }



        public void CreerCompteAjax(string paramUserName, string paramAppartement, string paramBatiment, string paramEscalier, int paramEtage, string paramPorte, string paramType, bool paramIsDoubleVitrage, bool paramIsRobinetsThermo, string paramOrientation)
        {
            try
            {
                bdd_appartement = new Bdd_Appartement();


                Appartement appt = new Appartement();
                appt.AppartementName = paramAppartement;
                appt.Batiment = paramBatiment;
                appt.Escalier = paramEscalier;
                appt.Etage = paramEtage;
                appt.IsDoubleVitrage = paramIsDoubleVitrage;
                appt.IsRobinetsThermo = paramIsRobinetsThermo;
                appt.Orientation = paramOrientation;
                appt.Porte = paramPorte;
                appt.Type = paramType;
                appt.UserName = paramUserName;

                if(!bdd_appartement.IsAppartementNamePresent(paramAppartement))
                {
                    bdd_appartement.Add_Appartement(appt);
                    TempData["message"] = "Compte créé";
                }
                else
                {
                    TempData["message"] = "NOKCet appartement a déjà été créé";
                } 
            }
            catch (Exception ex)
            {
                TempData["message"] = "NOKErreur lors de le création: " + ex.Message;
            }
        }



        public JsonResult Get_message()
        {
            string message = "Pas d'informations";
            if(TempData["message"]!=null)
            {
                message = TempData["message"].ToString();
            }


            var result = new List<object>();
            result.Add(new { message });

            return Json(result, JsonRequestBehavior.AllowGet);
        }




        public ActionResult Deconnexion()
        {
            FormsAuthentication.SignOut();
            return Redirect(@Url.Content("~/"));
        }
    }
}