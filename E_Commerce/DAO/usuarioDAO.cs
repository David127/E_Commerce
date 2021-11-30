using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Commerce.Models;

namespace E_Commerce.DAO
{
    public class usuarioDAO
    {
        conexionDAO cn;
        public string insertarUsuario(Usuario reg)
        {
            cn = new conexionDAO();
            string mensaje = "";
            try
            {
                SqlCommand cmd = new SqlCommand("sp_insertar_usuario", cn.getcn);
                cn.getcn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dni", reg.dni_usua);
                cmd.Parameters.AddWithValue("@nom", reg.nom_usua);
                cmd.Parameters.AddWithValue("@apel", reg.ape_usua);
                cmd.Parameters.AddWithValue("@tel", reg.tel_usua);
                cmd.Parameters.AddWithValue("@fec_nac_usua", reg.fec_nac_usua);
                cmd.Parameters.AddWithValue("@usuario", reg.usuario);
                cmd.Parameters.AddWithValue("@pass", reg.pass);
                cmd.Parameters.AddWithValue("@email_log", reg.email_log);

                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    mensaje = "Te has registrado exitosamente";
                }
            }
            catch (SqlException ex)
            { 
                mensaje = "Error al registrarse, vuelve a intentarlo.";
            }
            finally 
            {
                cn.getcn.Close();
            }
            return mensaje;
        }
        public Usuario buscar_usuario(string usuario = null, string pass = null)
        {
            cn = new conexionDAO();
            Usuario reg = null;
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_buscar_user", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", usuario);
                cmd.Parameters.AddWithValue("@pass", pass);
                
                if (dr.Read())
                {
                    reg = new Usuario();
                    reg.id_usua = dr.GetString(0);
                    reg.dni_usua = dr.GetString(1);
                    reg.id_rol = dr.GetInt32(2);
                    reg.nom_usua = dr.GetString(3);
                    reg.ape_usua = dr.GetString(4);
                    reg.tel_usua = dr.GetString(5);
                    reg.fec_nac_usua = dr.GetDateTime(6);
                    reg.usuario = dr.GetString(7);
                    reg.pass = dr.GetString(8);
                    reg.email_log = dr.GetString(9);
                    reg.estado = dr.GetInt32(10);
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close(); cn.getcn.Close();
            }
            return reg;
        }
        public IEnumerable<Usuario> lista_usuario()
        {
            cn = new conexionDAO();
            List<Usuario> temporal = new List<Usuario>();            
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_lista_usuario", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                while (dr.Read())
                {
                    Usuario reg = new Usuario();
                    reg.id_usua = dr.GetString(0);
                    reg.dni_usua = dr.GetString(1);
                    reg.id_rol = dr.GetInt32(2);
                    reg.nom_usua = dr.GetString(3);
                    reg.ape_usua = dr.GetString(4);
                    reg.tel_usua = dr.GetString(5);
                    reg.fec_nac_usua = dr.GetDateTime(6);
                    reg.usuario = dr.GetString(7);
                    reg.pass = dr.GetString(8);
                    reg.email_log = dr.GetString(9);
                    reg.estado = dr.GetInt32(10);
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
        public Usuario buscar_usuario_id(string id)
        {
            Usuario reg;
            if (id == null)
            {
                reg = new Usuario();
            }
            else
            {
                reg = lista_usuario().Where(x => x.id_usua == id).FirstOrDefault();
            }
            return reg;
        }
        public string modificar_usuario(Usuario reg)
        {
            string mensaje = null;
            cn = new conexionDAO();
            cn.getcn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_modificar_usuario", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usua", reg.id_usua);
                cmd.Parameters.AddWithValue("@dni", reg.dni_usua);
                cmd.Parameters.AddWithValue("@nom", reg.nom_usua);
                cmd.Parameters.AddWithValue("@apel", reg.ape_usua);
                cmd.Parameters.AddWithValue("@tel", reg.tel_usua);
                cmd.Parameters.AddWithValue("@fec_nac_usua", reg.fec_nac_usua);
                cmd.Parameters.AddWithValue("@usuario", reg.usuario);
                cmd.Parameters.AddWithValue("@pass", reg.pass);
                cmd.Parameters.AddWithValue("@email_log", reg.email_log);
                int i = 0;
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    mensaje = "Usuario modificado";
                }
            }
            catch (SqlException e)
            {
                mensaje = "Error al modificar usuario, vuelve a intentarlo";
                mensaje = e.Message;
            }
            finally
            {
                cn.getcn.Close();
            }
            return mensaje;
        }
    }
}