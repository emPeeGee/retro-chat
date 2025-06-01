




Retro.API --> Presentation Layer (Controllers, Swagger)
Retro.Application --> Application Layer (Interfaces, Use Cases, DTOs)
Retro.Infrastructure --> Infrastructure Layer (EF Core, Repositories, Services)
Retro.Domain --> Domain Layer (Entities, Enums, maybe Value Objects)

1. Retro.API (Presentation Layer)
Contains: Controllers, Startup configs (e.g., Swagger, DI), API endpoints

Responsibilities:

Receives HTTP requests

Delegates to Application layer via injected services

No business logic here!

Depends on: Application

▶️ 2. Retro.Application (Application Layer)
Contains: Service interfaces, DTOs, business logic orchestrators

Responsibilities:

Application-specific business rules

Defines use cases like RegisterUserAsync()

Defines contracts (interfaces) for services and DTOs

Depends on: Domain

Used by: API and Infrastructure

▶️ 3. Retro.Infrastructure (Infrastructure Layer)
Contains: EF Core DbContext, repositories, service implementations

Responsibilities:

Implements service interfaces from Application

Handles DB access, I/O, external services

Depends on: Application, Domain

Used by: Injected into API via DI

▶️ 4. Retro.Domain (Domain Layer)
Contains: Core entities (like AppUser), enums, maybe value objects

Responsibilities:

Represents core business models

Pure C# classes — no EF Core, no external dependencies

Used by: All other layers (especially Application and Infrastructure)

Depends on: Nothing (independent)