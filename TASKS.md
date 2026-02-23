# sdk.language.serialization — Task Tracker

> **Purpose**: Track all work to bring this repo from dev-only state to a production-ready, published NuGet package.
> This is the **pilot repo** — patterns established here will be copied across all SDK repos.

---

## Current State (after deep-dive)

| Aspect | Status | Details |
|--------|--------|---------|
| **Code** | On `dev` only | 7 projects: Abstractions, System.Text.Json, Newtonsoft, 2x DI, Converters |
| **Naming** | Legacy | Projects: `Backbone.Language.Features.Serialization.*`, PackageIds: `WoW2.Backbone.*` |
| **README** | Placeholder | Each project has 2-3 line boilerplate (copy-paste from AI template) |
| **CLAUDE.md** | Missing | No repo-level AI context |
| **CI/CD** | Missing | No `.github/workflows/` |
| **Tests** | ❌ None | Zero test projects |
| **Samples** | ❌ None | No sample/example projects |
| **Versioning** | Inconsistent | Most are `9.0.0-alpha.0`, System.DI is `8.0.0-alpha.1`, Converters is `9.0.0-alpha.1` |
| **Build config** | Missing | No `Directory.Build.props`, no `.editorconfig`, no central package management |
| **Source Link** | Missing | No `Microsoft.SourceLink.GitHub`, no symbol packages |

### Package Structure (dev branch)

```
src/
├── Backbone.Language.Features.Serialization.Json.Abstractions     → IJsonSerializer, IAsyncJsonSerializer
├── Backbone.Language.Features.Serialization.Json.System           → SystemJsonSerializer (sync + async)
├── Backbone.Language.Features.Serialization.Json.System.DI        → AddSystemTextJsonSerializer()
├── Backbone.Language.Features.Serialization.Json.Newtonsoft       → NewtonsoftJsonSerializer (sync only)
├── Backbone.Language.Features.Serialization.Json.Newtonsoft.DI    → AddNewtonsoftJsonSerializer()
└── Backbone.Language.Features.Serialization.Json.Newtonsoft.Converters → Enum + Interface converters
```

### Key Issues Found
- **Version mismatch**: System.DI stuck at v8.0.0
- **No async on Newtonsoft**: System.Text.Json implements `IAsyncJsonSerializer`, Newtonsoft doesn't
- **Namespace ≠ PackageId**: namespaces say `Backbone.*`, packages say `WoW2.Backbone.*`
- **External dep**: Converters depends on `WoW.Two.Backbone.Language.Core.Extensions.Enums` (needs alignment)

---

## Phase 1: Foundation (Current)

### 1.1 Analyze & Plan
- [x] Explore repo state (branches, files, history)
- [x] Explore workspace CI/CD patterns (platform.pipelines, platform.data.transport)
- [x] Create this task tracker
- [x] Deep-dive dev branch code: understand package structure, public API surface, dependencies
- [x] Research package delivery best practices (tests, samples, README, Source Link, Directory.Build.props)
- [ ] Check existing NuGet packages (old `Backbone.*` names) — what's published, what versions
- [x] ~~Decide naming~~ → `WoW2.Sdk.Language.Serialization.*` (PascalCase, dot-separated, `WoW2` reserved prefix)
- [x] ~~Decide .NET target~~ → .NET 9

### 1.2 Code Finalization
- [ ] Merge dev → main (squash merge — clean history for v1)
- [ ] Rename projects + namespaces: `Backbone.Language.Features.Serialization.Json.*` → `WoW2.Sdk.Language.Serialization.*`
- [ ] Rename solution file: → `WoW2.Sdk.Language.Serialization.sln`
- [ ] Fix version consistency: all packages → `1.0.0-alpha.1`
- [ ] Clean up solution structure (remove .DS_Store, fix .gitignore)
- [ ] Add `Directory.Build.props` (shared: TargetFramework, Authors, RepositoryUrl, Source Link, Nullable, etc.)
- [ ] Add `Directory.Packages.props` (central package management for Newtonsoft.Json, MS.Extensions.DI, etc.)
- [ ] Update all `.csproj` files with full NuGet metadata (per best practices)
- [ ] Add `Microsoft.SourceLink.GitHub` + symbol package config
- [ ] Add XML doc comments on any undocumented public APIs (already mostly done)
- [ ] Delete `staging` and `test` branches

