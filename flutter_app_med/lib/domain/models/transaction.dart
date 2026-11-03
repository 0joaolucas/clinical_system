import 'dart:convert';

class Transacao {
  int? id;
  int? idPaciente;
  int? idRemedio;
  int? quantidade;
  double? valor;
  DateTime? dataTransacao;

  Transacao({
    this.id,
    this.idPaciente,
    this.idRemedio,
    this.quantidade,
    this.valor,
    this.dataTransacao,
  });

  Map<String, dynamic> toMap() {
    return {
      if (id != null) 'id': id,
      'idPaciente': idPaciente,
      'idRemedio': idRemedio,
      'quantidade': quantidade,
      'valor': valor,
      'dataTransacao': dataTransacao == null
          ? null
          : _formatDate(dataTransacao!),
    };
  }

  factory Transacao.fromMap(Map<String, dynamic> map) {
    return Transacao(
      id: map['id'],
      idPaciente: map['idPaciente'],
      idRemedio: map['idRemedio'],
      quantidade:
          (map['quantidade'] as num?)?.toInt(),
      valor: (map['valor'] as num?)?.toDouble(),
      dataTransacao: map['dataTransacao'] == null
          ? null
          : DateTime.tryParse(
              map['dataTransacao'].toString(),
            ),
    );
  }

  static String _formatDate(DateTime data) {
    final ano = data.year.toString().padLeft(4, '0');
    final mes = data.month.toString().padLeft(2, '0');
    final dia = data.day.toString().padLeft(2, '0');

    return '$ano-$mes-$dia';
  }

  String toJson() {
    return jsonEncode(toMap());
  }

  factory Transacao.fromJson(String source) {
    return Transacao.fromMap(jsonDecode(source));
  }
}