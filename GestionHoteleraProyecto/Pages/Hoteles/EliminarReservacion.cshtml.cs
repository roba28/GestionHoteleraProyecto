using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

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

            string pathReservacion = "Reservacion.txt";

            if (System.IO.File.Exists(pathReservacion))
            {
                var lines = System.IO.File.ReadAllLines(pathReservacion).ToList();
                var reservacionEncontrada = false;

                // Filtramos las líneas que no coinciden con la cédula y el hotel
                var nuevasReservaciones = lines.Where(line =>
                {
                    var datosReservacion = line.Split(',');
                    return !(datosReservacion.Length > 3 && datosReservacion[3] == cedulaIdentidad && datosReservacion[7] == hotel);
                }).ToList();

                if (nuevasReservaciones.Count < lines.Count)
                {
                    System.IO.File.WriteAllLines(pathReservacion, nuevasReservaciones);
                    reservacionEncontrada = true;
                }

                if (reservacionEncontrada)
                {
                    CargarReservaciones();
                }
            }

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostEliminarTodasReservaciones(string hotel)
        {
            string pathReservacion = "Reservacion.txt";

            if (System.IO.File.Exists(pathReservacion))
            {
                var lines = System.IO.File.ReadAllLines(pathReservacion).ToList();
                var nuevasReservaciones = lines.Where(line =>
                {
                    var datosReservacion = line.Split(',');
                    return !(datosReservacion.Length > 7 && datosReservacion[7] == hotel);
                }).ToList();

                if (nuevasReservaciones.Count < lines.Count)
                {
                    System.IO.File.WriteAllLines(pathReservacion, nuevasReservaciones);
                }
            }

            CargarReservaciones();
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostEliminarTodasReservacionesPorPersona(string cedulaIdentidad)
        {
            if (!ValidarCedula(cedulaIdentidad))
            {
                ModelState.AddModelError(string.Empty, "La cédula de identidad no cumple con los requisitos.");
                CargarReservaciones();
                return Page();
            }

            string pathReservacion = "Reservacion.txt";

            if (System.IO.File.Exists(pathReservacion))
            {
                var lines = System.IO.File.ReadAllLines(pathReservacion).ToList();
                var nuevasReservaciones = lines.Where(line =>
                {
                    var datosReservacion = line.Split(',');
                    return !(datosReservacion.Length > 3 && datosReservacion[3] == cedulaIdentidad);
                }).ToList();

                if (nuevasReservaciones.Count < lines.Count)
                {
                    System.IO.File.WriteAllLines(pathReservacion, nuevasReservaciones);
                    CargarReservaciones();
                }
            }

            return RedirectToPage("/Index");
        }

        public IActionResult OnPostSalir()
        {
            return RedirectToPage("/Reservacion");
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
