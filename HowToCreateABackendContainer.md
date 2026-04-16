# Documentação — Preparando o container de Back-end com .NET 10

Este guia mostra como criar e executar um ambiente de back-end **do zero**, usando **.NET 10** dentro de um container Docker.

A estrutura foi pensada para trabalhar com:

* **Clean Architecture**
* **DDD** com bounded contexts
* **Minimal APIs**
* **MSTest**
* **Docker Compose**

---

## Pré-requisitos

Antes de começar, você precisa ter instalado apenas:

* Docker Desktop
* WSL 2 habilitado no Windows
* Git, se quiser versionar o projeto

Você **não precisa** instalar o SDK do .NET na máquina, desde que o projeto seja criado e executado dentro do container.

---

## 1) Estrutura inicial do projeto

Crie a pasta raiz do projeto e a pasta do back-end:

```txt
MeuProjeto/
  Backend/
  docker-compose.yml
```

Neste momento, a pasta `Backend` pode estar vazia.

---

## 2) Criar um container temporário com o SDK do .NET

Abra o PowerShell na raiz do projeto, no nível da pasta `Backend`, e execute:

```bash
docker run --rm -it -v "${PWD}\Backend:/src" -w /src mcr.microsoft.com/dotnet/sdk:10.0 bash
```

Esse comando:

* sobe um container temporário com o SDK do .NET
* monta a pasta `Backend` dentro do container
* deixa você pronto para criar a solução e os projetos

---

## 3) Criar a solução em branco

Ainda dentro do container, execute:

```bash
dotnet new sln -n MeuProjeto
```

Isso cria o arquivo da solução na raiz montada em `/src`.

---

## 4) Criar a estrutura do bounded context

Crie a pasta do contexto de domínio. Exemplo:

```bash
mkdir -p Source/Contexts/UserContext
cd Source/Contexts/UserContext
```

A ideia é manter cada contexto isolado, com suas camadas separadas.

---

## 5) Criar os projetos do bounded context

Ainda dentro de `Source/Contexts/UserContext`, crie os projetos:

### API (Minimal API)

```bash
dotnet new webapi -n UserContext.Api
```

### Application

```bash
dotnet new classlib -n UserContext.Application
```

### Domain

```bash
dotnet new classlib -n UserContext.Domain
```

### Infrastructure

```bash
dotnet new classlib -n UserContext.Infrastructure
```

### Tests

```bash
dotnet new mstest -n UserContext.Tests
```

---

## 6) Voltar para a raiz da solução

```bash
cd /src
```

---

## 7) Adicionar os projetos na solution

Agora registre todos os projetos na solução:

```bash
dotnet sln add Source/Contexts/UserContext/UserContext.Api/UserContext.Api.csproj
dotnet sln add Source/Contexts/UserContext/UserContext.Application/UserContext.Application.csproj
dotnet sln add Source/Contexts/UserContext/UserContext.Domain/UserContext.Domain.csproj
dotnet sln add Source/Contexts/UserContext/UserContext.Infrastructure/UserContext.Infrastructure.csproj
dotnet sln add Source/Contexts/UserContext/UserContext.Tests/UserContext.Tests.csproj
```

---

## 8) Configurar as referências entre os projetos

Agora crie as dependências corretas entre as camadas.

### Application depende de Domain

```bash
dotnet add Source/Contexts/UserContext/UserContext.Application/UserContext.Application.csproj reference Source/Contexts/UserContext/UserContext.Domain/UserContext.Domain.csproj
```

### Infrastructure depende de Application e Domain

```bash
dotnet add Source/Contexts/UserContext/UserContext.Infrastructure/UserContext.Infrastructure.csproj reference Source/Contexts/UserContext/UserContext.Application/UserContext.Application.csproj
dotnet add Source/Contexts/UserContext/UserContext.Infrastructure/UserContext.Infrastructure.csproj reference Source/Contexts/UserContext/UserContext.Domain/UserContext.Domain.csproj
```

### API depende de Application

```bash
dotnet add Source/Contexts/UserContext/UserContext.Api/UserContext.Api.csproj reference Source/Contexts/UserContext/UserContext.Application/UserContext.Application.csproj
```

### Tests depende de Domain e Application

```bash
dotnet add Source/Contexts/UserContext/UserContext.Tests/UserContext.Tests.csproj reference Source/Contexts/UserContext/UserContext.Domain/UserContext.Domain.csproj
dotnet add Source/Contexts/UserContext/UserContext.Tests/UserContext.Tests.csproj reference Source/Contexts/UserContext/UserContext.Application/UserContext.Application.csproj
```

---

## 9) Restaurar pacotes

```bash
dotnet restore
```

---

## 10) Validar a compilação

```bash
dotnet build
```

Se tudo estiver correto, a solução deve compilar sem erros.

---

## 11) Executar a API manualmente dentro do container

Entre na pasta do projeto da API:

```bash
cd Source/Contexts/UserContext/UserContext.Api
```

Execute a aplicação expondo a porta para fora do container:

```bash
dotnet run --urls=http://0.0.0.0:5000
```

Abra no navegador:

```txt
http://localhost:5000/weatherforecast
```

Se tudo estiver certo, a API deve responder com JSON.

---

## 12) Sair do container

Quando terminar, saia com:

```bash
exit
```

---

## 13) Estrutura final esperada

Ao final, a pasta deve ficar parecida com isto:

```txt
Backend/
  Source/
    Contexts/
      UserContext/
        UserContext.Api/
        UserContext.Application/
        UserContext.Domain/
        UserContext.Infrastructure/
        UserContext.Tests/
  MeuProjeto.sln
```

---

## 14) Dockerfile do back-end

Depois de criar e validar a solução, crie um `Dockerfile` dentro da pasta `Backend`.

Exemplo de base para desenvolvimento:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0

WORKDIR /src

COPY . .

WORKDIR /src/Source/Contexts/UserContext/UserContext.Api

CMD ["dotnet", "run", "--urls=http://0.0.0.0:5000"]
```

Esse tipo de configuração é útil para desenvolvimento. Mais tarde, você pode evoluir para uma imagem multi-stage para produção.

---

## 15) Executar com Docker Compose

Na raiz do projeto, o back-end pode ser iniciado via `docker compose` junto com o front-end e o banco.

Exemplo de execução:

```bash
docker compose up --build
```

A API ficará acessível em:

```txt
http://localhost:5000/weatherforecast
```

---

## 16) Parar o container

Para desligar tudo:

```bash
docker compose down
```

---

## 17) Resumo do fluxo

1. Criar a pasta `Backend`
2. Abrir um container temporário com o SDK do .NET
3. Criar solução, contexto e projetos
4. Configurar referências entre as camadas
5. Restaurar e compilar
6. Rodar a API
7. Evoluir para Docker Compose

---

## Observação final

Esse fluxo permite trabalhar sem instalar o SDK do .NET na máquina.

O ambiente de execução fica isolado no container, e o código permanece no projeto local, pronto para ser versionado e evoluído com segurança.