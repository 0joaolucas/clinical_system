# Integração Flutter + API CuidaMed

## Dependências

Depois de extrair o projeto, execute:

```bash
flutter pub get
```

Foi incluído `flutter_secure_storage` para guardar o JWT de forma segura no dispositivo.

## Endereço da API

O endereço padrão está em `lib/data/api/api_config.dart`:

- Android Emulator: `http://10.0.2.2:5293/api`
- Windows e outros desktops: `http://localhost:5293/api`

Se a porta ou o endereço forem diferentes, execute passando a URL base:

```bash
flutter run --dart-define=API_BASE_URL=http://localhost:5293/api
```

Para testar em um celular físico, use o IP local do computador onde a API está rodando, por exemplo:

```bash
flutter run --dart-define=API_BASE_URL=http://192.168.0.10:5293/api
```

Nesse caso, o computador e o celular precisam estar na mesma rede, a porta precisa estar liberada no firewall e a API precisa aceitar conexões pela rede local (não apenas por `localhost`).

## Autenticação

O aplicativo inicia na tela de login. A autenticação envia CPF e senha para `POST api/Auth/login`, recebe o JWT, armazena-o usando armazenamento seguro e envia `Authorization: Bearer <token>` nas demais requisições.

O token expirado é descartado na inicialização. O botão de saída na tela principal encerra a sessão e retorna ao login.

O login foi mantido com parâmetros na query string para acompanhar a forma de chamada já usada pelo projeto. Para produção, é recomendável que o endpoint receba as credenciais no corpo da requisição POST, evitando que a senha apareça em logs de URL.

## Cadastro de profissionais

O cadastro de médico e de recepcionista envia a senha junto com os dados do respectivo profissional. A API deve criar o registro correspondente na tabela de login, como já foi implementado no backend. O Flutter não faz uma segunda chamada para cadastrar o login do médico, evitando duplicidade.

## Observação sobre o contrato da API

Este ZIP contém somente o projeto Flutter. Os nomes de propriedades e rotas foram mantidos ou ajustados com base nos contratos que o próprio aplicativo já utilizava e nas informações disponíveis sobre a API. Para validar cada DTO e cada rota contra a implementação C# linha por linha, também é necessário ter o projeto da API.
