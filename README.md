# NovaHarvest - Fase 1

Proyecto de infraestructura web para la gestión de excedentes agrícolas del Valle del Jerte.

---

## 1. Arranque

### Con Docker (Fase 1)
* **Levantar el entorno:** `docker compose up -d --build`
* **Parar el entorno:** `docker compose down`
* **URL de acceso:** `http://localhost:8080/`

## 2. Arquitectura

Flujo de una petición en la Fase 1:

```text
+-----------+       HTTP / Port 8080       +--------------------+                    +------------------+
|           | -------------------------->  |                    |                    |                  |
| Navegador |                              |  Contenedor 'web'  | -----------------> |  index.html      |
|    Web    | <--------------------------  | (Nginx 1.27-alpine)|                    |  (Estatico)      |
+-----------+        Respuesta HTML        +--------------------+                    +------------------+