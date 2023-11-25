using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class BuscarPersonaModel : PageModel
    {
        public List<string> Reservaciones { get; set; }

        public void OnGet()
        {
            CargarReservaciones();
        }

        private void CargarReservaciones()
        {
            Reservaciones = new List<string>();

            string connectionString = "Server=ADSP-13207\\MSSQLSERVER01;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";

            string queryString = "SELECT Nombre, PrimerApellido, SegundoApellido, CedulaIdentidad, Nacionalidad, Telefono, CorreoElectronico, NombreHotel, Torre, Piso, NumeroHabitacion FROM Reservaciones";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string nombre = reader["Nombre"].ToString();
                    string primerApellido = reader["PrimerApellido"].ToString();
                    string segundoApellido = reader["SegundoApellido"].ToString();
                    string cedulaIdentidad = reader["CedulaIdentidad"].ToString();
                    string nacionalidad = reader["Nacionalidad"].ToString();
                    string telefono = reader["Telefono"].ToString();
                    string correoElectronico = reader["CorreoElectronico"].ToString();
                    string nombreHotel = reader["NombreHotel"].ToString();
                    string torre = reader["Torre"].ToString();
                    string piso = reader["Piso"].ToString();
                    string numeroHabitacion = reader["NumeroHabitacion"].ToString();

                    string reservacion = $"{nombre}, {primerApellido}, {segundoApellido}, {cedulaIdentidad}, {nacionalidad}, {telefono}, {correoElectronico}, {nombreHotel}, {torre}, {piso}, {numeroHabitacion}";
                    Reservaciones.Add(reservacion);
                }

                reader.Close();
            }
        }
        private void CargarReservaciones(string cedulaIdentidad)
        {
            Reservaciones = new List<string>();

            string connectionString = "Server=ADSP-13207\\MSSQLSERVER01;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";

            string queryString = "SELECT Nombre, PrimerApellido, SegundoApellido, CedulaIdentidad, Nacionalidad, Telefono, CorreoElectronico, NombreHotel, Torre, Piso, NumeroHabitacion FROM Reservaciones WHERE CedulaIdentidad = @CedulaIdentidad";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@CedulaIdentidad", cedulaIdentidad);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string nombre = reader["Nombre"].ToString();
                    string primerApellido = reader["PrimerApellido"].ToString();
                    string segundoApellido = reader["SegundoApellido"].ToString();
                    string cedulaId = reader["CedulaIdentidad"].ToString();
                    string nacionalidad = reader["Nacionalidad"].ToString();
                    string telefono = reader["Telefono"].ToString();
                    string correoElectronico = reader["CorreoElectronico"].ToString();
                    string nombreHotel = reader["NombreHotel"].ToString();
                    string torre = reader["Torre"].ToString();
                    string piso = reader["Piso"].ToString();
                    string numeroHabitacion = reader["NumeroHabitacion"].ToString();

                    string reservacion = $"{nombre}, {primerApellido}, {segundoApellido}, {cedulaId}, {nacionalidad}, {telefono}, {correoElectronico}, {nombreHotel}, {torre}, {piso}, {numeroHabitacion}";
                    Reservaciones.Add(reservacion);
                }

                reader.Close();
            }
        }


        public IActionResult OnPostBuscarPersona(string cedulaIdentidad)
        {
            if (!ValidarCedula(cedulaIdentidad))
            {
                ModelState.AddModelError(string.Empty, "La cédula de identidad no cumple con los requisitos.");
                CargarReservaciones();
                return Page();
            }

            CargarReservaciones(cedulaIdentidad);

           return Page();
        }
        public IActionResult OnGetFiltrarTodos()
        {
            

            CargarReservaciones();

            return Page();
        }

        private bool ValidarCedula(string cedula)
        {
            if (cedula.Length != 11)
            {
                return false;
            }

            return true;
        }

    }
}
