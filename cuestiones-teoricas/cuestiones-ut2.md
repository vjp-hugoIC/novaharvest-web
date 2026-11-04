# Cuestiones UT2 – Proyecto NovaHarvest

## 1. Cliente y servidor

Cuando abro la ruta `/status`, el cliente es el navegador, que envía una petición HTTP y recibe la página HTML para mostrarla. En el servidor se ejecuta la aplicación NovaHarvest, concretamente el código C# de `Program.cs`, que genera la página con la fecha y la hora actuales.

Cuando la aplicación permita reservar productos, la comprobación de que no se puedan reservar más kilos de los disponibles debe realizarse obligatoriamente en el servidor. El servidor es quien controla la disponibilidad real y no debe confiar en las comprobaciones del navegador, ya que el usuario podría modificarlas o evitarlas.

## 2. Estático o dinámico
En NovaHarvest, las páginas `/` y `/about` podrían ser HTML estático si su contenido no cambiase, porque muestran información de presentación del proyecto. En cambio, `/status` debe generarse dinámicamente, ya que muestra la fecha y la hora del servidor y estos datos cambian en cada petición.

Además, NovaHarvest necesitará páginas que tampoco podrían ser estáticas, como una página de productos disponibles, porque las cantidades de excedentes pueden cambiar, y una página para realizar reservas, porque debe comprobar la disponibilidad y registrar cada operación.

## 3. Ejecución en el servidor

Cuando el navegador solicita `http://localhost:8080/status`, la petición llega primero al servicio `web`, que ejecuta nginx y publica el puerto 8080 del equipo. La configuración de `web/nginx.conf` hace que nginx reenvíe la petición al servicio `backend`.

El backend ejecuta la aplicación ASP.NET Core mediante Kestrel, que escucha en el puerto 8080 del contenedor. Kestrel recibe la petición y ejecuta el código C# asociado a `/status` en `Program.cs`. La aplicación genera el HTML y se lo devuelve a nginx, que finalmente lo envía al navegador.

Si se utilizara CGI, el servidor web pondría en marcha un proceso externo para ejecutar el programa que atiende la petición. Con un módulo embebido, el código se ejecutaría dentro del propio servidor web, en lugar de utilizar un servidor de aplicaciones independiente como Kestrel.

## 4. Reverse proxy

En NovaHarvest, cuando accedo mediante Docker a `http://localhost:8080/status`, la cabecera `Server` esperada es `nginx`, porque nginx recibe la petición del navegador y actúa como reverse proxy. Si ejecuto la aplicación directamente con `dotnet run`, la cabecera esperada es `Kestrel`, porque accedo directamente al servidor de ASP.NET Core.

Si detengo el backend con `docker compose stop backend` y solicito `/status` o `/css/site.css`, nginx devuelve un error `502 Bad Gateway`. Esto ocurre porque nginx sigue recibiendo las peticiones, pero no puede comunicarse con el backend para obtener la respuesta.

El servicio `backend` no publica ningún puerto al exterior porque el acceso debe realizarse a través de nginx. Así, Kestrel no queda expuesto directamente y nginx es el único servicio accesible desde el equipo anfitrión.

## 5. nginx

En este proyecto nginx no sirve directamente `site.css` ni `logo.png`, porque su función es actuar como reverse proxy. La configuración de `web/nginx.conf` reenvía todas las peticiones al backend, y es la aplicación ASP.NET Core la que genera las páginas y sirve los archivos estáticos que están dentro de `wwwroot/`.

Si NovaHarvest se publicara en Internet, nginx podría aportar, por ejemplo, balanceo de carga para repartir las peticiones entre varios servidores backend y terminación HTTPS para gestionar las conexiones cifradas. Estas funciones se podrían añadir sin cambiar la función principal de la aplicación.

## 6. Tecnologías

