using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Commerce.Models;

namespace E_Commerce.DAO
{
    public class detalleBoletaDAO
    {
        conexionDAO cn;
        public IEnumerable<DetalleBoleta> lista_detalle_boleta(string num_bol = null)
        {
            List<DetalleBoleta> temporal = new List<DetalleBoleta>();
            cn = new conexionDAO();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_lista_detalle_boleta", cn.getcn);
            SqlDataReader dr = cmd.ExecuteReader();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                while (dr.Read())
                {
                    DetalleBoleta reg = new DetalleBoleta();
                    reg.num_det_bol = dr.GetString(0);
                    reg.num_bol = dr.GetString(1);
                    reg.id_prod = dr.GetString(2);
                    reg.cant_prod = dr.GetInt32(3);
                    reg.precio = dr.GetDecimal(4);
                    reg.sub_tot = dr.GetDecimal(5);
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
        public string make_payment(List<Item> temporal, Usuario reg, SqlTransaction tr, string id_direc, string id_tarj, double subtotal, double descuento, double total)
        {
            string mensaje = "";
            cn = new conexionDAO();
            cn.getcn.Open();
            SqlCommand cmd = new SqlCommand("sp_insertar_boleta", cn.getcn, tr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_usua", reg.id_usua);
            cmd.Parameters.AddWithValue("@id_direc", id_direc);
            cmd.Parameters.AddWithValue("@id_tarj", id_tarj);
            cmd.Parameters.AddWithValue("@impo_bol", subtotal);
            cmd.Parameters.AddWithValue("@desc_bol", descuento);
            cmd.Parameters.AddWithValue("@envio", 0);
            cmd.Parameters.AddWithValue("@total_bol", total);
            cmd.ExecuteNonQuery();

            foreach (Item item in temporal)
            {
                cmd = new SqlCommand("sp_insertar_detalle_boleta", cn.getcn, tr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_prod", item.id_prod);
                cmd.Parameters.AddWithValue("@cant_prod", item.cant);
                cmd.Parameters.AddWithValue("@precio_prod", item.precio);
                cmd.Parameters.AddWithValue("@sub_tot", item.sub_total);
                cmd.ExecuteNonQuery();
            }

            tr.Commit(); //actualiza
            mensaje = "Se ha realizado correctamente la transacción, gracias por tu compra";            
            return mensaje;
        }
    }
}