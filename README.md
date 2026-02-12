# Celsia Tech Visits - DevOps Challenge

A technical visit management system for residential customers, developed as part of the Celsia DevOps Challenge. This application simulates a field service operation for an energy company, allowing customers to schedule technical visits through a web interface.

## Architecture Overview

The solution follows a standard 3-tier architecture:
- **Frontend**: React (Vite) Single Page Application
- **Backend**: C# .NET 8 REST API
- **Database**: SQLite (managed via Entity Framework Core)

See `docs/solution-overview.md` for detailed architecture diagrams.

## Technology Stack

- **Frontend**: React 18, Vite, Tailwind CSS
- **Backend**: ASP.NET Core 8, Entity Framework Core
- **Database**: SQLite
- **QA**: xUnit, SonarQube, Coverlet
- **DevOps**: Azure DevOps (Pipelines), Docker
- **Cloud**: Microsoft Azure (PaaS)

## Prerequisites

- .NET SDK 8.0
- Node.js 18+
- Docker (Optional)

## Getting Started

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd backend/Celsia.TechVisits.Api
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

   The API will be available at `http://localhost:5157`.
   Swagger documentation: `http://localhost:5157/swagger`.

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm run dev
   ```

   Access the application at `http://localhost:5173`.

### Docker Execution

To run the entire stack using Docker Compose:

```bash
docker-compose up --build
```

- Frontend: `http://localhost:3000`
- Backend: `http://localhost:5157`

## Features

- **Schedule Visit**: Customers can schedule technical visits by providing their NIC, name, preferred date, and time slot.
- **My Visits**: Customers can look up their scheduled appointments using their NIC.
- **Form Validation**: Client-side validation ensures data integrity (e.g., valid NIC format, no past dates, no weekends).
- **Automatic Customer Management**: The system automatically links appointments to existing customers or creates new ones as needed.
- **Duplicate Prevention**: Prevents scheduling duplicate visits for the same time slot.

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/customers` | List all customers |
| GET | `/api/customers/{id}` | Get customer details by ID |
| GET | `/api/customers/nic/{nic}` | Get customer details by NIC |
| POST | `/api/customers` | Create a new customer |
| GET | `/api/appointments` | List all appointments |
| GET | `/api/appointments/nic/{nic}` | Get appointments filtered by customer NIC |
| POST | `/api/appointments` | Schedule a new appointment |

## Project Structure

- `frontend/`: React source code and configuration.
- `backend/`: .NET 8 API solution and project files.
- `docs/`: Project documentation including backlog and architecture diagrams.
- `pipelines/`: Azure DevOps CI/CD pipeline definitions.

## Testing and Quality

The project includes unit tests for the backend API using xUnit and SonarQube for static analysis.

To run tests locally:
```bash
cd backend/Celsia.TechVisits.Api.Tests
dotnet test
```

## Known Limitations

- The database uses SQLite for simplicity and portability; for production, Azure SQL Database is recommended.
- Authentication and Authorization are not implemented in this version.
- API URL is hardcoded in the frontend configuration.

## Author

Developed as part of the Celsia DevOps Challenge - Technical Assessment.
