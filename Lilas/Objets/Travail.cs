﻿using System;
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
        public int TravailId { get; set; }
        public string type { get; set; }
        public int AppartementId { get; set; }
        public int Prix { get; set; }
        public string Entreprise { get; set; }
        public string Contact { get; set; }
        public string TypeAppartement { get; set; }
    


    }
}