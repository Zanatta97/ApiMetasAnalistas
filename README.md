# ApiMetasAnalistas

[![.NET Version](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![C# Version](https://img.shields.io/badge/C%23-14.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)

*Choose your language / Escolha o seu idioma:*
- 🇺🇸 [English](#-english-version)
- 🇧🇷 [Português](#-versão-em-português)
 
---
 
## 🇧🇷 Versão em Português
 
### Sobre o Projeto
 
**ApiMetasAnalistas** é um projeto de **aprendizado pessoal** desenvolvido para consolidar, na prática, os principais conceitos do desenvolvimento backend moderno com **.NET 10** e **C# 14**.
 
O tema central é uma API RESTful para gerenciamento de metas de analistas de suporte técnico — um contexto real que serviu de base para explorar arquitetura, concorrência, boas práticas de código e comunicação entre serviços.
 
> ⚠️ **Este é um projeto de estudos.** O objetivo principal não é entregar um produto final, mas sim aplicar e documentar os conceitos aprendidos de forma estruturada e progressiva.
 
---
 
### Conceitos Estudados e Aplicados
 
#### Clean Architecture
A solução foi estruturada em camadas com responsabilidades bem definidas, seguindo os princípios da Arquitetura Limpa:
 
- **Domain:** Entidades e regras de negócio puras, sem dependência de frameworks externos.
- **Application:** Casos de uso, interfaces de serviços e contratos (interfaces de repositório).
- **Infrastructure:** Implementações concretas de persistência, logging e serviços externos.
- **API (Presentation):** Controllers, configuração de middlewares e ponto de entrada da aplicação.
 
Essa separação garante que as regras de negócio não dependam de detalhes de infraestrutura, facilitando manutenção e testabilidade.
 
---
 
#### API RESTful
 
A API segue os princípios REST de forma deliberada e estudada:
 
| Conceito | Aplicação no projeto |
|---|---|
| **Verbos HTTP** | `GET`, `POST`, `PUT`, `DELETE` com semântica correta |
| **Status Codes** | `200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `404 Not Found` |
| **Recursos e URIs** | Rotas no padrão `/api/analistas`, `/api/metas/{id}` |
| **Stateless** | Cada requisição carrega todas as informações necessárias |
| **Content Negotiation** | Respostas em `application/json` |
 
---
 
#### Injeção de Dependência (DI)
 
Utilização do container de DI nativo do .NET para desacoplar as implementações das abstrações:

---
  
#### Logging Customizado em Arquivo
 
Em vez de utilizar apenas o `ILogger` padrão do .NET, foi implementado um sistema de logging próprio para entender os desafios por trás dessas ferramentas:
 
**O que o `CustomLogger` registra:**
- Acessos aos endpoints (com horário e parâmetros)
- Erros e exceções com stack trace
- Eventos de ciclo de vida da aplicação (inicialização, encerramento)
  
---

### Tecnologias
 
| Tecnologia | Versão | Papel no projeto |
|---|---|---|
| .NET | 10.0 | Framework principal |
| C# | 14.0 | Linguagem |
| ASP.NET Core | 10.0 | Framework web / API |
| System.Text.Json | Nativo | Serialização JSON |
| OpenAPI | Nativo (.NET 10) | Especificação e documentação dos endpoints |
| Scalar | Latest | Interface visual interativa para explorar a API |
| Entity Framework Core | Latest | ORM para acesso e mapeamento de dados |
 
---
 
## 🇺🇸 English Version
 
### About the Project
 
**ApiMetasAnalistas** is a **personal learning project** built to consolidate, through hands-on practice, the core concepts of modern backend development with **.NET 10** and **C# 14**.
 
The central theme is a RESTful API for managing goals of tech support analysts — a real-world context used as a foundation to explore architecture, concurrency, code best practices, and inter-service communication.
 
> ⚠️ **This is a study project.** The main goal is not to deliver a final product, but to apply and document learned concepts in a structured and progressive way.
 
---
 
### Concepts Studied and Applied
 
#### Clean Architecture
 
The solution is structured in layers with well-defined responsibilities, following Clean Architecture principles:
 
- **Domain:** Pure entities and business rules, with no dependency on external frameworks.
- **Application:** Use cases, service interfaces, and repository contracts.
- **Infrastructure:** Concrete implementations for persistence, logging, and external services.
- **API (Presentation):** Controllers, middleware configuration, and application entry point.
 
This separation ensures business rules are decoupled from infrastructure details, improving maintainability and testability.
 
---
 
#### RESTful API
 
The API deliberately and deliberately follows REST principles:
 
| Concept | Application in the project |
|---|---|
| **HTTP Verbs** | `GET`, `POST`, `PUT`, `DELETE` with correct semantics |
| **Status Codes** | `200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `404 Not Found` |
| **Resources & URIs** | Routes like `/api/analistas`, `/api/metas/{id}` |
| **Stateless** | Each request carries all required information |
| **Content Negotiation** | Responses in `application/json` |
 
---
 
#### Dependency Injection (DI)
 
.NET's built-in DI container is used to decouple implementations from abstractions:
 
---

#### 📁 Logging Customizado em Arquivo
 
Em vez de utilizar apenas o `ILogger` padrão do .NET, foi implementado um sistema de logging próprio para entender os desafios por trás dessas ferramentas:
 
**O que o `CustomLogger` registra:**
- Acessos aos endpoints (com horário e parâmetros)
- Erros e exceções com stack trace
- Eventos de ciclo de vida da aplicação (inicialização, encerramento)
 
**Formato do log:**
```
[2025-06-15 14:32:01] [INFO ] GET /api/metas - 200 OK (12ms)
[2025-06-15 14:32:05] [ERROR] Analista ID 99 não encontrado - NotFoundException
```

---
 
### 🛠️ Technologies
 
| Technology | Version | Role in the project |
|---|---|---|
| .NET | 10.0 | Main framework |
| C# | 14.0 | Language |
| ASP.NET Core | 10.0 | Web / API framework |
| System.Text.Json | Native | JSON serialization |
| OpenAPI | Native (.NET 10) | Endpoint specification and documentation |
| Scalar | Latest | Interactive visual UI to explore the API |
| Entity Framework Core | Latest | ORM for data access and mapping |
