using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace E_Commerce.DAO
{
    public class conexionDAO
    {
        SqlConnection cn = new SqlConnection(
        ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
        public SqlConnection getcn
        {
            get { return cn; }
        }
    }
}