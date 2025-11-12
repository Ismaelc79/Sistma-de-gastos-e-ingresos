# Proyecto Finanzas App - Guia de Testing

Finanzas App es una plataforma para registrar ingresos, gastos, reportes y gestionar el perfil del usuario. El frontend es una SPA en React + Vite con Tailwind, mientras que el backend expone APIs .NET 8 organizadas en Domain/Application/Infrastructure/WebApi. Esta guia consolida las suites unitarias de ambos lados y la documentacion de E2E creada anteriormente.

## Indice

1. [Descripcion del Sistema](#descripcion-del-sistema)
2. [Resumen General](#resumen-general)
3. [Tecnologias de Testing](#tecnologias-de-testing)
4. [Pruebas Unitarias Frontend](#pruebas-unitarias-frontend)
5. [Pruebas Unitarias Backend](#pruebas-unitarias-backend)
6. [Pruebas E2E (End-to-End)](#pruebas-e2e-end-to-end)
7. [Como Ejecutar las Pruebas](#como-ejecutar-las-pruebas)
8. [Pie de Pagina](#pie-de-pagina)

## Descripcion del Sistema

- **Dominio funcional:** dashboard financiero, categorias de gasto/ingreso, transacciones, reportes y perfil.
- **Arquitectura:** SPA (React 19 + Vite + Tailwind) consumiendo servicios REST .NET 8 (Domain, Application, Infrastructure/Dapper y WebApi).
- **Seguridad:** Autenticacion JWT con refresh tokens, rutas protegidas en frontend y `[Authorize]` en el backend.
- **Persistencia:** Capa Dapper + UnitOfWork y value objects (Email, Password, Currency, PhoneNumber) para asegurar consistencia.

## Resumen General

- **Pruebas unitarias Frontend:** 33 tests pasando (`npm run test`).
- **Pruebas unitarias Backend:** 56 tests pasando (`dotnet test`) — Domain (26), Application (16), Infrastructure (4), WebApi (10).
- **Pruebas E2E:** 106 tests pasando (`npm run test:e2e`), cubriendo todo el flujo de usuario final.
- **Cobertura destacada:** UI kit y rutas protegidas, store de auth y helpers, pagina de categorias, capas API del frontend, value objects y entidades .NET, servicios de negocio (auth, categorias, transacciones, reportes, usuarios), servicios de infraestructura (TokenService/DapperContext), controladores WebApi y flujos de usuario extremo a extremo (auth, dashboard, categorias, transacciones, reportes, perfil).

## Tecnologias de Testing

### Frontend (Unitarias)

- **Runner:** Vitest 4 (`threads: false` para evitar timeouts en Windows).
- **Renderizado:** React Testing Library + `@testing-library/user-event`.
- **Asserts:** `@testing-library/jest-dom/vitest`.
- **Ambiente:** `jsdom`, CSS habilitado y `setupTests.ts` que limpia `localStorage/sessionStorage`.
- **Mocking:** `vi.mock` + `vi.hoisted` para aislar Axios, stores y env vars.

### Backend (Unitarias)

- **Framework:** xUnit + coverlet para cada proyecto (`Domain.Tests`, `Application.Tests`, `Infrastructure.Test`, `WebApi.Tests`).
- **Asserts:** FluentAssertions.
- **Mocking:** Moq para `IUnitOfWork`, repositorios y servicios (`ITokenService`, `ICategoryService`, etc.).
- **Bootstrap:** `MapperFactory` usa `ServiceCollection` + `AddAutoMapper` con `MappingProfile`; `UnitOfWorkMockBuilder` centraliza mocks de repositorios y `SaveChanges`.
- **Infra utilitaria:** `Microsoft.Extensions.DependencyInjection/Logging` replica el entorno real, permitiendo levantar AutoMapper y servicios sin WebHost.

### Pruebas E2E

- **Framework:** Playwright (Chromium).
- **Patron:** Page Object Model para mantener los flows.
- **Ejecucion:** soporte para multiples workers, screenshots, video y traces en reintentos.
- **Configuracion:** `playwright.config.ts` inicia el dev server automaticamente y usa `.env.e2e`.

## Pruebas Unitarias Frontend

### Estructura de Archivos

```text
frontend/
  src/
    setupTests.ts
    App.test.tsx
    app/routes/__tests__/ProtectedRoute.test.tsx
    shared/
      lib/__tests__/mock.test.ts
      stores/__tests__/authStore.test.ts
      ui/__tests__/
        Button.test.tsx
        Input.test.tsx
        Card.test.tsx
    features/
      categories/
        api/categories.api.test.ts
        pages/CategoriesPages.test.tsx
      transactions/api/transactions.api.test.ts
      profile/api/profile.api.test.ts
      reports/api/reports.api.test.ts
```

### Cobertura Principal

- **App y temas (3 tests):** aplica o remueve `theme-dark` usando preferencias de usuario y `localStorage`, confirmando que `AppRoutes` se monta.
- **ProtectedRoute (3 tests):** redirecciona a `/login`, respeta sesiones autenticas y `VITE_BYPASS_AUTH`.
- **UI kit (8 tests):** Button (variantes/tamaños/loading), Input (label/helper/error/toggle) y Card (padding + clases).
- **Store de autenticacion (5 tests):** login/register con tokens persistidos, manejo de errores y helpers `setUser`/`clearError`/`logout`.
- **Helpers comunes (3 tests):** `delay`, `readJSON`, `writeJSON` validan fallback ante datos corruptos.
- **Pagina de Categorias (2 tests):** carga inicial via `getCategories`, creacion y reseteo del formulario.
- **Capa API (9 tests):** endpoints de categorias, transacciones, perfil y reportes (incluye calculo de tasa de ahorro mensual).

### Resumen de Configuracion

- Dependencias dev: `vitest`, `jsdom`, `@testing-library/react`, `@testing-library/user-event`, `@testing-library/jest-dom`.
- Scripts nuevos en `frontend/package.json`: `test`, `test:watch`, `test:coverage`.
- `vite.config.ts`: bloque `test` con `environment: 'jsdom'`, `setupFiles`, reporter de cobertura y `threads: false`.
- `tsconfig.app.json`: incluye `vitest/globals` para disponer de `describe/it`.
- `src/setupTests.ts`: registra jest-dom y limpia storage antes de cada suite.
- `Input.tsx`: ahora genera `id` con `useId` y enlaza `label htmlFor`, mejorando A11Y y permitiendo `getByLabelText`.

## Pruebas Unitarias Backend

### Estructura de Archivos

```text
backend-dotnet/
  test/
    Domain.Tests/
      Entities/
        CategoryTests.cs
        TransactionTests.cs
        UserTests.cs
      ValueObjects/
        CurrencyTests.cs
        EmailTests.cs
        PasswordTests.cs
        PhoneNumberTests.cs
    Application.Tests/
      Services/
        AuthServiceTests.cs
        CategoryServiceTests.cs
        ReportServiceTests.cs
        TransactionServiceTests.cs
        UserServiceTests.cs
      TestHelpers/
        MapperFactory.cs
        UnitOfWorkMockBuilder.cs
    Infrastructure.Test/
      DapperContextTests.cs
      TokenServiceTests.cs
    WebApi.Tests/
      AuthControllerTests.cs
      CategoriesControllerTests.cs
      ReportsControllerTests.cs
      TransactionsControllerTests.cs
      UsersControllerTests.cs
```

### Cobertura Principal

- **Value Objects (8 tests):** Email, Currency, PhoneNumber y Password validan formato, normalizacion y hashing/verificacion.
- **Entidades (7 tests):** User (constructor, `UpdateProfile`, `ChangePassword`), Category (`EditCategory` corrige strings vacios) y Transaction (`Update` ahora solo acepta montos > 0 y strings validas).
- **Servicios de Aplicacion (16 tests):**
  - `CategoryService`: crear, filtrar por tipo (case-insensitive), manejar `KeyNotFound` y eliminar con `SaveChanges`.
  - `TransactionService`: crear, actualizar (con guardias), filtrar por fecha y eliminar.
  - `ReportService`: agrega montos/porcentajes con y sin datos, usando historial de 6 meses.
  - `UserService`: actualizar perfil (telefono, moneda, avatar, idioma) y cambiar contraseñas con validaciones de seguridad.
  - `AuthService`: registro con email duplicado, login con credenciales invalidas y refresh token (revocacion y regeneracion).
- **Infrastructure (4 tests):** `TokenService` (claims, validacion y recuperacion de principal en tokens expirados) y `DapperContext` (cadena de conexion configurada).
- **WebApi (10 tests):** controladores de auth, categories, transactions, reports y users validan `ClaimTypes.NameIdentifier`, retornos `Ok/Created/Unauthorized` y delegan correctamente en los servicios.

### Resumen de Configuracion

- Cada proyecto de pruebas referencia la capa correspondiente y usa `FluentAssertions`, `Moq`, `coverlet.collector` y `Microsoft.NET.Test.Sdk`.
- `MapperFactory` crea un `ServiceCollection`, añade logging + `AddAutoMapper` con `MappingProfile` (mismos mapeos que la API).
- `UnitOfWorkMockBuilder` entrega un `IUnitOfWork` doble con repos mockeados y `SaveChangesAsync` preconfigurado.
- Se corrigieron `Category.EditCategory` y `Transaction.Update` para reflejar los escenarios que detectan las pruebas.
- Las suites no dependen de SQL Server ni servicios externos: todo se mockea (repos, conexiones y controller contexts).

## Pruebas E2E (End-to-End)

### Estructura de Archivos

```text
frontend/
  e2e/
    auth.spec.ts                    # Autenticacion (17 tests)
    dashboard.spec.ts               # Dashboard (20 tests)
    categories.spec.ts              # Categorias (15 tests)
    transactions.spec.ts            # Transacciones (26 tests)
    reports.spec.ts                 # Reportes (14 tests)
    profile.spec.ts                 # Perfil (14 tests)
```

> Cada spec sigue Page Object Model (PO) para componentes clave como LoginPage, DashboardPage, CategoryPage, etc.

### Cobertura Principal

- **Autenticacion (17 tests):**
  - Login exitoso, bloqueo por credenciales invalidas, recuerdame.
  - Registro, verificaciones de email, expiracion de codigo.
  - Flujos de refresh token en UI (verificacion grafica).
- **Dashboard (20 tests):**
  - Widgets de balance, grafico de ahorro, cards de KPI.
  - Layout responsivo (desktop/tablet/mobile).
  - Validacion de estados vacios y loaders.
- **Categorias (15 tests):**
  - CRUD completo, filtros e importaciones masivas.
  - Validacion de formularios y errores del API.
- **Transacciones (26 tests):**
  - Creacion/edicion/borrado, filtros por rango de fechas y categoria.
  - Validacion de importaciones CSV y calculo de totales.
  - Regresion de timezone para usuarios internacionales.
- **Reportes (14 tests):**
  - Resumen por tipo, comparativas mensuales, percentiles recomendados.
  - Exportaciones PDF/CSV y accesibilidad (aria labels).
- **Perfil (14 tests):**
  - Edicion de datos personales, cambio de password, preferencias de idioma/tema/moneda.
  - Validaciones de formularios y guardado optimista.
- **Validaciones transversales (EC + Responsive):**
  - Formularios con emails invalidos, campos vacios y mascaras de telefono.
  - Layout mobile/tablet/desktop para las paginas criticas.

### Resumen de Configuracion E2E

- Dependencias: `@playwright/test`, `dotenv-cli`.
- `playwright.config.ts`:
  - Corre solo Chromium para velocidad (pero configurable).
  - Ejecuta web server automaticamente (`npm run dev`).
  - Captura screenshots/video en fallos y adjunta traces en retries.
  - Configura `use.baseURL`, timeouts y workers paralelos.
- Scripts en `package.json`:
  - `test:e2e` (headless)
  - `test:e2e:headed`
  - `test:e2e:ui` (Playwright UI)
  - `test:e2e:debug`
- Global setup verificando que el backend este arriba y limpiando datos temporales.

## Como Ejecutar las Pruebas

### Frontend Unitarias

1. `cd frontend`
2. `npm install`
3. Suite completa: `npm run test`
4. Watch mode: `npm run test:watch`
5. Cobertura: `npm run test:coverage` (text + LCOV + HTML en `/coverage`)

> Las unitarias mockean Axios, asi que no necesitas backend corriendo.

### Backend Unitarias

1. `cd backend-dotnet`
2. `dotnet restore` (si no se ha restaurado).
3. Ejecutar todo: `dotnet test`
4. Ejecutar proyectos individuales:
   - `dotnet test test/Domain.Tests/Domain.Tests.csproj`
   - `dotnet test test/Application.Tests/Application.Tests.csproj`
   - `dotnet test test/Infrastructure.Test/Infrastructure.Test.csproj`
   - `dotnet test test/WebApi.Tests/WebApi.Tests.csproj`

> No se necesita base de datos ni servicios externos; las dependencias se mockean.

### Pruebas E2E

#### Requisitos

- Backend en ejecucion (Playwright arranca el frontend automaticamente).
- `.env.e2e` configurado (ver plantilla `.env.e2e.example`).

#### Comandos

```bash
# Ubicacion
cd frontend

# Suite completa headless
npm run test:e2e

# Headed
npm run test:e2e:headed

# Playwright UI
npm run test:e2e:ui

# Debug paso a paso
npm run test:e2e:debug

# Ejecutar con 4 workers
npx playwright test e2e/ --workers=4

# Test especifico
npx playwright test e2e/auth.spec.ts

# Ver reporte HTML
npx playwright show-report
```

## Pie de Pagina

---
**Ultima actualizacion:** 11/11/2025 — 33 pruebas unitarias frontend pasando, 56 backend, 106 E2E.
