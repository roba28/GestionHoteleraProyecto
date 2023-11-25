using GestionHoteleraProyecto.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class BuscarHabitacionDisponibleModel : PageModel
    {
        public List<string> Habitaciones { get; set; }

        public void OnGet()
        {
            CargarHabitaciones();
        }

        private void CargarHabitaciones()
        {
            Habitaciones = new List<string>();

            string connectionString = "Server=localhost;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";

            string queryString = "SELECT Nombre, Torre, Piso, NumeroHabitacion, Disponibilidad FROM Habitaciones";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string nombre = reader["Nombre"].ToString();
                    string torre = reader["Torre"].ToString();
                    string piso = reader["Piso"].ToString();
                    string numeroHabitacion = reader["NumeroHabitacion"].ToString();
                    int disponibilidad = Convert.ToInt32(reader["Disponibilidad"]);

                    string libre;

                    if (disponibilidad == 1)
                    {
                        libre = "Disponible";
                    }
                    else
                    {
                        libre = "No Disponible";
                    }

                    string habitacion = $"{nombre}, {torre}, {piso}, {numeroHabitacion}, Disponibilidad: {libre}";
                    Habitaciones.Add(habitacion);
                }

                reader.Close();
            }
        }

        private void CargarHabitaciones(string torre, string piso, string numeroHabitacion)
        {
            Habitaciones = new List<string>();

            string connectionString = "Server=localhost;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";

            string queryString = "SELECT Nombre, Torre, Piso, NumeroHabitacion, Disponibilidad FROM Habitaciones WHERE Torre = @Torre AND Piso = @Piso AND NumeroHabitacion = @NumeroHabitacion";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Torre", torre);
                command.Parameters.AddWithValue("@Piso", piso);
                command.Parameters.AddWithValue("@NumeroHabitacion", numeroHabitacion);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string nombre = reader["Nombre"].ToString();
                    string torreHabitacion = reader["Torre"].ToString();
                    string pisoHabitacion = reader["Piso"].ToString();
                    string NumeroHabitacion = reader["NumeroHabitacion"].ToString();
                    int disponibilidad = Convert.ToInt32(reader["Disponibilidad"]);

                    if (disponibilidad == 1)
                    {
                        // La habitación está disponible, puedes cargar la información o realizar otras acciones según tus necesidades
                        string informacionHabitacion = $"{nombre}, {torreHabitacion}, {pisoHabitacion}, {NumeroHabitacion}, Disponible";
                        Habitaciones.Add(informacionHabitacion);
                    }
                    else
                    {
                        // La habitación no está disponible, puedes manejar esto según tus necesidades
                        string informacionHabitacion = $"{nombre}, {torreHabitacion}, {pisoHabitacion}, {NumeroHabitacion}, No Disponible";
                        Habitaciones.Add(informacionHabitacion);
                    }
                }

                reader.Close();
            }
        }




        public IActionResult OnPostBuscarHabitaciones(string torre, string piso, string numeroHabitacion)
        {

            CargarHabitaciones(torre, piso, numeroHabitacion);

            return Page();
        }
        public IActionResult OnGetFiltrarTodos()
        {


            CargarHabitaciones();

            return Page();
        }
    }
}
