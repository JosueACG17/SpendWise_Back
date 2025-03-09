# Control de Ingresos y Egresos - SPENDWISE

## Problemática

Muchas personas tienen dificultades para llevar un control adecuado de sus finanzas personales. La falta de herramientas accesibles y fáciles de usar provoca que los usuarios pierdan el seguimiento de sus ingresos y egresos, lo que puede llevar a una mala gestión del dinero.

## Propósito

Desarrollar una aplicación web para ayudar a las personas a llevar un mejor control acerca de sus ingresos y egresos. Queremos que cualquier usuario se pueda registrar y organizar sus gastos de manera fácil y rápida, sin necesidad de tener conocimientos sobre finanzas ni vincular cuentas bancarias.

## Objetivos

1. Permitir a los usuarios registrar sus ingresos y egresos con categorías.
2. Mostrar un historial de transacciones con fechas y montos.
3. Generar gráficas simples para visualizar los gastos.
4. Crear una interfaz fácil de usar y accesible desde cualquier dispositivo.
5. Permitir la exportación de datos en formato PDF.
6. Implementar filtros para buscar transacciones por categoría, fecha o monto.
7. Exportación de datos: jsPDF para generar archivos PDF.

## Integrantes

- **Josue Antonio Chan Gutierrez** (Líder) - FULLSTACK
- **Carlos Josué Oviedo Cisneros** - FULLSTACK 
- **Josias Efrain Kumul Quetzal** – FULLSTACK
- **Bolon Cifuentes Miguel Angel** – FULLSTACK

## Librerías Utilizadas

El proyecto utiliza las siguientes librerías:

- **.NET Core** para la creación de la API y servicios backend.
- **Entity Framework Core** para el acceso a la base de datos y la gestión de migraciones.
- **jsPDF** para la exportación de datos en formato PDF.
- **Chart.js** para la generación de gráficas interactivas.
- **Swashbuckle (Swagger)** para la documentación de la API.
- **AutoMapper** para simplificar el mapeo entre objetos.

## Clonar el Repositorio

Primero, clona el repositorio en tu máquina local utilizando Git:

```bash
git clone https://SpendWise_Back.git
cd SpendWise_Back
```

## Instalar dependencias

Asegúrate de tener las dependencias necesarias para el proyecto. Abre la terminal y ejecuta el siguiente comando para restaurar las dependencias del proyecto:

```bash
dotnet restore
```


## Migraciones

El proyecto utiliza Entity Framework Core para gestionar la base de datos. A continuación, se detallan los pasos para aplicar las migraciones:

Crear una migración: Si has realizado cambios en los modelos, puedes crear una nueva migración ejecutando:
``` bash
dotnet ef migrations add NombreDeLaMigracion
```

Aplicar la migración: Para aplicar las migraciones y actualizar la base de datos, ejecuta:
```bash
dotnet ef database update
```

Revertir una migración: Si necesitas revertir una migración, puedes usar:
```bash
dotnet ef database update NombreDeLaMigracionAnterior
```

## Ejecutar el Proyecto

Sigue estos pasos para correr el proyecto en tu entorno local:
Configurar la base de datos: Asegúrate de que la cadena de conexión a la base de datos esté correctamente configurada en el archivo appsettings.json.

Compilar el proyecto: Ejecuta el siguiente comando para compilar el proyecto:

``` bash
dotnet build
```

Ejecutar la aplicación: Una vez compilado, puedes correr la aplicación con:
```bash
dotnet run
```
