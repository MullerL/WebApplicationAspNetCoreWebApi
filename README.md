# WebApplicationAspNetCoreWebApi + Angular UI

Um sistema completo (`Full-Stack`) composto por uma **API RESTful** construída com **ASP.NET Core Web API (.NET 10)** e um **Frontend** interativo construído com **Angular 20**. Atualmente, o sistema demonstra operações CRUD (Create, Read, Update, Delete) básicas gerenciando `Produtos` utilizando o banco de dados via **Entity Framework Core**.

## 🖥️ Ferramentas & Ambiente de Desenvolvimento

Esta solução foi desenvolvida utilizando modernas ferramentas de engenharia de software e recursos robustos, configurados com as seguintes ferramentas e versões:

### Editor e Ambientes (IDE)
- **Visual Studio 2022 (v17.x)** / **Visual Studio Code (v1.8x)** - Ambientes primários de codificação do ecossistema .NET e Node.
- **SQL Server Management Studio (SSMS v19.x)** - Gerenciamento do banco de dados relacional.

### Infraestrutura & Plataforma
- **.NET SDK 10.0** - Core utilizado no backend. 
- **Node.js (v20.x)** e **NPM (v10.x)** - Runtime e gerenciador de pacotes do projeto frontend Angular.

### Assistência Especializada de Inteligência Artificial
O ecossistema contou com o apoio coordenado de modelos generativos de última geração, operando de forma embarcada através da extensão **Antigravity**:
- **Google DeepMind**: Gemini 3.1 Pro e Gemini 3 Flash.
- **Anthropic**: Claude 4.6 Sonnet.


---

## 🛠️ Tecnologias Utilizadas

### Backend (Web API)
- **ASP.NET Core Web API (.NET 10.0)**
- **Entity Framework Core (v8.0.0)**
- **SQL Server** (Banco de dados relacional)
- **xUnit (v2.9.3)** e **Moq (v4.20.72)**
- **Microsoft.NET.Test.Sdk (v17.14.1)**
- **Swagger / OpenAPI**
- **CORS** (Configurado sob a *policy* `AllowAngular`)

### Frontend (User Interface)
- **Angular 20.3.0** (Framework Single Page Application)
- **PrimeNG (v20.1.x)** e **PrimeFlex (v4.0.0)**
- **RxJS (v7.8.x)**
- **TypeScript (v5.9.x)**

---

## 📁 Estrutura do Projeto

O repositório está dividido em três projetos principais:

- **`WebApplicationAspNetCoreWebApi/`** - Projeto do Backend (API principal).
  - **`Controllers/`**: Expõe os Endpoints HTTP (ex: `ProdutoController`).
  - **`Models/`**: Entidades e regras do Banco de Dados (ex: `Produto.cs`, `AppDbContext.cs`).
  - **`ScriptsDB/`**: Scripts SQL puristas para criação das tabelas e do banco de dados (alternativa ao Migration).
- **`AngularProjectToWebApi/`** - Projeto do Frontend em Angular.
  - Consome os dados da Web API para exibir e gerenciar produtos de forma interativa.
- **`WebApplicationAspNetCoreWebApi.Tests/`** - Projeto de Testes xUnit.
  - Possui 9 cenários de testes unitários assegurando a blindagem de todas as rotas CRUD.

---

## ⚙️ Configuração do Banco de Dados (SQL Server)

Antes de rodar a API, prepare a sua base de dados do SQL Server local:

1. Acesse a pasta `WebApplicationAspNetCoreWebApi/ScriptsDB/` neste repositório.
2. Pelo **SSMS**, execute o script `CREATE DATABASE DbEmpresa.sql`.
3. Em seguida, execute o script `CREATE TABLE Produtos.sql`.
4. Vá até o arquivo `appsettings.json` na raiz da **Web API** e verifique a variável `DefaultConnection` para que aponte para a sua instância (ex: `localhost` ou `.\\SQLEXPRESS`).

Exemplo do `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DbEmpresa;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 🚀 Como Executar a Aplicação

Para ver o sistema funcionando plenamente, você precisará rodar o Backend e o Frontend paralelamente.

### 1. Rodando a API (Backend)
Abra um terminal, navegue até a pasta da API e inicie o projeto:
```bash
cd WebApplicationAspNetCoreWebApi
dotnet run
```
> A API estará rodando nos endereços locais indicados no terminal (ex: `https://localhost:7198`). A página inicial `/swagger` oferece testes interativos.

### 2. Rodando o Angular (Frontend)
Abra um **novo terminal**, navegue até a pasta do projeto Angular, instale as dependências com NPM e inicie o servidor:
```bash
cd AngularProjectToWebApi
npm install
npm start
```
> Acesse `http://127.0.0.1:4200` no seu navegador. 

---

## 🧪 Rodando os Testes de Unidade da API

A API obedece protocolos implementados através de testes no **xUnit**. É utilizado o provedor `Microsoft.EntityFrameworkCore.InMemory` garantindo testes de memória isolados, hiper-rápidos, sem poluir a base real do SQL.

```bash
cd WebApplicationAspNetCoreWebApi.Tests
dotnet test
```

---

## 📋 Endpoints Principais da API (ProdutoController)

- `GET /api/Produto` - Retorna a lista de todos os Produtos cadastrados.
- `GET /api/Produto/{id}` - Busca um Produto específico por seu Id.
- `POST /api/Produto` - Cria e adiciona um novo Produto.
- `PUT /api/Produto/{id}` - Atualiza um Produto existente.
- `DELETE /api/Produto/{id}` - Remove o Produto permanentemente.
