import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/consulta.dart';

class ConsultaRepository {
  final ApiService _api = ApiService.instance;

  ConsultaRepository([Object? databaseProvider]);

  Future<int> insert(Consulta consulta) async {
    await _api.post(
      'Consulta',
      consulta.toMap(),
    );

    return 1;
  }

  Future<int> update(Consulta consulta) async {
    await _api.put(
      'Consulta',
      consulta.toMap(),
    );

    return 1;
  }

  Future<int> delete(Consulta consulta) async {
    if (consulta.id == null) {
      throw ApiException(
        'ID da consulta não informado.',
      );
    }

    await _api.delete(
      'Consulta/${consulta.id}',
    );

    return 1;
  }

  Future<Consulta?> findById(int id) async {
    final data = await _api.get(
      'Consulta/$id',
    );

    return Consulta.fromMap(
      Map<String, dynamic>.from(data),
    );
  }

  Future<List<Consulta>> findAll() async {
    final data = await _api.get('Consulta');

    final lista = data as List? ?? [];

    return lista
        .map(
          (item) => Consulta.fromMap(
            Map<String, dynamic>.from(item),
          ),
        )
        .toList();
  }

  Future<List<Consulta>> findByPaciente(
    int idPaciente,
  ) async {
    final consultas = await findAll();

    return consultas
        .where(
          (consulta) =>
              consulta.idPaciente == idPaciente,
        )
        .toList();
  }

  Future<List<Consulta>> findByMedico(
    int idMedico,
  ) async {
    final consultas = await findAll();

    return consultas
        .where(
          (consulta) =>
              consulta.idMedico == idMedico,
        )
        .toList();
  }
}