<div align="center">

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:ffffff,50:ff6b8a,100:c0392b&height=200&section=header&text=HospitalSystem&fontSize=60&fontColor=ffffff&fontAlignY=38&desc=Enterprise-grade%20Hospital%20Management%20REST%20API&descAlignY=58&descSize=18&animation=fadeIn" width="100%"/>

<br/>

[![Typing SVG](https://readme-typing-svg.demolab.com?font=JetBrains+Mono&weight=600&size=22&pause=1000&color=ff6b8a&center=true&vCenter=true&width=700&lines=.NET+8+%7C+ASP.NET+Core+Web+API;5+Modules+%7C+33+Endpoints+%7C+5+Entities;Clean+Architecture+%7C+Repository+Pattern;Docker+Ready+%7C+SQL+Server+2022)](https://git.io/typing-svg)

<br/>

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-8.0.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-6.6.2-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)

<br/>

![FluentValidation](https://img.shields.io/badge/FluentValidation-12.1.0-ff6b8a?style=flat-square&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-c0392b?style=flat-square)
![Endpoints](https://img.shields.io/badge/API_Endpoints-33-ff6b8a?style=flat-square)
![Entities](https://img.shields.io/badge/Entities-5-ff6b8a?style=flat-square)
![Source Files](https://img.shields.io/badge/Source_Files-64-ff6b8a?style=flat-square)

</div>

---

## 🏥 Overview

**HospitalSystem** is a production-ready, containerized REST API built with **.NET 8** and **Clean Architecture** principles. It provides complete management of hospital operations — clinics, doctors, employees, patients, and reservations — through a structured, layered API with full validation, global error handling, and Swagger documentation.

Designed for **hospital staff and administrators**, the system enforces strict data integrity, supports rich filtering and pagination across all resources, and is fully deployable via Docker Compose with a single command.

---

## ✨ Features

<div align="center">

| Module | Capabilities |
|:---|:---|
| 🏢 **Clinics** | Full CRUD · Search · Pagination · Location tracking |
| 👨‍⚕️ **Doctors** | Full CRUD · Clinic assignment · Specialty filter · Search |
| 👔 **Employees** | Full CRUD · Role filter · Hire-date filter · Search |
| 🧑‍🤝‍🧑 **Patients** | Full CRUD · MRN lookup · Gender/Age filter · Search |
| 📅 **Reservations** | Create · Delete · Multi-filter · Doctor/Patient views |

</div>

<br/>

- 🔒 **Global Exception Middleware** — consistent error responses across all endpoints
- ✅ **FluentValidation** — request validation on all write operations
- 📄 **Swagger UI** — auto-generated, interactive API docs (Development only)
- 🐳 **Docker Compose** — one-command full-stack deployment with SQL Server 2022
- 📦 **Generic Repository** — `Repository<T>` base with specialized per-entity extensions
- 🔁 **DTO Pattern** — clean separation between API contracts and domain entities
- 📊 **Pagination** — `PagedResult<T>` on every list endpoint with configurable page size

---

## 🏗️ System Architecture

```mermaid
%%{init: {'theme': 'base', 'themeVariables': {
  'primaryColor': '#c0392b',
  'primaryTextColor': '#ffffff',
  'primaryBorderColor': '#ff6b8a',
  'lineColor': '#ff6b8a',
  'secondaryColor': '#7b1a1a',
  'tertiaryColor': '#3d0f0f',
  'clusterBkg': '#3d0f0f',
  'clusterBorder': '#ff6b8a',
  'edgeLabelBackground': '#c0392b',
  'fontFamily': 'JetBrains Mono'
}}}%%
flowchart TB
    subgraph CLIENT["🌐 Client Layer"]
        HTTP["HTTP Client / Swagger UI"]
    end

    subgraph API["🔷 Hospital.Api — Presentation Layer"]
        MW["GlobalExceptionMiddleware"]
        VAL["FluentValidation"]
        subgraph CTRL["Controllers"]
            CC["ClinicController"]
            DC["DoctorController"]
            EC["EmployeeController"]
            PC["PatientController"]
            RC["ReservationController"]
        end
        subgraph SVC["Services"]
            CS["ClinicService"]
            DS["DoctorService"]
            ES["EmployeeService"]
            PS["PatientService"]
            RS["ReservationService"]
        end
    end

    subgraph CORE["🔶 Hospital.Core — Domain Layer"]
        ENT["Entities\nClinic · Doctor · Employee\nPatient · Reservation"]
        DTO["DTOs\nCreate · Update · Response"]
        IFACE["Interfaces\nIRepository · IService"]
        VLDTR["Validators\nClinic · Employee\nPatient · Reservation"]
    end

    subgraph INFRA["🔸 Hospital.Infrastructure — Data Layer"]
        REPO["Repositories\nRepository⟨T⟩\n+ Specialized Repos"]
        CTX["HospitalDbContext\nEF Core 8"]
        MIG["Migrations\nInitialCreate\nAddClinicLocation"]
    end

    subgraph DB["🗄️ SQL Server 2022"]
        TABLES["Clinics · Doctors\nEmployees · Patients · Reservations"]
    end

    HTTP --> MW --> VAL --> CTRL
    CC --> CS
    DC --> DS
    EC --> ES
    PC --> PS
    RC --> RS
    SVC --> IFACE
    IFACE --> REPO
    REPO --> CTX
    CTX --> MIG
    CTX --> DB
    CORE -.->|contracts| API
    CORE -.->|contracts| INFRA
```

---

## 🔄 Request Lifecycle — POST /api/reservation

```mermaid
%%{init: {'theme': 'base', 'themeVariables': {
  'primaryColor': '#c0392b',
  'primaryTextColor': '#ffffff',
  'primaryBorderColor': '#ff6b8a',
  'lineColor': '#ff6b8a',
  'secondaryColor': '#7b1a1a',
  'tertiaryColor': '#3d0f0f',
  'activationBorderColor': '#ff6b8a',
  'activationBkgColor': '#c0392b',
  'sequenceNumberColor': '#ffffff',
  'fontFamily': 'JetBrains Mono'
}}}%%
sequenceDiagram
    autonumber
    actor Client
    participant MW as GlobalException<br/>Middleware
    participant VAL as ReservationCreate<br/>Validator
    participant CTRL as Reservation<br/>Controller
    participant SVC as Reservation<br/>Service
    participant REPO as Reservation<br/>Repository
    participant CTX as HospitalDb<br/>Context
    participant DB as SQL Server 2022

    Client->>MW: POST /api/reservation (JSON body)
    MW->>VAL: Pipe request through FluentValidation
    alt Validation fails
        VAL-->>Client: 400 Bad Request + error details
    end
    VAL->>CTRL: Valid ReservationCreateDto
    CTRL->>SVC: CreateAsync(dto)
    SVC->>REPO: AddAsync(reservation entity)
    REPO->>CTX: DbSet⟨Reservation⟩.AddAsync()
    CTX->>DB: INSERT INTO Reservations (T-SQL)
    DB-->>CTX: Row inserted + generated Id
    CTX-->>REPO: SaveChangesAsync() → OK
    REPO-->>SVC: Saved Reservation entity
    SVC-->>CTRL: ReservationResponseDto
    CTRL-->>Client: 201 CreatedAtAction + Location header
    Note over MW,Client: Any unhandled exception returns<br/>500 JSON error via GlobalExceptionMiddleware
```

---

## 📊 Entity Relationships

```mermaid
%%{init: {'theme': 'base', 'themeVariables': {
  'primaryColor': '#ffe4ef',
  'primaryTextColor': '#5a1a2e',
  'primaryBorderColor': '#f4a7c0',
  'lineColor': '#e8849f',
  'secondaryColor': '#fff0f5',
  'tertiaryColor': '#ffffff',
  'fontFamily': 'JetBrains Mono'
}}}%%
erDiagram
    CLINIC {
        int Id PK
        string Name
        string Address
        string Phone
        string Location
        string Description
    }
    DOCTOR {
        int Id PK
        int EmployeeId FK
        string Specialty
        int ClinicId FK
    }
    EMPLOYEE {
        int Id PK
        string FirstName
        string LastName
        string Email
        string Phone
        string Role
        datetime HireDate
    }
    PATIENT {
        int Id PK
        string FirstName
        string LastName
        datetime DateOfBirth
        string Gender
        string Phone
        string Email
        string Address
        string MedicalRecordNumber UK
    }
    RESERVATION {
        int Id PK
        int DoctorId FK
        int PatientId FK
        int ClinicId FK
        datetime StartTime
        datetime EndTime
        string Status
        string Notes
        datetime CreatedAt
    }

    CLINIC ||--o{ DOCTOR : "hosts"
    CLINIC ||--o{ RESERVATION : "location of"
    EMPLOYEE ||--|| DOCTOR : "is a"
    DOCTOR ||--o{ RESERVATION : "attends"
    PATIENT ||--o{ RESERVATION : "books"
```

---

## 📈 Metrics

<div align="center">

| Metric | Value |
|:---|:---:|
| 📁 Source Files | **64** |
| 🔗 API Endpoints | **33** |
| 🗂️ Domain Entities | **5** |
| 🗃️ DB Migrations | **2** |
| 🧱 Architecture Layers | **3** |
| ✅ Validated Operations | **4 modules** |

</div>

---

## 🛠️ Tech Stack

<div align="center">

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-8.0.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server_2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Docker](https://img.shields.io/badge/Docker_Compose-2496ED?style=for-the-badge&logo=docker&logoColor=white)

![FluentValidation](https://img.shields.io/badge/FluentValidation-12.1.0-ff6b8a?style=for-the-badge)
![Swashbuckle](https://img.shields.io/badge/Swashbuckle_Swagger-6.6.2-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![OpenAPI](https://img.shields.io/badge/OpenAPI-8.0.20-6BA539?style=for-the-badge&logo=openapiinitiative&logoColor=white)

</div>

---

## 📁 Project Structure

```
HospitalSystem/
├── 📄 HospitalSystem.sln
├── 🐳 docker-compose.yml
├── 📄 .env.example
│
├── 🔷 Hospital.Api/                    # Presentation layer
│   ├── Controllers/
│   │   ├── ClinicController.cs
│   │   ├── DoctorController.cs
│   │   ├── EmployeeController.cs
│   │   ├── PatientController.cs
│   │   └── ReservationController.cs
│   ├── Services/                       # Service implementations
│   ├── Middleware/
│   │   └── GlobalExceptionMiddleware.cs
│   ├── Dockerfile
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
│
├── 🔶 Hospital.Core/                   # Domain layer (no dependencies)
│   ├── Entities/
│   │   ├── Clinic.cs · Doctor.cs · Employee.cs
│   │   ├── Patient.cs · Reservation.cs
│   ├── DTOs/
│   │   ├── Common/  (PagedResult, PaginationParams)
│   │   ├── Clinic/ · Doctor/ · Employee/
│   │   └── Patient/ · Reservation/
│   ├── Interfaces/
│   │   ├── Repositories/  (IRepository + 5 specific)
│   │   └── Services/      (5 service interfaces)
│   └── Validators/
│       ├── ClinicCreateValidator.cs
│       ├── EmployeeCreateValidator.cs
│       ├── PatientCreateValidator.cs
│       └── ReservationCreateValidator.cs
│
└── 🔸 Hospital.Infrastructure/         # Data access layer
    ├── Data/
    │   └── HospitalDbContext.cs
    ├── Repositories/
    │   ├── Repository.cs              # Generic base
    │   ├── ClinicRepository.cs · DoctorRepository.cs
    │   ├── EmployeeRepository.cs · PatientRepository.cs
    │   └── ReservationRepository.cs
    ├── Extensions/
    │   └── QueryableExtensions.cs
    └── Migrations/
        ├── 20251114154201_InitialCreate.cs
        └── 20251114181647_AddClinicLocation.cs
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- SQL Server 2022 (or use Docker Compose — recommended)

---

### Option A — Docker Compose (Recommended)

```bash
# 1. Clone the repository
git clone https://github.com/YOUR_USERNAME/hospital-system.git
cd hospital-system

# 2. Set up environment variables
cp .env.example .env
# Edit .env and fill in your DB_SA_PASSWORD and other values

# 3. Start the full stack (API + SQL Server)
docker-compose up --build

# API is available at:
# http://localhost:8080/swagger
```

---

### Option B — Local Development

```bash
# 1. Clone and restore
git clone https://github.com/YOUR_USERNAME/hospital-system.git
cd hospital-system
dotnet restore

# 2. Configure connection string
# Edit Hospital.Api/appsettings.Development.json
# Set your local SQL Server connection string

# 3. Apply migrations
dotnet ef database update \
  --project Hospital.Infrastructure \
  --startup-project Hospital.Api

# 4. Run
cd Hospital.Api
dotnet run

# Swagger UI: https://localhost:5001/swagger
```

---

## 🔌 API Reference

<div align="center">

| Method | Endpoint | Description |
|:---:|:---|:---|
| `GET` | `/api/clinic` | Paged list with optional search |
| `GET` | `/api/clinic/{id}` | Get clinic by ID |
| `POST` | `/api/clinic` | Create clinic |
| `PUT` | `/api/clinic/{id}` | Update clinic |
| `DELETE` | `/api/clinic/{id}` | Delete clinic |
| `GET` | `/api/doctor` | Paged list with clinic & search filters |
| `GET` | `/api/doctor/{id}` | Get doctor by ID |
| `POST` | `/api/doctor` | Create doctor |
| `GET` | `/api/patient/mrn/{mrn}` | Get patient by Medical Record Number |
| `GET` | `/api/reservation/doctor/{doctorId}` | All reservations for a doctor |
| `GET` | `/api/reservation/patient/{patientId}` | All reservations for a patient |
| `POST` | `/api/reservation` | Create reservation |
| `DELETE` | `/api/reservation/{id}` | Cancel reservation |

> Full interactive documentation available at `/swagger` when running in Development mode.

</div>

---

## 🔮 Future Work

- [ ] 🔐 **Authentication & Authorization** — JWT Bearer tokens with role-based access (Admin, Doctor, Staff)
- [ ] 📧 **Notification Service** — Email/SMS confirmation on reservation create/cancel
- [ ] 📊 **Reporting Module** — Occupancy rates, doctor workload, patient statistics
- [ ] 🔁 **Reservation Update Endpoint** — `PUT /api/reservation/{id}` with status transitions
- [ ] 🧪 **Unit & Integration Tests** — xUnit test suite with EF Core in-memory provider
- [ ] 🗂️ **Audit Logging** — Track all create/update/delete operations with timestamps and actor
- [ ] 📦 **CI/CD Pipeline** — GitHub Actions workflow for build, test, and Docker publish
- [ ] 🌍 **Localization** — Multi-language API error messages

---

<div align="center">

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:ffffff,50:ff6b8a,100:c0392b&height=120&section=footer&animation=fadeIn" width="100%"/>

<br/>

**Built with ❤️ using .NET 8 · EF Core · SQL Server · Docker**

[![MIT License](https://img.shields.io/badge/License-MIT-ff6b8a?style=flat-square)](LICENSE)
[![.NET 8](https://img.shields.io/badge/Made_with-.NET_8-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com)

</div>
