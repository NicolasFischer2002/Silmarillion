# Documentação — Preparando o container de Front-end com React + TypeScript

Este guia mostra como criar e executar um ambiente de front-end **do zero**, usando **React + TypeScript** dentro de um container Docker.

## Pré-requisitos

Antes de começar, você precisa ter instalado apenas:

* Docker Desktop
* WSL 2 habilitado no Windows
* Git, se quiser versionar o projeto

Você **não precisa** instalar Node, npm, TypeScript ou Vite globalmente na máquina.

---

## 1) Estrutura inicial do projeto

Crie a pasta raiz do projeto e a pasta do front-end:

```txt
MeuProjeto/
  Frontend/
  docker-compose.yml
```

Neste momento, a pasta `Frontend` pode estar vazia.

---

## 2) Criar o projeto React + TypeScript dentro de um container temporário

Abra o PowerShell na raiz do projeto, no nível da pasta `Frontend`, e execute:

```bash
docker run --rm -it -v "${PWD}\Frontend:/app" -w /app node:22-alpine sh
```

Esse comando:

* sobe um container temporário com Node
* monta a pasta `Frontend` dentro do container
* deixa você pronto para criar o projeto

---

## 3) Criar o projeto com Vite

Ainda dentro do container, execute:

```bash
npm create vite@latest .
```

Quando o assistente perguntar:

* Framework: `React`
* Variant: `TypeScript`

Depois, instale as dependências:

```bash
npm install
```

Finalize saindo do container:

```bash
exit
```

---

## 4) Ajustar o Vite para rodar dentro do container

Abra o arquivo `Frontend/vite.config.ts` e deixe assim:

```ts
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    host: '0.0.0.0',
    port: 5173,
  },
})
```

Esse ajuste é necessário para o Vite aceitar conexões externas ao container.

---

## 5) Criar o `Dockerfile`

Na pasta `Frontend`, crie o arquivo `Dockerfile` sem extensão e use este conteúdo:

```dockerfile
FROM node:22-alpine

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

EXPOSE 5173

CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"]
```

---

## 6) Criar o `.dockerignore`

Na pasta `Frontend`, crie o arquivo `.dockerignore` com:

```gitignore
node_modules
dist
.git
.vscode
```

Esse arquivo evita copiar arquivos desnecessários para a imagem.

---

## 7) Criar o `docker-compose.yml`

Na raiz do projeto, crie o arquivo `docker-compose.yml` com:

```yaml
services:
  frontend:
    build:
      context: ./Frontend
    ports:
      - "5173:5173"
    volumes:
      - ./Frontend:/app
      - /app/node_modules
    command: npm run dev -- --host 0.0.0.0
```

---

## 8) Subir o front-end

Na raiz do projeto, execute:

```bash
docker compose up --build
```

---

## 9) Acessar o front-end

Abra no navegador:

```txt
http://localhost:5173
```

Se tudo estiver certo, o Vite + React será carregado normalmente.

---

## 10) Parar o container

Para desligar:

```bash
docker compose down
```

Se quiser rodar em segundo plano:

```bash
docker compose up -d --build
```

---

## 11) Estrutura final esperada

Ao final, sua pasta deve ficar parecida com isto:

```txt
MeuProjeto/
  Frontend/
    src/
    public/
    package.json
    tsconfig.json
    vite.config.ts
    Dockerfile
    .dockerignore
  docker-compose.yml
```

---

## 12) Resumo do fluxo

1. Criar a pasta `Frontend`
2. Entrar com um container Node temporário
3. Criar o projeto React + TypeScript com Vite
4. Ajustar o `vite.config.ts`
5. Criar `Dockerfile` e `.dockerignore`
6. Subir com `docker compose up --build`
7. Acessar em `http://localhost:5173`

---

## Observação final

Esse fluxo permite trabalhar sem instalar Node localmente.
O ambiente de execução fica isolado no container, e o código permanece no seu projeto local.