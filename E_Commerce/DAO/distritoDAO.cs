using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Commerce.Models;

namespace E_Commerce.DAO
{
    public class distritoDAO
    {
        conexionDAO cn;
        public IEnumerable<Distrito> lista_distrito()
        {
            cn = new conexionDAO();
            List<Distrito> temporal = new List<Distrito>();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("select*from tb_distrito", cn.getcn);            
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    Distrito reg = new Distrito();
                    reg.id_dist = dr.GetInt32(0);
                    reg.id_prov = dr.GetInt32(1);
                    reg.nom_dist = dr.GetString(2);

                    temporal.Add(reg);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close(); cn.getcn.Close();
            }
            return temporal;
        }
    }
}