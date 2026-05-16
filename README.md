# ERP SaaS

Aplicação ERP SaaS em desenvolvimento, com **front-end em React + TypeScript** e **back-end em .NET**, estruturada com foco em **Clean Architecture**, **DDD** e boas práticas de engenharia de software.

O objetivo do projeto é modelar uma base sólida para um sistema de gestão com módulos como:

- controle de usuários e acesso
- cadastro de organizações
- controle de estoque
- pedidos
- compras
- vendas

A solução está sendo construída com separação clara de responsabilidades, domínio bem definido e camadas independentes.

---

## Tecnologias

### Back-end
- **.NET 10**
- **C#**
- **ASP.NET Core Minimal APIs**
- **Entity Framework Core**
- **PostgreSQL**
- **MSTest**

### Front-end
- **React**
- **TypeScript**

### Infraestrutura
- **Docker**
- **Docker Compose**

---

## Ferramentas

- **Git**
- **GitHub**
- **Visual Studio 2026**
- **Visual Studio Code**

---

## Práticas e princípios adotados

- **Clean Architecture**
- **Domain-Driven Design (DDD)**
- **Clean Code**
- **Programação Orientada a Objetos (POO)**
- **SOLID**
- **Result Pattern**
- **Guard Clauses**
- **Value Objects**
- **Aggregates**
- **Bounded Contexts**
- **Testes unitários**

---

## Estrutura de domínio

O projeto está sendo organizado em contextos delimitados para manter o domínio coeso e evolutivo.

---

## Estratégia de modelagem

O domínio está sendo modelado com foco em:

- **entidades com identidade própria**
- **Value Objects para regras de valor**
- **Aggregates para proteger invariantes**
- **políticas para validação de entrada**
- **estado explícito no modelo**
- **operações expressivas no domínio**

---

## Estratégia de testes

Os testes unitários estão sendo escritos desde a modelagem do domínio, validando:

- regras de criação
- validações de Value Objects
- comportamento dos Aggregates
- transições de estado
- erros de domínio esperados

O objetivo é manter o modelo confiável e evolutivo desde o início.

---

## Status do projeto

Projeto em desenvolvimento.

A estrutura inicial está sendo construída com foco em:

- base de domínio consistente
- separação de responsabilidades
- escalabilidade para evolução futura
- manutenção simples e previsível

---

## Objetivo técnico

Construir um ERP SaaS com uma base arquitetural sólida, capaz de sustentar crescimento de domínio e de escopo sem perder clareza, testabilidade e controle de dependências.

---

## Licença

Projeto pessoal/em desenvolvimento.