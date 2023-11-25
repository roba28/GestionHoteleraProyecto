using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class ReservacionAutomaticaModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string nombre, string primerApellido, string segundoApellido, string cedulaIdentidad, string nacionalidad, string telefono, string correoElectronico, string nombreHotel)
        {
            string connectionString = "Server=ADSP-13207\\MSSQLSERVER01;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";

            if (!string.IsNullOrEmpty(nombreHotel))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Buscar la primera habitación disponible del hotel
                    string primeraHabitacionDisponibleQuery = "SELECT TOP 1 Torre, Piso, NumeroHabitacion FROM Habitaciones " +
                                                               "WHERE Nombre = @NombreHotel AND Disponibilidad = 1 " +
                                                               "ORDER BY Torre, Piso, NumeroHabitacion";

                    using (SqlCommand primeraHabitacionCommand = new SqlCommand(primeraHabitacionDisponibleQuery, connection))
                    {
                        primeraHabitacionCommand.Parameters.AddWithValue("@NombreHotel", nombreHotel);

                        using (SqlDataReader primeraHabitacionReader = primeraHabitacionCommand.ExecuteReader())
                        {
                            if (primeraHabitacionReader.Read())
                            {

                                // Obtener detalles de la primera habitación disponible
                                string torre = primeraHabitacionReader["Torre"].ToString();
                                string piso = primeraHabitacionReader["Piso"].ToString();
                                string numeroHabitacion = primeraHabitacionReader["NumeroHabitacion"].ToString();

                                // Insertar la reserva en la tabla Reservaciones
                                string insertQuery = "INSERT INTO Reservaciones (Nombre, PrimerApellido, SegundoApellido, CedulaIdentidad, Nacionalidad, Telefono, CorreoElectronico, NombreHotel, Torre, Piso, NumeroHabitacion) " +
                                                    "VALUES (@Nombre, @PrimerApellido, @SegundoApellido, @CedulaIdentidad, @Nacionalidad, @Telefono, @CorreoElectronico, @NombreHotel, @Torre, @Piso, @NumeroHabitacion)";
                                
                                primeraHabitacionReader.Close();

                                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@Nombre", nombre);
                                    command.Parameters.AddWithValue("@PrimerApellido", primerApellido);
                                    command.Parameters.AddWithValue("@SegundoApellido", segundoApellido);
                                    command.Parameters.AddWithValue("@CedulaIdentidad", cedulaIdentidad);
                                    command.Parameters.AddWithValue("@Nacionalidad", nacionalidad);
                                    command.Parameters.AddWithValue("@Telefono", telefono);
                                    command.Parameters.AddWithValue("@CorreoElectronico", correoElectronico);
                                    command.Parameters.AddWithValue("@NombreHotel", nombreHotel);
                                    command.Parameters.AddWithValue("@Torre", torre);
                                    command.Parameters.AddWithValue("@Piso", piso);
                                    command.Parameters.AddWithValue("@NumeroHabitacion", numeroHabitacion);

                                    command.ExecuteNonQuery();
                                }

                                // Actualizar disponibilidad de la habitación a 0 (no disponible)
                                string updateDisponibilidadQuery = "UPDATE Habitaciones " +
                                                                   "SET Disponibilidad = 0 " +
                                                                   "WHERE Nombre = @NombreHotel AND Torre = @Torre AND Piso = @Piso AND NumeroHabitacion = @NumeroHabitacion";

                                using (SqlCommand updateCommand = new SqlCommand(updateDisponibilidadQuery, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@NombreHotel", nombreHotel);
                                    updateCommand.Parameters.AddWithValue("@Torre", torre);
                                    updateCommand.Parameters.AddWithValue("@Piso", piso);
                                    updateCommand.Parameters.AddWithValue("@NumeroHabitacion", numeroHabitacion);

                                    updateCommand.ExecuteNonQuery();
                                }
                                TempData["Mensaje"] = "Reserva guardada con éxito";
                                TempData["Tipo"] = "Ok";
                            }
                            else
                            {
                                // No hay habitaciones disponibles en todo ese hotel, muestra una alerta
                                TempData["Mensaje"] = "Lo sentimos, no hay habitaciones disponibles en este momento en el hotel " + nombreHotel;
                                TempData["Tipo"] = "error";
                                return Page();
                            }

                        }
                    }
                }
            }
            return Page();
            //return RedirectToPage("/Hoteles/Reservacion");
        }



    }
}
