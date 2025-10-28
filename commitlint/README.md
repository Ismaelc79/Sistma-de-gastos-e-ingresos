# Commitlint Configuration

Este directorio contiene la configuración de commitlint para validar los mensajes de commit según las convenciones definidas en `docs/CONVENTIONS.md`.

## Configuración

- **commitlint.config.cjs**: Archivo de configuración principal que define:
  - Tipos de commits permitidos (feat, fix, docs, style, refactor, test, chore, perf, ci, build, revert)
  - Ámbitos permitidos (expenses, income, ui, api, db, config, tests, docs, reports)
  - Longitud máxima del encabezado: 72 caracteres
  - Formato de scope: kebab-case o lower-case
  - Formato de subject: minúscula sin mayúsculas

**Nota**: Los hooks de git están en `../.husky/` (raíz del proyecto)

## Uso

### Opción 1: Usar Commitizen (Recomendado)
```bash
npm run commit
```
Esto abrirá un asistente interactivo para crear commits siguiendo las convenciones.

### Opción 2: Commit Manual
```bash
git commit -m "feat(expenses): add expense categorization"
```

### Validar un commit
```bash
npm run commitlint
```

## Ejemplos de Commits Válidos

```bash
feat(expenses): add expense categorization system
fix(income): resolve income calculation error
docs(api): add JSDoc comments to expense utilities
test(reports): add comprehensive tests for report generation
refactor(components): extract reusable expense form component
chore(deps): update project dependencies
```

## Ejemplos de Commits Inválidos

```bash
# ❌ Tipo inválido
bugfix(expenses): fix calculation error

# ❌ Scope inválido
feat(user-management): add new feature

# ❌ Encabezado muy largo (>72 caracteres)
feat(expenses): add a new feature that does something very important and long

# ❌ Mayúscula en el subject
feat(expenses): Add expense categorization

# ❌ Punto al final
feat(expenses): add expense categorization.
```

## Configuración de Husky

Los hooks de git están configurados en `.husky/commit-msg` para ejecutar commitlint automáticamente cuando haces un commit.

Si necesitas deshabilitar temporalmente los hooks:
```bash
npm run husky-off
```

Para volver a habilitarlos:
```bash
npm run husky-on
```

## Referencias

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Commitlint Documentation](https://commitlint.js.org/)
- [Commitizen](http://commitizen.github.io/cz-cli/)
