using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lilas.Models
{
    public class MonAppartementViewModel
    {

        public MonAppartementViewModel()
        {

        }


        public MonAppartementViewModel(Appartement appt)
        {
            this.AppartementId = appt.AppartementId;
            this.UserName = appt.UserName;
            this.Appartement = appt.AppartementName;
            this.Batiment = appt.Batiment;
            this.Escalier = appt.Escalier;
            this.Etage = appt.Etage;
            this.Porte = appt.Porte;
            this.Type = appt.Type;
            this.IsDoubleVitrage = appt.IsDoubleVitrage;
            this.IsRobinetsThermo = appt.IsRobinetsThermo;
            this.Partager = appt.Partager;

            if (appt.Orientation.Contains("Nord")) this.OrientationNS = "Nord";
            if (appt.Orientation.Contains("Sud")) this.OrientationNS = "Sud";
            if (appt.Orientation.Contains("Est")) this.OrientationEO = "Est";
            if (appt.Orientation.Contains("Ouest")) this.OrientationEO = "Ouest";

            if (this.Batiment.Equals("A"))
                this.IsBatA = true;
            else
                this.IsBatA = false;

            if (this.Escalier.Equals("A"))
                this.IsEscalierA=true;
            else
                this.IsEscalierA = false;

            

        }



        public int AppartementId { get; set; }
        public string UserName { get; set; }
        public string Appartement { get; set; }
        public string Batiment { get; set; }
        public bool IsBatA { get; set; }
        public bool IsEscalierA { get; set; }
        public string Escalier { get; set; }
        public int Etage { get; set; }
        public string Porte { get; set; }
        public string Type { get; set; }
        public bool IsDoubleVitrage { get; set; }
        public bool IsRobinetsThermo { get; set; }
        public string OrientationNS { get; set; }
        public string OrientationEO { get; set; }

        public bool Partager { get; set; }




    }
}