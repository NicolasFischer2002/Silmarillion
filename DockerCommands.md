## 🚀 Subindo a aplicação (Front + Back)

Na raiz do projeto (onde está o `docker-compose.yml`), execute:

```bash
docker compose up --build
```

---

## 🌐 Acessos

* 🎨 Front-end: http://localhost:5173
* ⚙️ API Back-end: http://localhost:5000/weatherforecast

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