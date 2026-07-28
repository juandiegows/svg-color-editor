# Ficha de Microsoft Store — Editor de Colores SVG

Textos listos para pegar en Partner Center → *Store listing* (idioma: Español).

---

## Nombre del producto

```
Editor de Colores SVG
```

## Descripción breve

*(máx. 200 caracteres — aparece en resultados de búsqueda)*

```
Cambia los colores de cualquier imagen SVG y exporta el resultado como SVG, PNG, JPG o BMP, en el tamaño que necesites. Sin conexión, sin cuenta.
```

## Descripción

```
Editor de Colores SVG te permite recolorear imágenes vectoriales sin abrir un editor gráfico pesado ni tocar una línea de código.

Abre un archivo SVG, elige el color que quieres reemplazar, elige el nuevo, y listo. El resultado sigue siendo un vector: lo puedes guardar como SVG, o exportarlo como imagen en el tamaño que necesites.

QUÉ PUEDES HACER

• Reemplazar uno o varios colores a la vez, con una paleta que muestra los colores reales del archivo
• Tomar los colores con un selector, directamente desde la imagen
• Guardar el resultado como SVG (sigue siendo vector) o exportarlo como PNG, JPG o BMP
• Exportar en varios tamaños de una sola vez (íconos, redes sociales, lo que necesites)
• Deshacer y rehacer cambios (Ctrl+Z / Ctrl+Y)
• Arrastrar y soltar un archivo SVG sobre la ventana para abrirlo
• Acercar y alejar la vista previa, con fondo de cuadros para revisar la transparencia
• Tema claro u oscuro, con tus últimos colores siempre a mano
• Empezar desde la galería de personajes incluida, sin buscar archivos

GALERÍA INCLUIDA

La aplicación trae 12 personajes de ejemplo listos para editar. Haz clic en cualquiera de la galería y empieza a cambiarle los colores de inmediato.

FUNCIONA SIN CONEXIÓN

No necesita internet. No recolecta datos. No requiere cuenta.
```

## Características del producto

*(viñetas cortas, máx. 200 caracteres cada una)*

```
Reemplaza uno o varios colores de una imagen SVG a la vez
Guarda el resultado como SVG, PNG, JPG o BMP
Exporta en varios tamaños de una sola pasada
Deshacer y rehacer, arrastrar y soltar, zoom y tema oscuro
Galería de 12 personajes de ejemplo incluida
```

## Palabras clave de búsqueda

*(máx. 7)*

```
SVG
editor SVG
cambiar color
recolorear
vectorial
exportar SVG
color
```

## Novedades de esta versión (1.1.0.0)

*(Partner Center → Packages → "What's new in this version", o Store listing si el
mercado no separa el campo. Máx. 1500 caracteres.)*

```
Novedades de la versión 1.1:

• Guardar como SVG: el resultado recoloreado ahora se puede exportar como vector, no solo como imagen.
• Reemplazar varios colores a la vez, eligiendo entre la paleta real de la imagen.
• Exportar en varios tamaños de una sola pasada (íconos, redes sociales, etc.), en SVG, PNG, JPG o BMP.
• Deshacer y rehacer (Ctrl+Z / Ctrl+Y).
• Arrastrar y soltar un archivo SVG sobre la ventana para abrirlo.
• Zoom en la vista previa y fondo de cuadros para revisar la transparencia.
• Tema claro y oscuro, y colores recientes a mano en el selector.
• El reemplazo de color ahora cubre toda la imagen (relleno, contorno y degradados), no solo el relleno de algunas figuras.
• Se corrigieron el maximizado en varios monitores y los botones de ampliar/reducir tamaño, que antes fallaban de forma intermitente.
```

*(Texto completo de todos los cambios en `CHANGELOG.md`, por si hace falta más
detalle en alguna sección.)*

---

## Otras secciones del envío

| Sección | Valor sugerido |
| --- | --- |
| **Pricing and availability** | Gratis. Todos los mercados |
| **Categoría** | Photo & video *(alternativa: Developer tools)* |
| **Age ratings** | Cuestionario IARC: sin contenido generado por usuarios, sin compras, sin datos personales, sin internet → clasificación mínima |
| **Privacy policy URL** | No debería exigirla: la app no recolecta datos ni accede a la red. Si el formulario la pide igual, sirve una página del repo |
| **Support contact info** | Tu correo, o la URL del repo |

## Notas para el equipo de certificación

*(Submission options → Notes for certification. Importante: hay que justificar
`runFullTrust` y explicar cómo probar la app.)*

