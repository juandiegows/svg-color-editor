<#
.SYNOPSIS
    Genera el paquete MSIX de Editor de Colores SVG para subir a Microsoft Store.

.DESCRIPTION
    Publica la aplicacion self-contained, le suma el manifiesto y los logos, y
    empaqueta todo con makeappx. El resultado queda en Empaquetado\salida.

    Requiere el Windows SDK:
        winget install --id Microsoft.WindowsSDK.10.0.26100 -e

.PARAMETER Version
    Version del paquete. El cuarto numero tiene que ser 0 para la Store.

.EXAMPLE
    .\empaquetar.ps1
    .\empaquetar.ps1 -Version 1.1.0.0
#>
[CmdletBinding()]
param(
    [ValidatePattern('^\d+\.\d+\.\d+\.0$')]
    [string]$Version = "1.0.0.0"
)

$ErrorActionPreference = "Stop"

$raizEmpaquetado = $PSScriptRoot
$raizRepo = Split-Path $raizEmpaquetado -Parent
$proyecto = Join-Path $raizRepo "Cambiar_Color_Imagen_SVG"
$staging = Join-Path $raizEmpaquetado "staging"
$salida = Join-Path $raizEmpaquetado "salida"
$manifiesto = Join-Path $raizEmpaquetado "AppxManifest.xml"

# --- makeappx ---
$makeappx = Get-ChildItem "C:\Program Files (x86)\Windows Kits\10\bin" -Recurse -Filter "makeappx.exe" -ErrorAction SilentlyContinue |
    Where-Object { $_.DirectoryName -like "*x64*" } |
    Sort-Object FullName -Descending |
    Select-Object -First 1

if (-not $makeappx) {
    throw "No se encontro makeappx.exe. Instala el Windows SDK: winget install --id Microsoft.WindowsSDK.10.0.26100 -e"
}
Write-Host "makeappx: $($makeappx.FullName)"

# --- el manifiesto no puede tener marcadores sin reemplazar ---
$textoManifiesto = Get-Content $manifiesto -Raw
if ($textoManifiesto -match "REEMPLAZAR_") {
    throw "Faltan valores en AppxManifest.xml. Copialos de Partner Center -> Product management -> Product identity."
}

# --- publish self-contained ---
Write-Host "`nPublicando..."
Remove-Item $staging -Recurse -Force -ErrorAction SilentlyContinue
dotnet publish $proyecto -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o $staging
if ($LASTEXITCODE -ne 0) { throw "Fallo el publish." }

# --- fuera simbolos de depuracion: pesan y filtran rutas de compilacion ---
Get-ChildItem $staging -Recurse -Filter "*.pdb" | Remove-Item -Force

# --- manifiesto con la version pedida + logos ---
Write-Host "`nArmando el paquete (version $Version)..."
$textoManifiesto -replace '(<Identity[^>]*?Version=")[^"]*(")', "`${1}$Version`${2}" |
    Set-Content (Join-Path $staging "AppxManifest.xml") -Encoding UTF8

Copy-Item (Join-Path $raizEmpaquetado "Assets") (Join-Path $staging "Assets") -Recurse -Force

# --- empaquetar ---
New-Item -ItemType Directory -Force -Path $salida | Out-Null
$msix = Join-Path $salida "EditorDeColoresSVG_$Version.msix"
Remove-Item $msix -Force -ErrorAction SilentlyContinue

& $makeappx.FullName pack /d $staging /p $msix /o
if ($LASTEXITCODE -ne 0) { throw "Fallo makeappx." }

$mb = [math]::Round((Get-Item $msix).Length / 1MB, 1)
Write-Host "`nListo: $msix  ($mb MB)"
Write-Host "Subelo en Partner Center -> tu producto -> Packages."
