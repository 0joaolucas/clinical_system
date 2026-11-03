import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/medico.dart';

class MedicoRepository {
  final ApiService _api = ApiService.instance;

  MedicoRepository([Object? databaseProvider]);

  Future<int> insert(Medicos medico) async {
    await _api.post(
      'Medico',
      {
        'cpf': medico.cpf ?? '',
        'nome': medico.nome ?? '',
        'CRM': medico.crm ?? '',
        'especialidade': medico.especialidade ?? '',
        'telefone': medico.telefone ?? '',
        'email': medico.email ?? '',
        'funcao': medico.funcao ?? 'Medico',
        'senha': medico.senhaTemp ?? '',
      },
    );

    return 1;
  }

  Future<int> update(Medicos medico) async {
    await _api.put('Medico', medico.toMap());
    return 1;
  }

  Future<int> delete(Medicos medico) async {
    if (medico.id == null) {
      throw ApiException('ID do médico não informado.');
    }

    await _api.delete('Medico/${medico.id}');
    return 1;
  }

  Future<Medicos> findById(String crm) async {
    final data = await _api.get(
      'Medico/crm/${Uri.encodeComponent(crm)}',
    );

    return Medicos.fromMap(Map<String, dynamic>.from(data));
  }

  Future<List<Medicos>> findAll() async {
    final data = await _api.get('Medico');
    final lista = data as List? ?? [];

    return lista
        .map((item) => Medicos.fromMap(
              Map<String, dynamic>.from(item),
            ))
        .toList();
  }
}