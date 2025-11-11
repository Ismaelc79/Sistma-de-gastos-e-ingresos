# Proyecto Finanzas App - Guia de Testing

Finanzas App es una plataforma web para registrar ingresos, gastos y reportes. El frontend React consume las APIs .NET del backend, aplica layouts responsivos con Tailwind y persiste sesion y tema de usuario mediante Zustand.

## Indice

1. [Descripcion del Sistema](#descripcion-del-sistema)
2. [Resumen General](#resumen-general)
3. [Tecnologia para las Unitarias de Frontend](#tecnologia-para-las-unitarias-de-frontend)
4. [Estructura de Archivos](#estructura-de-archivos)
5. [Cobertura Principal](#cobertura-principal)
6. [Resumen de Configuracion](#resumen-de-configuracion)
7. [Como Ejecutar las Pruebas](#como-ejecutar-las-pruebas)
8. [Pie de Pagina](#pie-de-pagina)

## Descripcion del Sistema

- **Dominio:** gestion de finanzas personales y pymes (dashboard, categorias, transacciones, reportes y perfil).
- **Arquitectura:** SPA en React 19 + Tailwind con rutas protegidas, estado global en Zustand y layout principal `MainLayout`.
- **Integraciones clave:** API REST (auth JWT, categorias, transacciones y reportes), Chart.js para visualizaciones y hooks compartidos en `shared/`.

## Resumen General

- **Pruebas unitarias Frontend:** 33 tests pasando (`npm run test`).
- **Pruebas unitarias Backend:** 0 tests (pendiente en backend-dotnet).
- **Pruebas E2E:** 0 tests (no cubiertas en esta iteracion).
- **Alcance cubierto:** UI kit (Button/Input/Card), App y routing protegido, store de autenticacion, helpers compartidos, pagina de categorias y todas las capas API del frontend.

## Tecnologia para las Unitarias de Frontend

- **Runner:** Vitest 4 con `threads: false` para estabilidad en Windows.
- **Renderizado:** React Testing Library + `@testing-library/user-event`.
- **Asserts extendidos:** `@testing-library/jest-dom/vitest`.
- **Ambiente:** `jsdom` definido en `vite.config.ts` y `setupTests.ts` que limpia `localStorage`/`sessionStorage` y restaura `vi` antes y despues de cada suite.
- **Mocking:** `vi.mock` + `vi.hoisted` para interceptar Axios/stores sin fugas entre archivos de prueba.

## Estructura de Archivos

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

> Cada suite vive junto al modulo que valida (convencion `__tests__` para shared y `*.test.ts(x)` dentro de cada feature).

## Cobertura Principal

- **App y temas (3 tests):** aplica o elimina `theme-dark` segun usuario o `localStorage`, y confirma que `AppRoutes` se monta.
- **ProtectedRoute (3 tests):** redirecciona a `/login`, permite paso con sesion y respeta `VITE_BYPASS_AUTH`.
- **UI kit (8 tests):**
  - Button: variantes, tamanos, `fullWidth` y estado `isLoading`.
  - Input: etiqueta accesible, helper/error y toggle de contrasena.
  - Card: combinaciones de padding y clases personalizadas.
- **Store de autenticacion (5 tests):** login/register persistiendo tokens, manejo de error, logout resiliente y helpers `setUser`/`clearError`.
- **Helpers compartidos (3 tests):** `delay`, `readJSON`, `writeJSON` incluyendo fallbacks ante datos corruptos.
- **Pagina CategoriesPages (2 tests):** carga inicial via `getCategories`, creacion de nuevas categorias y reseteo del formulario.
- **Capa API (9 tests):** validacion de rutas y payloads para categorias, transacciones, perfil y reportes (incluye calculo de tasa de ahorro cuando no hay ingresos).

## Resumen de Configuracion

- Dependencias dev agregadas: `vitest`, `jsdom`, `@testing-library/react`, `@testing-library/user-event`, `@testing-library/jest-dom`.
- Scripts nuevos en `frontend/package.json`: `test`, `test:watch`, `test:coverage`.
- `vite.config.ts`: bloque `test` con `environment: 'jsdom'`, `setupFiles`, cobertura (text/lcov/html) y `threads: false`.
- `tsconfig.app.json`: se anadio `vitest/globals` en `types` para habilitar `describe` y `it`.
- `src/setupTests.ts`: registra jest-dom y reinicia mocks/storage.
- `Input.tsx`: ahora genera `id` con `useId` y enlaza `label htmlFor`, lo que mejora accesibilidad y permite `getByLabelText` en las pruebas.

## Como Ejecutar las Pruebas

1. Ubicacion: `cd frontend`
2. Instalar dependencias (una sola vez): `npm install`
3. Suite completa modo CI: `npm run test`
4. Modo interactivo: `npm run test:watch`
5. Reporte de cobertura (text, lcov, html): `npm run test:coverage`

> Todas las pruebas mockean Axios, por lo que no es necesario levantar el backend para ejecutarlas.

## Pie de Pagina

---
**Ultima actualizacion:** 11/11/2025 -- 33 pruebas unitarias frontend pasando, 0 backend, 0 E2E.
