# Configuración de Commitlint y Commitizen

Este documento describe cómo se ha configurado commitlint y commitizen en el proyecto para validar los commits según las convenciones definidas en `CONVENTIONS.md`.

## ¿Qué se instaló?

### 1. Commitizen (Global)
```bash
npm install -g commitizen cz-conventional-changelog
```
- **Commitizen**: Herramienta interactiva para crear commits siguiendo convenciones
- **cz-conventional-changelog**: Adaptador para Conventional Commits

### 2. Commitlint (Dependencia de Desarrollo)
```bash
npm install --save-dev @commitlint/cli @commitlint/config-conventional husky
```
- **@commitlint/cli**: Validador de mensajes de commit
- **@commitlint/config-conventional**: Configuración estándar de Conventional Commits
- **husky**: Gestor de git hooks

## Archivos de Configuración

### `.commitlintrc.cjs`
Archivo de configuración raíz que extiende la configuración del directorio `commitlint/`.

### `commitlint/commitlint.config.cjs`
Configuración principal con:
- **Tipos permitidos**: feat, fix, docs, style, refactor, test, chore, perf, ci, build, revert
- **Ámbitos permitidos**: expenses, income, ui, api, db, config, tests, docs, reports
- **Reglas**:
  - Longitud máxima del encabezado: 72 caracteres
  - Scope en kebab-case o lower-case
  - Subject en minúscula sin mayúsculas

### `.husky/commit-msg`
Hook de git que ejecuta commitlint automáticamente al hacer commit.

### `~/.czrc` (Global)
Configuración global de commitizen:
```json
{ "path": "cz-conventional-changelog" }
```

## Cómo Usar

### Opción 1: Commitizen (Recomendado)
```bash
npm run commit
```
Se abrirá un asistente interactivo que te guiará para crear un commit válido.

### Opción 2: Commit Manual
```bash
git commit -m "feat(expenses): add expense categorization"
```
El hook `commit-msg` validará automáticamente el mensaje.

### Validar Manualmente
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
perf(list): optimize expense rendering
ci(github): add automated testing workflow
build(webpack): update build configuration
revert: feat(expenses): add expense categorization
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

# ❌ Verbo en pasado
feat(expenses): added expense categorization
```

## Gestión de Hooks

### Deshabilitar Temporalmente
```bash
npm run husky-off
```

### Volver a Habilitar
```bash
npm run husky-on
```

## Scripts Disponibles

En `package.json` se han agregado los siguientes scripts:

```json
{
  "scripts": {
    "commit": "cz commit",
    "commitlint": "commitlint --from HEAD~1 --to HEAD --verbose",
    "husky-on": "git config core.hooksPath commitlint/.husky",
    "husky-off": "git config core.hooksPath .git/hooks"
  }
}
```

## Solución de Problemas

### El hook no se ejecuta
1. Verifica que husky esté instalado: `npm list husky`
2. Verifica que los hooks estén habilitados: `git config core.hooksPath`
3. Reinicia el repositorio: `npm run husky-on`

### Commitizen no funciona
1. Verifica que esté instalado globalmente: `npm list -g commitizen`
2. Verifica la configuración: `cat ~/.czrc`
3. Reinstala si es necesario: `npm install -g commitizen cz-conventional-changelog`

### Mensaje de commit rechazado
1. Revisa el error de commitlint
2. Asegúrate de seguir el formato: `<tipo>(<scope>): <descripción>`
3. Verifica que el tipo y scope sean válidos
4. Verifica que la descripción no exceda 72 caracteres

## Referencias

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Commitlint Documentation](https://commitlint.js.org/)
- [Commitizen](http://commitizen.github.io/cz-cli/)
- [Husky](https://typicode.github.io/husky/)
