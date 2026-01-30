# Copilot Instructions for Semantic Kernel Quick-Start

## Project Overview
- **Semantic Kernel Quick-Start** is a C# solution for AI-powered monitoring, classification, and mitigation of digital assets.
- Built on a clean architecture: ASP.NET Core Web API backend, Blazor WebAssembly frontend, SQL Server storage, and Microsoft Semantic Kernel for AI/LLM integration.
- Infrastructure is managed via Azure Bicep and deployed using GitHub Actions.

## Key Architectural Patterns
- **Frontend:** `src/Presentation.Blazor/` (Blazor WebAssembly)
- **Backend:** `src/Presentation.WebApi/` (ASP.NET Core Web API)
- **Core Logic:** `src/Core.Application/`, `src/Core.Domain/`
- **AI Integration:** `src/Infrastructure.SemanticKernel/` (Semantic Kernel plugins, prompt orchestration)
- **Persistence:** `src/Infrastructure.SqlServer/` (SQL Server, migrations)
- **IaC:** `data/` (SQL), Azure Bicep in deployment scripts

## Developer Workflows
- **Build:** Use `build.cmd` or `build.sh` in `src/` to build the solution.
- **Test:** Integration specs in `src/gherkin Tests.Specs.Integration/` and `src/Tests.Specs.Integration/`.
- **Run:** Launch via Visual Studio or `dotnet run` from solution root.
- **CI/CD:** Managed by GitHub Actions (`.github/workflows/`).
- **IaC Deploy:** See `can-digital-insights-iac.yml` for infrastructure deployment.

## Naming & Conventions
- **C# code:** PascalCase for types/methods/properties, _camelCase for private fields, camelCase for locals.
- **Database:** PascalCase plural for tables, PascalCase for columns, `Id` for PK, `RelatedEntityId` for FK.
- **API:** Kebab-case, plural nouns for endpoints (e.g., `/api/digital-agents`).
- **Files/Folders:** C# files match main class, folders use PascalCase, config/docs use lowercase-hyphens.
- See `docs/naming-conventions.md` for details.

## Integration & Extensibility
- **Semantic Kernel:** Add plugins in `src/Infrastructure.SemanticKernel/Plugins/`.
- **External Integrations:** Use `src/Core.Application/Common/` and `src/Infrastructure.SemanticKernel/` for connectors.
- **RBAC & Security:** Enforced in API layer, see `ConfigureServicesAuth.cs`.

## References
- [README.md](../README.md): Project overview and getting started

## Examples
- To add a new agent plugin: create in `src/Infrastructure.SemanticKernel/Plugins/`, register in `ConfigureServices.cs`.
- To add a new API endpoint: implement in `src/Presentation.WebApi/`, follow API naming conventions.

---
For further details, always check the referenced docs and existing code patterns in the relevant folders.