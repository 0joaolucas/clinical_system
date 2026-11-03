import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/medicine.dart';

class MedicineRepository {
  final ApiService _api = ApiService.instance;

  MedicineRepository([Object? databaseProvider]);

  Future<int> insert(Medicine medicine) async {
    await _api.post(
      'Remedio',
      medicine.toMap(),
    );

    return 1;
  }

  Future<int> update(Medicine medicine) async {
    await _api.put(
      'Remedio',
      medicine.toMap(),
    );

    return 1;
  }

  Future<int> delete(Medicine medicine) async {
    if (medicine.id == null) {
      throw ApiException(
        'ID do remédio não informado.',
      );
    }

    await _api.delete(
      'Remedio/${medicine.id}',
    );

    return 1;
  }

  Future<Medicine> findById(int id) async {
    final data = await _api.get(
      'Remedio/$id',
    );

    return Medicine.fromMap(
      Map<String, dynamic>.from(data),
    );
  }

  Future<List<Medicine>> findAll() async {
    final data = await _api.get('Remedio');

    final lista = data as List? ?? [];

    return lista
        .map(
          (item) => Medicine.fromMap(
            Map<String, dynamic>.from(item),
          ),
        )
        .toList();
  }

  Future<List<Medicine>> searchByName(
    String name,
  ) async {
    final remedios = await findAll();

    final pesquisa = name.toLowerCase();

    return remedios.where((remedio) {
      return remedio.nome
          .toLowerCase()
          .contains(pesquisa);
    }).toList();
  }
}