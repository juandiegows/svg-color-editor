using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace Cambiar_Color_Imagen_SVG.Preferencias
{
    /// <summary>
    /// Los ajustes del usuario que sobreviven al cierre de la aplicacion.
    /// Se guardan en AppData, no junto al ejecutable: en la version empaquetada para la
    /// tienda la carpeta de instalacion es de solo lectura.
    /// </summary>
    public class PreferenciasApp
    {
        private const int RecientesMaximos = 12;

        private static readonly JsonSerializerOptions Formato = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        /// <summary>
        /// Los colores que el usuario eligio ultimamente, del mas reciente al mas viejo.
        /// Se guardan como ARGB porque Color no se serializa de forma estable a JSON.
        /// </summary>
        public List<int> ColoresRecientes { get; set; } = new List<int>();

        /// <summary>
        /// True si la interfaz debe usar el tema oscuro.
        /// </summary>
        public bool TemaOscuro { get; set; }

        /// <summary>
        /// True si el lienzo muestra el tablero de cuadros que representa la
        /// transparencia. Por defecto se arranca con el fondo solido: es como se veia la
        /// aplicacion siempre, y el tablero se activa cuando hace falta comprobar la
        /// transparencia antes de exportar.
        /// </summary>
        public bool FondoCuadros { get; set; }

        /// <summary>
        /// La ultima carpeta desde la que se abrio o a la que se exporto.
        /// </summary>
        public string UltimaCarpeta { get; set; }

        private static string RutaArchivo
        {
            get
            {
                string carpeta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "EditorColoresSVG");

                return Path.Combine(carpeta, "preferencias.json");
            }
        }

        /// <summary>
        /// Lee las preferencias guardadas. Si no hay archivo, o esta corrupto, devuelve
        /// las de fabrica: un ajuste ilegible nunca debe impedir que la app abra.
        /// </summary>
        public static PreferenciasApp Cargar()
        {
            try
            {
                string ruta = RutaArchivo;

                if (!File.Exists(ruta))
                {
                    return new PreferenciasApp();
                }

                PreferenciasApp leidas = JsonSerializer.Deserialize<PreferenciasApp>(File.ReadAllText(ruta));
                return leidas ?? new PreferenciasApp();
            }
            catch (Exception)
            {
                return new PreferenciasApp();
            }
        }

        /// <summary>
        /// Escribe las preferencias en disco. Si falla se ignora: perder un ajuste no
        /// justifica interrumpirle el trabajo al usuario.
        /// </summary>
        public void Guardar()
        {
            try
            {
                string ruta = RutaArchivo;
                Directory.CreateDirectory(Path.GetDirectoryName(ruta));
                File.WriteAllText(ruta, JsonSerializer.Serialize(this, Formato));
            }
            catch (Exception)
            {
                // Sin permisos o disco lleno: se sigue trabajando con lo que hay en memoria.
            }
        }

        /// <summary>
        /// Sube un color al principio de los recientes, sin repetirlo.
        /// </summary>
        /// <param name="color">El color que acaba de usar el usuario.</param>
        public void AgregarReciente(Color color)
        {
            int valor = Color.FromArgb(255, color).ToArgb();

            ColoresRecientes.Remove(valor);
            ColoresRecientes.Insert(0, valor);

            while (ColoresRecientes.Count > RecientesMaximos)
            {
                ColoresRecientes.RemoveAt(ColoresRecientes.Count - 1);
            }
        }

        /// <summary>
        /// Los colores recientes ya convertidos.
        /// </summary>
        public List<Color> ObtenerRecientes()
        {
            List<Color> colores = new List<Color>();

            foreach (int valor in ColoresRecientes)
            {
                colores.Add(Color.FromArgb(valor));
            }

            return colores;
        }
    }
}
