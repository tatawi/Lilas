using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lilas.Models
{
    public class ListeDesTravauxViewModel
    {

        public ListeDesTravauxViewModel()
        {


        }

        public int AppartementId { get; set; }

        public bool PartageTravaux { get; set; }

        public List<Travail> listeTravaux{ get; set; }

        public int NbIsolationPartielle { get; set; }
        public int NbIsolationTotale { get; set; }
        public int NbDoubleVitrage { get; set; }
        public int NbRobinets { get; set; }
        public int NbValesAuto { get; set; }
        public string Trie { get; set; }









    }
}