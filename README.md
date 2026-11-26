
# Clínica API + Flutter

Aplicação de gerenciamento de clínicas desenvolvida com **Flutter** no aplicativo cliente e **ASP.NET Core Web API** no backend.

Os dois projetos estão organizados no mesmo repositório e se comunicam por meio de requisições HTTP, utilizando JSON para a troca de dados.

## Visão geral

O aplicativo permite gerenciar os principais cadastros e operações de uma clínica, incluindo:

- Pacientes;
- Médicos;
- Recepcionistas;
- Consultas;
- Medicamentos;
- Transações.

A API centraliza as regras de negócio e o acesso ao banco de dados MySQL. O aplicativo Flutter consome os endpoints disponibilizados pela API para consultar, cadastrar, atualizar e excluir informações.

## Tecnologias utilizadas

### Backend

- C# e ASP.NET Core Web API
- .NET 10
- Entity Framework Core
- MySQL
- Pomelo Entity Framework Core Provider
- Autenticação baseada em JWT
- IMemoryCache para gerenciamento de cache em memória

### Aplicativo

- Flutter
- Dart
- Pacote HTTP para comunicação com a API
- JSON para troca de dados

## Arquitetura e organização dos projetos

### Organização da API

A API utiliza uma arquitetura em camadas, separando as responsabilidades de cada parte do sistema.

| Projeto | Responsabilidade |
|---|---|
| **Api** | Controllers, configurações da aplicação e exposição dos endpoints HTTP. |
| **Application** | Serviços, DTOs, interfaces e regras de aplicação. |
| **Domain** | Entidades que representam os principais conceitos do sistema. |
| **Infrastructure** | Persistência, Entity Framework Core, contexto do banco de dados, repositórios e implementação do Unit of Work. |

Os Services dependem de interfaces dos repositórios, enquanto suas implementações concretas são registradas no contêiner de injeção de dependência do ASP.NET Core.

### Organização do Flutter

O aplicativo também possui uma estrutura separada por responsabilidades:

- **Presentation:** telas e componentes da interface.
- **Domain:** modelos e repositórios utilizados pela aplicação.
- **Data:** configurações e serviços de comunicação com a API.

O `ApiService` centraliza as chamadas HTTP, a montagem das URLs, a configuração dos cabeçalhos, o envio do token de autenticação e o tratamento das respostas.

Os repositórios do aplicativo utilizam esse serviço para acessar os endpoints disponibilizados pelo backend.

## Funcionalidades e recursos técnicos

O projeto conta com os seguintes recursos:

- **Operações CRUD:** cadastro, consulta, atualização e exclusão dos recursos disponibilizados pela API.
- **Autenticação JWT:** login utilizando CPF e senha, com emissão de token JWT após a validação das credenciais.
- **Segurança de senhas:** utilização de `PasswordHasher` para realizar o hash das senhas no backend.
- **Respostas padronizadas:** utilização de `ResultWrapper`, contendo informações de sucesso, mensagem, dados e erros.
- **Cache em memória:** utilização de `IMemoryCache` nos serviços que possuem essa implementação.
- **Gerenciamento de transações:** utilização de `IUnitOfWork` para confirmar ou desfazer operações relacionadas ao banco de dados.
- **Integração entre aplicações:** comunicação entre Flutter e ASP.NET Core Web API por meio de HTTP e JSON.

## Como executar o projeto

### 1. Pré-requisitos

Antes de executar o projeto, instale as seguintes ferramentas:

