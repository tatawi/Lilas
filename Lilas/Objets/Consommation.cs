using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lilas.Objets
{
    public class Consommation
    {
        public Consommation()
        {

        }


        public int ConsommationId { get; set; }
        public DateTime Date { get; set; }
        public int AppartementId { get; set; }
        public int Cuisine { get; set; }
        public int Salon { get; set; }
        public int Chambre_Salon { get; set; }
        public int Chambre1 { get; set; }
        public int Chambre2 { get; set; }
        public int Chambre3 { get; set; }
        public int Sdb { get; set; }

        public Consommation ShallowCopy()
        {
            return (Consommation)this.MemberwiseClone();
        }
        public int calculerTotal()
        {
            return this.Cuisine + this.Salon + this.Chambre_Salon + this.Chambre1 + this.Chambre2 + this.Chambre3 + this.Sdb;
        }

        public bool isConsoValide()
        {
            return (this.Cuisine>0 || this.Salon > 0 || this.Chambre_Salon > 0 || this.Chambre1 > 0 || this.Chambre2 > 0 || this.Chambre3 > 0 || this.Sdb > 0);
        }


    }
}