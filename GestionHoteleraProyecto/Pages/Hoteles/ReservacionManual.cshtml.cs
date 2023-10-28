using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class ReservacionManualModel : PageModel
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
                    
                    break;
            }

            if (!string.IsNullOrEmpty(path))
            {
                lines = System.IO.File.ReadAllLines(path);
                bool habitacionEncontrada = false;
                // Encuentra el �ndice de la �ltima l�nea que contiene la cadena buscada
                int lastIndex = -1;
                for (int i = lines.Length - 1; i >= 0; i--)
                {
                    if (lines[i].Contains($"{nombreHotel},{torre},{piso},{numeroHabitacion}") && lines[i].Contains("1"))
                    {
                        lastIndex = i;
                        break;
                    }
                }

                // Reemplaza el �ltimo "1" si se encontr� la l�nea
                if (lastIndex != -1)
                {
                    lines[lastIndex] = lines[lastIndex].Substring(0, lines[lastIndex].LastIndexOf("1")) + "0";
                    habitacionEncontrada = true;
                }


                System.IO.File.WriteAllLines(path, lines);

                if (habitacionEncontrada)
                {
                    // Agrega la informaci�n de la reserva a un archivo de texto separado
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

            return RedirectToPage("/Index"); // Redirige a la p�gina principal despu�s de guardar la reservaci�n
        }
    }
}
