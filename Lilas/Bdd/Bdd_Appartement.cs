using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Lilas.Bdd
{
    public class Bdd_Appartement : Bdd
    {
        public Bdd_Appartement()
        {

        }

        //------------------------------------------------------------------------------------------------------------
        // GET APPARTEMENT
        //------------------------------------------------------------------------------------------------------------


        public Appartement Get_AppartementFromId(int ApptId)
        {
            Appartement appt = new Appartement();
            appt.AppartementId = ApptId;

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT UserName, Appartement, Batiment, Escalier, Etage, Porte, Type, IsDoubleVitrage, IsRobinetsThermo, Orientation, Partager FROM Appartement WHERE AppartementId=@paramApptId", conn))
                {
                    cmd.Parameters.AddWithValue("paramApptId", ApptId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appt.UserName = reader.GetString(0);
                            appt.AppartementName = reader.GetString(1);
                            appt.Batiment = reader.GetString(2);
                            appt.Escalier = reader.GetString(3);
                            appt.Etage = reader.GetInt32(4);
                            appt.Porte = reader.GetString(5);
                            appt.Type = reader.GetString(6);
                            appt.IsDoubleVitrage = reader.GetBoolean(7);
                            appt.IsRobinetsThermo = reader.GetBoolean(8);
                            appt.Orientation = reader.GetString(9);
                            appt.Partager = reader.GetBoolean(10);
                        }
                    }
                }
                conn.Close();
            }

            return appt;
        }


        public Appartement Get_AppartementFromName(string ApptName)
        {
            Appartement appt = new Appartement();
            appt.AppartementName = ApptName;

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT AppartementId, UserName, Batiment, Escalier, Etage, Porte, Type, IsDoubleVitrage, IsRobinetsThermo, Orientation, Partager FROM Appartement WHERE Appartement=@paramApptId", conn))
                {
                    cmd.Parameters.AddWithValue("paramApptId", ApptName);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appt.AppartementId = reader.GetInt32(0);
                            appt.UserName = reader.GetString(1);
                            appt.Batiment = reader.GetString(2);
                            appt.Escalier = reader.GetString(3);
                            appt.Etage = reader.GetInt32(4);
                            appt.Porte = reader.GetString(5);
                            appt.Type = reader.GetString(6);
                            appt.IsDoubleVitrage = reader.GetBoolean(7);
                            appt.IsRobinetsThermo = reader.GetBoolean(8);
                            appt.Orientation = reader.GetString(9);
                            appt.Partager = reader.GetBoolean(10);
                        }
                    }
                }
                conn.Close();
            }
            return appt;
        }

        public List<Appartement> Get_AllAppartements()
        {
            List<Appartement> listAppt = new List<Appartement>();

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT AppartementId, UserName, Appartement, Batiment, Escalier, Etage, Porte, Type, IsDoubleVitrage, IsRobinetsThermo, Orientation, Partager FROM Appartement", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Appartement appt = new Appartement();
                            appt.AppartementId = reader.GetInt32(0);
                            appt.UserName = reader.GetString(1);
                            appt.AppartementName = reader.GetString(2);
                            appt.Batiment = reader.GetString(3);
                            appt.Escalier = reader.GetString(4);
                            appt.Etage = reader.GetInt32(5);
                            appt.Porte = reader.GetString(6);
                            appt.Type = reader.GetString(7);
                            appt.IsDoubleVitrage = reader.GetBoolean(8);
                            appt.IsRobinetsThermo = reader.GetBoolean(9);
                            appt.Orientation = reader.GetString(10);
                            appt.Partager = reader.GetBoolean(11);
                            listAppt.Add(appt);
                        }
                    }
                }
                conn.Close();
            }

            return listAppt;
        }







        public bool IsAppartementNamePresent(string apptName)
        {
            bool isPresent = true;

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT Count(*) FROM Appartement WHERE Appartement=@paramAppartement", conn))
                {
                    cmd.Parameters.Add("@paramAppartement", SqlDbType.VarChar).Value = apptName;

                    Int32 count = (Int32)cmd.ExecuteScalar();
                    if (count == 0) isPresent = false;
                }
                conn.Close();
            }

            return isPresent;
        }









        //------------------------------------------------------------------------------------------------------------
        // INSERT
        //------------------------------------------------------------------------------------------------------------
        public bool Add_Appartement(Appartement appt)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Appartement VALUES(@UserName,@Appartement,@Batiment,@Etage,@Porte,@Type,@IsDoubleVitrage, @IsRobinetsThermo, @Orientation, @Partager,@Escalier)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = appt.UserName;
                    cmd.Parameters.Add("@Appartement", SqlDbType.VarChar).Value = appt.AppartementName;
                    cmd.Parameters.Add("@Batiment", SqlDbType.VarChar).Value = appt.Batiment;
                    cmd.Parameters.Add("@Escalier", SqlDbType.VarChar).Value = appt.Escalier;
                    cmd.Parameters.Add("@Etage", SqlDbType.Int).Value = appt.Etage;
                    cmd.Parameters.Add("@Porte", SqlDbType.VarChar).Value = appt.Porte;
                    cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = appt.Type;
                    cmd.Parameters.Add("@IsDoubleVitrage", SqlDbType.Bit).Value = appt.IsDoubleVitrage;
                    cmd.Parameters.Add("@IsRobinetsThermo", SqlDbType.Bit).Value = appt.IsRobinetsThermo;
                    cmd.Parameters.Add("@Orientation", SqlDbType.VarChar).Value = appt.Orientation;
                    cmd.Parameters.Add("@Partager", SqlDbType.Bit).Value = appt.Partager;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
                return true;
            }
            catch
            {
                return false;
            }

        }


        //------------------------------------------------------------------------------------------------------------
        // UPDATE
        //------------------------------------------------------------------------------------------------------------

        public bool Update_Appartement(Appartement appt)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE Appartement SET Appartement=@paramAppartement, UserName=@paramUsername, Type=@paramType, IsDoubleVitrage=@paramIsDoubleVitrage, IsRobinetsThermo = @paramIsRobinetsThermo, Orientation = @paramOrientation, Partager=@paramPartager WHERE AppartementId=@paramApptId";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@paramApptId", SqlDbType.VarChar).Value = appt.AppartementId;
                    cmd.Parameters.Add("@paramAppartement", SqlDbType.VarChar).Value = appt.AppartementName;
                    cmd.Parameters.Add("@paramUsername", SqlDbType.VarChar).Value = appt.UserName;
                    cmd.Parameters.Add("@paramType", SqlDbType.VarChar).Value = appt.Type;
                    cmd.Parameters.Add("@paramIsDoubleVitrage", SqlDbType.Bit).Value = appt.IsDoubleVitrage;
                    cmd.Parameters.Add("@paramIsRobinetsThermo", SqlDbType.Bit).Value = appt.IsRobinetsThermo;
                    cmd.Parameters.Add("@paramOrientation", SqlDbType.VarChar).Value = appt.Orientation;
                    cmd.Parameters.Add("@paramPartager", SqlDbType.Bit).Value = appt.Partager;

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
                return true;
            }
            catch
            {
                return false;
            }

        }





    }
}