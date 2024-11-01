using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;


namespace servicioWEBsoap.data_base
{
    public class Conexion
    {
        private MySqlConnection conexion;

        public Conexion()
        {
            string connection = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            conexion = new MySqlConnection(connection);
        }

        public void OpenConnection()
        {
            if (conexion.State == System.Data.ConnectionState.Closed)
            {
                conexion.Open();
                Console.WriteLine("Conexion exitosa");
            }
        }

        public void CloseConnection()
        {
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                conexion.Close();
                Console.WriteLine("Conexion cerrada");
            }
        }

        public MySqlConnection GetConnection()
        {
            return conexion;
        }



    }
}