using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Commerce.Models;

namespace E_Commerce.DAO
{
    public class boletaDAO
    {
        conexionDAO cn;        
        public IEnumerable<Boleta> lista_boleta(string id_usua = null)
        {
            cn = new conexionDAO();
            List<Boleta> temporal = new List<Boleta>();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_lista_boleta", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id_usua);

                while (dr.Read())
                {
                    Boleta reg = new Boleta();
                    reg.num_bol = dr.GetString(0);
                    reg.id_usua = dr.GetString(1);
                    reg.id_tarj = dr.GetString(2);
                    reg.id_direc = dr.GetString(3);
                    reg.fec_bol = dr.GetDateTime(4);
                    reg.impo_bol = dr.GetDecimal(5);
                    reg.desc_bol = dr.GetDecimal(6);
                    reg.envio = dr.GetDecimal(7);
                    reg.total_bol = dr.GetDecimal(8);
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