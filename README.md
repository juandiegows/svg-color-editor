# Cambiar el color de una imagen SVG

[**Descargar Instalador**](https://github.com/JuanDiegogit/CambiarColorImagenSVG/files/6469450/ImagenSVG.zip) | [**Leer más**](https://dev.to/juandiego/como-cambiar-el-color-de-una-imagen-svg-en-c-1j5m)

![Cambiar color Imagen SVG](https://user-images.githubusercontent.com/65135568/118011367-8f03ad00-b315-11eb-8920-de40a7f49f3c.png)




Este proyecto permite reemplazar el color de una imagen SVG por otro, y la opción de descargarlo en una imagen.

![image](https://user-images.githubusercontent.com/65135568/117901905-f11ace80-b291-11eb-9c72-e710ba5e67f3.png)

Solo busca la imagen SVG y puede editar color por color

![image](https://user-images.githubusercontent.com/65135568/117901971-1c052280-b292-11eb-895e-00f5c8626dae.png)

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download) para compilar
- [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/download) para ejecutar el binario
- Windows

## Compilar y ejecutar

```bash
dotnet build CambiarColorImagenSVG_.sln -c Release
dotnet run --project Cambiar_Color_Imagen_SVG
```

Para generar un ejecutable autocontenido (no requiere instalar el runtime):

```bash
dotnet publish Cambiar_Color_Imagen_SVG -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## Dependencias

| Paquete | Versión |
| --- | --- |
| [Svg](https://www.nuget.org/packages/Svg) | 3.4.7 |
| [Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms) | 2.0.4.8 |



