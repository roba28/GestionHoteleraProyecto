using GestionHoteleraProyecto.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class HabitacionesTokyoModel : PageModel
    {
        public List<Habitacion> HabitacionesDisponibles { get; set; }

        public void OnGet()
        {
            HabitacionesDisponibles = new List<Habitacion>();

            string connectionString = "Server=CRC-LP-0109\\SQLEXPRESS;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";
            string queryString = "SELECT * FROM Habitaciones WHERE Nombre = 'Hotel Continental de Roma'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    HabitacionesDisponibles.Add(new Habitacion
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Torre = reader["Torre"].ToString(),
                        Piso = reader["Piso"].ToString(),
                        NumeroHabitacion = reader["NumeroHabitacion"].ToString(),
                        Disponibilidad = Convert.ToBoolean(reader["Disponibilidad"])
                    });
                }

                reader.Close();
            }
        }
    }
}
