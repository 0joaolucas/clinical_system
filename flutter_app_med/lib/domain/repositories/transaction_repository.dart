import 'package:flutter_app_med/data/api/api_service.dart';
import 'package:flutter_app_med/domain/models/transaction.dart';

class TransactionRepository {
  final ApiService _api = ApiService.instance;

  TransactionRepository([Object? databaseProvider]);

  Future<int> insert(Transacao transacao) async {
    await _api.post(
      'Transacao',
      transacao.toMap(),
    );

    return 1;
  }

  Future<int> update(Transacao transacao) async {
    await _api.put(
      'Transacao',
      transacao.toMap(),
    );

    return 1;
  }

  Future<int> delete(Transacao transacao) async {
    if (transacao.id == null) {
      throw ApiException(
        'ID da transação não informado.',
      );
    }

    await _api.delete(
      'Transacao/${transacao.id}',
    );

    return 1;
  }

  Future<Transacao> findById(int id) async {
    final data = await _api.get(
      'Transacao/$id',
    );

    return Transacao.fromMap(
      Map<String, dynamic>.from(data),
    );
  }

  Future<List<Transacao>> findAll() async {
    final data = await _api.get(
      'Transacao',
    );

    final lista = data as List? ?? [];

    return lista
        .map(
          (item) => Transacao.fromMap(
            Map<String, dynamic>.from(item),
          ),
        )
        .toList();
  }
}