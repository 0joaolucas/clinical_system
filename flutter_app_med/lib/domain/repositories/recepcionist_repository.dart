import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/recepcionista.dart';

class RecepcionistaRepository {
  final ApiService _api = ApiService.instance;

  RecepcionistaRepository([Object? databaseProvider]);

  Future<int> insert(Recepcionista recepcionista) async {
    await _api.post(
      'Recepcionista',
      {
        'cpf': recepcionista.cpf ?? '',
        'nome': recepcionista.nome ?? '',
        'telefone': recepcionista.telefone ?? '',
        'email': recepcionista.email ?? '',
        'senha': recepcionista.senhaTemp ?? '',
      },
    );

    return 1;
  }

  Future<int> update(Recepcionista recepcionista) async {
    await _api.put('Recepcionista', recepcionista.toMap());
    return 1;
  }

  Future<int> delete(Recepcionista recepcionista) async {
    if (recepcionista.id == null) {
      throw ApiException('ID da recepcionista não informado.');
    }

    await _api.delete('Recepcionista/${recepcionista.id}');
    return 1;
  }

  Future<Recepcionista?> findById(int id) async {
    final data = await _api.get('Recepcionista/$id');

    return Recepcionista.fromMap(
      Map<String, dynamic>.from(data),
    );
  }

  Future<List<Recepcionista>> findAll() async {
    final data = await _api.get('Recepcionista');
    final lista = data as List? ?? [];

    return lista
        .map((item) => Recepcionista.fromMap(
              Map<String, dynamic>.from(item),
            ))
        .toList();
  }
}