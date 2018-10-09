using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lilas.Objets
{
    public class Appartement
    {

        public Appartement()
        {
            listeTravaux = new List<Travail>();
        }


        public int AppartementId { get; set; }
        public string UserName { get; set; }
        public string AppartementName { get; set; }
        public string Batiment { get; set; }
        public string Escalier { get; set; }
        public int Etage { get; set; }
        public string Porte { get; set; }
        public string Type { get; set; }
        public bool IsDoubleVitrage { get; set; }
        public bool IsRobinetsThermo { get; set; }
        public bool IsIsolationPartielle { get; set; }
        public bool IsIsolationTotale { get; set; }
        public bool IsValvesAuto { get; set; }
        public string Orientation { get; set; }
        public bool Partager { get; set; }
        public bool PartagerTravaux { get; set; }

        public bool IsT2()
        {
            return this.Type.Equals("T2");
        }

        public bool IsT3()
        {
            return this.Type.Equals("T3");
        }

        public bool IsT4()
        {
            return this.Type.Equals("T4");
        }

        public List<Travail>listeTravaux { get; set; }
    }
}