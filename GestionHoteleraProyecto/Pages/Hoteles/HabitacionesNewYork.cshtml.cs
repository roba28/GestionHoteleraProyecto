using GestionHoteleraProyecto.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class HabitacionesNewYorkModel : PageModel
    {
        public List<Habitacion> HabitacionesDisponibles { get; set; }

        public void OnGet()
        {
            HabitacionesDisponibles = new List<Habitacion>();

            // Lee el archivo de texto del Hotel Continental de New York
            var lines = System.IO.File.ReadAllLines("HotelContinentalNewYork.txt");

            // Itera sobre cada línea y agrega las habitaciones disponibles a la lista
            foreach (var line in lines)
            {
                var data = line.Split(',');
                var nombre = data[0];
                var torre = data[1];
                var piso = data[2];
                var habitacion = data[3];
                var disponibilidad = data[4] == "1";

                if (disponibilidad)
                {
                    HabitacionesDisponibles.Add(new Habitacion
                    {
                        Nombre = nombre,
                        Torre = torre,
                        Piso = piso,
                        NumeroHabitacion = habitacion,
                        Disponibilidad = disponibilidad
                    });
                }
            }
        }
    }
}
