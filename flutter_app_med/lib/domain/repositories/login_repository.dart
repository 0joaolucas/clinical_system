import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/login.dart';

class LoginRepository {
  final ApiService _api = ApiService.instance;

  LoginRepository([Object? databaseProvider]);

  Future<Login?> login(String cpf, String senha) async {
    final data = await _api.postWithQuery(
      'Auth/login',
      {
        'cpf': cpf,
        'senha': senha,
      },
    );

    final token = data?.toString();

    if (token == null || token.isEmpty) {
      return null;
    }

    _api.token = token;

    return Login(
      cpf: cpf,
      senha: senha,
      token: token,
    );
  }

  Future<Login?> findByCpfAndPassword(
    String cpf,
    String senha,
  ) {
    return login(cpf, senha);
  }
}