```
Aplicación de escritorio Win32 (.NET 10 / WinForms) empaquetada en MSIX.

Capacidad runFullTrust: es obligatoria para cualquier aplicación de escritorio
empaquetada. La app la usa para leer archivos SVG y escribir el PNG exportado,
siempre en rutas que el usuario elige mediante los diálogos estándar de Windows.

No requiere cuenta, no usa la red y no recolecta ningún dato.

Cómo probarla:
1. Al abrir, hacer clic en cualquier miniatura del panel "Ejemplos" (derecha).
   La imagen se carga en el centro.
2. Pulsar "Elegir" bajo "Color a reemplazar" y tomar un color de la imagen.
3. Pulsar "Elegir" bajo "Color por reemplazar" y escoger el color nuevo.
4. Pulsar "Cambiar". La imagen se actualiza.
5. "Descargar" guarda el resultado como PNG.
```

## Justificación de runFullTrust

*(Submission options → Restricted capabilities. Campo obligatorio.)*

```
Editor de Colores SVG es una aplicación de escritorio Win32 hecha en .NET 10 con
Windows Forms y empaquetada como MSIX. La capacidad runFullTrust es obligatoria
para cualquier aplicación de escritorio Win32 distribuida en un paquete MSIX: sin
ella el ejecutable no puede iniciarse. No es una capacidad opcional en este tipo
de aplicación.

Se usa exclusivamente para:

1. Leer el archivo SVG que el usuario selecciona mediante el diálogo estándar de
   Windows para abrir archivos (OpenFileDialog).
2. Escribir la imagen PNG resultante en la ruta que el usuario elige mediante el
   diálogo estándar de Windows para guardar archivos (SaveFileDialog).
3. Leer los 12 archivos SVG de ejemplo incluidos dentro del propio paquete, en la
   carpeta "Personajes Ejemplo" del directorio de instalación. Solo lectura.
4. Rasterizar el SVG con las API de GDI+ de System.Drawing para mostrarlo en
   pantalla.

La aplicación no realiza conexiones de red, no ejecuta procesos ni servicios en
segundo plano, no escribe en el registro de Windows, no instala controladores ni
servicios, no lee ni modifica archivos fuera de los que el usuario elige de forma
explícita, y no recolecta ningún dato del usuario.
```

## Respuestas de Properties

| Pregunta | Respuesta |
| --- | --- |
| Category | Photo & video |
| ¿Recolecta información personal? | No |
| Privacy policy URL | No aplica (no recolecta datos ni usa la red) |
| Website | `https://github.com/juandiegows/svg-color-editor` |
| Support contact info | Tu correo, o la URL del repo |
| Product declarations | No marcar accesibilidad: no se ha auditado |

## Respuestas del cuestionario IARC (Age ratings)

Todo **No**: violencia, contenido sexual, lenguaje, drogas, apuestas, miedo,
compras dentro de la app, publicidad, interacción entre usuarios, compartir
información personal, compartir ubicación, acceso libre a internet.

Resultado esperado: la clasificación más baja (3+ / Todos).

## Capturas

En `Empaquetado/Capturas/`, las tres en 1366×768 (formato aceptado por la Store):

| Archivo | Qué muestra |
| --- | --- |
| `01-editor.png` | Vista principal con un personaje cargado |
| `02-galeria.png` | Selección desde la galería |
| `03-personaje.png` | Otro personaje de ejemplo |

## Imágenes: qué va dentro del paquete y qué va en la ficha

Son dos cosas distintas y conviene no mezclarlas, porque `empaquetar.ps1` copia
la carpeta `Assets` entera dentro del `.msix`.

**`Empaquetado/Assets/`** — van dentro del paquete. Las referencia el manifiesto:

| Archivo | Dónde se usa |
| --- | --- |
| `StoreLogo.png` (50×50) | `<Logo>` del manifiesto |
| `Square150x150Logo.png` | Mosaico de la aplicación |
| `Square44x44Logo.png` | Icono de la barra de tareas |

**`Empaquetado/LogosTienda/`** — solo se suben a Partner Center. **No** deben
estar en `Assets/`, o acabarían ocupando espacio dentro del paquete sin que nada
los use:

| Archivo | Casilla en la ficha |
| --- | --- |
| `Poster9x16_720x1080.png` | Store logos → 9:16 Poster art |
| `BoxArt1x1_1080x1080.png` | Store logos → 1:1 Box art |
| `AppTile_300x300.png` | Store display images → 1:1 App tile icon |

Los tres se componen sobre el morado de la barra de la aplicación
(RGB 99, 86, 176) con el logo centrado, en vez de escalar el PNG original de
500×500 hasta llenar el lienzo, que se vería borroso.
