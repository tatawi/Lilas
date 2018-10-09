using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Lilas.Bdd
{
    public class Bdd_Consommation : Bdd
    {

        public Bdd_Consommation()
        {

        }

        //------------------------------------------------------------------------------------------------------------
        // GET APPARTEMENT
        //------------------------------------------------------------------------------------------------------------


        //GET - Liste Consommations - par année
        public List<Consommation> get_ListConsommationForYear(int annee)
        {
            List<Consommation> list_infos = new List<Consommation>();
            string sql = "SELECT Conso_Id, Date, AppartementId, Cuisine, Salon, Chambre_Salon, Chambre1, Chambre2, Chambre3, Sdb FROM Consommation WHERE Year(Date) = @paramDate order by Date";

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("paramDate", annee);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Consommation info = new Consommation();

                            info.ConsommationId = reader.GetInt32(0);
                            info.Date = reader.GetDateTime(1);
                            info.AppartementId = reader.GetInt32(2);
                            info.Cuisine = reader.GetInt32(3);
                            info.Salon = reader.GetInt32(4);
                            info.Chambre_Salon = reader.GetInt32(5);
                            info.Chambre1 = reader.GetInt32(6);
                            info.Chambre2 = reader.GetInt32(7);
                            info.Chambre3 = reader.GetInt32(8);
                            info.Sdb = reader.GetInt32(9);

                            list_infos.Add(info);
                        }
                    }
                }
                conn.Close();
            }
            return list_infos;
        }



        //GET - Liste Consommations - par année et par appartement
        public List<Consommation> get_ListConsommationForYearAndAppt(int annee, int apptId)
        {
            List<Consommation> list_infos = new List<Consommation>();
            string sql = "SELECT Conso_Id, Date, AppartementId, Cuisine, Salon, Chambre_Salon, Chambre1, Chambre2, Chambre3, Sdb FROM Consommation WHERE Year(Date) = @paramDate AND AppartementId = @paramApptId order by Date";

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("paramDate", annee);
                    cmd.Parameters.AddWithValue("paramApptId", apptId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Consommation info = new Consommation();

                            info.ConsommationId = reader.GetInt32(0);
                            info.Date = reader.GetDateTime(1);
                            info.AppartementId = reader.GetInt32(2);
                            info.Cuisine = reader.GetInt32(3);
                            info.Salon = reader.GetInt32(4);
                            info.Chambre_Salon = reader.GetInt32(5);
                            info.Chambre1 = reader.GetInt32(6);
                            info.Chambre2 = reader.GetInt32(7);
                            info.Chambre3 = reader.GetInt32(8);
                            info.Sdb = reader.GetInt32(9);

                            list_infos.Add(info);
                        }
                    }
                }
                conn.Close();
            }
            return list_infos;
        }


        //Get - Conso courante/max - par année et par appartement
        public Consommation get_ConsommationForYearAndAppt(int annee, int apptId)
        {
            Consommation maConso = new Consommation();
            List<Consommation> listConsos = get_ListConsommationForYearAndAppt(annee, apptId);

            if(listConsos.Count>0)
            {
                DateTime maxDate = listConsos.Max(d => d.Date);
                maConso = listConsos.FirstOrDefault(v => v.Date == maxDate);
            }
            
            return maConso;
        }


        public bool IsConsoAjour(int apptId)
        {
            bool isAjour = true;

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT Count(*) FROM Consommation WHERE AppartementId=@paramApptId and Year(Date)=@paramYear and Month(Date)=@paramMonth", conn))
                {
                    cmd.Parameters.AddWithValue("paramApptId", apptId);
                    cmd.Parameters.AddWithValue("paramYear", DateTime.Now.Year);
                    cmd.Parameters.AddWithValue("paramMonth", DateTime.Now.Month);

                    Int32 count = (Int32)cmd.ExecuteScalar();
                    if (count == 0) isAjour = false;
                }
                conn.Close();
            }

            return isAjour;
        }



        public Consommation Get_MinConsoForType(string type)
        {
            Consommation conso = new Consommation();

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("select top 1 min([Cuisine] + [Salon]+[Chambre_Salon]+[Chambre1]+[Chambre2]+[Chambre3]) as TotalConso, conso.AppartementId from Consommation conso   join Appartement appt on appt.AppartementId = conso.AppartementId where Year(Date)=@paramYear and Month(Date)=@paramMonth and appt.Type=@paramType group by conso.AppartementId order by TotalConso", conn))
                {
                    cmd.Parameters.AddWithValue("paramYear", DateTime.Now.Year);
                    cmd.Parameters.AddWithValue("paramMonth", DateTime.Now.Month);
                    cmd.Parameters.AddWithValue("paramType", type);

                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            conso.Salon = reader.GetInt32(0);
                            conso.AppartementId = reader.GetInt32(1);
                            
                        }
                    }
                }
                conn.Close();
            }
            return conso;
        }



        //GET - Liste Consommations - par année et par appartement
        public int get_MinimumYear(int apptId)
        {
            string sql = "SELECT min(Year(Date)) FROM Consommation WHERE AppartementId = @paramApptId";
            int anneeMinimale = DateTime.Now.Year;

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("paramApptId", apptId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            anneeMinimale = reader.GetInt32(0);
                        }
                    }
                }
                conn.Close();
            }
            return anneeMinimale;
        }




        //------------------------------------------------------------------------------------------------------------
        // INSERT
        //------------------------------------------------------------------------------------------------------------
        public void Add_ConsoChauffage(Consommation info)
        {

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Consommation (Date, AppartementId, Cuisine, Salon, Chambre_Salon, Chambre1, Chambre2, Chambre3, Sdb) VALUES(@Date, @AppartementId, @ConsoCuisine, @ConsoSalon, @ConsoChambre_Salon, @ConsoChambre1, @ConsoChambre2, @ConsoChambre3, @ConsoSdb)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = info.Date;
                cmd.Parameters.Add("@AppartementId", SqlDbType.Decimal).Value = info.AppartementId;
                cmd.Parameters.Add("@ConsoCuisine", SqlDbType.Int).Value = info.Cuisine;
                cmd.Parameters.Add("@ConsoSalon", SqlDbType.Int).Value = info.Salon;
                cmd.Parameters.Add("@ConsoChambre_Salon", SqlDbType.Int).Value = info.Chambre_Salon;
                cmd.Parameters.Add("@ConsoChambre1", SqlDbType.Int).Value = info.Chambre1;
                cmd.Parameters.Add("@ConsoChambre2", SqlDbType.Int).Value = info.Chambre2;
                cmd.Parameters.Add("@ConsoChambre3", SqlDbType.Int).Value = info.Chambre3;
                cmd.Parameters.Add("@ConsoSdb", SqlDbType.Int).Value = info.Sdb;

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }


        //------------------------------------------------------------------------------------------------------------
        // DELETE
        //------------------------------------------------------------------------------------------------------------
      

        public void Del_Conso(int consoId)
        {

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                string sql = "DELETE from Consommation WHERE Conso_Id = @paramId";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@paramId", SqlDbType.VarChar).Value = consoId;


                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                conn.Close();

            }
        }

    }
}