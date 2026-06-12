# DOCUMENTACIÓN DEL PROYECTO: SUSURROS - PLATAFORMA DE COMENTARIOS ANÓNIMOS

## 📋 ÍNDICE
1. [Descripción General](#descripción-general)
2. [Características Principales](#características-principales)
3. [Arquitectura del Proyecto](#arquitectura-del-proyecto)
4. [Stack Tecnológico](#stack-tecnológico)
5. [Estructura de Directorios](#estructura-de-directorios)
6. [Componentes del Sistema](#componentes-del-sistema)
7. [Flujo de Datos](#flujo-de-datos)
8. [Seguridad y Privacidad](#seguridad-y-privacidad)
9. [API REST](#api-rest)
10. [Guía de Instalación](#guía-de-instalación)

---

## 📌 Descripción General

**Susurros** es una plataforma web moderna diseñada para permitir que los usuarios compartan comentarios e ideas de forma completamente anónima. La aplicación genera identidades únicas y pseudonímicas para cada usuario, manteniendo un equilibrio entre el anonimato y la continuidad de la conversación.

### Propósito
- Facilitar expresión libre y sin filtros
- Proteger la privacidad e identidad de los usuarios
- Crear un espacio de diálogo inclusivo y seguro
- Permitir conversaciones temáticas mediante respuestas anidadas

### Público Objetivo
- Comunidades que valoran la privacidad
- Plataformas de retroalimentación anónima
- Espacios de discusión sin identidad personal
- Aplicaciones educativas o de investigación

---

## ⚡ Características Principales

### 1. **Identidad Anónima Persistente**
   - Cada usuario recibe un nombre único y aleatorio (ej: "Niebla.Bajo.Olvido.42")
   - La identidad se almacena en una cookie cifrada HTTP-only
   - Persiste durante 30 días en el navegador del usuario
   - Sistema imposible de rastrear desde el servidor

### 2. **Comentarios y Respuestas**
   - Los usuarios pueden crear comentarios principales
   - Capacidad de responder a comentarios existentes (threads anidados)
   - Soporte de hasta 4 niveles de profundidad en respuestas
   - Visualización jerárquica con indentación progresiva

### 3. **Multimedia**
   - Carga de imágenes (JPG, PNG, GIF, WebP, BMP)
   - Integración con API de Tenor para búsqueda de GIFs
   - Preview en tiempo real de imágenes seleccionadas
   - Almacenamiento seguro en servidor

### 4. **Privacidad Avanzada**
   - No se registra la dirección IP del usuario
   - Conexión HTTPS obligatoria
   - Cookies con atributos de seguridad (HttpOnly, Secure, SameSite=Lax)
   - HSTS (HTTP Strict Transport Security) habilitado

### 5. **Interfaz Moderna**
   - Diseño oscuro minimalista
   - Interfaz responsiva (mobile-first)
   - Animaciones suaves y transiciones fluidas
   - Paleta de colores coherente (púrpura, blanco, tonos oscuros)

---

## 🏗️ Arquitectura del Proyecto

La aplicación sigue el patrón **MVC (Model-View-Controller)** con una capa de API REST.

```
┌─────────────────────────────────────────────────────────┐
│                    NAVEGADOR DEL USUARIO                │
│                  (HTML + CSS + JavaScript)               │
└────────────────────────┬────────────────────────────────┘
                         │ HTTP/HTTPS
                         ▼
┌─────────────────────────────────────────────────────────┐
│                  ASP.NET CORE 10.0                       │
│  ┌──────────────────────────────────────────────────┐   │
│  │         MIDDLEWARE DE SEGURIDAD                  │   │
│  │  - Anonimización de IP                           │   │
│  │  - Generación de Identidad Persistente           │   │
│  │  - Encriptación de Cookies                       │   │
│  └──────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────┐   │
│  │          CONTROLADORES REST API                  │   │
│  │  - ComentarioController                          │   │
│  │  - HomeController                               │   │
│  └──────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────┐   │
│  │              MODELOS DE DATOS                    │   │
│  │  - Comentario                                    │   │
│  │  - AppDbContext (Entity Framework)               │   │
│  └──────────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────────┐   │
│  │           VISTAS RAZOR (HTML/CSS/JS)            │   │
│  │  - Index.cshtml                                  │   │
│  └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────┐
│           BASE DE DATOS POSTGRESQL (NEON)              │
│  - Tabla: Comentarios                                   │
│  - Relaciones: Comentario-Padre/Hijo                   │
└─────────────────────────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────┐
│            SERVICIOS EXTERNOS                           │
│  - Tenor API: Búsqueda de GIFs                         │
│  - Data Protection: Encriptación de Identidades       │
└─────────────────────────────────────────────────────────┘
```

---

## 🛠️ Stack Tecnológico

### Backend
| Tecnología | Versión | Propósito |
|-----------|---------|----------|
| .NET | 10.0 | Framework web |
| ASP.NET Core | 10.0 | Framework web |
| Entity Framework Core | 10.0.8 | ORM |
| PostgreSQL (Npgsql) | 10.0.2 | Controlador de BD |

### Frontend
| Tecnología | Propósito |
|-----------|----------|
| HTML5 | Estructura |
| CSS3 | Estilos y animaciones |
| JavaScript Vanilla | Interactividad |
| Tenor API | Búsqueda de GIFs |

### Base de Datos
| Sistema | Proveedor | Ubicación |
|--------|----------|----------|
| PostgreSQL | Neon | us-east-1 (AWS) |

### Herramientas de Desarrollo
- Visual Studio Code / Visual Studio
- .NET CLI
- Migraciones EF Core
- User Secrets (para credenciales sensibles)

---

## 📁 Estructura de Directorios

```
ComentariosAnonimos/
├── 📄 Program.cs                          # Configuración principal
├── 📄 ComentariosAnonimos.csproj          # Definición del proyecto
├── 📄 appsettings.json                    # Configuración general
├── 📄 appsettings.Development.json        # Configuración desarrollo
│
├── 📁 Controllers/                        # Controladores MVC/API
│   ├── ComentarioController.cs            # API de comentarios
│   └── HomeController.cs                  # Página de inicio
│
├── 📁 Models/                             # Modelos de datos
│   ├── Comentario.cs                      # Entidad de comentario
│   ├── AppDBContext.cs                    # Contexto EF Core
│   ├── NombreGenerator.cs                 # Generador de identidades
│   └── ErrorViewModel.cs                  # Modelo de errores
│
├── 📁 Views/                              # Vistas Razor
│   ├── ComentarioView/
│   │   └── Index.cshtml                   # Página principal con UI
│   ├── Home/
│   │   ├── Index.cshtml                   # Home
│   │   └── Privacy.cshtml                 # Privacidad
│   └── Shared/
│       ├── _ViewStart.cshtml              # Layout base
│       ├── _ViewImports.cshtml            # Imports globales
│
├── 📁 Properties/
│   └── launchSettings.json                # Configuración de ejecución
│
├── 📁 wwwroot/                            # Archivos estáticos
│   ├── css/
│   │   └── site.css                       # Estilos globales
│   ├── js/
│   │   └── site.js                        # Scripts globales
│   ├── lib/                               # Librerías (Bootstrap, jQuery, etc)
│   └── uploads/                           # Imágenes subidas por usuarios
│
├── 📁 Migrations/                         # Migraciones EF Core
│   ├── 20260606225151_InitialCreate.cs
│   ├── 20260606225151_InitialCreate.Designer.cs
│   └── AppDbContextModelSnapshot.cs
│
└── 📁 bin/Debug/net10.0/                  # Artefactos compilados
```

---

## 🔧 Componentes del Sistema

### 1. **Modelo de Datos: Comentario**

```csharp
public class Comentario
{
    public int Id { get; set; }              // ID único
    public string Texto { get; set; }        // Contenido del comentario
    public string NombreUsuario { get; set; } // Nombre anónimo único
    public int? ParentId { get; set; }       // ID de comentario padre (para respuestas)
    public string? MediaUrl { get; set; }    // URL de imagen/GIF
    public DateTime Fecha { get; set; }      // Timestamp (UTC)
}
```

**Relación Jerárquica:**
- Un comentario sin `ParentId` es un comentario de nivel raíz
- Un comentario con `ParentId` es una respuesta a otro comentario
- Eliminación en cascada: eliminar un comentario elimina todas sus respuestas

### 2. **Generador de Identidades Anónimas**

El clase `NombreGenerator` crea nombres únicos mediante combinación de palabras abstractas:

```
FORMATO: {Palabra1}.{Preposición}.{Palabra2}.{Número}
EJEMPLO: Niebla.Bajo.Olvido.42

Elementos:
├── Palabra1: 25 palabras abstractas (Niebla, Eclipse, Sombra, etc.)
├── Preposición: 10 preposiciones (Del, Sin, Entre, Sobre, etc.)
├── Palabra2: 20 palabras abstractas (Eco, Olvido, Vácío, etc.)
└── Número: Rango 10-99
```

**Ventajas:**
- 25 × 10 × 20 × 90 = 450,000 combinaciones posibles
- Nombres memorables y poéticos
- No vinculados a identidad real
- Fácil de recordar durante la conversación

### 3. **Sistema de Autenticación por Cookie**

```
FLUJO:
1. Usuario accede a la aplicación
2. Middleware verifica si existe cookie "anon_user"
3. Si existe: desencripta y lee nombre de usuario
4. Si no existe: genera nuevo nombre + encripta + crea cookie
5. Cookie tiene:
   - HttpOnly: true (no accesible desde JavaScript)
   - Secure: true (solo HTTPS)
   - SameSite: Lax (protege contra CSRF)
   - MaxAge: 30 días
```

### 4. **Contexto de Base de Datos (Entity Framework Core)**

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Comentario> Comentarios => Set<Comentario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de tabla
        // - Clave primaria con autoincremento
        // - Campos requeridos: NombreUsuario
        // - Campos opcionales: Texto, MediaUrl
        // - Timestamp por defecto: NOW()
        // - Relación de auto-referencia para comentarios padre/hijo
    }
}
```

### 5. **Controlador REST: ComentarioController**

**Rutas disponibles:**

| Método | Ruta | Función |
|--------|------|---------|
| GET | `/comentario` | Página principal |
| GET | `/comentario/list` | Listar todos los comentarios |
| GET | `/comentario/{id}` | Obtener un comentario específico |
| GET | `/comentario/mi-usuario` | Obtener identidad actual |
| POST | `/comentario` | Crear nuevo comentario |
| POST | `/comentario/upload` | Subir imagen |
| PUT | `/comentario/{id}` | Actualizar comentario |
| DELETE | `/comentario/{id}` | Eliminar comentario |

---

## 📊 Flujo de Datos

### Flujo de Creación de Comentario

```
1. USUARIO ESCRIBE COMENTARIO
   └─> Interfaz JavaScript captura texto/imagen

2. USUARIO PRESIONA "ENVIAR"
   └─> Validación en cliente (texto o imagen requeridos)

3. ENVÍO AL SERVIDOR (POST /Comentario)
   Body: { texto, parentId?, mediaUrl? }
   Headers: Content-Type: application/json

4. CONTROLADOR RECIBE SOLICITUD
   ├─> Valida datos
   ├─> Verifica que commentario padre existe (si aplica)
   ├─> Obtiene identidad desde HttpContext.Items
   └─> Crea instancia Comentario

5. ENTITY FRAMEWORK GUARDA EN DB
   ├─> Inserta row en tabla Comentarios
   ├─> BD asigna ID automático
   └─> BD asigna timestamp (NOW())

6. RESPUESTA AL CLIENTE (HTTP 201 Created)
   └─> Retorna objeto Comentario creado en JSON

7. INTERFAZ JAVASCRIPT
   ├─> Recibe comentario creado
   ├─> Genera elemento DOM
   ├─> Renderiza con animación fade-in
   └─> Limpia formulario y cierra panel
```

### Flujo de Carga de Comentarios

```
1. PÁGINA CARGA (window load)

2. JAVASCRIPT EJECUTA cargarIdentidad()
   └─> Fetch GET /comentario/mi-usuario
       └─> Obtiene nombre anónimo + lo muestra en badge

3. JAVASCRIPT EJECUTA cargarComentarios()
   └─> Fetch GET /comentario/list
       └─> Retorna array de todos los comentarios

4. PROCESAMIENTO DE ÁRBOL
   ├─> Identifica comentarios sin parentId (raíces)
   ├─> Agrupa respuestas por parentId (hijos)
   └─> Construye estructura jerárquica recursiva

5. RENDERIZACIÓN DOM
   ├─> Crea elemento para cada comentario
   ├─> Establece nivel de indentación
   ├─> Aplica estilos según nivel (nested, nested-2, nested-3)
   └─> Inserta en DOM con animación

6. VISUALIZACIÓN
   └─> Usuario ve comentarios organizados jerárquicamente
```

### Flujo de Carga de Imagen

```
1. USUARIO PRESIONA "Subir imagen"
   └─> Se abre selector de archivos nativo

2. USUARIO SELECCIONA IMAGEN
   └─> JavaScript captura archivo

3. VALIDACIÓN EN CLIENTE
   ├─> Extensión válida (.jpg, .png, .gif, .webp, .bmp)
   └─> Mostrar preview inmediato

4. ENVÍO AL SERVIDOR (POST /comentario/upload)
   └─> Multipart form-data con archivo

5. SERVIDOR PROCESA
   ├─> Valida extensión nuevamente
   ├─> Genera GUID único para nombre
   ├─> Guarda en wwwroot/uploads/
   └─> Retorna URL pública

6. CLIENTE RECIBE URL
   ├─> Actualiza mediaUrl
   ├─> Muestra imagen en preview
   └─> Listos para enviar comentario con imagen
```

---

## 🔒 Seguridad y Privacidad

### 1. **Privacidad del Usuario**

**Protección de IP:**
```csharp
app.Use(async (context, next) =>
{
    context.Connection.RemoteIpAddress = null;  // ← Bloquea registro de IP
    await next();
});
```
- La dirección IP remota se establece como `null`
- No se registra en logs ni en la base de datos
- Imposible rastrear usuario por dirección IP

**Identidad Persistente Encriptada:**
```
Cookie: anon_user
├── Valor: Base64(Encriptado(NombreUsuario))
├── Algoritmo: Data Protection API (DPAPI)
├── Atributos:
│   ├── HttpOnly: true (no accesible vía JavaScript)
│   ├── Secure: true (solo HTTPS)
│   ├── SameSite: Lax (CSRF protection)
│   └── MaxAge: 30 días
```

### 2. **Seguridad de Transporte**

- **HTTPS Obligatorio:** `app.UseHttpsRedirection();`
- **HSTS:** `app.UseHsts();` - Obliga a navegador a usar HTTPS
- **Header de Seguridad:** Implementado por defecto en ASP.NET Core

### 3. **Validación de Entrada**

**En el Controlador:**
```csharp
// Solo permite extensiones seguras
var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };

// Valida contenido no vacío
if (string.IsNullOrWhiteSpace(request.Texto) && 
    string.IsNullOrWhiteSpace(request.MediaUrl))
    return BadRequest();
```

**En el Cliente:**
```javascript
const allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
// Atributo accept en input file
<input type="file" accept="image/*">
```

### 4. **Prevención de Inyección de Código**

```javascript
function escapeHtml(text) {
    const d = document.createElement('div');
    d.textContent = text;  // ← Escapa caracteres especiales
    return d.innerHTML;
}
```
- Nombres de usuario escapados antes de insertar en DOM
- Evita XSS (Cross-Site Scripting)

### 5. **Almacenamiento de Archivos**

- Archivos guardados con GUID único: `{Guid.NewGuid()}{extension}`
- Imposible adivinar URLs de otras imágenes
- Acceso directo a través de ruta estática

---

## 🔌 API REST

### Endpoint: GET /Comentario/list

**Propósito:** Obtener todos los comentarios

```http
GET /Comentario/list HTTP/1.1
Host: localhost:5000
Accept: application/json
```

**Respuesta (200 OK):**
```json
[
  {
    "id": 1,
    "texto": "Este es mi primer comentario",
    "nombreUsuario": "Niebla.Del.Olvido.42",
    "parentId": null,
    "mediaUrl": null,
    "fecha": "2026-06-06T22:51:51.000Z"
  },
  {
    "id": 2,
    "texto": "Respondiendo al anterior",
    "nombreUsuario": "Eclipse.Entre.Fugacidad.87",
    "parentId": 1,
    "mediaUrl": "/uploads/a1b2c3d4.gif",
    "fecha": "2026-06-06T22:52:15.000Z"
  }
]
```

---

### Endpoint: GET /Comentario/{id}

**Propósito:** Obtener un comentario específico

```http
GET /Comentario/1 HTTP/1.1
Host: localhost:5000
```

**Respuesta (200 OK):**
```json
{
  "id": 1,
  "texto": "Este es mi primer comentario",
  "nombreUsuario": "Niebla.Del.Olvido.42",
  "parentId": null,
  "mediaUrl": null,
  "fecha": "2026-06-06T22:51:51.000Z"
}
```

**Respuesta (404 Not Found):**
```json
{
  "message": "Comentario no encontrado."
}
```

---

### Endpoint: POST /Comentario

**Propósito:** Crear nuevo comentario

```http
POST /Comentario HTTP/1.1
Host: localhost:5000
Content-Type: application/json

{
  "texto": "Mi comentario anónimo",
  "parentId": null,
  "mediaUrl": "/uploads/abc123.jpg"
}
```

**Campos:**
- `texto` (string, opcional): Contenido del comentario
- `parentId` (int?, opcional): ID del comentario padre (para respuestas)
- `mediaUrl` (string?, opcional): URL de imagen/GIF

**Validación:**
- Debe existir texto O mediaUrl (al menos uno)
- Si parentId está presente, debe existir comentario con ese ID

**Respuesta (201 Created):**
```json
{
  "id": 3,
  "texto": "Mi comentario anónimo",
  "nombreUsuario": "Sombra.Bajo.Entropy.56",
  "parentId": null,
  "mediaUrl": "/uploads/abc123.jpg",
  "fecha": "2026-06-12T10:30:00.000Z"
}
```

---

### Endpoint: POST /Comentario/upload

**Propósito:** Subir imagen

```http
POST /Comentario/upload HTTP/1.1
Host: localhost:5000
Content-Type: multipart/form-data; boundary=----Boundary

------Boundary
Content-Disposition: form-data; name="file"; filename="foto.jpg"
Content-Type: image/jpeg

[CONTENIDO BINARIO DEL ARCHIVO]
------Boundary--
```

**Validaciones:**
- Archivo requerido
- Extensiones permitidas: .jpg, .jpeg, .png, .gif, .webp, .bmp

**Respuesta (200 OK):**
```json
{
  "url": "/uploads/a1b2c3d4-e5f6-g7h8.jpg"
}
```

**Respuesta (400 Bad Request):**
```json
{
  "message": "Formato no permitido. Usa: jpg, png, gif, webp."
}
```

---

### Endpoint: GET /Comentario/mi-usuario

**Propósito:** Obtener identidad anónima actual

```http
GET /Comentario/mi-usuario HTTP/1.1
Host: localhost:5000
```

**Respuesta (200 OK):**
```json
{
  "nombreUsuario": "Niebla.Del.Olvido.42"
}
```

---

### Endpoint: PUT /Comentario/{id}

**Propósito:** Actualizar comentario (editar solo texto)

```http
PUT /Comentario/1 HTTP/1.1
Host: localhost:5000
Content-Type: application/json

{
  "texto": "Texto actualizado"
}
```

**Respuesta (200 OK):**
```json
{
  "id": 1,
  "texto": "Texto actualizado",
  "nombreUsuario": "Niebla.Del.Olvido.42",
  "parentId": null,
  "mediaUrl": null,
  "fecha": "2026-06-06T22:51:51.000Z"
}
```

---

### Endpoint: DELETE /Comentario/{id}

**Propósito:** Eliminar comentario

```http
DELETE /Comentario/1 HTTP/1.1
Host: localhost:5000
```

**Nota:** Elimina también todas las respuestas (eliminación en cascada)

**Respuesta (200 OK):**
```json
{
  "message": "Comentario eliminado correctamente"
}
```

---

## 🚀 Guía de Instalación

### Requisitos Previos
- .NET 10.0 SDK o superior
- PostgreSQL (o acceso a instancia Neon)
- Visual Studio Code o Visual Studio
- Node.js (opcional, para tooling)

### Paso 1: Clonar el Repositorio

```bash
git clone <URL_DEL_REPOSITORIO>
cd ComentariosAnonimos
```

### Paso 2: Restaurar Dependencias

```bash
dotnet restore
```

### Paso 3: Configurar Cadena de Conexión

**Opción A: Archivo appsettings.json**
```json
{
  "ConnectionStrings": {
    "Neon": "Host=YOUR_HOST;Database=YOUR_DB;Username=YOUR_USER;Password=YOUR_PASS;SSL Mode=Require"
  },
  "Tenor": {
    "ApiKey": "YOUR_TENOR_API_KEY"
  }
}
```

**Opción B: User Secrets (Recomendado)**
```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Neon" "Host=...;Database=...;..."
dotnet user-secrets set "Tenor:ApiKey" "YOUR_KEY"
```

### Paso 4: Aplicar Migraciones (Primera vez)

```bash
dotnet ef database update
```

### Paso 5: Ejecutar Aplicación

```bash
dotnet run
```

Acceder a: `https://localhost:5001` (o el puerto asignado)

### Paso 6: Compilar para Producción

```bash
dotnet publish -c Release -o ./publish
```

---

## 📈 Casos de Uso

### 1. **Retroalimentación Empresarial Anónima**
- Empleados pueden dar feedback sin temor a represalias
- Gestión de recursos humanos recopila insights valiosos

### 2. **Espacios Comunitarios**
- Comunidades online donde la privacidad es esencial
- Diskusiones sobre temas sensibles sin exposición pública

### 3. **Investigación Académica**
- Recopilación de opiniones para estudios
- Garantía de anonimato completo a participantes

### 4. **Foros de Apoyo**
- Espacios donde personas comparten experiencias delicadas
- Conversaciones significativas sin revolver identidad

### 5. **Sistemas de Sugerencias**
- Usuarios proponen ideas sin presión social
- Comunidad vota y comenta de forma abierta

---

## 🔐 Consideraciones de Seguridad

⚠️ **Para Producción:**

1. **Base de Datos:**
   - Cambiar contraseña por defecto
   - Habilitar SSL/TLS
   - Backups automáticos
   - Monitoreo de acceso

2. **Aplicación:**
   - Implementar rate limiting
   - Añadir CAPTCHA si es necesario
   - Logs de auditoría
   - Monitoreo de errores

3. **Infraestructura:**
   - CDN para archivos estáticos
   - WAF (Web Application Firewall)
   - DDoS protection
   - Load balancing

4. **Cumplimiento:**
   - Política de privacidad clara
   - GDPR compliance
   - Términos de servicio
   - Política de contenido

---

## 📚 Recursos Adicionales

- [Documentación ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Docs](https://docs.microsoft.com/ef/core)
- [PostgreSQL Documentation](https://www.postgresql.org/docs)
- [Tenor API Docs](https://tenor.com/developer/documentation)
- [OWASP Security Guidelines](https://owasp.org)

---

## 👥 Información de Contacto

**Proyecto:** Susurros - Plataforma de Comentarios Anónimos  
**Lenguaje:** C# / ASP.NET Core  
**Versión:** 1.0  
**Fecha de Documentación:** Junio 2026  

---

## 📄 Notas Finales

Esta aplicación demuestra:
✅ Implementación correcta de patrones de seguridad  
✅ Arquitectura escalable y mantenible  
✅ UX/UI moderna y responsiva  
✅ Privacidad como requisito central  
✅ Código limpio y bien documentado  

Ideal para:
- Portfolios de desarrolladores
- Proyectos de startup
- Plataformas comunitarias
- Sistemas empresariales privados

---

**Documento preparado para reclutadores y stakeholders.**
