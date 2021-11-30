using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Commerce.Models;

namespace E_Commerce.DAO
{
    public class tarjetaDAO
    {
        conexionDAO cn;
        public string insertar_tarjeta(Tarjeta reg, Usuario u)
        {
            string mensaje = "";
            cn = new conexionDAO();
            cn.getcn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_insertar_tarjeta", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tip_tarj", reg.tip_tarj);
                cmd.Parameters.AddWithValue("@num_tarj", reg.num_tarj);
                cmd.Parameters.AddWithValue("@fec_venc", reg.fec_venc);
                cmd.Parameters.AddWithValue("@cvv", reg.cvv);
                cmd.Parameters.AddWithValue("@id_usua", u.id_usua);
                int i = 0;
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    mensaje = "Tarjeta agregada";
                }
            }
            catch (SqlException e)
            {
                mensaje = "Error al agregar tarjeta, vuelve a intentarlo";
            }
            finally
            {
                cn.getcn.Close();
            }
            return mensaje;
        }
        public string modificar_tarjeta(Tarjeta reg)
        {
            string mensaje = "";
            cn = new conexionDAO();
            cn.getcn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_modificar_tarjeta", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_tarj", reg.id_tarj);
                cmd.Parameters.AddWithValue("@tip_tarj", reg.tip_tarj);
                cmd.Parameters.AddWithValue("@num_tarj", reg.num_tarj);
                cmd.Parameters.AddWithValue("@fec_venc", reg.fec_venc);
                cmd.Parameters.AddWithValue("@cvv", reg.cvv);
                int i = 0;
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    mensaje = "Tarjeta modificada";
                }
            }
            catch (SqlException e)
            {
                mensaje = "Error al modificar tarjeta, vuelve a intentarlo";
            }
            finally
            {
                cn.getcn.Close();
            }
            return mensaje;
        }
        public string eliminar_tarjeta(string id)
        {
            string mensaje = "";
            cn = new conexionDAO();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_eliminar_tarjeta", cn.getcn);
            try
            {                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_tarj", id);

                int i = 0;
                
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    mensaje = "Tarjeta eliminada";
                }
            }
            catch (SqlException e)
            {
                mensaje = e.Message;
            }
            finally
            {
                cn.getcn.Close();
            }
            return mensaje;
        }
        public IEnumerable<Tarjeta> lista_tarjeta(string id)
        {
            List<Tarjeta> temporal = new List<Tarjeta>();
            cn = new conexionDAO();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_lista_tarjeta", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                while (dr.Read())
                {
                    Tarjeta reg = new Tarjeta();
                    reg.id_tarj = dr.GetString(0);
                    reg.tip_tarj = dr.GetString(1);
                    reg.num_tarj = dr.GetString(2);
                    reg.fec_venc = dr.GetString(3);
                    reg.cvv = dr.GetInt32(4);
                    reg.id_usua = dr.GetString(5);
                    reg.estado = dr.GetInt32(6);
                    if (reg.estado == 1)
                    {
                        temporal.Add(reg);
                    }
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
        public IEnumerable<Tarjeta> lista_tarjeta_total(string id)
        {
            List<Tarjeta> temporal = new List<Tarjeta>();
            cn = new conexionDAO();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_lista_tarjeta", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                while (dr.Read())
                {
                    Tarjeta reg = new Tarjeta();
                    reg.id_tarj = dr.GetString(0);
                    reg.tip_tarj = dr.GetString(1);
                    reg.num_tarj = dr.GetString(2);
                    reg.fec_venc = dr.GetString(3);
                    reg.cvv = dr.GetInt32(4);
                    reg.id_usua = dr.GetString(5);
                    reg.estado = dr.GetInt32(6);
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