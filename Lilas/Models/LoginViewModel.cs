using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lilas.Models
{
    public class LoginViewModel
    {

        public string Utilisateur { get; set; }
        public string mdp { get; set; }
        public string Appart { get; set; }

        public bool Authentifie { get; set; }

        public bool isErreur { get; set; }

    }
}