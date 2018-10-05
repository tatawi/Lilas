using Lilas.Objets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Lilas.Bdd
{
    public class Bdd_Travaux : Bdd
    {

        //------------------------------------------------------------------------------------------------------------
        // GET 
        //------------------------------------------------------------------------------------------------------------

        public List<Travail> Get_TravauxAppt(int AppartementId)
        {
            List<Travail> listTravauxAppt= new List<Travail>();

            using (var conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT TravailId, type, AppartementId, Prix, Entreprise, Contact, TypeAppartement FROM Travaux WHERE AppartementId=@paramApptId", conn))
                {
                    cmd.Parameters.AddWithValue("paramApptId", AppartementId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Travail tr = new Travail();
                            tr.TravailId = reader.GetInt32(0);
                            tr.type = reader.GetString(1);
                            tr.AppartementId = reader.GetInt32(2);
                            tr.Prix = reader.GetInt32(3);
                            tr.Entreprise = reader.GetString(4);
                            tr.Contact = reader.GetString(5);
                            tr.TypeAppartement = reader.GetString(6);

                            listTravauxAppt.Add(tr);
                        }
                    }
                }
                conn.Close();
            }

            return listTravauxAppt;
        }


        //------------------------------------------------------------------------------------------------------------
        // INSERT 
        //------------------------------------------------------------------------------------------------------------
        public bool Add_Travail(Travail tr)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO Travaux VALUES(@type,@AppartementId,@Prix,@Entreprise,@Contact,@TypeAppartement)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = tr.type;
                    cmd.Parameters.Add("@AppartementId", SqlDbType.VarChar).Value = tr.AppartementId;
                    cmd.Parameters.Add("@Prix", SqlDbType.VarChar).Value = tr.Prix;
                    cmd.Parameters.Add("@Entreprise", SqlDbType.VarChar).Value = tr.Entreprise;
                    cmd.Parameters.Add("@Contact", SqlDbType.Int).Value = tr.Contact;
                    cmd.Parameters.Add("@TypeAppartement", SqlDbType.VarChar).Value = tr.TypeAppartement;
                    
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


    }
}