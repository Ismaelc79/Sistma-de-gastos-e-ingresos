# Sistema de Gastos e Ingresos (Finanzas App)

Aplicación fullstack para gestionar finanzas personales/pyme. Incluye frontend en React + Vite y backend .NET 8 (Domain/Application/Infrastructure/WebApi). El objetivo es registrar ingresos/gastos, generar reportes, manejar categorías y perfilar al usuario con autenticación JWT.

## Contenido
- [Tecnologías](#tecnologias)
- [Estructura de carpetas](#estructura-de-carpetas)
- [Requisitos](#requisitos)
- [Configuración y ejecución](#configuracion-y-ejecucion)
- [Pruebas](#pruebas)
- [CI y despliegue](#ci-y-despliegue)
- [Licencia](#licencia)

## Tecnologías

### Frontend
- React 19 + Vite 7
- TypeScript
- TailwindCSS
- Zustand (store auth)
- React Router v7
- Vitest + Testing Library
- Playwright (E2E)

### Backend
- .NET 8 (Domain, Application, Infrastructure, WebApi)
- Dapper
- JWT Auth
- xUnit + Moq + FluentAssertions

## Estructura de carpetas
```
backend-dotnet/
  src/ (Domain, Application, Infrastructure, Presentation/WebApi)
  test/ (Domain.Tests, Application.Tests, Infrastructure.Test, WebApi.Tests)
frontend/
  src/ (features, shared, app)
  e2e/ (Playwright specs + page objects)
.github/workflows/ci.yml
README.md
docs/ (documentación adicional, testing guide, imágenes)
```

## Requisitos
- Node.js 20+
- npm 10+
- .NET 8 SDK
- (Opcional) SQL Server/servicios backend propios si quieres interfaz real

## Configuración y ejecución

### Frontend
```bash
cd frontend
npm install
npm run dev      # inicia en http://localhost:5173
npm run build    # compila producción
npm run preview  # sirve build
```
Variables (crear `.env` y/o `.env.e2e`):
```
VITE_APP_URL=http://localhost:5173
VITE_API_URL=http://localhost:5000/api
VITE_BYPASS_AUTH=false
E2E_BASE_URL=http://localhost:5173      # para Playwright
E2E_TEST_USER_EMAIL=...
E2E_TEST_USER_PASSWORD=...
```

### Backend
```bash
cd backend-dotnet
dotnet restore
dotnet run --project src/Presentation/WebApi/WebApi.csproj
```

## Pruebas

### Frontend
- Unitarias: `npm run test` (Vitest, incluye coverage con `npm run test:coverage`)
- E2E: `npm run test:e2e` (Playwright). Usa `.env.e2e` para credenciales y `E2E_BASE_URL`. Apunta a localhost o producción.

### Backend
- Todas: `dotnet test` (ejecuta Domain/Application/Infrastructure/WebApi tests).

## CI y despliegue
- Workflow GitHub Actions: `.github/workflows/ci.yml`
  - Job frontend: instala dependencias + `npm run test`.
  - Job backend: `dotnet build` + `dotnet test`.
- Deploy frontend en Vercel (`npm run build`).
- Backend puede desplegarse en Azure/App Service/contenerización (no incluido aquí).

## Licencia
Proyecto interno/educativo. Ajusta según tus necesidades. Si reutilizas, cita a los autores originales. Contributions via PRs bienvenidas.
