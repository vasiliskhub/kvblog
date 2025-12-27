# CLAUDE.md

## Project Overview

- Primary language: C#
- Framework: .NET (version based on project files)
- Goal: Keep code easy to understand, maintain, and extend.
- Priorities: clarity, reliability, and consistency.

Assume:
- Straightforward structure.
- No strict architecture patterns required.
- Keep changes scoped and safe.

---

## Coding Style

### General
- Prefer async APIs for anything that touches I/O.
- Use dependency injection where applicable.
- Keep classes and methods reasonably small and purposeful.
- Avoid unnecessary abstractions unless the code clearly benefits.

### Naming
- Classes: `PascalCase`
- Methods: `PascalCase`
- Variables/parameters: `camelCase`
- Private fields: `_camelCase`
- Async methods should end with `Async`.

### Error Handling
- Use exceptions where appropriate.
- Return error responses from API layers using whichever format the project currently uses (e.g., ProblemDetails, custom error DTOs, simple JSON).

---

## .NET Specifics

### Nullability
- Enable nullable reference types (`<Nullable>enable</Nullable>`)
- Use `?` for nullable types explicitly
- Prefer `ArgumentNullException.ThrowIfNull()` for parameter validation
- Use `??` and `?.` operators instead of explicit null checks where readable

### Collections & LINQ
- Prefer `List<T>` for implementation, `IEnumerable<T>` for parameters/returns
- Use LINQ methods over manual loops when it improves readability
- Avoid `.ToList()` unless you need materialization
- Prefer `foreach` over `for` unless you need the index

### Async Best Practices
- Never use `.Result` or `.Wait()` - always await
- Use `Task` for async operations, not `async void` (except event handlers)
- ConfigureAwait(false) in library code, omit in application code
- Prefer `ValueTask<T>` for hot paths that often complete synchronously

### Dependency Injection
- Register services with appropriate lifetime: Singleton, Scoped, Transient
- Avoid injecting Scoped/Transient services into Singletons
- Prefer constructor injection over property injection
- Use `IOptions<T>` for configuration classes

### Performance & Resource Management
- Use `using` statements (or declarations) for `IDisposable` resources
- Prefer `StringBuilder` for string concatenation in loops
- Use `Span<T>` and `Memory<T>` for performance-critical buffer operations
- Avoid `async` keyword if just returning a `Task` directly

### Logging
- Use structured logging with `ILogger<T>`
- Use log levels appropriately (Trace/Debug/Info/Warning/Error/Critical)
- Prefer LoggerMessage source generators for high-performance logging
- Don't log sensitive data (passwords, tokens, PII)

---

## Project Organization

- Keep related code grouped together (e.g., controllers with controllers, services with services).
- When adding new functionality, follow the existing folder structure and naming conventions.
- Don’t introduce new layers or frameworks unless explicitly requested.

### File Changes
When modifying existing code:
1. Follow the style already used in that file.
2. Avoid refactoring unrelated code unless needed for correctness.
3. Keep changes minimal unless otherwise requested.
4. If a change impacts several files, summarize what you’re doing before applying edits.

---

## APIs & HTTP Endpoints

- Use existing patterns for routing, controllers, and API responses.
- Match the serialization and validation patterns already present.
- When creating new endpoints, include:
  - Request model
  - Response model
  - Input validation (if the project already uses validation)
  - Logging where helpful

---

## Testing

- Follow the layout and style already present in the project’s test folder.
- Use clear assertion messages.
- Prefer smaller tests that focus on one behavior at a time.

Example structure:
- Arrange
- Act
- Assert

---

## Documentation & Comments

- Add comments only when something isn’t obvious from reading the code.
- Use XML docs on public methods if the rest of the codebase uses them.
- Keep README or inline notes short and focused.

---

## Git / Commit Messages

Use clear, simple commit messages such as:

- `add: new endpoint for payment details`
- `fix: null check for settings`
- `refactor: clean up mapping logic`
- `chore: update package versions`

---

## How to Interact as Claude Code

When responding to requests or making changes:
1. Follow the project’s existing style and patterns.
2. Keep modifications focused on the task.
3. If something is ambiguous, outline a few options and recommend one.
4. Prefer diffs or clear before/after sections for code edits.
5. Don’t introduce new architectural patterns unless specifically asked.
