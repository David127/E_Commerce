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
using E_Commerce.Controllers;
using System.Globalization;

namespace E_Commerce.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        conexionDAO cn = new conexionDAO();
        usuarioDAO usuarios = new usuarioDAO();
        productoDAO productos = new productoDAO();
        categoriaDAO categorias = new categoriaDAO();
        distritoDAO distritos = new distritoDAO();
        direccionDAO direcciones = new direccionDAO();
        tarjetaDAO tarjetas = new tarjetaDAO();
        detalleBoletaDAO detalleBoletas = new detalleBoletaDAO();
        boletaDAO boletas = new boletaDAO();

        private Usuario InicioSesion()
        {
            if (Session["login"] == null)
            {
                return null;
            }
            else
            {
                return (Session["login"] as Usuario);
            }
        }
        private void addProduct(string id = null, int cant = 0)
        {
            Producto reg = productos.buscar_producto(id);

            Item item = new Item();
            item.imagen = reg.imagen;
            item.nom_prod = reg.marca + " " + reg.modelo;
            item.id_prod = reg.id_prod;
            item.precio = reg.precio;
            item.cant = cant;

            List<Item> temporal = (List<Item>)Session["carrito"];

            if (temporal.Count() > 0)
            {
                Boolean tag = true;
                foreach (Item c in temporal)
                {
                    if (c.id_prod == item.id_prod)
                    {
                        c.cant += item.cant;
                        tag = false;
                        break;
                    }
                }
                if (tag)
                {
                    temporal.Add(item);
                }
            }
            else
            {
                temporal.Add(item);
            }
        }
        public ActionResult Index(string mensaje = null)
        {
            //validamos la existencia de la sesion
            if (Session["carrito"] == null)
            {
                Session["carrito"] = new List<Item>();
                ViewBag.producto = productos.producto_calidad().ToList();
            }
            else
            {
                ViewBag.producto = producto_carrito(productos.producto_calidad().ToList());
            }

            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }

            TempData["usuario"] = InicioSesion() as Usuario; //datos del usuario
            ViewBag.categoria = categorias.lista_categoria().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Index(string id = null, int stock = 0, int cant = 0)
        {
            string mensaje = null;
            if (cant > stock || cant < 1)
            {
                mensaje = "Ingrese cantidad válida";
            }
            else
            {
                addProduct(id, cant); //agregar al carrito de compra
                mensaje = "Producto agregado";
            }

            TempData["usuario"] = InicioSesion() as Usuario; //datos del usuario
            ViewBag.categoria = categorias.lista_categoria().ToList();

            return RedirectToAction("Index", new { mensaje = mensaje });
            //return null;
        }
        public List<Producto> producto_carrito(List<Producto> producto)
        {
            List<Item> temporal = (List<Item>)Session["carrito"];
            List<Producto> lista = new List<Producto>();
            foreach (Producto p in producto.ToList())
            {
                foreach (Item i in temporal)
                {
                    if (p.id_prod == i.id_prod)
                    {
                        p.stock = p.stock - i.cant;
                    }
                }
                lista.Add(p);
            }
            return lista;
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
            ViewBag.mensaje =  usuarios.insertarUsuario(reg);
            return RedirectToAction("RegisterUser", new { mensaje = ViewBag.mensaje });
        }

        // Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string user, string pass, Boolean pay = false, double subtotal = 0,
            double descuento = 0, double total = 0)
        {
            Usuario reg = usuarios.buscar_usuario(user, pass);
            if (reg == null)
            {
                ViewBag.mensaje = "Usuario o Clave Incorrecto";
                return View();
            }
            else
            {
                Session["login"] = reg;
                if (pay)
                {
                    return RedirectToAction("Pay_Data", new { subtotal = subtotal, descuento = descuento, total = total });
                }
                else
                {
                    return RedirectToAction("Index", new { mensaje = "Ha iniciado sesion" });
                }
            }
        }
        public ActionResult CloseSession()
        {
            Session["login"] = null;
            return RedirectToAction("Product", new { mensaje = "La sesión fue cerrada" });
        }
        // 
        public ActionResult Profile(string id = null, Boolean estado = false, string mensaje = null)
        {
            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }
            if (estado)
            {
                TempData["usuario"] = InicioSesion() as Usuario; //datos del usuario

                Usuario reg = usuarios.buscar_usuario_id(id);
                ViewBag.nombreCompleto = reg.nom_usua + ' ' + reg.ape_usua;
                return View(reg);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        [HttpPost]
        public ActionResult Profile(Usuario reg)
        {
            string mensaje = "";
            Usuario u = InicioSesion() as Usuario;
            mensaje = usuarios.modificar_usuario(reg);
            return RedirectToAction("Profile", new { mensaje = mensaje, id = u.id_usua, estado = true });
        }
        public ActionResult Address(string mensaje = null)
        {
            TempData["usuario"] = InicioSesion() as Usuario; //datos del usuario
            Usuario u = InicioSesion() as Usuario;
            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }
            ViewBag.distritos = distritos.lista_distrito().ToList();
            ViewBag.distrito = new SelectList(distritos.lista_distrito().ToList(), "id_dist", "nom_dist");
            return View(direcciones.lista_direccion(u.id_usua));
        }
        [HttpPost]
        public ActionResult Address(Direccion reg)
        {
            Usuario u = InicioSesion() as Usuario;
            ViewBag.mensaje = direcciones.insertar_direccion(reg, u);

            return RedirectToAction("Address", new { mensaje = ViewBag.mensaje });
        }
        public ActionResult Edit_Address(string id = null)
        {
            Usuario u = InicioSesion() as Usuario;
            Direccion reg = direcciones.lista_direccion(u.id_usua).Where(x => x.id_direc == id).FirstOrDefault();
            ViewBag.distrito = new SelectList(distritos.lista_distrito().ToList(), "id_dist", "nom_dist");
            return View(reg);
        }

        [HttpPost]
        public ActionResult Edit_Address(Direccion reg)
        {
            string mensaje = "";
            mensaje = direcciones.modificar_direccion(reg);
            ViewBag.mensaje = mensaje;
            return RedirectToAction("Address", new { mensaje = ViewBag.mensaje });
        }
        public ActionResult Delete_Address(string id = null)
        {
            string mensaje = "";
            mensaje = direcciones.eliminar_direccion(id);
            ViewBag.mensaje = mensaje;

            return RedirectToAction("Address", new { mensaje = ViewBag.mensaje });
        }
        public ActionResult Purchases()
        {
            Usuario u = InicioSesion() as Usuario;
            ViewBag.detalle = (List<DetalleBoleta>)detalleBoletas.lista_detalle_boleta();
            ViewBag.direccion = (List<Direccion>)direcciones.lista_direccion_total(u.id_usua).ToList();
            ViewBag.tarjeta = (List<Tarjeta>)tarjetas.lista_tarjeta_total(u.id_usua);
            ViewBag.producto = (List<Producto>)productos.producto();

            TempData["usuario"] = InicioSesion() as Usuario; //datos del usuario

            return View(boletas.lista_boleta(u.id_usua));
        }
        public ActionResult Card(string mensaje = null)
        {
            TempData["usuario"] = InicioSesion() as Usuario; //datos del usuario
            Usuario u = InicioSesion() as Usuario;
            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }
            return View(tarjetas.lista_tarjeta(u.id_usua));
        }
        [HttpPost]
        public ActionResult Card(Tarjeta reg)
        {
            string mensaje = "";
            int num_tip = int.Parse(reg.num_tarj.Substring(0, 1));
            if (num_tip == 4)
            {
                reg.tip_tarj = "Visa";
            }
            else if (num_tip == 5)
            {
                reg.tip_tarj = "Mastercad";
            }
            else
            {
                reg.tip_tarj = "Desconocido";
            }

            Usuario u = InicioSesion() as Usuario;
            mensaje = tarjetas.insertar_tarjeta(reg, u);
            ViewBag.mensaje = mensaje;
            return RedirectToAction("Card", new { mensaje = ViewBag.mensaje });
        }
        public ActionResult Edit_Card(string id = null)
        {
            Usuario u = InicioSesion() as Usuario;
            Tarjeta reg = tarjetas.lista_tarjeta(u.id_usua).Where(x => x.id_tarj == id).FirstOrDefault();
            return View(reg);
        }
        [HttpPost]
        public ActionResult Edit_Card(Tarjeta reg)
        {
            string mensaje = "";
            Usuario u = InicioSesion() as Usuario;
            int num_tip = int.Parse(reg.num_tarj.Substring(0, 1));
            if (num_tip == 4)
            {
                reg.tip_tarj = "Visa";
            }
            else if (num_tip == 5)
            {
                reg.tip_tarj = "Mastercad";
            }
            else
            {
                reg.tip_tarj = "Desconocido";
            }
            mensaje = tarjetas.modificar_tarjeta(reg);
            ViewBag.mensaje = mensaje;
            return RedirectToAction("Card", new { mensaje = ViewBag.mensaje });
        }
        public ActionResult Delete_Card(string id = null)
        {
            string mensaje = "";
            mensaje = tarjetas.eliminar_tarjeta(id);
            ViewBag.mensaje = mensaje;

            return RedirectToAction("Card", new { mensaje = ViewBag.mensaje });
        }
        // Productos
        public ActionResult Product(string mensaje = null, int p = 0, int id_categ = 0, string marca = "", string flecha = "")
        {
            TempData["usuario"] = InicioSesion() as Usuario; //datos del usuario

            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }

            IEnumerable<Producto> temporal = productos.producto();
            //validamos la existencia de la sesion
            if (Session["carrito"] == null)
            {
                Session["carrito"] = new List<Item>();
            }
            else
            {
                temporal = producto_carrito(productos.producto().ToList()).ToList();
            }

            ViewBag.categoria = categorias.lista_categoria().ToList();

            if (id_categ > 0)
            {
                temporal = temporal.Where(m => m.id_categ == id_categ);
            }

            if (marca != "")
            {
                temporal = temporal.Where(m => m.marca.StartsWith(marca,
                      StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            int f = 12;
            int c = temporal.Count();
            int npags = c % f == 0 ? c / f : c / f + 1;
            if (temporal.Count() > 0)
            {
                if (flecha == "ini")
                {
                    p = 0;
                }
                else if (flecha == "i")
                {
                    p--;
                    if (p < 0) p = 0;
                }
                else if (flecha == "d")
                {
                    p++;
                    if (p > npags - 1) p = npags - 1;
                }
                else if (flecha == "fin")
                {
                    p = npags - 1;
                }

                ViewBag.p = p;
                ViewBag.npags = npags;
                ViewBag.id_categ = id_categ;
                ViewBag.marca = marca;
            }
            return View(temporal.Skip(f * p).Take(f));
        }
        

        [HttpPost]
        public ActionResult Product(string id = null, int stock = 0, int cant = 0)
        {
            string mensaje = null;
            if (cant > stock || cant < 1)
            {
                mensaje = "Ingrese cantidad válida";
            }
            else
            {
                addProduct(id, cant); //agregar al carrito de compra
                mensaje = "Producto agregado";
            }

            TempData["usuario"] = InicioSesion() as Usuario; //datos del usuario
            ViewBag.categoria = categorias.lista_categoria().ToList();

            return RedirectToAction("Product", new { mensaje = mensaje });
            //return null;
        }
        public ActionResult Detail_Product(string mensaje, string id)
        {
            TempData["usuario"] = InicioSesion() as Usuario;
            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }

            Producto reg = productos.buscar_producto(id);
            if (Session["carrito"] != null)
            {
                List<Item> temporal = (List<Item>)Session["carrito"];
                foreach (Item i in temporal)
                {
                    if (reg.id_prod == i.id_prod)
                    {
                        reg.stock = reg.stock - i.cant;
                    }
                }
            }
            return View(reg);
        }

        [HttpPost]
        public ActionResult Detail_Product(string id = null, int stock = 0, int cant = 0)
        {
            string mensaje = null;
            if (cant > stock || cant < 1)
            {
                mensaje = "Ingrese cantidad válida";
            }
            else
            {
                addProduct(id, cant); //agregar al carrito de compra
                mensaje = "Producto agregado";
            }

            return RedirectToAction("Detail_Product", new { mensaje = mensaje });
        }
        public ActionResult Shopping_cart(string mensaje = null)
        {
            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }
            TempData["usuario"] = InicioSesion() as Usuario;
            List<Item> temporal = null;
            if (Session["carrito"] != null)
            {
                temporal = (List<Item>)Session["carrito"];
            }
            else
            {
                temporal = new List<Item>();
            }
            double total_prod = 0.0;
            double descuento = 0.0;
            double total = 0.0;
            if (temporal != null)
            {
                foreach (Item t in temporal)
                {
                    total_prod += (double)t.sub_total;
                }
                if (total_prod > 10)
                {
                    descuento = 10 % total_prod;
                    total = total_prod - descuento;
                }
            }
            ViewBag.subtotal = total_prod;
            ViewBag.descuento = descuento;
            ViewBag.total = total;
            return View(temporal);
        }
        public ActionResult Delete(string id = null, string nombre = null)
        {

            if (Session["carrito"] != null)
            {
                List<Item> temporal = (List<Item>)Session["carrito"];
                Item reg = temporal.Find(i => i.id_prod == id);
                temporal.Remove(reg);

                return RedirectToAction("Shopping_cart", new { mensaje = "Producto (" + nombre + ") fue eliminado" });
            }
            else
            {
                return RedirectToAction("Shopping_cart");
            }
        }
        public ActionResult Pay_Data(string id = null, string nombre = null, double subtotal = 0, double total = 0,
            string mensaje = null, double descuento = 0)
        {
            if (mensaje != null)
            {
                ViewBag.mensaje = mensaje;
            }
            if (((List<Item>)Session["carrito"]).Count() < 1)
            {
                return RedirectToAction("Shopping_cart", new { mensaje = "Escoja al menos un producto" });
            }
            else
            {
                if (InicioSesion() == null)
                {
                    return RedirectToAction("Login", new { pay = true, subtotal = subtotal, descuento = descuento, total = total });
                }
                else
                {
                    ViewBag.subtotal = subtotal;
                    ViewBag.descuento = descuento;
                    ViewBag.total = total;
                    Usuario reg = InicioSesion() as Usuario;
                    ViewBag.usuario = reg.nom_usua + " " + reg.ape_usua;
                    ViewBag.actual = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                    TempData["usuario"] = InicioSesion() as Usuario;

                    ViewBag.usua = reg.id_usua;
                    ViewBag.direccion = new SelectList(direcciones.lista_direccion(reg.id_usua), "id_direc", "etiqueta");
                    ViewBag.distrito = new SelectList(distritos.lista_distrito(), "id_dist", "nom_dist");
                    return View();
                }
            }
        }
        [HttpPost]
        public ActionResult Pay_Data(Direccion reg, double subtotal = 0, double total = 0, double descuento = 0)
        {
            Usuario u = InicioSesion() as Usuario;
            string mensaje = "";
            mensaje = direcciones.insertar_direccion(reg, u);
            ViewBag.mensaje = mensaje;

            return RedirectToAction("Pay_Data", new
            {
                mensaje = ViewBag.mensaje,
                subtotal = subtotal,
                descuento = descuento,
                total = total
            });
        }
        public ActionResult Payment_Methods(double subtotal = 0, double total = 0,
            double descuento = 0, string id_direc = null, string mensaje = null, Boolean estado = false)
        {

            if (estado)
            {
                if (mensaje != null)
                {
                    ViewBag.mensaje = mensaje;
                }

                TempData["usuario"] = InicioSesion() as Usuario;
                Usuario reg = InicioSesion() as Usuario;
                ViewBag.usuario = reg.nom_usua + " " + reg.ape_usua;
                ViewBag.subtotal = subtotal;
                ViewBag.descuento = descuento;
                ViewBag.total = total;
                ViewBag.usua = reg.id_usua;
                if (id_direc != null)
                {
                    ViewBag.direccion = direcciones.lista_direccion(reg.id_usua).Where(d => d.id_direc == id_direc).FirstOrDefault().desc_direc;
                    ViewBag.id_direc = id_direc;
                }
                ViewBag.tarjeta = new SelectList(tarjetas.lista_tarjeta(reg.id_usua), "id_tarj", "num_tarj");
                return View();
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        [HttpPost]
        public ActionResult Payment_Methods(Tarjeta reg, double subtotal = 0, double total = 0,
            double descuento = 0, string id_direc = null, bool estado = false)
        {
            string mensaje = "";
            int num_tip = int.Parse(reg.num_tarj.Substring(0, 1));           
            if (num_tip == 4)
            {
                reg.tip_tarj = "Visa";
            }
            else if (num_tip == 5)
            {
                reg.tip_tarj = "Mastercad";
            }
            else
            {
                reg.tip_tarj = "Desconocido";
            }
            /*---agregar tarjeta--------*/
            Usuario u = InicioSesion() as Usuario;
            mensaje = tarjetas.insertar_tarjeta(reg, u);
            ViewBag.mensaje = mensaje;

            return RedirectToAction("Payment_Methods", new
            {
                mensaje = ViewBag.mensaje,
                subtotal = subtotal,
                descuento = descuento,
                total = total,
                id_direc = id_direc,
                estado = estado
            });
        }
        public ActionResult Confirm_Payment(double subtotal = 0, double descuento = 0, double total = 0,
            string id_direc = null, string id_tarj = null, Boolean estado = false)
        {
            if (estado)
            {
                TempData["usuario"] = InicioSesion() as Usuario;
                Usuario u = InicioSesion() as Usuario;
                ViewBag.subtotal = subtotal;
                ViewBag.descuento = descuento;
                ViewBag.total = total;
                ViewBag.id_direc = id_direc;
                ViewBag.id_tarj = id_tarj;

                Direccion dc = direcciones.lista_direccion(u.id_usua).Where(d => d.id_direc == id_direc).FirstOrDefault();
                Tarjeta tj = tarjetas.lista_tarjeta(u.id_usua).Where(t => t.id_tarj == id_tarj).FirstOrDefault();
                ViewBag.direccion = dc.desc_direc + " " + distritos.lista_distrito().Where(di => di.id_dist == dc.id_dist).FirstOrDefault().nom_dist +
                    " (" + dc.etiqueta + ") ";
                ViewBag.tarjeta = tj.num_tarj + " (" + tj.tip_tarj + ")";

                ViewBag.carrito = (List<Item>)Session["carrito"];
                return View();
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Make_Pagament(double subtotal = 0, double descuento = 0, double total = 0,
            string id_direc = null, string id_tarj = null, Boolean estado = false)
        {
            if (estado)
            {
                Usuario reg = Session["login"] as Usuario;
                List<Item> temporal = (List<Item>)Session["carrito"];
                string mensaje = null;
                
                conexionDAO cn = new conexionDAO();
                cn.getcn.Open();
                SqlTransaction tr = cn.getcn.BeginTransaction(IsolationLevel.Serializable);
                try
                {
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
                }
                catch (SqlException ex)
                {
                    mensaje = "No se puede realizar proceso, vuelva a intentarlo";
                    tr.Rollback();
                }
                finally
                {
                    Session["carrito"] = new List<Item>();
                    // cn.getcn.Open();
                }
                // Otra forma
                /*
                SqlTransaction tr = cn.getcn.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    mensaje = detalleBoletas.make_payment(temporal, reg, tr, id_direc, id_tarj, subtotal, descuento, total);
                }
                catch (SqlException ex)
                {
                    mensaje = "No se puede realizar proceso, vuelva a intentarlo";
                    tr.Rollback();
                }
                finally
                {
                    Session["carrito"] = new List<Item>();
                    // cn.getcn.Close();
                }*/
                return RedirectToAction("Shopping_Cart", new { mensaje = mensaje });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Error()
        {
            TempData["usuario"] = InicioSesion() as Usuario;
            return View();
        }
    }
}