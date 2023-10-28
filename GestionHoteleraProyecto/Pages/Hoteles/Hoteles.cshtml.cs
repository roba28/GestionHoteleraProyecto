using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionHoteleraProyecto.Pages.Hoteles
{
    public class HotelesModel : PageModel
    {
        public List<Hotel> Hoteles { get; set; }

        public void OnGet()
        {
            Hoteles = new List<Hotel>();

            // Lee los archivos de texto de cada hotel y agrega la información al listado de hoteles
            string[] archivosHoteles = { "HotelContinentalNewYork.txt", "HotelContinentalRoma.txt", "HotelContinentalMarruecos.txt", "HotelContinentalOsakaTokyo.txt" };

            foreach (var archivo in archivosHoteles)
            {
                var lines = System.IO.File.ReadAllLines(archivo);

                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    var nombre = data[0];
                    var torre = data[1];
                    var piso = data[2];
                    var habitacion = data[3];
                    var disponibilidad = data[4] == "1";

                    Hoteles.Add(new Hotel
                    {
                        Nombre = nombre,
                        Torre = torre,
                        Piso = piso,
                        Habitacion = habitacion,
                        Disponibilidad = disponibilidad
                    });
                }
            }
        }
    }

    public class Hotel
    {
        public string Nombre { get; set; }
        public string Torre { get; set; }
        public string Piso { get; set; }
        public string Habitacion { get; set; }
        public bool Disponibilidad { get; set; }
    }

}
