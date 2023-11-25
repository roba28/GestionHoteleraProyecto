using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class EliminarReservacionModel : PageModel
    {
        public List<string> Reservaciones { get; set; }

        public void OnGet()
        {
            CargarReservaciones();
        }

        public IActionResult OnPostEliminarReservacion(string cedulaIdentidad, string hotel)
        {
            if (!ValidarCedula(cedulaIdentidad))
            {
                ModelState.AddModelError(string.Empty, "La cédula de identidad no cumple con los requisitos.");
                CargarReservaciones();
                return Page();
            }

            string connectionString = "Server=ADSP-13207\\MSSQLSERVER01;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";

            // Consulta SQL para eliminar reservaciones con la cédula e hotel especificados
            string deleteQuery = "DELETE FROM Reservaciones WHERE CedulaIdentidad = @CedulaIdentidad AND NombreHotel = @NombreHotel";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@CedulaIdentidad", cedulaIdentidad);
                    command.Parameters.AddWithValue("@NombreHotel", hotel);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Reservaciones eliminadas correctamente
                        CargarReservaciones();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se encontraron reservaciones para la cédula y hotel especificados.");
                    }
                }
            }

            return Page();
        }


        public IActionResult OnPostEliminarTodasReservaciones(string hotel)
        {
            string connectionString = "Server=ADSP-13207\\MSSQLSERVER01;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";

            // Consulta SQL para eliminar todas las reservaciones
            string deleteQuery = (hotel.ToLower() == "todos")
                ? "DELETE FROM Reservaciones"
                : "DELETE FROM Reservaciones WHERE NombreHotel = @NombreHotel";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    if (hotel.ToLower() != "todos")
                    {
                        command.Parameters.AddWithValue("@NombreHotel", hotel);
                    }

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Reservaciones eliminadas correctamente
                        CargarReservaciones();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se encontraron reservaciones para el hotel especificado.");
                    }
                }
            }

            return Page();
        }



        public IActionResult OnPostEliminarTodasReservacionesPorPersona(string cedulaIdentidad)
        {
            if (!ValidarCedula(cedulaIdentidad))
            {
                ModelState.AddModelError(string.Empty, "La cédula de identidad no cumple con los requisitos.");
                CargarReservaciones();
                return Page();
            }

            string connectionString = "Server=ADSP-13207\\MSSQLSERVER01;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";

            // Consulta SQL para eliminar todas las reservaciones de una persona
            string deleteQuery = "DELETE FROM Reservaciones WHERE CedulaIdentidad = @CedulaIdentidad";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@CedulaIdentidad", cedulaIdentidad);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Reservaciones eliminadas correctamente
                        CargarReservaciones();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se encontraron reservaciones para la cédula especificada.");
                    }
                }
            }

            return Page();
        }


        public IActionResult OnPostSalir()
        {
            return Page();
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
