# Contexto do Projeto — Voyage

## O que é
Voyage é um backend de marketplace/diretório local: usuários (`client`, `owner`, `admin`) cadastram empresas (`Company`), associam essas empresas a endereços (`Address`), controlam pagamentos/anúncios (`Payment`) e favoritam empresas (`Favorite`).

## Stack atual (a ser migrada)
- **Runtime:** Node.js + Express
- **ORM:** Prisma
- **Banco:** MySQL (hospedado na Hostinger)
- **Auth:** JWT (`jsonwebtoken`) + `bcrypt` para hash de senha
- **Validação:** Zod
- **Upload de imagem:** Multer (memoryStorage) → envia para ImgBB (serviço externo de hospedagem de imagem), guarda só a URL retornada
- **Deploy atual:** `voyagegabi.onrender.com` (Render)

## Stack alvo (migração — atividade da instituição)
- **Linguagem/Framework:** C# / ASP.NET Core
- **ORM:** Entity Framework Core + provider **Pomelo.EntityFrameworkCore.MySql** (banco continua o mesmo MySQL da Hostinger)
- **IDE:** Antigravity

---

## Modelo de dados (fonte: `schema.prisma`)

### User (`users`)
- `id`, `name`, `type` (string livre no banco, mas validado como enum `client | owner | admin` na aplicação), `email`, `password` (hash bcrypt), `phone?`, `cpf?`
- `signature`: enum `BASIC | PREMIUM` (default `BASIC`)
- Relações: `favorites[]`, `address[]` (N:N com Address, relação nomeada `addressUser`), `company[]` (1:N — um user `owner` pode ter várias companies)
- `createdAt`, `updatedAt`

### Company (`companies`)
- `id`, `name`, `category` (enum de negócio: Lanchonete, Restaurante, Pizzaria, Churrascaria, Supermercado, Farmácia, Serviços, Hospital, Outros, Bar), `cnpj` (único, validado com dígito verificador), `evaluate` (float 0–5, default 0), `places` (string livre, descrição textual do endereço)
- `userId` (FK) → dono da empresa
- Relações: `favorites[]`, `payments[]`, `addressCompany[]`
- `createdAt`, `updatedAt`

### Payment (`payments`)
- `id`, `companyId` (FK), `toDate`, `dueDate`, `paymentForm`, `advertising`, `key`, `type`
- `createdAt`, `updatedAt`

### Favorite (`favorites`)
- `id`, `userId` (FK), `companyId` (FK) — tabela de junção explícita User↔Company

### Address (`address`)
- `id`, `place`, `number`, `zipcode`, `lat`, `long`, `url` (imagem via ImgBB)
- Relações: `users[]` (N:N implícita com User), `addressCompany[]`
- `createdAt`, `updatedAt`

### AddressCompany (`address_company`)
- `id`, `companyId` (FK), `addressId` (FK) — tabela de junção explícita Address↔Company

### ⚠️ Ponto de atenção para o EF Core
`User` ↔ `Address` é N:N **implícita** no Prisma (sem tabela pivô modelada). Decidir, na migração: manter implícita (EF Core 8+ suporta) ou explicitar a tabela pivô — vale documentar a decisão tomada.

---

## Autenticação e autorização

- Login (`POST /user/login`) valida email/senha (bcrypt) e emite JWT contendo `sub` (id), `type`, `email`, `name`, expira em 1 dia.
- Middleware `auth.js`: exige header `Authorization: Bearer <token>`, valida o JWT e injeta `req.logged = { id, type, email, name }`.
- Regras de autorização por rota/recurso:
  - `POST /company`: só `type === 'owner'`
  - `PUT/DELETE /company/:id`, `PUT/DELETE /address/:id`: só o dono do recurso (`userId`/dono do address via relação `users`)
  - `GET /user`: só `type === 'admin'`
  - `GET/PUT /user/:id`: só o próprio usuário ou `admin`
  - `/company` e `/payment` no `server.js` têm `auth` aplicado globalmente no router; `/user` e `/address` têm auth só em rotas específicas (algumas leituras de address são públicas)

## Validações de negócio (Zod) — regras a preservar na tradução

- **CPF**: validação completa de dígito verificador (algoritmo padrão brasileiro)
- **CNPJ**: validação completa de dígito verificador + regex de formatação (aceita com ou sem máscara)
- **Senha**: mínimo 10 caracteres, 1 maiúscula, 1 símbolo, rejeita sequências óbvias (`12345`, `qwerty`, `password`)
- **Nome**: mínimo 3 caracteres, só letras/acentos/espaço/hífen/apóstrofo
- **Address**: `place` só texto (sem número), `number` até 6 dígitos + 1 letra opcional, `zipcode` no formato `00000-000`, `lat` [-90,90], `long` [-180,180]
- **Company**: `category` restrita a uma lista fixa de valores; `evaluate` entre 0 e 5

## Upload de imagem
Fluxo atual: Multer recebe o arquivo em memória → converte para base64 → `POST` para API do ImgBB (`IMG_BB_KEY` via env) → salva só a URL retornada no campo `url` do Address. É **obrigatório** enviar imagem tanto na criação quanto na edição de Address.

## Rotas (contrato atual, via arquivos `.http`)

| Recurso | Rotas |
|---|---|
| `/user` | `POST /` (criar), `POST /login`, `GET /` (admin), `GET /:id` (próprio ou admin), `PUT /:id` (próprio ou admin) |
| `/company` | `POST /` (owner), `GET /`, `GET /:id`, `PUT /:id` (dono), `DELETE /:id` (dono) — tudo atrás de `auth` global |
| `/address` | `POST /` (com imagem), `GET /` (com filtros: `lat`, `long`, `radius`, `user`, `company`, `category`, `favorite`), `GET /:id`, `PUT /:id` (auth, dono, com imagem), `DELETE /:id` (auth, dono) |
| `/payment` | `POST /`, `GET /` (filtros: `companyId`, datas, `paymentForm`, `advertising`, `type`), `GET /:id`, `PUT /:id`, `DELETE /:id` — tudo atrás de `auth` global |

Base de teste atual: `https://voyagegabi.onrender.com`.

---

## Débitos técnicos conhecidos no código Node (não replicar na migração)

- `attachSave` (`utils/save.js`) reimplementa manualmente um `.save()` estilo ActiveRecord por cima do Prisma — no EF Core isso é substituído nativamente por change tracking + `SaveChangesAsync()`.
- Validação espalhada e duplicada entre schemas Zod inline nos controllers — no ASP.NET Core, migrar para DTOs com Data Annotations/FluentValidation.
- `req.logged` setado manualmente no middleware — no ASP.NET Core, usar `ClaimsPrincipal` nativo com JWT Bearer scheme + `[Authorize]`.
- Alguns retornos de erro inconsistentes (`res.status(404).json("string solta")` em vez de objeto padronizado) — vale padronizar na reescrita.

---

## Decisão de migração já alinhada
Reescrever pensando em C#/ASP.NET Core idiomático (Controllers + DTOs + Services + EF Core + `[Authorize]`), **não traduzir função por função**. Manter as regras de negócio e o contrato de API (rotas, filtros, respostas), trocar a implementação por padrões nativos do .NET.

- Detalhe quero use como o exemplo do arquivo **Usuario.cs**
