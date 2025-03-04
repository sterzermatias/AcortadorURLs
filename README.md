# AcortadorURLs
📌  Tecnologías Utilizadas
✅ Lenguaje: C#
✅ Framework: .NET 9
✅ IDE : VS Code
✅ Arquitectura: Clean Architecture + DDD
✅ Patrones de diseño aplicados: Strategy, Repository, Value Object, Factory
✅ Documentación API: Swagger

 Conceptos Aplicados
1️⃣ Domain-Driven Design (DDD)
📌 DDD
Domain-Driven Design (DDD) - Con este enfoque colocamos el dominio del negocio en el centro de la aplicación. Nos enfocamos en modelar las reglas y comportamientos reales de un negocio mediante entidades, value objects y agregados.

🔹 DDD en nuestro Acortador de URLs
✔ Entidades:

Url → Representa una URL acortada con sus propiedades (UrlOriginal, CodigoCorto, Clics, FechaCreacion).
✔ Value Objects:

CodigoURL → Garantiza que el código corto tenga un formato válido (mínimo 6 caracteres).
Ventaja: Centraliza validaciones y evita inconsistencias.
✔ Repositorio (Repository Pattern):

IUrlRepository → Define las operaciones de acceso a datos (ObtenerPorCodigoAsync, ObtenerPorUrlAsync, AgregarAsync).
UrlRepository (implementación en memoria) → Simula una base de datos con un diccionario.

2️⃣ Clean Architecture
Proponemos dividir el código en capas independientes para lograr modularidad y mantenibilidad.

🔹 Organización del proyecto

AcortadorURLs
├── AcortadorURLs.Application
│   ├── AcortadorURLs.Application.csproj
│   ├── DTOs
│   ├── UseCases
│   ├── bin
│   └── obj
├── AcortadorURLs.Core
│   ├── AcortadorURLs.Core.csproj
│   ├── Entities
│   ├── Repositories
│   ├── Services
│   ├── ValueObjects
│   ├── bin
│   └── obj
├── AcortadorURLs.Infrastructure
│   ├── AcortadorURLs.Infrastructure.csproj
│   ├── Persistence
│   ├── bin
│   └── obj
├── AcortadorURLs.Presentation
│   ├── AcortadorURLs.Presentation.csproj
│   ├── AcortadorURLs.Presentation.http
│   ├── Controllers
│   ├── DTOs
│   ├── Program.cs
│   ├── Properties
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── bin
│   └── obj
├── AcortadorURLs.sln
└── README.md

📌 Beneficios de esta estructura:
✅ Separación de responsabilidades (cada capa tiene una función clara).
✅ El dominio no depende de detalles externos (puedes cambiar la base de datos sin modificar el dominio).
✅ Es fácil de probar (podemos hacer unit tests sin depender de la base de datos real).

3️⃣ Design Patterns
🔹 1. Strategy 
📌 ¿Para qué lo usamos?
Para permitir múltiples estrategias de generación de códigos cortos (aleatorio, hexadecimal, personalizado).

✔ Interfaz:

public interface IGeneradorCodigoStrategy
{
    string Generar();
}
✔ Implementaciones:

public class GeneradorCodigoAleatorio : IGeneradorCodigoStrategy { ... }
public class GeneradorCodigoHexadecimal : IGeneradorCodigoStrategy { ... }
✔ Servicio que permite cambiar la estrategia dinámicamente:

public class GeneradorCodigoService
{
    private IGeneradorCodigoStrategy _strategy;
    public void EstablecerEstrategia(IGeneradorCodigoStrategy strategy) { _strategy = strategy; }
    public string GenerarCodigo() => _strategy.Generar();
}

🔹 2. Repository
📌 ¿Para qué lo usamos?
Para desacoplar el acceso a datos del resto de la aplicación.

✔ Interfaz genérica para acceder a URLs:

public interface IUrlRepository
{
    Task<Url?> ObtenerPorCodigoAsync(string codigo);
    Task<Url?> ObtenerPorUrlAsync(string urlOriginal);
    Task AgregarAsync(Url url);
}
✔ Implementación en memoria:

public class UrlRepository : IUrlRepository
{
    private readonly Dictionary<string, Url> _urls = new();
}
🔹 3. Value Object
📌 ¿Para qué lo usamos?
Para encapsular validaciones en objetos inmutables que representan conceptos del dominio.

✔ Ejemplo: CodigoURL

public class CodigoURL
{
    public string Valor { get; }

    public CodigoURL(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor) || valor.Length < 6)
            throw new ArgumentException("El código debe tener al menos 6 caracteres.");

        Valor = valor;
    }
}
🔹 4. Factory
📌 ¿Para qué lo usamos?
Para encapsular la creación de objetos Url, asegurando que se creen con validaciones correctas.

✔ Método de fábrica en AcortarURL:

private async Task<string> CrearYGuardarUrl(string urlOriginal, string codigo)
{
    var url = new Url(urlOriginal, new CodigoURL(codigo));
    await _urlRepository.AgregarAsync(url);
    return codigo;
}


📌  Instalación y Configuración
🔹 1️⃣ Clonar el repositorio
git clone https://github.com/sterzermatias/AcortadorURLs.git
cd AcortadorURLs

🔹 2️⃣ Restaurar dependencias y compilar
dotnet restore
dotnet build
🔹 3️⃣ Ejecutar el proyecto
dotnet run --project AcortadorURLs.Presentation

🚀 Por defecto, el API se ejecutará en:
👉 http://localhost:5080

📌 Para ver la documentación Swagger, abrir en el navegador:
👉 http://localhost:5080/swagger

📌 4️Endpoints de la API
🔹 1️⃣ Acortar una URL (POST)
✅ Descripción: Permite acortar una URL.
✅ Opcionalmente, se puede proporcionar un código personalizado.

📌 Ejemplo de Request (JSON)
{
    "Url": "https://facebook.com",
    "CodigoPersonalizado": "fb"
}

{
    "mensaje": "URL acortada con éxito.",
    "urlCorta": "https://acortar.io/fb"
}

📌 Si la URL ya estaba acortada:
{
    "mensaje": "Esta URL ya fue acortada anteriormente.",
    "urlCorta": "https://acortar.io/fb"
}

📌 Si el código personalizado ya está en uso (409 Conflict)
{
    "mensaje": "El código corto elegido ya está en uso."
}

🔹 2️⃣ Obtener una URL Acortada (GET)
✅ Descripción: Redirige a la URL original a partir del código corto.

📌 Ejemplo de Request
GET http://localhost:5080/api/urls/fb

📌 Ejemplo de Respuesta (302 Found - Redirección)

Location: https://facebook.com

{
    "mensaje": "Código no encontrado."
}

🔹 3️⃣ Cambiar la Estrategia de Generación de Código (POST)
✅ Descripción: Permite cambiar la estrategia de generación de códigos.
✅ Opciones: "aleatorio" o "hexadecimal"

📌 Ejemplo de Request ()
"hexadecimal"


{
    "mensaje": "Estrategia cambiada a hexadecimal."
}

