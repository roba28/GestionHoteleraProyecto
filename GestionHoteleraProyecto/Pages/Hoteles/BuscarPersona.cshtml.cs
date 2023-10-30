using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            string pathReservacion = "Reservacion.txt";

            if (System.IO.File.Exists(pathReservacion))
            {
                Reservaciones = System.IO.File.ReadAllLines(pathReservacion).ToList();
            }

        }
        private void CargarReservaciones(string CedulaIdentidad)
        {
            Reservaciones = new List<string>();
            string pathReservacion = "Reservacion.txt";

            if (System.IO.File.Exists(pathReservacion))
            {
                Reservaciones = System.IO.File.ReadAllLines(pathReservacion).Where(x=>x.Contains(CedulaIdentidad)).ToList();
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
