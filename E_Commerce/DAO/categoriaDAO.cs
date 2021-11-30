using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using E_Commerce.Models;

namespace E_Commerce.DAO
{
    public class categoriaDAO
    {
        conexionDAO cn;
        public IEnumerable<Categoria> lista_categoria()
        {
            cn = new conexionDAO();
            List<Categoria> temporal = new List<Categoria>();
            SqlCommand cmd = new SqlCommand("select * from tb_categoria", cn.getcn);
            cn.getcn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Categoria reg = new Categoria();
                reg.id_categ = dr.GetInt32(0);
                reg.nombre = dr.GetString(1);
                reg.descripcion = dr.GetString(2);
                reg.estado = dr.GetInt32(3);
                if (reg.estado == 1)
                {
                    temporal.Add(reg);
                }
            }
            dr.Close(); cn.getcn.Close();
            return temporal;
        }
    }
}