### 1.3 Repo Scaffolding
- [ ] Create `CLAUDE.md` (from SDK template, with serialization-specific context)
- [ ] Create root `README.md` (NuGet-rendered: badges, install, quick start, features, examples)
- [ ] Create per-package `README.md` files (included in .nupkg via PackageReadmeFile)
- [ ] Create `CHANGELOG.md` (Keep a Changelog format)
- [ ] Add `.editorconfig`
- [ ] Add `LICENSE` file (MIT)
- [ ] Add package icon (`icon.png`, 128x128)

### 1.4 Tests
- [ ] Create `tests/` folder structure
- [ ] Add `WoW2.Sdk.Language.Serialization.Abstractions.UnitTests` (interface contract tests)
- [ ] Add `WoW2.Sdk.Language.Serialization.System.UnitTests` (System.Text.Json impl tests)
- [ ] Add `WoW2.Sdk.Language.Serialization.Newtonsoft.UnitTests` (Newtonsoft impl tests)
- [ ] Add `WoW2.Sdk.Language.Serialization.Converters.UnitTests` (converter tests)
- [ ] Test DI registration (services resolve correctly)
- [ ] Test round-trip serialize/deserialize for common types
- [ ] Test settings provider (add/get/remove/configure)
- [ ] Test enum converters (description, field name, fallback)
- [ ] Test InterfaceToConcreteConverter
- [ ] Framework: xUnit + Moq (or NSubstitute)

### 1.5 Samples (optional for v1, nice to have)
- [ ] Create `samples/` folder
- [ ] Basic console app: serialize/deserialize with System.Text.Json
- [ ] Basic console app: serialize/deserialize with Newtonsoft
- [ ] DI registration example (ASP.NET minimal API or console with Host)

---

## Phase 2: CI/CD Pipeline

### 2.1 Pipeline Strategy
- [x] ~~Decide: reusable workflows vs per-repo~~ → **Per-repo first**, extract later
- [x] ~~Define branch → environment mapping~~ → `dev`→alpha (GitHub Packages), `main`→stable (NuGet.org)
- [ ] Analyze working pipelines in `platform.data.transport` for reference
- [ ] Design 3-workflow structure (see below)

### 2.2 Pipeline Implementation

**Target: 3 workflows**
```
.github/workflows/
├── ci.yml                    # PR → build + test (gate)
├── publish-prerelease.yml    # dev push → alpha to GitHub Packages
└── publish-release.yml       # main push → stable to NuGet.org + symbol packages
```

- [ ] Create `ci.yml` — build + test on PRs to main and dev
- [ ] Create `publish-prerelease.yml` — push to dev → pack + publish alpha to GitHub Packages
- [ ] Create `publish-release.yml` — push to main → pack + publish stable to NuGet.org (with .snupkg)
- [ ] Add NuGet caching (`actions/cache` with NuGet packages path)
- [ ] Configure repo secrets: `NUGET_API_KEY`
- [ ] Test: push to dev → verify alpha package on GitHub Packages
- [ ] Test: merge to main → verify stable package on NuGet.org

---

## Phase 3: Publish & Validate

- [ ] Publish first alpha packages under `WoW2.Sdk.Language.Serialization.*`
- [ ] Verify package installs correctly in a test project
- [ ] Verify README renders correctly on NuGet.org
- [ ] Verify Source Link works (step-into debugging in VS)
- [ ] Publish stable v9.0.0
- [ ] Update dependent repos (if any) to use new package name

---

## Decisions Log

