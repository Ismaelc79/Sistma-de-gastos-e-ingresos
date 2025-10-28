# Convenciones de Desarrollo - Sistema de Gastos e Ingresos

## 📋 Convenciones de Commits

### Formato de Commits
Utilizamos **Conventional Commits** con el siguiente formato:

```
<tipo>[ámbito opcional]: <descripción>

[cuerpo opcional]

[pie(s) opcional(es)]
```

### Tipos de Commits

| Tipo | Descripción | Ejemplo |
|------|-------------|---------|
| `feat` | Nueva funcionalidad | `feat(expenses): add expense categorization` |
| `fix` | Corrección de errores | `fix(income): resolve income calculation error` |
| `docs` | Documentación | `docs(readme): update installation instructions` |
| `style` | Cambios de formato (no afectan la lógica) | `style(ui): improve button spacing` |
| `refactor` | Refactorización de código | `refactor(utils): simplify expense validation logic` |
| `test` | Agregar o modificar tests | `test(expenses): add unit tests for expense creation` |
| `chore` | Mantenimiento, dependencias | `chore(deps): update dependencies` |
| `perf` | Mejoras de rendimiento | `perf(list): optimize expense rendering` |
| `ci` | Configuración de CI/CD | `ci(github): add automated testing workflow` |
| `build` | Sistema de build | `build(webpack): update build configuration` |
| `revert` | Revertir commits | `revert: feat(expenses): add expense categorization` |

### Ámbitos Sugeridos
- `expenses` - Gestión de gastos
- `income` - Gestión de ingresos
- `ui` - Interfaz de usuario
- `api` - Llamadas y lógica de API
- `db` - Base de datos
- `config` - Configuración
- `tests` - Sistema de testing
- `docs` - Documentación
- `reports` - Reportes y análisis

### Reglas para Mensajes de Commit

#### ✅ Hacer:
- Usar imperativo presente: "add" no "added" o "adds"
- Primera letra en minúscula
- No terminar con punto
- Ser descriptivo pero conciso (máximo 72 caracteres en el título)
- Usar inglés para consistencia

#### ❌ No hacer:
- Mensajes vagos: "fix stuff", "update code"
- Commits muy grandes (hacer commits atómicos)
- Mezclar diferentes tipos de cambios

### Ejemplos de Buenos Commits

```bash
feat(expenses): add expense categorization system
fix(income): resolve income calculation error
docs(api): add JSDoc comments to expense utilities
test(reports): add comprehensive tests for report generation
refactor(components): extract reusable expense form component
chore(deps): update project dependencies
```

## 🌿 Convenciones de Ramas

### Estructura de Ramas

```
main
├── develop
├── feature/SGI-001-expense-categorization
├── feature/SGI-002-income-tracking
├── bugfix/SGI-003-calculation-error
├── hotfix/SGI-004-critical-data-loss
├── release/v1.0.0
└── docs/SGI-005-update-readme
```

### Tipos de Ramas

| Tipo | Propósito | Formato | Ejemplo |
|------|-----------|---------|---------|
| `main` | Código en producción | `main` | `main` |
| `develop` | Integración de desarrollo | `develop` | `develop` |
| `feature` | Nuevas funcionalidades | `feature/SGI-XXX-descripcion-corta` | `feature/SGI-001-expense-categorization` |
| `bugfix` | Corrección de errores | `bugfix/SGI-XXX-descripcion-corta` | `bugfix/SGI-002-calculation-error` |
| `hotfix` | Correcciones críticas | `hotfix/SGI-XXX-descripcion-corta` | `hotfix/SGI-003-data-corruption` |
| `release` | Preparación de releases | `release/vX.Y.Z` | `release/v1.0.0` |
| `docs` | Solo documentación | `docs/SGI-XXX-descripcion-corta` | `docs/SGI-004-api-documentation` |

### Reglas para Nombres de Ramas

#### ✅ Hacer:
- Usar kebab-case (guiones)
- Incluir número de ticket/issue cuando aplique
- Ser descriptivo pero conciso
- Usar inglés
- Prefijo con tipo de rama

#### ❌ No hacer:
- Espacios o caracteres especiales
- Nombres muy largos (máximo 50 caracteres)
- CamelCase o snake_case
- Nombres vagos como "fix" o "update"

## 🏷️ Convenciones de Versionado

Utilizamos **Semantic Versioning (SemVer)**:

```
MAJOR.MINOR.PATCH (ejemplo: 1.4.2)
```

- **MAJOR**: Cambios que rompen compatibilidad
- **MINOR**: Nuevas funcionalidades compatibles
- **PATCH**: Correcciones de errores compatibles

### Ejemplos:
- `1.0.0` - Release inicial
- `1.1.0` - Nueva funcionalidad (categorización de gastos)
- `1.1.1` - Corrección de error en categorización
- `2.0.0` - Cambio mayor (nueva arquitectura)

## 📝 Convenciones de Pull Requests

### Título del PR
```
[SGI-XXX] Tipo: Descripción concisa
```

**Ejemplos:**
- `[SGI-001] Feature: Add expense categorization system`
- `[SGI-002] Fix: Resolve income calculation error`
- `[SGI-003] Docs: Update API documentation`

### Template de PR
```markdown
## 📋 Descripción
Breve descripción de los cambios realizados.

## 🎯 Tipo de Cambio
- [ ] Bug fix (corrección que soluciona un problema)
- [ ] Nueva funcionalidad (cambio que agrega funcionalidad)
- [ ] Breaking change (corrección o funcionalidad que causa que funcionalidad existente no funcione como se esperaba)
- [ ] Esta cambio requiere actualización de documentación

## 🧪 Pruebas
- [ ] Tests unitarios agregados/actualizados
- [ ] Tests de integración agregados/actualizados
- [ ] Todas las pruebas pasan localmente

## 📝 Checklist
- [ ] Mi código sigue las convenciones del proyecto
- [ ] He realizado self-review de mi código
- [ ] He comentado mi código en áreas difíciles de entender
- [ ] He actualizado la documentación correspondiente
- [ ] Mis cambios no generan nuevas advertencias
```

## 🔧 Configuración de Herramientas

### Commitizen
Para ayudar con el formato de commits:
```bash
npm install -g commitizen cz-conventional-changelog
echo '{ "path": "cz-conventional-changelog" }' > ~/.czrc
```

### Husky + Commitlint
Para validar commits automáticamente:
```bash
npm install --save-dev @commitlint/cli @commitlint/config-conventional husky
```

## 📚 Recursos Adicionales

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Semantic Versioning](https://semver.org/)
- [Git Flow](https://nvie.com/posts/a-successful-git-branching-model/)

---

**Nota**: Estas convenciones son obligatorias para todos los contributors del proyecto Sistema de Gastos e Ingresos.
