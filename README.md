# NovaHarvest

Proyecto de infraestructura web para la gestión de excedentes agrícolas del Valle del Jerte.

---

## 1. Arranque

### Con Docker (Fase de Contenedores)

- **Levantar el entorno:** `docker compose up -d --build`
- **Parar el entorno:** `docker compose down`
- **URL de acceso principal:** `http://localhost:8080/`

### Sin Docker (Ejecución Local)

- **Compilar el proyecto:** Entrar en la ruta `backend/NovaHarvest.Web/` y ejecutar `dotnet build`.
- **Ejecutar el servidor local:** `dotnet run`
- **URL de acceso:** La indicada por la consola del framework Kestrel (habitualmente `http://localhost:5000` o la configurada en `launchSettings.json`).

## 2. Arquitectura

El sistema utiliza un contenedor de **Nginx** expuesto en el puerto `8080` que actúa como pasarela o proxy inverso, redirigiendo el tráfico de manera transparente hacia el **backend de .NET Core**.

### Flujo de una petición

```text
+-----------+       HTTP / Port 8080       +--------------------+                    +---------------------+
|           | -------------------------->  |                    |  Proxy Pass (80)   |                     |
| Navegador |                              |    Servicio web    | -----------------> |  Servicio backend   |
|    Web    | <--------------------------  | (Nginx Rev. Proxy) |                    |  (ASP.NET Core 10)  |
+-----------+       Respuesta HTML         +--------------------+                    +---------------------+
```

## 3. Paginas

| URL | Fichero `.cshtml` | `PageModel` (`.cshtml.cs`) |
| --- | --- | --- |
| `/` | `Pages/Index.cshtml` | No tiene |
| `/surplus` | `Pages/Surplus.cshtml` | `Pages/Surplus.cshtml.cs` |
| `/about` | `Pages/About.cshtml` | No tiene |
| `/status` | `Pages/Status.cshtml` | `Pages/Status.cshtml.cs` |
| `/ticket` | `Pages/Ticket.cshtml` | No tiene |

### Ficheros especiales de `Pages/`

- **`_ViewImports.cshtml`**: Usa directivas comunes como `@using` a todas las páginas de su carpeta y subcarpetas para no repetirlas.
- **`_ViewStart.cshtml`**: Ejecuta código común antes de cada página, asigna el layout por defecto.
- **`Shared/_Layout.cshtml`**: Contiene la estructura HTML común de las páginas.
