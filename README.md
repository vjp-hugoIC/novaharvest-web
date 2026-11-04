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

### Flujo de una petición:

```text
+-----------+       HTTP / Port 8080       +--------------------+                    +---------------------+
|           | -------------------------->  |                    |  Proxy Pass (80)   |                     |
| Navegador |                              |    Servicio web    | -----------------> |  Servicio backend   |
|    Web    | <--------------------------  | (Nginx Rev. Proxy) |                    |  (ASP.NET Core 10)  |
+-----------+       Respuesta HTML         +--------------------+                    +---------------------+