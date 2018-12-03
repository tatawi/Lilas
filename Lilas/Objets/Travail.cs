using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lilas.Objets
{
    public class Travail
    {
        public Travail()
        {

        }
        public Travail(int apptId, string type, string typeAppt)
        {
            this.AppartementId = apptId;
            this.type = type;
            this.Prix = 0;
            this.Entreprise = "";
            this.Contact = "";
            this.TypeAppartement = typeAppt;
            this._Appt_PartageTravaux = false;
            this._Appt_AppartementName = "";
        }

        public int TravailId { get; set; }
        public string type { get; set; }
        public int AppartementId { get; set; }
        public int Prix { get; set; }
        public string Entreprise { get; set; }
        public string Contact { get; set; }
        public string TypeAppartement { get; set; }

        public bool _Appt_PartageTravaux { get; set; }
        public string _Appt_AppartementName { get; set; }


    }
}