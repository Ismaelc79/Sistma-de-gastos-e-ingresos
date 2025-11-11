# Proyecto Finanzas App - Guia de Testing

Finanzas App es una plataforma web para registrar ingresos, gastos y reportes. El frontend React consume las APIs .NET del backend, aplica layouts responsivos con Tailwind y persiste sesion y tema de usuario mediante Zustand.

## Indice

1. [Descripcion del Sistema](#descripcion-del-sistema)
2. [Resumen General](#resumen-general)
3. [Tecnologias de Testing](#tecnologias-de-testing)
4. [Pruebas Unitarias Frontend](#pruebas-unitarias-frontend)
5. [Pruebas E2E (End-to-End)](#pruebas-e2e-end-to-end)
6. [Como Ejecutar las Pruebas](#como-ejecutar-las-pruebas)
7. [Pie de Pagina](#pie-de-pagina)

## Descripcion del Sistema

- **Dominio:** gestion de finanzas personales y pymes (dashboard, categorias, transacciones, reportes y perfil).
- **Arquitectura:** SPA en React 19 + Tailwind con rutas protegidas, estado global en Zustand y layout principal `MainLayout`.
- **Integraciones clave:** API REST (auth JWT, categorias, transacciones y reportes), Chart.js para visualizaciones y hooks compartidos en `shared/`.

## Resumen General

- **Pruebas unitarias Frontend:** 33 tests pasando (`npm run test`).
- **Pruebas unitarias Backend:** 0 tests (pendiente en backend-dotnet).
- **Pruebas E2E:** 106 tests pasando (`npm run test:e2e`).
- **Alcance cubierto:** UI kit (Button/Input/Card), App y routing protegido, store de autenticacion, helpers compartidos, pagina de categorias, todas las capas API del frontend, y flujos completos E2E de todas las funcionalidades.

## Tecnologias de Testing

### Pruebas Unitarias

- **Runner:** Vitest 4 con `threads: false` para estabilidad en Windows.
- **Renderizado:** React Testing Library + `@testing-library/user-event`.
- **Asserts extendidos:** `@testing-library/jest-dom/vitest`.
- **Ambiente:** `jsdom` definido en `vite.config.ts` y `setupTests.ts` que limpia `localStorage`/`sessionStorage` y restaura `vi` antes y despues de cada suite.
- **Mocking:** `vi.mock` + `vi.hoisted` para interceptar Axios/stores sin fugas entre archivos de prueba.

### Pruebas E2E

- **Framework:** Playwright con Chromium
- **Patron:** Page Object Model para mejor mantenibilidad
- **Configuracion:** Ejecucion paralela con soporte para multiples workers
- **Capturas:** Screenshots automaticos en fallos
- **Reportes:** HTML report con detalles de ejecucion

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

> Cada suite vive junto al modulo que valida (convencion `__tests__` para shared y `*.test.ts(x)` dentro de cada feature).

### Cobertura Principal

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

### Resumen de Configuracion

- Dependencias dev agregadas: `vitest`, `jsdom`, `@testing-library/react`, `@testing-library/user-event`, `@testing-library/jest-dom`.
- Scripts nuevos en `frontend/package.json`: `test`, `test:watch`, `test:coverage`.
- `vite.config.ts`: bloque `test` con `environment: 'jsdom'`, `setupFiles`, cobertura (text/lcov/html) y `threads: false`.
- `tsconfig.app.json`: se anadio `vitest/globals` en `types` para habilitar `describe` y `it`.
- `src/setupTests.ts`: registra jest-dom y reinicia mocks/storage.
- `Input.tsx`: ahora genera `id` con `useId` y enlaza `label htmlFor`, lo que mejora accesibilidad y permite `getByLabelText` en las pruebas.

## Pruebas E2E (End-to-End)

### Estructura de Archivos E2E

```text
frontend/
  e2e/
    auth.spec.ts                    # Autenticacion (17 tests)
    dashboard.spec.ts               # Dashboard (20 tests)
    categories.spec.ts              # Categorias (15 tests)
    transactions.spec.ts            # Transacciones (26 tests)
    reports.spec.ts                 # Reportes (22 tests)
    profile.spec.ts                 # Perfil (20 tests)
    global-setup.ts                 # Configuracion global
    global-teardown.ts              # Limpieza global
    page-objects/
      auth.page.ts                  # Page Object de autenticacion
      dashboard.page.ts             # Page Object de dashboard
      categories.page.ts            # Page Object de categorias
      transactions.page.ts          # Page Object de transacciones
      reports.page.ts               # Page Object de reportes
      profile.page.ts               # Page Object de perfil
  playwright.config.ts              # Configuracion de Playwright
```

### Cobertura Principal E2E

#### Autenticacion (17 tests)

- **Login Page (7 tests):**
  - Display de formulario con todos los elementos
  - Validacion de email vacio e invalido
  - Validacion de password vacio
  - Toggle de visibilidad de password
  - Navegacion a registro
  - Link de forgot password
- **Register Page (5 tests):**
  - Display de formulario de registro
  - Validacion de campos vacios
  - Validacion de email invalido
  - Navegacion a login
  - Toggle de visibilidad de password
- **Protected Routes (5 tests):**
  - Redireccion a login sin autenticacion
  - Proteccion de rutas: dashboard, categories, transactions, reports, profile

#### Dashboard (20 tests)

- **Visualizacion (8 tests):**
  - Display de titulo y descripcion
  - Tarjetas de estadisticas (Balance, Income, Expenses, Savings Rate)
  - Seccion de transacciones recientes
  - Seccion de gastos por categoria
- **Filtros de Periodo (7 tests):**
  - Botones de filtro (Dia, Semana, Mes, Acumulado)
  - Seleccion de cada periodo
  - Display de balance por periodo
  - Visualizacion de ingresos, gastos y balance
- **Navegacion (1 test):**
  - Link a historico de transacciones
- **Responsive Design (4 tests):**
  - Vista mobile (375x667)
  - Vista tablet (768x1024)

#### Categorias (15 tests)

- **Visualizacion (4 tests):**
  - Display de pagina con titulo
  - Formulario de creacion
  - Tabla de categorias
  - Columnas de tabla (Name, Type)
- **Creacion de Categorias (5 tests):**
  - Crear categoria Income
  - Crear categoria Expense
  - Limpiar formulario despues de crear
  - Validacion de campo nombre requerido
  - Tipo Income por defecto
- **Funcionalidad (4 tests):**
  - Cambiar entre tipos Income/Expense
  - Display de categorias en tabla
  - Crear multiples categorias
- **Responsive Design (2 tests):**
  - Vista mobile y tablet

#### Transacciones (26 tests)

- **Visualizacion (6 tests):**
  - Display de pagina con titulo
  - Formulario de creacion
  - Botones de filtro
  - Tarjetas de resumen
  - Tabla de transacciones
  - Columnas de tabla
- **Creacion (5 tests):**
  - Crear nueva transaccion
  - Validacion de nombre requerido
  - Validacion de monto requerido
  - Limpiar formulario despues de crear
- **Filtrado (4 tests):**
  - Filtrar por All, Income, Expense
  - Actualizacion de contador al filtrar
- **Tarjetas de Resumen (3 tests):**
  - Display de Total Income, Expense, Balance con moneda
- **Creacion con Diferentes Datos (4 tests):**
  - Transaccion de ingreso
  - Transaccion de gasto
  - Transaccion sin descripcion
  - Multiples transacciones
- **Responsive Design (2 tests):**
  - Vista mobile y tablet

#### Reportes (22 tests)

- **Visualizacion (6 tests):**
  - Display de pagina con titulo
  - Secciones de graficos (Bar, Doughnut, Line)
  - Tarjeta de resumen con totales
- **Renderizado de Graficos (3 tests):**
  - Todos los canvas elements
  - Display de cada tipo de grafico
- **Visibilidad de Graficos (3 tests):**
  - Grafico de barras visible
  - Grafico de dona visible
  - Grafico de linea visible
- **Renderizado de Canvas (2 tests):**
  - Dimensiones apropiadas
  - Canvas dentro de contenedores
- **Layout y Grid (2 tests):**
  - Grid layout de graficos
  - Espaciado entre graficos
- **Responsive Design (3 tests):**
  - Vista mobile y tablet
  - Aspect ratio en diferentes viewports
- **Visualizacion de Datos (2 tests):**
  - Leyendas de graficos
  - Estadisticas junto a graficos

#### Perfil (20 tests)

- **Visualizacion (6 tests):**
  - Display de pagina con titulo
  - Campos del formulario (name, email, phone, currency, theme)
- **Edicion de Campos (5 tests):**
  - Actualizar nombre
  - Actualizar email
  - Actualizar telefono
  - Opciones de moneda
  - Opciones de tema
- **Validacion (3 tests):**
  - Formato de email
  - Formato de telefono
- **Actualizacion de Perfil (4 tests):**
  - Actualizar con datos validos
  - Actualizar solo nombre
  - Actualizar preferencia de moneda
  - Actualizar preferencia de tema
- **Validacion de Formulario (3 tests):**
  - Email valido/invalido
  - Manejo de campos vacios
- **Responsive Design (3 tests):**
  - Vista mobile y tablet
  - Layout en diferentes tamaños

### Resumen de Configuracion E2E

- **Dependencias agregadas:** `@playwright/test`, `dotenv-cli`
- **Archivo de configuracion:** `playwright.config.ts` con:
  - Solo Chromium para velocidad
  - Soporte para ejecucion paralela
  - Screenshots en fallos
  - Video en fallos
  - Trace en retry
  - WebServer automatico
- **Scripts en package.json:**
  - `test:e2e` - Suite completa headless
  - `test:e2e:headed` - Con interfaz visual
  - `test:e2e:ui` - Modo UI de Playwright
  - `test:e2e:debug` - Modo debug
- **Global setup/teardown:** Verificacion de aplicacion lista y limpieza

## Como Ejecutar las Pruebas

### Pruebas Unitarias

1. Ubicacion: `cd frontend`
2. Instalar dependencias (una sola vez): `npm install`
3. Suite completa modo CI: `npm run test`
4. Modo interactivo: `npm run test:watch`
5. Reporte de cobertura (text, lcov, html): `npm run test:coverage`

> Todas las pruebas mockean Axios, por lo que no es necesario levantar el backend para ejecutarlas.

### Pruebas E2E

#### Requisitos Previos

- Backend debe estar corriendo (el frontend se inicia automaticamente)
- Variables de entorno configuradas (ver `.env.e2e.example`)

#### Comandos de Ejecucion

**Ubicacion:** `cd frontend`

**Suite completa (headless):**

```bash
npm run test:e2e
```

**Con interfaz visual (headed):**

```bash
npm run test:e2e:headed
```

**Modo UI interactivo:**

```bash
npm run test:e2e:ui
```

**Modo debug:**

```bash
npm run test:e2e:debug
```

**Con 4 workers en paralelo (headless):**

```bash
npx playwright test e2e/ --workers=4
```

**Con 4 workers en paralelo (headed):**

```bash
npx playwright test e2e/ --workers=4 --headed
```

**Test especifico:**

```bash
npx playwright test e2e/auth.spec.ts
npx playwright test e2e/dashboard.spec.ts
npx playwright test e2e/categories.spec.ts
npx playwright test e2e/transactions.spec.ts
npx playwright test e2e/reports.spec.ts
npx playwright test e2e/profile.spec.ts
```

**Ver reporte HTML:**

```bash
npx playwright show-report
```

#### Ejemplos de Ejecucion

```bash
# Ejecutar solo tests de autenticacion en modo headed
npx playwright test e2e/auth.spec.ts --headed

# Ejecutar tests de dashboard con 2 workers
npx playwright test e2e/dashboard.spec.ts --workers=2

# Ejecutar todos los tests en paralelo (4 workers) headless
npx playwright test e2e/ --workers=4

# Ejecutar todos los tests en paralelo (4 workers) headed
npx playwright test e2e/ --workers=4 --headed

# Ejecutar con modo debug (paso a paso)
npx playwright test e2e/transactions.spec.ts --debug
```

## Pie de Pagina

---

**Ultima actualizacion:** 11/11/2025 -- 33 pruebas unitarias frontend pasando, 0 backend, 106 E2E pasando.
**Cobertura E2E:** Autenticacion, Dashboard, Categorias, Transacciones, Reportes y Perfil completamente cubiertos.
**Tecnologia E2E:** Playwright + Chromium con Page Object Model.
