# 📦 BoxBoxApi

API RESTful desarrollada con **ASP.NET Core**, **Entity Framework Core** y **Swagger**, organizada en una arquitectura por capas: Controllers, Repositories, Models y DTOs. 
La API está desplegada en **Azure App Service** y conectada a **Azure SQL Database**, con el manejo de secretos mediante **Key Vault**

---

## 📚 Tecnologías utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- [ASP.NET Core Web API](https://learn.microsoft.com/aspnet/core/web-api/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [Swagger / Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
- [Azure SQL Database](https://learn.microsoft.com/azure/azure-sql/)
- [Azure App Service (para hosting de la API)](https://learn.microsoft.com/azure/app-service/)
- [Azure Key Vault](https://learn.microsoft.com/azure/key-vault/)
- SQL Server compatible

---

## 🌐 Despliegue en Azure

El proyecto completo está desplegado en **Azure App Service**, disponible públicamente en la siguiente URL:

🔗 **[https://boxboxapi.azurewebsites.net](https://boxboxapi.azurewebsites.net)**

Desde esta URL puedes:

- Acceder a todos los endpoints de la API
- Ver la documentación interactiva de Swagger
- Probar peticiones en tiempo real

---

## 🧼 Buenas prácticas

✅ Arquitectura por capas (Controllers, Repositories, Models, DTOs)  
✅ Uso de **Entity Framework Core** con el patrón **Repository**  
✅ Documentación con **Swagger** accesible públicamente  
✅ **Autenticación** implementada con JWT Bearer Tokens  
✅ Separación de secretos con **Azure Key Vault**  
✅ Inyección de dependencias centralizada en `Program.cs`  
✅ Validaciones mediante anotaciones de datos (`[Required]`, `[MaxLength]`, etc.)  


## 🧪 Ejemplos de endpoints

| Método | Ruta                  | Descripción                       | Autenticación |
|--------|------------------------|-----------------------------------|----------------|
| GET    | `/api/users`           | Obtener todos los usuarios        | 🔐 Requiere token |
| GET    | `/api/users/{id}`      | Obtener un usuario por ID         | ❌ Pública        |
| GET    | `/api/users/profile`   | Obtener el perfil del usuario logueado| 🔐 Requiere token |
| POST   | `/api/auth/login`      | Iniciar sesión y obtener JWT      | ❌ Pública        |   
| POST   | `/api/users`           | Crear un nuevo usuario            | ❌ Pública        |
| PUT    | `/api/users/{id}`      | Actualizar un usuario existente   | 🔐 Requiere token |
| DELETE | `/api/users/{id}`      | Eliminar un usuario               | 🔐 Requiere token |

