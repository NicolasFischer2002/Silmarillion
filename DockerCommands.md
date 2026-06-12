## 🚀 Subindo a aplicação (Front + Back + Database)

Na raiz do projeto (onde está o `docker-compose.yml`), execute:

```bash
docker compose up --build
```

---

## Para subir o banco de dados💺🎲

Na raiz do projeto (onde está o `docker-compose.yml`), execute:

```bash
docker compose up postgres -d
```

---

## 🌐 Acessos

* 🎨 Front-end: http://localhost:5173
* ⚙️ API Back-end: http://localhost:5000/weatherforecast
* 🗄️ PostgreSQL: localhost:5432

---

## 🛑 Parar os containers

Para desligar tudo:

```bash
docker compose down
```

---

## 💡 Observações

* Use `--build` quando fizer alterações no código ou configuração
* Para rodar em segundo plano (background):

```bash
docker compose up -d --build
```

---

## 🧠 Banco de Dados (PostgreSQL)

* Host (Docker): `postgres`
* Host (máquina local): `localhost`
* Porta: `5432`
* Database: definido no `.env`
* User: definido no `.env`
* Password: definido no `.env`

---

## 🔐 Variáveis de ambiente

O projeto utiliza um arquivo `.env` na raiz para configurar o banco de dados.

Exemplo (`.env.example`):

```env
POSTGRES_DB=SilmarillionDb
POSTGRES_USER=silmarillion
POSTGRES_PASSWORD=your_password_here
```

> O arquivo `.env` real não deve ser versionado.

---

## ⚠️ Importante

* O backend deve se conectar ao banco usando o host `postgres`, não `localhost`
* Os dados do banco são persistidos em volume Docker (`postgres_data`)
* Alterações no `.env` não afetam um banco já criado (volume existente)
* Para resetar completamente o banco:

```bash
docker compose down -v
```
