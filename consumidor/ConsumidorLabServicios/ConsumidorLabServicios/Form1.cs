using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Librerías para HTTP y JSON
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

// Clases de modelo
using ConsumidorLabServicios.Models;
using System.Globalization;

namespace ConsumidorLabServicios
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Métodos REST
        public async Task<List<Cotizacion>> GetCotizacionesRestAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };

                return JsonSerializer.Deserialize<List<Cotizacion>>(jsonContent, options);
            }
        }

        public async Task<Cotizacion> GetCotizacionPorFechaRestAsync(string url, string fecha)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{url}?fecha={fecha}");
                response.EnsureSuccessStatusCode();

                string jsonContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };

                List<Cotizacion> cotizaciones = JsonSerializer.Deserialize<List<Cotizacion>>(jsonContent, options);
                return cotizaciones?.FirstOrDefault();
            }
        }

        public async Task<Cotizacion> AddCotizacionRestAsync(string url, string fecha, double cotizacion, double cotizacionOficial)
        {
            using (HttpClient client = new HttpClient())
            {
                var nuevaCotizacion = new
                {
                    fecha = fecha,
                    cotizacion = cotizacion,
                    cotizacion_oficial = cotizacionOficial
                };

                string jsonContent = JsonSerializer.Serialize(nuevaCotizacion);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };

                return JsonSerializer.Deserialize<Cotizacion>(responseContent, options);
            }
        }

        // Métodos SOAP
        public async Task<string> AddCotizacionSoapAsync(string url, string fecha, double cotizacion, double cotizacionOficial)
        {
            string soapRequest = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <tem:registrarCotizacion>
                                             <tem:fecha>{fecha}</tem:fecha>
                                             <tem:cotizacion>{cotizacion.ToString(CultureInfo.InvariantCulture)}</tem:cotizacion>
                                             <tem:cotizacion_oficial>{cotizacionOficial.ToString(CultureInfo.InvariantCulture)}</tem:cotizacion_oficial>
                                          </tem:registrarCotizacion>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";

            MessageBox.Show("Contenido del XML enviado:\n\n" + soapRequest, "Depuración de SOAP Request");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/xml"));
                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(responseContent);
                var resultNode = doc.GetElementsByTagName("registrarCotizacionResult").Item(0);

                return resultNode != null ? resultNode.InnerText : "No se recibió una confirmación del registro de cotización.";
            }
        }

        public async Task<string> GetCotizacionPorFechaSoapAsync(string url, string fecha)
        {
            string soapRequest = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <tem:obtenerCotizacion>
                                             <tem:fecha>{fecha}</tem:fecha>
                                          </tem:obtenerCotizacion>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(responseContent);
                var resultNode = doc.GetElementsByTagName("obtenerCotizacionResult").Item(0);

                return resultNode != null ? resultNode.InnerText : "No se encontró una cotización para la fecha especificada.";
            }
        }

        // Métodos GraphQL
        public async Task<string> AddCotizacionGraphQLAsync(string url, string fecha, double cotizacion, double cotizacionOficial)
        {
            //construye la consulta GraphQL
            string query = $"mutation {{ addCotizacion(fecha: \\\"{fecha}\\\", cotizacion: {cotizacion.ToString(CultureInfo.InvariantCulture)}, cotizacion_oficial: {cotizacionOficial.ToString(CultureInfo.InvariantCulture)}) {{ id fecha cotizacion cotizacion_oficial }} }}";

            using (HttpClient client = new HttpClient())
            {
                // Prepara el contenido en el formato JSON correcto para GraphQL
                var content = new StringContent($"{{\"query\": \"{query}\"}}", Encoding.UTF8, "application/json");

                //envio la solicitud
                HttpResponseMessage response = await client.PostAsync(url, content);

                response.EnsureSuccessStatusCode();

                //lee JSON de la respuesta
                string jsonContent = await response.Content.ReadAsStringAsync();

                //extraer el resultado de la respuesta
                var jsonDocument = JsonDocument.Parse(jsonContent);
                var data = jsonDocument.RootElement.GetProperty("data").GetProperty("addCotizacion");

                // Formatea el resultado para devolverlo de manera legible
                return $"ID: {data.GetProperty("id").GetString()}, Fecha: {data.GetProperty("fecha").GetString()}, " +
                       $"Cotización: {data.GetProperty("cotizacion").GetDouble()}, " +
                       $"Cotización Oficial: {data.GetProperty("cotizacion_oficial").GetDouble()}";
            }
        }


        public async Task<string> GetCotizacionPorFechaGraphQLAsync(string url, string fecha)
        {
            // Construye la consulta GraphQL en un string JSON
            string query = $"{{ \"query\": \"query {{ cotizacion(fecha: \\\"{fecha}\\\") {{ id fecha cotizacion cotizacion_oficial }} }}\" }}";

            using (HttpClient client = new HttpClient())
            {
                // Configura el contenido como JSON
                var content = new StringContent(query, Encoding.UTF8, "application/json");

                // Envía la solicitud POST al servidor GraphQL
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Asegúrate de que la respuesta sea exitosa
                response.EnsureSuccessStatusCode();

                // Lee el contenido de la respuesta
                string jsonContent = await response.Content.ReadAsStringAsync();

                // Procesa el JSON para devolver el resultado en un formato legible
                var jsonDocument = JsonDocument.Parse(jsonContent);
                var data = jsonDocument.RootElement.GetProperty("data").GetProperty("cotizacion");

                // Validar si hay datos o no
                if (data.ValueKind == JsonValueKind.Null)
                {
                    return "No se encontró una cotización para la fecha especificada.";
                }

                // Extrae los valores
                string result = $"ID: {data.GetProperty("id").GetString()}, Fecha: {data.GetProperty("fecha").GetString()}, " +
                                $"Cotización: {data.GetProperty("cotizacion").GetDouble()}, " +
                                $"Cotización Oficial: {data.GetProperty("cotizacion_oficial").GetDouble()}";

                return result;
            }
        }


        // Método para cargar los servicios disponibles en el ComboBox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        // Método del botón para ejecutar la operación seleccionada
        private async void btnEjecutar_Click(object sender, EventArgs e)
        {
            try
            {
                string resultado = "";
                string fecha = txtFecha.Text;
                double cotizacion = 0, cotizacionOficial = 0;
                string url = "";

                if (cmbOperation.SelectedItem.ToString() == "Agregar")
                {
                    cotizacion = double.Parse(txtCotizacion.Text, CultureInfo.InvariantCulture);
                    cotizacionOficial = double.Parse(txtCotizacionOficial.Text, CultureInfo.InvariantCulture);
                }

                switch (cmbServiceType.SelectedItem.ToString())
                {
                    case "REST":
                        url = "http://127.0.0.1:8000/api/cotizaciones";
                        if (cmbOperation.SelectedItem.ToString() == "Listar")
                        {
                            var cotizaciones = await GetCotizacionesRestAsync(url);
                            resultado = string.Join("\n", cotizaciones.Select(c => $"ID: {c.id}, Fecha: {c.fecha}, Cotización: {c.cotizacion}, Cotización Oficial: {c.cotizacionOficial}"));
                        }
                        else if (cmbOperation.SelectedItem.ToString() == "Consultar por Fecha")
                        {
                            var cotizacionItem = await GetCotizacionPorFechaRestAsync(url, fecha);
                            resultado = cotizacionItem != null
                                ? $"ID: {cotizacionItem.id}, Fecha: {cotizacionItem.fecha}, Cotización: {cotizacionItem.cotizacion}, Cotización Oficial: {cotizacionItem.cotizacionOficial}"
                                : "No se encontró una cotización para la fecha especificada.";
                        }
                        else if (cmbOperation.SelectedItem.ToString() == "Agregar")
                        {
                            var cotizacionItem = await AddCotizacionRestAsync(url, fecha, cotizacion, cotizacionOficial);
                            resultado = $"ID: {cotizacionItem.id}, Fecha: {cotizacionItem.fecha}, Cotización: {cotizacionItem.cotizacion}, Cotización Oficial: {cotizacionItem.cotizacionOficial}";
                        }
                        break;

                    case "SOAP":
                        url = "http://localhost:52273/WebService.asmx";
                        if (cmbOperation.SelectedItem.ToString() == "Agregar")
                        {
                            resultado = await AddCotizacionSoapAsync(url, fecha, cotizacion, cotizacionOficial);
                        }
                        else if (cmbOperation.SelectedItem.ToString() == "Consultar por Fecha")
                        {
                            resultado = await GetCotizacionPorFechaSoapAsync(url, fecha);
                        }
                        break;

                    case "GraphQL":
                        url = "http://localhost:4000/graphql";
                        if (cmbOperation.SelectedItem.ToString() == "Agregar")
                        {
                            resultado = await AddCotizacionGraphQLAsync(url, fecha, cotizacion, cotizacionOficial);
                        }
                        else if (cmbOperation.SelectedItem.ToString() == "Consultar por Fecha")
                        {
                            resultado = await GetCotizacionPorFechaGraphQLAsync(url, fecha);
                        }
                        break;
                }

                txtResultado.Text = resultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
