# 🧪 Sr Full Stack Challenge – N5 Company

Este proyecto es una solución para el challenge técnico de N5 Company, donde se desarrolla un sistema de gestión de permisos utilizando tecnologías modernas tanto en backend como en frontend.

---

## 🧩 Tecnologías Utilizadas

### 🔧 Backend
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Elasticsearch 9.x
- Apache Kafka
- Docker & Docker Compose
- CQRS + Unit of Work + Repository Pattern
- xUnit (Unit & Integration Tests)

### 🎨 Frontend
- ReactJS
- Axios
- Material-UI

---

## 🚀 Funcionalidades

### API REST
- `POST /api/permission` - Solicitar un nuevo permiso
- `PUT /api/permission` - Modificar un permiso existente
- `GET /api/permission` - Listar todos los permisos
- `GET /api/permission/{id}` - Obtener permiso específico
- `POST /api/permissionType` - Crea un nuevo tipo de permiso
- `PUT /api/permissionType` - Modificar un tipo de permiso existente
- `GET /api/permissionType` - Listar todos los tipos de permisos
- `GET /api/permissionType/{id}` - Obtener un tipo de permiso específico

Cada acción:
- Persiste el permiso en SQL Server
- Indexa el permiso en Elasticsearch
- Publica un mensaje en Kafka con el nombre de la operación

---

## 📦 Arquitectura

- **Domain Driven Design (DDD)**: Separación clara de responsabilidades.
- **CQRS**: Comandos y queries separados para escalabilidad.
- **Tests**: Unitarios e integrados para asegurar estabilidad.

---

## 🐳 Levantar el proyecto con Docker

### 1. Requisitos

- Docker
- Docker Compose
- .NET SDK 9 (si lo ejecutás sin contenedores)

### 2. Iniciar servicios

```bash
docker compose up --build -d
