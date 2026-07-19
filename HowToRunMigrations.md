# Executando Migrations (EF Core)

## 1. Acessar a pasta Backend

Sempre execute os comandos a partir da pasta raiz do Backend.

```powershell
cd C:\Desenvolvimento\Repositories\Personal\Silmarillion\Backend
```

> Nunca execute os comandos dentro dos projetos `Infrastructure` ou `API`.

---

## 2. Verificar se o DbContext está funcionando

Antes de criar qualquer migration, execute:

```powershell
dotnet ef dbcontext info `
    --project .\Source\Contexts\AccessControlContext\Infrastructure\Infrastructure.csproj `
    --startup-project .\Source\Presentation\API\API\API.csproj
```

Resultado esperado:

```text
Build started...
Build succeeded.

Type: Infrastructure.Persistence.AccessControlDbContext
Provider name: Npgsql.EntityFrameworkCore.PostgreSQL
Database name: SilmarillionDb
Data source: tcp://localhost:5432
```

Se este comando falhar, corrija o problema antes de continuar.

---

## 3. Criar a Migration

```powershell
dotnet ef migrations add NomeDaMigration `
    --project .\Source\Contexts\AccessControlContext\Infrastructure\Infrastructure.csproj `
    --startup-project .\Source\Presentation\API\API\API.csproj
```

Exemplo:

```powershell
dotnet ef migrations add AddSessions `
    --project .\Source\Contexts\AccessControlContext\Infrastructure\Infrastructure.csproj `
    --startup-project .\Source\Presentation\API\API\API.csproj
```

---

## 4. Revisar a Migration

Antes de atualizar o banco, revisar:

- Tabelas criadas
- Colunas
- Tipos
- Nullable
- Índices
- Chaves
- Constraints

Nunca aplicar uma migration sem revisar o código gerado.

---

## 5. Aplicar a Migration

```powershell
dotnet ef database update `
    --project .\Source\Contexts\AccessControlContext\Infrastructure\Infrastructure.csproj `
    --startup-project .\Source\Presentation\API\API\API.csproj
```

---

## 6. Verificar no PostgreSQL

Entrar no container:

```powershell
docker exec -it silmarillion-postgres psql -U silmarillion -d SilmarillionDb
```

Listar tabelas:

```sql
\dt
```

Visualizar a estrutura de uma tabela:

```sql
\d sessions
```

Sair do PostgreSQL:

```sql
\q
```

---

# Fluxo Resumido

```text
1. Acessar a pasta Backend

2. Executar:
   dotnet ef dbcontext info

3. Criar a migration:
   dotnet ef migrations add NomeDaMigration

4. Revisar a migration gerada

5. Aplicar:
   dotnet ef database update

6. Validar no PostgreSQL
```

## Observações

- Executar sempre os comandos na pasta `Backend`.
- O parâmetro `--project` deve apontar para o projeto **Infrastructure**.
- O parâmetro `--startup-project` deve apontar para o projeto **API**.
- Revisar toda migration antes de aplicá-la.
- Validar o resultado diretamente no PostgreSQL após executar o `database update`.