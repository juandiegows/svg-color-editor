# Changelog

Todos los cambios importantes de **Editor de Colores SVG**.

El formato sigue [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/)
y el versionado sigue [SemVer](https://semver.org/lang/es/).

## [1.1.0] - 2026-07-28

### Añadido

- **Guardar como SVG.** El resultado recoloreado ahora se puede guardar como
  vector, no solo como imagen. Antes la única salida era un PNG, así que el
  archivo dejaba de ser escalable.
- **Reemplazar varios colores a la vez.** Un diálogo nuevo muestra la paleta real
  del archivo y permite armar varias sustituciones (origen → destino) y
  aplicarlas de una sola pasada, con tolerancia configurable.
- **Exportar en lote.** Genera el mismo dibujo en varios tamaños (16 a 1024 px, o
  uno personalizado) en PNG, JPG, BMP o SVG, con fondo transparente o de color.
- **Deshacer y rehacer** (`Ctrl+Z` / `Ctrl+Y`) para los cambios de color y de
  tamaño.
- **Arrastrar y soltar**: se puede soltar un `.svg` sobre la ventana para abrirlo.
- **Zoom de la vista previa**, independiente del tamaño del documento:
  `Ctrl` + rueda del ratón, `Ctrl` + `+` / `-` / `0`, o los botones del panel.
- **Fondo de cuadros** para ver la transparencia real antes de exportar.
- **Tema claro y oscuro**, recordado entre sesiones.
- **Colores recientes**: el selector de color se precarga con los últimos colores
  usados y con los que de verdad tiene la imagen abierta.
- **Atajos de teclado**: `Ctrl+O` abrir, `Ctrl+S` guardar SVG, `Ctrl+E` exportar.

### Cambiado

- **El reemplazo de color ahora cubre todo el SVG.** Antes solo miraba el relleno
  de los elementos `<path>`, por lo que en la mayoría de archivos reales no
  cambiaba casi nada. Ahora recorre el árbol completo y atiende relleno
  (`fill`), contorno (`stroke`) y paradas de degradado (`stop-color`) en
  cualquier elemento.
- **El cuentagotas se ajusta a la paleta del archivo.** El color leído de la
  pantalla pasa por el suavizado del rasterizador y casi nunca coincidía exacto
  con el del SVG, así que "Cambiar" no hacía nada. Ahora se ajusta al color real
  más cercano.
- **Ampliar y reducir el tamaño** mantienen la proporción de la imagen y ya no
  dependen del tamaño de la vista previa.
- Los fallos al abrir, dibujar, guardar o exportar se informan con su causa en
  vez de descartarse en silencio.

### Corregido

- **Maximizar no ocupaba toda la pantalla.** En monitores distintos al principal
  la ventana se quedaba corta o se salía al monitor vecino, y quedaba un margen
  sobrante. Ahora se calcula sobre el monitor donde está la ventana.
- **No se podía redimensionar la ventana**, y al restaurar desde maximizado
  conservaba el tamaño maximizado.
- **Los botones de ampliar y reducir funcionaban de forma intermitente.** Dejaban
  de responder, sin avisar, en cuanto la imagen alcanzaba el tamaño del área
  visible, y ese límite cambiaba al redimensionar la ventana. Además deformaban
  cualquier imagen que no fuera cuadrada.
- **Cancelar el diálogo de descarga guardaba el archivo igual**: se ignoraba el
  resultado del diálogo.
- Un tamaño muy grande escrito a mano podía agotar la memoria al dibujar; ahora
  el lado está acotado.
- Cada cambio de color o tamaño provocaba tres redibujados y un fotograma
  intermedio deformado.
- Se liberan los mapas de bits que reemplaza la vista previa, que antes se
  acumulaban en memoria.

## [1.0.0] - 2021

### Añadido

- Versión inicial: abrir un SVG, sustituir un color por otro, ajustar el tamaño,
  elegir el color de fondo y descargar el resultado como imagen.
- Galería de ejemplos incluida con la aplicación.
