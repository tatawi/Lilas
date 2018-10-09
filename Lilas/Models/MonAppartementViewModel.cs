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
            this.IsIsolationPartielle = appt.IsIsolationPartielle;
            this.IsIsolationTotale = appt.IsIsolationTotale;
            this.IsValvesAuto = appt.IsValvesAuto;
            this.Partager = appt.Partager;
            this.PartagerTravaux = appt.PartagerTravaux;

            this.InfosVitrage = new Travail();
            this.InfosIsolationPartielle = new Travail();
            this.InfosIsolationTotale = new Travail();
            this.InfosRobinets = new Travail();
            this.InfosValves = new Travail();


            if (appt.Orientation.Contains("Nord")) this.IsNord = true;
            if (appt.Orientation.Contains("Sud")) this.IsSud = true;
            if (appt.Orientation.Contains("Est")) this.IsEst = true;
            if (appt.Orientation.Contains("Ouest")) this.IsOuest = true;

            if (this.Batiment.Equals("A"))
                this.IsBatA = true;
            else
                this.IsBatA = false;

            if (this.Escalier.Equals("A"))
                this.IsEscalierA=true;
            else
                this.IsEscalierA = false;

            if(appt.listeTravaux.Count>0)
            {
                if(appt.listeTravaux.Any(tr=>tr.type==EnumTravail.DoubleVitrage))
                    this.InfosVitrage = appt.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.DoubleVitrage);

                if (appt.listeTravaux.Any(tr => tr.type == EnumTravail.IsolationPartielle))
                    this.InfosIsolationPartielle = appt.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.IsolationPartielle);

                if (appt.listeTravaux.Any(tr => tr.type == EnumTravail.IsolationTotale))
                    this.InfosIsolationTotale = appt.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.IsolationTotale);

                if (appt.listeTravaux.Any(tr => tr.type == EnumTravail.Robinets))
                    this.InfosRobinets = appt.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.Robinets);

                if (appt.listeTravaux.Any(tr => tr.type == EnumTravail.Valves))
                    this.InfosValves = appt.listeTravaux.FirstOrDefault(tr => tr.type == EnumTravail.Valves);
            }

            

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
        public bool IsIsolationPartielle { get; set; }
        public bool IsIsolationTotale { get; set; }
        public bool IsValvesAuto { get; set; }
        public Travail InfosVitrage { get; set; }
        public Travail InfosIsolationTotale { get; set; }
        public Travail InfosIsolationPartielle{ get; set; }
        public Travail InfosRobinets { get; set; }
        public Travail InfosValves { get; set; }
        public bool IsNord { get; set; }
        public bool IsSud { get; set; }
        public bool IsEst { get; set; }
        public bool IsOuest { get; set; }

        public bool Partager { get; set; }
        public bool PartagerTravaux { get; set; }




    }
}