# Ejemplos de Uso - Commitlint y Commitizen

## 📝 Cómo Hacer Commits

### Opción 1: Usar Commitizen (Recomendado)

```bash
npm run commit
```

Esto abrirá un asistente interactivo:

```
? Select the type of change that you're committing: (Use arrow keys)
❯ feat:     A new feature
  fix:      A bug fix
  docs:     Documentation only changes
  style:    Changes that do not affect the meaning of the code
  refactor: A code change that neither fixes a bug nor adds a feature
  perf:     A code change that improves performance
  test:     Adding missing tests or correcting existing tests
  chore:    Changes to the build process or auxiliary tools
  ci:       Changes to CI configuration files and scripts
  build:    Changes to the build system or dependencies
  revert:   Revert to a commit

? What is the scope of this change? (e.g. component or file name): (press enter to skip)
expenses

? Write a short, imperative tense description of the change:
add expense categorization system

? Provide a longer description of the changes: (press enter to skip)
Implement a new categorization system for expenses that allows users to organize their expenses by predefined categories.

? Are there any breaking changes? (y/N)
N

? Does this change affect any open issues? (y/N)
N
```

Resultado:
```
feat(expenses): add expense categorization system

Implement a new categorization system for expenses that allows users to organize their expenses by predefined categories.
```

### Opción 2: Commit Manual

```bash
git commit -m "feat(expenses): add expense categorization system"
```

El hook `commit-msg` validará automáticamente el mensaje.

## ✅ Ejemplos de Commits Válidos

### Features (Nuevas Funcionalidades)

```bash
feat(expenses): add expense categorization system
feat(income): implement recurring income tracking
feat(reports): add monthly expense summary report
feat(ui): create responsive dashboard layout
feat(api): add authentication endpoints
```

### Fixes (Correcciones)

```bash
fix(income): resolve income calculation error
fix(expenses): fix duplicate expense entries
fix(ui): correct button alignment on mobile
fix(api): handle null values in transaction response
```

### Documentation

```bash
docs(api): add JSDoc comments to expense utilities
docs(readme): update installation instructions
docs(conventions): clarify commit message format
docs(setup): add troubleshooting guide
```

### Style (Cambios de Formato)

```bash
style(ui): improve button spacing
style(components): update color scheme
style(layout): adjust padding and margins
```

### Refactoring

```bash
refactor(components): extract reusable expense form component
refactor(utils): simplify expense validation logic
refactor(api): reorganize request handlers
```

### Tests

```bash
test(expenses): add unit tests for expense creation
test(income): add integration tests for income calculation
test(reports): add tests for report generation
```

### Chore (Mantenimiento)

```bash
chore(deps): update project dependencies
chore(build): update webpack configuration
chore(ci): add GitHub Actions workflow
```

### Performance

```bash
perf(list): optimize expense rendering
perf(api): cache frequently accessed data
perf(ui): reduce bundle size
```

### CI/CD

```bash
ci(github): add automated testing workflow
ci(actions): configure deployment pipeline
```

### Build

```bash
build(webpack): update build configuration
build(npm): add new build scripts
```

### Revert

```bash
revert: feat(expenses): add expense categorization
```

## ❌ Ejemplos de Commits Inválidos

### Tipo Inválido

```bash
# ❌ INCORRECTO - "bugfix" no es un tipo válido
git commit -m "bugfix(expenses): fix calculation error"

# ✅ CORRECTO
git commit -m "fix(expenses): fix calculation error"
```

### Scope Inválido

```bash
# ❌ INCORRECTO - "user-management" no es un scope válido
git commit -m "feat(user-management): add new feature"

# ✅ CORRECTO
git commit -m "feat(config): add new feature"
```

### Encabezado Muy Largo

```bash
# ❌ INCORRECTO - Más de 72 caracteres
git commit -m "feat(expenses): add a new feature that does something very important and long"

# ✅ CORRECTO
git commit -m "feat(expenses): add expense categorization system"
```

### Mayúscula en el Subject

```bash
# ❌ INCORRECTO - Comienza con mayúscula
git commit -m "feat(expenses): Add expense categorization"

# ✅ CORRECTO
git commit -m "feat(expenses): add expense categorization"
```

### Punto al Final

```bash
# ❌ INCORRECTO - Termina con punto
git commit -m "feat(expenses): add expense categorization."

# ✅ CORRECTO
git commit -m "feat(expenses): add expense categorization"
```

### Verbo en Pasado

```bash
# ❌ INCORRECTO - Verbo en pasado
git commit -m "feat(expenses): added expense categorization"

# ✅ CORRECTO
git commit -m "feat(expenses): add expense categorization"
```

### Mensaje Vago

```bash
# ❌ INCORRECTO - Mensaje vago
git commit -m "feat(expenses): fix stuff"

# ✅ CORRECTO
git commit -m "feat(expenses): add expense categorization system"
```

## 📊 Estructura de Commits Complejos

Para commits con cuerpo y pie:

```bash
git commit -m "feat(expenses): add expense categorization system

This commit implements a new categorization system that allows users to:
- Create custom expense categories
- Assign expenses to categories
- Filter expenses by category
- View category-based reports

Closes #123
Relates to #456"
```

Estructura:
```
<tipo>(<scope>): <descripción corta>

<cuerpo opcional - explicación detallada>

<pie opcional - referencias a issues>
```

## 🔍 Validar Commits

### Validar el Último Commit

```bash
npm run commitlint
```

### Validar un Rango de Commits

```bash
npx commitlint --from HEAD~5 --to HEAD --verbose
```

### Validar un Mensaje Específico

```bash
echo "feat(expenses): add categorization" | npx commitlint
```

## 🎯 Casos de Uso Comunes

### Agregar Nueva Funcionalidad

```bash
npm run commit
# Seleccionar: feat
# Scope: expenses
# Descripción: add expense categorization system
```

### Corregir un Bug

```bash
npm run commit
# Seleccionar: fix
# Scope: income
# Descripción: resolve income calculation error
```

### Actualizar Documentación

```bash
npm run commit
# Seleccionar: docs
# Scope: api
# Descripción: add JSDoc comments to expense utilities
```

### Actualizar Dependencias

```bash
npm run commit
# Seleccionar: chore
# Scope: deps
# Descripción: update project dependencies
```

### Refactorizar Código

```bash
npm run commit
# Seleccionar: refactor
# Scope: components
# Descripción: extract reusable expense form component
```

## 📚 Recursos Adicionales

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Commitizen Documentation](http://commitizen.github.io/cz-cli/)
- [Commitlint Rules](https://commitlint.js.org/reference/rules.html)
