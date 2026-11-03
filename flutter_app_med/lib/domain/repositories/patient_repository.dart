import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/patient.dart';

class PatientRepository {
  final ApiService _api = ApiService.instance;

  PatientRepository([Object? databaseProvider]);

  Future<int> insert(Patient patient) async {
    await _api.post(
      'Paciente',
      patient.toMap(),
    );

    return 1;
  }

  Future<int> update(Patient patient) async {
    await _api.put(
      'Paciente',
      patient.toMap(),
    );

    return 1;
  }

  Future<int> delete(Patient patient) async {
    if (patient.id == null) {
      throw ApiException(
        'ID do paciente não informado.',
      );
    }

    await _api.delete(
      'Paciente/${patient.id}',
    );

    return 1;
  }

  Future<Patient?> findById(int id) async {
    final data = await _api.get(
      'Paciente/$id',
    );

    return Patient.fromMap(
      Map<String, dynamic>.from(data),
    );
  }

  Future<List<Patient>> findAll() async {
    final data = await _api.get('Paciente');

    final lista = data as List? ?? [];

    return lista
        .map(
          (item) => Patient.fromMap(
            Map<String, dynamic>.from(item),
          ),
        )
        .toList();
  }

  Future<List<Patient>> searchByName(
    String name,
  ) async {
    final pacientes = await findAll();

    final pesquisa = name.toLowerCase();

    return pacientes.where((paciente) {
      return (paciente.nome ?? '')
          .toLowerCase()
          .contains(pesquisa);
    }).toList();
  }
}