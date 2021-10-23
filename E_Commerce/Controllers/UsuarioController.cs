using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using E_Commerce.DAO;
using E_Commerce.Models;

namespace E_Commerce.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;
        usuarioDAO usuarios = new usuarioDAO();


        public ActionResult Index()
        {
            return View();
        }

        // Register a new user
        public ActionResult RegisterUser(string mensaje = null)
        {
            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(Usuario reg)
        {

            SqlConnection cn = new SqlConnection(cadena);
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_insertar_usuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", reg.username);
                cmd.Parameters.AddWithValue("@email", reg.email);
                cmd.Parameters.AddWithValue("@contraseña", reg.contraseña);
                cmd.Parameters.AddWithValue("@nombre", reg.nombre);
                cmd.Parameters.AddWithValue("@apellido", reg.apellido);
                cmd.Parameters.AddWithValue("@telefono", reg.telefono);
                cmd.Parameters.AddWithValue("@nacimiento", reg.nacimiento);
                cmd.Parameters.AddWithValue("@foto", reg.foto);

                int i = 0;
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    ViewBag.mensaje = "Te has registrado exitosamente";
                }
            }
            catch (SqlException e)
            {
                ViewBag.mensaje = "Error al registrarse, vuelve a intentarlo";
            }
            finally
            {
                cn.Close();
            }
            return RedirectToAction("RegisterUser", new { mensaje = ViewBag.mensaje });
        }

        public ActionResult Registrar(string mensaje = null)
        {
            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario reg, HttpPostedFileBase archivo)
        {
            if (archivo == null || archivo.ContentLength < 300)
            {
                ViewBag.mensaje = "Seleccione un archivo de tamaño mínimo 300 bytes";
                return RedirectToAction("Registrar", new { mensaje = ViewBag.mensaje });
            }

            string ruta = "~/imagenes/usuario/" + System.IO.Path.GetFileName(archivo.FileName);
            archivo.SaveAs(Server.MapPath(ruta));

            SqlParameter[] pars =
            {
                new SqlParameter(){ParameterName="@username",Value  =reg.username},
                new SqlParameter(){ParameterName="@email",Value =reg.email},
                new SqlParameter(){ParameterName="@contraseña",Value =reg.contraseña},
                new SqlParameter(){ParameterName="@nombre",Value =reg.nombre},
                new SqlParameter(){ParameterName="@apellido",Value =reg.apellido},
                new SqlParameter(){ParameterName="@telefono",Value =reg.telefono},
                new SqlParameter(){ParameterName="@nacimiento",Value =reg.nacimiento},
                new SqlParameter(){ParameterName="@foto",Value =ruta}
            };

            ViewBag.mensaje = usuarios.insertarUsuario(pars);

            // return RedirectToAction("Registrar", new { mensaje = ViewBag.mensaje });
            return View(reg);
        }
    }
}