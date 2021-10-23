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
        public string insertarUsuario(SqlParameter[] pars = null)
        {
            cn = new conexionDAO();
            string mensaje = "";
            using (cn.getcn)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_insertar_usuario", cn.getcn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (pars != null)
                    {
                        cmd.Parameters.AddRange(pars.ToArray());
                    }

                    cn.getcn.Open();
                    int c = cmd.ExecuteNonQuery();

                    mensaje = "Te has registrado exitosamente";
                }
                catch (SqlException ex) { mensaje = ex.Message; }
                finally { cn.getcn.Close(); }
            }
            return mensaje;
        }
    }
}