Si la cooperativa pidiera desarrollar el backend con otra tecnología de servidor vista en clase, habría que adaptar principalmente los archivos propios de la aplicación y su construcción. Por ejemplo, cambiarían el código del backend y su `Dockerfile`, porque habría que utilizar el lenguaje, el entorno de ejecución y las herramientas de esa tecnología.

También habría que revisar `compose.yaml` y `web/nginx.conf` si cambiaran el nombre del servicio, el puerto o la forma de comunicarse con el backend. En cambio, podrían mantenerse la estructura general del repositorio, el servicio nginx como reverse proxy, el puerto público 8080 y la idea de gestionar el entorno con Docker Compose. Los archivos estáticos podrían conservarse si la nueva tecnología los sirviera de la misma manera.

## 7. Marcado y código

En NovaHarvest, las páginas se generan en C# desde `Program.cs`, devolviendo cadenas de texto que contienen el marcado HTML. Esta forma de generar HTML tiene algunos inconvenientes. Primero, el código puede resultar difícil de leer y mantener porque se mezclan instrucciones C# con etiquetas HTML. Segundo, es fácil cometer errores al escribir las cadenas, por ejemplo, al olvidar una comilla o cerrar incorrectamente una etiqueta.

En las tres páginas se repite la estructura común: la declaración `DOCTYPE`, el elemento `html` con el idioma español, la información `meta`, el título de la página, el enlace a `/css/site.css`, la cabecera con el logotipo y el menú de navegación, y el pie de página. En el proyecto, esta estructura debe mantenerse igual y solo cambia el contenido principal de cada ruta.

## 8. Docker

Con el entorno de NovaHarvest levantado hay dos imágenes y dos contenedores: uno para nginx, correspondiente al servicio `web`, y otro para la aplicación ASP.NET Core, correspondiente al servicio `backend`. Una imagen es la plantilla con la que se crea un contenedor; un contenedor es una instancia de esa imagen que se está ejecutando.

El `Dockerfile` del backend utiliza dos etapas. En la primera se emplea la imagen del SDK de .NET 10 para compilar la aplicación. En la segunda se utiliza la imagen de ASP.NET Core 10 para ejecutarla. Por eso, la imagen final no necesita incluir el SDK, que solo hace falta durante la compilación.

El archivo `backend/.dockerignore` evita que las carpetas `bin/` y `obj/` se incluyan en el contexto de construcción de la imagen. Sin este archivo, podrían copiarse archivos generados de compilaciones anteriores, aumentando el tamaño y pudiendo introducir contenido innecesario.

## 9. Herramientas

| Herramienta | Uso en el proyecto NovaHarvest |
|---|---|
| Visual Studio Code | Editar el código C# y los archivos de configuración del proyecto. |
| .NET 10 y ASP.NET Core | Desarrollar el backend y generar las páginas dinámicamente. |
| Kestrel | Recibir las peticiones y ejecutar la aplicación ASP.NET Core. |
| nginx | Actuar como reverse proxy y reenviar las peticiones al backend. |
| Docker | Construir imágenes y ejecutar los servicios en contenedores. |
| Docker Compose | Definir y levantar los servicios `web` y `backend` con un único comando. |
| Git | Guardar el historial de cambios mediante commits. |
| GitHub | Alojar el repositorio y compartir el proyecto. |
| curl | Realizar peticiones HTTP y comprobar respuestas y cabeceras. |

## 10. Observación

La hora que muestra `/status` puede ser diferente al ejecutar NovaHarvest con `dotnet run` y al ejecutarlo dentro de Docker. Esto se debe a que la aplicación obtiene la hora del entorno en el que se está ejecutando. El sistema anfitrión puede utilizar la zona horaria local, mientras que el contenedor puede estar configurado en UTC.

Por tanto, aunque se ejecute el mismo código C# de `Program.cs`, la hora mostrada puede variar. Para comprobarlo en el proyecto, se puede abrir `/status` primero con `dotnet run` y después con Docker, y comparar los valores que aparecen.