- [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Flutter SDK](https://docs.flutter.dev/get-started/install)
- MySQL Server
- Um editor de código, como Visual Studio ou Visual Studio Code.

### 2. Configuração do banco de dados

Crie um banco de dados MySQL com o seguinte comando:

```sql
CREATE DATABASE clinicaapi;
```

A API utiliza uma connection string chamada `DefaultConnection`. Configure-a para apontar para a sua instância do MySQL.

Exemplo de configuração para ambiente local:

```text
server=localhost;port=3306;database=clinicaapi;user=SEU_USUARIO;password=SUA_SENHA;
```

Também é necessário configurar os parâmetros de autenticação JWT:

- `Key`
- `Issuer`
- `Audience`

**Atenção à segurança:** não publique senhas do banco de dados, chaves JWT ou outros dados sensíveis no repositório. Utilize User Secrets, variáveis de ambiente ou outro mecanismo seguro de configuração.

A chave JWT deve ser longa, aleatória e diferente de qualquer chave utilizada em exemplos.

### 3. Executar a API

Na raiz do repositório, acesse a pasta da API:

```bash
cd ClinicaApi/Api
```

Restaure as dependências:

```bash
dotnet restore
```

Em seguida, execute a aplicação:

```bash
dotnet run
```

Por padrão, o perfil HTTP do projeto utiliza o seguinte endereço:

```text
http://localhost:5293
```

A API utiliza o banco de dados configurado em `DefaultConnection`.

Caso o esquema do banco ainda não tenha sido criado, aplique as migrations existentes antes de utilizar os endpoints.

### 4. Executar o Flutter

Em outro terminal, na raiz do repositório, acesse a pasta do aplicativo:

```bash
cd flutter_app_med
```

Instale as dependências:

```bash
flutter pub get
```

Execute o aplicativo:

```bash
flutter run
```

### 5. Configuração da URL da API

A URL base utilizada pelo aplicativo está definida no arquivo:

```text
flutter_app_med/lib/data/api/api_config.dart
```

A configuração considera o ambiente de execução:

| Ambiente | Endereço base |
|---|---|
| Android Emulator | `http://10.0.2.2:5293/api` |
| Desktop/Local | `http://localhost:5293/api` |

No emulador Android, o endereço `10.0.2.2` é utilizado para acessar o `localhost` da máquina host.

Caso a API esteja sendo executada em outra máquina ou porta, ajuste a configuração correspondente no aplicativo.

## Endpoints principais

Os controllers da API estão organizados de acordo com os recursos do sistema:

| Recurso | Rota base |
|---|---|
| Autenticação | `/api/Auth` |
| Consultas | `/api/Consulta` |
| Médicos | `/api/Medico` |
| Pacientes | `/api/Paciente` |
| Recepcionistas | `/api/Recepcionista` |
| Medicamentos | `/api/Remedio` |
| Transações | `/api/Transacao` |
| Usuários | `/api/Usuario` |

### Autenticação

O endpoint de login é:

```http
POST /api/Auth/login?cpf={cpf}&senha={senha}
```

Ele recebe o CPF e a senha como parâmetros de consulta e retorna a resposta de autenticação, incluindo o token JWT quando as credenciais são válidas.

As demais operações seguem os métodos HTTP definidos em cada controller:

- `GET`: consulta de informações.
- `POST`: cadastro de novos registros.
- `PUT`: atualização de registros.
- `DELETE`: exclusão de registros.

## Comunicação entre o aplicativo e a API

O fluxo geral de uma operação no sistema segue a seguinte sequência:

```text
Tela Flutter
     ↓
Repositório Flutter
     ↓
ApiService (HTTP)
     ↓
Controller ASP.NET Core
     ↓
Service da aplicação
     ↓
Interface do Repository
     ↓
Repository / Entity Framework Core
     ↓
MySQL
```

Após o processamento da operação, a resposta percorre o caminho inverso até chegar ao aplicativo.

O `ApiService` interpreta o JSON retornado pela API, tratando as respostas de sucesso, possíveis erros e a estrutura padronizada `ResultWrapper`.

## Observações importantes

- A API e o aplicativo precisam estar em execução para que as funcionalidades integradas funcionem corretamente.
- O endereço configurado no Flutter deve corresponder ao endereço em que a API está disponível.
- O token JWT é mantido em memória pelo serviço de API do aplicativo e não é persistido para reutilização automática após o fechamento do aplicativo.
- A configuração de autenticação JWT está presente na API. Para exigir autenticação em uma rota, o endpoint deve estar protegido com `[Authorize]` ou uma política equivalente.
- O cache utilizado é armazenado em memória e não é compartilhado entre diferentes instâncias da API.

## Sobre o projeto

Projeto acadêmico desenvolvido com o objetivo de demonstrar a integração entre um aplicativo Flutter e uma API REST construída com ASP.NET Core.

A aplicação utiliza separação em camadas, acesso a dados com Entity Framework Core, autenticação baseada em JWT e comunicação entre sistemas por meio de requisições HTTP.

O projeto reúne conceitos de desenvolvimento mobile e backend, demonstrando uma estrutura de integração entre diferentes tecnologias em uma aplicação de gerenciamento de clínicas.
