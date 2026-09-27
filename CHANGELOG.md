# Changelog

## Unreleased

### Breaking changes

- Migrated the core package to Avalonia 12 and .NET 10/.NET 11.
- Removed ReactiveUI-specific and legacy MVU APIs from the core library.
- Removed the legacy `GenerateExtensionsForAssemblyAttribute`; use `GenerateMarkupExtensionsForAssemblyAttribute` instead.

### Added

- Added a semantic control model for generated setters, bindings, events, attached properties, styles, factories, and view-factory registrations.
- Added generated control factories with source file, line, view, and control metadata.
- Added runtime diagnostics and external assembly generation checks.
- Added Basic, MVVM, and Custom Controls samples.
