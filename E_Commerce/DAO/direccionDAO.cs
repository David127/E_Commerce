using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Commerce.Models;

namespace E_Commerce.DAO
{
    public class direccionDAO
    {
        conexionDAO cn;
        public IEnumerable<Direccion> lista_direccion(string id)
        {
            List<Direccion> temporal = new List<Direccion>();
            cn = new conexionDAO();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_lista_direccion", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                while (dr.Read())
                {
                    Direccion reg = new Direccion();
                    reg.id_direc = dr.GetString(0);
                    reg.desc_direc = dr.GetString(1);
                    reg.referencia = dr.GetString(2);
                    reg.etiqueta = dr.GetString(3);
                    reg.id_dist = dr.GetInt32(4);
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
        public string insertar_direccion(Direccion reg, Usuario u)
        {
            string mensaje = "";
            cn = new conexionDAO();
            cn.getcn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_insertar_direccion", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@desc", reg.desc_direc);
                cmd.Parameters.AddWithValue("@referencia", reg.referencia);
                cmd.Parameters.AddWithValue("@etiqueta", reg.etiqueta);
                cmd.Parameters.AddWithValue("@id_dist", reg.id_dist);
                cmd.Parameters.AddWithValue("@id_usua", u.id_usua);

                int i = 0;
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    mensaje = "Direccion agregada";
                }
            }
            catch (SqlException e)
            {
                mensaje = "Error al agregar direccion, vuelva a intentarlo";
            }
            finally
            {
                cn.getcn.Close();
            }
            return mensaje;
        }
        public string modificar_direccion(Direccion reg)
        {
            string mensaje = "";
            cn = new conexionDAO();
            cn.getcn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_modificar_direccion", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", reg.id_direc);
                cmd.Parameters.AddWithValue("@desc", reg.desc_direc);
                cmd.Parameters.AddWithValue("@referencia", reg.referencia);
                cmd.Parameters.AddWithValue("@etiqueta", reg.etiqueta);
                cmd.Parameters.AddWithValue("@id_dist", reg.id_dist);
                int i = 0;
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    mensaje = "Dirección modificada";
                }
            }
            catch (SqlException e)
            {
                mensaje = "Error al modificar la dirección, vuelve a intentarlo";
            }
            finally
            {
                cn.getcn.Close();
            }
            return mensaje;
        }
        public string eliminar_direccion(string id)
        {
            string mensaje = "";
            cn = new conexionDAO();
            cn.getcn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_eliminar_direccion", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_direc", id);

                int i = 0;                
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    mensaje = "Direccion eliminada";
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
        public IEnumerable<Direccion> lista_direccion_total(string id)
        {
            List<Direccion> temporal = new List<Direccion>();
            cn = new conexionDAO();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_lista_direccion", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                while (dr.Read())
                {
                    Direccion reg = new Direccion();
                    reg.id_direc = dr.GetString(0);
                    reg.desc_direc = dr.GetString(1);
                    reg.referencia = dr.GetString(2);
                    reg.etiqueta = dr.GetString(3);
                    reg.id_dist = dr.GetInt32(4);
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