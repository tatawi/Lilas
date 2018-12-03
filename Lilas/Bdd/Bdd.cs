using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lilas.Bdd
{
    public abstract class Bdd
    {
        protected string connectionString;
        public Bdd()
        {
            this.connectionString = "Data Source=LT-SOD00005718;Initial Catalog=Lilas;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        }
    }
}