| Decision | Options | Chosen | Date | Notes |
|----------|---------|--------|------|-------|
| Package naming | `Backbone.*` / `WoW.Two.Sdk.*` / `WoW2.Sdk.*` | `WoW2.Sdk.Language.Serialization` | 2026-02-23 | `WoW2` prefix reserved on NuGet. PascalCase + dots = NuGet convention. |
| .NET target | .NET 8 / .NET 9 / Multi-target | .NET 9 | 2026-02-23 | Target latest, can add multi-target later |
| Pipeline approach | Reusable workflows vs per-repo copy | Per-repo first | 2026-02-23 | Get it working here, then extract to platform.pipelines |
| Branching strategy | dev+main+staging+test vs simplified | dev + main only | 2026-02-23 | Drop staging/test. dev→alpha, main→stable. PR checks as gate. |
| Package registry | NuGet.org / GitHub Packages / Azure Artifacts | Dual: GitHub Pkgs (alpha) + NuGet.org (stable) | 2026-02-23 | Fast prerelease + public stable |
| Test framework | xUnit / NUnit / MSTest | xUnit | 2026-02-23 | Modern standard, best isolation, built-in parallel |
| Version strategy | Continue from 9.0.0-alpha / Reset to 1.0.0 | `9.0.0-alpha.1` | 2026-02-23 | Major = .NET version. See `versioning-strategy.md` in workspace root. |

---

## Package Naming Map

| Current (dev branch) | New Name |
|----------------------|----------|
| `Backbone.Language.Features.Serialization.Json.Abstractions` | `WoW2.Sdk.Language.Serialization.Json.Abstractions` |
| `Backbone.Language.Features.Serialization.Json.System` | `WoW2.Sdk.Language.Serialization.Json.System` |
| `Backbone.Language.Features.Serialization.Json.System.DependencyInjection` | `WoW2.Sdk.Language.Serialization.Json.System.DependencyInjection` |
| `Backbone.Language.Features.Serialization.Json.Newtonsoft` | `WoW2.Sdk.Language.Serialization.Json.Newtonsoft` |
| `Backbone.Language.Features.Serialization.Json.Newtonsoft.DependencyInjection` | `WoW2.Sdk.Language.Serialization.Json.Newtonsoft.DependencyInjection` |
| `Backbone.Language.Features.Serialization.Json.Newtonsoft.Converters` | `WoW2.Sdk.Language.Serialization.Json.Newtonsoft.Converters` |

---

## Best Practices Checklist (from research)

### Must-have for v1
- [ ] `Directory.Build.props` — shared build config (TargetFramework, Nullable, Source Link, Authors, etc.)
- [ ] `Directory.Packages.props` — central package management
- [ ] `Microsoft.SourceLink.GitHub` — step-into debugging for consumers
- [ ] Symbol packages (`.snupkg`) — published alongside `.nupkg`
- [ ] xUnit tests with `{Package}.UnitTests` naming
- [ ] Root README with: badges, install commands, quick start, features, API examples
- [ ] Per-package README (embedded in .nupkg)
- [ ] `CHANGELOG.md` (Keep a Changelog format)
- [ ] `LICENSE` (MIT)
- [ ] Package icon (128x128 PNG)
- [ ] Full NuGet metadata in .csproj (PackageId, Description, Tags, License, RepositoryUrl, etc.)

### Nice-to-have (v1.1+)
- [ ] Sample projects
- [ ] Benchmarks (BenchmarkDotNet)
- [ ] Code coverage reporting
- [ ] `CONTRIBUTING.md`
- [ ] GitHub Issue/PR templates

---

## Cross-Repo Impact

Once serialization is done, replicate to:
1. `sdk.language.core` — same state (stub on main, code elsewhere)
2. `sdk.language.linq` — same state
3. `sdk.ai.semantic-kernel` — same state
4. `sdk.ai.nlp` — same state
5. `sdk.resilience-patterns` — same state

**Template artifacts to extract:**
- `Directory.Build.props` template
- `Directory.Packages.props` template
- CLAUDE.md (already exists in `.claude/rules/templates/`)
- README.md template (from serialization's final README)
- CI/CD workflow files (3 workflows, parameterized)
- `.editorconfig`
- `.gitignore`
- Package icon
