using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class ReservacionManualModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string nombre, string primerApellido, string segundoApellido, string cedulaIdentidad, string nacionalidad, string telefono, string correoElectronico, string nombreHotel, string torre, string piso, string numeroHabitacion)
        {
            string connectionString = "Server=CRC-LP-0109\\SQLEXPRESS;Database=GestionHotelera;Trusted_Connection=True;TrustServerCertificate=true;";


            if (!string.IsNullOrEmpty(nombreHotel))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar disponibilidad de la habitación
                    string disponibilidadQuery = "SELECT Disponibilidad FROM Habitaciones " +
                                                 "WHERE Nombre = @NombreHotel AND Torre = @Torre AND Piso = @Piso AND NumeroHabitacion = @NumeroHabitacion";

                    using (SqlCommand disponibilidadCommand = new SqlCommand(disponibilidadQuery, connection))
                    {
                        disponibilidadCommand.Parameters.AddWithValue("@NombreHotel", nombreHotel);
                        disponibilidadCommand.Parameters.AddWithValue("@Torre", int.Parse(torre));
                        disponibilidadCommand.Parameters.AddWithValue("@Piso", int.Parse(piso));
                        disponibilidadCommand.Parameters.AddWithValue("@NumeroHabitacion", int.Parse(numeroHabitacion));

                        object disponibilidadResult = disponibilidadCommand.ExecuteScalar();

                        if (disponibilidadResult != null && disponibilidadResult != DBNull.Value)
                        {
                            int disponibilidad = Convert.ToInt32(disponibilidadResult);

                            if (disponibilidad == 1)
                            {
                                // La habitación está disponible, procede con la reservación

                                // Insertar la reserva en la tabla Reservaciones
                                string insertQuery = "INSERT INTO Reservaciones (Nombre, PrimerApellido, SegundoApellido, CedulaIdentidad, Nacionalidad, Telefono, CorreoElectronico, NombreHotel, Torre, Piso, NumeroHabitacion) " +
                                                    "VALUES (@Nombre, @PrimerApellido, @SegundoApellido, @CedulaIdentidad, @Nacionalidad, @Telefono, @CorreoElectronico, @NombreHotel, @Torre, @Piso, @NumeroHabitacion)";

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
                                    command.Parameters.AddWithValue("@Torre", int.Parse(torre));
                                    command.Parameters.AddWithValue("@Piso", int.Parse(piso));
                                    command.Parameters.AddWithValue("@NumeroHabitacion", int.Parse(numeroHabitacion));

                                    command.ExecuteNonQuery();
                                }

                                // Actualizar disponibilidad de la habitación a 0 (no disponible)
                                string updateDisponibilidadQuery = "UPDATE Habitaciones " +
                                                                   "SET Disponibilidad = 0 " +
                                                                   "WHERE Nombre = @NombreHotel AND Torre = @Torre AND Piso = @Piso AND NumeroHabitacion = @NumeroHabitacion";

                                using (SqlCommand updateCommand = new SqlCommand(updateDisponibilidadQuery, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@NombreHotel", nombreHotel);
                                    updateCommand.Parameters.AddWithValue("@Torre", int.Parse(torre));
                                    updateCommand.Parameters.AddWithValue("@Piso", int.Parse(piso));
                                    updateCommand.Parameters.AddWithValue("@NumeroHabitacion", int.Parse(numeroHabitacion));

                                    updateCommand.ExecuteNonQuery();
                                }
                                TempData["Mensaje"] = "Reserva guardada con éxito";
                                TempData["Tipo"] = "Ok";
                            }
                            else
                            {
                                // La habitación no está disponible, muestra una alerta
                                TempData["Mensaje"] = "Lo sentimos, la habitación no está disponible en este momento.";
                                TempData["Mensaje"] = "Lo sentimos, no hay habitaciones disponibles en este momento en el hotel " + nombreHotel;
                                TempData["Tipo"] = "error";
                                return Page();
                            }
                        }
                        else
                        {
                            // Error al obtener la disponibilidad
                        }
                    }
                }
            }


            return RedirectToPage("/Hoteles/Reservacion");
        }

    }
}
