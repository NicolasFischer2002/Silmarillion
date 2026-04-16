# Documentação — Preparando o container de Banco de Dados com PostgreSQL

Este guia mostra como configurar um banco de dados **PostgreSQL** utilizando Docker, com persistência de dados e boas práticas para ambiente de desenvolvimento.

---

## Pré-requisitos

Antes de começar, você precisa ter instalado apenas:

* Docker Desktop
* WSL 2 habilitado no Windows
* Git (opcional)

---

## 1) Estrutura inicial do projeto

Considere a seguinte estrutura:

```txt
MeuProjeto/
  Backend/
  Frontend/
  docker-compose.yml
  .env
```

---

## 2) Criar o arquivo `.env`

Na raiz do projeto, crie um arquivo chamado `.env`:

```env
POSTGRES_DB=MeuProjetoDb
POSTGRES_USER=appuser
POSTGRES_PASSWORD=senha_segura_aqui
```

⚠️ Esse arquivo **não deve ser versionado** (adicione ao `.gitignore`).

---

## 3) Configurar o serviço no `docker-compose.yml`

Adicione o serviço do PostgreSQL:

```yaml
services:
  postgres:
    image: postgres:18.3
    container_name: meu-projeto-postgres
    environment:
      POSTGRES_DB: ${POSTGRES_DB}
      POSTGRES_USER: ${POSTGRES_USER}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${POSTGRES_USER}"]
      interval: 10s
      timeout: 5s
      retries: 5

volumes:
  postgres_data:
```

---

## 4) Subir o container do banco

Na raiz do projeto, execute:

```bash
docker compose up --build
```

---

## 5) Validar se o banco está rodando

Execute:

```bash
docker compose ps
```

Você deve ver algo como:

```txt
postgres   Up (healthy)
```

---

## 6) Acessar o banco via terminal

Você pode acessar o PostgreSQL dentro do container:

```bash
docker exec -it meu-projeto-postgres psql -U appuser -d MeuProjetoDb
```

Exemplos de comandos SQL:

```sql
\dt
SELECT version();
```

Para sair:

```bash
\q
```

---

## 7) Acessar o banco externamente

Você também pode acessar via ferramentas como:

* DBeaver
* pgAdmin

Configuração:

* Host: localhost
* Porta: 5432
* Database: MeuProjetoDb
* User: appuser
* Password: senha definida no .env

---

## 8) Persistência de dados

O volume configurado:

```yaml
postgres_data:/var/lib/postgresql
```

Garante que:

* Dados não são perdidos ao parar o container
* Estrutura do banco (tabelas, dados) permanece

---

## 9) Resetar o banco (apagando dados)

Para apagar completamente os dados:

```bash
docker compose down -v
```

Isso remove:

* Container
* Volume (dados do banco)

---

## 10) Como o backend se conecta ao banco

Dentro do Docker, o backend deve usar:

```txt
Host=postgres;Port=5432;Database=MeuProjetoDb;Username=appuser;Password=senha
```

⚠️ Importante:

* Dentro do Docker: use `postgres`
* Fora do Docker: use `localhost`

---

## 11) Ciclo de vida do banco

* Primeiro `up`: banco é criado
* Próximos `up`: banco reutiliza dados existentes
* `down`: container para
* `down -v`: banco é recriado do zero

---

## 12) Boas práticas aplicadas

* Uso de `.env` para credenciais
* Volume para persistência
* Healthcheck para garantir disponibilidade
* Isolamento via container

---

## 13) Resumo do fluxo

1. Criar `.env`
2. Configurar PostgreSQL no `docker-compose`
3. Subir com `docker compose up`
4. Acessar banco
5. Persistir dados com volume
6. Resetar quando necessário

---

## Observação final

O banco de dados roda isolado em container, mas totalmente acessível:

* pelo backend
* por ferramentas externas
* por terminal

Isso garante um ambiente reproduzível, consistente e pronto para evoluir com migrations e EF Core.