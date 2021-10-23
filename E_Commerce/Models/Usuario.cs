using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Usuario
    {
        public string id { get; set; }
        public string username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [DataType(DataType.Password)]
        public string contraseña { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string telefono { get; set; }
        [DataType(DataType.Date)]
        [Required, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string nacimiento { get; set; }
        [DataType(DataType.Upload)]
        public string foto { get; set; }
    }
}