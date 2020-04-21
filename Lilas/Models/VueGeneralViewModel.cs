using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lilas.Models
{
    public class VueGeneralViewModel
    {


        public VueGeneralViewModel()
        {
            listeTravauxAfaire = new List<Travail>();
            IsMeilleurDansCategorie = false;
            IsDonneesAjour = false;
        }

        public int Annee { get; set; }
        public int AppartementId { get; set; }

        public String Type { get; set; }

        public bool IsMeilleurDansCategorie { get; set; }

        public bool IsDonneesAjour{ get; set; }

        public String pctEconomiesChauffage { get; set; }

        public int MontantAEngagerPour { get; set; }

        public List<Travail>listeTravauxAfaire { get; set; }
    }
}