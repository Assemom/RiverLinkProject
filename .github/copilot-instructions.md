# Copilot Instructions

## Project Guidelines
- Project follows 3-tier architecture (Controllers → Services → Repositories → DbContext) with thin controllers, business logic in services, repositories for data access only, DTOs only, FluentValidation, unified responses, AutoMapper, pagination in services, admin endpoints protected, no hard delete for catalogs, and always inspect existing implementations before generating new ones.