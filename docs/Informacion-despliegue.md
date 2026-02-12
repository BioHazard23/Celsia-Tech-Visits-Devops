## Arquitectura de Despliegue (Azure)

La solución está diseñada para operar en un entorno de nube **PaaS (Platform as a Service)** en Microsoft Azure, garantizando alta disponibilidad y menor carga operativa.

![alt text](<Arquitectura de Despliegue (Azure).drawio.png>)

## Componentes de la Solución

### 1. Frontend (Experiencia de Usuario)
Desarrollado en **React 18** con **Vite**, ofrece una interfaz rápida y responsiva.
- **Identidad Corporativa**: Implementación fiel de la marca Celsia (colores, tipografía y logo).
- **Usabilidad**: Formulario inteligente con validaciones en tiempo real para reducir errores de usuario.
- **Funcionalidad**: Dos módulos principales: *Programación de Visitas* y *Consulta de Citas (My Visits)*.

### 2. Backend (Lógica de Negocio)
Construido sobre **ASP.NET Core 8**, proporciona una API RESTful segura y eficiente.
- **Gestión de Datos**: Manejo de creación de clientes, agendamiento y validación de reglas de negocio (horarios, fines de semana).
- **Prevención de Duplicados**: Lógica interna para evitar doble agendamiento en el mismo horario.
- **Documentación Viva**: Swagger UI integrado para facilitar la integración y pruebas.

### 3. Persistencia de Datos
Utiliza **SQLite** gestionado a través de **Entity Framework Core**.
- **Portabilidad**: Base de datos ligera embebida que facilita el despliegue y las pruebas locales.
- **Abstracción**: El uso de EF Core permite migrar transparentemente a Azure SQL Database en un entorno productivo sin cambiar el código de la aplicación.

### 4. Contenerización (Docker)
La solución es totalmente compatible con Docker para garantizar consistencia entre entornos.

![alt text](<Contenerización (Docker).drawio.png>)

## Estrategia de Calidad y DevOps

La solución adhiere a las mejores prácticas de **CI/CD (Integración y Despliegue Continuo)** utilizando **Azure DevOps**:

- **Calidad de Código**: Análisis estático automático con **SonarQube** para detectar deuda técnica, bugs y vulnerabilidades.
- **Pruebas Automatizadas**: Suite de pruebas unitarias (**xUnit**) con cobertura de código, garantizando la estabilidad de las funcionalidades críticas.
- **Pipeline Automatizado**: Compilación, Pruebas y Despliegue sin intervención manual.

## Stack Tecnológico

| Capa | Tecnología | Descripción |
|---|---|---|
| **Frontend** | React 18, Vite | Biblioteca UI moderna para SPAs rápidas. |
| **Estilos** | Tailwind CSS 3.4 | Framework de utilidades para diseño responsivo y branding Celsia. |
| **Backend** | ASP.NET Core 8, C# | Framework de alto rendimiento multiplataforma. |
| **Base de Datos** | SQLite (EF Core) | DB relacional ligera y portable. |
| **QA** | xUnit, SonarQube | Framework de pruebas y análisis de código estático. |
| **DevOps** | Azure DevOps, Docker | Plataforma ALM y motor de contenedores. |

## Modelo de Datos

![alt text](<Modelo de Datos.drawio.png>)

## Beneficios Clave para el Negocio
- ** Eficiencia Operativa**: Reducción significativa de la carga en el Contact Center.
- ** Satisfacción del Cliente**: Capacidad de autogestión inmediata.
- ** Time-to-Market**: Ciclo de vida de desarrollo automatizado que permite liberar nuevas funcionalidades en minutos, no en días.
- ** Escalabilidad de Nube**: Arquitectura lista para escalar horizontalmente en **Microsoft Azure (PaaS)** según la demanda.
