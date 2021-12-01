using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Commerce.Models;

namespace E_Commerce.DAO
{
    public class productoDAO
    {
        conexionDAO cn;

        public IEnumerable<Producto> producto()
        {
            List<Producto> temporal = new List<Producto>();
            cn = new conexionDAO();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_listado_producto", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                while (dr.Read())
                {
                    Producto reg = new Producto();
                    reg.id_prod = dr.GetString(0);
                    reg.codigo = dr.GetString(1);
                    reg.id_categ = dr.GetInt32(2);
                    reg.marca = dr.GetString(3);
                    reg.modelo = dr.GetString(4);
                    reg.descripcion = dr.GetString(5);
                    reg.observacion = dr.GetString(6);
                    reg.fec_compra = dr.GetDateTime(7);
                    reg.stock = dr.GetInt32(8);
                    reg.precio = dr.GetDecimal(9);
                    reg.imagen = dr.GetString(10);
                    reg.calidad = dr.GetDecimal(11);
                    reg.estado = dr.GetInt32(12);
                    temporal.Add(reg);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close(); cn.getcn.Close();
            }            
            return temporal;
        }
        public IEnumerable<Producto> producto_calidad()
        {
            List<Producto> temporal = new List<Producto>();
            cn = new conexionDAO();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_listado_producto_calidad", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                while (dr.Read())
                {
                    Producto reg = new Producto();
                    reg.id_prod = dr.GetString(0);
                    reg.codigo = dr.GetString(1);
                    reg.id_categ = dr.GetInt32(2);
                    reg.marca = dr.GetString(3);
                    reg.modelo = dr.GetString(4);
                    reg.descripcion = dr.GetString(5);
                    reg.observacion = dr.GetString(6);
                    reg.fec_compra = dr.GetDateTime(7);
                    reg.stock = dr.GetInt32(8);
                    reg.precio = dr.GetDecimal(9);
                    reg.imagen = dr.GetString(10);
                    reg.calidad = dr.GetDecimal(11);
                    reg.estado = dr.GetInt32(12);
                    temporal.Add(reg);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                dr.Close(); cn.getcn.Close();
            }            
            return temporal;
        }
        public Producto buscar_producto(string id)
        {
            Producto reg;
            if (id == null)
            {
                reg = new Producto();
            }
            else
            {
                reg = producto().Where(x => x.id_prod == id).FirstOrDefault();
            }
            return reg;
        }

        
    }
}