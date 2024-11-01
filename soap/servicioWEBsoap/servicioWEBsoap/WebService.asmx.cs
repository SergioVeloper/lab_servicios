using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using servicioWEBsoap.data_base;
using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using System.Data.Common;

namespace servicioWEBsoap
{
    /// <summary>
    /// Descripción breve de WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string ConexionDB()
        {
            Conexion db = new Conexion();
            db.OpenConnection();

            db.CloseConnection();

            return "Conexion exitosa";
        }

        [WebMethod]
        public string obtenerCotizacion(string fecha)
        {
            Conexion db = new Conexion();
            db.OpenConnection();

            string query = "SELECT * FROM cotizaciones WHERE fecha = '" + fecha + "'";

            MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
            MySqlDataReader reader = cmd.ExecuteReader();

            string cotizacion = "";
            while (reader.Read())
            {
                cotizacion = reader["cotizacion"].ToString();
            }

            db.CloseConnection();

            return cotizacion;
        }

        [WebMethod]
        public string registrarCotizacion(string fecha, double monto, double monto_oficial)
        {
            Conexion db = new Conexion();
            db.OpenConnection();

            string query = "INSERT INTO cotizaciones (fecha, cotizacion, cotizacion_oficial) VALUES ('" + fecha + "', " + monto + ", " + monto_oficial + ")";

            MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
            cmd.ExecuteNonQuery();

            db.CloseConnection();

            return "Cotizacion registrada exitosamente";
        }


    }
}
