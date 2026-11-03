import 'dart:convert';

class Medicine {
  int? id;
  String nome;
  int quantidade;

  Medicine({
    this.id,
    required this.nome,
    required this.quantidade,
  });

  Map<String, dynamic> toMap() {
    return {
      if (id != null) 'id': id,
      'nome': nome,
      'quantidade': quantidade,
    };
  }

  factory Medicine.fromMap(Map<String, dynamic> map) {
    return Medicine(
      id: map['id'],
      nome: map['nome']?.toString() ?? '',
      quantidade:
          (map['quantidade'] as num?)?.toInt() ?? 0,
    );
  }

  String toJson() {
    return jsonEncode(toMap());
  }

  factory Medicine.fromJson(String source) {
    return Medicine.fromMap(jsonDecode(source));
  }
}