Clínica API + Flutter
Aplicação de gerenciamento de clínica desenvolvida com Flutter no
aplicativo cliente e ASP.NET Core Web API no backend. Os dois
projetos estão organizados no mesmo repositório e se comunicam por
requisições HTTP, utilizando JSON.
Visão geral
O aplicativo permite trabalhar com os principais cadastros e operações
de uma clínica, como pacientes, médicos, recepcionistas, consultas,
medicamentos e transações.

A API centraliza as regras de negócio e o acesso ao banco de dados
MySQL. O Flutter consome os endpoints da API para consultar e realizar
operações sobre os dados.
Tecnologias
Backend
- C# e ASP.NET Core Web API
- .NET 10
- Entity Framework Core
- MySQL
- Pomelo Entity Framework Core Provider
- Autenticação baseada em JWT
- IMemoryCache para cache em memória
Aplicativo
- Flutter
- Dart
- Pacote http para comunicação com a API
- JSON para troca de dados

Organização da API
A API utiliza uma separação em camadas:
  Projeto                             Responsabilidade
  Api                               Controllers, configuração da
aplicação e exposição dos endpoints
HTTP.

  Application                       Serviços, DTOs, interfaces e regras
de aplicação.

  Domain                            Entidades que representam os
conceitos principais do sistema.
Infrastructure                    Persistência, Entity Framework
Core, contexto do banco,
repositórios e implementação do
Unit of Work.
Os Services dependem de interfaces dos repositórios. As implementações
concretas são registradas no contêiner de injeção de dependência do
ASP.NET Core.
Organização do Flutter
O aplicativo separa: - Presentation: telas e componentes da
interface. - Domain: modelos e repositórios utilizados pela
aplicação. - Data: configurações e serviços de comunicação com a
API.

O ApiService centraliza as chamadas HTTP, a montagem das URLs, os
cabeçalhos, o envio do token e o tratamento das respostas. Os
repositórios do aplicativo utilizam esse serviço para acessar os
endpoints.
Funcionalidades e recursos técnicos
- Operações de cadastro, consulta, atualização e exclusão para os
  recursos disponibilizados pela API.
- Autenticação por CPF e senha, com emissão de token JWT após a
  validação das credenciais.
- Hash de senhas no backend utilizando PasswordHasher.
- Respostas padronizadas por meio de ResultWrapper<T>, com campos de
  sucesso, mensagem, dados e erros.
- Cache em memória com IMemoryCache nos serviços que possuem essa
  implementação.
- Transações no banco de dados com IUnitOfWork, permitindo confirmar
  ou desfazer operações relacionadas.
- Integração entre Flutter e API por HTTP/JSON.
Como executar o projeto
1. Pré-requisitos
Instale as ferramentas abaixo:
- .NET SDK 10
- Flutter SDK
- MySQL Server
- Um editor de código, como Visual Studio ou VS Code.
2. Configurar o banco de dados
Crie um banco MySQL chamado:
CREATE DATABASE clinicaapi;
A API utiliza a connection string chamada DefaultConnection.
Configure-a para apontar para a sua instância do MySQL.

Por exemplo, em ambiente local:
server=localhost;port=3306;database=clinicaapi;user=SEU_USUARIO;password=SUA_SENHA;
Configure também os valores de JWT (Key, Issuer e Audience) no
ambiente local.

Segurança: não publique senhas do banco, chaves JWT ou outros
segredos no repositório. Utilize User Secrets, variáveis de ambiente ou
outro mecanismo de configuração seguro. A chave JWT deve ser longa,
aleatória e diferente da usada em exemplos.
3. Executar a API
Na raiz do repositório, execute:
cd ClinicaApi/Api
dotnet restore
dotnet run
O perfil HTTP do projeto utiliza, por padrão:
http://localhost:5293
A API utiliza o banco configurado em DefaultConnection. Se o banco
ainda não tiver o esquema criado, aplique as migrations existentes antes
de utilizar os endpoints.
4. Executar o Flutter
Em outro terminal, na raiz do repositório:
cd flutter_app_med
flutter pub get
flutter run
5. Configuração da URL da API
A URL base é definida em:
flutter_app_med/lib/data/api/api_config.dart
A configuração considera o ambiente de execução:

  Ambiente           Endereço base
  Android Emulator   http://10.0.2.2:5293/api
Desktop/local      http://localhost:5293/api

No emulador Android, 10.0.2.2 é o endereço utilizado para acessar o
localhost da máquina host. Se a API estiver em outra máquina ou porta,
ajuste a configuração.
Endpoints principais
Os controllers da API estão organizados por recurso:

  Recurso          Rota base
  Autenticação     /api/Auth
Consultas        /api/Consulta
Médicos          /api/Medico
Pacientes        /api/Paciente
Recepcionistas   /api/Recepcionista
Medicamentos     /api/Remedio
Transações       /api/Transacao
Usuários         /api/Usuario

O endpoint de login é:
POST /api/Auth/login?cpf={cpf}&senha={senha}
Ele recebe CPF e senha como parâmetros de consulta e retorna a resposta
de autenticação, incluindo o token quando as credenciais são válidas.

As demais operações seguem os métodos HTTP definidos em cada controller
(GET, POST, PUT e DELETE).
Comunicação entre o aplicativo e a API
O fluxo geral de uma operação é:
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
A resposta percorre o caminho de volta até o aplicativo. O ApiService
interpreta o JSON retornado pela API e trata respostas de erro e o
formato ResultWrapper<T>.
Observações
- A API e o aplicativo precisam estar em execução para que as
  funcionalidades integradas funcionem.
- O endereço configurado no Flutter precisa corresponder ao endereço
  em que a API está disponível.
- O token JWT é mantido no serviço de API do aplicativo em memória;
  ele não é persistido para reutilização automática após fechar o app.
- A configuração de autenticação JWT está presente na API. Para exigir
  autenticação em uma rota, o endpoint deve estar protegido com
  [Authorize] ou uma política equivalente.
- O cache utilizado é em memória e não é compartilhado entre
  diferentes instâncias da API.
Sobre o projeto
Projeto acadêmico desenvolvido para demonstrar a integração de um
aplicativo Flutter com uma API REST em ASP.NET Core, aplicando separação
em camadas, acesso a dados com Entity Framework Core e comunicação por
HTTP.
