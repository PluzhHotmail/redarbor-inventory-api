# Inventory API – CQRS (.NET)

API REST para gestión de inventario implementada con Clean Architecture y CQRS, ejecutándose en entorno local mediante Docker y Docker Compose.

---

## Tecnologías

- .NET 6
- ASP.NET Core Web API
- CQRS (Commands / Queries)
- SQL Server 2022
- Dapper (Write side)
- Entity Framework Core (Read side)
- JWT Authentication
- Swagger / OpenAPI
- Docker / Docker Compose

---

## Arquitectura

```
Inventory.Api            → Controllers / Endpoints
Inventory.Application    → Commands, Queries, DTOs, Interfaces
Inventory.Domain         → Entidades y reglas de dominio
Inventory.Infrastructure → Persistencia (EF Core / Dapper)
```

Principios aplicados:
- Separación de lectura y escritura (CQRS)
- Soft delete mediante campo Status
- Movimientos de inventario (entrada / salida)
- Dominio desacoplado de infraestructura

---

## Requisitos previos

- Docker
- Docker Compose
- Git

> No es necesario tener .NET ni SQL Server instalados localmente.

---

## Configuración local

### 1. Clonar el repositorio

```bash
git clone https://github.com/PluzhHotmail/redarbor-inventory-api.git
cd redarbor-inventory-api
```

---

### 2. Variables de entorno

Revisar el archivo:

```
docker-compose.yml
```

Ejemplo de configuración de SQL Server:

```yaml
environment:
  SA_PASSWORD: "YourStrong!Passw0rd"
  ACCEPT_EULA: "Y"
```

> Puedes cambiar la contraseña si lo deseas.

---

## Ejecutar la aplicación

Desde la raíz del proyecto:

```bash
docker compose up --build
```

Servicios que se levantan:
- API (`inventory-api`)
- SQL Server (`inventory-sqlserver`)

---

## Acceso a la aplicación

### API
```
http://localhost:5000
```

### Swagger
```
http://localhost:5000/swagger
```

---

## Probar la aplicación

## Postman Collection

El repositorio incluye una colección de **Postman** con los endpoints principales de la API para facilitar las pruebas manuales.

### Ubicación

```
/postman/Inventory.postman_collection.json
```

### Cómo usarla

1. Abrir Postman
2. Importar la colección desde el archivo:
   - `Import` → `File` → seleccionar `Inventory.postman_collection.json`
3. Configurar las variables globales:
   - `baseUrl` → `https://localhost:5001`
   - `token` → ``
4. Ejecutar las peticiones desde la colección

La colección incluye ejemplos de:
- Autenticación (JWT)
- CRUD de categorías
- CRUD de productos
- Movimientos de inventario

---

## Depuración en local

La aplicación puede depurarse en local ejecutando los contenedores con Docker y adjuntando el depurador desde Visual Studio o VS Code al contenedor `inventory-api`.

---

## Consideraciones de diseño

- Los Id se generan en el dominio
- El stock se modifica únicamente mediante movimientos
- Los borrados son lógicos
- Los productos inactivos no se devuelven en consultas
- Cada operación de escritura tiene su propio Command y Handler

---

## Detener la aplicación

```bash
docker compose down -v
```