using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lilas.Models
{
    public class AppartementsViewModel
    {

        public List<Appartement> list_appts { get; set; }

        public int nbAppts { get; set; }

        public int nbApptsTotal { get; set; }

        public int nbPartage { get; set; }

        public int Type { get; set; }
        
        public bool IsDoubleVitrage { get; set; }
        public bool IsRobinetsThermo { get; set; }

        public bool JePartage { get; set; }
    }
}