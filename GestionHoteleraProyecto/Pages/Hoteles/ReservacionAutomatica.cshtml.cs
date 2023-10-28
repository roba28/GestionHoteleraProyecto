using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class ReservacionAutomaticaModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string nombre, string primerApellido, string segundoApellido, string cedulaIdentidad, string nacionalidad, string telefono, string correoElectronico, string nombreHotel, string torre, string piso, string numeroHabitacion)
        {
            string path = ""; // Ruta real del archivo del hotel seleccionado
            string[] lines;

            switch (nombreHotel)
            {
                case "Hotel Continental de New York":
                    path = "HotelContinentalNewYork.txt";
                    break;
                case "Hotel Continental de Roma":
                    path = "HotelContinentalRoma.txt";
                    break;
                case "Hotel Continental de Marruecos":
                    path = "HotelContinentalMarruecos.txt";
                    break;
                case "Hotel Continental de Osaka Tokyo":
                    path = "HotelContinentalOsakaTokyo.txt";
                    break;
                default:
                    // Manejar el caso si no se encuentra
                    break;
            }

            if (!string.IsNullOrEmpty(path))
            {
                lines = System.IO.File.ReadAllLines(path);
                bool habitacionEncontrada = false;

                // Buscar y reemplazar el último "1" si se encuentra
                for (int i = lines.Length - 1; i >= 0; i--)
                {
                    if (lines[i].EndsWith(",1"))
                    {
                        lines[i] = lines[i].TrimEnd('1') + '0'; // Reemplazar la última '1' por '0'
                        habitacionEncontrada = true;

                        string[] datosHabitacion = lines[i].Split(',');
                        nombreHotel = datosHabitacion[0];
                        torre = datosHabitacion[1];
                        piso = datosHabitacion[2];
                        numeroHabitacion = datosHabitacion[3];
                        break;
                    }
                }

                System.IO.File.WriteAllLines(path, lines);

                if (habitacionEncontrada)
                {
                    // Agregar la información de la reserva a un archivo de texto separado llamdo reservacio.txt
                    string pathReservacion = "Reservacion.txt";
                    using (StreamWriter sw = new StreamWriter(pathReservacion, true))
                    {
                        sw.Write($"{nombre},");
                        sw.Write($"{primerApellido},");
                        sw.Write($"{segundoApellido},");
                        sw.Write($"{cedulaIdentidad},");
                        sw.Write($"{nacionalidad},");
                        sw.Write($"{telefono},");
                        sw.Write($"{correoElectronico},");
                        sw.Write($"{nombreHotel},");
                        sw.Write($"{torre},");
                        sw.Write($"{piso},");
                        sw.WriteLine($"{numeroHabitacion}");
                    }
                }
            }

            return RedirectToPage("/Index"); // Redirige a la página principal después de guardar la reservación
        }


    }
}
