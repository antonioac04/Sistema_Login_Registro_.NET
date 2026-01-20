# 🔐 Sistema Login & Registro en .NET

Proyecto desarrollado en **.NET** que implementa un sistema básico de **Inicio de Sesión (Login)** y **Registro de usuarios**, ideal como base para aplicaciones web o de escritorio con autenticación.

---

## 📌 Características

✅ Registro de nuevos usuarios  
✅ Login con validación de credenciales  
✅ Gestión de sesiones / autenticación básica  
✅ Validaciones de formulario  
✅ Estructura clara y fácil de extender  
✅ Ideal como plantilla para proyectos reales

---

## 🧰 Tecnologías utilizadas

- **C#**
- **.NET / ASP.NET**
- **Entity Framework** (si aplica)
- **SQL Server / LocalDB** (si aplica)
- **HTML / CSS** (si aplica en frontend)
- **Bootstrap** (si aplica)

> ⚠️ *Nota:* Ajusta esta sección si estás usando una base de datos o framework diferente.

---

## 📂 Estructura del proyecto

Ejemplo de organización típica:

```
Sistema_Login_Registro_.NET/
│
├── Controllers/        # Controladores (si es ASP.NET MVC)
├── Models/             # Modelos y entidades
├── Views/              # Vistas (Razor / UI)
├── Data/               # Contexto de BD / EF
├── wwwroot/            # Archivos estáticos (CSS, JS)
└── Program.cs / Startup.cs
```

---

## ⚙️ Requisitos

Antes de ejecutar el proyecto asegúrate de tener:

- ✅ **Visual Studio 2022** (recomendado)
- ✅ **.NET SDK instalado**
- ✅ (Opcional) **SQL Server** o **LocalDB**
- ✅ Git

---

## 🚀 Instalación y ejecución

1️⃣ Clona el repositorio:

```bash
git clone https://github.com/antonioac04/Sistema_Login_Registro_.NET.git
```

2️⃣ Entra al proyecto:

```bash
cd Sistema_Login_Registro_.NET
```

3️⃣ Abre el proyecto en Visual Studio  
- Archivo → Abrir → Proyecto/Solución (`.sln`)

4️⃣ Ejecuta el proyecto:

▶️ **Ctrl + F5** (sin depurar)  
o  
▶️ **F5** (con depuración)

---

## 🗄️ Configuración de Base de Datos (si aplica)

Si el proyecto usa base de datos, revisa el archivo de configuración:

📌 `appsettings.json`

Ejemplo de cadena de conexión:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=LoginRegistroDB;Trusted_Connection=True;"
}
```

Luego ejecuta migraciones (si usas Entity Framework):

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 👤 Uso del sistema

### ✅ Registro
1. Accede a la pantalla de **Registro**
2. Completa los campos
3. Guarda el usuario

### ✅ Login
1. Accede a la pantalla de **Login**
2. Introduce usuario y contraseña
3. Si son correctos → accede al sistema

---

## 🛡️ Mejoras recomendadas (ideas)

Si quieres llevarlo a nivel profesional podrías agregar:

🔸 Encriptación de contraseñas (BCrypt / Identity)  
🔸 Roles (Admin / User)  
🔸 Recuperación de contraseña  
🔸 JWT Tokens (API)  
🔸 Bloqueo por intentos fallidos  
🔸 Confirmación por email  

---

📌 GitHub: https://github.com/antonioac04

---

⭐ Si este proyecto te sirvió, no olvides darle una estrella al repositorio.
